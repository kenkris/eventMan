using System;
using System.Collections.Generic;

namespace Lambda.Models
{
    public class EventModel : BaseModel
    {
        public string Name;
        public DateTime StartDate;
        public DateTime EndDate;
        public string Venue;
        public string Address;
    }
}