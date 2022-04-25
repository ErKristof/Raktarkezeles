using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Raktarkezeles.API.Data;
using Raktarkezeles.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raktarkezeles.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RaktarkezelesController : ControllerBase
    {
        private readonly AlkatreszContext _context;

        public RaktarkezelesController(AlkatreszContext context)
        {
            _context = context;
        }
        //[HttpGet]
        //[Route("/Alkatreszek")]
        //public async Task<ActionResult<List<int>>> GetAlkatreszIds()
        //{

        //}
    }
}
