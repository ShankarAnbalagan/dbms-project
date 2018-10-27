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
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        private AdminLogin adminLogin;
        private MainWindow mainWindow;
        private DatabaseConnection databaseConnection;
        public NewUser(AdminLogin a, MainWindow m, DatabaseConnection db)
        {
            this.adminLogin = a;
            this.mainWindow = m;
            this.databaseConnection = db;
            InitializeComponent();
        }

        public void StartPage()
        {
            this.Show();
            status1.Text = "(Enter 10 or less digit number)";
            status2.Text = "(Atmost 20 characters[LEAVE THIS FIELD BLANK IF REMOVING USER])";
            status3.Text = "(Atmost 10 characters)";
            status4.Text = "(Retype password)";
            adminLogin.Close();
            mainWindow.IsEnabled = false;
        }

        private void AddUser_Button_Click(object sender, RoutedEventArgs e)
        {
            status1.Text = "";
            status2.Text = "";
            status3.Text = "";
            status4.Text = "";

            String uid, uname, upass, upass_v;
            uid = UserId.Text.Trim();
            uname = Username.Text.Trim();
            upass = UPassword.Password.ToString().Trim();
            upass_v = VerifyUPassword.Password.ToString().Trim();

            if(VerifyData(uid, uname, upass, upass_v))
            {
                if(databaseConnection.addUser(uid, uname, upass))
                    MessageBox.Show("User Added Successfully");
                mainWindow.IsEnabled = true;
                this.Close();
            }
            
        }

        private void RemoveUser_Button_Click(object sender, RoutedEventArgs e)
        {
            status1.Text = "";
            status3.Text = "";
            status4.Text = "";

            String uid, upass, upass_v;
            uid = UserId.Text.Trim();
            upass = UPassword.Password.ToString().Trim();
            upass_v = VerifyUPassword.Password.ToString().Trim();

            if (VerifyData(uid, upass, upass_v))
            {
                if (databaseConnection.removeUser(uid, upass))
                    MessageBox.Show("User removed Successfully");
                mainWindow.IsEnabled = true;
                this.Close();
            }
        }

        private Boolean VerifyData(String uid, String uname, String upass, String upass_v)
        {
            if(uid.Length==0 || uname.Length == 0 || upass.Length == 0 || upass_v.Length == 0)
            {
                status1.Foreground = new SolidColorBrush(Colors.Red);
                status2.Foreground = new SolidColorBrush(Colors.Red);
                status3.Foreground = new SolidColorBrush(Colors.Red);
                status4.Foreground = new SolidColorBrush(Colors.Red);
                status1.Text = "(Enter 10 or less digit number)";
                status2.Text = "(Atmost 20 characters)";
                status3.Text = "(Atmost 10 characters)";
                status4.Text = "(Retype password)";
                return false;
            }
            if (!IsDigitsOnly(uid))
            {
                status1.Foreground = new SolidColorBrush(Colors.Red);
                status1.Text = "Uid contains characters other than numbers";
                return false;
            }
            if (uid.Length > 10 && IsDigitsOnly(uid))
            {
                status1.Foreground = new SolidColorBrush(Colors.Red);
                status1.Text = "Too Long";
                return false;
            }
            if (uname.Length > 20)
            {
                status2.Foreground = new SolidColorBrush(Colors.Red);
                status2.Text = "Too Long";
                return false;
            }
            if (!upass.Equals(upass_v))
            {
                status4.Foreground = new SolidColorBrush(Colors.Red);
                status4.Text = "Passwords don't match";
                return false;
            }
            if (upass.Equals(upass_v) && upass.Length > 10)
            {
                status3.Foreground = new SolidColorBrush(Colors.Red);
                status3.Text = "Too Long";
                return false;
            }
            return true;
        }
               
        private void Window_Closed(object sender, EventArgs e)
        {
            mainWindow.IsEnabled = true;
        }
                
        private Boolean VerifyData(String uid, String upass, String upass_v)
        {
            if (uid.Length == 0 || upass.Length == 0 || upass_v.Length == 0)
            {
                status1.Foreground = new SolidColorBrush(Colors.Red);
                status3.Foreground = new SolidColorBrush(Colors.Red);
                status4.Foreground = new SolidColorBrush(Colors.Red);
                status1.Text = "(Enter 10 or less digit number)";
                status3.Text = "(Atmost 10 characters)";
                status4.Text = "(Retype password)";
                return false;
            }
            if (!IsDigitsOnly(uid))
            {
                status1.Foreground = new SolidColorBrush(Colors.Red);
                status1.Text = "Uid contains characters other than numbers";
                return false;
            }
            if (uid.Length > 10 && IsDigitsOnly(uid))
            {
                status1.Foreground = new SolidColorBrush(Colors.Red);
                status1.Text = "Too Long";
                return false;
            }
            if (!upass.Equals(upass_v))
            {
                status4.Foreground = new SolidColorBrush(Colors.Red);
                status4.Text = "Passwords don't match";
                return false;
            }
            if (upass.Equals(upass_v) && upass.Length > 10)
            {
                status3.Foreground = new SolidColorBrush(Colors.Red);
                status3.Text = "Too Long";
                return false;
            }
            return true;
        }

        private bool IsDigitsOnly(String str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] < '0' || str[i] > '9')
                    return false;
            }

            return true;
        }
    }
}
