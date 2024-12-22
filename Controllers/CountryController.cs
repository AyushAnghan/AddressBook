using LocMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using LocMvc.Helper;

namespace LocMvc.Controllers
{
    public class CountryController : Controller
    {

        #region HttpClient
        Uri baseAddress = new Uri("https://localhost:7194/api");

        private readonly HttpClient _client;

        public CountryController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            
        }

        #endregion

        #region SelectAllCountry
        public IActionResult SelectAllCountry()
        {
            List<CountryModel> countryModels = new List<CountryModel>();
            HttpResponseMessage response =_client.GetAsync($"{_client.BaseAddress}/Country/GetAllCountry").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                countryModels = JsonConvert.DeserializeObject<List<CountryModel>>(data);
            }
            return View("CountryView",countryModels);
        }
        #endregion

        #region  AddOrEditCountry
        public async  Task<IActionResult> AddOrEditCountry(int? CountryId)
        {

            //int? decryptedCityID = null;
            //if (!string.IsNullOrEmpty(CountryId))
            //{
            //    string decryptedCityIDString = UrlEncryptor.Decrypt(CountryId); // Decrypt the encrypted CityID
            //    decryptedCityID = int.Parse(decryptedCityIDString); // Convert decrypted string to integer
            //}

            if (CountryId.HasValue)
            {
                HttpResponseMessage response = await _client.GetAsync($"{_client.BaseAddress}/Country/GetCountryById/{CountryId}");
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var state=JsonConvert.DeserializeObject<CountryModel>(data);
                    return View("CountryForm",state);
                }

            }
            return View("CountryForm");
        }

        #endregion

        #region Save

        [HttpPost]

        public async Task<IActionResult> Save(CountryModel country)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(country);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;
                if (country.countryId == null)
                {
                    response = await _client.PostAsync($"{_client.BaseAddress}/Country/InsertCountry", content);

                }
                else
                {
                    response =await _client.PutAsync($"{_client.BaseAddress}/Country/UpdateCountry/{country.countryId}",content);
                }
                if(response.IsSuccessStatusCode)
                {
                    return RedirectToAction("SelectAllCountry");
                }

            }
            return View("CountryForm",country);
        }

        #endregion

        #region DeleteCountry

        public async Task<IActionResult> DeleteCountry(int CountryId)
        {
            //int decryptedCityID = Convert.ToInt32(UrlEncryptor.Decrypt(CountryId.ToString()));
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/Country/DeleteCountryById/{CountryId}");
            return RedirectToAction("SelectAllCountry");
        }

        #endregion
    }
}
