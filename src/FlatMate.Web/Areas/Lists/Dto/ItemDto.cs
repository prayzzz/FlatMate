using System;
using System.ComponentModel.DataAnnotations;
using FlatMate.Web.Areas.Account.Dto;

namespace FlatMate.Web.Areas.Lists.Dto
{
    public class ItemDto
    {
        [Display(Name = "Erstellungsdatum")]
        public DateTime CreationDate { get; internal set; }

        [Display(Name = "Id")]
        public int Id { get; internal set; }

        [Display(Name = "Letzter Bearbeiter")]
        public UserInfoDto LastEditor { get; internal set; }

        [Display(Name = "Bearbeitungsdatum")]
        public DateTime LastModified { get; internal set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Reihenfolge")]
        public int Order { get; set; }

        [Display(Name = "Ersteller")]
        public UserInfoDto Owner { get; internal set; }
    }

    public class ItemUpdateDto
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Reihenfolge")]
        public int Order { get; set; }

        [Display(Name = "Letzter Bearbeiter")]
        internal int LastEditorId { get; set; }
    }
}