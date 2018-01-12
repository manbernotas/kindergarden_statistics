using KindergardenStatistics.BL;
using KindergardenStatistics.Model;
using Microsoft.AspNetCore.Mvc;

namespace KindergardenStatistics.Service.StatisticsService.Controllers
{
    [Route("api/[controller]")]
    public class KindergardenController : Controller
    {
        private readonly DAL.KindergardenContext context;

        private KindergardenManager kindergardenManager;

        public KindergardenController(DAL.KindergardenContext context)
        {
            this.context = context;
            kindergardenManager = new KindergardenManager(this.context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>
        /// 
        /*
        {
            "kindergardenChildren":
            [
                {
                    "name":"Gintarelis",
                    "childrenCount":201
                },
                {
                    "name":"Gintarelis",
                    "childrenCount":201
                }
            ]
        }
        */
        /// </remarks>
        [HttpPost]
        public RequestData Post([FromBody]RequestData data)
        {
            return kindergardenManager.GetKindergardensOrderedByChildrenCount(data);
        }
    }
}
