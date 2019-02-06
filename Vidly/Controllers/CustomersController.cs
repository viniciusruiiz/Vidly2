using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vidly.Models;
using vidly.ViewModels;
using vidly.Mapping;
using NHibernate;
using Vidly.Models;

namespace vidly.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                int top = 0;
                var customers = session.QueryOver<Customer>().List();

                foreach (var customer in customers)
                {
                    top = customer.Movies.Count;
                }// foreach para dar tempo ao NHibernate de pegar os filmes relacionandos aos clientes

                var viewModel = new CustomersClassViewModel
                {
                    Customers = customers
                };

                return View(viewModel);
            }
        }

        // GET: Customers/CustomerDetails
        public ActionResult CustomerDetails(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Customers");
            }

            id = Convert.ToInt32(id);
            CustomerDetailsClassViewModel viewModel = null;

            using (ISession session = SessionFactory.OpenSession())
            {
                var movies = session.QueryOver<Movie>()
                    .JoinQueryOver<Customer>(c => c.Customers)
                    .Where(c => c.Id == id)
                    .List();
                var customers = session.Get<Customer>(id);

                if (movies.Count > 0)
                {
                    viewModel = new CustomerDetailsClassViewModel
                    {
                        Name = customers.Name,
                        CPF = customers.CPF,
                        DateOfBirth = customers.DateOfBirth,
                        User = customers.User,
                        Movies = movies
                    };
                }
                else
                {
                    viewModel = new CustomerDetailsClassViewModel
                    {
                        Name = customers.Name,
                        CPF = customers.CPF,
                        DateOfBirth = customers.DateOfBirth,
                        User = customers.User,
                        Movies = null
                    };
                }

                return View(viewModel);
            }
        }
    }
}