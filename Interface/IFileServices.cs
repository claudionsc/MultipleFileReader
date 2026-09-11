namespace MultipleFileReader.Interface
{
    public interface IFileServices
    {
        public IAsyncEnumerable<FileModel> ReadFileAsync(List<IFormFile> files);
    }
}
