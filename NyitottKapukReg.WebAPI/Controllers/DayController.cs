using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                                            id = d.Id,
                                            date = d.Date,
                                            shortDate = d.Date.ToString("yyyy.MM.dd"),
                                            longDate = d.Date.ToString("yyyy MMMM dd."),
                                            maxVisitors = d.MaxVisitors
                                         }
                                   )
               );
           });
        }

        [HttpPut]
        public ActionResult New(Day model)
        {
            return this.Run(() =>
            {
                dbContext.Set<Day>().Add(model);
                dbContext.SaveChanges();

                return Ok(model);
            });
        }

        [HttpPost]
        public ActionResult Modify(Day model)
        {
            return this.Run(() =>
            {
                dbContext.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                dbContext.SaveChanges();

                return Ok(model);
            });
        }

        [HttpDelete]
        public ActionResult Delete(Day model)
        {
            return this.Run(() =>
            {
                var exists = dbContext.Set<Registration>()
                                      .Include(r => r.Day)
                                      .Any(r => r.Day.Id == model.Id);
                if (exists)
                {
                    return StatusCode(500, new
                    {
                        ErrorMessage = "A törlendő napra már van regisztráció"
                    });
                }
                else
                {
                    dbContext.Remove(model);
                    dbContext.SaveChanges();

                    return Ok(model);
                }
            });
        }
    }
}
