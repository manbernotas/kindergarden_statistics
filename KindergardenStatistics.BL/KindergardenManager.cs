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
            string mostSickGroupName = string.Empty;
            int mostSick = 0;

            foreach (var group in groups)
            {
                int sick = 0;
                // TODO: to linq
                foreach (var child in group.Children)
                {
                    foreach (var att in child.Attendances)
                    {
                        sick += att.Sick;
                    }
                }
                if (sick > mostSick)
                {
                    mostSick = sick;
                    mostSickGroupName = group.Name;
                }
            }

            return mostSickGroupName;
        }

        public string GetHealthiestGroup()
        {
            var kindergardens = repo.GetKindergardens();
            string healthiestGroupName = string.Empty;
            int leastSick = int.MaxValue;

            foreach (var kg in kindergardens)
            {
                foreach (var group in kg.Groups)
                {
                    int sick = 0;
                    // TODO: to linq
                    foreach (var child in group.Children)
                    {
                        foreach (var att in child.Attendances)
                        {
                            sick += att.Sick;
                        }
                    }

                    if (sick < leastSick)
                    {
                        leastSick = sick;
                        healthiestGroupName = group.Name;
                    }
                }
            }

            return healthiestGroupName;
        }

        public List<int> GetChildrenIdOrderdByAttendance()
        {
            var kindergardens = repo.GetKindergardens();
            var childrenAttendance = new List<ChildsSickDays>();

            // TODO: to linq
            foreach (var kg in kindergardens)
            {
                foreach (var group in kg.Groups)
                {
                    foreach (var child in group.Children)
                    {
                        foreach (var att in child.Attendances)
                        {
                            childrenAttendance.Add(new ChildsSickDays() { Id = child.Id, Sick = att.Sick });
                        }
                    }
                }
            }

            return childrenAttendance.OrderBy(csd => csd.Sick).Select(csd => csd.Id).ToList();
        }

        public List<string> GetTopAttendanceKg()
        {
            var kindergardens = repo.GetKindergardens();
            string healthiestKgName = string.Empty;
            var kgAttendance = new List<List<string>>();
            // TODO: to linq
            foreach (var kg in kindergardens)
            {
                int sick = 0;

                foreach (var group in kg.Groups)
                {
                    foreach (var child in group.Children)
                    {
                        foreach (var att in child.Attendances)
                        {
                            sick += att.Sick;
                        }
                    }
                }
                kgAttendance.Add(new List<string> { kg.Name, sick.ToString() });
            }
            // TODO: fix with structure Dictionary
            var topAttendanceKg = kgAttendance.OrderBy(list => list[1]).ToList();
            var topAttendanceKgName = topAttendanceKg.Select(list => list[0]).Take(2).ToList();

            return topAttendanceKgName;
        }

        public string GetMostSickKg()
        {
            var kindergardens = repo.GetKindergardens();
            var mostSickKgName = string.Empty;
            int mostSick = 0;
            // TODO: to linq
            foreach (var kg in kindergardens)
            {
                int sick = 0;

                foreach (var group in kg.Groups)
                {
                    foreach (var child in group.Children)
                    {
                        foreach (var att in child.Attendances)
                        {
                            sick += att.Sick;
                        }
                    }
                }
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