using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Data;
using Contact.API.Models;
using Contact.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace Contact.API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ContactController : BaseController
    {
        private IContactApplyRequestRepository _contactApplyRequestRepository;
        private IUserService _userService;
        public ContactController(IContactApplyRequestRepository contactApplyRequestRepository,
            IUserService userService)
        {
            _contactApplyRequestRepository = contactApplyRequestRepository;
            _userService = userService;
        }

        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("apply-requests")]
        public async Task<IActionResult> GetApplyRequest()
        {
            var requst = await _contactApplyRequestRepository.GetRequestListAsync(UserIdentity.UserId);
            return Ok(requst);
        }
        /// <summary>
        /// 添加好友请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("apply-requests")]
        public async Task<IActionResult> AddApplyRequest(int userId)
        {
            var userBaseInfo = await _userService.GetBaseUseInfoAsync(userId);

            if (userBaseInfo == null)
            {
                throw new Exception("用户参数错误");
            }

            var result = await _contactApplyRequestRepository.AddRequestAsync(new ContactApplyRequest
            {
                UserId = userId,
                ApplierId = UserIdentity.UserId,
                Name = userBaseInfo.Name,
                Company = userBaseInfo.Company,
                CreateTime = DateTime.Now,
                Title = userBaseInfo.Title,
                Avatar = userBaseInfo.Avatar

            });

            if (!result)
            {
                //log tdb
                return BadRequest();
            }

            return Ok();

        }


        /// <summary>
        /// 通过好友请求
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("apply-requests")]
        public async Task<IActionResult> ApprovalApplyRequest(int applierId)
        {
            var result = await _contactApplyRequestRepository.ApprovalAsync(applierId);

            if (!result)
            {
                //log tdb
                return BadRequest();
            }

            return Ok();
        }
    }
}
