

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.ResponseHandler.Models;



namespace Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ApiControllerBase
    {
      

        #region ctor
        public TestController()
        { 
        }
        #endregion

      
        [Authorize]
        [Route("test")]
        [HttpPost]
        public async Task<IActionResult> Test()
        {

            return Ok("test");
        }
        

     
    }
}
