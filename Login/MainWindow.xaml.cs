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

namespace Login
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Window1 window1;
        public MainWindow()
        {
            InitializeComponent();
            status.Text = "";
            
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnection database = new DatabaseConnection();
            database.connect();
            if (database.verifyUser(Username_textbox.Text.Trim(),
                Password_textbox.Password.ToString().Trim()))
            {
                window1 = new Window1();
                window1.startPage(Username_textbox.Text);
                this.Close();
            }
            else
            {
                Username_textbox.Clear();
                Password_textbox.Clear();
                status.Text = "Wrong username or password.";
            }
        }
    }
}
