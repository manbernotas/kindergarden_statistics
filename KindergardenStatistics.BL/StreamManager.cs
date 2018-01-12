using KindergardenStatistics.DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KindergardenStatistics.BL
{
    public class StreamManager
    {
        private KindergardenContext context;

        public StreamManager(KindergardenContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// <summary>
        /// Put all lines of the stream to array of strings
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string[] GetData(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new FileNotFoundException();
            }

            var data = new List<string>();
            var stream = new StreamReader(file.OpenReadStream());

            while (stream.Peek() != -1)
            {
                data.Add(stream.ReadLine());
            }

            var kindergardenManager = new KindergardenManager(context);
            kindergardenManager.UploadData(data.ToArray());
            
            return data.ToArray();
        }
    }
}
