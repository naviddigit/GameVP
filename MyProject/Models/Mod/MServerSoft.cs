using DataService.Entity;
using IBSNG;
using ProjectLayer.Models.Stucture;
using SoftEther.VPNServerRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLayer.Models.Mod
{
    public class MServerSoft
    {
        public List<VpnRpcEnumSessionItem> ListSession()
        {
            List<ServerSoft>? _server = (List<ServerSoft>?)new Repository.RServerSoft()?.List(null);

            List<VpnRpcEnumSessionItem> vpnRpcEnumSessionItems = new List<VpnRpcEnumSessionItem>();
            foreach (var server in _server)
            {

                // get server info
                var ApiVPNSERVER = new SoftEther.Models.VPNRPCT(server.VpnserverHost, server.VpnserverPort.Value, server.AdminPassword, server.HubName);

                string name;

                vpnRpcEnumSessionItems.AddRange(ApiVPNSERVER.ListSession(server.HubName).ToList());
                vpnRpcEnumSessionItems = vpnRpcEnumSessionItems.OrderBy(i => i.LastCommTime_dt).ToList();

                //foreach (var UserServer in ApiVPNSERVER.ListSession())
                //{
                //    //name = UserServer.Name_str;
                //    //rUser.Update()
                //}

            }

            return vpnRpcEnumSessionItems;
        }


        public void TestRemoteAllServer()
        {
            Console.WriteLine("\n  \btest1.:1\n");

            List<ServerSoft>? _server = (List<ServerSoft>?)new Repository.RServerSoft()?.List(null);
            Console.Write(" .");

            List<VpnRpcEnumSessionItem> vpnRpcEnumSessionItems = new List<VpnRpcEnumSessionItem>();
            Console.Write(" .");

            List<VpnRpcEnumSessionItemDublicate> vpnRpcEnumSessionItemsSelect = new List<VpnRpcEnumSessionItemDublicate>();
            Console.Write(" .");

            foreach (var server in _server)
            {
                Console.Write(" .");

                try
                {
                    // get server info
                    var ApiVPNSERVER = new SoftEther.Models.VPNRPCT(server.VpnserverHost, server.VpnserverPort.Value, server.AdminPassword, server.HubName);

                    var list2 = ApiVPNSERVER.ListSession(server.HubName).ToList();

                    Console.WriteLine("ip connect: " + server.Ip + " / Count User Online: " + list2.Count);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ip: " + server.Ip + " message:" + ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }

            }
        }


        public List<VpnRpcEnumSessionItemDublicate> ListSessionDublicate()
        {
            Console.WriteLine("Test Connections:");

            List<ServerSoft>? _server = (List<ServerSoft>?)new Repository.RServerSoft()?.List(null);
            Console.Write(" .");

            List<VpnRpcEnumSessionItem> vpnRpcEnumSessionItems = new List<VpnRpcEnumSessionItem>();
            Console.Write(" .");

            List<VpnRpcEnumSessionItemDublicate> vpnRpcEnumSessionItemsSelect = new List<VpnRpcEnumSessionItemDublicate>();
            Console.Write(" .");

            List<string> stringsName = new List<string>();
            Console.Write(" .");


            var user = new Repository.RUser();

            foreach (var server in _server)
            {
                Console.Write(" / *" + server.Ip);
                List<VpnRpcEnumSessionItem> VpnRpcEnumSessionItem_List = new List<VpnRpcEnumSessionItem>();

                // get server info
                var ApiVPNSERVER = new SoftEther.Models.VPNRPCT(server.VpnserverHost, server.VpnserverPort.Value, server.AdminPassword, server.HubName);

                Console.Write(" *");

                VpnRpcEnumSessionItem[] list = ApiVPNSERVER.ListSession(server.HubName);
                foreach (var item in list)
                {

                    VpnRpcEnumSessionItem_List.Add(new VpnRpcEnumSessionItem()
                    {
                        Name_str = item.Name_str,
                        ClientIP_ip = item.ClientIP_ip,
                        CreatedTime_dt = item.CreatedTime_dt,
                        Hostname_str = item.RemoteHostname_str,
                        LastCommTime_dt = item.LastCommTime_dt,
                        RemoteHostname_str = item.RemoteHostname_str,
                        Username_str = item.Username_str,
                        Server_Id = server.Id,
                        Server_Ip = server.Ip,

                    });
                }
                Console.Write(" *");


                vpnRpcEnumSessionItems.AddRange(VpnRpcEnumSessionItem_List);
                Console.Write(" *");

                vpnRpcEnumSessionItems = vpnRpcEnumSessionItems.OrderBy(i => i.LastCommTime_dt).ToList();
                Console.Write(" *");

            }


            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nTotal Online: " + vpnRpcEnumSessionItems.Count);
            Console.ForegroundColor = ConsoleColor.White;

            foreach (var item in vpnRpcEnumSessionItems)
            {

                var countSesion = vpnRpcEnumSessionItems.Count(i => i.Username_str == item.Username_str);
                if (countSesion > 1 && item.Username_str != "Local Bridge")
                {
                    if (!vpnRpcEnumSessionItemsSelect.Any(i => i.Username_str == item.Username_str))
                    {
                        List<VpnRpcEnumSessionItemDublicateDetails> Details = new List<VpnRpcEnumSessionItemDublicateDetails>();

                        foreach (var item2 in vpnRpcEnumSessionItems.Where(i => i.Username_str == item.Username_str))
                        {
                            try
                            {
                                Details.Add(new VpnRpcEnumSessionItemDublicateDetails
                                {
                                    ClientIP_ip = item2.ClientIP_ip,
                                    CreatedTime_dt = item2.CreatedTime_dt,
                                    LastCommTime_dt = item2.LastCommTime_dt,
                                    RemoteHostname_str = item2?.Hostname_str,
                                    Name_str = item2?.Name_str,

                                    serverId = item2.Server_Id,
                                    serverIp = item2.Server_Ip,
                                });
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n er4:" + ex.Message + "\n");
                                Console.ForegroundColor = ConsoleColor.White;

                            }

                        }

                        DataService.Entity.User _u = (DataService.Entity.User)user.Get(item.Username_str);

                        vpnRpcEnumSessionItemsSelect.Add(new VpnRpcEnumSessionItemDublicate
                        {
                            Username_str = item.Username_str,
                            Agent_srt = _u?.Account?.DisplayName,

                            CountConnection = countSesion,
                            VpnRpcEnumSessionItemDublicateDetails = Details,
                        });
                    }

                }

            }


            return vpnRpcEnumSessionItemsSelect;
        }


        public void RemoveSession(List<VpnRpcEnumSessionItemDublicate> listDablicate, string username = "")
        {
            List<ServerSoft>? _server = (List<ServerSoft>?)new Repository.RServerSoft()?.List(null);

            var rUser = new Repository.RUser();

            Console.WriteLine("Dublicite: " + listDablicate.Count);

            int count = 0;

            foreach (var item in listDablicate)
            {
                count++;

                if (!rUser.Any1(item.Username_str))
                {
                    if (!item.Username_str.Contains("SecureNAT"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Not Esixt Username in Database\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }

                if (username == "") Console.WriteLine("\n" + count + "- Username_str: " + item.Username_str + "\n");

                foreach (var item2 in item.VpnRpcEnumSessionItemDublicateDetails)
                {

                    var server = _server.Single(i => i.Id == item2.serverId);
                    var ApiVPNSERVER = new SoftEther.Models.VPNRPCT(server.VpnserverHost, server.VpnserverPort.Value, server.AdminPassword, server.HubName);



                    // تمام کاربر ها ::
                    if (username == "")
                    {
                        var LimitUser = 0;

                        if (rUser.Any1(item.Username_str))
                        {
                            var user = (DataService.Entity.User?)rUser.Get(item.Username_str);
                            LimitUser = user.LimitUser;
                        }

                        if (item.CountConnection > LimitUser)
                        {
                            try
                            {
                                ApiVPNSERVER.DeleteSesion(server.HubName, item2.Name_str);

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("DeleteSesion / IP: " + server.Ip + " / Remot: " + item2.RemoteHostname_str + " / ClientIP " + item2.ClientIP_ip);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("DeleteSesion / IP: " + server.Ip + " / Remot: " + item2.RemoteHostname_str + " / ClientIP" + item2.ClientIP_ip + " / er: " + ex.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                    }


                    // فقط یک کاربر مشخص ::
                    else
                    {
                        if (item2.Name_str == username)
                        {
                            try
                            {
                                ApiVPNSERVER.DeleteSesion(server.HubName, item2.Name_str);

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("DeleteSesion / IP: " + server.Ip);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("DeleteSesion / IP: " + server.Ip + " / er: " + ex.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                    }



                }
            }
        }
    }
}
