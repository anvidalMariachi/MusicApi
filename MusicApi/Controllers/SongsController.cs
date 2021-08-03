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
    public class SongsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public SongsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Song song)
        {
            var ImageUrl = await FileHelper.UploadImage(song.Image);
            var AudioUrl = await FileHelper.UploadAudio(song.AudioFile);
            song.ImageUrl = ImageUrl;
            song.AudioUrl = AudioUrl;
            song.UploadedDate = DateTime.Now;
            await _dbContext.Songs.AddAsync(song);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSongs(int? pageNumber, int? pageSize)
        {
            int defaultPageNumber = pageNumber ?? 0;
            int defaultPageSize = pageSize ?? 5;
            var songs = await _dbContext.Songs.Select(x =>
            new
            {
                Id = x.Id,
                Title = x.Title,
                Duration = x.Duration,
                ImageUrl = x.ImageUrl,
                AudioUrl = x.AudioUrl
            }).ToListAsync();
            return Ok(songs.Skip((defaultPageNumber-1)* defaultPageSize).Take(defaultPageSize));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> FeaturedSongs()
        {
            var artist = await _dbContext.Songs.Where(s => s.IsFeatured == true).Select(x =>
               new
               {
                   Id = x.Id,
                   Title = x.Title,
                   Duration = x.Duration,
                   ImageUrl = x.ImageUrl,
                   AudioUrl = x.AudioUrl
               }).ToListAsync();
            return Ok(artist);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> NewSongs()
        {
            var artist = await _dbContext.Songs.OrderByDescending(s=>s.UploadedDate).Select(x =>
               new
               {
                   Id = x.Id,
                   Title = x.Title,
                   Duration = x.Duration,
                   ImageUrl = x.ImageUrl,
                   AudioUrl = x.AudioUrl
               }).Take(10).ToListAsync();
            return Ok(artist);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SearchSongs(string query)
        {
            var artist = await _dbContext.Songs.Where(s=>s.Title.StartsWith(query)).Select(x =>
                 new
                 {
                     Id = x.Id,
                     Title = x.Title,
                     Duration = x.Duration,
                     ImageUrl = x.ImageUrl,
                     AudioUrl = x.AudioUrl
                 }).Take(10).ToListAsync();
            return Ok(artist);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ArtistDetails(int id)
        {
            var artist = await _dbContext.Songs.Where(a => a.Id == id).ToListAsync();
            return Ok(artist);
        }
    }
}
