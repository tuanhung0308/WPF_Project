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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public FarmManagementContext context;
        public Register()
        {
            context = new FarmManagementContext();
            InitializeComponent();
        }

        private void btnBackToLogin_CLick(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            this.Close();
            login.Show();
        }
        private void btRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text;
            string password = tbPassword.Password;
            string confirmPassword = tbConfirmPassword.Password;


            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill all information", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
       
            if (password != confirmPassword)
            {
                MessageBox.Show("Password incorrect. Please re-enter", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbPassword.Clear();
                tbConfirmPassword.Clear();
                return;
            }  
            
            staff staff = new staff();
            staff.Username = username;
            staff.Password = password;
            context.staff.Add(staff);
            context.SaveChanges();
            MessageBox.Show("Register successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);         
        }
    }
}
