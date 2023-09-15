using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Rumrejsen.Models;

namespace Rumrejsen.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoadRoutesController : ControllerBase
    {
        [HttpPost("LoadRoutes")]
        public ActionResult LoadRoutes()
        {
            // Read the JSON data from the file
            string jsonFilePath = "C:\\Users\\zbclshi\\source\\repos\\Rumrejsen\\Rumrejsen\\galacticRoutes.json"; // Replace with the actual file path
            string jsonContent = System.IO.File.ReadAllText(jsonFilePath);

            // Deserialize the JSON data into a GalacticRouteData object
            GalacticRouteData routeData = JsonConvert.DeserializeObject<GalacticRouteData>(jsonContent);

            // Store the loaded data in the session
            HttpContext.Session.SetObjectAsJson("GalacticRoutes", routeData.galacticRoutes);


            // Retrieve the galacticRoutes data from the session
            var galacticRoutes = HttpContext.Session.GetObjectFromJson<List<GalacticRoute>>("GalacticRoutes") ?? new List<GalacticRoute>();

            return Ok(galacticRoutes);
        }
    }

}