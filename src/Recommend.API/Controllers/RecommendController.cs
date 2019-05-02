using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recommend.API.Data;

namespace Recommend.API.Controllers
{
    [Route("api/recommend")]
    [ApiController]
    public class RecommendController : BaseController
    {

        private RecommendDbContext _dbContext;
        public RecommendController(RecommendDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        // GET api/values
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var result = await _dbContext.Recommends.AsNoTracking()
                  .Where(r => r.UserId == UserIdentity.UserId).ToListAsync();

            return Ok(result);
        }
    }
}
