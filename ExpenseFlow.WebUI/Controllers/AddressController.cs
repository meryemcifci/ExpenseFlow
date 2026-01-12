using ExpenseFlow.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
[Route("api/address")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;
    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }
    [HttpGet("countries")]
    public async Task<IActionResult> GetCountries()
    {
        try
        {
            var countries = await _addressService.GetCountriesAsync();
            return Ok(countries); // Zaten List<string> döndürüyor
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("cities")]
    public async Task<IActionResult> GetCities(string country)
    {
        try
        {
            if (string.IsNullOrEmpty(country))
                return BadRequest(new { error = "Country parameter is required" });

            var cities = await _addressService.GetCitiesByCountryAsync(country);
            return Ok(cities);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("districts")]
    public async Task<IActionResult> GetDistricts(string country, string city)
    {
        try
        {
            if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(city))
                return BadRequest(new { error = "Country and city parameters are required" });

            var districts = await _addressService.GetDistrictsAsync(country, city);
            return Ok(districts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}