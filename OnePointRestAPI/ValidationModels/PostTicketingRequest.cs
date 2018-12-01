using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnePointRestAPI.ValidationModels
{

    public class passenger
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string title { get; set; }
        public string eTicket { get; set; }
        public PassengerType passengerType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum PassengerType
        {
           
            ADT,
            CHD,
            INF,
            SEA,
            SRC,
            MRE,
            LBR,
            VAC,
            STU
        }
    }
    public class PostTicketingRequest
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PTRType
        {
            None,
            VoidQuote,
            Void,
            //RefundQuote,
            //Refund,
            ReIssueQuote,
            ReIssue,
            //ASCRefund,
            //ASCExchange,
            //PTRQueue
        }
       [EnumDataType(typeof(PTRType), ErrorMessage = "PTRType value doesn't exist within enum")]
       [Required]
        public PTRType ptrType { get; set; }

        [MFRefCheck]
        public string mFRef { get; set; }
        internal int clientId { get; set; }
        internal int memberId { get; set; }
        public List<passenger> passengers { get; set; }
        public SegmentPreference segmentPreferences { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (((ptrType.ToString()== "ReIssueQuote")|| (ptrType.ToString() == "ReIssue")) 
                && segmentPreferences!=null)
                yield return new ValidationResult("segmentPreferences is required.");
        }

        public class SegmentPreference
        {
            public List<Preference> preferences { get; set; }
        }

        public class Preference
        {
            public int option { get; set; }
            public List<Segment> segments { get; set; }
        }

        public class Segment
        {
            public string origin { get; set; }
            public string destination { get; set; }
            //public int SegmentRefNo { get; set; }
            public string cabinClass { get; set; }
            public DateTime departureDateTime { get; set; }
            //public DateTime DepartureTime { get; set; }
            public string cityPair { get; set; }
        }
    }

    public class SearchPostTicketingRequest
    {

        //[StringLength(20)]
        //public string ClientId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PTRType
        {
            None,
            VoidQuote,
            Void,
            //RefundQuote,
           // Refund,
            ReIssueQuote,
            ReIssue,
            //ASCRefund,
            //ASCExchange,
            //PTRQueue
        }
        [EnumDataType(typeof(PTRType), ErrorMessage = "PTRType value doesn't exist within enum")]
        [Required]
        public PTRType ptrType { get; set; }

        [StringLength(20)]
        [MFRefCheck]
        public string MFRef { get; set; }

       
       
        public ptrCategory PTRCategory { get; set; }

        public ptrStatus PTRStatus { get; set; }


        
        //[DefaultValue(01)]
       // [PTRIdCheck]
        public uint PTRId { get; set; }

        [DefaultValue(1)]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please enter valid page greater than 0.")]      
        public int Page { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum ptrCategory
        {
            None,
            NotRead,
            Read,
            All
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum ptrStatus
        {
            None,
            Requested,
            InProcess,
            Completed,
            Deleted,
            Rejected,
            QuoteChanged
        }
    }

    public class MarkAsRead
    { 
        [Required(ErrorMessage = "Please enter Valid Id")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please enter valid id greater than 0.")]
        public int id { get; set; }

      
        [Required]
        //[EnumDataType(typeof(PTRType), ErrorMessage = "PTRType value doesn't exist within enum")]        
        public PTRType requestType { get; set; }

        internal int clientId { get; set; }

        [StringLength(20)]
        [MFRefCheck]
        [Required]
        public string MFRef { get; set; }

       
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PTRType
        {
            None,
            VoidQuote,
            Void,
            //RefundQuote,
           // Refund,
            ReIssueQuote,
            ReIssue,
            //ASCRefund,
            //ASCExchange,
            //PTRQueue
        }
    }



}


//[Required(ErrorMessage = "Please enter how many Stream Entries are displayed per page.")]
//[Range(0, 250, ErrorMessage = "Please enter a number between 0 and 250.")]
//[Column]
//[DataAnnotationsExtensions.Integer(ErrorMessage = "Please enter a valid number.")] 