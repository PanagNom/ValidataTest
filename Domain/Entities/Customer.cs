using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }

        public Customer() { }

        public Customer(string firstName, string lastName, string address, string postalCode)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            PostalCode = postalCode;
        }
    }
}
