using System;
using System.Collections.Generic;
using System.Text;

namespace KindergardenStatistics.DAL
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int KindergardenId { get; set; }

        public List<Child> Children { get; set; } = new List<Child>();
}
}
