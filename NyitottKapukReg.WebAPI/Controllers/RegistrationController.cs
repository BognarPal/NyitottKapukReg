using Microsoft.AspNetCore.Http;
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
    [Route("registration")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {

        private readonly ApplicationDbContext dbContext;

        public RegistrationController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("{id}")]                                     // http://localhost:5000/registration/12
        public ActionResult Get(int id)
        {
           return this.Run(() =>
           {
               var registration = dbContext.Set<Registration>()
                                           .Include(r => r.Day)
                                           .Include(r => r.VisitorGroup)
                                           .Where(r => r.Id == id)
                                           .Select(r => new
                                           {
                                               id = r.Id,
                                               groupNumber = r.VisitorGroup.GroupNumber,
                                               email = r.Email,
                                               parents = r.Parents,
                                               students = r.Students,
                                               date = r.Day.Date.ToString("yyyy.MM.dd"),
                                               classroomNumber = r.VisitorGroup.ClassroomNumber
                                           })
                                           .FirstOrDefault();
                
               //TODO ellenőrzés, hogy a felhasználónak joga van-e lekérdezni ezt a regisztrációt
               if (registration == null)
                   return BadRequest(new
                   {
                       ErrorMessage = "Nem létező regisztráció"
                   });
               return Ok(registration);
           });
        }

        [HttpGet]
        public ActionResult List([FromQuery] DateTime date)   // http://localhost:5000/registration?Date=2022.05.11
        {
            return this.Run(() =>
            {
                var day = dbContext.Set<Day>()
                   .FirstOrDefault(d => d.Date == date);
                if (day == null)
                    return BadRequest(new
                    {
                        ErrorMessage = "A megadott nap nem létezik"
                    });

                var visitors = dbContext.Set<Registration>()
                                        .Include(r => r.Day)
                                        .Include(r => r.VisitorGroup)
                                        .Where(r => r.Day.Date == date)
                                        .ToList()
                                        .Select(r => new
                                        {
                                            id = r.Id,
                                            groupNumber = r.VisitorGroup.GroupNumber,
                                            email = r.Email,
                                            parents = r.Parents,
                                            students = r.Students,
                                            countOfVisitors = r.CountOfVisitors()
                                        })
                                        .OrderBy(r => r.groupNumber).ThenBy(r => r.id);
                var result = new List<dynamic>();
                for (int i = 1; i <= 12; i++)
                {
                    result.Add(new
                    {
                        groupNumber = i,
                        countOfVisitors = visitors.Where(v => v.groupNumber == i).Sum(v => v.countOfVisitors),
                        visitors = visitors.Where(v => v.groupNumber == i)
                                           .Select(v => new
                                           {
                                               v.id,
                                               v.email,
                                               v.parents,
                                               v.students,
                                               v.countOfVisitors
                                           })
                                           .ToArray()
                    });
                }
                return Ok(result);
            });
        }

        [HttpGet("byEmail")]
        public ActionResult GetByEmailAddress([FromQuery] string email)   // http://localhost:5000/registration/byEmail/?email=xy@gmail.com
        {
            return this.Run(() =>
            {
                var registration = dbContext.Set<Registration>()
                                            .Include(r => r.Day)
                                            .Include(r => r.VisitorGroup)
                                            .Where(r => r.Email == email)
                                            .Select(r => new
                                            {
                                                id = r.Id,
                                                groupNumber = r.VisitorGroup.GroupNumber,
                                                email = r.Email,
                                                parents = r.Parents,
                                                students = r.Students,
                                                date = r.Day.Date.ToString("yyyy.MM.dd"),
                                                classroomNumber = r.VisitorGroup.ClassroomNumber
                                            })
                                            .FirstOrDefault();

                //TODO ellenőrzés, hogy a felhasználónak joga van-e lekérdezni ezt a regisztrációt
                if (registration == null)
                    return BadRequest(new
                    {
                        ErrorMessage = "Nem létező regisztráció"
                    });
                return Ok(registration);
            });
        }

        [HttpPut]
        public ActionResult New(Registration registration)
        {
            return this.Run(() =>
            {
                if (dbContext.Set<Registration>().Any(r => r.Email == registration.Email))
                    return BadRequest(new
                    {
                        ErrorMessage = "A megadott e-mail címmel már történt korábban regisztráció"
                    });

                var day = dbContext.Set<Day>()
                                   .FirstOrDefault(d => d.Date == registration.Day.Date);
                if (day == null)
                    return BadRequest(new
                    {
                        ErrorMessage = "A megadott nap nem létezik"
                    });

                var groups = dbContext.Set<Registration>()
                                      .Include(r => r.Day)
                                      .Include(r => r.VisitorGroup)
                                      .Where(r => r.Day.Id == day.Id)
                                      .ToList()
                                      .GroupBy(r => r.VisitorGroup.GroupNumber)
                                      .Select(r => new
                                      {
                                          groupNumber = r.Key,
                                          visitorCount = r.Sum(v => v.CountOfVisitors())
                                      })
                                      .OrderBy(g => g.visitorCount)
                                      .ThenBy(g => g.groupNumber)
                                      .ToList();


                VisitorGroup visitorGroup = null;
                if (groups.Count() < 12)
                {
                    for (int i = 1; i <= 12; i++)
                        if (!groups.Any(g => g.groupNumber == i))
                        {
                            visitorGroup = new VisitorGroup()
                            {
                                Day = day,
                                GroupNumber = i,
                                ClassroomNumber = dbContext.Set<VisitorGroup>()
                                                           .First(v => v.Day == null && v.GroupNumber == i)
                                                           .ClassroomNumber
                            };
                            dbContext.Set<VisitorGroup>().Add(visitorGroup);
                            break;
                        }
                }
                else
                    visitorGroup = dbContext.Set<VisitorGroup>()
                                            .FirstOrDefault(v => v.Day == day &&
                                                                 v.GroupNumber == groups.First().groupNumber);
                if (visitorGroup == null)
                    return BadRequest(new
                    {
                        ErrorMessage = "Vártatlan hiba a csoportba sorolás során"
                    });

                registration.Password = GeneratePassword();
                registration.Day = day;
                registration.VisitorGroup = visitorGroup;                
                dbContext.Set<Registration>().Add(registration);
                dbContext.SaveChanges();

                return Ok(new
                {
                    groupNumber = visitorGroup.GroupNumber,
                    classRoom = visitorGroup.ClassroomNumber,
                    date = registration.Day.Date.ToString("yyyy.MM.dd"),
                    password = registration.Password
                });
            });
        }

        [HttpPost]
        public ActionResult Modify(Registration registration)
        {
            //TODO megcsinálni -> Jogosultság
            return this.Run(() =>
            {
                return Ok();
            });
        }

        [HttpDelete]
        public ActionResult Delete(Registration registration)
        {
            //TODO megcsinálni -> Jogosultság
            return this.Run(() =>
            {
                return Ok();
            });
        }

        private string GeneratePassword(int numbers = 2, int lowers = 4, int uppers = 2)
        {
            var asciiCodes = new List<int>();
            for (int i = 48; i <= 57; i++)
                asciiCodes.Add(i);
            for (int i = 65; i <= 90; i++)
                asciiCodes.Add(i);
            for (int i = 97; i <= 122; i++)
                asciiCodes.Add(i);

            var pwd = "";
            var pwdLength = numbers + lowers + uppers;
            while (pwd.Length < pwdLength)
            {
                var r = asciiCodes[this.NewRandom(asciiCodes.Count)];
                if (r <= 57)
                {
                    if (numbers > 0)
                    {
                        numbers--;
                        pwd += (char)r;
                    }
                }
                else if (r >= 97)
                {
                    if (lowers > 0)
                    {
                        lowers--;
                        pwd += (char)r;
                    }
                }
                else
                {
                    if (uppers > 0)
                    {
                        uppers--;
                        pwd += (char)r;
                    }
                }
            }
            return pwd;
        }


    }
}
