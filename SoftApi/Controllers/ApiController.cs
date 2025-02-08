using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using SoftEther.VPNServerRpc;

namespace SoftApi.Controllers
{
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {

        VpnServerRpc api;

        ////public ApiController(string vpnserver_host, int vpnserver_port, string admin_password, string hub_name = null)
        ////{
        ////    api = new VpnServerRpc(vpnserver_host, vpnserver_port, admin_password, "");       // Speficy your VPN Server's password here.
        ////}


        ////[HttpPost("[action]")]
        ////public bool HasUser(string Ip,int Port, string Password,string HubName, string username)
        ////{
        ////    var ApiVPNSERVER = new SoftEther.Models.VPNRPCT(Ip, Port, Password, HubName);

        ////    // count user:
        ////    var getUser = Remote_AnyUserAsync(username, HubName);
        ////    if (getUser) return true;

        ////    return false;   
        ////}


        VpnServerRpc client = new VpnServerRpc("hostname", 443, "hubname", "password");

        public async Task<bool?> Remote_AnyUserAsync(string Ip, int Port, string Password, string HubName, string username)
        {




            // Check if a user exists
            string hubName = "MyHub";
            bool userExists = (bool)client.CallMethod("IsUserExists", hubName, username);

            if (userExists)
            {
                Console.WriteLine($"User {username} exists in hub {hubName}");
            }
            else
            {

                Console.WriteLine($"User {username} does not exist in hub {hubName}");
            }



            //            //string Ip,int Port, string Password,string HubName, string username

            //            using (var client = new HttpClient())
            //            {
            //                client.BaseAddress = new Uri("http://externalserver.com/");
            //                client.DefaultRequestHeaders.Accept.Clear();
            //                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //                var content = new FormUrlEncodedContent(new[]
            //{
            //    new KeyValuePair<string, string>("Ip", Ip.ToString()),
            //    new KeyValuePair<string, string>("Port", Port.ToString()),
            //    new KeyValuePair<string, string>("Password", Password),
            //    new KeyValuePair<string, string>("HubName", HubName),
            //    new KeyValuePair<string, string>("username", username)
            //});


            //                HttpResponseMessage response = await client.PostAsync("api/externalclass", content);

            //                if (response.IsSuccessStatusCode)
            //                {

            //                    object responseString = await response.Content.ReadAsStringAsync();
            //                    return Convert.ToBoolean(responseString);

            //                }
            //                else
            //                {
            //                    return null;
            //                }
            //            }


        }



        }
}