using ATNET.Models;
using ATNET.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATNET.Controllers
{
    public class PersonController : Controller
    {
        PersonRepository peopleRepository = new PersonRepository();

        public ActionResult Index()
        {
            BirthdayWrapper bw = new BirthdayWrapper();
            bw.birthdayPeople = peopleRepository.GetBirthdayPeople();
            bw.closeBirthdays = peopleRepository.GetCloseBirthdays();

            return View(bw);
        }

        public ActionResult List()
        {
            return View(peopleRepository.GetPeople());
        }

        [HttpPost]
        public ActionResult Search(string pSearch)
        {
            return View(peopleRepository.SearchByName(pSearch));
        }

        public ActionResult Details(int id)
        {
            return View(peopleRepository.GetPerson(id));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            peopleRepository.AddPerson(collection["name"], collection["surName"], DateTime.Parse(collection["birthday"]));

            return RedirectToAction("List");
        }
                
        public ActionResult Edit(int id)
        {
            return View(peopleRepository.GetPerson(id));
        }

        [HttpPost]
        public ActionResult Edit(FormCollection formCollection)
        {
            Person p = new Person()
            {
                id = int.Parse(formCollection["id"]),
                name = formCollection["name"],
                surName = formCollection["surName"],
                birthday = DateTime.Parse(formCollection["birthday"])
            };

            peopleRepository.UpdatePerson(p);

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            peopleRepository.RemovePerson(id);

            return RedirectToAction("List");
        }
    }
}
