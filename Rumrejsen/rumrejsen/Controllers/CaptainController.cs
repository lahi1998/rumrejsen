using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Rumrejsen.Models;
using Rumrejsen;
using Microsoft.AspNetCore.Authorization;

namespace Rumrejsen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaptainController : ControllerBase
    {
        [HttpGet("CaptainRoutes")]
        [Authorize(Roles = "Captain")]
        public ActionResult CaptainRoutes()
        {
            // Retrieve the galacticRoutes data from the session
            var galacticRoutes = HttpContext.Session.GetObjectFromJson<List<GalacticRoute>>("GalacticRoutes") ?? new List<GalacticRoute>();

            // Check if it has any data
            if (galacticRoutes != null && galacticRoutes.Any())
            {
                // Do something with galacticRoutes
                return Ok(galacticRoutes);
            }
            else
            {
                return NotFound("No galactic routes found.");
            }
        }

        [HttpGet("CaptainPickDestination")]
        [Authorize(Roles = "Captain")]
        public ActionResult CaptainPickDestination([FromQuery] string name )
        {
            // Retrieve the galacticRoutes data from the session
            var galacticRoutes = HttpContext.Session.GetObjectFromJson<List<GalacticRoute>>("GalacticRoutes") ?? new List<GalacticRoute>();

            foreach (var route in galacticRoutes)
            {
                if (route.name == name )
                {
                    return Ok(route);
                }
            }

            return BadRequest("Route not Found");
        }


    }

}