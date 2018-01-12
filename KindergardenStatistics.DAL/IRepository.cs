using System;
using System.Collections.Generic;
using System.Text;

namespace KindergardenStatistics.DAL
{
    public interface IRepository
    {
        List<Kindergarden> GetKindergardens();

        void AttachChild(List<Child> child);

        void AttachGroupChild(List<GroupChild> groupChild);

        void AttachAttendance(List<Attendance> attendance);

        List<Kindergarden> SaveKindergarden(HashSet<Kindergarden> kindergardens);

        void SaveGroup(List<Group> groups);
    }
}
