using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATNET.Models
{
    public class Person
    {
        static int counter = 0;

        public Person() { }

        public Person(string personName, string personSurName, string personFavoriteFood, DateTime personBirthday)
        {
            id = counter++;
            name = personName;
            surName = personSurName;
            birthday = personBirthday;
        }

        public int id { get; set; }
        [DisplayName("Nome")]
        public string name { get; set; }
        [DisplayName("Sobrenome")]
        public string surName { get; set; }
        [DisplayName("Data de aniversário")]
        [Required(ErrorMessage = "Entre com a data de aniversário")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime birthday { get; set; }
    }
}