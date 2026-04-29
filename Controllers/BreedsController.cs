using Microsoft.AspNetCore.Mvc;
using DogBreedApi.Services;

namespace DogBreedApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BreedsController : ControllerBase
    {
        private readonly DogApiService _dogService;

        public BreedsController(DogApiService dogService)
        {
            _dogService = dogService;
        }

        // GET /api/breeds
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var breeds = await _dogService.GetAllBreedsAsync();
            return Ok(breeds);
        }

        // GET /api/breeds/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var breed = await _dogService.GetBreedByIdAsync(id);
            return breed == null ? NotFound($"Breed with id {id} not found.") : Ok(breed);
        }

        // GET /api/breeds/search?q=labrador
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Search query 'q' is required.");

            var results = await _dogService.SearchBreedsAsync(q);
            return Ok(results);
        }

        // GET /api/breeds/groups
        [HttpGet("groups")]
        public async Task<IActionResult> GetGroups()
        {
            var groups = await _dogService.GetBreedGroupsAsync();
            return Ok(groups);
        }

        // GET /api/breeds/group/Herding
        [HttpGet("group/{group}")]
        public async Task<IActionResult> GetByGroup(string group)
        {
            var breeds = await _dogService.GetBreedsByGroupAsync(group);
            return Ok(breeds);
        }
    }
}
