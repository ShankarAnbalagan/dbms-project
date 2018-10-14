using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MockSAP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HomePage homePage;
        private AdminLogin adminLogin;
        private DatabaseConnection database;
        public MainWindow()
        {
            InitializeComponent();
            status.Text = "";
            Username_textbox.Focus();
            database = new DatabaseConnection();
            database.connect();
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {            
            if (database.verifyUser(Username_textbox.Text.Trim(),
                Password_textbox.Password.ToString().Trim()))
            {
                homePage = new HomePage();
                homePage.StartPage(Username_textbox.Text, database);
                this.Close();
            }
            else
            {
                Username_textbox.Clear();
                Password_textbox.Clear();
                status.Text = "Wrong username or password.";
            }
        }

        private void NewUser_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            adminLogin = new AdminLogin(database,this);
            adminLogin.StartPage();
        }
    }
}
