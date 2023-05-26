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
            var model = new UploadFileViewModel();
            return View(model);
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

                        var name = values[0];
                        var dateOfBirthString = values[1];
                        var marriedString = values[2];
                        var phone = values[3];
                        var salaryString = values[4];

                        if (string.IsNullOrEmpty(name) || name.Length <= 3)
                        {
                            var model = new UploadFileViewModel { ErrorMessage = "The file has a name shorter than 3 letters" };
                            return View("UploadFile", model);
                        }

                        var dateOfBirth = ParseDate(dateOfBirthString);

                        bool married;
                        if (string.IsNullOrEmpty(marriedString) || !bool.TryParse(marriedString, out married))
                        {
                            married = false; 
                        }

                        if (string.IsNullOrEmpty(phone))
                        {
                            var model = new UploadFileViewModel { ErrorMessage = "Missed phone number" };
                            return View("UploadFile", model);
                        }

                        decimal salary;
                        if (string.IsNullOrEmpty(salaryString) || !decimal.TryParse(salaryString, out salary))
                        {
                            var model = new UploadFileViewModel { ErrorMessage = "The salary is not specified or the format is incorrect" };
                            return View("UploadFile", model);
                        }

                        var addUser = new User
                        {
                            Name = name,
                            DateOfBirth = dateOfBirth,
                            Married = married,
                            Phone = phone,
                            Salary = salary
                        };

                        users.Add(addUser);
                    }
                }

                await _context.Users.AddRangeAsync(users);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("UserList");
        }


        private DateTime ParseDate(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return new DateTime(1, 1, 1); 
            }

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