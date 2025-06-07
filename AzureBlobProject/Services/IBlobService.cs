using AzureBlobProject.Models;

namespace AzureBlobProject.Services;

public interface IBlobService
{
    Task<string> GetBlob(string name, string containerName);
    Task<List<string>> GetAllBlobs(string containerName);
    Task<bool> UploadBlob(string name, IFormFile file, string containerName, BlobModel blob);
    Task<bool> DeleteBlob(string name, string containerName);
    Task<List<BlobModel>> GetAllBlobsWithUri(string containerName);
}
