using Microsoft.AspNetCore.Mvc;
using SpendSmart.Models;
using System.Diagnostics;

namespace SpendSmart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SpendSmartDBContext _context;

        public HomeController(ILogger<HomeController> logger, SpendSmartDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Expenses()
        {
            var allExpenses = _context.Expenses.ToList();
            var totalExpenses = allExpenses.Sum(x => x.Value);
            ViewBag.Expenses = totalExpenses;
            return View(allExpenses);
        }

        public IActionResult CreateEditExpense(int? id)
        {

            if (id != null)
            {

                var expensInDB = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
                return View(expensInDB);
            }
            return View();
        }

        public IActionResult DeleteExpense(int id)
        {
            var expensInDB = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
         
            if (expensInDB != null)
            {
                _context.Expenses.Remove(expensInDB);
                _context.SaveChanges();
            }

            return RedirectToAction("Expenses");
        }
        public IActionResult ExpenseForm(Expense model)
        {
            if(model.Id == 0) //create
            {
                _context.Expenses.Add(model);

            }
            else //edit
            {
                _context.Expenses.Update(model);
            }
            _context.SaveChanges();
            return RedirectToAction("Expenses");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
