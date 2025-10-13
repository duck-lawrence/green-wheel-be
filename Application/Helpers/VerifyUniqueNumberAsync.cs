using Application.AppExceptions;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Constants;

namespace Application.Helpers
{
    public static class VerifyUniqueNumberAsync
    {
        public static async Task VerifyUniqueIdentityNumberAsync(string number, Guid userId, ICitizenIdentityRepository repo)
        {
            if (string.IsNullOrEmpty(number))
                throw new BadRequestException(Message.UserMessage.InvalidDriverLicenseData);
            var exiting = await repo.GetByIdNumberAsync(number);
            if (exiting == null) return;
            if (exiting.UserId == userId) return;
            if (exiting.DeletedAt == null)
                throw new ConflictDuplicateException(Message.UserMessage.CitizenIdentityDuplicate);
        }

        public static async Task VerifyUniqueDriverLicenseNumberAsync(string number, Guid userId, IDriverLicenseRepository repo)
        {
            if (string.IsNullOrEmpty(number)) throw new BadRequestException(Message.UserMessage.InvalidDriverLicenseData);
            var exiting = await repo.GetByLicenseNumber(number);
            if (exiting == null) return;
            if (exiting.UserId == userId) return;
            if (exiting.DeletedAt == null) throw new ConflictDuplicateException(Message.UserMessage.DriverLicenseDuplicate);
        }
    }
}