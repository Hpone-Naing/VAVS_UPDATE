using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VAVS_Client.Classes;
using VAVS_Client.Factories;
using VAVS_Client.Util;

namespace VAVS_Client.Controllers.PersonalDetailController
{
    public class PersonalDetailController : Controller
    {
        public ServiceFactory _serviceFactory;

        public PersonalDetailController(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        private void MakeViewBag()
        {
            ViewBag.StateDivisions = _serviceFactory.CreateStateDivisionService().GetSelectListStateDivisions();
            ViewBag.Townships = _serviceFactory.CreateTownshipService().GetSelectListTownships();
        }

        public JsonResult GetTownships(string stateDivisionCode)
        {
            List<Township> townships = _serviceFactory.CreateTownshipService().GetTownshipsByStateDivisionCode(stateDivisionCode);
            return Json(townships);
        }
    }
}
