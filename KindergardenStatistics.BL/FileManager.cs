using KindergardenStatistics.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KindergardenStatistics.BL
{
    public class FileManager
    {
        public String[] ReadFile(string fileName)
        {
            var data = File.ReadAllLines(fileName);

            return data;
        }
    }
}
