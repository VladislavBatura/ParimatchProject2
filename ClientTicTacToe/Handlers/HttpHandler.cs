using System.Net;

namespace ClientTicTacToe.Handlers
{
    public class HttpHandler
    {
        private readonly HttpClient _httpClient = new();

        public async Task<HttpStatusCode> PostAsync<T>(string url, T obj)
        {
            var data = obj.ToStringContent();
            var response = await _httpClient.PostAsync(url, data);

            return response.StatusCode;
        }

        public async Task<(HttpStatusCode status, string? content)> GetAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();
            return (response.StatusCode, content);
        }

        /// <summary>
        /// <see cref="TResult"/> - the type of object you want to retrieve from the server.
        /// <see cref="TObj"/> - the type of object to send to the server
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<(HttpStatusCode status, TResult? result)> PostAsync<TResult, TObj>(string url, TObj obj)
        {
            var data = obj.ToStringContent();
            var response = await _httpClient.PostAsync(url, data);

            var content = await response.Content.ReadAsStringAsync();
            var result = response.IsSuccessStatusCode ? content.Deserialize<TResult>() : default;

            return (response.StatusCode, result);
        }
    }
}
