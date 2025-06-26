using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace SQL_Final_Project
{
    public class EmployeeManager
    {
        private DataGrid dataGrid; 
        public EmployeeManager(DataGrid dataGrid)
        {
            this.dataGrid = dataGrid;
        }

        public void LoadEmployees(MySqlCommand command)
        {
            List<Employee> employees = new List<Employee>();
            using (MySqlDataReader reader = command.ExecuteReader()) // "using" to close connection
            {
                while (reader.Read()) // Read through the MySqlDataReader object after ExecuteReader() 
                {
                    employees.Add( new Employee
                    {   Id = reader.GetInt32("id"),
                        GivenName = reader.GetString("given_name"),
                        FamilyName = reader.GetString("family_name"),
                        DateOfBirth = reader.GetDateTime("date_of_birth"),
                        GenderIdentity = reader.GetString("gender_identity"),
                        GrossSalary = reader.GetDecimal("gross_salary"),
                        BranchId = reader.GetInt32("branch_id"),
                        SupervisorId = reader.GetInt32("supervisor_id") });
                       
                }
                this.dataGrid.ItemsSource = employees; // Tell the DataGrid that 'employees list' should be its source for data 
            }
        }

    }
}
