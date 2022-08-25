using System.Text.Encodings.Web;

namespace CrmBox.WebUI.TwoFactorAuth
{
    public class TwoFactorService
    {
        // 2 faktörlü kimlik doğrulama eklenirse bu servis kullanılacak.


        private readonly UrlEncoder _urlEncoder;
        public TwoFactorService(UrlEncoder urlEncoder)
        {
            _urlEncoder = urlEncoder;   
        }
        public int GetCodeVerification()
        {
            Random rnd = new Random();
            return rnd.Next(1000, 9999);
        }

    }
}
