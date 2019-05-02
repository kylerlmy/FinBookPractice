using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Data;
using Contact.API.Models;
using Contact.API.Service;
using Contact.API.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Contact.API.Controllers
{
    [Route("api/Contacts")]
    //[ApiController]
    public class ContactController : BaseController
    {
        private IContactApplyRequestRepository _contactApplyRequestRepository;

        private IContactRepository _contactRepository;
        private IUserService _userService;
        public ContactController(IContactApplyRequestRepository contactApplyRequestRepository,
            IUserService userService,
            IContactRepository contactRepository)
        {
            _contactApplyRequestRepository = contactApplyRequestRepository;
            _userService = userService;
            _contactRepository = contactRepository;
        }

        [HttpGet]   
        [Route("")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            return Ok(await _contactRepository.GetContactsAsync(UserIdentity.UserId, cancellationToken));
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> Get(int userId,CancellationToken cancellationToken)
        {
            return Ok(await _contactRepository.GetContactsAsync(userId, cancellationToken));
        }

        [HttpPut]
        [Route("tag")]
        public async Task<IActionResult> TagContact([FromBody]TagContactViewModel tagContactViewModel,CancellationToken cancellationToken)
        {
           var result=await _contactRepository.TagContactAsync(UserIdentity.UserId, tagContactViewModel.ContactId, tagContactViewModel.Tags, cancellationToken);

            if (result)
            {
                return Ok();
            }

            //LOG TBD

            return BadRequest();
        }
        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("apply-requests")]
        public async Task<IActionResult> GetApplyRequest(CancellationToken cancellationToken)
        {
            var requst = await _contactApplyRequestRepository.GetRequestListAsync(UserIdentity.UserId, cancellationToken);
            return Ok(requst);
        }
        /// <summary>
        /// 添加好友请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("apply-requests/{userId}")]
        public async Task<IActionResult> AddApplyRequest(int userId, CancellationToken cancellationToken)
        {
            /*
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
                ApplyTime = DateTime.Now,
                Title = userBaseInfo.Title,
                Avatar = userBaseInfo.Avatar

            }, cancellationToken);

            if (!result)
            {
                //log tdb
                return BadRequest();
            }

            return Ok();

    */

            var result = await _contactApplyRequestRepository.AddRequestAsync(new ContactApplyRequest
            {
                UserId = userId,
                ApplierId = UserIdentity.UserId,
                Name = UserIdentity.Name,
                Company = UserIdentity.Company,
                HandelTime = DateTime.Now,
                ApplyTime = DateTime.Now,
                Title = UserIdentity.Title,
                Avatar = UserIdentity.Avatar

            }, cancellationToken);

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
        [Route("apply-requests/{applierId}")]//注意此处的applierid不要写成userid，否则接受到的applierId的值将是0
        public async Task<IActionResult> ApprovalApplyRequest(int applierId, CancellationToken cancellationToken)
        {
            var result = await _contactApplyRequestRepository.ApprovalAsync(UserIdentity.UserId, applierId, cancellationToken);

            if (!result)    
            {
                //log tdb
                return BadRequest();
            }


            var applier = await _userService.GetBaseUseInfoAsync(applierId);
            var userInfo = await _userService.GetBaseUseInfoAsync(UserIdentity.UserId);

            await _contactRepository.AddContactAsync(UserIdentity.UserId, applier, cancellationToken);
            await _contactRepository.AddContactAsync(applierId, userInfo , cancellationToken);


            return Ok();
        }
    }
}
