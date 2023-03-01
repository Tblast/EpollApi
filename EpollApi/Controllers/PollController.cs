using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EpollApi.Models;
using Microsoft.Extensions.Options;
using EpollApi.Services;

namespace EpollApi.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class PollController : Controller
    {
        private readonly MongoDBService _mongoDBService;

        public PollController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }
        

        [HttpGet]
        public async Task<List<Poll>> Get()
        {
            return await _mongoDBService.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPoll(string id)
        {
            var poll = await _mongoDBService.GetByIdAsync(id);

            if (poll == null)
            {
                return NotFound();
            }

            return Ok(poll);
        }

        [HttpPost("{id}/vote/{option}")]
        public async Task<IActionResult> Vote(string id, int option)
            {
                var poll = await _mongoDBService.GetByIdAsync(id);
                if (poll == null)
                {
                    return NotFound();
                }

                var pollOption = poll.Options.FirstOrDefault(o => o.Id == option);
                if (pollOption == null)
                {
                    return NotFound();
                }

                await _mongoDBService.VoteOptionAsync(id, option);

                return Ok(poll);
            }


        [HttpPost("add")]
        public async Task<IActionResult> AddPoll([FromBody] Poll poll)
        {
            await _mongoDBService.CreateAsync(poll);
            return CreatedAtAction(nameof(Get), new { id = poll.Id }, poll);
        }
    };
}

