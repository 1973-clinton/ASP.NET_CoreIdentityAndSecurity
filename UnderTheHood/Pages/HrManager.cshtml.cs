using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnderTheHood.Models;

namespace UnderTheHood.Pages
{
    [Authorize(Policy = "HRMangerOnly")]
    public class HrManagerModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public List<WeatherForecastDto> WeatherForecastItems { get; set; }
        public HrManagerModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task OnGet()
        {
            WeatherForecastItems = await InvokeEndpoint<List<WeatherForecastDto>>("api/WeatherForecast", "UnderTheHoodClient");
        }

        private async Task<T> InvokeEndpoint<T>(string url, string clientName)
        {
            //get token from session
            JwtToken token = null;
            var strTokenObj = HttpContext.Session.GetString("access_token");

            // generate a new token from authProvider if existing token has expired
            if (string.IsNullOrEmpty(strTokenObj))
            {
                token = await Authenticate();
            }
            else
            {
                token = JsonConvert.DeserializeObject<JwtToken>(strTokenObj);
                //var response = httpClient.GetAsync("api/WeatherForecast").Result;
            }

            if (token == null || string.IsNullOrEmpty(token.AccessToken) || token.ExpiresAt <= DateTime.UtcNow)
            {
                token = await Authenticate();
            }


            // Authentication and getting the token
            var httpClient = _httpClientFactory.CreateClient(clientName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
             return await httpClient.GetFromJsonAsync<T>(url);
        }

        private async Task<JwtToken> Authenticate()
        {
            var httpClient = _httpClientFactory.CreateClient("UnderTheHoodClient");
            var res = await httpClient.PostAsJsonAsync("api/Auth", new Credential()
            {
                UserName = "admin",
                Password = "password"
            });
            res.EnsureSuccessStatusCode();
            var strJwt = await res.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("access_token", strJwt);
            return JsonConvert.DeserializeObject<JwtToken>(strJwt);
        }
    }
}
