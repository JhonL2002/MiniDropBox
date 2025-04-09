using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;

namespace MiniDropBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> CreateFolder([FromBody] FolderDTO folderDTO)
        {
            if (folderDTO == null || string.IsNullOrWhiteSpace(folderDTO.Name))
            {
                return BadRequest("Folder name cannot be empty");
            }

            var folder = await _folderService.CreateFolderAsync(folderDTO);

            return Ok(folder);
        }

        [HttpGet("tree")]
        [Authorize]
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

        [HttpDelete("{folderId}")]
        public async Task<IActionResult> DeleteFolder(int folderId)
        {
            var result = await _folderService.DeleteFolderAsync(folderId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
