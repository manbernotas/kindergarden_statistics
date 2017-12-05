using System;
using System.Collections.Generic;
using System.Linq;

namespace KindergardenStatistics.DAL
{
    public class Repository
    {
        public string GetName()
        {
            return "name";
        }
        public List<Kindergarden> GetKindergardens()
        {
            var kindergardens = FakeData.Kindergardens;
            return kindergardens;
        }

        public Child GetChild(int id)
        {
            var kindergardens = FakeData.Kindergardens;

            foreach (var kg in kindergardens)
            {
                foreach (var group in kg.Groups)
                {
                    foreach (var child in group.Children)
                    {
                        if (child.Id == id)
                        {
                            return child;
                        }
                    }
                }
            }

            return null;
        }

        public string GetChildsKindergarden(int id)
        {
            var kindergardens = FakeData.Kindergardens;

            //var group = kindergardens.Select(kg => kg.Groups).ToList();
            //var child = group.Select(grp => grp.Children).ToList();

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
            var kindergardens = FakeData.Kindergardens;
            string mostSickGroupName = string.Empty;
            int mostSick = 0;

            foreach (var kg in kindergardens)
            {
                foreach (var group in kg.Groups)
                {
                    int sick = 0;

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
            }

            return mostSickGroupName;
        }

        public string GetHealthiestGroup()
        {
            var kindergardens = FakeData.Kindergardens;
            string healthiestGroupName = string.Empty;
            int leastSick = int.MaxValue;

            foreach (var kg in kindergardens)
            {
                foreach (var group in kg.Groups)
                {
                    int sick = 0;

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

        public List<int> GetTopAttendance()
        {
            var kindergardens = FakeData.Kindergardens;
            var childrenAttendance = new List<List<int>>();

            foreach (var kg in kindergardens)
            {
                foreach (var group in kg.Groups)
                {
                    foreach (var child in group.Children)
                    {
                        foreach (var att in child.Attendances)
                        {
                            childrenAttendance.Add(new List<int> {child.Id, att.Sick });
                        }
                    }
                }
            }
            var topChildrenAttendance = childrenAttendance.OrderBy(list => list[1]).ToList();
            var topAttendanceChildrenId = topChildrenAttendance.Select(list => list[0]).ToList();

            return topAttendanceChildrenId;
        }

        public List<string> GetTopAttendanceKg()
        {
            var kindergardens = FakeData.Kindergardens;
            string healthiestKgName = string.Empty;
            var kgAttendance = new List<List<string>>();

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
            var topAttendanceKg = kgAttendance.OrderBy(list => list[1]).ToList();
            var topAttendanceKgName = topAttendanceKg.Select(list => list[0]).Take(2).ToList();

            return topAttendanceKgName;
        }

        public string GetMostSickKg()
        {
            var kindergardens = FakeData.Kindergardens;
            string mostSickKgName = string.Empty;
            int mostSick = 0;

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
            var kindergardens = FakeData.Kindergardens;
            string healthiestKgName = string.Empty;
            int leastSick = int.MaxValue;

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
