using DataService.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Tasks
{
    internal class AutoLimit
    {

        // -------------------------------------------------- SERVICE START -------------------------------------------
        /// <summary>
        /// اجرای سرویس
        /// </summary>
        public async Task DeleteSessionDublicate()
        {
            DateTime DateHoursAgo = DateTime.Now.AddHours(-5);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  \bChoese Item From This List:\n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine(" 0 = Test Connection Servers Softether\n 1 = kill\n 2 = Continu\n 3 = Total Dablicate\n 4 = Total List Dablicate\n 5 = Kill by Username");
            var k = Console.ReadLine();

            try
            {
                var serverSoft = new ProjectLayer.Models.Mod.MServerSoft();
                var getDub = serverSoft.ListSessionDublicate();

                Console.WriteLine();
                Console.WriteLine();

                if (k == "0")
                {
                    try
                    {
                        serverSoft.TestRemoteAllServer();
                    }
                    catch (Exception ex)
                    {

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n  \ber0.:" + ex.Message + "\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                }

                if (k == "1")
                {
                    serverSoft.RemoveSession(getDub);
                }

                if (k == "2")
                {
                }

                if (k == "3")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Total Dublicate: " + getDub.Count);
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if (k == "4")
                {
                    foreach (var item in getDub)
                    {

                        string details = "";
                        try
                        {
                            foreach (var item2 in item?.VpnRpcEnumSessionItemDublicateDetails)
                            {
                                TimeSpan sdt = DateTime.Now.AddMinutes(-213) - (DateTime)item2.CreatedTime_dt;
                                details += "IP: " + item2?.serverIp + " Remote: " + item2?.RemoteHostname_str + " ClientIP: " + item2?.ClientIP_ip + " duration date: " + sdt.Days + " day " + sdt.Hours + ":" + sdt.Minutes + "\n";
                            }
                        }
                        catch (Exception er)
                        {
                            Console.WriteLine("er" + er.Message);
                        }

                        if (item.CountConnection > 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("\n" + item.Username_str + " count: " + item.CountConnection + " Agent: " + item.Agent_srt);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(details);
                        }
                    }
                }



                if (k == "5")
                {
                    var username = Console.ReadLine();

                    if (username != "")
                        serverSoft.RemoveSession(getDub, username);
                }

            }
            catch (Exception ex)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  \ber2.:" + ex.Message + "\n" + ex.Source);
            }

            DeleteSessionDublicate();

        }

    }
}
