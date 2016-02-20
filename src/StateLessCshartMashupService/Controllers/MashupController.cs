﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using static StateLessCshartMashupService.Controllers.Serve;

namespace StateLessCshartMashupService.Controllers
{
    [Route("api/[controller]")]
    public class MashupController : Controller
    {       
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return JsonConvert.SerializeObject(ServeMbid(id).Result);
        }        
    }
}