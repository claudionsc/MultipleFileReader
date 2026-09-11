using MultipleFileReader.Interface;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace MultipleFileReader.Services
{
    public class FileService : IFileServices
    {
        public IEnumerable<FileModel> ReadFilesAConcurrent(List<IFormFile> files)
        {
            var bag = new ConcurrentBag<FileModel>();

            Parallel.ForEach(files, file =>
            {
                bag.Add(Process(file));
            });

            return bag.ToList();
        }

        public IEnumerable<FileModel> ReadFile(List<IFormFile> files)
        {
            var models = new List<FileModel>();

            foreach (var file in files)
            {
                models.Add(Process(file));
            }

            return models;
        }

        private static FileModel Process(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var sha256 = SHA256.Create();
            var hash = Convert.ToHexString(sha256.ComputeHash(stream));

            return new FileModel
            {
                Name = file.FileName,
                Size = $"{file.Length / 1024.0:F2} KB",
                Type = file.ContentType,
                Hash = hash,
                Modified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
    }
}