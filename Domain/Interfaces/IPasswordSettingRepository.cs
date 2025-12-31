using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Interfaces
{
    public interface IPasswordSettingRepository
    {
        Task OtpVerification(string email,string otp);

        Task ChangePassword(string email, string newPassword);
    }
}
