﻿using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        MySqlConnection con = new MySqlConnection("server=127.0.0.1;port=3306;user=root;persistsecurityinfo=True;database=schoolmanagementapp;password='hello123'");

        public StudentController()
        {
            CreateEnrollStudentTable();
            CreateStudentTable();
        }

        [HttpGet("GetStudent")]
        public string GetStudent()
        {
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT student.*, course.course_name FROM student LEFT JOIN course ON student.course = course.course_id", con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                StudentResponceMessageClass response = new StudentResponceMessageClass();
                List<GetStudentClass> studentList = new List<GetStudentClass>();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GetStudentClass getStudent = new GetStudentClass();
                        getStudent.StudentID = Convert.ToString(dt.Rows[i]["student_id"]);
                        getStudent.FirstName = Convert.ToString(dt.Rows[i]["first_name"]);
                        getStudent.LastName = Convert.ToString(dt.Rows[i]["last_name"]);
                        getStudent.Email = Convert.ToString(dt.Rows[i]["email"]);
                        getStudent.PhoneNumber = Convert.ToString(dt.Rows[i]["phone_no"]);
                        getStudent.UserID = Convert.ToString(dt.Rows[i]["user_id"]);
                        getStudent.Course = Convert.ToString(dt.Rows[i]["course_name"]);
                        studentList.Add(getStudent);
                    }

                    if (studentList.Count > 0)
                    {
                        response.Status = "200";
                        response.Data = JsonConvert.SerializeObject(studentList);
                        return JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        return JsonConvert.SerializeObject("No data Found");
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject("No data Found");
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject($"Error: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        [HttpPost("InsertStudent")]
        public string PostStudent(StudentClass student)
        {
            StudentResponceMessageClass response = new StudentResponceMessageClass();
            string Id = Guid.NewGuid().ToString();

            MySqlCommand cmd = new MySqlCommand("INSERT INTO Student (student_id, first_name, last_name, email, phone_no, user_id, course) VALUES (@Id, @FirstName, @LastName, @Email, @PhoneNumber, @UserId, @Course)", con);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
            cmd.Parameters.AddWithValue("@LastName", student.LastName);
            cmd.Parameters.AddWithValue("@Email", student.Email);
            cmd.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
            cmd.Parameters.AddWithValue("@UserId", student.UserID);
            cmd.Parameters.AddWithValue("@Course", student.Course);

            try
            {
                con.Open();
                int i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    response.Status = "200";
                    response.Message = "Successfully Inserted Data";
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    response.Status = "400";
                    response.Message = "Student Cannot be Inserted";
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject($"Error: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        [HttpPut("UpdateStudent/{id}")]
        public string PutStudent(string id, StudentClass student)
        {
            StudentResponceMessageClass response = new StudentResponceMessageClass();

            MySqlCommand cmd = new MySqlCommand("UPDATE student SET first_name = @FirstName, last_name = @LastName, email = @Email, phone_no = @PhoneNumber, course = @Course WHERE student_id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
            cmd.Parameters.AddWithValue("@LastName", student.LastName);
            cmd.Parameters.AddWithValue("@Email", student.Email);
            cmd.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
            cmd.Parameters.AddWithValue("@Course", student.Course);

            try
            {
                con.Open();
                int i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    response.Status = "200";
                    response.Message = "Successfully Updated Data";
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    response.Status = "400";
                    response.Message = "Student Cannot be Updated";
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject($"Error: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        [HttpGet("FindStudent/{id}")]
        public string FindStudent(string id)
        {
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT student.*, course.course_name FROM student LEFT JOIN course ON student.course = course.course_id WHERE student.student_id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                StudentResponceMessageClass response = new StudentResponceMessageClass();
                List<GetStudentClass> studentList = new List<GetStudentClass>();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GetStudentClass getStudent = new GetStudentClass();
                        getStudent.StudentID = Convert.ToString(dt.Rows[i]["student_id"]);
                        getStudent.FirstName = Convert.ToString(dt.Rows[i]["first_name"]);
                        getStudent.LastName = Convert.ToString(dt.Rows[i]["last_name"]);
                        getStudent.Email = Convert.ToString(dt.Rows[i]["email"]);
                        getStudent.PhoneNumber = Convert.ToString(dt.Rows[i]["phone_no"]);
                        getStudent.UserID = Convert.ToString(dt.Rows[i]["user_id"]);
                        getStudent.Course = Convert.ToString(dt.Rows[i]["course_name"]);
                        studentList.Add(getStudent);
                    }

                    if (studentList.Count > 0)
                    {
                        response.Status = "200";
                        response.Data = JsonConvert.SerializeObject(studentList);
                        return JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        return JsonConvert.SerializeObject("Student not found");
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject("Student not found");
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject($"Error: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        [HttpDelete("DeleteStudent/{id}")]
        public string DeleteStudent(string id)
        {
            StudentResponceMessageClass response = new StudentResponceMessageClass();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM student WHERE student_id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);

            try
            {
                con.Open();
                int i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    response.Status = "200";
                    response.Message = "Student Deleted Successfully";
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    response.Status = "400";
                    response.Message = "No Data Found";
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject($"Error: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        private void CreateEnrollStudentTable()
        {
            try
            {
                con.Open();
                string query = @"CREATE TABLE IF NOT EXISTS StudentEnrolled (
                                    student_id VARCHAR(255) NOT NULL PRIMARY KEY,
                                    first_name VARCHAR(255) NOT NULL,
                                    last_name VARCHAR(255) NOT NULL,
                                    email VARCHAR(255) NOT NULL UNIQUE,
                                    phone_no VARCHAR(20),
                                    user_id VARCHAR(255),
                                    course VARCHAR(255)
                                )";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating StudentEnrolled table: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        [HttpGet("GetEnrollStudents")]
        public IActionResult GetEnrollStudents()
        {
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT se.*, c.course_name FROM StudentEnrolled se JOIN course c ON se.course = c.course_id", con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                List<GetEnrollStudentsClass> studentList = new List<GetEnrollStudentsClass>();

                foreach (DataRow row in dt.Rows)
                {
                    GetEnrollStudentsClass getStudent = new GetEnrollStudentsClass();
                    getStudent.StudentID = Convert.ToString(row["student_id"]);
                    getStudent.FirstName = Convert.ToString(row["first_name"]);
                    getStudent.LastName = Convert.ToString(row["last_name"]);
                    getStudent.Email = Convert.ToString(row["email"]);
                    getStudent.PhoneNumber = Convert.ToString(row["phone_no"]);
                    getStudent.UserID = Convert.ToString(row["user_id"]);
                    getStudent.Course = Convert.ToString(row["course_name"]);
                    studentList.Add(getStudent);
                }

                if (studentList.Count > 0)
                {
                    return Ok(studentList); // Return IActionResult with Ok
                }
                else
                {
                    return NotFound("No data Found 13"); // Return IActionResult with NotFound
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}"); // Return IActionResult with BadRequest
            }
            finally
            {
                con.Close();
            }
        }




        [HttpGet("FindEnrollStudent/{id}")]
        public string FindEnrollStudent(string id)
        {
            StudentResponceMessageClass response = new StudentResponceMessageClass();

            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM StudentEnrolled WHERE student_id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                List<GetEnrollStudentsClass> studentList = new List<GetEnrollStudentsClass>();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GetEnrollStudentsClass getStudent = new GetEnrollStudentsClass();
                        getStudent.StudentID = Convert.ToString(dt.Rows[i]["student_id"]);
                        getStudent.FirstName = Convert.ToString(dt.Rows[i]["first_name"]);
                        getStudent.LastName = Convert.ToString(dt.Rows[i]["last_name"]);
                        getStudent.Email = Convert.ToString(dt.Rows[i]["email"]);
                        getStudent.PhoneNumber = Convert.ToString(dt.Rows[i]["phone_no"]);
                        getStudent.UserID = Convert.ToString(dt.Rows[i]["user_id"]);
                        getStudent.Course = Convert.ToString(dt.Rows[i]["course"]);
                        studentList.Add(getStudent);
                    }

                    if (studentList.Count > 0)
                    {
                        response.Status = "200";
                        response.Data = JsonConvert.SerializeObject(studentList);
                        return JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        return JsonConvert.SerializeObject("Student not found");
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject("Student not found");
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject($"Error: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        [HttpPost("InsertEnrollStudent")]
        public string InsertEnrollStudent(EnrollStudent student)
        {
            StudentResponceMessageClass response = new StudentResponceMessageClass();
            string Id = Guid.NewGuid().ToString();

            if (IsEmailUnique(student.Email))
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO StudentEnrolled (student_id, first_name, last_name, email, phone_no, user_id, course) VALUES (@Id, @FirstName, @LastName, @Email, @PhoneNumber, @UserId, @Course)", con);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
                cmd.Parameters.AddWithValue("@LastName", student.LastName);
                cmd.Parameters.AddWithValue("@Email", student.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                cmd.Parameters.AddWithValue("@UserId", student.UserId);
                cmd.Parameters.AddWithValue("@Course", student.Course);

                try
                {
                    con.Open();
                    int i = cmd.ExecuteNonQuery();

                    if (i > 0)
                    {
                        response.Status = "200";
                        response.Message = "Successfully Inserted Data";
                        return JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        response.Status = "400";
                        response.Message = "Student Cannot be Inserted";
                        return JsonConvert.SerializeObject(response);
                    }
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject($"Error: {ex.Message}");
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                response.Status = "400";
                response.Message = "Email address already exists. Please use a different email.";
                return JsonConvert.SerializeObject(response);
            }
        }

        private bool IsEmailUnique(string email)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM StudentEnrolled WHERE email = @Email", con);
            cmd.Parameters.AddWithValue("@Email", email);

            try
            {
                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count == 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        [HttpPut("UpdateEnrollStudent/{id}")]
        public string PutEnrollStudent(string id, EnrollStudent student)
        {
            StudentResponceMessageClass response = new StudentResponceMessageClass();

            MySqlCommand cmd = new MySqlCommand("UPDATE StudentEnrolled SET first_name = @FirstName, last_name = @LastName, email = @Email, phone_no = @PhoneNumber, course = @Course WHERE student_id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
            cmd.Parameters.AddWithValue("@LastName", student.LastName);
            cmd.Parameters.AddWithValue("@Email", student.Email);
            cmd.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
            cmd.Parameters.AddWithValue("@Course", student.Course);

            try
            {
                con.Open();
                int i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    response.Status = "200";
                    response.Message = "Successfully Updated Data";
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    response.Status = "400";
                    response.Message = "Student Cannot be Updated";
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject($"Error: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        [HttpDelete("DeleteEnrollStudent/{id}")]
        public string DeleteEnrollStudent(string id)
        {
            StudentResponceMessageClass response = new StudentResponceMessageClass();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM StudentEnrolled WHERE student_id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);

            try
            {
                con.Open();
                int i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    response.Status = "200";
                    response.Message = "Student Deleted Successfully";
                    return JsonConvert.SerializeObject(response);
                }
                else
                {
                    response.Status = "400";
                    response.Message = "No Data Found";
                    return JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject($"Error: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }
        private void CreateStudentTable()
        {
            try
            {
                con.Open();
                string query = @"CREATE TABLE IF NOT EXISTS Student (
                            student_id VARCHAR(255) NOT NULL PRIMARY KEY,
                            first_name VARCHAR(255) NOT NULL,
                            last_name VARCHAR(255) NOT NULL,
                            email VARCHAR(255) NOT NULL UNIQUE,
                            phone_no VARCHAR(20),
                            user_id VARCHAR(255),
                            course VARCHAR(255),
                            FOREIGN KEY (course) REFERENCES course(course_id)
                        )";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Student table: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        private class StudentResponceMessageClass
        {
            public string Status { get; set; }
            public string Message { get; set; }
            public string Data { get; set; }
        }

        private class GetStudentClass
        {
            public string StudentID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string UserID { get; set; }
            public string Course { get; set; }
        }

        private class GetEnrollStudentsClass
        {
            public string StudentID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string UserID { get; set; }
            public string Course { get; set; }
        }
    }
}
