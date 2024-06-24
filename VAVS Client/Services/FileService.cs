namespace VAVS_Client.Services
{
    public interface FileService
    {
        String GetFileName(IFormFile ImageFile);
        String GetFileExtension(IFormFile ImageFile);
        void SaveFile(string subDirectoryName, List<(string fileName, IFormFile file)> files);
        bool ContainImageInPath(string directoryName, string fileName);
        void DeleteFile(string directoryName, string fileName);
        void DeleteDirectory(string subDirectoryName);

    }
}
