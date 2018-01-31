
using System;

namespace SampleOidc.Models
{
    public class Address
    {
        public string AddressId { get; set; }
        public string ContactId { get; set; }
        public Contact Contact { get; set; }
        public string Label { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipOrPostal { get; set; }
        public DateTimeOffset AddedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}