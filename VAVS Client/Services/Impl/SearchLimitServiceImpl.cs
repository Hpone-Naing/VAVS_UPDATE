using System.Net;
using VAVS_Client.Data;
using VAVS_Client.Models;
using VAVS_Client.Util;

namespace VAVS_Client.Services.Impl
{
    public class SearchLimitServiceImpl : AbstractServiceImpl<SearchLimit>, SearchLimitService
    {
        private readonly ILogger<SearchLimitServiceImpl> _logger;

        public SearchLimitServiceImpl(VAVSClientDBContext context, ILogger<SearchLimitServiceImpl> logger) : base(context, logger)
        {
            _logger = logger;
        }

        public bool CreateSearchLimit(SearchLimit searchLimit)
        {
            try
            {
                return Create(searchLimit);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occur while save" + ex);
                throw;
            }
        }

        public SearchLimit GetSearchLimitByNrc(string nrc)
        {
            return FindByString("Nrc", nrc);
        }
        public bool HardDeleteSearchLimit(string nrc)
        {
            try
            {
                SearchLimit searchlimit = GetSearchLimitByNrc(nrc);
                if(searchlimit != null)
                {
                    return HardDelete(searchlimit);
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occur while dekete" + ex);
                throw;
            }
        }

        public bool UpdateSearchLimit(string nrc)
        {
            try
            {
                SearchLimit searchLimit = GetSearchLimitByNrc(nrc);
                if (searchLimit.IsExceedMaximunSearch())
                { 
                    if(searchLimit.ReSearchTime != null && searchLimit.AllowNextTimeRegiste())
                    {
                        return HardDelete(searchLimit);
                        
                    }
                    searchLimit.ReSearchTime = DateTime.Now.AddMinutes(Utility.NEXT_SEARCH_TIME_IN_MINUTE).ToString();
                    return Update(searchLimit);
                }
                searchLimit.SearchCount++;
                return Update(searchLimit);                
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occur while update" + ex);
                throw;
            }
        }


    }
}
