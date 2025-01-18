using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        /* private readonly AppDbContext _context;

         private static readonly string[] Summaries = new[]
         {
             "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
         };

         private readonly ILogger<WeatherForecastController> _logger;

         public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext context)
         {
             _logger = logger;
             this._context = context;
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
         }*/
    }
}
