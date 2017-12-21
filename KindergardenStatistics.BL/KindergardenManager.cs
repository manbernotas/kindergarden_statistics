using KindergardenStatistics.DAL;
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

        public KindergardenData ConvertData(string[] data)
        {
            List<string> value;
            var kindergardensData = new KindergardenData();

            value = data.First().Split(Separator, StringSplitOptions.None).ToList();

            if (value[1] == "KindergardenName" && value[2] == "GroupName" && value[3] == "ChildId"
                && value[4] == "RegisteredInCity" && value[5] == "Sick" && value[6] == "OtherReasons" && value[7] == "NoReasons")
            {
                foreach (var line in data.Skip(1))
                {
                    value = line.Split(Separator, StringSplitOptions.None).ToList();

                    kindergardensData.Kindergardens.Add(new Kindergarden()
                    {
                        Name = value[1],
                    });
                    kindergardensData.GroupNames.Add(new Group()
                    {
                        Name = value[2],
                    });

                    if (Int64.TryParse(value[3], out long childId))
                    {
                        kindergardensData.Children.Add(new Child()
                        {
                            Id = childId,
                            RegisteredInCity = Convert.ToBoolean(Convert.ToInt16(value[4])),
                        });
                    }

                    kindergardensData.SickList.Add(value[5]);
                    kindergardensData.OtherReasonList.Add(value[6]);
                    kindergardensData.NoReasonList.Add(value[7]);
                }
            }
            return kindergardensData;
        }

        // TODO: Factory design pattern - read and see some examples
        // 1) How are you using interfaces?
        // 2) How to create Objects without passing params?
        /// <summary>
        /// Prepare data from file for uploading to DB
        /// </summary>
        public void UploadDataFromFile(string fileName)
        {
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            var fileManager = new FileManager();
            var rawData = fileManager.ReadFile(fileName);
            var kindergardensData = ConvertData(rawData);

            foreach (var kgName in kindergardensData.Kindergardens)
            {
                if (kindergardensData.UniqueKindergardens.FirstOrDefault(x => x.Name == kgName.Name) == null)
                {
                    kindergardensData.UniqueKindergardens.Add(new Kindergarden()
                    {
                        Name = kgName.Name,
                    });
                }
            }

            repo.SaveKindergarden(kindergardensData.UniqueKindergardens);

            for (int i = 0; i < kindergardensData.GroupNames.Count; i++)
            {
                var groupName = kindergardensData.GroupNames[i].Name;
                if (kindergardensData.UniqueGroups.FirstOrDefault(x => x.Name == groupName) == null)
                {
                    var kindergardenId = context.Kindergarden.First(k => k.Name == kindergardensData.Kindergardens[i].Name).Id;
                    kindergardensData.UniqueGroups.Add(new Group()
                    {
                        Name = groupName,
                        KindergardenId = kindergardenId,
                    });
                }
            }

            repo.SaveGroup(kindergardensData.UniqueGroups);

            for (int j = 0; j < kindergardensData.Children.Count; j++)
            {
                if (kindergardensData.UniqueChildren.FirstOrDefault(x => x.Id == kindergardensData.Children[j].Id) == null)
                {
                    kindergardensData.UniqueChildren.Add(new Child()
                    {
                        Id = kindergardensData.Children[j].Id,
                        RegisteredInCity = kindergardensData.Children[j].RegisteredInCity,
                    });

                    var groupId = kindergardensData.UniqueGroups.First(x => x.Name == kindergardensData.GroupNames[j].Name).Id;

                    kindergardensData.GroupChild.Add(new GroupChild()
                    {
                        ChildId = kindergardensData.Children[j].Id,
                        GroupId = groupId,
                        Current = true,
                        Started = DateTime.Today,
                    });

                    if (int.TryParse(kindergardensData.SickList[j], out int sick)
                        && int.TryParse(kindergardensData.OtherReasonList[j], out int otherReasons)
                        && int.TryParse(kindergardensData.NoReasonList[j], out int noReasons))
                    {
                        kindergardensData.Attendance.Add(new Attendance()
                        {
                            ChildId = kindergardensData.Children[j].Id,
                            Date = DateTime.Today,
                            NoReasons = noReasons,
                            OtherReasons = otherReasons,
                            Sick = sick,
                        });
                    }
                }
            }

            repo.SaveChild(kindergardensData.UniqueChildren);
            repo.SaveGroupChild(kindergardensData.GroupChild);
            repo.SaveAttendance(kindergardensData.Attendance);

            //    // TODO: task in trello "Extension method for DBContext"
            //    if (j > 0 && j % 3000 == 0)
            //    {
            //        //context.BulkSaveChanges(???)
            //        context.SaveChanges();
            //    }


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