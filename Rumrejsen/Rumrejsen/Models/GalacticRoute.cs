namespace Rumrejsen.Models
{
    public class GalacticRoute
    {
        public string name { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public NavigationPoint[] navigationPoints { get; set; }
        public string duration { get; set; }
        public Dangers[] dangers { get; set; }
        public string fuelUsage { get; set; }
        public string description { get; set; }
    }
}
