using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLab1.Models.Entities;
using WebLab1.Models;
using WebLab1.Models.ViewModels;
using System.Security.Cryptography;
using System.Web.Security;
using System.Text;

namespace WebLab1.Controllers
{
    public class Lab2Controller : Controller
    {
        // GET: Lab2
        [AllowAnonymous]
        public ActionResult ListOfStudents()
        {
            List<Студент> студенты = new List<Студент>();
            using (var db = new Entities2())
            {
                студенты = db.Студенты.OrderByDescending(x => x.Уровень_владения_ИЯ)
                    .ThenBy(x => x.Имя)
                    .ThenBy(x => x.Фамилия).ToList();
            }
            return View(студенты);
        }

        [HttpGet]
        [Authorize]
        public ActionResult StudentDetails(Guid studentID)
        {
            Студент model = new Студент();
            StudentVM studentVM;
            using (var db = new Entities2())
            {

                model = db.Студенты.Find(studentID);

                studentVM = new StudentVM
                {
                    ID_студента = model.ID_студента,
                    Фамилия = model.Фамилия,
                    Имя = model.Имя,
                    Отчество = model.Отчество,
                    Адрес_проживания = model.Адрес_проживания,
                    Дата_рождения = model.Дата_рождения,
                    Уровень_владения_ИЯ = model.Уровень_владения_ИЯ
                };
            }
            return View(studentVM);
        }

        List<Tuple<bool, string>> GetGendersList()
        {
            List<Tuple<bool, string>> genders = new List<Tuple<bool, string>>
            {
                new Tuple<bool, string>(true, "Мужской"),
                new Tuple<bool, string>(false, "Женский")

            };
            return genders;
        }
        List<Tuple<Guid, string>> GetUserList()
        {
            List<Tuple<Guid, String>> users = new List<Tuple<Guid, string>>();
            using (var context = new Entities2())
            {
                foreach (var p in context.Пользователь.ToList())
                {
                    users.Add(new Tuple<Guid, string>(p.Id, p.Login));
                }
            }
            return users;
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
        public ActionResult CreateStudent()
        {
            ViewBag.Genders = new SelectList(GetGendersList(), "Item1", "Item2");
            ViewBag.Users = new SelectList(GetUserList(), "Item1", "Item2");

            return View();
        }

        [HttpPost]//атрибут для отправки данных на сервер
        [ValidateAntiForgeryToken()] //атрибут для валидации токенов при обращении к методу действия
        [Authorize(Roles = "Admin")]
        public ActionResult CreateStudent(StudentVM newStudent)
        {
            if (ModelState.IsValid) //валидация модели
            {
                using (var context = new Entities2())
                {
                    Студент студент = new Студент
                    {
                        ID_студента = Guid.NewGuid(),
                        Фамилия = newStudent.Фамилия,
                        Имя = newStudent.Имя,
                        Отчество = newStudent.Отчество,
                        Пол = newStudent.Пол,
                        Адрес_проживания = newStudent.Адрес_проживания,
                        Дата_рождения = newStudent.Дата_рождения,
                        Уровень_владения_ИЯ = newStudent.Уровень_владения_ИЯ,
                        Id_пользователя = newStudent.Id_пользователя
                    };
                    context.Студенты.Add(студент); //Добавление объекта в локальную колекцию
                    context.SaveChanges();
                }
                    return RedirectToAction("ListOfStudents");
            }
            ViewBag.Genders = new SelectList(GetGendersList(), "Item1", "Item2");
            ViewBag.Users = new SelectList(GetUserList(), "Item1", "Item2");
            return View(newStudent);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditStudent(Guid studentID)
        {
            StudentVM model;
            using (var context = new Entities2())
            {
                Студент студент = context.Студенты.Find(studentID);
                model = new StudentVM
                {
                    ID_студента = студент.ID_студента,
                    Имя = студент.Имя,
                    Фамилия = студент.Фамилия,
                    Отчество = студент.Отчество,
                    Пол = студент.Пол,
                    Адрес_проживания = студент.Адрес_проживания,
                    Дата_рождения = студент.Дата_рождения,
                    Уровень_владения_ИЯ = студент.Уровень_владения_ИЯ,
                    Id_пользователя = студент.Id_пользователя
                };
            }
            ViewBag.Genders = new SelectList(GetGendersList(), "Item1", "Item2");
            ViewBag.Users = new SelectList(GetUserList(), "Item1", "Item2");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()] //атрибут для валидации токенов при обращении к методу действия
        [Authorize(Roles = "Admin")]
        public ActionResult EditStudent(StudentVM model)
        {
            if (ModelState.IsValid)//валидация модели
            {
                using (var context = new Entities2())
                {
                    Студент EditedCтудент = new Студент
                    {
                        ID_студента = model.ID_студента,
                        Имя = model.Имя,
                        Фамилия = model.Фамилия,
                        Отчество = model.Отчество,
                        Пол = model.Пол,
                        Адрес_проживания = model.Адрес_проживания,
                        Дата_рождения = model.Дата_рождения,
                        Уровень_владения_ИЯ = model.Уровень_владения_ИЯ,
                        Id_пользователя = model.Id_пользователя

                    };
                    context.Студенты.Attach(EditedCтудент); //Присоединяет объект EditedСТудент к контексту данных
                    context.Entry(EditedCтудент).State = System.Data.Entity.EntityState.Modified; //устанавливает состояние в Изменен
                    context.SaveChanges();
                }
                return RedirectToAction("ListOfStudents");
            }
            ViewBag.Genders = new SelectList(GetGendersList(), "Item1", "Item2"); //восстанавливаем вспомогательные объекты (выпадающий список)
            ViewBag.Users = new SelectList(GetUserList(), "Item1", "Item2");

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteStudent(Guid studentID)
        {
            Студент студентToDelete;
            StudentVM model;

            using (var context = new Entities2())
            {
                студентToDelete = context.Студенты.Find(studentID);
                model = new StudentVM
                {
                    ID_студента = студентToDelete.ID_студента,
                    Фамилия = студентToDelete.Фамилия,
                    Имя = студентToDelete.Имя,
                    Отчество = студентToDelete.Отчество,
                    Адрес_проживания = студентToDelete.Адрес_проживания,
                    Дата_рождения = студентToDelete.Дата_рождения,
                    Уровень_владения_ИЯ = студентToDelete.Уровень_владения_ИЯ
                };
            }
            return View(model);
        }

        [HttpPost, ActionName("DeleteStudent")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(Guid studentID)
        {
            using (var context = new Entities2())
            {
                //var1
                Студент студентToDelete = new Студент
                {
                    ID_студента = studentID
                };
                context.Entry(студентToDelete).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
            return RedirectToAction("ListOfStudents");
        }

        [ChildActionOnly]
        public ActionResult Age(Guid studentID)
        {
            string message = "";
            using (var context = new Entities2())
            {
                var student = context.Студенты.Find(studentID);
                DateTime birthDate = student.Дата_рождения;
                DateTime currentDate = DateTime.Now;
                int age = currentDate.Year - birthDate.Year;

                if (birthDate > currentDate.AddYears(-age))
                    age--;
                message = $"Возраст: {age}.";
            }
            return PartialView("Age", message);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserVM webUser)
        {
            if (ModelState.IsValid)
            {
                using (Entities2 context = new Entities2())
                { // Идентификация

                    Пользователь пользователь = null;
                    пользователь = context.Пользователь.Where(u => u.Login == webUser.Login).FirstOrDefault();
                    if(пользователь != null)
                    { // Аутентификация
                        string passwordHash = ReturnHashCode(webUser.Password + пользователь.Salt.ToString().ToUpper());
                        if(passwordHash == пользователь.PasswordHash)
                        {
                            string userRole = "";
                            // Авторизация
                            switch (пользователь.UserRole)
                            {
                                case 1:
                                    userRole = "Admin";
                                    break;
                                case 2:
                                    userRole = "Participant";
                                    break;
                            }

                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                1,                         // Version
                                пользователь.Login,        // User name
                                DateTime.Now,              // Created
                                DateTime.Now.AddDays(1),   // Expires
                                false,                     // Persistent
                                userRole                   // User data => Roles
                                );
                            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                            HttpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
                            return RedirectToAction("ListOfStudents", "Lab2");
                        }
                    }
                }
            }
            ViewBag.Error = "Пользователя с таким логином или паролем не существует, попробуйте еще раз.";
            return View(webUser);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        string ReturnHashCode(string loginAndSalt)
        {
            string hash = "";
            using (SHA1 sha1Hash = SHA1.Create())
            {
                byte[] data = sha1Hash.ComputeHash(Encoding.UTF8.GetBytes(loginAndSalt));
                StringBuilder sBuilder = new StringBuilder();
                for(int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                hash = sBuilder.ToString().ToUpper();
            }
            return hash;
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("ListOfStudents", "Lab2");
        }
    }
}