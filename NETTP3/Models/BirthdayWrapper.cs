using ATNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATNET.Models
{
    public class BirthdayWrapper
    {
        public List<Person> birthdayPeople { get; set; }
        public List<Person> closeBirthdays { get; set; }
    }
}