using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;

namespace MiniDropBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService _folderService;
        private readonly ICurrentUserService _currentUser;

        public FolderController(IFolderService folderService, ICurrentUserService currentUser)
        {
            _folderService = folderService;
            _currentUser = currentUser;
        }

        [HttpPost]
        [ProducesResponseType(typeof(FolderDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        public async Task<IActionResult> CreateFolder([FromBody] FolderDTO folderDTO)
        {
            if (!int.TryParse(_currentUser.UserId, out var userId))
            {
                return Unauthorized();
            }

            if (folderDTO == null || string.IsNullOrWhiteSpace(folderDTO.Name))
            {
                return BadRequest("Folder name cannot be empty");
            }

            var result = await _folderService.CreateFolderAsync(folderDTO, userId);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("tree")]
        public async Task<IActionResult> GetFolderTree()
        {
            if (!int.TryParse(_currentUser.UserId, out var userId))
            {
                return Unauthorized();
            }

            var tree = await _folderService.GetTreeForUserAsync(userId);
            return Ok(tree);
        }

        [HttpGet("{folderId}")]
        public async Task<IActionResult> GetFolderById(int folderId)
        {
            var folder = await _folderService.GetFolderByIdAsync(folderId);
            if (folder == null)
            {
                return NotFound();
            }
            return Ok(folder);
        }

        [HttpPut("move")]
        public async Task<IActionResult> MoveFolder([FromBody] MoveFolderDTO moveFolderDTO)
        {
            // Validate current user
            if (!int.TryParse(_currentUser.UserId, out var userId))
                return Unauthorized();

            var result = await _folderService.MoveFolderAsync(moveFolderDTO, userId);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpDelete("{folderId}")]
        public async Task<IActionResult> DeleteFolder(int folderId, bool deleteContents = false)
        {
            if (!int.TryParse(_currentUser.UserId, out var userId))
                return Unauthorized();

            var result = await _folderService.DeleteFolderAsync(folderId, userId, deleteContents);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
    }
}
