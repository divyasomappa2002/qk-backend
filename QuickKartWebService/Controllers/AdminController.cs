using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuickKartWebService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : Controller
    {

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var fileName = file.FileName;
                string url = "";
                if (file.Length > 0)
                {
                    string connectionString = "DefaultEndpointsProtocol=https;AccountName=qkstrgacct;AccountKey=As7Isw07/iS6YXI5nBRKKRrU2qKAwb+rSkPfMZKs9u24h8W4Du2HqIGWWG+riqMTWCdfSB0yIFqm+AStEZWhCA==;EndpointSuffix=core.windows.net";
                    string containerName = "data";
                    BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

                    try
                    {
                        // Get a reference to a blob
                        BlobClient blob = container.GetBlobClient(fileName);

                        // Open the file and upload its data
                        using (Stream file1 = file.OpenReadStream())
                        {
                            blob.Upload(file1);
                        }

                         url = blob.Uri.AbsoluteUri;
                    }
                    catch
                    {
                        return BadRequest();
                    }

                    return Ok(url);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }





    }
}
