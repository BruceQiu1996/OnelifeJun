using Microsoft.JSInterop;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace LiJunSpace.ServerMode.Helpers
{
    public class MyHttpRequest
    {
        internal string _token;
        private readonly HttpClient _httpClient;
        public static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(new TextEncoderSettings(UnicodeRanges.All)),
            ReadCommentHandling = JsonCommentHandling.Skip,
            PropertyNameCaseInsensitive = true,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        private readonly IJSRuntime _jsRuntime;

        public MyHttpRequest(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _httpClient = new HttpClient();
            BuildHttpClient(_httpClient);
        }

        public async Task SetTokenAsync()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "token");
            if (!string.IsNullOrEmpty(token))
            {
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }

                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string url, dynamic body)
        {
            await SetTokenAsync();
            HttpResponseMessage resp = null;
            if (body == null)
            {
                resp = await _httpClient.PostAsync(url, null);
            }
            else
            {
                var content = new StringContent(JsonSerializer.Serialize(body, _jsonSerializerOptions));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp = await _httpClient.PostAsync(url, content);
            }
            if (resp.IsSuccessStatusCode)
            {
                return resp;
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                ExcuteWhileUnauthorized?.Invoke();
                return default;
            }
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
            {
                var message = await resp.Content.ReadAsStringAsync();
                ExcuteWhileBadRequest?.Invoke(message);
            }
            else if (resp.StatusCode == HttpStatusCode.InternalServerError)
            {
                var message = await resp.Content.ReadAsStringAsync();
                ExcuteWhileInternalServerError?.Invoke(message);
            }

            return default;
        }

        public async Task<HttpResponseMessage> PutAsync(string url, dynamic body)
        {
            await SetTokenAsync();
            HttpResponseMessage resp = null;
            if (body == null)
            {
                resp = await _httpClient.PutAsync(url, null);
            }
            else
            {
                var content = new StringContent(JsonSerializer.Serialize(body, _jsonSerializerOptions));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp = await _httpClient.PutAsync(url, content);
            }
            if (resp.IsSuccessStatusCode)
            {
                return resp;
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                ExcuteWhileUnauthorized?.Invoke();
                return default;
            }
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
            {
                var message = await resp.Content.ReadAsStringAsync();
                ExcuteWhileBadRequest?.Invoke(message);
            }
            else if (resp.StatusCode == HttpStatusCode.InternalServerError)
            {
                var message = await resp.Content.ReadAsStringAsync();
                ExcuteWhileInternalServerError?.Invoke(message);
            }

            return default;
        }

        public async Task<HttpResponseMessage> PostImageAsync(string url, string filename, byte[] bytes)
        {
            await SetTokenAsync();
            var fileContent = new ByteArrayContent(bytes);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = filename
            };
            var content = new MultipartFormDataContent
                {
                    fileContent
                };

            var resp = await _httpClient.PostAsync(url, content);
            if (resp.IsSuccessStatusCode)
            {
                return resp;
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                ExcuteWhileUnauthorized?.Invoke();
                return default;
            }
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
            {
                var message = await resp.Content.ReadAsStringAsync();
                ExcuteWhileBadRequest?.Invoke(message);
            }
            else if (resp.StatusCode == HttpStatusCode.InternalServerError)
            {
                var message = await resp.Content.ReadAsStringAsync();
                ExcuteWhileInternalServerError?.Invoke(message);
            }

            return default;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            await SetTokenAsync();
            var resp = await _httpClient.GetAsync(url).ConfigureAwait(false);
            if (resp.IsSuccessStatusCode)
            {
                return resp;
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                ExcuteWhileUnauthorized?.Invoke();
                return default;
            }
            else if (resp.StatusCode == HttpStatusCode.BadRequest)
            {
                var message = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                ExcuteWhileBadRequest?.Invoke(message);
            }
            else if (resp.StatusCode == HttpStatusCode.InternalServerError)
            {
                var message = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                ExcuteWhileInternalServerError?.Invoke(message);
            }

            return default;
        }

        public Func<Task<bool>> TryRefreshToken;//当服务端返回401的时候，尝试利用refreshtoken重新获取accesstoken以及refreshtoken
        public event Action ExcuteWhileUnauthorized; //401
        public event Action<string> ExcuteWhileBadRequest;//400
        public event Action<string> ExcuteWhileInternalServerError;//500

        public void BuildHttpClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(Program.APIEndPoint);
            httpClient.Timeout = TimeSpan.FromSeconds(600);
            httpClient.DefaultRequestVersion = HttpVersion.Version10;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
