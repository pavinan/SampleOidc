
using System;
using System.Collections.Generic;

namespace SampleOidc.Models
{
    public class Contact
    {
        public string ContactId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Prefix { get; set; }
        public string Designation { get; set; }
        public string Company { get; set; }
        public string Notes { get; set; }
        public DateTimeOffset AddedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Email> Emails { get; set; }
        public List<Phone> Phones { get; set; }
    }
}