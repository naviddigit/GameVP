using Microsoft.AspNetCore.Mvc;

namespace UserApi.Controllers.UserAdmin
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        [Route("[action]")]
        [HttpGet]
        public object? List()
        {
            var responseData_token = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData_token == null) return Unauthorized();

            return new ProjectLayer.Models.Mod.MUser().List(responseData_token);
        }

        [Route("[action]")]
        [HttpPost]
        public object? Get_And_Update_ExpairetionDate(string username, string serverIp)
        {
            var responseData_token = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData_token == null) return Unauthorized();

            var user = new ProjectLayer.Models.Mod.MUser().IBSNG_Get_And_Update_ExpairetionDate(username, serverIp);
            if (!user.Success)
                return NotFound(user);

            return user;
        }


        [Route("[action]")]
        [HttpPost]
        public object? Get(string username)
        {
            var responseData_token = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData_token == null) return Unauthorized();

            return new ProjectLayer.Models.Mod.MUser().Get(responseData_token, username);
        }

        [Route("[action]")]
        [HttpPost]
        public object Create(string username, string password, string phoneNumber,string description, int productUserId)
        {
            var responseData_Token = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData_Token == null) return Unauthorized();

            var Create = new ProjectLayer.Models.Mod.MUser().IBSNG_CreateUser(username, password, phoneNumber,  description, productUserId, responseData_Token);

            if (!Create.Success)
                return NotFound(Create);
            return Create;
        }


        [Route("[action]")]
        [HttpPost]
        //      username, password, phoneNumber, description, Banned, BannedText
        public object? Update(string username, string password, string phoneNumber, string description,bool Banned, string BannedText)
        {
            var responseData_token = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData_token == null) return Unauthorized();

            var user = new ProjectLayer.Models.Mod.MUser().IBSNG_Update(responseData_token, username, password, phoneNumber, description, Banned, BannedText);
            if (!user.Success)
                return NotFound(user);

            return user;
        }


        [Route("[action]")]
        [HttpPost]
        public object? Renew(string username, int productUserId, string serverIp)
        {
            var responseData_token = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData_token == null) return Unauthorized();

            var user = new ProjectLayer.Models.Mod.MUser().IBSNG_Renew(responseData_token, username, productUserId, serverIp);
            if (!user.Success)
                return NotFound(user);

            return user.Success;
        }




        [Route("[action]")]
        [HttpPost]
        public object? Renew_Only(string username, int productUserId, string serverIp)
        {
            var responseData_token = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData_token == null) return Unauthorized();

            var user = new ProjectLayer.Models.Mod.MUser().IBSNG_Renew_Only(responseData_token, username, productUserId, serverIp);
            if (!user.Success)
                return NotFound(user);

            return user.Success;
        }


        [Route("[action]")]
        [HttpPost]
        public object? ChangeServerIp(string username, string serverIp)
        {
            var responseData_token = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData_token == null) return Unauthorized();

            var user = new ProjectLayer.Models.Mod.MUser().ChangeServerIp(responseData_token, username, serverIp);
            if (!user.Success)
                return NotFound(user);

            return user.Success;
        }


        [Route("[action]")]
        [HttpPost]
        public object? ChangeAccountId(string username, int changeAccountId)
        {
            var responseData_token = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData_token == null) return Unauthorized();

            var user = new ProjectLayer.Models.Mod.MUser().IBSNG_ChangeAccountId(responseData_token, username, changeAccountId);
            if (!user.Success)
                return NotFound(user.Message);

            return user.Success;
        }


        [Route("[action]")]
        [HttpGet]
        public object? Report()
        {
            var responseData_token = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData_token == null) return Unauthorized();

            //return new ProjectLayer.Models.Mod.MUser().List(responseData_token);
            return new ProjectLayer.Models.Mod.MUser().Report(responseData_token);
        }

    }




    [Route("api/[controller]")]
    public class TEST_UserController : Controller
    {


        [Route("[action]")]
        [HttpPost]
        public object CreateTEST(string username, string ip)
        {
            if (ip == "1")
                return Unauthorized();
            if (ip == "2")
                return NotFound();


            var responseData_Token = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData_Token == null) return Unauthorized();

            var Create = new ProjectLayer.Models.Mod.MUser().CreateUserTest(username, ip);

            if (!Create.Success)
                return NotFound(Create);
            return Create;
        }


        [Route("[action]")]
        [HttpPost]
        public object IBSNG_CreateTEST(string username, string password, string group = "1M", string comment = "ADMIN-TEST")
        {
            var responseData_token = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData_token == null) return Unauthorized();


            var Create = new ProjectLayer.Models.Mod.MUser().IBSNG_CreateUserTest(username, password, responseData_token, group, comment);

            if (!Create.Success)
                return NotFound(Create);
            return Create;
        }


        [Route("[action]")]
        [HttpPost]
        public object IBSNG_Renew(string username, string password, string Group_IBSng = "1M")
        {
            var responseData_token = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData_token == null) return Unauthorized();

            var Create = new ProjectLayer.Models.Mod.MUser().IBSNG_RenewTest(responseData_token, username, Group_IBSng);

            if (!Create.Success)
                return NotFound(Create);
            return Create;
        }



        [Route("[action]")]
        [HttpPost]
        public object AnyUser(string username, string ip)
        {
            if (ip == "1")
                return Unauthorized();
            if (ip == "2")
                return NotFound();


            object get = new ProjectLayer.Models.Mod.MUser().AnyUserA(username, ip); ;
            return (bool)get;



        }




        [Route("[action]")]
        [HttpPost]
        public object GetIdByUsername(string username)
        {

            return new ProjectLayer.Models.Mod.MUser().IBSNG_GetIdByUsername(username);
        }
    }
}
