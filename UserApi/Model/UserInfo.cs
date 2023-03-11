using System.ComponentModel.DataAnnotations;

namespace UserApi.Model
{
    public class UserInfo
    {
        [Key]
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool marketingConsent { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
