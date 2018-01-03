using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using KindergardenStatistics.BL;

namespace KindergardenStatistics.Service.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly DAL.KindergardenContext _context;

        private KindergardenManager kindergardenManager;

        public ValuesController(DAL.KindergardenContext context)
        {
            _context = context;
            kindergardenManager = new KindergardenManager(_context);
        }

        // GET api/values
        [HttpGet]
        public List<DAL.Kindergarden> Get()
        {
            var kindergardens = kindergardenManager.GetKindergardens();

            return kindergardens;
        }

        /// <summary>
        /// Returns all kindergardens
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("names")]
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
        public string GetMostSickGroup()
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
            kindergardenManager.UploadDataFromFile("D:\\test.csv");
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
