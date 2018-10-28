using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using DuckFeeding.Models;


namespace DuckFeeding.Controllers 
{
    [Route("api/DuckFeeding")]
    [ApiController]
    public class DuckFeedingController : ControllerBase
    {
        private readonly DuckFeedingContext _context;

        public DuckFeedingController(DuckFeedingContext context)
        {
            _context = context;

            if (_context.DuckFeedingRecords.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.DuckFeedingRecords.Add(new DuckFeedingRecord {
                    Time = "000",
                    Count = "0",
                    Food = "doon",
                    FoodType = "dry",
                    FoodAmount = "1",
                    Location = "TO",
                    Repeat = false,
                    Period = "0"
                });
                _context.SaveChanges();
            }
        }
        [HttpGet]
        public ActionResult<List<DuckFeedingRecord>> GetAll()
        {
            return _context.DuckFeedingRecords.ToList();
        }

        [HttpGet("{id}", Name = "GetDuckFeedingRecord")]
        public ActionResult<DuckFeedingRecord> GetById(long id)
        {
            var item = _context.DuckFeedingRecords.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        [HttpPost]
        public IActionResult Create(DuckFeedingRecord item)
        {
            _context.DuckFeedingRecords.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetDuckFeedingRecord", new { id = item.Id }, item);
        }
    }
}
