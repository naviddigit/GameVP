using Microsoft.AspNetCore.Mvc;

namespace UserApi.Controllers.UserAdmin
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        [HttpPost]
        [Route("[action]")]
        public object login(string email, string password)
        {
            var login = new ProjectLayer.Models.Mod.MAuth().login(email, password);
            if (login.user == null) return NotFound(login);
            if (!login.user.Active) return NotFound(login);

            return login;
        }

        [HttpPost]
        [Route("[action]")]
        public object registerA(string display, string username, string password, string email, string mobile, int deposit, int creditorLimit)
        {
            var responseData_token = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData_token == null) return Unauthorized();

            var login = new ProjectLayer.Models.Mod.MAuth().registerA(display, username, password, email, mobile, deposit, creditorLimit, responseData_token);
            if (login.user == null)
                return NotFound(login);
            return login;
        }

        [HttpGet]
        [Route("my-account")]
        public object my_account()
        {
            var responseData = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData == null) return Unauthorized();

            var obj = new
            {
                user = new ProjectLayer.Models.Mod.MAuth().CallBack(responseData)
            };

            //var login = new JsonResult("user:" + new ProjectLayer.Models.Mod.MAuth().CallBack(responseData));
            return obj;
        }

        [HttpGet]
        [Route("[action]")]
        public object finance()
        {
            var responseData = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData == null) return Unauthorized();

            return new ProjectLayer.Models.Mod.MAccount().finance(responseData);
        }

        [HttpGet]
        [Route("[action]")]
        public object financeByDateLast(DateTime DateLast)
        {
            var responseData = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData == null) return Unauthorized();

            return new ProjectLayer.Models.Mod.MAccount().finance(responseData, DateLast);
        }

        [HttpGet]
        [Route("Transaction-list")]
        public object Transaction()
        {
            var responseData = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData == null) return Unauthorized();

            return new ProjectLayer.Models.Mod.MTransaction().list(responseData);
        }

        [Route("Transaction/Charge")]
        [HttpPost]
        public object Transaction(string username, decimal amount, int typeId, string description)
        {
            var responseData_Token = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData_Token == null) return Unauthorized();

            var Create = new ProjectLayer.Models.Mod.MTransaction().Charge(responseData_Token, username, amount, typeId, description);

            if (!Create.Success)
                return NotFound(Create);
            return Create;
        }

        [HttpPost]
        [Route("[action]")]
        public object? Get(string username)
        {
            var responseData = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData == null) return Unauthorized();

            return new ProjectLayer.Models.Mod.MAccount().Get(responseData, username);
        }


        [HttpPost]
        [Route("[action]")]
        public object? Update(string username, string password, string email, string mobile, int creditorLimit, bool active)
        {
            var responseData = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData == null) return Unauthorized();

            return new ProjectLayer.Models.Mod.MAuth().Update(username, password, active, email, mobile, creditorLimit, responseData);
        }


        [HttpGet]
        [Route("[action]")]
        public object list()
        {
            var responseData = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData == null) return Unauthorized();

            return new ProjectLayer.Models.Mod.MAccount().list(responseData);
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
    public class TEST_AccountController : Controller
    {

        [HttpGet]
        [Route("[action]")]
        public object GetBalance(int accountId, decimal price = 80000)
        {
            return new ProjectLayer.Models.Mod.MAccount().financeAllow(accountId, price);

        }
    }
}
