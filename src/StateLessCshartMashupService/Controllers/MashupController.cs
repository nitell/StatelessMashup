using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using static StateLessCshartMashupService.Controllers.Serve;

namespace StateLessCshartMashupService.Controllers
{
    [Route("api/[controller]")]
    public class MashupController : Controller
    {       
        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<object> Get(string id)
        {
            return await ServeArtist(id) ?? (object)HttpNotFound();
        }     
    }
}
