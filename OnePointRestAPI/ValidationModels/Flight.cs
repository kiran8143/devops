using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnePointRestAPI.ValidationModels
{
    public class Flight
    {
        [Required]
        public IList<onDInfosList> onDInfos { get; set; }
        [Required]
        public IList<PaxData> paxData { get; set; }
        [Required]
        public string nationality { get; set; }
        [Required]
        public PricingType pricingType { get; set; }
        [Required]
        public uint branchId { get; set; }
        [Required]
        public Boolean isResidentFare { get; set; }
        [Required]
        public Boolean isRefundable { get; set; }
        [Required]
        internal int clientId { get; set; }
        internal int memberId { get; set; }
        [Required]
        public int MyProperty { get; set; }
        [Required]
        public Preferences preferences { get; set; }
    }
public class PaxData
{
        [Required]
        public PaxType paxType { get; set; }
        [Required]
        public uint quantity { get; set; }
}
public class Preferences
{
        [Required]
        public CabinType cabinType { get; set; }
        [Required]
        public TripType tripType { get; set; }
        [Required]
        public StopsQuantity stopsQuantity { get; set; }
        [Required]
        public string preferredAirlines { get; set; }
        [Required]
        public CabinPreference cabinPreference { get; set; }
}

public class onDInfosList
{
        [Required]
        public string departureDateTime { get; set; }
        [Required]
        public string origin { get; set; }
        [Required]
        public string destination { get; set; }
}
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PricingType { Default, Public, Private, All }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StopsQuantity { Default, Direct, OneStop, All }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CabinType { Default, Y, S, C, J, F, P }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TripType { Default, OneWay, Return, Circle, OpenJaw, Other }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CabinPreference { Default, Restricted, Preferred }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaxType { Default, ADT, CHD, INF, SEA, SRC, MRE, LBR, VAC, STU }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RequestOptions { Default, Fifty, Hundred, TwoHundred }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PenaltyType { Cancel, Exchange }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FareType { Default, Public, Private, WebFare }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum IsRefundable { Yes, No, InfoNotAvailable }


    public class RevalidateFlight {
        [Required]
        public string FareSourceCode { get; set; }
        [Required]
        public uint branchId { get; set; }
        internal int clientId { get; set; }
        internal int memberId { get; set; }
    }





    public class FlightFareRules
    {
        [Required]
        public string FareSourceCode { get; set; }
        [Required]
        public string UniqueID { get; set; }
        [Required]
        public string ConversationId { get; set; }
        [Required]
        public uint branchId { get; set; }
        internal int clientId { get; set; }
        internal int memberId { get; set; }
    }

    

    public class OrderTicket
    {
        [Required]
        public string FareSourceCode { get; set; }
        [Required]
        public uint branchId { get; set; }

        internal int clientId { get; set; }
        internal int memberId { get; set; }
    }


    [Serializable]
    public class BookFlight
    {
        internal int clientId { get; set; }
        internal int memberId { get; set; }
        [Required]
        public int BranchId { get; set; }
        [Required]
        public string FareSourceCode{ get; set; }
        [Required]
        public string PaymentTransactionID{ get; set; }
        [Required]
        public decimal ClientMarkup { get; set; }
        [Required]
        public TravelerInfo TravelerInfo { get; set; }
        [Required]
        public PaymentCardInfo PaymentCardInfo { get; set; }
        [Required]
        public PaymentReferences PaymentReferences { get; set; }
        [Required]
        public string ClientReferenceNo { get; set; }
        [Required]
        public string ConversationId { get; set; }
        [Required]
        public bool LccHoldBooking { get; set; }
    }
    [Serializable]
    public class TravelerInfo
    {
        [Required]
        public List<AirTraveler> AirTravelers { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        public string AreaCode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PostCode { get; set; }
    }
    [Serializable]
    public class AirTraveler
    {
        [Required]
        public string PaxType { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public PassengerName PassengerName { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public Passport Passport { get; set; }
        [Required]
        public ResidentFareDocumentType ResidentFareDocumentType { get; set; }
        [Required]
        public string FrequentFlyerNumber { get; set; }
        [Required]
        public SpecialServiceRequest SpecialServiceRequest { get; set; }
        [Required]
        public List<Services> ExtraServices { get; set; }
        [Required]
        public string PassengerNationality { get; set; }
        [Required]
        public string KnowTravelerNo { get; set; }
        [Required]
        public string RedressNo { get; set; }
        [Required]
        public string NationalID { get; set; }
    }
    [Serializable]
    public class PassengerName
    {
        [Required]
        public string PaxTitle { get; set; }
        [Required]
        public string PaxFirstName { get; set; }
        [Required]
        public string PaxLastName { get; set; }
    }
    [Serializable]
    public class Passport
    {
        [Required]
        public string PassportNumber { get; set; }
        [Required]
        public string ExpiryDate { get; set; }
        [Required]
        public string Country { get; set; }
    }
    [Serializable]
    public class ResidentFareDocumentType
    {
        [Required]
        public string DocumentType { get; set; }
        [Required]
        public string DocumentNumber { get; set; }
        [Required]
        public string Municipality { get; set; }
    }
    [Serializable]
    public class SpecialServiceRequest
    {
        [Required]
        public string SeatPreference { get; set; }
        [Required]
        public string MealPreference { get; set; }
        [Required]
        public List<RequestedSegment> RequestedSegments { get; set; }
    }
    [Serializable]
    public class RequestedSegment
    {
        [Required]
        public string Origin { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public string FlightNumber { get; set; }
        [Required]
        public string DepartureDateTime { get; set; }
        [Required]
        public string SeatSelection { get; set; }
        [Required]
        public List<RequestSSR> RequestSSRs { get; set; }
    }
    [Serializable]
    public class Services
    {
        [Required]
        public int ExtraServiceId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
    [Serializable]
    public class RequestSSR
    {
        [Required]
        public string SSRCode { get; set; }
        [Required]
        public string FreeText { get; set; }
    }
    [Serializable]
    public class PaymentReferences
    {
        [Required]
        public string PaymentID { get; set; }
        [Required]
        public List<Parameter> PaymentParams { get; set; }
    }
    [Serializable]
    public class Parameter
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public string Type { get; set; }
    }
    public class PaymentCardInfo
    {
        [Required]
        public string cardScheme { get; set; }
        [Required]
        public string cardType { get; set; }
        [Required]
        public string cardNumber { get; set; }
        [Required]
        public short cvv { get; set; }
        [Required]
        public CardValidFrom cardValidFrom { get; set; }
        [Required]
        public CardExpiry cardExpiry { get; set; }
        [Required]
        public string cardHolderName { get; set; }
        [Required]
        public BillingAddress billingAddress { get; set; }
        [Required]
        public string securePassword { get; set; }
        [Required]
        public string computerIP { get; set; }
    }
    public class CardValidFrom
    {
        [Required]
        public string month { get; set; }
        [Required]
        public short year { get; set; }
    }
    public class CardExpiry
    {
        [Required]
        public string month { get; set; }
        [Required]
        public short year { get; set; }
    }
    public class BillingAddress
    {
        [Required]
        public string customerTitle { get; set; }
        [Required]
        public string customerFirstName { get; set; }
        [Required]
        public string customerMiddleName { get; set; }
        [Required]
        public string customerLastName { get; set; }
        [Required]
        public string address1 { get; set; }
        [Required]
        public string address2 { get; set; }
        [Required]
        public string address3 { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string state { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public string zip { get; set; }
        [Required]
        public string emailId { get; set; }
    }
}



