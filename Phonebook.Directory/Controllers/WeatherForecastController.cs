using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Phonebook.Directory.Persistence;

namespace Phonebook.Directory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly PhonebookDbContext phonebookDbContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, PhonebookDbContext phonebookDbContext)
        {
            _logger = logger;
            this.phonebookDbContext = phonebookDbContext;
        }

        [HttpGet(Name = "GetTest")]
        public async Task<IActionResult> Get()
        {
            return Ok(null);
        }
    }
}
