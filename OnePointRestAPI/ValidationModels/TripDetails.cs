using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnePointRestAPI.ValidationModels
{

    public class SearchTripDetails
    {
       
       
        [StringLength(20)]
        // [RegularExpression("(?<first>MF)*")]
        [MFRefCheck]
        [Required]
        public string MFRef { get; set; }

     
    }
}
