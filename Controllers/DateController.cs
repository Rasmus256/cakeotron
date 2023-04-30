using CakeOTron.Service;
using Microsoft.AspNetCore.Mvc;

namespace CakeOTron.Controllers
{
    [ApiController]
    [Route("")]
    public class DateController : ControllerBase
    {

        private readonly ILogger<DateController> _logger;

        public DateController(ILogger<DateController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<CakeReason> Get()
        {
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
}