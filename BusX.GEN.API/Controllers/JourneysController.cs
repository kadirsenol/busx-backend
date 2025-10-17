using BusX.AppService.Interfaces;
using BusX.AppServiceModels.Request;
using BusX.Data.Models;
using BusX.Models.Base;
using BusXAppServiceModels.DTO;
using BusXAppServiceModels.Response;
using Microsoft.AspNetCore.Mvc;
using static BusX.AppService.Services.JourneysAppService;
namespace BusX.GEN.API.Controllers
{
    [Route("api/v1/journey")]
    [ApiController]
    public class JourneysController(IJourneysAppService JourneyAppService) : ControllerBase()
    {
        private readonly IJourneysAppService _JourneyAppService = JourneyAppService;

        [HttpGet]
        public IActionResult Search([FromQuery] JourneySearch request) => Ok(_JourneyAppService.Search(request));
        [HttpGet("get")]
        public IActionResult Get([FromQuery] GetDetailRequest request) => Ok(_JourneyAppService.Get(request));
        [HttpGet("info")]
        public IActionResult Info([FromQuery] GetDetailRequest request) => Ok(_JourneyAppService.Info(request));
        [HttpPost]
        public IActionResult CreateOrEdit(JourneyDto request) => Ok(_JourneyAppService.CreateOrEdit(request));
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) => Ok(_JourneyAppService.Delete(id));
        [HttpGet("getAllStations")]
        public IActionResult GetAllStations() => Ok(_JourneyAppService.GetAllStations());
        [HttpGet("{id}/seats")]
        public IActionResult GetSeats(int id) => Ok(_JourneyAppService.GetSeatsPlan(id));

        [HttpGet("health")]
        public IActionResult HealthCheck() => Ok(_JourneyAppService.CheckHealth());


        [HttpPost("journey-seat")]
        public IActionResult CreateOrEditJourneySeat(InProcessJourneySeatDto request)
        {
            try
            {
                var result = _JourneyAppService.CreateOrEditJourneySeat(request);

                return Ok(result);
            }
            catch (SeatAlreadyReservedException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("journey-seat/{journeyID}/{seatNo}")]
        public IActionResult DeleteJourneySeat(int journeyID, int seatNo) => Ok(_JourneyAppService.DeleteJourneySeat(journeyID, seatNo));
    }
}