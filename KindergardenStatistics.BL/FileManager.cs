using KindergardenStatistics.DAL;
using System.IO;

namespace KindergardenStatistics.BL
{
    public class FileManager
    {
        private KindergardenContext context;

        public FileManager(KindergardenContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Put all lines of the file to array of strings
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public void GetData(string fileName)
        {
            var data = File.ReadAllLines(fileName);
            var kindergardenManager = new KindergardenManager(context);

            kindergardenManager.UploadData(data);
        }
    }
}
