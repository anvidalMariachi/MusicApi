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
    public class AlbumsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public AlbumsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Album album)
        {
            var ImageUrl = await FileHelper.UploadImage(album.Image);
            album.ImageUrl = ImageUrl;
            await _dbContext.Albums.AddAsync(album);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAlbums(int? pageNumber, int? pageSize)
        {
            var realPageNumber = pageNumber ?? 0;
            var realPageSize = pageSize ?? 5;
            var albums = await _dbContext.Albums.Select(e => new
            {
                Id = e.Id,
                Name = e.Name,
                ImageUrl = e.ImageUrl
            }).ToListAsync();
            return Ok(albums.Skip((realPageNumber-1)*realPageSize).Take(realPageSize));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> AlbumDetails(int albumId)
        {
            var album = await _dbContext.Albums.Where(e => e.Id == albumId).Include(a => a.Songs).ToListAsync();
            return Ok(album);
        }
    }
}
