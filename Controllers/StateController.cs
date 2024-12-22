using LocMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace LocMvc.Controllers
{
    public class StateController : Controller
    {

        #region HttpClient
        Uri baseAddress = new Uri("https://localhost:7194/api");

        private readonly HttpClient _client;

        public StateController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;

        }

        #endregion

        #region StateView
        public IActionResult StateView(int? CountryId)

        {
            List<StateModel> state = new List<StateModel>();
            if (CountryId.HasValue)
            {
                HttpResponseMessage response2 = _client.GetAsync($"{_client.BaseAddress}/Country/GetStatebyCountryId/{CountryId}").Result;
                if (response2.IsSuccessStatusCode)
                {   
                    string data = response2.Content.ReadAsStringAsync().Result;
                    state = JsonConvert.DeserializeObject<List<StateModel>>(data);
                }
                return View("StateView", state);

            }
            HttpResponseMessage response1 = _client.GetAsync($"{_client.BaseAddress}/State/Get").Result;
            if (response1.IsSuccessStatusCode)
            {
                string data = response1.Content.ReadAsStringAsync().Result;
                state = JsonConvert.DeserializeObject<List<StateModel>>(data);
            }
            return View("StateView", state);

        }

        #endregion

        #region AddorEditState
        public async Task<IActionResult> AddorEditState(int? StateId)
        {
            await GetCountryList();
            if(StateId.HasValue)
            {
                var respnse = await _client.GetAsync($"{_client.BaseAddress}/State/GetStateById/{StateId}");
                if (respnse.IsSuccessStatusCode)
                {
                    var data = await respnse.Content.ReadAsStringAsync();
                    var state=JsonConvert.DeserializeObject<StateModel>(data);
                    return View("StateForm",state);
                }
            }
            return View("StateForm");
        }

        #endregion

        #region Save

        [HttpPost]

        public async Task<IActionResult> Save(StateModel state)
        {
            if (ModelState.IsValid)
            {
                var json =JsonConvert.SerializeObject(state);
                var content =new StringContent(json,Encoding.UTF8,"application/json");
                HttpResponseMessage response;
                if (state.StateId == null)
                {
                    response = await _client.PostAsync($"{_client.BaseAddress}/State/AddState",content);
                }
                else
                {
                    response = await _client.PutAsync($"{_client.BaseAddress}/State/UpdateState/{state.StateId}",content);

                }
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("StateView");
                }
            }
            await GetCountryList();
            return View("StateForm", state);
        }

        #endregion

        #region DeleteState
        public async Task<IActionResult> DeleteState(int StateID)
        {
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/State/DeleteStateByPk/{StateID}");
            return RedirectToAction("StateView");

        }

        #endregion

        #region getCountryList
        public async Task GetCountryList()
        {
            HttpResponseMessage response =_client.GetAsync($"{_client.BaseAddress}/State/GetCountry").Result;
            if (response.IsSuccessStatusCode)
            {
                var data=await response.Content.ReadAsStringAsync();
                var countries=JsonConvert.DeserializeObject<List<CountryList>>(data);
                
   
                ViewBag.CountryList = countries;
            }
        }

        #endregion

    }
}
