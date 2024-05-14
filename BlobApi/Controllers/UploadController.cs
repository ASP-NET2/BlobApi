using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlobApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _client;
        private readonly BlobContainerClient _container;
        private BlobClient? _blobClient;

        public UploadController(IConfiguration configuration)
        {
            _configuration = configuration;

            _client = new BlobServiceClient(_configuration.GetConnectionString("BlobStorage"));
            _container = _client.GetBlobContainerClient("images");
        }



        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null || file!.Length != 0) {
                _blobClient = _container.GetBlobClient(file.FileName);
                await using var stream = file.OpenReadStream();
                await _blobClient.UploadAsync(stream, overwrite: true);
                return Ok(_blobClient.Uri);
            }
            return BadRequest();
        }
    }
}
