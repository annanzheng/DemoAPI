using System.Linq;
using System.Web.Http;
using DemoAPI.Context;
using DemoAPI.Models;
using System.Collections.Generic;
using System;
using System.Net;

namespace DemoAPI.Controllers
{
    public class ValuesController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();
  
    
    [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                var result = from country in db.Countries
                             select new
                             {
                                 country.CountryId,
                                 country.Name,
                                 State = from state in db.States
                                         where state.CountryId == country.CountryId
                                         select new
                                         {
                                             state.StateId,
                                             state.Name,
                                             City = from city in db.Cities
                                                    where city.StateId == state.StateId
                                                    select new
                                                    {
                                                        city.CityId,
                                                        city.Name
                                                    }
                                         }
                             };
                return Ok(result);

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }


        // GET: api/Values/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {

            var result = from country in db.Countries
                         where country.CountryId == id
                         select new
                         {
                             country.CountryId,
                             country.Name,
                             State = from state in db.States
                                     where state.CountryId == country.CountryId
                                     select new
                                     {
                                         state.StateId,
                                         state.Name,
                                         City = from city in db.Cities
                                                where city.StateId == state.StateId
                                                select new
                                                {
                                                    city.CityId,
                                                    city.Name
                                                }
                                     }
                         };

            return Ok(result);
        }

        // POST: api/Values
        [HttpPost]
        public IHttpActionResult Post([FromBody] Country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Countries.Add(country);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = country.CountryId }, country);
        }

        // PUT: api/Values/5
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody] Country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCountry = db.Countries.SingleOrDefault(c => c.CountryId == id);
            
            if (existingCountry == null)
            {
                return NotFound();
            }
            // Update the country details
            existingCountry.Name = country.Name;

            // Remove states and cities that are no longer present
            foreach (var state in existingCountry.State.ToList())
            {
                if (!country.State.Any(s => s.StateId == state.StateId))
                {
                    db.States.Remove(state);
                }
                else
                {
                    // Update cities within each state
                    var existingState = country.State.Single(s => s.StateId == state.StateId);
                    foreach (var city in state.City.ToList())
                    {
                        if (!existingState.City.Any(c => c.CityId == city.CityId))
                        {
                            db.Cities.Remove(city);
                        }
                    }
                }
            }

            // Update or add states and cities
            foreach (var state in country.State)
            {
                var existingState = existingCountry.State.SingleOrDefault(s => s.StateId == state.StateId);
                if (existingState == null)
                {
                    // New state
                    existingCountry.State.Add(state);
                }
                else
                {
                    // Update existing state
                    existingState.Name = state.Name;
                    foreach (var city in state.City)
                    {
                        var existingCity = existingState.City.SingleOrDefault(c => c.CityId == city.CityId);
                        if (existingCity == null)
                        {
                            // New city
                            existingState.City.Add(city);
                        }
                        else
                        {
                            // Update existing city
                            existingCity.Name = city.Name;
                        }
                    }
                }
            }

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Values/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var country = db.Countries.SingleOrDefault(c => c.CountryId == id);
            if (country == null)
            {
                return NotFound();
            }

            db.Countries.Remove(country);
            db.SaveChanges();

            return Ok(country);
        }

    }
}
