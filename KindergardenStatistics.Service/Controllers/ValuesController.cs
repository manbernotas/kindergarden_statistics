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
        private KindergardenManager km;

        public ValuesController()
        {
           km = new KindergardenManager();
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
        /// Grazinti vaika pagal id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public DAL.Child GetChild(int id)
        {
            return km.GetChild(id);
        }

        /// <summary>
        /// Kuriam darzeli vaikas pagal vaiko id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/kindergarden")]
        public string GetChildsKindergarden(int id)
        {
            return km.GetChildsKindergarden(id);
        }

        /// <summary>
        /// Labiausiai serganti grupe
        /// </summary>
        /// <returns></returns>
        [HttpGet("most-sick-group")]
        public string GetMostSick()
        {
            return km.GetMostSickGroup();
        }

        /// <summary>
        /// Sveikiausia grupe
        /// </summary>
        /// <returns></returns>
        [HttpGet("healthiest-group")]
        public string GetHealthiestGroup()
        {
            return km.GetHealthiestGroup();
        }

        /// <summary>
        /// Sveikiausias darzelis
        /// </summary>
        /// <returns></returns>
        [HttpGet("healthiest-kg")]
        public string GetHealthiestKg()
        {
            return km.GetHealthiestKg();
        }

        /// <summary>
        /// Labiausiai sergantis darzelis
        /// </summary>
        /// <returns></returns>
        [HttpGet("most-sick-kg")]
        public string GetMostSickKg()
        {
            return km.GetMostSickKg();
        }

        /// <summary>
        /// Grazinti vaikus pagal ju lankomuma (nuo geriausio iki prasciausio)
        /// </summary>
        /// <returns></returns>
        [HttpGet("top-attendance")]
        public List<int> GetTopAttendance()
        {
            return km.GetChildrenIdOrderdByAttendance();
        }

        /// <summary>
        /// Grazinti 2 geriausiai lankomus darzelius
        /// </summary>
        [HttpGet("top-kg")]
        public List<string> GetTopAttendanceKg()
        {
            return km.GetTopAttendanceKg();
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
