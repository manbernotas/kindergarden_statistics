using KindergardenStatistics.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace KindergardenStatistics.BL
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Method to compare two kindergardens objects
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int CompareKindergardens(this Kindergarden x, Kindergarden y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
