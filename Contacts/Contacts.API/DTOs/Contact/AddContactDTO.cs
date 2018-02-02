
using System.Collections.Generic;


namespace SampleOidc.DTOs
{
    public class AddContactDTO : ContactDTO
    {
        public List<AddAddressDTO> Addresses { get; set; }
        public List<AddEmailDTO> Emails { get; set; }
        public List<AddPhoneDTO> Phones { get; set; }
    }
}