namespace MultipleFileReader.Interface
{
    public interface IFileServices
    {
        IEnumerable<FileModel> ReadFilesAConcurrent(List<IFormFile> files);
        IEnumerable<FileModel> ReadFile(List<IFormFile> files);
    }
}
