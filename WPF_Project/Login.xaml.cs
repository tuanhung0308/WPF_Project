using Microsoft.Win32;
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
using WPF_Project.Models;

namespace WPF_Project
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public FarmManagementContext context;
        public Login()
        {
            context = new FarmManagementContext();
            InitializeComponent();
        }


        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               

                string Username = tbUsername.Text;
                string Password = tbPassword.Password;
                bool accValid = context.staff.FirstOrDefault(x => x.Username == Username && x.Password == Password) != null;

                if (accValid == true)
                {
                    MessageBox.Show($"Login successfully!", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                    Home home = new Home();
                    home.Show();
                    this.Close();
                }
                else if (accValid == false)
                {
                    MessageBox.Show($"Wrong username or password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            tbPassword.PasswordChar = '*';
            Register register = new Register();
            this.Hide();
            register.Show();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

