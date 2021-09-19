using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using ApiConsumer.Filters;
using ApiConsumer.Interfaces;
using ApiConsumer.Models;
using ApiConsumer.Models.Requests;
using ApiConsumer.Services.Cache;

namespace ApiConsumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ISlotService _slotService;
        public AppController(IMessageService messageService,ISlotService slotService)
        {
            _slotService = slotService;
            _messageService = messageService;
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ServiceFilter(typeof(GetMessagesValidation))]
        [HttpGet("GetMessages")]
        public async Task<IActionResult> GetMessages([FromQuery] SearchMessages query)
        {
            var result = await _messageService.GetMessagesByFiltersAsync(query);
            return result.Any()? Ok(result):NotFound("List of message is empty");
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetSlots")]
        public async Task<IActionResult> GetSlots([FromQuery] SearchSlots query)
        {
            var result = await _slotService.GetSlotsInfoAsync(query);
            return result.Any() ? Ok(result) : NotFound("List of slots is empty");
        }
    }
}
