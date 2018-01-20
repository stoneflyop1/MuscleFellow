using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MuscleFellow.ViewModels;
using Newtonsoft.Json;

namespace MuscleFellow
{
    public class ApiClient : IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _host;

        private ApiClient()
        {
            // https://stackoverflow.com/questions/5528850/how-to-connect-localhost-in-android-emulator
            _host = "http://localhost:5001"; //安卓模拟器需要使用10.0.2.2访问localhost
            _client = new HttpClient() { BaseAddress = new Uri(_host) };
        }

        private static ApiClient _default;

        public static ApiClient Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new ApiClient();
                }
                return _default;
            }
        }

        private string _accessToken;

        public async Task<string> LoginAsync(string userName, string password)
        {
            var loginModel = new Dictionary<string, string> { { "UserId", userName }, { "Password", password } };
            var url = "api/Account/Login";
            var content = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");
            var res = await _client.PostAsync(url, content);
            var resContent = await res.Content.ReadAsStringAsync();
            if (res.IsSuccessStatusCode)
            {
                var tokenDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(resContent);
                _accessToken = tokenDict["access_token"];
                return String.Empty;
            }
            else
            {
                return resContent;
            }
        }

        public async Task<List<ProductViewModel>> GetProductsAsync()
        {
            var url = "api/Products";
            var res = await _client.GetAsync(url);
            var resContent = await res.Content.ReadAsStringAsync();
            if (res.IsSuccessStatusCode)
            {
                var items = JsonConvert.DeserializeObject<List<ProductViewModel>>(resContent);
                foreach(var i in items)
                {
                    i.ThumbnailImage = _host + i.ThumbnailImage;
                }
                return items;
            }
            return new List<ProductViewModel>();
        }

        public void Dispose()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }
    }
}
