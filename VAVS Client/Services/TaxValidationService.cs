using System.Data;
using VAVS_Client.Paging;

namespace VAVS_Client.Services
{
    public interface TaxValidationService
    {
        bool IsTaxedVehicle(string vehicleNumber);
        TaxValidation FindTaxValidationByNrc(string nrc);
        public TaxValidation FindTaxValidationByIdEgerLoad(int id);
        PagingList<TaxValidation> GetTaxValidationPendigListPagin(HttpContext httpContext, int? pageNo, int PageSize);
        PagingList<TaxValidation> GetTaxValidationPendigListForExcelPagin(HttpContext httpContext, int? pageNo, int PageSize);

        PagingList<TaxValidation> GetTaxValidationApprevedListPagin(HttpContext httpContext, int? pageNo, int PageSize);
        PagingList<TaxValidation> GetTaxValidationApprevedListForExcelPagin(HttpContext httpContext, int? pageNo, int PageSize);


        DataTable MakeVehicleDataExcelData(PagingList<TaxValidation> taxValidations, HttpContext httpContext);
    }
}
