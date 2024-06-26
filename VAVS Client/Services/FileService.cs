namespace VAVS_Client.Services
{
    public interface FileService
    {
        String GetFileName(IFormFile ImageFile);
        String GetFileExtension(IFormFile ImageFile);
        void SaveFile(string subDirectoryName, string? vehicleNumber, List<(string fileName, IFormFile file)> files, bool IsTaxedVehicle = true);
        bool ContainImageInPath(string directoryName, string fileName);
        void DeleteFile(string directoryName, string fileName);
        void DeleteDirectory(string subDirectoryName);

    }
}
