using System;
using System.Collections.Generic;
using System.Text;

namespace KindergardenStatistics.BL
{
    public interface IDataManager
    {
        string[] GetData(string source);
    }
}
