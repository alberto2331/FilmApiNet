using AutoMapper;
using FilmAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmAPI.Tests
{
    public class TestBasis
    {
        protected ApplicationDbContext BuildContext(string nameDB) //This method give us a dbContext with access to a db
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(nameDB).Options;
            
            var dbContext = new ApplicationDbContext(options);
            return dbContext;
        }

        protected IMapper AutomapperConfiguration()
        {
            var config = new MapperConfiguration(options =>
            {
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                options.AddProfile(new AutoMapperProfiles(geometryFactory));
            });
            return config.CreateMapper();
        }
    }
}
