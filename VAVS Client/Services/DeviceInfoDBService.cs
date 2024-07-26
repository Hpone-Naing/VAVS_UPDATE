namespace VAVS_Client.Services
{
    public interface DeviceInfoDBService
    {
        public bool CreateDeviceInfo(DeviceInfo deviceInfo);
        public DeviceInfo GetDeviceInfoByIPAddress(string ipAddress);
        public void UpdateRegistrationTime(string ipAddress);
        public void UpdateOtp(string ipAddress, string hashedOtp = null);
        public void UpdateResendCodeTime(string ipAddress, string hashedOtp = null);
        public bool HardDeleteDeviceInfo(DeviceInfo deviceInfo);
        public Task<string> GetPublicIPAddress();
        public Task<bool> VpnTurnOn();
    }
}
