using CakeOTron.Service;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
            foreach (var item in _cache.Keys.Except(new List<string>{cacheKey}))
            {
                _logger.LogInformation($"Removed stale result from cache for key {item}");
                _cache.Remove(item);
            }
            if (_cache.ContainsKey(cacheKey)) {
                var v = _cache.GetValueOrDefault(cacheKey);
                if(v != null) {
                    _logger.LogInformation($"Found result in cache for key {cacheKey}");
                    return v;
                }
            }
            var criteria = CriteriaRepo.criteria();
            var referenceDates = ReferenceRepo.references();
            var returnValue = new List<CakeReason> { };
            _logger.LogInformation($"About to check {referenceDates.Count()} dates against {criteria.Count()} criteria");
            foreach (var r in referenceDates) 
            {
                foreach(var c in criteria.Where(crit => crit.MakesDateSpecial(r.Date)))
                {
                    try
                    {
                            returnValue.Add(new CakeReason
                            {
                                ReferenceDate = r,
                                Reason = c.Prettyreason
                            });
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
    [Route("/criteriafromsvc")]
    public class CriteriaSvcController : ControllerBase
    {
        private readonly ILogger<CriteriaSvcController> _logger;
        static HttpClient client = new HttpClient();

        
        public CriteriaSvcController(ILogger<CriteriaSvcController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Criteria> Get()
        {

            _logger.LogInformation($"Initiated external call");
            HttpResponseMessage response = await client.GetAsync("http://cakeotron.cake.svc.cluster.local/criteria");
            _logger.LogInformation(response.toString());
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                _logger.LogInformation(json);
                return JsonConvert.DeserializeObject<List<Criteria>>(jsonString);
            }
            return new List<Criteria>();
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
