using System;
using System.Collections.Generic;
using System.Text;

namespace KindergardenStatistics.DAL
{
    public class Child
    {
        public int Id { get; set; }

        public bool RegisteredInCity { get; set; }

        public int GroupId { get; set; }

        public List<Attendance> Attendences { get; set; } = new List<Attendance>();
    }
}
