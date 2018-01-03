using KindergardenStatistics.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KindergardenStatistics.BL
{
    public class FileManager : IDataManager
    {
        /// <summary>
        /// Put all lines of the file to array of strings
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string[] GetData(string fileName)
        {
            var data = File.ReadAllLines(fileName);

            return data;
        }
    }
}
