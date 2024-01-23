using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebLab1.Controllers
{
    public class Lab1Controller : Controller
    {
        // GET: Lab1
        public ActionResult FirstViewMethod()
        {
            List<string> vegetables = GetVegetablesList();
            return View(vegetables);
        }
        public ActionResult SecondViewMethod()
        {
            return View(GetVegetablesList().OrderBy(x => x).ToList());
        }

        public ActionResult ThirdViewMethod()
        {
            return View(GetVegetablesList().OrderBy(x => x).ToList());
        }
        public ActionResult FourthViewMethod()
        {
            return View(GetVegetablesList().OrderBy(x => x.Count()).ToList());
        }


        public List<string> GetVegetablesList()
        {
            List<string> vegetables = new List<string>();
            vegetables.Add("Томат");
            vegetables.Add("Огурец");
            vegetables.Add("Картофель");
            vegetables.Add("Кабочок");
            vegetables.Add("Баклажан");
            vegetables.Add("Капуста");
            vegetables.Add("Брокколи");

            return vegetables;
        }
    }
}