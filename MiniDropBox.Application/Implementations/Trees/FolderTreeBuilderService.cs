using MiniDropBox.Application.DTOs.Nodes;
using MiniDropBox.Application.Interfaces.Trees;
using MiniDropBox.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniDropBox.Application.Implementations.Trees
{
    public class FolderTreeBuilderService : IFolderTreeBuilderService
    {
        public List<FolderTreeNodeDTO> BuildTree(List<Folder> folders)
        {
            // Convert all folders as flat nodes
            var nodeMap = folders.ToDictionary(
                f => f.Id,
                f => new FolderTreeNodeDTO
                {
                    Id = f.Id,
                    Name = f.Name,
                    ParentFolderId = f.ParentFolderId
                }
            );

            // List for the root nodes
            var rootNodes = new List<FolderTreeNodeDTO>();

            // Build hierarchy
            foreach (var node in nodeMap.Values)
            {
                if (node.ParentFolderId is null)
                {
                    rootNodes.Add(node); // It's a root node
                }
                else if (nodeMap.TryGetValue(node.ParentFolderId.Value, out var parentNode))
                {
                    // It's child from another node
                    parentNode.SubFolders.Add(node);
                }
            }

            return rootNodes;
        }
    }
}
