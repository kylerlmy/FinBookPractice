using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.API.Dtos;

namespace User.API.Controllers {
    public class BaseController : Controller {
        protected UserIdentity UserIdentity {
            get {
                var identity = new UserIdentity () { UserId = 1 };

                var claims = User?.Claims;

                if (claims != null && claims.Any ()) {
                    identity.UserId = Convert.ToInt32 (claims.FirstOrDefault (c => c.Type == "sub").Value);
                    identity.Name = claims.FirstOrDefault (c => c.Type == "name").Value;
                    identity.Company = claims.FirstOrDefault (c => c.Type == "company").Value;
                    identity.Title = claims.FirstOrDefault (c => c.Type == "title").Value;
                    identity.Avatar = claims.FirstOrDefault (c => c.Type == "avatar").Value;
                }

                return identity;
            }
        }
    }
}