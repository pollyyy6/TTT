using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TicTacToe.Areas.Stat.Controllers
{
    [Area("Stat")]
    public class StatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
