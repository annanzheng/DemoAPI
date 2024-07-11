using System;
using System.Collections.Generic;
using System.Data.Entity;
using DemoAPI.Models;

namespace DemoAPI.Context
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
     
        {
            base.Seed(context);

            var cityInMaharashtra = new List<City> {
                new City {Name="Mumbai" },
                                new City {Name= "Pune" }
            };
            var cityInUttarPradesh = new List<City> {
                new City {Name="Lucknow" },
                                new City {Name="Banaras" }
            };
            var cityInTamilnadu = new List<City> {
                new City {Name="Bangaluru" },
                                new City {Name="Chennai" }
            };
            var cityInUttaranchal = new List<City> {
                new City {Name="Dehradun" },
                                new City {Name="Rishikesh" }
            };
            var cityInPanjab = new List<City> {
                new City {Name="Chandigarh" },
                                new City {Name="Ludhiana" }
            };
            var stateInIndia = new List<State> {
                new State {
                    Name="Maharashtra",City=cityInMaharashtra
                },
                new State {
                    Name="Uttar Pradesh",City=cityInUttarPradesh
                },
                new  State {
                    Name="Tamil nadu",City=cityInTamilnadu
                },
                new  State {
                    Name="Uttaranchal",City=cityInUttaranchal
                },
                new  State {
                    Name="Panjab",City=cityInPanjab
                }
            };
            Country country = new Country
            {
                Name = "India",
                State = stateInIndia
            };
            context.Countries.Add(country);
            context.SaveChanges();
        }
    }
}
