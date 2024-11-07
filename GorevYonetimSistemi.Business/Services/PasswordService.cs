using BCrypt.Net;

namespace GorevYonetimSistemi.Business.Services
{
    public class PasswordService
    {
        // Şifreyi hash'leyerek döndüren metot
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);  // HashPassword doğru kullanımı
        }

        // Şifrenin doğruluğunu kontrol etme
        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);  // Verify doğru kullanımı
        }
    }
}
