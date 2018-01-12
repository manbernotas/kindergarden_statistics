using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using KindergardenStatistics.BL;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using KindergardenStatistics.Model;

namespace KindergardenStatistics.Service.Controllers
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
        /// Calls data import service
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("callDataImport")]
        public async Task<bool> CallDataImportAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var serializer = new DataContractJsonSerializer(typeof(IEnumerable<string>));
            var stream = client.GetStreamAsync("http://localhost:5001/api/values");
            var response = serializer.ReadObject(await stream) as IEnumerable<string>;

            if (response.FirstOrDefault() != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calls statistics service 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("callStatisticsService")]
        public RequestData CallStatisticsService()
        {
            var requestData = kindergardenManager.GetKindergardensChildrenCount();
            var client = new HttpClient();
            var serializer = new DataContractJsonSerializer(typeof(RequestData));
            var jsonInString = JsonConvert.SerializeObject(requestData);
            var response = client.PostAsync(
                "http://localhost:5002/api/kindergarden",
                new StringContent(jsonInString, Encoding.UTF8, "application/json")
                ).Result;
            
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<RequestData>(responseContent);

            return result;
        }

        /// <summary>
        /// Return all kindergardens
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-kindergardens")]
        public List<DAL.Kindergarden> Get()
        {
            var kindergardens = kindergardenManager.GetKindergardens();

            return kindergardens;
        }

        /// <summary>
        /// Returns all kindergardens names
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<String> GetAllKindergardenNames()
        {
            var kindergardens = kindergardenManager.GetKindergardens();
            var kgNames = new List<String>();

            foreach (var kindergarden in kindergardens)
            {
                kgNames.Add(kindergarden.Name);
            }
            
            return kgNames;
        }

        /// <summary>
        /// Returns child by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public DAL.Child GetChild(long id)
        {
            return kindergardenManager.GetChild(id);
        }

        /// <summary>
        /// Return kindergarden from child Id
        /// </summary>
        /// <param ChildId="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/kindergarden")]
        public string GetChildsKindergarden(long childId)
        {
            return kindergardenManager.GetChildsKindergarden(childId);
        }

        /// <summary>
        /// Most sick group
        /// </summary>
        /// <returns></returns>
        [HttpGet("most-sick-group")]
        public DAL.Group GetMostSickGroup()
        {
            return kindergardenManager.GetMostSickGroup();
        }

        /// <summary>
        /// Upload data from file
        /// </summary>
        /// <returns></returns>
        [HttpGet("upload")]
        public void UploadData()
        {
            var fileManager = new FileManager(context);
            fileManager.GetData("D:\\test.csv");
        }

        /// <summary>
        /// Healthiest group
        /// </summary>
        /// <returns></returns>
        [HttpGet("healthiest-group")]
        public string GetHealthiestGroup()
        {
            return kindergardenManager.GetHealthiestGroup();
        }

        /// <summary>
        /// Healthiest kindergarden
        /// </summary>
        /// <returns></returns>
        [HttpGet("healthiest-kg")]
        public string GetHealthiestKg()
        {
            return kindergardenManager.GetHealthiestKg();
        }

        /// <summary>
        /// Most sick kindergarden
        /// </summary>
        /// <returns></returns>
        [HttpGet("most-sick-kg")]
        public string GetMostSickKg()
        {
            return kindergardenManager.GetMostSickKg();
        }

        /// <summary>
        /// Children id ordered by attendance
        /// </summary>
        /// <returns></returns>
        [HttpGet("top-attendance")]
        public List<int> GetTopAttendance()
        {
            return kindergardenManager.GetChildrenIdOrderedByAttendance();
        }

        /// <summary>
        /// Top two kindergarden names ordered by attendance
        /// </summary>
        [HttpGet("top-kg")]
        public List<string> GetTopTwoKgNamesOrderedByAttendance()
        {
            return kindergardenManager.GetTopTwoKgNamesOrderedByAttendance();
        }
    }
}
