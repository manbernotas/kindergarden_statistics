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

        /// <summary>
        /// Returns list of all kindergardens
        /// </summary>
        /// <returns></returns>
        public List<Kindergarden> GetKindergardens()
        {
            var kindergardens = context.Kindergarden.Include("Groups.GroupChildRelation.Child.Attendance");

            return kindergardens.ToList();
        }

        /// <summary>
        /// Attach list of childs to the database
        /// </summary>
        /// <param name="child"></param>
        public void AttachChild(List<Child> child)
        {
            context.Child.AddRange(child);
        }

        /// <summary>
        /// Attach list of groupChild relations to the database
        /// </summary>
        /// <param name="groupChild"></param>
        public void AttachGroupChild(List<GroupChild> groupChild)
        {
            context.GroupChild.AddRange(groupChild);
        }

        /// <summary>
        /// Attach list of children attendance to the database
        /// </summary>
        /// <param name="attendance"></param>
        public void AttachAttendance(List<Attendance> attendance)
        {
            context.Attendance.AddRange(attendance);
        }

        /// <summary>
        /// Save list of kindergardens to the database
        /// </summary>
        /// <param name="kindergardens"></param>
        /// <returns></returns>
        public List<Kindergarden> SaveKindergarden(List<Kindergarden> kindergardens)
        {
            context.Kindergarden.AddRange(kindergardens);
            context.SaveChanges();

            return context.Kindergarden.ToList();
        }

        /// <summary>
        /// Save list of groups to the database
        /// </summary>
        /// <param name="groups"></param>
        public void SaveGroup(List<Group> groups)
        {
            context.Group.AddRange(groups);
            context.SaveChanges();
        }
    }
}
