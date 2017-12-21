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
        public void SaveChild(List<Child> child)
        {
            context.Child.AddRange(child);
            //var child = context.Child.Add(new Child()
            //{
            //    Id = childId,
            //    RegisteredInCity = registerInCity,
            //});

            //context.GroupChild.Add(new GroupChild()
            //{
            //    ChildId = childId,
            //    GroupId = groupId,
            //    Started = DateTime.Today,
            //    Current = true,
            //});

            //context.SaveChanges();

            //return child.Entity;
        }
        public void SaveGroupChild(List<GroupChild> groupChild)
        {
            context.GroupChild.AddRange(groupChild);
        }

        public void SaveAttendance(List<Attendance> attendance)
        {
            context.Attendance.AddRange(attendance);
        }

        /// <summary>
        /// Save kindergarden data in the DB
        /// </summary>
        /// <param name="kgName"></param>
        /// <returns></returns>
        public void SaveKindergarden(List<Kindergarden> kindergardens)
        {
            // TODO: Create a helper method to convert List<string> to List<Kindergarden>
            // and then use context.Kindergarden.AddRange method
            //foreach (var kgName in kgNames)
            //{
            context.Kindergarden.AddRange(kindergardens);
            //      new Kindergarden()
            //    {
            //        Name = kgName,
            //    });
            //}
            
            context.SaveChanges();

            //return kindergarden.Entity;
        }

        /// <summary>
        /// Save group data in the DB
        /// </summary>
        /// <param name="kindergardenId"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        //public Group SaveGroup(int kindergardenId, string groupName)
        public void SaveGroup(List<Group> groups)
        {
            context.Group.AddRange(groups);
            //foreach (var group in groups)
            //{
            //    context.Group.Add(new Group()
            //    {
            //        KindergardenId = group.Value,
            //        Name = group.Key,
            //    });
            //}

            context.SaveChanges();

            //return group.Entity;
        }
    }
}
