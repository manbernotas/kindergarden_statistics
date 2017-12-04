using System;
using System.Collections.Generic;
using System.Text;

namespace KindergardenStatistics.DAL
{
    public class Attendance
    {
        public int Id { get; set; }

        public int ChildId { get; set; }

        public DateTime Date { get; set; } = new DateTime();

        public int Sick { get; set; }

        public int OtherReasons { get; set; }

        public int NoReasons { get; set; }

    }
}
