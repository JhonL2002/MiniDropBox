using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniDropBox.API.ApiDTOs;
using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Application.Interfaces.FileServices;
using MiniDropBox.Infraestructure.Adapters;
using System.Security.Claims;

namespace MiniDropBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ICurrentUserService _currentUser;

        public FileController(IFileService fileService, ICurrentUserService currentUser)
        {
            _fileService = fileService;
            _currentUser = currentUser;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileFormDTO formData)
        {
            if (!int.TryParse(_currentUser.UserId, out var userId))
            {
                return Unauthorized();
            }

            var uploadDTO = new UploadFileDTO<IFileUpload>(
                new FormFileAdapter(formData.File),
                formData.FolderId,
                ""
            );

            var result = await _fileService.UploadFileAsync(uploadDTO, userId);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpPut("move")]
        public async Task<IActionResult> MoveFile([FromBody] MoveFileDTO moveFileDTO)
        {
            if (!int.TryParse(_currentUser.UserId, out var userId))
                return Unauthorized();

            var result = await _fileService.MoveFileAsync(moveFileDTO, userId);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpGet("download/{fileId}")]
        public async Task<IActionResult> DownloadFile(int fileId)
        {
            if (!int.TryParse(_currentUser.UserId, out var userId))
                return Unauthorized();

            var result = await _fileService.DownloadStreamAsync(fileId, userId);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            var (stream, fileName) = result.Value;
            return File(stream, "application/octet-stream", fileName);
        }
    }
}
