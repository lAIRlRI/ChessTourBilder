using Azure;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ChessTourBuilderApp.Data.Api
{
    internal class ApiControler
    {
        public static string baseURL = "https://localhost:7271/api/";

        static HttpClient httpClient = new();

        public static async Task<string> Post(string url, object body) 
        {
            var response = await httpClient.PostAsJsonAsync(baseURL+url, body);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public static async Task<string> Get(string url)
        {
            var response = await httpClient.GetAsync(baseURL + url);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}