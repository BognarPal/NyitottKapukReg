using Microsoft.AspNetCore.Mvc;
using NyitottKapukReg.Service;
using NyitottKapukReg.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NyitottKapukReg.WebAPI.Controllers
{
    [ApiController]
    [Route("days")]
    public class DayController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public DayController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult List()
        { 
           return this.Run(() =>
           {
               return Ok(
                   dbContext.Set<Day>().OrderBy(d => d.Date)
                            .Select(d => new
                                         {
                                            date = d.Date,
                                            shortDate = d.Date.ToString("yyyy.MM.dd"),
                                            longDate = d.Date.ToString("yyyy MMMM dd.")
                                         }
                                   )
               );
           });
        }

        //TODO: hozzon létre újabb függvényeket: CRUD
    }
}
