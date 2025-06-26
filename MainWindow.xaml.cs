using System.Configuration.Provider;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;
using Org.BouncyCastle.Bcpg.OpenPgp;


namespace SQL_Final_Project
{
    public partial class MainWindow : Window
    {
        private MySqlConnection connection;
        private EmployeeManager employeeManager;
        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            employeeManager = new EmployeeManager(DataGridEmployees);
        }

        private void InitializeDatabaseConnection()
        {
            string connectionString = "server=localhost; user=root; password=mkbhd123!; database=qm_ictprg431;";
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        private void BtnShowAll_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM employees;";
            MySqlCommand command = new MySqlCommand(query,connection);
            employeeManager.LoadEmployees(command); // Pass command to LoadEmployees to execute via ExecuteReader() 
        }

        private void BtnSearchByName_Click(object sender, RoutedEventArgs e)
        {
            string userSearch = txtSearchName.Text;
            if (string.IsNullOrEmpty(userSearch)) // check if nothing is entered
            {
                MessageBox.Show("Please enter a name:");
            }
            else
            {
                string query = "SELECT * FROM employees WHERE family_name LIKE @userSearch OR given_name LIKE @userSearch";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@userSearch", userSearch); // To replace @userSearch ( the string variable) into #userSearch in the query
                employeeManager.LoadEmployees(command);
            }
        }

        private void BtnSearchByBranch_Click(object sender, RoutedEventArgs e)
        {
            string userSearch = txtBranchNumber.Text;
            bool checkIfUserSearchIsValid = int.TryParse(userSearch, out int intUserSearch);
            if (checkIfUserSearchIsValid == false) 
            {
                MessageBox.Show("Invalid branch number, please enter a number: ");
            }
            else if (string.IsNullOrEmpty(userSearch))
            {
                MessageBox.Show("Please enter a branch number: ");
            }
            else
            {
                string query = "SELECT * FROM employees WHERE branch_id = @userSearch";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@userSearch", intUserSearch); 
                employeeManager.LoadEmployees(command);
            }


        }

        private void BtnSearchBySalary_Click(object sender, RoutedEventArgs e)
        {
            string userSearch = txtSalaryFilter.Text;
            bool checkIfUserSearchIsValid = int.TryParse(userSearch, out int intUserSearch);
            if (checkIfUserSearchIsValid == false)
            {
                MessageBox.Show("Invalid, please enter a number: ");
            }
            else if (string.IsNullOrEmpty(userSearch))
            {
                MessageBox.Show("Please enter a salary: ");
            }
            else if (intUserSearch < 0)
            {
                MessageBox.Show("Nobody works for free");
            }
            else
            {
                string query = "SELECT * FROM employees WHERE gross_salary > @userSearch";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@userSearch", intUserSearch);
                employeeManager.LoadEmployees(command);
            }


        }

        private void BtnAddEmployee_Click(object sender, RoutedEventArgs e)
        {

            DateTime dob = DateTime.MinValue;
            string gender = string.Empty;

            //ID
            if (!int.TryParse(txtEmployeeId.Text, out int id))
            {
                MessageBox.Show("Invalid ID, please enter a valid number: ");
                return;
            }

            //NAME
            string givenName = txtGivenName.Text;
            string familyName = txtFamilyName.Text;

            if (string.IsNullOrWhiteSpace(givenName) || givenName.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid given Name (not empty or all numbers): ");
                return;
            }

            if (string.IsNullOrWhiteSpace(familyName) || familyName.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid family Name (not empty or all numbers): ");
                return;
            }

            //DOB
            if (DateOfBirth.SelectedDate == null)
            {
                MessageBox.Show("Please select a valid Date of Birth.");
                return;
            }
            else
            {
                dob = DateOfBirth.SelectedDate.Value;
            }
               
            //GENDER
            if (GenderIdentity.SelectedItem == null)
            {
                MessageBox.Show("Please select a gender identity");
                return;
            }
            else
            {
                string selectedGender = (GenderIdentity.SelectedItem as ComboBoxItem).Content.ToString();

                switch (selectedGender)
                {
                    case "Female":
                        gender = "F";
                        break;
                    case "Male":
                        gender = "M";
                        break;
                    case "Non-binary":
                        gender = "O";
                        break;
                }
            }

            //SALARY
            if (!int.TryParse(txtSalary.Text, out int salary) || (string.IsNullOrEmpty(txtSalary.Text)))
            {
                MessageBox.Show("Invalid salary, please enter a valid number: ");
                return;
            }
            else if (salary < 0)
            {
                MessageBox.Show("Nobody works for free");
                return;
            }

            //BRANCH ID 
            if (!int.TryParse(txtBranch.Text, out int branchId) || (string.IsNullOrEmpty(txtBranch.Text)))
            {
                MessageBox.Show("Invalid branch ID, please enter a valid number: ");
                return;
            }

            //SUPERVISOR ID 
            if (!int.TryParse(txtBranch.Text, out int supervisorId) || (string.IsNullOrEmpty(txtSupervisor.Text)))
            {
                MessageBox.Show("Invalid supervisor ID, please enter a valid number: ");
                return;
            }
  
            DateTime createdAt = DateTime.Now;

            string query = @"INSERT INTO Employees (id, given_name, family_name, date_of_birth, gender_identity, gross_salary, branch_id, supervisor_id, created_at)
                           VALUES (@id, @given_name, @family_name, @dob, @gender, @salary, @branch_id, @supervisor_id, @created_at)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@given_name", givenName);
            command.Parameters.AddWithValue("@family_name", familyName);
            command.Parameters.AddWithValue("@dob", dob);
            command.Parameters.AddWithValue("@gender", gender);
            command.Parameters.AddWithValue("@salary", salary);
            command.Parameters.AddWithValue("@branch_id", branchId);
            command.Parameters.AddWithValue("@supervisor_id", supervisorId);
            command.Parameters.AddWithValue("@created_at", DateTime.Now);

            employeeManager.LoadEmployees(command);
            MessageBox.Show("Employee submitted"); 


        }

        private void BtnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            int selectedId = 0; // To access ID later outside of IF ELSE 
            if (DataGridEmployees.SelectedItem == null)
            {
                MessageBox.Show("No employee selected");
                return;
            }
            else
            {
                var selectedEmployee = (Employee)DataGridEmployees.SelectedItem; // Make selected object an Employee
                selectedId = selectedEmployee.Id;
            }

            string query = "DELETE FROM employees WHERE id = @id";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", selectedId);
            employeeManager.LoadEmployees(command);
            MessageBox.Show("Employee deleted!"); 

        }
    }
}



