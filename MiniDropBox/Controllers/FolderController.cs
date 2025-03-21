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

        public FolderController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFolder([FromBody] FolderDTO folderDTO)
        {
            if (folderDTO == null || string.IsNullOrWhiteSpace(folderDTO.Name))
            {
                return BadRequest("Folder name cannot be empty");
            }
            var folder = await _folderService.CreateFolderAsync(folderDTO);
            return CreatedAtAction(nameof(GetFolderById), new { folderId = folder.Id }, folder);
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

        [HttpGet]
        public async Task<IActionResult> GetAllFolders()
        {
            var result = await _folderService.GetAllFoldersAsync();
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
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
