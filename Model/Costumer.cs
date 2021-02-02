using System;
using System.Collections.Generic;

#nullable disable

namespace Model
{
    public partial class Costumer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cellphone { get; set; }
        public string Adress { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
