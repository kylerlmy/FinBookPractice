using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using User.Identity.Services;

namespace User.Identity.Authentication
{
    public class SmsAuthCodeValidator : IExtensionGrantValidator
    {

        private readonly IAuthCodeService _authCodeService;
        private readonly IUserService _userService;

        public SmsAuthCodeValidator(IAuthCodeService authCodeService, IUserService userService)
        {
            _authCodeService = authCodeService;
            _userService = userService;
        }
        public string GrantType => "sms_auth_code";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw["phone"];
            var code = context.Request.Raw["auth_code"];

            //由于这里定义的是InvalidGrant，所以再请求时，无论时什么错误都会抛出InvalidGrant错误
            var errorValidationResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);


            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(code))
                context.Result = errorValidationResult;
            //检查验证码
            if (!_authCodeService.Validate(phone, code))
            {
                context.Result = errorValidationResult;
                return;
            }

            //完成用户注册
            var userInfo = await _userService.CheckOrCreate(phone);//如果请求路径不对，例如在UserService的_userServiceUrl的变量中忘记添加http://执行到这里就不会继续进行了，并在postman中提示 "error": "invalid_grant"

            if (userInfo == null)
            {
                context.Result = errorValidationResult;
                return;
            }

            var claims = new Claim[] {
                new Claim("name",userInfo.Name??string.Empty),
                new Claim("company",userInfo.Company??string.Empty),
                new Claim("title",userInfo.Title??string.Empty),
                new Claim("avatar",userInfo.Avatar??string.Empty),
            };

            //context.Result = new GrantValidationResult(userId.ToString(), GrantType);

            context.Result = new GrantValidationResult(userInfo.UserId.ToString(), GrantType, claims);

        }
    }
}
