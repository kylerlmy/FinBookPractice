using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.JsonPatch;

namespace User.API.Controllers
{
    [Route("api/users")]
    //[ApiController]
    public class UserController : BaseController //ControllerBase
    {
        private UserContext _userContext;

        private ILogger<UserController> _logger;
        public UserController(UserContext userContext, ILogger<UserController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var user = await _userContext.Users
                  .AsNoTracking()//不再进行状态跟踪，节省EF开销
                  .Include(u => u.Properties)
                  .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);


            if (user == null)
            {
                throw new UserOperationException($"错误的用户上下文Id{UserIdentity.UserId}");
            }

            return Json(user);
        }


        [Route("")]
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody]JsonPatchDocument<Models.AppUser> patch)//Json请求，需要加FromBody
        {
            var user = await _userContext.Users
                // .Include(o=>o.Properties) 导致获取的修改前的Properties与修改后的Properties内容相同
                .SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);

            patch.ApplyTo(user);

            foreach (var property in user?.Properties)
            {
                _userContext.Entry(property).State = EntityState.Detached;//全部设置为不跟踪，这种通过postman等传递过来的数据的状态不和EF的状态相关的
            }

            //var originProperties = user.Properties;//通过这种方法，导致获取的originProperties与修改后的内容相同

            var originProperties = await _userContext.UserProperties
                //.AsNoTracking() //不取消，将导致在调用Remove方法时，抛出异常（The instance of entity type 'UserProperty' cannot be tracked because another instance with the same key value for {'Key', 'AppUserId', 'Value'} is already being tracked.）
                .Where(u => u.AppUserId == UserIdentity.UserId).ToListAsync();
            var allProperties = originProperties.Union(user.Properties).Distinct();

            var removeProperties = originProperties.Except(user.Properties);
            //var newProperties = allProperties.Except(user.Properties);
            var newProperties = allProperties.Except(originProperties);

            foreach (var property in removeProperties)
            {
                //_userContext.Entry(property).State = EntityState.Deleted;

                _userContext.Remove(property);
            }

            foreach (var property in newProperties)
            {
                //_userContext.Entry(property).State = EntityState.Added;

                _userContext.Add(property);
            }

            _userContext.Users.Update(user);
            _userContext.SaveChanges();

            return Json(user);
        }


        /// <summary>
        /// 检查或创建用户（当用户手机号不存在的时候创建用户）
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [Route("check-or-create")]
        [HttpPost]
        public async Task<IActionResult> CheckOrCreate(string phone)
        {
            //TODO: 做手机号码的格式验证

            var user = await _userContext.Users.SingleOrDefaultAsync(u => u.Phone == phone);
            if (user == null)//await _userContext.Users.AnyAsync(u => u.Phone == phone)
            {
                user = new Models.AppUser { Phone = phone };
                _userContext.Users.Add(user);
               await _userContext.SaveChangesAsync();
                //_userContext.Users.Add(new Models.AppUser { Phone = phone });
            }

            return Ok(user.Id);
        }
    }
}
