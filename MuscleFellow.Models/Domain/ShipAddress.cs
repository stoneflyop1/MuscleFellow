using System;
using System.ComponentModel.DataAnnotations;

namespace MuscleFellow.Models.Domain
{
    public class ShipAddress
    {
        public int AddressID { get; set; }

        public string UserID { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 4)]
        public string Address { get; set; }

        public string ZipCode { get; set; }

        public string Receiver { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public override string ToString()
        {
            return string.Format("[ShipAddress: AddressID={0}, UserID={1}, Province={2}, City={3}, Address={4}, ZipCode={5}, Receiver={6}, PhoneNumber={7}]", AddressID, UserID, Province, City, Address, ZipCode, Receiver, PhoneNumber);
        }
    }
}
