using System;
using System.Collections.Generic;

namespace KindergardenStatistics.DAL
{
    public class FakeRepository : IRepository
    {
        public void AttachAttendance(List<Attendance> attendance)
        {
            throw new NotImplementedException();
        }

        public void AttachChild(List<Child> child)
        {
            throw new NotImplementedException();
        }

        public void AttachGroupChild(List<GroupChild> groupChild)
        {
            throw new NotImplementedException();
        }

        public List<Kindergarden> GetKindergardens()
        {
            throw new NotImplementedException();
        }

        public void SaveGroup(List<Group> groups)
        {
            throw new NotImplementedException();
        }

        public List<Kindergarden> SaveKindergarden(HashSet<Kindergarden> kindergardens)
        {
            throw new NotImplementedException();
        }
    }
}
