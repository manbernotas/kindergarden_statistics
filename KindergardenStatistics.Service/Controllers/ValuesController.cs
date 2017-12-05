using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KindergardenStatistics.DAL;

namespace KindergardenStatistics.Service.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public List<Kindergarden> Get()
        {
            Repository repository = new Repository();

            var kindergardens = repository.GetKindergardens();

            return kindergardens;
        }

        [HttpGet]
        [Route("names")]
        public List<String> GetAllKindergardenNames()
        {
            Repository repository = new Repository();
            var kindergardens = repository.GetKindergardens();
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
        public Child GetChild(int id)
        {
            Repository repository = new Repository();

            return repository.GetChild(id);
        }

        /// <summary>
        /// Kuriam darzeli vaikas pagal vaiko id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/kindergarden")]
        public string GetChildsKindergarden(int id)
        {
            Repository repository = new Repository();

            return repository.GetChildsKindergarden(id);
        }

        /// <summary>
        /// Labiausiai serganti grupe
        /// </summary>
        /// <returns></returns>
        [HttpGet("most-sick-group")]
        public string GetMostSick()
        {
            Repository repository = new Repository();

            return repository.GetMostSickGroup();
        }

        /// <summary>
        /// Sveikiausia grupe
        /// </summary>
        /// <returns></returns>
        [HttpGet("healthiest-group")]
        public string GetHealthiestGroup()
        {
            Repository repository = new Repository();

            return repository.GetHealthiestGroup();
        }

        /// <summary>
        /// Sveikiausias darzelis
        /// </summary>
        /// <returns></returns>
        [HttpGet("healthiest-kg")]
        public string GetHealthiestKg()
        {
            Repository repository = new Repository();

            return repository.GetHealthiestKg();
        }

        /// <summary>
        /// Labiausiai sergantis darzelis
        /// </summary>
        /// <returns></returns>
        [HttpGet("most-sick-kg")]
        public string GetMostSickKg()
        {
            Repository repository = new Repository();

            return repository.GetMostSickKg();
        }

        /// <summary>
        /// Grazinti vaikus pagal ju lankomuma (nuo geriausio iki prasciausio)
        /// </summary>
        /// <returns></returns>
        [HttpGet("top-attendance")]
        public List<int> GetTopAttendance()
        {
            Repository repository = new Repository();

            return repository.GetTopAttendance();
        }

        /// <summary>
        /// Grazinti 2 geriausiai lankomus darzelius
        /// </summary>
        [HttpGet("top-kg")]
        public List<string> GetTopAttendanceKg()
        {
            Repository repository = new Repository();

            return repository.GetTopAttendanceKg();
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
