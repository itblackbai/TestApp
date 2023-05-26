using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using WebApp1.Data;
using WebApp1.Models;

namespace WebApp1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
        [HttpGet]
        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var users = new List<User>();

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var isFirstLine = true;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        var values = line.Split(',');

                        var adduser = new User
                        {
                            Name = values[0],
                            DateOfBirth = ParseDate(values[1]),
                            Married = bool.Parse(values[2]),
                            Phone = values[3],
                            Salary = decimal.Parse(values[4])
                        };

                        users.Add(adduser);
                    }
                }

                await _context.Users.AddRangeAsync(users);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("UserList");
        }

        private DateTime ParseDate(string dateString)
        {
            var formattedDateString = dateString.Replace('.', '-');
            return DateTime.Parse(formattedDateString);
        }
        public IActionResult UserList()
        {
            var users = _context.Users.ToList();
            return View(users);
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("UserList");
            }

            return View(user);
        }
        [HttpGet]
public IActionResult Delete(int id)
{
    var user = _context.Users.Find(id);
    if (user == null)
    {
        return NotFound();
    }

    return View(user);
}

[HttpPost]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var user = await _context.Users.FindAsync(id);
    if (user == null)
    {
        return NotFound();
    }

    _context.Users.Remove(user);
    await _context.SaveChangesAsync();

    return RedirectToAction("UserList");
}


    }
}