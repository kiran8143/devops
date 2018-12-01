using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnePointRestAPI.ValidationModels
{
    public class Invoice
    {
        //[StringLength(20)]
        //public string ClientId { get; set; }
        
        [DefaultValue(1)]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please enter valid page greater than 0.")]
        public int Page { get; set; }
    }
}
