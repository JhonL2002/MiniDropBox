using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Application.Interfaces.FileServices;
using System.Security.Claims;

namespace MiniDropBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFolderService _folderService;
        private readonly IFileStorageService<IFormFile> _fileStorageService;
        private readonly IFileService _fileService;

        public FileController(IFolderService folderService, IFileStorageService<IFormFile> fileStorageService, IFileService fileService)
        {
            _folderService = folderService;
            _fileStorageService = fileStorageService;
            _fileService = fileService;
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileDTO<IFormFile> uploadFileDTO)
        {
            var folder = await _folderService.GetFolderByNameAsync(uploadFileDTO.FolderPath);

            if (folder == null)
            {
                return NotFound();
            }

            string filePath = await _fileStorageService.UploadFileAsync(uploadFileDTO);

            var newFileDTO = new FileDTO(
                uploadFileDTO.File.FileName,
                uploadFileDTO.File.Length,
                Path.GetExtension(uploadFileDTO.File.FileName),
                filePath,
                int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
                folder.Id
            );

            var file = await _fileService.UploadFileAsync(newFileDTO);
            return Ok(file);
        }
    }
}
