using KindergardenStatistics.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace KindergardenStatistics.BL
{
    public class KindergardenData
    {
        public KindergardenData() {}

        public List<Kindergarden> Kindergardens { get; set; } = new List<Kindergarden>();
        public List<Kindergarden> UniqueKindergardens { get; set; } = new List<Kindergarden>();
        public List<Group> GroupNames { get; set; } = new List<Group>();
        public List<Group> UniqueGroups { get; set; } = new List<Group>();
        public List<Child> Children { get; set; } = new List<Child>();
        public List<Child> UniqueChildren { get; set; } = new List<Child>();
        public List<GroupChild> GroupChild { get; set; } = new List<GroupChild>();
        public List<Attendance> Attendance { get; set; } = new List<Attendance>();
        public List<int> SickList { get; set; } = new List<int>();
        public List<int> OtherReasonList { get; set; } = new List<int>();
        public List<int> NoReasonList { get; set; } = new List<int>();
    }
}
