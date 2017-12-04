using System;
using System.Collections.Generic;
using System.Text;

namespace KindergardenStatistics.DAL
{
    public class Kindergarden

    {
        public Kindergarden() {}

        public Kindergarden(int id, string name, List<Group> groups)
        {
            Id = id;
            Name = name;
            Groups = groups;
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Group> Groups { get; set; } = new List<Group>();
    }
}
