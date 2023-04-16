using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using System.Security.Claims;
using Appointment_Scheduler.Models;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Appointment_Scheduler.Controllers
{
    [Authorize]
    public class ScheduleController : Controller
    {
        public static string errorMessage = "";
        
        // To connect db
        IConfiguration configuration;
        public SqlConnection connection;
        public ScheduleController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connection = new SqlConnection(configuration.GetConnectionString("Appointment_SchedulerContextConnection"));

        }


        // Method to get Schedules.
        private List<Schedule> GetSchedules()
        {
            List<Schedule> schedules = new List<Schedule>();
            connection.Open();

            SqlCommand command = new SqlCommand("fetchSchedules", connection);

            // Get user id of currently loggedIn user
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string query = $"Select Id, ScheduleTime, Description from Schedule where ( UserId = '{userId}' )";
            Console.WriteLine(query);

            command.CommandText = query;

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                
                Schedule schedule = new Schedule();
                schedule.Id = (int)reader["Id"];
                schedule.ScheduleTime = (DateTime)reader["ScheduleTime"];
                schedule.Description = (string)reader["Description"]; 
                
                schedules.Add(schedule);
            }

            reader.Close();
            connection.Close();

            return schedules;
        }

        // GET: ScheduleController
        public ActionResult Index()
        {
            Console.WriteLine("yeah");
            return View(GetSchedules());
        }

        // GET: ScheduleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ScheduleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // Method to insert into Schedule table
        private void AddSchedule(DateTime dateTime, string description)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("addSchedule", connection);

            // Get user id of currently loggedIn user
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string processedDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine(processedDate);
            string query = $"INSERT INTO Schedule( ScheduleTime, UserId , Description ) VALUES( '{processedDate}', '{userId}','{description}' )";
            Console.WriteLine(query);
            command.CommandText = query;


            command.ExecuteNonQuery();
            connection.Close();

        }

        // POST: ScheduleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( IFormCollection collection)
        {
            try
            {
                ViewBag.errorMessage = "";
                
                AddSchedule( Convert.ToDateTime( collection["ScheduleTime"]) , collection["Description"]);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                //ViewData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // Method to get the schedule object
        private Schedule getSchedule(int id)
        {
            connection.Open();

            SqlCommand command = new SqlCommand("getSchedule", connection);

            // Get user id of currently loggedIn user
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string query = $"Select Id, ScheduleTime, Description from Schedule where ( UserId = '{userId}' )";
            Console.WriteLine(query);

            command.CommandText = query;

            SqlDataReader reader = command.ExecuteReader();

            Schedule schedule = new Schedule();
            while (reader.Read())
            {
                
                schedule.Id = (int)reader["Id"];
                schedule.ScheduleTime = (DateTime)reader["ScheduleTime"];
                schedule.Description = (string)reader["Description"];   
                
            }

            reader.Close();
            connection.Close();

            return schedule;
        }

        // GET: ScheduleController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(getSchedule(id));
        }


        public void updateSchedule(int Id, DateTime dateTime, string description)
        {
            Console.WriteLine("top of funtion: "+ dateTime);
            connection.Open();
            SqlCommand command = new SqlCommand("addSchedule", connection);

           
            string processedDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            Console.WriteLine(processedDate);
            string query = $"UPDATE Schedule " +
                $"SET ScheduleTime='{processedDate}' , Description='{description}'  " +
                $"WHERE ( Id = {Id} )";

            Console.WriteLine(query);

            command.CommandText = query;


            command.ExecuteNonQuery();
            connection.Close();
        }

        // POST: ScheduleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                
                updateSchedule( id, Convert.ToDateTime(collection["ScheduleTime"]), collection["Description"] );
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }

        // GET: ScheduleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        private void DeleteSchedule(int Id)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("deleteSchedule", connection);

            
            
            string query = $"DELETE FROM Schedule WHERE ( Id = '{Id}' )";
            Console.WriteLine(query);
            command.CommandText = query;


            command.ExecuteNonQuery();
            connection.Close();

        }

        // POST: ScheduleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                DeleteSchedule((int)id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
