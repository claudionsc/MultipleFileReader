using MultipleFileReader.Interface;

namespace MultipleFileReader.Services
{
    public class FileService : IFileServices
    {
        public IAsyncEnumerable<FileModel> ReadFileAsync(List<IFormFile> files)
        {
            throw new NotImplementedException();
        }
    }
}
