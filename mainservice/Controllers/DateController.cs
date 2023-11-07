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
        private HttpClient client;
        
        public DateController(ILogger<DateController> logger, HttpClient httpClient)
        {
            _logger = logger;
            client = httpClient;
        }
        
        public async Task<IEnumerable<ReferenceDate>> GetDates()
        {
            return await client.GetFromJsonAsync<List<ReferenceDate>>("https://cake.hosrasmus.hopto.org/dates");
        }
        
        private async Task<IEnumerable<CakeReason>> processReasons(IEnumerable<Criteria> criteria, ReferenceDate r) {
            var returnValue = new List<CakeReason>{};
            returnValue.AddRange(criteria.Where(crit => crit.MakesDateSpecial(r.Date)).Select(c => new CakeReason
                    {
                        ReferenceDate = r,
                        Reason = c.Prettyreason(r.Date)
                    }));
            return returnValue;
        }

        [HttpGet("/cake")]
        public async Task<string> HelloMessage()
        {
            return "Test";
        }
        
        [HttpGet()]
        public async Task<IEnumerable<CakeReason>> Get(bool clearcache = false)
        {
            var dateTask = GetDates();
            if (clearcache)
            {
                _cache.Clear();
            }
            var cacheKey = DateTime.Now.ToShortDateString();
            foreach (var item in _cache.Keys.Except(new List<string>{cacheKey}))
            {
                _logger.LogInformation($"Removed stale result from cache for key {item}");
                _cache.Remove(item);
            }
            if (_cache.ContainsKey(cacheKey)) {
                
                if(_cache.ContainsKey(cacheKey)) {
                    _logger.LogInformation($"Found result in cache for key {cacheKey}");
                    return _cache[cacheKey];
                }
            }
            var criteria = CriteriaRepo.criteria();
            var referenceDates = await dateTask;
            var returnValue = new List<CakeReason> { };
            _logger.LogInformation($"About to check {referenceDates.Count()} dates against {criteria.Count()} criteria");
            var tasks = new List<Task>{};
            foreach (var r in referenceDates) 
            {
                tasks.Add(processReasons(criteria, r)
                    .ContinueWith(async p => {
                        var res = await p;
                        lock(returnValue){
                            returnValue.AddRange(res);
                        }
                    }
                ));
            };
            var criteriaForCurr = CriteriaRepo.criteriaForToday();
            _logger.LogInformation($"About to check today against {criteriaForCurr.Count()} criteria");
            var referenceDate  = new ReferenceDate { Date = DateTimeOffset.UtcNow, Description = "Today" };
                tasks.Add(processReasons(criteriaForCurr, referenceDate)
                    .ContinueWith(async p => {
                        var res = await p;
                        lock(returnValue){
                            returnValue.AddRange(res);
                        }
                    }
                ));
            await Task.WhenAll(tasks);
            _cache.Add(cacheKey, returnValue);
            return returnValue;
        }
    }
}
