using Microsoft.AspNetCore.Http;

namespace Service.Contracts
{
    public interface IFileService
    {
        string SaveFile(IFormFile file);
        string SaveImageOptimized(IFormFile file);
    }
}