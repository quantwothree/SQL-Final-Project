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
            employeeManager.LoadEmployees(command);
        }


    }
}



