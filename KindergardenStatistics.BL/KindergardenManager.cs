using KindergardenStatistics.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KindergardenStatistics.BL
{
    public class KindergardenManager
    {
        private Repository repo;

        public KindergardenManager()
        {
            repo = new Repository();
        }

        public KindergardenManager(Repository repo)
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

        public Child GetChild(int id)
        {
            var kindergardens = repo.GetKindergardens();
            var child = kindergardens
                .SelectMany(kg => kg.Groups)
                .SelectMany(grp => grp.Children)
                .FirstOrDefault(cld => cld.Id == id);

            return child;
        }

        public string GetChildsKindergarden(int id)
        {
            var kindergardens = repo.GetKindergardens();
            foreach (var kg in kindergardens)
            {
                foreach (var group in kg.Groups)
                {
                    foreach (var child in group.Children)
                    {
                        if (child.Id == id)
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
            var groups = repo.GetKindergardens().SelectMany(kg => kg.Groups);
            var mostSickGroupName = string.Empty;
            int mostSick = 0;

            foreach (var group in groups)
            {
                int sick = group.Children
                    .SelectMany(cld => cld.Attendances)
                    .Sum(att => att.Sick);
                
                if (sick > mostSick)
                {
                    mostSick = sick;
                    mostSickGroupName = group.Name;
                }
                sick = 0;
            }

            return mostSickGroupName;
        }

        public string GetHealthiestGroup()
        {
            var groups = repo.GetKindergardens().SelectMany(kg => kg.Groups);
            var healthiestGroupName = string.Empty;
            var leastSick = int.MaxValue;

            foreach (var group in groups)
            {
                int sick = group.Children
                    .SelectMany(cld => cld.Attendances)
                    .Sum(att => att.Sick);

                if (sick < leastSick)
                {
                    leastSick = sick;
                    healthiestGroupName = group.Name;
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
                .SelectMany(grp => grp.Children)
                .SelectMany(cld => cld.Attendances)
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
                    .SelectMany(grp => grp.Children)
                    .SelectMany(cld => cld.Attendances)
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
                    .SelectMany(grp => grp.Children)
                    .SelectMany(cld => cld.Attendances)
                    .Sum(att => att.Sick);

                if (sick > mostSick)
                {
                    mostSick = sick;
                    mostSickKgName = kg.Name;
                }
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
                    .SelectMany(grp => grp.Children)
                    .SelectMany(cld => cld.Attendances)
                    .Sum(att => att.Sick);

                if (sick < leastSick)
                {
                    leastSick = sick;
                    healthiestKgName = kg.Name;
                }
            }

            return healthiestKgName;
        }
    }
}