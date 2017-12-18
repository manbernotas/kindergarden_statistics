using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KindergardenStatistics.DAL
{
    public class Child
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        //TODO change to bool
        public int RegisteredInCity { get; set; }

        public List<GroupChild> GroupChildRelation { get; set; }

        public List<Attendance> Attendance { get; set; } = new List<Attendance>();
    }
}
