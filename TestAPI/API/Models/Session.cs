
using System;

namespace TestAPI.API.Models
{
    public class Session
    {
        public string SessionId { get; set; }
        
        public long PersonId { get; set; }
        
        public DateTime ExpirationDate { get; set; }

    }
}