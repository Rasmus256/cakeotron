using CakeOTron.Service;
using Microsoft.AspNetCore.Mvc;

namespace CakeOTron.Controllers
{
    [ApiController]
    [Route("")]
    public class DateController : ControllerBase
    {

        private readonly ILogger<DateController> _logger;
        private static Dictionary<string, IEnumerable<CakeReason>> _cache = new Dictionary<string, IEnumerable<CakeReason>>();

        public DateController(ILogger<DateController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public IEnumerable<CakeReason> Get()
        {
            var cacheKey = DateTime.Now.ToShortDateString();
            if (_cache.ContainsKey(cacheKey)) {
                var v = _cache.GetValueOrDefault(cacheKey);
                if(v != null) {
                    return v;
                }
            }
            var criteria = CriteriaRepo.criteria();
            var referenceDates = ReferenceRepo.references();
            var returnValue = new List<CakeReason> { };
            _logger.LogInformation($"About to check {referenceDates.Count()} dates against {criteria.Count()} criteria");
            foreach (var r in referenceDates) 
            {
                foreach(var c in criteria)
                {
                    try
                    {

                        if (c.MakesDateSpecial(r.Date))
                        {

                            returnValue.Add(new CakeReason
                            {
                                ReferenceDate = r,
                                Reason = c.Prettyreason
                            });

                        }
                    }
                    catch (Exception _) { }

                }
            };
            var criteriaForCurr = CriteriaRepo.criteriaNoDate();
            _logger.LogInformation($"About to check today against {criteria.Count()} criteria");
            foreach (var c in criteriaForCurr)
            {
                if(c.MakesDateSpecial())
                {
                    returnValue.Add(new CakeReason
                    {
                        ReferenceDate = new ReferenceDate { Date = DateTimeOffset.UtcNow, Description = "Today" },
                        Reason = c.Prettyreason
                    });
                }
            }
            
            _cache.Add(cacheKey, returnValue);
            return returnValue;
        }
    }

    [ApiController]
    [Route("/cake")]
    public class CakeController : ControllerBase
    {
        [HttpGet]
        public String Get()
        {
            return "Hello from cakeotron";
        }
    }
    [ApiController]
    [Route("/criteria")]
    public class CriteriaController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Criteria> Get()
        {
            return CriteriaRepo.criteria();
        }
    }
    [ApiController]
    [Route("/dates")]
    public class RefrenceController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<ReferenceDate> Get()
        {
            return ReferenceRepo.references();
        }
    }
}
