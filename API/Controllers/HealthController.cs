using DTO;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)] // Explicitly disable caching
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Check the health status of the API.
        /// </summary>
        /// <returns>Returns a health status response.</returns>
        /// <response code="200">API is healthy.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("health")]
        [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<HealthResponse> GetHealth()
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var response = new HealthResponse
                {
                    Status = "OK",
                    Version = "1.0",
                    DateTime = DateTime.UtcNow,
                    TimeResponse = stopwatch.ElapsedMilliseconds
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                stopwatch.Stop();
            }
        }
    }
}