
using System;

namespace SampleOidc.Models
{
    public class Phone
    {
        public string PhoneId { get; set; }
        public string ContactId { get; set; }
        public Contact Contact { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public DateTimeOffset AddedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}