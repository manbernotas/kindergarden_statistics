using System;
using System.Collections.Generic;
using System.Linq;

namespace KindergardenStatistics.DAL
{
    public class Repository
    {
        List<Kindergarden> kg;

        public Repository()
        {
            this.kg = FakeData.Kindergardens;
        }
        public Repository(List<Kindergarden> kg)
        {
            this.kg = kg;
        }

        public List<Kindergarden> GetKindergardens()
        {
            return kg;
        }
    }
}
