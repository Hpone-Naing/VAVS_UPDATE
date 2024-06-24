using VAVS_Client.Data;
using VAVS_Client.Util;

namespace VAVS_Client.Services.Impl
{
    public class NRCANDTownshipServiceImpl : AbstractServiceImpl<NRC_And_Township>, NRCANDTownshipService
    {
        private readonly ILogger<NRCANDTownshipServiceImpl> _logger;

        public NRCANDTownshipServiceImpl(VAVSClientDBContext context, ILogger<NRCANDTownshipServiceImpl> logger) : base(context, logger)
        {
            _logger = logger;
        }

        public NRC_And_Township GetNrcAndTownship(string nrcInitialCodeInMyanmar)
        {
            return _context.NRC_And_Townships.FirstOrDefault(nrcTownship => nrcTownship.NrcInitialCodeMyanmar == nrcInitialCodeInMyanmar);
        }

        private string makeNrcTownshipCode(string nrcTownshipCode)
        {
            string convertEngValue = Utility.ConvertToEng(nrcTownshipCode);
            return string.Concat((convertEngValue.Length == 1) ? "0" + convertEngValue : convertEngValue);
        }

        private string makeNrcType(string nrcType)
        {
            if (nrcType == "(နိုင်)")
                return "1";
            if(nrcType == "(ဧည့်)")
                return "2";
            if (nrcType == "(ပြု)")
                return "3";
            if (nrcType == "(ဝတ်)")
                return "4";
            return "1";
        }
        public string MakeGIR(string nrcTownshipCode, string nrcInitialCodeInMyanmar, string nrcType, string nrcNumber)
        {
            Console.WriteLine("gir values.................." + nrcTownshipCode + " / " + nrcInitialCodeInMyanmar + " / " + nrcType + " / " + nrcNumber);
            return string.Concat(makeNrcTownshipCode(nrcTownshipCode), GetNrcAndTownship(nrcTownshipCode).TownshipDigitCode, makeNrcType(nrcType), nrcNumber, "/စကလက");
        }
    }
}
