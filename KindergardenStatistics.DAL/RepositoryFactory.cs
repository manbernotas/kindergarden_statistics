using System;
using System.Collections.Generic;
using System.Text;

namespace KindergardenStatistics.DAL
{
    public class RepositoryFactory
    {
        /// <summary>
        /// Returns repository according to passed context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IRepository GetRepository(KindergardenContext context)
        {
            IRepository repository = null;

            switch (context)
            {
                case null:
                    repository = new FakeRepository();
                    break;
                default:
                    repository = new Repository(context);
                    break;
            }

            return repository;
        }
    }
}
