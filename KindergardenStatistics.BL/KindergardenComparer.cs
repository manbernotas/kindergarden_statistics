using KindergardenStatistics.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace KindergardenStatistics.BL
{
    public class KindergardenComparer : IEqualityComparer<Kindergarden>
    {
        public bool Equals(Kindergarden x, Kindergarden y)
        {
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode(Kindergarden obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
