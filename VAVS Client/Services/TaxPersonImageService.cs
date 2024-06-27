namespace VAVS_Client.Services
{
    public interface TaxPersonImageService
    {
        public bool SaveTaxPersonImage(TaxPersonImage taxPesonImage);
        public bool UpdateTaxPersonImage(TaxPersonImage taxPesonImage);
        public TaxPersonImage GetTaxPersonImageByCarNumber(string carNumber);
        public TaxPersonImage GetTaxPersonImageByPersonalDetailPkId(int personalDetailPkId);
        public TaxPersonImage GetTaxPersonImageByPersonalDetailPkIdAndCarNumber(int personalDetailPkId, string carNumber);
    }
}
