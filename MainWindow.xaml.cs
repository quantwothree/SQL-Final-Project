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
    }
}



