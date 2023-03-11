using System.ComponentModel.DataAnnotations;

namespace UserApi.ViewModel
{
    public class RegisterVM
    {
        [Required]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public bool marketingConsent { get; set; }=false;
        public Model.UserInfo Map()
        {
            return new Model.UserInfo()
            {
                UserId= Guid.NewGuid().ToString(),
                CreatedDate = DateTime.Now,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,    
                UserName = UserName,    
                marketingConsent = marketingConsent,
                Password = Password
                
            };
        }
    }
}
