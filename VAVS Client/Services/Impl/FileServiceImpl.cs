using System.Text.RegularExpressions;

namespace VAVS_Client.Services.Impl
{
    public class FileServiceImpl : FileService
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public FileServiceImpl(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public String GetFileName(IFormFile ImageFile)
        {
            string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
            return fileName + DateTime.Now.ToString("yymmssfff") + GetFileExtension(ImageFile);
        }

        public String GetFileExtension(IFormFile ImageFile)
        {
            string extension = Path.GetExtension(ImageFile.FileName);
            return extension;
        }

        private string GetPath(string rootPath, string directoryName, string fileName)
        {
            string path = Path.Combine(rootPath + directoryName, fileName);
            return path;
        }

        private string GetCustomSubdirectoryPath(string rootPath, string parentDirectoryName, string subdirectoryName)
        {
            string directoryPath = Path.Combine(rootPath, parentDirectoryName);
            string subdirectoryPath = Path.Combine(directoryPath, subdirectoryName);
            return subdirectoryPath;
        }

        public async void SaveFile(string subDirectoryName, string? vehicleNumber, List<(string fileName, IFormFile file)> files, bool IsTaxedVehicle = true)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string sanitizedSubDirectoryName = subDirectoryName.Replace(";", "_").Replace("/", "");
            string subdirectoryPath = GetCustomSubdirectoryPath(wwwRootPath, "nrc", sanitizedSubDirectoryName);

            if (!string.IsNullOrEmpty(vehicleNumber))
            {
                vehicleNumber = Regex.Replace(vehicleNumber, @"[\/\-\(\)\s]", "_");
                sanitizedSubDirectoryName = Path.Combine(sanitizedSubDirectoryName, vehicleNumber);
                subdirectoryPath = GetCustomSubdirectoryPath(wwwRootPath, "nrc", sanitizedSubDirectoryName);
            }
            else
            {
                sanitizedSubDirectoryName = Path.Combine(sanitizedSubDirectoryName, sanitizedSubDirectoryName);
                subdirectoryPath = GetCustomSubdirectoryPath(wwwRootPath, "nrc", sanitizedSubDirectoryName);
            }
            try
            {
                if (!Directory.Exists(subdirectoryPath))
                {
                    Directory.CreateDirectory(subdirectoryPath);
                }
                else
                {
                    if (IsTaxedVehicle)
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(subdirectoryPath);
                        foreach (FileInfo file in directoryInfo.GetFiles())
                        {
                            file.Delete();
                        }
                    }
                }
                foreach (var (fileName, ImageFile) in files)
                {
                    string filePath = Path.Combine(subdirectoryPath, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                        Console.WriteLine($"File '{fileName}' saved successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while creating the directory: " + ex.Message);
            }
        }

        public bool ContainImageInPath(string directoryName, string fileName)
        {
            var imagePath = GetPath(_hostEnvironment.WebRootPath, directoryName, fileName);
            return System.IO.File.Exists(imagePath);
        }

        private bool isDefaultImage(string fileName)
        {
            return fileName.Split(".")[0] == "default";
        }
        public void DeleteFile(string directoryName, string fileName)
        {
            string defaultFile = fileName.Split(".")[0];
            if (!isDefaultImage(defaultFile))
            {
                var imagePath = GetPath(_hostEnvironment.WebRootPath, directoryName, fileName);
                if (ContainImageInPath(directoryName, fileName))
                    System.IO.File.Delete(imagePath);
            }
        }

        public void DeleteDirectory(string subDirectoryName)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string sanitizedSubDirectoryName = subDirectoryName.Replace(";", "_").Replace("/", "");
            string subdirectoryPath = GetCustomSubdirectoryPath(wwwRootPath, "nrc", sanitizedSubDirectoryName);

            if (Directory.Exists(subdirectoryPath))
            {
                string[] files = Directory.GetFiles(subdirectoryPath);

                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                }

                Directory.Delete(subdirectoryPath);
            }
        }
    }
}
