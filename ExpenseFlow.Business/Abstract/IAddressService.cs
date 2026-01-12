using ExpenseFlow.Core.ResultModels;

namespace ExpenseFlow.Business.Abstract
{
    public interface IAddressService
    {
        Task<List<string>> GetCountriesAsync();
        Task<List<string>> GetCitiesByCountryAsync(string country);
        Task<List<string>> GetDistrictsAsync(string country, string city);
    }


}
