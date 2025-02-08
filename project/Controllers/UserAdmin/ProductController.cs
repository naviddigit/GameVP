using Microsoft.AspNetCore.Mvc;

namespace UserApi.Controllers.UserAdmin
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        [Route("[action]")]
        [HttpGet]
        public object? List()
        {
            var responseData = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData == null) return Unauthorized();

            return new ProjectLayer.Models.Mod.MProductUser().List(responseData);
        }

        [Route("[action]")]
        [HttpPost]
        public object? ListByUsername(string username)
        {
            var responseData = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData == null) return Unauthorized();

            return new ProjectLayer.Models.Mod.MProductUser().List(responseData, username);
        }


        [Route("[action]")]
        [HttpPost]
        public object Create()
        {
            return null;
        }


        [Route("[action]")]
        [HttpPost]
        public object? Update(string username, int productUserId, decimal staticPrice)
        {
            var responseData = new ProjectLayer.Models.Mod.MAuth().Authorize(Request.Headers["Authorization"]);
            if (responseData == null) return Unauthorized();

            return new ProjectLayer.Models.Mod.MProductUser().Update(responseData, username, productUserId, staticPrice);
        }
    }

    [Route("api/[controller]")]
    public class TEST_ProductController : Controller
    {

        [Route("[action]")]
        [HttpGet]
        public object? List(string token, int productUserId)
        {
            return new ProjectLayer.Models.Mod.MProductUser().GetProductManager(token, productUserId);


        }

    }


}
