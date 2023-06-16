using CakeOTron.Service;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace CakeOTron.Controllers
{
    [ApiController]
    [Route("")]
    public class DateController : ControllerBase
    {

        private readonly ILogger<DateController> _logger;
        private static Dictionary<string, IEnumerable<CakeReason>> _cache = new Dictionary<string, IEnumerable<CakeReason>>();
        private static HttpClient client = new HttpClient();
        
        public DateController(ILogger<DateController> logger)
        {
            _logger = logger;
        }
        
        public async Task<IEnumerable<ReferenceDate>> GetDates()
        {

            _logger.LogInformation($"Initiated external call");
            HttpResponseMessage response = await client.GetAsync("http://cakeotron-dataservice.cake.svc.cluster.local:80/dates");
            _logger.LogInformation($"Finished external call");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                _logger.LogInformation(json);
                var r = JsonConvert.DeserializeObject<List<ReferenceDate>>(json);
                _logger.LogInformation("Deserialized dates");
                return r;
            }
            return new List<ReferenceDate>();
        }
        [HttpGet()]
        public async Task<IEnumerable<CakeReason>> Get()
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
            var referenceDates = await GetDates();
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
}
