using System.Collections.Generic;
using System.IO;
using KindergardenStatistics.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KindergardenStatistics.DataImport.Service.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly DAL.KindergardenContext context;

        private KindergardenManager kindergardenManager;

        public ValuesController(DAL.KindergardenContext context)
        {
            this.context = context;
            kindergardenManager = new KindergardenManager(this.context);
        }

        /// <summary>
        /// Uploads file from stream
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public IActionResult UploadFile(IFormFile file)
        {
            var streamManager = new StreamManager(context);
            try
            {
                streamManager.GetData(file);
            }
            catch (FileNotFoundException)
            {
                return StatusCode(400);
            }

            return StatusCode(200);
        }
    }
}
