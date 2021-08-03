using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using MusicApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public ArtistsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Artist artist)
        {
            var ImageUrl = await FileHelper.UploadImage(artist.Image);
            artist.ImageUrl = ImageUrl;
            await _dbContext.Artists.AddAsync(artist);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetArtist(int? pageNumber, int? pageSize)
        {
            var realPageNumber = pageNumber ?? 0;
            var realPageSize = pageSize ?? 5;
            var artist = await _dbContext.Artists.Select(x =>
            new
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl
            }).ToListAsync();
            return Ok(artist.Skip((realPageNumber - 1) * realPageSize).Take(realPageSize));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ArtistDetails(int id)
        {
            var artist = await _dbContext.Artists.Where(a => a.Id == id).Include(a => a.Songs).ToListAsync();
            return Ok(artist);
        }
    }
}
