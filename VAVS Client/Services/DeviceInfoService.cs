using VAVS_Client.Classes;
using DeviceInfo = VAVS_Client.Classes.DeviceInfo;

namespace VAVS_Client.Services
{
    public interface DeviceInfoService
    {
        bool CreateDeviceInfo(DeviceInfo deviceInfo);
        DeviceInfo GetDeviceInfoByIPAddress(string ipAddress);
        public void UpdateOtp(string ipAddress, string hashedOtp = null);
        void UpdateRegistrationTime(string ipAddress);
        void UpdateResendCodeTime(string ipAddress, string hashedOtp = null);
        public Task<string> GetPublicIPAddress();
        public Task<bool> VpnTurnOn();
    }
}
