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

        public string GetMostSick()
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
                        foreach (var att in child.Attendences)
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
    }
}
