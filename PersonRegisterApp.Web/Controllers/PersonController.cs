using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonRegisterApp.Web.Models;
using System;

namespace PersonRegisterApp.Web.Controllers;

public class PersonController : Controller
{
    private readonly ILogger<PersonController> _logger;
    private readonly AppDbContext _ctx;    

    public PersonController(ILogger<PersonController> logger, AppDbContext ctx)
    {
        _logger = logger;
        _ctx = ctx;        
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var people = await _ctx.People.AsNoTracking().OrderBy(p => p.FirstName).ThenByDescending(p => p.LastName).ToListAsync();
            
            return View(people);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            SetTempDataMessage("error", ex.Message);
            return View();
        }

    }

    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Person person)
    {
        if (!ModelState.IsValid)
            return View();

        try
        {
            _ctx.People.Add(person);  // person object is added to context
            await _ctx.SaveChangesAsync(); // actual database call
            SetTempDataMessage("success", "Saved successfully!");
            return RedirectToAction(nameof(Add));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            SetTempDataMessage("error", ex.Message);
            return View();
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var person = await GetSinglePeopleAsync(id);

            if (person is null)
            {
                SetTempDataMessage("error", "No record found!");
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            SetTempDataMessage("error", ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Person person)
    {
        if (!ModelState.IsValid)
            return View(person);

        try
        {
            if (!await _ctx.People.AnyAsync(p => object.Equals(p.Id,person.Id)))
            {
                SetTempDataMessage("error", "No record found!");
                return View(person);
            }

            _ctx.People.Update(person);  // person object is added to context
            await _ctx.SaveChangesAsync(); // actual database call
            SetTempDataMessage("success", "Updated successfully!");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            SetTempDataMessage("error", ex.Message);
            return View();
        }
    }


    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var person = await GetSinglePeopleAsync(id);

            if (person is null)
            {
                SetTempDataMessage("error", "No record found!");
                return RedirectToAction(nameof(Index));
            }
            _ctx.People.Remove(person);
            await _ctx.SaveChangesAsync();
            SetTempDataMessage("success", "Deleted successfull!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            SetTempDataMessage("error", "Something went wrong!");
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task<Person?> GetSinglePeopleAsync(int id) => await _ctx.People.AsNoTracking().SingleOrDefaultAsync(p => object.Equals(p.Id, id));

    private void SetTempDataMessage(string messageType, string message) 
    {
        TempData[messageType] = message;
    }
}
