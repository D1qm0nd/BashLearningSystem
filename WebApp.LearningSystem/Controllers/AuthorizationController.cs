using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels;
using Microsoft.AspNetCore.Mvc;
using WebApp.LearningSystem.BussinesModels;

namespace WebApp.LearningSystem.Controllers
{
    public class AuthorizationController : Controller
    {
        private BusinessViewModel _businessViewModel;

        public AuthorizationController(BusinessViewModel businessViewModel)
        {
            _businessViewModel = businessViewModel;
        }

        public IActionResult Index()
        {
            return View(_businessViewModel);
        }

        public IActionResult Login([Bind("Login")] string? login, [Bind("Password")] string? password)
        {
            _businessViewModel.AuthorizationModel.Account = _businessViewModel.ContextModel.DataContext.Repository
                .GetEntity<Account>().FirstOrDefault(acc => acc.Login == login && acc.Password == password);
            return Redirect("../Home");
        }

        // public IActionResult Login()
        // {
        //     var account = _businessViewModel.ContextModel.DataContext.Repository.GetEntity<Account>().FirstOrDefault();
        //     _businessViewModel.AuthorizationModel.Account = account;
        //     return Redirect("/Home");
        // }
    }
}