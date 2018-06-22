using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace only.Identity.Client
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

            // request token
            //var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            //var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");alice password
            var tokenClient = new TokenClient(disco.TokenEndpoint, "mvc", "secret");
            
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("416269473@qq.com", "Zss521@", "api1");
            //var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "App.Api");
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("https://localhost:5001/api/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
