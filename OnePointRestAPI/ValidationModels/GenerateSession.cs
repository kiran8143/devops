using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnePointRestAPI.ValidationModels
{
    /// 
    /// API object
    /// 
    public class GenerateSession
    {
        /// 
        /// ## Password - Remarks ## 
        /// a secret word or phrase that must be issued for user
        /// 
        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        /// 
        /// ## AccountNumber - Remarks ## 
        /// Unique Identifier issued for user which follows a pattern MCNXXXXXXX
        /// 
        [Required]
        [StringLength(20)]
        // [RegularExpression("(?<first>MCN)*")]
        [MCNCheck]
        public string AccountNumber { get; set; }

        /// 
        /// ## UserName - Remarks ## 
        /// UserName issued for user during registration
        /// 
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }
    }
}
