namespace GorevYonetimSistemi.Business.Dtos.User.Auth
{
    public class AuthResponseDto
{
    public string? Token { get; set; }  // JWT Token veya başka bir kimlik doğrulama token'ı
    public string? Message { get; set; } // Başarı veya hata mesajı
    public Guid UserId { get; set; }    // Kullanıcının unique ID'si
    public string? Email { get; set; }   // Kullanıcının e-posta adresi
}

}