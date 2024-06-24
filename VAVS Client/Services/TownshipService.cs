using Microsoft.AspNetCore.Mvc.Rendering;

namespace VAVS_Client.Services
{
    public interface TownshipService
    {
        List<Township> GetTownships();
        List<SelectListItem> GetSelectListTownships();
        List<SelectListItem> GetSelectListTownshipsByStateDivision(int stateDivisionPkId = 1);
        List<Township> GetTownshipsByStateDivisionPkId(int stateDivisionPkId);
        Township FindTownshipByPkId(int pkId);
        List<Township> GetTownshipsByStateDivisionCode(string stateDivisionCode);

    }
}
