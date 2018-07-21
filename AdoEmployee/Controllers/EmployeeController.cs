using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neudesic.AdoEmployee.Model;
using System.Data.SqlClient;
using System.Data;

namespace Employee.Controllers
{
    [Produces("application/json")]
    [Route("api/Employee")]
    public class EmployeeController : Controller
    {
        private string connect;
        public EmployeeController()
        {
            connect = "Data Source = localhost; Initial Catalog = Employee;Integrated Security = SSPI";
        }
        [HttpGet]
        public ActionResult TableDisplay()
        {
            List<EmployeeModel> EmployeeList = new List<EmployeeModel>();
            string query = "Select * from EmployeesDetails";
           
            using (SqlConnection connection = new SqlConnection(connect))
            {
                SqlCommand command = new SqlCommand(query);
                connection.Open();
                command.Connection = connection;
                SqlDataReader data = command.ExecuteReader();
                if (data.HasRows)
                {

                    while (data.Read())
                    {
                        EmployeeModel employee = new EmployeeModel();
                        employee.Id = int.Parse(data["Id"].ToString());
                        employee.Name = data["Name"].ToString();
                        employee.Salary = int.Parse(data["Salary"].ToString());
                        EmployeeList.Add(employee);
                    }

                }

            }
            return Ok(EmployeeList);
        }
        [HttpPost]
        public ActionResult Insert([FromBody]EmployeeModel employeeModel)
        {
            
            using (SqlConnection connection = new SqlConnection(connect))
            {
                string query = "insert into  EmployeesDetails values(@id,@name,@salary)";
                SqlCommand command = new SqlCommand(query);
                command.Parameters.Add(new SqlParameter("@id",employeeModel.Id));
                command.Parameters.Add(new SqlParameter("@name", employeeModel.Name));
                command.Parameters.Add(new SqlParameter("@salary", employeeModel.Salary));
                connection.Open();
                command.Connection = connection;
                command.ExecuteNonQuery();
            }

            return Ok("One row inserted");
        }

        [HttpPut]
        public ActionResult Update([FromBody]EmployeeModel model)
        {
            using (SqlConnection connection = new SqlConnection(connect))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("[dbo].[updatesalary]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@salary", model.Salary));
                command.Parameters.Add(new SqlParameter("@id", model.Id));
                command.ExecuteScalar();
            }

            return Ok("One row updated");
        }
        [HttpDelete]
        public ActionResult Delete([FromBody]EmployeeModel model)
        {
            string query = "DELETE from EmployeesDetails  WHERE Id=@id";
            SqlCommand command = new SqlCommand(query);
            command.Parameters.Add(new SqlParameter("@id", model.Id));

            using (SqlConnection connection = new SqlConnection(connect))
            {
                connection.Open();
                command.Connection = connection;
                command.ExecuteNonQuery();
            }

            return Ok("One row deleted");
        }
    }
}