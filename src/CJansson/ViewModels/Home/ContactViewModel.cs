using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CJansson.ViewModels.Home
{
    public class ContactViewModel
    {
        public string GoogleRecaptchaKey { get; set; }

        [Required]
        [Display(Name = "Name", Prompt = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email", Prompt = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Message", Prompt = "Message")]
        public string Text { get; set; }

        [Display(Name = "Source", Prompt = "Source")]
        public string Source { get; set; }
    }
}
