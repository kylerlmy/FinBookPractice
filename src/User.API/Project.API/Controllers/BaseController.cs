using Microsoft.AspNetCore.Mvc;
using Project.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Controllers
{
    public class BaseController : Controller
    {
        protected UserIdentity UserIdentity
        {
            get
            {
                var identity = new UserIdentity();

                var claims = User.Claims;
                //获取 Claims
                if (claims.Any())
                {
                    identity.UserId = Convert.ToInt32(claims.FirstOrDefault(c => c.Type == "sub").Value);
                    identity.Name = claims.FirstOrDefault(c => c.Type == "name").Value;
                    identity.Company = claims.FirstOrDefault(c => c.Type == "company").Value;
                    identity.Title = claims.FirstOrDefault(c => c.Type == "title").Value;
                    identity.Avatar = claims.FirstOrDefault(c => c.Type == "avatar").Value;
                }

                return identity;
            }
        }
    }
}
