﻿using KindergardenStatistics.DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace KindergardenStatistics.BL
{

    public class KindergardenManager
    {
        private KindergardenContext context;

        private Repository repo;

        static readonly string Separator = ";";

        public KindergardenManager(KindergardenContext context)
        {
            repo = new Repository(context);
            this.context = context;
        }

        public KindergardenManager(KindergardenContext context, Repository repo)
        {
            this.repo = repo;
        }

        public string GetName()
        {
            return "name";
        }

        public List<Kindergarden> GetKindergardens()
        {
            var kindergardens = repo.GetKindergardens();

            return kindergardens;
        }

        // TODO: Factory design pattern - read and see some examples
        // 1) How are you using interfaces?
        // 2) How to create Objects without passing params?
        /// <summary>
        /// Prepare data from file for uploading to DB
        /// </summary>
        public void UploadDataFromFile(string fileName)
        {
            List<string> value;
            Dictionary<string, int> kindergardens = new Dictionary<string, int>();

            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            // TODO: Split reading and writing to database
            // 1) Go through all kindergardens and save them to get their IDs
            // 2) Manage all other data
            foreach (string line in File.ReadAllLines(fileName).Skip(1))
            {
                value = line.Split(Separator, StringSplitOptions.None).ToList();
                var kindergardenName = value[1];

                if (!kindergardens.ContainsKey(kindergardenName))
                {
                    var kindergarden = repo.SaveKindergarden(kindergardenName);
                    kindergardens.Add(kindergarden.Name, kindergarden.Id);
                }
                
                var kindergardenId = kindergardens.First(x => x.Key == kindergardenName).Value;

                var groupName = value[2];

                var group = context.Group.FirstOrDefault(grp => grp.Name == groupName && grp.KindergardenId.Equals(kindergardenId))
                    ?? repo.SaveGroup(kindergardenId, groupName);

                bool registeredInCity;

                if (value[4] == "1")
                {
                    registeredInCity = true;
                }
                else
                {
                    registeredInCity = false;
                }

                if (Int64.TryParse(value[3], out long childId))
                {
                    if (context.Child.Local.FirstOrDefault(cld => cld.Id == childId) == null && context.Child.FirstOrDefault(cld => cld.Id == childId) == null)
                    {
                        repo.SaveChild(childId, group.Id, registeredInCity);
                        
                        if (int.TryParse(value[5], out int sick)
                            && int.TryParse(value[6], out int noReasons)
                            && int.TryParse(value[7], out int otherReasons))
                        {
                            repo.SaveAttendance(childId, sick, noReasons, otherReasons);
                        }
                    }
                }
            }

            context.SaveChanges();
            stopwatch.Stop();
        }

        public Child GetChild(long id)
        {
            var kindergardens = repo.GetKindergardens();
            var child = kindergardens
                .SelectMany(kg => kg.Groups)
                .SelectMany(gcr => gcr.GroupChildRelation)
                .Select(cld => cld.Child)
                .FirstOrDefault(c => c.Id == id);

            return child;
        }
    
        public string GetChildsKindergarden(long id)
        {
            var kindergardens = repo.GetKindergardens();

            foreach (var kg in kindergardens)
            {
                foreach (var group in kg.Groups)
                {
                    foreach (var grc in group.GroupChildRelation)
                    {
                        if (grc.ChildId == id)
                        {
                            return kg.Name;
                        }
                    }
                }
            }

            return null;
        }

        public string GetMostSickGroup()
        {
            var kindergardens = repo.GetKindergardens();
            var mostSickGroupName = string.Empty;
            int mostSick = 0;

            foreach (var kg in kindergardens)
            {
                foreach (var group in kg.Groups)
                {
                    int sick = group.GroupChildRelation
                        .SelectMany(grp => grp.Child.Attendance)
                        .Sum(att => att.Sick);

                    if (sick > mostSick)
                    {
                        mostSick = sick;
                        mostSickGroupName = sick + " " + kg.Name + " " + group.Name;
                    }
                    sick = 0;
                }
            }

            return mostSickGroupName;
        }

        public string GetHealthiestGroup()
        {
            var kindergardens = repo.GetKindergardens();
            var healthiestGroupName = string.Empty;
            var leastSick = int.MaxValue;

            foreach (var kg in kindergardens)
            {
                foreach (var group in kg.Groups)
                {
                    int sick = group.GroupChildRelation
                        .SelectMany(cld => cld.Child.Attendance)
                        .Sum(att => att.Sick);

                    if (sick < leastSick)
                    {
                        leastSick = sick;
                        healthiestGroupName = sick + " " + kg.Name + " " + group.Name;
                    }
                    sick = 0;
                }
            }

            return healthiestGroupName;
        }

        public List<int> GetChildrenIdOrderedByAttendance()
        {
            var kindergardens = repo.GetKindergardens();
            Dictionary<int, int> childrenAttendance = new Dictionary<int, int>();

            childrenAttendance = kindergardens
                .SelectMany(kg => kg.Groups)
                .SelectMany(grp => grp.GroupChildRelation)
                .SelectMany(cld => cld.Child.Attendance)
                .Distinct()
                .ToDictionary(att => att.Id, att => att.Sick);

            return childrenAttendance.OrderBy(x => x.Value).Select(x => x.Key).ToList();
        }

        public List<string> GetTopTwoKgNamesOrderedByAttendance()
        {
            var kindergardens = repo.GetKindergardens();
            Dictionary<string, int> kgAttendance = new Dictionary<string, int>();

            foreach (var kg in kindergardens)
            {
                int sick = kg.Groups
                    .SelectMany(grp => grp.GroupChildRelation)
                    .SelectMany(cld => cld.Child.Attendance)
                    .Sum(att => att.Sick);

                if (!kgAttendance.ContainsKey(kg.Name))
                {
                    kgAttendance.Add(kg.Name, sick);
                }
            }

            return kgAttendance.OrderBy(x => x.Value).Select(x => x.Key).Take(2).ToList();
        }

        public string GetMostSickKg()
        {
            var kindergardens = repo.GetKindergardens();
            var mostSickKgName = string.Empty;
            int mostSick = 0;

            foreach (var kg in kindergardens)
            {
                int sick = kg.Groups
                    .SelectMany(grp => grp.GroupChildRelation)
                    .SelectMany(cld => cld.Child.Attendance)
                    .Sum(att => att.Sick);

                if (sick > mostSick)
                {
                    mostSick = sick;
                    mostSickKgName = kg.Name;
                }
                sick = 0;
            }

            return mostSickKgName;
        }

        public string GetHealthiestKg()
        {
            var kindergardens = repo.GetKindergardens();
            var healthiestKgName = string.Empty;
            int leastSick = int.MaxValue;

            foreach (var kg in kindergardens)
            {
                int sick = kg.Groups
                    .SelectMany(grp => grp.GroupChildRelation)
                    .SelectMany(cld => cld.Child.Attendance)
                    .Sum(att => att.Sick);

                if (sick < leastSick)
                {
                    leastSick = sick;
                    healthiestKgName = kg.Name;
                }
                sick = 0;
            }

            return healthiestKgName;
        }
    }
}