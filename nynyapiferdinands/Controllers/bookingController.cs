using System;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;
using nynyapiferdinands.Models;

namespace nynyapiferdinands.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly string connectionString = "Server=mysql87.unoeuro.com;Database=ferdinandsboefhus_dk_db;Uid=ferdinandsboefhus_dk;Pwd=6h2rybne;";

        [HttpGet]
        public IActionResult GetAllBookings()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM booking";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    DataTable dataTable = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    adapter.Fill(dataTable);

                    var bookings = new List<Booking>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        TimeSpan.TryParse(row["tid"].ToString(), out TimeSpan tid);

                        var booking = new Booking
                        {
                            id = Convert.ToInt32(row["id"]),
                            firstName = row["firstName"].ToString(),
                            lastName = row["lastName"].ToString(),
                            email = row["email"].ToString(),
                            phoneNumber = row["phoneNumber"].ToString(),
                            postalCode = row["postalCode"].ToString(),
                            antalPersoner = Convert.ToInt32(row["antalPersoner"]),
                            dato = Convert.ToDateTime(row["dato"]),
                            tid = tid,
                            ops = row["ops"].ToString() // Use the updated column name "ops"
                        };

                        bookings.Add(booking);
                    }

                    return Ok(bookings);
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                Console.WriteLine(ex.ToString());

                // Return a more descriptive error message
                return StatusCode(500, $"An error occurred while retrieving the bookings: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddBooking([FromBody] Booking booking)
        {
            try
            {
                if (booking == null)
                {
                    // Log the null object
                    Console.WriteLine("Received null booking object");
                    return BadRequest("Booking object is null");
                }


                Console.WriteLine("Received booking:");
                Console.WriteLine($"id: {booking.id}");
                Console.WriteLine($"firstName: {booking.firstName}");
                Console.WriteLine($"lastName: {booking.lastName}");
                Console.WriteLine($"email: {booking.email}");
                Console.WriteLine($"phoneNumber: {booking.phoneNumber}");
                Console.WriteLine($"postalCode: {booking.postalCode}");
                Console.WriteLine($"antalPersoner: {booking.antalPersoner}");
                Console.WriteLine($"dato: {booking.dato}");
                Console.WriteLine($"tid: {booking.tid}");
                Console.WriteLine($"ops: {booking.ops}");

                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO booking (firstName, lastName, email, phoneNumber, postalCode, antalPersoner, dato, tid, ops) " +
                                   "VALUES (@firstName, @lastName, @email, @phoneNumber, @postalCode, @antalPersoner, @dato, @tid, @ops)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@firstName", booking.firstName);
                    command.Parameters.AddWithValue("@lastName", booking.lastName);
                    command.Parameters.AddWithValue("@email", booking.email);
                    command.Parameters.AddWithValue("@phoneNumber", booking.phoneNumber);
                    command.Parameters.AddWithValue("@postalCode", booking.postalCode);
                    command.Parameters.AddWithValue("@antalPersoner", booking.antalPersoner);
                    command.Parameters.AddWithValue("@dato", booking.dato.ToString("yyyy-MM-dd")); // Format the date as "yyyy-MM-dd"
                    command.Parameters.AddWithValue("@tid", booking.tid.ToString("c")); // Format the time as "HH:mm:ss"
                    command.Parameters.AddWithValue("@ops", booking.ops);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok("Booking added successfully");
                    }
                    else
                    {
                        return BadRequest("Failed to add booking");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                Console.WriteLine(ex.ToString());

                // Return a more descriptive error message
                return StatusCode(500, $"An error occurred while adding the booking: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBooking(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM booking WHERE id = @id";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok("Booking deleted successfully");
                    }
                    else
                    {
                        return NotFound("Booking not found");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                Console.WriteLine(ex.ToString());

                // Return a more descriptive error message
                return StatusCode(500, $"An error occurred while deleting the booking: {ex.Message}");
            }
        }


    }
}
