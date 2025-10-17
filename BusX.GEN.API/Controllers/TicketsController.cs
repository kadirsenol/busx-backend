using BusXAppServiceModels.DTO;
using BusXAppServiceModels.Request;
using BusX.AppService.Interfaces;
using BusX.Models.Base;
using Microsoft.AspNetCore.Mvc;
using BusXAppServiceModels.Response;
using BusX.AppService.Services;
namespace BusX.GEN.API.Controllers
{
    [Route("api/v1/tickets")]
    [ApiController]
    public class TicketsController(ITicketsAppService TicketAppService ) : ControllerBase()
    {
        private readonly ITicketsAppService _TicketAppService = TicketAppService;
        [HttpGet]
        public IActionResult Search([FromQuery] TicketSearch request) => Ok(_TicketAppService.Search(request));
        [HttpGet("get")]
        public IActionResult Get([FromQuery] GetDetailRequest request) => Ok(_TicketAppService.Get(request));
        [HttpGet("info")]
        public IActionResult Info([FromQuery] GetDetailRequest request) => Ok(_TicketAppService.Info(request));
        [HttpPost]
        public IActionResult CreateOrEdit(TicketDto request) => Ok(_TicketAppService.CreateOrEdit(request));
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) => Ok(_TicketAppService.Delete(id));

        [HttpPost("checkout")]
        public IActionResult Checkout(CheckoutDto request) => Ok(_TicketAppService.Checkout(request));

        [HttpGet("health")]
        public IActionResult HealthCheck() => Ok(_TicketAppService.CheckHealth());
    }
}