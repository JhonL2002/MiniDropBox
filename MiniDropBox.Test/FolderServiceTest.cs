using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Implementations;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;
using Moq;

namespace MiniDropBox.Test
{
    public class FolderServiceTest
    {
        private readonly FolderService _folderService;
        private readonly Mock<IFolderRepository> _folderRepositoryMock;

        public FolderServiceTest()
        {
            _folderRepositoryMock = new Mock<IFolderRepository>();
            _folderService = new FolderService(_folderRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateFolderAsync_ShouldReturnFolder()
        {
            // Arrange
            var folderDTO = new FolderDTO(Id: 1, Name: "Test Folder", ParentFolderId: 0, UserId: 1);
            var folder = new Folder
            {
                Id = folderDTO.Id,
                Name = folderDTO.Name,
                ParentFolderId = folderDTO.ParentFolderId,
                UserId = folderDTO.UserId
            };

            _folderRepositoryMock.Setup(repo => 
                repo.AddAsync(It.IsAny<Folder>()))
                .ReturnsAsync(folder);

            // Act
            var result = await _folderService.CreateFolderAsync(folderDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(folder.Id, result.Id);
            Assert.Equal(folder.Name, result.Name);

        }

        [Fact]
        public async Task GetFolderByIdAsync_ShouldReturnFolder()
        {
            // Arrange
            var folderId = 1;
            var folder = new Folder
            {
                Id = folderId,
                Name = "Test Folder",
                ParentFolderId = 0,
                UserId = 1
            };
            _folderRepositoryMock.Setup(repo => repo.GetByIdAsync(folderId))
                .ReturnsAsync(folder);

            // Act
            var result = await _folderService.GetFolderByIdAsync(folderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(folderId, result.Id);
            Assert.Equal("Test Folder", result.Name);
        }

        [Fact]
        public async Task GetFolderByIdAsync_ShouldReturnNull_WhenFolderNotFound()
        {
            // Arrange
            var folderId = 1;

            // Mock for repository (return null if does not find folder)
            _folderRepositoryMock.Setup(repo => repo.GetByIdAsync(folderId))
                .ReturnsAsync((Folder)null!);

            // Act
            var result = await _folderService.GetFolderByIdAsync(folderId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllFoldersAsync_ShouldReturnFolders()
        {
            // Arrange
            var folders = new List<Folder>
            {
                new Folder {Id = 1, Name = "Test Folder 1", ParentFolderId = 0, UserId = 1 },
                new Folder { Id = 2, Name = "Test Folder 2", ParentFolderId = 0, UserId = 1 }
            };
            
            // Mock for repository
            _folderRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(folders);

            // Act
            var result = await _folderService.GetAllFoldersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(folders.Count, result.Count());
        }

        [Fact]
        public async Task UpdateFolderAsync_ShouldReturnFalse_IfFolderDoesNotExist()
        {
            // Arrange
            var folder = new Folder
            {
                Id = 1,
                Name = "Test Folder"
            };

            // Mock for repository (simulate folder is null)
            _folderRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Folder>()))
                .ReturnsAsync((Folder?)null);
            // Act
            var result = await _folderService.UpdateFolderAsync(folder);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateFolderAsync_ShouldReturnTrue_IfFolderExists()
        {
            // Arrange
            var folderId = 1;
            var newName = "Updated Folder Test Name";
            var existingFolder = new Folder
            {
                Id = folderId,
                Name = "Old Folder Name",
                ParentFolderId = 0,
                UserId = 1
            };

            // Simulate repo find existing folder
            _folderRepositoryMock.Setup(repo => repo.GetByIdAsync(folderId))
                .ReturnsAsync(existingFolder);

            // Simulate udpate folder
            var updatedFolder = new Folder
            {
                Id = folderId,
                Name = newName,
                ParentFolderId = 0,
                UserId = 1
            };

            // Simulate repo save updated folder
            _folderRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Folder>()))
                .ReturnsAsync(updatedFolder);

            // Act
            var result = await _folderService.UpdateFolderAsync(updatedFolder);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteFolderAsync_ShouldReturnTrue_WhenFolderDelete()
        {
            // Arrange
            var folderId = 1;
            var folder = new Folder
            {
                Id = folderId,
                Name = "Test Folder",
                ParentFolderId = 0,
                UserId = 1
            };

            // Simulate repo find folder
            _folderRepositoryMock.Setup(repo => repo.GetByIdAsync(folderId))
                .ReturnsAsync(folder);

            // Mock for repository, simulating success delete
            _folderRepositoryMock.Setup(repo => repo.DeleteAsync(folderId))
                .ReturnsAsync(folder);

            // Act
            var result = await _folderService.DeleteFolderAsync(folderId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteFolderAsync_ShouldReturnFalse_WhenFolderNotDeleted()
        {
            // Arrange
            var folderId = 1;

            // Mock for repository (return null if does not find folder)
            _folderRepositoryMock.Setup(repo => repo.DeleteAsync(folderId))
                .ReturnsAsync((Folder?)null);

            // Act
            var result = await _folderService.DeleteFolderAsync(folderId);

            // Assert
            Assert.False(result);
        }
    }
}
