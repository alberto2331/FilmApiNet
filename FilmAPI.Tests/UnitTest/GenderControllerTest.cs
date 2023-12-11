using FilmAPI.Controllers;
using FilmAPI.DTOs;
using FilmAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmAPI.Tests.UnitTest
{
    [TestClass]
    public class GenderControllerTest: TestBasis
    {
        [TestMethod]
        public async Task GetAllGenders()
        {
            // Preparation:
            var nameDB = Guid.NewGuid().ToString(); // Create name from DB
            var context = BuildContext(nameDB); //The method "BuildContext()" is from "TestBasis" class
            var mapper = AutomapperConfiguration(); //The method "AutomapperConfiguration()" is from "TestBasis" class
            context.Genders.Add(new Gender() { Name = "Gender 1" }); // We populate the temporary database
            context.Genders.Add(new Gender() { Name = "Gender 2" }); // We populate the temporary database
            await context.SaveChangesAsync(); // Save Changes
            
            var context2 = BuildContext(nameDB); // Create a context without any record BUT WITH THE SAME DATABASE
            // Test:
            var controller = new GenderController(context2, mapper);
            var respons = await controller.GetAll();
            // Check:
            var genders = respons.Value;
            Assert.AreEqual(2, genders.Count);
        }

        [TestMethod]
        public async Task TryToGetGenderByIdDoesntExist()
        {
            // Preparation:
            var nameDB = Guid.NewGuid().ToString();
            var context = BuildContext(nameDB); 
            var mapper = AutomapperConfiguration(); 
            
            // Test:
            var controller = new GenderController(context, mapper);
            var respons = await controller.GetById(1);
            // Check:
            var res = respons.Result as StatusCodeResult; // cast res
            Assert.AreEqual(404, res.StatusCode);
        }

        [TestMethod]
        public async Task GetGenderById()
        {
            // Preparation:
            var nameDB = Guid.NewGuid().ToString();
            var context = BuildContext(nameDB);
            var mapper = AutomapperConfiguration();
            context.Genders.Add(new Gender() { Name = "Gender 1" });
            await context.SaveChangesAsync();

            var context2 = BuildContext(nameDB); // Create a context without any record BUT WITH THE SAME DATABASE
            // Test:
            var controller = new GenderController(context2, mapper);
            var respons = await controller.GetById(1);
            // Check:
            var genderId = respons.Value.Id;
            Assert.AreEqual(1, genderId);
        }

        [TestMethod]
        public async Task CreateGender()
        {
            // Preparation:
            var nameDB = Guid.NewGuid().ToString();
            var context = BuildContext(nameDB);
            var mapper = AutomapperConfiguration();

            var newGender = new GenderAddDto() { Name = "Gender 1"}; //Create new Gender Object            
            // Test:
            var controller = new GenderController(context, mapper);
            var respons = await controller.Post(newGender);
            var res = respons as CreatedAtRouteResult;// CreatedAtRouteResult is what the method Post return;

            // Check:
            Assert.IsNotNull(res); 
            var genderId = controller.GetById(1);
            Assert.AreEqual(1, genderId.Id);
        }

        [TestMethod]
        public async Task UpdateByPutGender()
        {
            // Preparation:
            var nameDB = Guid.NewGuid().ToString();
            var context = BuildContext(nameDB);
            var mapper = AutomapperConfiguration();

            context.Genders.Add(new Gender() { Name = "Gender 1" });
            await context.SaveChangesAsync();

            var context2 = BuildContext(nameDB);
            var controller = new GenderController(context2, mapper);

            var genderCreationDto = new GenderAddDto() { Name = "New Name"};

            var id = 1;
            var respons = await controller.Put(id,genderCreationDto);

            var res = respons as StatusCodeResult;
            Assert.AreEqual(204, res.StatusCode);

            var context3 = BuildContext(nameDB);
            var exist = await context3.Genders.AnyAsync(x => x.Name == "New Name");
            Assert.IsTrue(exist);
        }

        [TestMethod]
        public async Task DeleteGender()
        {
            // Preparation:
            var nameDB = Guid.NewGuid().ToString();
            var context = BuildContext(nameDB);
            var mapper = AutomapperConfiguration();

            context.Genders.Add(new Gender() { Name = "Gender 1" });
            await context.SaveChangesAsync();

            var context2 = BuildContext(nameDB);
            var controller = new GenderController(context2, mapper);
            
            var respons = await controller.Delete(1);
            var res = respons as StatusCodeResult;

            Assert.AreEqual(200, res.StatusCode);
            var context3 = BuildContext(nameDB);
            var exist = await context3.Genders.AnyAsync();
            Assert.IsFalse(exist);

        }
    }
}
