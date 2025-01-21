using CRM_ERP_UNID.Controllers.Users;
using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUsersService _usersService;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext context,
            IUsersService usersService)
        {
            _logger = logger;
            this._context = context;
            this._usersService = usersService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }

        [HttpGet("UsersTest")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            List<User> users = await this._context.Users.ToListAsync();

            if (users.Count == 0)
            {
                return NotFound(users);
            }

            return Ok(users);
        }

        [HttpGet("getUser")]
        public async Task<ActionResult<User>> GetUser([FromQuery] Guid id)
        {
            User? user = await this._usersService.GetById(id);

            if (user == null)
            {
                return NotFound(user);
            }

            return Ok(user);
        }

        [HttpPost("create")]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            User? user = await this._usersService.Create(createUserDto);

            if (user == null)
            {
                return BadRequest("Ocurred some probleming creating the user");
            }

            return Ok(user);
        }
    }
}