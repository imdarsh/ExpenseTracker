using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ExpenseDbContext _context;

    public HomeController(ILogger<HomeController> logger, ExpenseDbContext context)
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
        var total = allExpenses.Sum(x => x.Value);
        ViewBag.Expenses = total;
        return View(allExpenses);
    }

    public IActionResult CreateEditExpense(int? id)
    {
        if(id != null) {
            var exp = _context.Expenses.Find(id);
            return View(exp);
        }
        return View();
    }

    public IActionResult Delete(int id)
    {
        var expense = _context.Expenses.Find(id);
        _context.Expenses.Remove(expense);
        _context.SaveChanges();
        
        return RedirectToAction(nameof(Expenses));
    }

    public IActionResult CreateEditExpenseForm(Expense model)
    {
        var check = _context.Expenses.Find(model.Id);
        if(check != null) {
            _context.Entry(check).CurrentValues.SetValues(model);
             _context.SaveChanges();
        } else{
            _context.Add(model);
            _context.SaveChanges();
        }
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
