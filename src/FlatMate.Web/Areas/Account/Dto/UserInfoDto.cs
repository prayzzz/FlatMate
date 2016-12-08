using System.ComponentModel.DataAnnotations;

namespace FlatMate.Web.Areas.Account.Dto
{
    public class UserInfoDto
    {
        public static UserInfoDto Default => new UserInfoDto {Id = -1};

        [Editable(false)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Editable(false)]
        [Display(Name = "Nutzername")]
        public string UserName { get; set; }
    }
}