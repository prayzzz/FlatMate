using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FlatMate.Common;
using FlatMate.Web.Areas.Account.Dto;

namespace FlatMate.Web.Areas.Lists.Dto
{
    public class GroupDto
    {
        [Display(Name = "Erstellungsdatum")]
        public DateTime CreationDate { get; internal set; }

        [Display(Name = "Id")]
        public int Id { get; internal set; }

        [Display(Name = "Einträge")]
        public List<ItemDto> Items { get; set; } = new List<ItemDto>();

        [Display(Name = "Letzter Bearbeiter")]
        public UserInfoDto LastEditor { get; internal set; }

        [Display(Name = "Bearbeitungsdatum")]
        public DateTime ModifiedDate { get; internal set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Reihenfolge")]
        public int Order { get; set; }

        [Display(Name = "Ersteller")]
        public UserInfoDto Owner { get; internal set; }
    }

    public class GroupUpdateDto
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Reihenfolge")]
        public int Order { get; set; }

        internal int LastEditorId { get; set; }
    }
}