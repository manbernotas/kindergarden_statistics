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

        /// <summary>
        /// Return list of kindergardens
        /// </summary>
        /// <returns></returns>
        public List<Kindergarden> GetKindergardens()
        {
            var kindergardens = repo.GetKindergardens();

            return kindergardens;
        }

        // TODO: Add unit test here (One success and one failing)

        /// <summary>
        /// Converts array of strings data to KindergardenData
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public KindergardenData ConvertData(string[] data)
        {
            List<string> value;
            var kindergardensData = new KindergardenData();

            if ((data != null || data.Length != 0) 
                && data.First() == "Nr;KindergardenName;GroupName;ChildId;RegisteredInCity;Sick;OtherReasons;NoReasons;SpareDays")
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
                            RegisteredInCity = Convert.ToBoolean(Convert.ToByte(value[4])),
                        });

                        if (int.TryParse(value[5], out int sick)
                        && int.TryParse(value[6], out int otherReasons)
                        && int.TryParse(value[7], out int noReasons))
                        {
                            kindergardensData.SickList.Add(sick);
                            kindergardensData.OtherReasonList.Add(otherReasons);
                            kindergardensData.NoReasonList.Add(noReasons);
                        }
                    }
                }
            }

            return kindergardensData;
        }

        // TODO: Factory design pattern - read and see some examples
        // 1) How are you using interfaces?
        // 2) How to create Objects without passing params?
        
        /// <summary>
        /// Prepare data for uploading to DB
        /// </summary>
        public void UploadDataFromFile(string fileName)
        {
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            var fileManager = new FileManager();
            var kindergardensData = ConvertData(fileManager.GetData(fileName));

            foreach (var kg in kindergardensData.Kindergardens)
            {
                // TODO: We want unique values why not use a Collection with UNIQUE values?
                // There is https://msdn.microsoft.com/en-us/library/bb359438.aspx
                // However - it's for primitive types only, you need to implement specific Interface for it to be able to
                // distinguish unique objects
                if (kindergardensData.UniqueKindergardens.FirstOrDefault(x => x.Name == kg.Name) == null)
                {
                    kindergardensData.UniqueKindergardens.Add(new Kindergarden()
                    {
                        Name = kg.Name,
                    });
                }
            }

            var kindergardens = repo.SaveKindergarden(kindergardensData.UniqueKindergardens);

            for (int i = 0; i < kindergardensData.GroupNames.Count; i++)
            {
                var groupName = kindergardensData.GroupNames[i].Name;
                var kindergardenId = kindergardens.First(kg => kg.Name == kindergardensData.Kindergardens[i].Name).Id;

                if (kindergardensData.UniqueGroups.FirstOrDefault(x => x.Name == groupName 
                    && x.KindergardenId == kindergardenId) == null)
                {
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
                    var groupName = kindergardensData.GroupNames[j].Name;
                    var kindergardenId = kindergardens.First(kg => kg.Name == kindergardensData.Kindergardens[j].Name).Id;
                    var groupId = kindergardensData.UniqueGroups.First(x => x.Name == groupName
                        && x.KindergardenId == kindergardenId).Id;
                    var childId = kindergardensData.Children[j].Id;

                    kindergardensData.UniqueChildren.Add(new Child()
                    {
                        Id = childId,
                        RegisteredInCity = kindergardensData.Children[j].RegisteredInCity,
                    });

                    kindergardensData.GroupChild.Add(new GroupChild()
                    {
                        ChildId = childId,
                        GroupId = groupId,
                        Current = true,
                        Started = DateTime.Today,
                    });

                    int sick = kindergardensData.SickList[j];
                    int otherReasons = kindergardensData.OtherReasonList[j];
                    int noReasons = kindergardensData.NoReasonList[j];
                    
                    kindergardensData.Attendance.Add(new Attendance()
                    {
                        ChildId = childId,
                        Date = DateTime.Today,
                        NoReasons = noReasons,
                        OtherReasons = otherReasons,
                        Sick = sick,
                    });
                }
            }

            repo.AttachChild(kindergardensData.UniqueChildren);
            repo.AttachGroupChild(kindergardensData.GroupChild);
            repo.AttachAttendance(kindergardensData.Attendance);

            context.SaveChanges();
            stopwatch.Stop(); 
        }

        /// <summary>
        /// Returns child by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Returns child kindergarden by child Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns most sick group
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns healthiest group
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns children Ids order by children attendance
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns top two kindergardens ordered by children attendance
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns most sick group
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns healthiest group
        /// </summary>
        /// <returns></returns>
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