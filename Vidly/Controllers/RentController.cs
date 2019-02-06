using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vidly;
using vidly.Models;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class RentController : Controller
    {
        // GET: Rent
        public ActionResult Index(int id)
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var userEmail = HttpContext.User.Identity.Name;

                    var queryUser = session.QueryOver<User>()
                        .Where(c => c.Email == userEmail)
                        .SingleOrDefault();

                    var queryMovie = session.Get<Movie>(id);

                    foreach (Movie filme in queryUser.Customer.Movies)
                        if (filme == queryMovie)
                            return Redirect("/Home"); //tratar erro

                    if (queryUser.Customer.CanReserveMore() && queryMovie.Disponibility())
                    {
                        queryUser.Customer.Movies.Add(queryMovie);

                        session.SaveOrUpdate(queryUser);

                        transaction.Commit();

                        return Redirect("/Movies"); //tratar sucesso
                    }
                    else
                        return Redirect("/Home"); //tratar erro
                }
            }
        }
    }
}