using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PayrollProcessor.Functions.Infrastructure
{
    public static class Request
    {
        public async static Task<T> Parse<T>(HttpRequest request)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();

            return JsonConvert.DeserializeObject<T>(requestBody);
        }
    }
}
