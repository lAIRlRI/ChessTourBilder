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
            try
            {
                var response = await httpClient.PostAsJsonAsync(baseURL + url, body);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }

        public static async Task<string> Put(string url, object body)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync(baseURL + url, body);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }

        public static async Task<string> Get(string url)
        {
            try
            {
                var response = await httpClient.GetAsync(baseURL + url);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch(Exception e)
            {
                return e.Message.ToString();
            }
        }

        public static async Task<string> Delete(string url)
        {
            try
            {
                var response = await httpClient.DeleteAsync(baseURL + url);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }
    }
}