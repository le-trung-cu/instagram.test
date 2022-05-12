using Microsoft.AspNetCore.Http;
using PhotoSauce.MagicScaler;
using Service.Contracts;

namespace Service
{
    public class FileService : IFileService
    {
        //  480, 640, 750, 828, 1080
        private readonly List<int> _imgSizes = new List<int>() { 240, 320};
        private readonly string _pathDirectory;
        public FileService(string pathDirectory)
        {
            _pathDirectory = pathDirectory;
        }

        public string SaveFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var name = RandomFileName();
            var savePath = Path.Combine(_pathDirectory, name);

            using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
            file.CopyTo(fileStream);
            return name;
        }

        public string SaveImageOptimized(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var name = RandomFileName();
            var savePath = Path.Combine(_pathDirectory, name);

            var setings = new ProcessImageSettings
            {
                ResizeMode = CropScaleMode.Crop,
                SaveFormat = FileFormat.Jpeg,
                JpegQuality = 100,
                JpegSubsampleMode = ChromaSubsampleMode.Subsample420,
            };

            foreach (var width in _imgSizes)
            {
                int height = (int)(width * 1.25);
                setings.Width = width;
                setings.Height = height;
                using var fileStream = new FileStream($"{savePath}_({width}x{height}){extension}", FileMode.Create);
                MagicImageProcessor.ProcessImage(file.OpenReadStream(), fileStream, setings);
            }

            return name + extension;
        }

        private string RandomFileName() =>
            $"{Path.GetRandomFileName()}_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}";

    }
}
