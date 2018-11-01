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
using System.Windows.Shapes;

namespace MockSAP
{
    /// <summary>
    /// Interaction logic for AdminLogin.xaml
    /// </summary>
    public partial class AdminLogin : Window
    {
        private DatabaseConnection databaseConnection;
        private MainWindow mainWindow;
        private NewUser newUser;
        public AdminLogin(DatabaseConnection db, MainWindow m)
        {
            this.databaseConnection = db;
            this.mainWindow = m;
            InitializeComponent();
        }

        public void StartPage()
        {
            status.Text = "";
            this.Show();
            mainWindow.IsEnabled = false;
            AdminPassword.Focus();           
        }

        private void VerifyAdmin_Button_Click(object sender, RoutedEventArgs e)
        {
            if (databaseConnection.verifyUser(AdminPassword.Password.ToString().Trim()))
            {
                newUser = new NewUser(this, mainWindow, databaseConnection);
                newUser.StartPage();
                mainWindow.IsEnabled = false;
            }
            else
            {
                status.Text = "Wrong password";
                AdminPassword.Clear();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mainWindow.IsEnabled = true;
        }
    }
}
