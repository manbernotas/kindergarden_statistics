using System;
using System.Collections.Generic;
using System.Text;

namespace KindergardenStatistics.BL
{
    public static class ExtensionMethods
    {
        public static void BulkSaveChanges(int count)
        {
            if (count > 1 && count % 5000 == 0)
            {
                //context.SaveChanges();
            }
        }
    }
}
