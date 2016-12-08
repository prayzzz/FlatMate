using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FlatMate.Web.Areas.Account.Dto;

namespace FlatMate.Web.Areas.Lists.Dto
{
    public class ListDto
    {
        [Display(Name = "Erstellungsdatum")]
        public DateTime? CreationDate { get; internal set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Display(Name = "Id")]
        public int Id { get; internal set; }

        [Display(Name = "Gruppen")]
        public List<GroupDto> Groups { get; set; } = new List<GroupDto>();

        [Display(Name = "Öffentlich?")]
        public bool? IsPublic { get; set; }

        [Display(Name = "Bearbeitungsdatum")]
        public DateTime? ModifiedDate { get; internal set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Ersteller")]
        public UserInfoDto Owner { get; internal set; }
    }

    public class ListUpdateDto
    {
        [Required]
        [Display(Name = "Beschreibung")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Öffentlich?")]
        public bool? IsPublic { get; set; } = false;

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;
    }
}