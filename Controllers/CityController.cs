using LocMvc.Helper;
using LocMvc.Models;
using LocMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace LocMvc.Controllers
{
    public class CityController : Controller
    {
        #region HttpClient
        Uri baseAddress = new Uri("https://localhost:7194/api");

        private readonly HttpClient _client;
        public CityController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;

        }
        #endregion

        #region CityView
        public IActionResult CityView(int? StateId)
        {
            List<CityModel> cityModel = new List<CityModel>();
            if (StateId.HasValue)
            {
                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/City/GetCityByStateId/{StateId}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    cityModel = JsonConvert.DeserializeObject<List<CityModel>>(data);
                }
                return View("CityView", cityModel);
            }
            HttpResponseMessage response1 = _client.GetAsync($"{_client.BaseAddress}/City/GetAllCities").Result;

            if (response1.IsSuccessStatusCode)
            {
                string data = response1.Content.ReadAsStringAsync().Result;
                cityModel = JsonConvert.DeserializeObject<List<CityModel>>(data);
            }
            return View("CityView", cityModel);
        }
        #endregion

        #region AddorEditCity
        public async  Task<IActionResult> AddorEditCity(int? CityId)
        {
            await GetCountryList();
            if (CityId.HasValue)
            {
                HttpResponseMessage response = await _client.GetAsync($"{_client.BaseAddress}/City/GetCityById/{CityId}");
                if (response.IsSuccessStatusCode)
                {
                    var data= response.Content.ReadAsStringAsync().Result;
                    var city=JsonConvert.DeserializeObject<CityModel>(data);
                    
                    ViewBag.StateList = await GetStateList(city.CountryId);
                    return View("CityForm",city);

                }
            }

            return View("CityForm",new CityModel());
        }
        #endregion

        #region Save
        [HttpPost]

        public async Task<IActionResult> Save(CityModel city)
        {
            if(ModelState.IsValid)
            {
                var json= JsonConvert.SerializeObject(city);
                var content=new StringContent(json,Encoding.UTF8,"application/json");
                HttpResponseMessage response;
                if (city.CityID == null)
                {
                    response = await _client.PostAsync($"{_client.BaseAddress}/City/InsertCity",content);
                }
                else{
                    response = await _client.PutAsync($"{_client.BaseAddress}/City/UpdateCity/{city.CityID}", content);
                }
                if(response.IsSuccessStatusCode)
                {
                    return RedirectToAction("CityView");
                } 

            }
            await GetCountryList();
            return View("CityForm", city);
        }

        #endregion

        #region DeleteCity
        public async Task<IActionResult> DeleteCity(int CityID)
        {
            //int decryptedCityID = Convert.ToInt32(UrlEncryptor.Decrypt(CityID.ToString()));

            var response = await _client.DeleteAsync($"{_client.BaseAddress}/City/DeleteCityById/{CityID}");
            return RedirectToAction("CityView");
        }

        #endregion

        #region getCountryList
        public async Task GetCountryList()
        {
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/State/GetCountry").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var countries = JsonConvert.DeserializeObject<List<CountryList>>(data);


                ViewBag.CountryList = countries;
                ViewBag.StateList = new List<StateList>();
            }
        }


        #endregion

        #region GetStateListJson
        [HttpPost]

        public async Task<JsonResult> GetStateListJson(int CountryId)
        {
            var states = await GetStateList(CountryId);
            return Json(states);

        }

        #endregion

        #region GetStateList

        public async Task<List<StateList>> GetStateList (int CountryId)
        {
            var response = await _client.GetAsync($"{_client.BaseAddress}/City/GetStates/{CountryId}");
            if (response.IsSuccessStatusCode)
            {
                var data= await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<StateList>>(data);
            }
            return new List<StateList>();

        }



        #endregion
    }
}
