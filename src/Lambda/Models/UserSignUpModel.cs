using System;

namespace Lambda.Models
{
    public class UserSignUpModel
    {
        public Guid UserPk;
        public Guid EventPk;
        public DateTime SignUpDate;
        public DateTime CancelDate;
        public bool Payed;
    }
}