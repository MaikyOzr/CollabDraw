using Microsoft.AspNetCore.Mvc;

namespace RealTimeCollaborativeWhiteboard.Services
{
    public interface IFileStorage
    {
        Task<IActionResult> SaveFile(IFormFile file);
        IActionResult GetFile(string fileName);
        Task<IActionResult> DeleteFile(int id);
    }
}
