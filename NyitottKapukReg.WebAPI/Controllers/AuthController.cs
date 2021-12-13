using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NyitottKapukReg.Service;
using NyitottKapukReg.Service.Auth;
using NyitottKapukReg.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NyitottKapukReg.WebAPI.Controllers
{
    [Route("registration/login")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration config;
        public AuthController(IConfiguration config, ApplicationDbContext dbContext)
        {
            this.config = config;
            this.dbContext = dbContext;
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            return this.Run(() =>
            {
                if (!string.IsNullOrEmpty(model.Email))
                    return VisitorLoginByEmail(model);
                else
                    return AdLogin(model);
            });
        }

        private ActionResult VisitorLoginByEmail(LoginModel model)
        {
            var registration = dbContext.Set<Registration>()
                            .FirstOrDefault(r => r.Email == model.Email && r.Password == model.Password);
            if (registration == null)
            {
                return Unauthorized(new
                {
                    errorMessage = "Hibás e-mail cím vagy jelszó"
                });
            }
            var jwt = new JwtService(config);
            var token = jwt.GenerateSecurityToken(model.Email, new List<Claim>() { new Claim("RegistrationId", registration.Id.ToString()) });

            return Ok(new
            {
                token = token
            });
        }

        private ActionResult AdLogin(LoginModel model)
        {
            //TODO: megcsinálni
            return Ok();
        }

    }
}
