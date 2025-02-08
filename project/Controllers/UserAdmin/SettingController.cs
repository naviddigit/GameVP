using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;

namespace UserApi.Controllers.UserAdmin
{
    [Route("api/[controller]")]
    public class SettingController : Controller
    {
        [Route("[action]")]
        [HttpPost]
        public object ImportByFile(string filename = "LAST UPLOAD 2023-4-26.txt") //"server-81.txt")
        {

            FileInfo f = new FileInfo(filename);

            using (StreamReader sr = f.OpenText())
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {


                    string username = s.Split("	")[0];
                    string password = s.Split("	")[1];
                    string startDate = s.Split("	")[2];
                    string expairDate = s.Split("	")[3];
                    string agent = s.Split("	")[4];
                    string comment = s.Split("	")[5];
                    string group = "import";

                    //Console.WriteLine(username);
                    //Console.WriteLine(password);
                    //Console.WriteLine(startDate);
                    //Console.WriteLine(expairDate);
                    //Console.WriteLine(agent);
                    //Console.WriteLine(comment);


                    //if (!new IBSNG.User().AnyUser(username))
                    //{
                    //    new IBSNG.User().Create(username, password, group, agent, comment, "", expairDate);
                    //    Console.WriteLine(s);
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Exist: " + username);

                    //}

                }
            }

            return "";

        }

        [Route("[action]")]
        [HttpPost]
        public object ImportByFileAndData(string filename = "LAST UPLOAD 2023-4-26.txt") //"server-81.txt")
        {

            FileInfo f = new FileInfo(filename);

            using (StreamReader sr = f.OpenText())
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {


                    string username = s.Split("	")[0];
                    string password = s.Split("	")[1];
                    string startDate = s.Split("	")[2];
                    string expairDate = s.Split("	")[3];
                    string agent = s.Split("	")[4];
                    string comment = s.Split("	")[5];
                    string group = "import";

                    //Console.WriteLine(username);
                    //Console.WriteLine(password);
                    //Console.WriteLine(startDate);
                    //Console.WriteLine(expairDate);
                    //Console.WriteLine(agent);
                    //Console.WriteLine(comment);


                    //if (!new IBSNG.User().AnyUser(username))
                    //{
                    //    object? userId_IBSng = new IBSNG.User().Create(username, password, group, agent, comment, "", expairDate);

                    //    if (userId_IBSng != null)
                    //    {

                    //        DataService.Entity.User item = new DataService.Entity.User()
                    //        {
                    //            Username = username,
                    //            Password = password,
                    //            ExpirationDate = expairDate.ToString() != "-" ? Convert.ToDateTime(expairDate) : null,
                    //            //ProductId = 0,
                    //            AccountId = 2,
                    //            ServerId = 1,
                    //            UserIdIbsng = Convert.ToInt32(userId_IBSng),
                    //            CreateDate = DateTime.Now,
                    //            ProductId = 4, // 4 == IMPORT
                    //            Comment = comment,
                    //        };

                    //        new ProjectLayer.Models.Repository.RUser().Insert(item);

                    //        //new ProjectLayer.Models.Repository.RUser().UpdateUserId_IBSng(username, userId_IBSng);
                    //    }


                    //    Console.WriteLine(s);
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Exist: " + username);

                    //}

                }
            }

            return "";

        }

        [Route("[action]")]
        [HttpPost]
        public object Import()
        {

            var userList = (List<DataService.Entity.User>)new ProjectLayer.Models.Repository.RUser().List(0);


            foreach (var item in userList)
            {


                //if (!new IBSNG.User().AnyUser(item.Username))
                //{
                //    object? userId_IBSng = new IBSNG.User().Create(item.Username, item.Password, item.Product == null ? "1M" : item.Product.GroupIbsng, item.Account.Username, "import", item.PhoneNumber, item.ExpirationDate);

                //    if (userId_IBSng != null) new ProjectLayer.Models.Repository.RUser().UpdateUserId_IBSng(item.Username, userId_IBSng);

                //    Console.WriteLine("add : " + item.Username);
                //}
                //else
                //{
                //    Console.WriteLine("Exist: " + item.Username);

                //}

            }

            return "";

        }


    }

    [Route("api/[controller]")]
    public class ServerController : Controller
    {

        [Route("[action]")]
        [HttpGet]
        public object? List()
        {
            return new ProjectLayer.Models.Mod.MServer().List();
        }

        [Route("list/ip")]
        [HttpGet]
        public object? ListIp()
        {
            return new ProjectLayer.Models.Mod.MServer().ListIp();
        }
    }

}
