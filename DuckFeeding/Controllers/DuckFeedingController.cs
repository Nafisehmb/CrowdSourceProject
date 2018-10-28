using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using DuckFeeding.Models;
using System.IO;

namespace DuckFeeding.Controllers 
{
    [Route("api/DuckFeeding")]
    [ApiController]
    public class DuckFeedingController : ControllerBase
    {
        private readonly DuckFeedingContext _context;

        // Using Stream Reader to restore previously inserted data - stored in context
        // Initialize the records list

        public DuckFeedingController(DuckFeedingContext context)
        {
            _context = context;
            int count = _context.DuckFeedingRecords.Count();
            for (int i = 0; i < count; i++)
            {
                var tmp = _context.DuckFeedingRecords.FirstOrDefault();
                _context.DuckFeedingRecords.Remove(tmp);
                _context.SaveChanges();
            }

            //To prevent saving duplicate data, check if it is not loaded before

            if (System.IO.File.Exists("/users/namb/Documents/duckFeedingDB.csv") && _context.DuckFeedingRecords.Count() == 0)
            {
                StreamReader sr = new StreamReader("/users/namb/Documents/duckFeedingDB.csv");
                string str = sr.ReadLine();//first line is header
                str = sr.ReadLine();
                while (str != null)
                {
                    string[] strs = str.Split(',');
                    _context.DuckFeedingRecords.Add(new DuckFeedingRecord
                    {
                        Time = strs[1],
                        Food = strs[2],
                        Location = strs[3],
                        Count = strs[4],
                        FoodAmount = strs[5],
                        FoodType = strs[6],
                        Repeat = bool.Parse(strs[7]),
                        Period = strs[8]
                    });
                    _context.SaveChanges();

                    str = sr.ReadLine();
                }
                sr.Close();
            }
            if (_context.DuckFeedingRecords.Where(t => t.Location == "-1").Any())
            {
                var tmp2 = _context.DuckFeedingRecords.Where(t => t.Location == "-1").FirstOrDefault();
                _context.DuckFeedingRecords.Remove(tmp2);
                _context.SaveChanges();
            }

            if (_context.DuckFeedingRecords.Count() == 0)
            {
                // Create a new DuckFeedingRecord if collection is empty,
                // which means you can't delete all DuckFeedingRecords.
                _context.DuckFeedingRecords.Add(new DuckFeedingRecord {
                    Time = "000",
                    Count = "0",
                    Food = "Small Fish",
                    FoodType = "Meat",
                    FoodAmount = "1",
                    Location = "-1",
                    Repeat = false,
                    Period = "0"
                });
                _context.SaveChanges();
            }
        }

        // To Show records when log in , both are needed GetById is called by GetAll

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

        // Add a new record

        [HttpPost]
        public IActionResult Create(DuckFeedingRecord item)
        {
            _context.DuckFeedingRecords.Add(item);
            _context.SaveChanges();

            AddToFile(item);

            return CreatedAtRoute("GetDuckFeedingRecord", new { id = item.Id }, item);
        }

        // Using Stream Writer to store inserted data
        public void AddToFile(DuckFeedingRecord recorditem)
        {
            StreamWriter sw = new StreamWriter("/users/namb/Documents/duckFeedingDB.csv");
            sw.WriteLine(string.Concat("Id", ",", "Time", ",", "Food",
                                          ",", "Location", ",", ",", "Count", "FoodAmount", ",", "FoodType", ",isPeriodic", ",", "Period"));
            int count = _context.DuckFeedingRecords.Count();
            for (int i = 0; i < count; i++)
            {
                var item = _context.DuckFeedingRecords.LastOrDefault();
                //if (i != _context.DuckFeedingItems.Count()-1)
                //{
                //sw.WriteLine(string.Concat(tmp.Id.ToString(), ",", tmp.Time.ToString(), ",", tmp.Food,
                //",", tmp.Location, ",", tmp.Count, ",", tmp.FoodAmount, ",", tmp.FoodType, ",", tmp.Repeat.ToString(), ",", tmp.Period));
                //}
                if (item.Repeat == true)
                    sw.WriteLine(string.Concat(item.Id.ToString(), ",", item.Time.ToString(), ",", item.Food,
                                               ",", item.Location, ",", item.Count, ",", item.FoodAmount, ",", item.FoodType, ",", item.Repeat.ToString(), ",", item.Period));
                else
                    sw.WriteLine(string.Concat(item.Id.ToString(), ",", item.Time.ToString(), ",", item.Food,
                    ",", item.Location, ",", item.Count, ",", item.FoodAmount, ",", item.FoodType, ",", "false", ",", "0"));

                _context.DuckFeedingRecords.Remove(item);
                _context.SaveChanges();
            }
            sw.Close();
        }
    }
}
