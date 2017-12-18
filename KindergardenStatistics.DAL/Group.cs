using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KindergardenStatistics.DAL
{
    public class Group
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int KindergardenId { get; set; }

        public List<GroupChild> GroupChildRelation { get; set; }
    }
}
