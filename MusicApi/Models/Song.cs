using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApi.Models
{
    public class Song
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title cannot be null nor empty")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Language cannot be null nor empty")]
        public string Duration { get; set; }
        public DateTime UploadedDate { get; set; }
        public bool IsFeatured { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile AudioFile { get; set; }
        public string AudioUrl { get; set; }
        public int ArtistId { get; set; }
        public int? AlbumId { get; set; }
    }
}
