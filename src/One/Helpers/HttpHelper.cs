using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace One.Helpers
{
    public class HttpHelper<T> where T : class
    {
        public static async Task<T> GetAsync(string requestUri)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(requestUri);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                try
                {
                    return (T)Newtonsoft.Json.JsonConvert.DeserializeObject(result, typeof(T));
                }
                catch (Exception ex)
                {
                    return default(T);
                }
            }
            return default(T);
        }
    }
}
