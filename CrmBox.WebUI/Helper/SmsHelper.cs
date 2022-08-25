using CrmBox.WebUI.TwoFactorAuth;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CrmBox.WebUI.Helper
{
    public class SmsHelper
    {
        private readonly TwoFactorService _twoFactorService;
        public SmsHelper(TwoFactorService twoFactorService)
        {
            _twoFactorService= twoFactorService;
        }
        
        public string SendSmsForTwoFactor(string phone)
        {
            string code = _twoFactorService.GetCodeVerification().ToString();

            //sms sağlayıcı kodları buraya gelecek.(turkcell, vodofone gibi)

            return code;
        }
        public string SendSmsForMessages(string message)
        {

            //sms sağlayıcı kodları buraya gelecek.(turkcell, vodofone gibi)

            return message;
        }
    }
}
