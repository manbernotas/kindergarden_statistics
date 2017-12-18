using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace KindergardenStatistics.DAL
{
    public class Repository
    {
        private KindergardenContext context;

        public Repository(KindergardenContext context)
        {
            this.context = context;
        }

        public List<Kindergarden> GetKindergardens()
        {
            var kindergardens = context.Kindergarden
                //.Include("Groups")
                //.Include("Groups.GroupChildRelation")
                //.Include("Groups.GroupChildRelation.Child")
                .Include("Groups.GroupChildRelation.Child.Attendance");

            return kindergardens.ToList();
        }

        /// <summary>
        /// Save child data in the DB
        /// </summary>
        /// <param name="childId"></param>
        /// <param name="groupId"></param>
        /// <param name="registerInCity"></param>
        /// <returns></returns>
        public Child SaveChild(long childId, int groupId, int registerInCity)
        {
            var child = context.Child.Add(new Child()
            {
                Id = childId,
                RegisteredInCity = registerInCity,
            });

            context.GroupChild.Add(new GroupChild()
            {
                ChildId = childId,
                GroupId = groupId,
                Started = DateTime.Today,
                Current = true,
            });

            //context.SaveChanges();

            return child.Entity;
        }

        /// <summary>
        /// Save child attendance data in the DB
        /// </summary>
        /// <param name="childId"></param>
        /// <param name="sick"></param>
        /// <param name="noReasons"></param>
        /// <param name="otherReasons"></param>
        public void SaveAttendance(long childId, int sick, int noReasons, int otherReasons)
        { 
            context.Attendance.Add(new Attendance()
            {
                ChildId = childId,
                Date = DateTime.Today,
                Sick = sick,
                NoReasons = noReasons,
                OtherReasons = otherReasons,
            });

            //context.SaveChanges();
        }

        /// <summary>
        /// Save kindergarden data in the DB
        /// </summary>
        /// <param name="kgName"></param>
        /// <returns></returns>
        public Kindergarden SaveKindergarden(string kgName)
        {
            var kindergarden = context.Kindergarden.Add(new Kindergarden()
            {
                Name = kgName,
            });

            context.SaveChanges();

            return kindergarden.Entity;
        }

        /// <summary>
        /// Save group data in the DB
        /// </summary>
        /// <param name="kindergardenId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public Group SaveGroup(int kindergardenId, string groupName)
        {
            var group = context.Group.Add(new Group()
            {
                KindergardenId = kindergardenId,
                Name = groupName,
            });

            //context.SaveChanges();

            return group.Entity;
        }
    }
}
