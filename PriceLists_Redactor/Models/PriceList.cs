using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PriceLists_Redactor.Models
{
    public class PriceList
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Заполните поле")]
        public string Name { get; set; }
    }
}