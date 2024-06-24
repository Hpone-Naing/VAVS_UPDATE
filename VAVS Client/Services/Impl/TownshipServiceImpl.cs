using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VAVS_Client.Data;
using VAVS_Client.Models;

namespace VAVS_Client.Services.Impl
{
    public class TownshipServiceImpl : AbstractServiceImpl<Township>, TownshipService
    {
        private readonly ILogger<TownshipServiceImpl> _logger;

        public TownshipServiceImpl(VAVSClientDBContext context, ILogger<TownshipServiceImpl> logger) : base(context, logger)
        {
            _logger = logger;
        }

        public Township FindTownshipByPkId(int pkId)
        {
            _logger.LogInformation(">>>>>>>>>> [TownshipServiceImpl][FindTownshipByPkId]  Find  Township by PkId. <<<<<<<<<<");
            try
            {
                _logger.LogInformation($">>>>>>>>>> Success. Find Township by Pkid. <<<<<<<<<<");
                return FindById(pkId);
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding  township by pkid. <<<<<<<<<<" + e);
                throw;
            }
        }

        public List<SelectListItem> GetSelectListTownships()
        {
            _logger.LogInformation(">>>>>>>>>> [TownshipServiceImpl][GetSelectListTownships]  Get SelectList Townships. <<<<<<<<<<");
            try
            {
                _logger.LogInformation($">>>>>>>>>> Success. Get SelectList Townships. <<<<<<<<<<");
                return GetItemsFromList(GetTownships(), "TownshipPkid", "TownshipName");
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when getting selectList townships. <<<<<<<<<<" + e);
                throw;
            }
        }

        public List<SelectListItem> GetSelectListTownshipsByStateDivision(int stateDivisionPkId = 1)
        {
            _logger.LogInformation(">>>>>>>>>> [TownshipServiceImpl][GetSelectListTownships]  Get SelectList Townships by StateDivision. <<<<<<<<<<");
            try
            {
                _logger.LogInformation($">>>>>>>>>> Success. Get SelectList Townships by StateDivision. <<<<<<<<<<");
                return GetItemsFromList(GetTownshipsByStateDivisionPkId(stateDivisionPkId), "TownshipPkid", "TownshipName");
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when getting selectList townships by StateDivision. <<<<<<<<<<" + e);
                throw;
            }
        }

        public List<Township> GetTownships()
        {
            _logger.LogInformation(">>>>>>>>>> [TownshipServiceImpl][GetTownships]  Get all Townships. <<<<<<<<<<");
            try
            {
                _logger.LogInformation($">>>>>>>>>> Success. Get all Townships. <<<<<<<<<<");
                return GetAll();
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when getting all townships. <<<<<<<<<<" + e);
                throw;
            }
        }

        public List<Township> GetTownshipsByStateDivisionPkId(int stateDivisionPkId)
        {
            _logger.LogInformation(">>>>>>>>>> [TownshipServiceImpl][GetTownshipsByStateDivisionPkId]  Get all Townships by StateDivisionPkId. <<<<<<<<<<");
            try
            {
                _logger.LogInformation($">>>>>>>>>> Success. Get all Townships by StateDivisionPkId. <<<<<<<<<<");
                return GetListByIntVal("StateDivisionPkid", stateDivisionPkId);
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when getting all townships by StateDivisionPkId. <<<<<<<<<<" + e);
                throw;
            }
        }

        public List<Township> GetTownshipsByStateDivisionCode(string stateDivisionCode)
        {
            _logger.LogInformation(">>>>>>>>>> [TownshipServiceImpl][GetTownshipsByStateDivisionPkId]  Get all Townships by StateDivisionPkId. <<<<<<<<<<");
            try
            {
                _logger.LogInformation($">>>>>>>>>> Success. Get all Townships by StateDivisionPkId. <<<<<<<<<<");
                List< Township> Townships = _context.Townships.Where(Township1 => Township1.StateDivisionID == stateDivisionCode).ToList();
                return Townships;
                /*List<string> TownshipNames = new List<string>();
                foreach (Township1 Township in Townships)
                {
                    Console.WriteLine("NrcTownshipCodeMyn............." + Township.TownshipName);
                    TownshipNames.Add(Township.TownshipName);
                }
                return TownshipNames;*/
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when getting all townships by StateDivisionPkId. <<<<<<<<<<" + e);
                throw;
            }
        }

        public Township GetTownshipByName(string townshipName)
        {
            _logger.LogInformation(">>>>>>>>>> [TownshipServiceImpl][GetTownshipsByStateDivisionPkId]  Get all Townships by StateDivisionPkId. <<<<<<<<<<");
            try
            {
                _logger.LogInformation($">>>>>>>>>> Success. Get all Townships by StateDivisionPkId. <<<<<<<<<<");
                Township Township = _context.Townships.FirstOrDefault(Township => Township.TownshipName == townshipName);
                return Township;
                /*List<string> TownshipNames = new List<string>();
                foreach (Township1 Township in Townships)
                {
                    Console.WriteLine("NrcTownshipCodeMyn............." + Township.TownshipName);
                    TownshipNames.Add(Township.TownshipName);
                }
                return TownshipNames;*/
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when getting all townships by StateDivisionPkId. <<<<<<<<<<" + e);
                throw;
            }
        }
    }
}
