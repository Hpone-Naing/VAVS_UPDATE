namespace VAVS_Client.Services
{
    public interface NRCANDTownshipService
    {
        public string MakeGIR(string nrcTownshipCode, string nrcInitialCodeInMyanmar, string nrcType, string nrcNumber);
    }
}
