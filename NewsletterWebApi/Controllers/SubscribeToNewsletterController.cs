using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsletterWebApi.Contexts;
using NewsletterWebApi.Entities;
using NewsletterWebApi.Models;

namespace NewsletterWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class SubscribeToNewsletterController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    #region CREATE
    [HttpPost]
    public async Task<IActionResult> Create(SubscriberRegistration form)
    {
        if (ModelState.IsValid)
        {
            var existingSubscriber = await _context.Subscribers.Where(x => x.Email == form.Email).FirstOrDefaultAsync();
            if (existingSubscriber == null)
            {
                try
                {
                    var subscriberEntity = new SubscriberEntity
                    {
                        Email = form.Email,
                        DailyNewsletter = form.DailyNewsletter,
                        AdvertisingUpdates = form.AdvertisingUpdates,
                        WeekInReview = form.WeekInReview,
                        EventUpdates = form.EventUpdates,
                        StartupsWeekly = form.StartupsWeekly,
                        Podcasts = form.Podcasts,
                    };
                    _context.Subscribers.Add(subscriberEntity);
                    await _context.SaveChangesAsync();

                    return Created("", null);
                }
                catch
                {
                    return Problem("Unable to create subscription");
                }
            }

            return Conflict("Email is already subscribed");
        }
        return BadRequest();

    }
    #endregion


    #region Read All

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var subscribers = await _context.Subscribers.ToListAsync();

        if (subscribers.Count != 0)
        {
            return Ok(subscribers);
        }

        return NotFound();
    }

    #endregion


    #region Read One

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(string id)
    {
        var subscriber = await _context.Subscribers.FirstOrDefaultAsync(x => x.Id == id);
        if (subscriber != null)
        {
            return Ok(subscriber);
        }
        return NotFound();
    }

    #endregion


    #region Update

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOne(string id, string email)
    {
        var subscriber = await _context.Subscribers.FirstOrDefaultAsync(x => x.Id == id);
        if (subscriber != null)
        {
            subscriber.Email = email;
            _context.Subscribers.Update(subscriber);
            await _context.SaveChangesAsync();

            return Ok(subscriber);
        }

        return NotFound();
    }

    #endregion


    #region DeleteOne

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOne(string id)
    {

        if (ModelState.IsValid)
        {
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(x => x.Id == id);
            if (subscriber != null)
            {
                _context.Subscribers.Remove(subscriber);
                await _context.SaveChangesAsync();

                return Ok();
            }
            return NotFound();
        }
        return BadRequest();
    }
    #endregion



}
