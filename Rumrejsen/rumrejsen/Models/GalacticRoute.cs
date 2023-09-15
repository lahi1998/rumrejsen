namespace Rumrejsen.Models
{
    public class GalacticRoute
    {
        public required string name { get; set; }
        public required string start { get; set; }
        public required string end { get; set; }
        public required string[] navigationPoints { get; set; } = new string[] { }; // Initialize as an empty array
        public required int duration { get; set; }
        public required string[] dangers { get; set; } = new string[] { }; // Initialize as an empty array
        public required string fuelUsage { get; set; }
        public required string description { get; set; }
    }
}
