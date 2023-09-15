using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rumrejsen.Models;

namespace Rumrejsen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KadetController : ControllerBase
    {
        [HttpGet("KadetRoutes")]
        [Authorize(Roles = "Kadet")]
        public ActionResult KadetRoutes()
        {
            // Retrieve the galacticRoutes data from the session
            var galacticRoutes = HttpContext.Session.GetObjectFromJson<List<GalacticRoute>>("GalacticRoutes") ?? new List<GalacticRoute>();

            if (Request.Cookies.ContainsKey("CountCookie"))
            {
                int? CountCookie = Convert.ToInt32(Request.Cookies["CountCookie"]);
                int count;
                // Check if 'CountCookie' has a value and if it's greater than 5
                if (CountCookie.HasValue && CountCookie.Value < 5)
                {
                    count = CountCookie.Value + 1;
                    // Add or update the "CountCookie" in the response cookies with the new 'count' value
                    Response.Cookies.Append("CountCookie", count.ToString());
                }
                else
                {
                    return BadRequest("You have asked your quota come back in 30 minutes time");
                }

            }
            else
            {
                // Create a CookieOptions instance and set the expiration time to 30 minutes from now
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddMinutes(30);

                // Add or update the "CountCookie" in the response cookies with the new 'count' value
                Response.Cookies.Append("CountCookie", "1".ToString(), cookieOptions);
            }

            // Check if it has any data
            if (galacticRoutes != null && galacticRoutes.Any())
            {
                List<GalacticRoute> approvedRoutes = new List<GalacticRoute>();

                foreach (var route in galacticRoutes)
                {
                    if(route.duration < 365)
                    {
                        approvedRoutes.Add(route);
                    }
                }

                return Ok(approvedRoutes);
            }
            else
            {
                return NotFound("No galactic routes found.");
            }
        }

        [HttpGet("KadetPickDestination")]
        [Authorize(Roles = "Kadet")]
        public ActionResult KadetPickDestination([FromQuery] string name)
        {
            // Retrieve the galacticRoutes data from the session
            var galacticRoutes = HttpContext.Session.GetObjectFromJson<List<GalacticRoute>>("GalacticRoutes") ?? new List<GalacticRoute>();



            if (Request.Cookies.ContainsKey("CountCookie"))
            {
                int? CountCookie = Convert.ToInt32(Request.Cookies["CountCookie"]);
                int count;
                // Check if 'CountCookie' has a value and if it's greater than 5
                if (CountCookie.HasValue && CountCookie.Value < 5)
                {
                    count = CountCookie.Value + 1;
                    // Add or update the "CountCookie" in the response cookies with the new 'count' value
                    Response.Cookies.Append("CountCookie", count.ToString());
                }
                else
                {
                    return BadRequest("You have asked your quota come back in 30 minutes time");
                }

            }
            else
            {
                // Create a CookieOptions instance and set the expiration time to 30 minutes from now
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddMinutes(30);

                // Add or update the "CountCookie" in the response cookies with the new 'count' value
                Response.Cookies.Append("CountCookie", "1".ToString(), cookieOptions);
            }



            List<GalacticRoute> approvedRoutes = new List<GalacticRoute>();

            foreach (var route in galacticRoutes)
            {
                if (route.duration < 365)
                {
                    approvedRoutes.Add(route);
                }
            }

            foreach (var aproute in approvedRoutes)
            {
                if (aproute.name == name) 
                { 
                    return Ok(aproute);
                }
            }
            

            return BadRequest("Route not Found or access denied");
        }
    }
}
