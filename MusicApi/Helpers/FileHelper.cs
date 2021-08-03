using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApi.Helpers
{
    public static class FileHelper
    {
        public static async Task<string> UploadImage(IFormFile file)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=musicpitestsacc;AccountKey=k0U25XdSFZZSAx+ryQp7Wy2PJvdVZbC7BAce9Bvny1lnbo6ZzVRpTMgY4jeOeAg9FZo8CUD7ZQEsnWG367RcTw==;EndpointSuffix=core.windows.net";
            string containername = "songscover";

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containername);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
            var memoryStrem = new MemoryStream();
            //await song.Image.CopyToAsync(memoryStrem);
            memoryStrem.Position = 0;
            await blobClient.UploadAsync(memoryStrem);
            return blobClient.Uri.AbsoluteUri; ;
        }

        public static async Task<string> UploadAudio(IFormFile file)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=musicpitestsacc;AccountKey=k0U25XdSFZZSAx+ryQp7Wy2PJvdVZbC7BAce9Bvny1lnbo6ZzVRpTMgY4jeOeAg9FZo8CUD7ZQEsnWG367RcTw==;EndpointSuffix=core.windows.net";
            string containername = "audiofiles";

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containername);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
            var memoryStrem = new MemoryStream();
            //await song.Image.CopyToAsync(memoryStrem);
            memoryStrem.Position = 0;
            await blobClient.UploadAsync(memoryStrem);
            return blobClient.Uri.AbsoluteUri; ;
        }
    }
}
