﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KindergardenStatistics.DAL
{
    public class Attendance
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public long ChildId { get; set; }

        public DateTime Date { get; set; } = new DateTime();

        public int Sick { get; set; }

        public int OtherReasons { get; set; }

        public int NoReasons { get; set; }

        public Child Child { get; set; }
    }
}
