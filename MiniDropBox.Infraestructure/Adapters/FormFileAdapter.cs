using Microsoft.AspNetCore.Http;
using MiniDropBox.Application.Interfaces.FileServices;

namespace MiniDropBox.Infraestructure.Adapters
{
    public class FormFileAdapter : IFileUpload
    {
        private readonly IFormFile _formFile;

        public FormFileAdapter(IFormFile formFile)
        {
            _formFile = formFile;
        }

        public string FileName => _formFile.FileName;
        public long Lenght => _formFile.Length;

        public Stream OpenReadStream() => _formFile.OpenReadStream();
    }
}
