using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Dx29
{
    static public partial class HttpClientExtensions
    {
        //
        //  GET
        //
        static public async Task<string> GetStringAsync(this HttpClient http, string action, params (string, string)[] headers) => await SendAsync(http, action, HttpMethod.Get, headers);
        static public async Task<TValue> GetAsync<TValue>(this HttpClient http, string action, params (string, string)[] headers) => await SendAsync<TValue>(http, action, HttpMethod.Get, headers);

        //
        //  POST
        //
        static public async Task<string> PostAsync(this HttpClient http, string action, object value, params (string, string)[] headers) => await SendAsync(http, action, HttpMethod.Post, value, headers);
        static public async Task<string> PostAsync(this HttpClient http, string action, Stream stream, params (string, string)[] headers) => await SendAsync(http, action, HttpMethod.Post, stream, headers);
        static public async Task<string> PostAsync(this HttpClient http, string action, HttpContent content, params (string, string)[] headers) => await SendAsync(http, action, HttpMethod.Post, content, headers);

        static public async Task<TValue> PostAsync<TValue>(this HttpClient http, string action, object value, params (string, string)[] headers) => await SendAsync<TValue>(http, action, HttpMethod.Post, value, headers);
        static public async Task<TValue> PostAsync<TValue>(this HttpClient http, string action, Stream stream, params (string, string)[] headers) => await SendAsync<TValue>(http, action, HttpMethod.Post, stream, headers);
        static public async Task<TValue> PostAsync<TValue>(this HttpClient http, string action, HttpContent content, params (string, string)[] headers) => await SendAsync<TValue>(http, action, HttpMethod.Post, content, headers);

        //
        //  PUT
        //
        static public async Task<string> PutAsync(this HttpClient http, string action, object value, params (string, string)[] headers) => await SendAsync(http, action, HttpMethod.Put, value, headers);
        static public async Task<string> PutAsync(this HttpClient http, string action, Stream stream, params (string, string)[] headers) => await SendAsync(http, action, HttpMethod.Put, stream, headers);
        static public async Task<string> PutAsync(this HttpClient http, string action, HttpContent content, params (string, string)[] headers) => await SendAsync(http, action, HttpMethod.Put, content, headers);

        static public async Task<TValue> PutAsync<TValue>(this HttpClient http, string action, object value, params (string, string)[] headers) => await SendAsync<TValue>(http, action, HttpMethod.Put, value, headers);
        static public async Task<TValue> PutAsync<TValue>(this HttpClient http, string action, Stream stream, params (string, string)[] headers) => await SendAsync<TValue>(http, action, HttpMethod.Put, stream, headers);
        static public async Task<TValue> PutAsync<TValue>(this HttpClient http, string action, HttpContent content, params (string, string)[] headers) => await SendAsync<TValue>(http, action, HttpMethod.Put, content, headers);

        //
        //  PATCH
        //
        static public async Task<string> PatchAsync(this HttpClient http, string action, object value, params (string, string)[] headers) => await SendAsync(http, action, HttpMethod.Patch, value, headers);
        static public async Task<string> PatchAsync(this HttpClient http, string action, Stream stream, params (string, string)[] headers) => await SendAsync(http, action, HttpMethod.Patch, stream, headers);
        static public async Task<string> PatchAsync(this HttpClient http, string action, HttpContent content, params (string, string)[] headers) => await SendAsync(http, action, HttpMethod.Patch, content, headers);

        static public async Task<TValue> PatchAsync<TValue>(this HttpClient http, string action, object value, params (string, string)[] headers) => await SendAsync<TValue>(http, action, HttpMethod.Patch, value, headers);
        static public async Task<TValue> PatchAsync<TValue>(this HttpClient http, string action, Stream stream, params (string, string)[] headers) => await SendAsync<TValue>(http, action, HttpMethod.Patch, stream, headers);
        static public async Task<TValue> PatchAsync<TValue>(this HttpClient http, string action, HttpContent content, params (string, string)[] headers) => await SendAsync<TValue>(http, action, HttpMethod.Patch, content, headers);

        //
        //  DELETE
        //
        static public async Task<string> DeleteAsync(this HttpClient http, string action, params (string, string)[] headers) => await SendAsync(http, action, HttpMethod.Delete, headers);
        static public async Task<TValue> DeleteAsync<TValue>(this HttpClient http, string action, params (string, string)[] headers) => await SendAsync<TValue>(http, action, HttpMethod.Delete, headers);


        static public async Task<TValue> SendAsync<TValue>(this HttpClient http, string action, HttpMethod method, params (string, string)[] headers)
        {
            return await SendAsync<TValue>(http, action, method, (HttpContent)null, headers);
        }
        static public async Task<TValue> SendAsync<TValue>(this HttpClient http, string action, HttpMethod method, object obj, params (string, string)[] headers)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            return await SendAsync<TValue>(http, action, method, content, headers);
        }
        static public async Task<TValue> SendAsync<TValue>(this HttpClient http, string action, HttpMethod method, Stream stream, params (string, string)[] headers)
        {
            var content = new StreamContent(stream);
            return await SendAsync<TValue>(http, action, method, content, headers);
        }
        static public async Task<TValue> SendAsync<TValue>(this HttpClient http, string action, HttpMethod method, HttpContent content, params (string, string)[] headers)
        {
            string json = await SendAsync(http, action, method, content, headers);
            return JsonConvert.DeserializeObject<TValue>(json);
        }

        static public async Task<string> SendAsync(this HttpClient http, string action, HttpMethod method, params (string, string)[] headers)
        {
            return await SendAsync(http, action, method, (HttpContent)null, headers);
        }
        static public async Task<string> SendAsync(this HttpClient http, string action, HttpMethod method, object obj, params (string, string)[] headers)
        {
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            return await SendAsync(http, action, method, content, headers);
        }
        static public async Task<string> SendAsync(this HttpClient http, string action, HttpMethod method, Stream stream, params (string, string)[] headers)
        {
            var content = new StreamContent(stream);
            return await SendAsync(http, action, method, content, headers);
        }
        static public async Task<string> SendAsync(this HttpClient http, string action, HttpMethod method, HttpContent content, params (string, string)[] headers)
        {
            var request = CreateRequest(action, method, headers);
            if (content != null) request.Content = content;
            (var resp, var status) = await SendAsync(http, request);
            if (status == HttpStatusCode.OK)
            {
                return resp;
            }
            throw new HttpRequestException(resp, null, status);
        }

        static public async Task<(string, HttpStatusCode)> SendAsync(this HttpClient http, HttpRequestMessage request)
        {
            var response = await http.SendAsync(request);
            return (await response.Content.ReadAsStringAsync(), response.StatusCode);
        }
    }
}
