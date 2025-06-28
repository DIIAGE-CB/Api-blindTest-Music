namespace DTO
{
    public class HealthResponse
    {
        /// <summary>
        /// Service status (OK, Degraded, Unhealthy)
        /// </summary>
        public string Status { get; set; } = "OK";

        /// <summary>
        /// API version number
        /// </summary>
        public string Version { get; set; } = "1.0";

        /// <summary>
        /// UTC timestamp of the response
        /// </summary>
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Response time in milliseconds
        /// </summary>
        public long TimeResponse { get; set; }
    }
}