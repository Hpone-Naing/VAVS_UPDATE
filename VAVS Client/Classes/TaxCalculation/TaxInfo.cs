namespace VAVS_Client.Classes.TaxCalculation
{
    public class TaxInfo
    {
        public int StateDivisionPkid { get; set; }
        public int TownshipPkid { get; set; }
        public string StandardValue { get; set; }
        public string ContractValue { get; set; }
        public string TaxAmount { get; set; }
    }
}
