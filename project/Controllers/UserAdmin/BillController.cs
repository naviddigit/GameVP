using Microsoft.AspNetCore.Mvc;

namespace UserApi.Controllers.UserAdmin
{
    [Route("api/[controller]")]
    public class BillController : Controller
    {
        [Route("[action]")]
        [HttpPost]
        public object HazineLoop(int Day_Period = 10)
        {

            // check date 10

            // if(any transaction date > day-10)

            return "";

        }
    }
}
