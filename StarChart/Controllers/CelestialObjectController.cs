using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var cObject = _context.CelestialObjects.Find(id);
            if (cObject == null)
            {
                return NotFound();
            }
            cObject.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == cObject.Id).ToList();
            return Ok(cObject);
        }

        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var nObjects = _context.CelestialObjects.Where(o => o.Name == name);
            if (!nObjects.Any())
            {
                return NotFound();
            }
            foreach (var nObject in nObjects)
            {
                nObject.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == nObject.Id).ToList();
            }
            return Ok(nObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var cObjects = _context.CelestialObjects.ToList();
            foreach (var cObject in cObjects)
            {
                cObject.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == cObject.Id).ToList();
            }
            return Ok(cObjects);
        }
    }
}
