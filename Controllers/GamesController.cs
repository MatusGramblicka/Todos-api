using GamesApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GamesApi.Services;

namespace GamesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameService _gameService;

        public GamesController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<IEnumerable<Game>> GetAllAsync([FromQuery] string category = null)
        {
            return await _gameService.GetAll(category);
           
        }

        [HttpGet("{id:length(24)}", Name = "GetGame")]
        public async Task<ActionResult<Game>> GetAsync(string id)
        {
            var game = await _gameService.Get(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }
                
        [HttpGet]
        [Route("categories")]
        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            var cats = await _gameService.GetAll();

            return cats.Select(cat => cat.Category).Distinct();
        }

        [HttpPost]
        public async Task<ActionResult<Game>> CreateAsync([FromBody] Game game)
        {
            await _gameService.Create(game);

            return CreatedAtRoute("GetGame", new { id = game.Id.ToString() }, game);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, [FromBody] Game gameIn)
        {
            var game = _gameService.Get(id);            

            if (game == null)
            {
                return NotFound();
            }

            gameIn.CreatedOn = game.Result.CreatedOn;

            _gameService.Update(id, gameIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var game = await _gameService.Get(id);

            if (game == null)
            {
                return NotFound();
            }

            await _gameService.Remove(game.Id);

            return NoContent();
        }
    }
}
