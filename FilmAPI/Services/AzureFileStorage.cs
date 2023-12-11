using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace FilmAPI.Services
{
    public class AzureFileStorage : IFileStorage
    {
        private readonly string connectionString;
        public AzureFileStorage(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage");
        }
        public async Task DeleteFile(string route, string container)
        {
            if (string.IsNullOrEmpty(route))
            {
                return;
            }
            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            var file = Path.GetFileName(route);
            var blob = client.GetBlobClient(file);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> EditFile(byte[] content, string length, string container, string route, string contentType)
        {
            await DeleteFile(route, container);
            return await SaveFile(content, length, container, contentType);
        }

        public async Task<string> SaveFile(byte[] content, string length, string container, string contentType)
        {
            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(PublicAccessType.Blob);

            var fileName = $"{Guid.NewGuid()}-{length}";//To generate a random name
            var blob = client.GetBlobClient(fileName);

            var blobUploadOptions = new BlobUploadOptions();
            var blobHttpHeader = new BlobHttpHeaders();
            blobHttpHeader.ContentType = contentType;
            blobUploadOptions.HttpHeaders = blobHttpHeader;

            await blob.UploadAsync(new BinaryData(content), blobUploadOptions); //Pushing file to Azure
            return blob.Uri.ToString();//Here return a URL to save in a database
        }
    }
}
