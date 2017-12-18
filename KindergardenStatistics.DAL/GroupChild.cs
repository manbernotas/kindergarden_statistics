using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KindergardenStatistics.DAL
{
    public class GroupChild
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public long ChildId { get; set; }

        public int GroupId { get; set; }

        public DateTime Started { get; set; }

        public bool Current { get; set; }

        public Group Group { get; set; }

        public Child Child { get; set; }
    }
}
