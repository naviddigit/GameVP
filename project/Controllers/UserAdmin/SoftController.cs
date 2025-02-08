using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SoftEther.VPNServerRpc;

namespace UserApi.Controllers.UserAdmin
{
    [Route("api/[controller]")]
    public class SoftController : Controller
    {


        [Route("[action]")]
        [HttpPost]
        public List<VpnRpcEnumSessionItem> ListSession()
        {
            return new ProjectLayer.Models.Mod.MServerSoft().ListSession();
        }

        [Route("[action]")]
        [HttpGet]
        public List<VpnRpcEnumSessionItemDublicate> ListSessionDublicate()
        {
            return new ProjectLayer.Models.Mod.MServerSoft().ListSessionDublicate();
        }
    }
}
