using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for StaffManagement.xaml
    /// </summary>
    public partial class StaffManagement : Window
    {
        public FarmManagementContext context;
        public StaffManagement()
        {
            context = new FarmManagementContext();
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            lvData.ItemsSource = context.staff.ToList();
        }
        private void btHome_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            this.Hide();
            home.Show();
        }
        public staff getStaffFromUI()
        {
            staff staff = new staff();
            if (tbStaffId.Text != "")
            {
                staff.StaffId = int.Parse(tbStaffId.Text);
            }
            staff.Username = tbUsername.Text;
            staff.Password = tbPassword.Text;
            staff.Address = tbAddress.Text;
            staff.Phone = long.Parse(tbPhone.Text);
            staff.Email = tbEmail.Text;
            return staff;
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                staff newStaff = getStaffFromUI();
                newStaff.StaffId = int.Parse(tbStaffId.Text);

                staff oldStaff = context.staff.FirstOrDefault(x => x.StaffId == newStaff.StaffId);
                oldStaff.Password = newStaff.Password;
                oldStaff.Address = newStaff.Address;
                oldStaff.Phone = newStaff.Phone;
                oldStaff.Email = newStaff.Email;

                context.SaveChanges();
                LoadData();
                MessageBox.Show($"Staff Information updated successfully", "Update Staff Information");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show(
                  "Are you sure want to delete?",
                  "Confirm Deletion",
                  MessageBoxButton.YesNo,
                  MessageBoxImage.Warning
                  );
                if (result == MessageBoxResult.Yes)
                {
                    int staffId = int.Parse(tbStaffId.Text);             
                    context.Database.ExecuteSqlRaw($"delete staff where StaffId ={staffId}");
                    context.SaveChanges();
                    LoadData();
                    MessageBox.Show($"Staff Information deleted successfully", "Remove Staff");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? id = null;
                if (tbStaffId.Text != "")
                {
                    id = int.Parse(tbStaffId.Text);
                }
                string name = tbUsername.Text;
            
                var query = context.staff.AsQueryable();

                if (id != null)
                {
                    query = query.Where(p => p.StaffId == id);
                }
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.Username.Contains(name));
                }

                lvData.ItemsSource = query.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    }

