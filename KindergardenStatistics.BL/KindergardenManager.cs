using KindergardenStatistics.DAL;
using KindergardenStatistics.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace KindergardenStatistics.BL
{
    public class KindergardenManager
    {
        private KindergardenContext context;

        private IRepository repository;
        static readonly string Separator = ";";

        public KindergardenManager(KindergardenContext context)
        {
            var repositoryFactory = new RepositoryFactory();
            repository = repositoryFactory.GetRepository(context);
            
            this.context = context;
        }

        public KindergardenManager(KindergardenContext context, Repository repo)
        {
            repository = repo;
        }

        /// <summary>
        /// Return list of kindergardens
        /// </summary>
        /// <returns></returns>
        public List<Kindergarden> GetKindergardens()
        {
            var sw = new Stopwatch();
            sw.Start();
            var kindergardens = repository.GetKindergardens();
            sw.Stop();
            return kindergardens;
        }

        /// <summary>
        /// Return RequestData with kindergardens ordered by children count
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public RequestData GetKindergardensOrderedByChildrenCount(RequestData data)
        {
            var sortedKindergardens = data.KindergardenChildren.OrderByDescending(x => x.ChildrenCount).Take(5).ToList();

            var result = new RequestData
            {
                KindergardenChildren = sortedKindergardens
            };

            return result;
        }

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

                    if (value.Count() == 9)
                    {
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
            }

            return kindergardensData;
        }

        /// <summary>
        /// Prepare data for uploading to DB
        /// </summary>
        public void UploadData(string[] data)
        {
            var kindergardensData = ConvertData(data);

            foreach (var kg in kindergardensData.Kindergardens)
            {
                kindergardensData.UniqueKindergardens.Add(new Kindergarden()
                {
                    Name = kg.Name,
                });
            }
            var kindergardens = repository.SaveKindergarden(kindergardensData.UniqueKindergardens);

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

            repository.SaveGroup(kindergardensData.UniqueGroups);

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

            repository.AttachChild(kindergardensData.UniqueChildren);
            repository.AttachGroupChild(kindergardensData.GroupChild);
            repository.AttachAttendance(kindergardensData.Attendance);

            context.SaveChanges();
        }

        /// <summary>
        /// Returns child by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Child GetChild(long id)
        {
            var kindergardens = repository.GetKindergardens();
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
            var kindergardens = repository.GetKindergardens();

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
        /// Returns children count in different kindergardens
        /// </summary>
        /// <returns></returns>
        public RequestData GetKindergardensChildrenCount()
        {
            var requestData = new RequestData();
            var kindergardensChildrenCount = new List<KindergardenChildren>();
            var kindergardens = repository.GetKindergardens();
            int childrenCount;

            foreach (var kindergarden in kindergardens)
            {
                childrenCount = 0;
                foreach (var group in kindergarden.Groups)
                {
                    foreach (var groupChild in group.GroupChildRelation)
                    {
                        childrenCount++;
                    }
                }
                kindergardensChildrenCount.Add(new KindergardenChildren()
                {
                    Name = kindergarden.Name,
                    ChildrenCount = childrenCount,
                });
            }
            requestData.KindergardenChildren = kindergardensChildrenCount;

            return requestData;
        }

        /// <summary>
        /// Returns most sick group
        /// </summary>
        /// <returns></returns>
        public Group GetMostSickGroup()
        {
            var kindergardens = repository.GetKindergardens();
            Group mostSickGroup = null;
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
                        mostSickGroup = new Group()
                        {
                            KindergardenId = kg.Id,
                            Name = group.Name,
                        };
                    }
                    sick = 0;
                }
            }

            return mostSickGroup;
        }

        /// <summary>
        /// Returns healthiest group
        /// </summary>
        /// <returns></returns>
        public string GetHealthiestGroup()
        {
            var kindergardens = repository.GetKindergardens();
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
            var kindergardens = repository.GetKindergardens();
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
            var kindergardens = repository.GetKindergardens();
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
            var kindergardens = repository.GetKindergardens();
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
            var kindergardens = repository.GetKindergardens();
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