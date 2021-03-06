﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KindergardenStatistics.DAL
{
    public class Kindergarden : IComparer<Kindergarden>

    {
        public Kindergarden() {}

        public Kindergarden(int id, string name, List<Group> groups)
        {
            Id = id;
            Name = name;
            Groups = groups;
        }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Group> Groups { get; set; } = new List<Group>();

        public int Compare(Kindergarden x, Kindergarden y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
