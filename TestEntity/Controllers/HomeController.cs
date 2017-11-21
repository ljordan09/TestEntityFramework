using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using TestEntity.Models;
using System.Text.RegularExpressions;

namespace TestEntity.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            /*SqlConnection c = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=myDataBase;Integrated Security=SSPI;");//connection string, first step.

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = c;
            cmd.CommandText = "select * from customers";

            c.Open();
            cmd.ExecuteReader();
            ViewBag.Message = "Your contact page.";*/

            return View();
        }

        public ActionResult ListCustomers()
        {
            //1.Create an object for the ORM
            NorthwindEntities ORM = new NorthwindEntities();
            

            //2.Load the data from the DBSet into the data structure
            List<Customer>CustomerList=ORM.Customers.ToList();

            //3. Filter the data (optional)
            ViewBag.CountryList = ORM.Customers.Select(x => x.Country).Distinct().ToList();
            ViewBag.CustomerList = CustomerList;

            //gives me a list of countries from customer database without duplicates.
            
            return View("CustomersView");
            

        }

        public ActionResult ListCustomersByCountry(string Country)
        {
            NorthwindEntities ORM = new NorthwindEntities();
            List<Customer>OutputList = new List<Customer>();
            ViewBag.CountryList = ORM.Customers.Select(x => x.Country).Distinct().ToList();
            /* foreach (Customer CustomerRecord in ORM.Customers.ToList())
             {
                 if (CustomerRecord.Country.ToLower() == Country.ToLower())
                 {
                     OutputList.Add(CustomerRecord);
                 }

             }
             ViewBag.CustomerList = OutputList;
             */

            //LINQ Query Syntax
            /*OutputList = (from CustomerRecord in ORM.Customers
            where CustomerRecord.Country == Country
            select CustomerRecord).ToList();*/

            //LINQ Method Syntax
            //OutputList = ORM.Customers.Where(x => x.Country == Country).ToList();

            //Native SQL
            OutputList = ORM.Customers.SqlQuery("select * from customers where country=@param1", new SqlParameter("@param1",Country)).ToList();
            ViewBag.CustomerList = OutputList;
            return View("CustomersView");
        }

        public ActionResult ListCustomersByName(string ContactName)
        {
            NorthwindEntities ORM = new NorthwindEntities();
            List<Customer> OutputList = new List<Customer>();
            foreach (Customer CustomerRecord in ORM.Customers.ToList())
            {
                if (CustomerRecord.ContactName != null && Regex.IsMatch(CustomerRecord.ContactName, ContactName, RegexOptions.IgnoreCase))
                {
                    OutputList.Add(CustomerRecord);
                }

            }
            ViewBag.CustomerList = OutputList;
            return View("CustomersView");

        }
    }
}