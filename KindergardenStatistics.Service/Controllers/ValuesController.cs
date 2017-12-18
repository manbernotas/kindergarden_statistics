using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KindergardenStatistics.BL;

namespace KindergardenStatistics.Service.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly DAL.KindergardenContext _context;

        private KindergardenManager km;

        public ValuesController(DAL.KindergardenContext context)
        {
            _context = context;
            km = new KindergardenManager(_context);
        }

        // GET api/values
        [HttpGet]
        public List<DAL.Kindergarden> Get()
        {
            var kindergardens = km.GetKindergardens();

            return kindergardens;
        }

        [HttpGet]
        [Route("names")]
        public List<String> GetAllKindergardenNames()
        {
            var kindergardens = km.GetKindergardens();
            var kgNames = new List<String>();

            foreach (var kg in kindergardens)
            {
                kgNames.Add(kg.Name);
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
            return km.GetChild(id);
        }

        /// <summary>
        /// Return kindergarden from child Id
        /// </summary>
        /// <param ChildId="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/kindergarden")]
        public string GetChildsKindergarden(long id)
        {
            return km.GetChildsKindergarden(id);
        }

        /// <summary>
        /// Most sick group
        /// </summary>
        /// <returns></returns>
        [HttpGet("most-sick-group")]
        public string GetMostSickGroup()
        {
            return km.GetMostSickGroup();
        }

        /// <summary>
        /// Upload data from file
        /// </summary>
        /// <returns></returns>
        [HttpGet("upload")]
        public void UploadData()
        {
            km.UploadDataFromFile("D:\\test.csv");
        }

        /// <summary>
        /// Healthiest group
        /// </summary>
        /// <returns></returns>
        [HttpGet("healthiest-group")]
        public string GetHealthiestGroup()
        {
            return km.GetHealthiestGroup();
        }

        /// <summary>
        /// Healthiest kindergarden
        /// </summary>
        /// <returns></returns>
        [HttpGet("healthiest-kg")]
        public string GetHealthiestKg()
        {
            return km.GetHealthiestKg();
        }

        /// <summary>
        /// Most sick kindergarden
        /// </summary>
        /// <returns></returns>
        [HttpGet("most-sick-kg")]
        public string GetMostSickKg()
        {
            return km.GetMostSickKg();
        }

        /// <summary>
        /// Children id ordered by attendance
        /// </summary>
        /// <returns></returns>
        [HttpGet("top-attendance")]
        public List<int> GetTopAttendance()
        {
            return km.GetChildrenIdOrderedByAttendance();
        }

        /// <summary>
        /// Top two kindergarden names ordered by attendance
        /// </summary>
        [HttpGet("top-kg")]
        public List<string> GetTopTwoKgNamesOrderedByAttendance()
        {
            return km.GetTopTwoKgNamesOrderedByAttendance();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
