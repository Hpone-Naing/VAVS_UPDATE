namespace VAVS_Client.Services
{
    public interface SearchLimitService
    {
        public bool CreateSearchLimit(SearchLimit searchLimit);
        public SearchLimit GetSearchLimitByNrc(string nrc);
        public bool HardDeleteSearchLimit(SearchLimit searchLimit);
        public bool UpdateSearchLimit(string nrc);
    }
}
