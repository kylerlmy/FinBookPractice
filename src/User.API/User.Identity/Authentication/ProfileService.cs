using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity.Authentication
{
    public class ProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.FirstOrDefault(o => o.Type == "sub")?.Value;

            if (!int.TryParse(subjectId, out int _))
                throw new ArgumentException("Invalid subject identifier");
            context.IssuedClaims = context.Subject.Claims.ToList();

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.FirstOrDefault(o => o.Type == "sub")?.Value;

            context.IsActive = int.TryParse(subjectId, out int _);

            return Task.CompletedTask;
        }
    }
}
