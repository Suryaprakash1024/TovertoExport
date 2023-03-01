using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toverto.Logics
{
    public class ApiCaller
    {
        private async Task<HttpResponseMessage> CallApiAsync(string apiUrl)
        {
            using HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
            {
                throw new HttpRequestException($"API call failed with status code {response.StatusCode}");
            }
        }

        public async Task<HttpResponseMessage> CallApiWithRetryAsync(string apiUrl, int maxRetries)
        {
            int retryCount = 0;
            while (retryCount < maxRetries)
            {
                try
                {
                    return await CallApiAsync(apiUrl);
                }
                catch (HttpRequestException ex)
                {
                    retryCount++;
                    if (retryCount >= maxRetries)
                    {
                        throw new HttpRequestException($"API call failed after {maxRetries} retries", ex);
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(2));
                    }
                }
            }
            throw new InvalidOperationException("Unexpected error occurred while making API call");
        }
    }
}
