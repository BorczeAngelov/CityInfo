namespace CityInfo.API.Models
{
    /// <summary>
    /// Note: this Dto class is not ideal for Post requests, as we wabt the Id property to be set by server.
    /// </summary>
    public class PointOfInterestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
