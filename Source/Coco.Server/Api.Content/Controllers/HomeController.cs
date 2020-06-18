﻿using Microsoft.AspNetCore.Mvc;

namespace Api.Content.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET api/user
        [HttpGet, Route("")]
        public IActionResult Index()
        {
            return Content("Content Api");
        }
    }
}
