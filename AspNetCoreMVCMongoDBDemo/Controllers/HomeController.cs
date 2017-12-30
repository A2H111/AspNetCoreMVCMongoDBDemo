using AspNetCoreMVCMongoDBDemo.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Diagnostics;
using System.Linq;

namespace AspNetCoreMVCMongoDBDemo.Controllers
{
	public class HomeController : Controller
	{
		private IMongoDatabase mongoDatabase;

		//Generic method to get the mongodb database details
		public IMongoDatabase GetMongoDatabase()
		{
			var mongoClient = new MongoClient("mongodb://localhost:27017");
			return mongoClient.GetDatabase("CustomerDB");
		}

		[HttpGet]
		public IActionResult Index()
		{
			//Get the database connection
			mongoDatabase = GetMongoDatabase();
			//fetch the details from CustomerDB and pass into view
			var result = mongoDatabase.GetCollection<Customer>("Customers").Find(FilterDefinition<Customer>.Empty).ToList();
			return View(result);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Customer customer)
		{
			try
			{
				//Get the database connection
				mongoDatabase = GetMongoDatabase();
				mongoDatabase.GetCollection<Customer>("Customers").InsertOne(customer);
			}
			catch (Exception ex)
			{
				throw;
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			//Get the database connection
			mongoDatabase = GetMongoDatabase();
			//fetch the details from CustomerDB and pass into view
			Customer customer = mongoDatabase.GetCollection<Customer>("Customers").Find<Customer>(k => k.CustomerId == id).FirstOrDefault();
			if (customer == null)
			{
				return NotFound();
			}
			return View(customer);
		}

		[HttpGet]
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			//Get the database connection
			mongoDatabase = GetMongoDatabase();
			//fetch the details from CustomerDB and pass into view
			Customer customer = mongoDatabase.GetCollection<Customer>("Customers").Find<Customer>(k => k.CustomerId == id).FirstOrDefault();
			if (customer == null)
			{
				return NotFound();
			}
			return View(customer);
		}

		[HttpPost]
		public IActionResult Delete(Customer customer)
		{
			try
			{
				//Get the database connection
				mongoDatabase = GetMongoDatabase();
				//Delete the customer record
				var result = mongoDatabase.GetCollection<Customer>("Customers").DeleteOne<Customer>(k => k.CustomerId == customer.CustomerId);
				if (result.IsAcknowledged == false)
				{
					return BadRequest("Unable to Delete Customer " + customer.CustomerId);
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			//Get the database connection
			mongoDatabase = GetMongoDatabase();
			//fetch the details from CustomerDB based on id and pass into view
			var customer = mongoDatabase.GetCollection<Customer>("Customers").Find<Customer>(k => k.CustomerId == id).FirstOrDefault();
			if (customer == null)
			{
				return NotFound();
			}
			return View(customer);
		}

		[HttpPost]
		public IActionResult Edit(Customer customer)
		{
			try
			{
				//Get the database connection
				mongoDatabase = GetMongoDatabase();
				//Build the where condition
				var filter = Builders<Customer>.Filter.Eq("CustomerId", customer.CustomerId);
				//Build the update statement 
				var updatestatement = Builders<Customer>.Update.Set("CustomerId", customer.CustomerId);
				updatestatement = updatestatement.Set("CustomerName", customer.CustomerName);
				updatestatement = updatestatement.Set("Address", customer.Address);
				//fetch the details from CustomerDB based on id and pass into view
				var result = mongoDatabase.GetCollection<Customer>("Customers").UpdateOne(filter, updatestatement);
				if (result.IsAcknowledged == false)
				{
					return BadRequest("Unable to update Customer  " + customer.CustomerName);
				}
			}
			catch (Exception ex)
			{
				throw;
			}

			return RedirectToAction("Index");
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
