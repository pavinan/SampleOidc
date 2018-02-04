using System.Collections.Generic;
using SampleOidc.Models;

namespace SampleOidc.DTOs
{
    public class UpdateContactDTO : ContactDTO
    {
        public string ContactId { get; set; }
    }
}