﻿using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vidly;
using vidly.Models;
using Vidly.Models;
using Vidly.Repositorio;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User model)
        {
            if(UserRepositorio.Authentication(model.Email, model.Password) == false)
            {
                return View(model);
            }

            return RedirectToAction("Index", "Movies");
        }

        //GET: Home/Logout
        [Authorize]
        public ActionResult Logout()
        {
            var top = new UserRepositorio();

            top.Logout();

            return RedirectToAction("Index", "Home");
        }

        //GET: Home/Singup
        public ActionResult SingUp()
        {
            return View();
        }

        //POST: Home/Singup
        [HttpPost]
        //public ActionResult SingUp(string _email, string _password, string _name, string _birthdate, string _cpf)
        public ActionResult SingUp(SingUpClassViewModel model)
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                using(ITransaction transaction = session.BeginTransaction())
                {
                    User user = new User
                    {
                        Email = model.User.Email,
                        Password = model.User.Password
                    };

                    Customer customer = new Customer
                    {
                        Name = model.Customer.Name,
                        DateOfBirth = model.Customer.DateOfBirth,
                        CPF = model.Customer.CPF
                    };

                    var permission = session.Get<Permission>(model.User.Permission.Id);

                    user.Permission = permission;
                    user.Customer = customer;
                    customer.User = user;

                    session.Save(customer);
                    session.Save(user);

                    transaction.Commit();
                    
                    return RedirectToAction("Login");
                }
            }
        }
    }
}