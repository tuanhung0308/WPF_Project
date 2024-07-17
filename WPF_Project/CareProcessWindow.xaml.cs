using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for CareProcess.xaml
    /// </summary>
    public partial class CareProcessWindow : Window
    {
        public FarmManagementContext context;
        public CareProcessWindow()
        {
            context = new FarmManagementContext();
            InitializeComponent();
        }
        private void LoadData()
        {
            lvCareProcess.ItemsSource = context.CareProcesses.Include(x => x.Product).ToList();
        }
        private void btHome_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            this.Hide();
            home.Show();
        }
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        public CareProcess GetCareProcess()
        {
            CareProcess careProcess = new CareProcess();
            if (tbCareProcessId.Text != "")
            {
                careProcess.CareProcessId = int.Parse(tbCareProcessId.Text);
            }

            // Tạo một đối tượng Product mới và gán giá trị cho thuộc tính Name
            careProcess.Product = new Product();
            careProcess.Product.Name = tbProductName.Text;
            careProcess.Activity = tbActivity.Text;
            careProcess.Date = tbDate.SelectedDate;
            careProcess.Notes = tbNotes.Text;
            return careProcess;
        }
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CareProcess care = GetCareProcess();

                Product newProduct = new Product();
                newProduct.Name = care.Product.Name;

                CareProcess addnewCareProcess = new CareProcess();
                addnewCareProcess.Product = newProduct;
                addnewCareProcess.Activity = care.Activity;
                addnewCareProcess.Date = care.Date;
                addnewCareProcess.Notes = care.Notes;

                context.CareProcesses.Add(addnewCareProcess);
                context.SaveChanges();
                LoadData();
                MessageBox.Show($"Care Process {care.Activity} inserted successfully", "Insert CareProcess");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CareProcess newCareProcess = GetCareProcess();
                newCareProcess.CareProcessId = int.Parse(tbCareProcessId.Text);

                CareProcess oldCareProcess = context.CareProcesses.FirstOrDefault(x => x.CareProcessId == newCareProcess.CareProcessId);
                oldCareProcess.Product.Name = newCareProcess.Product.Name;
                oldCareProcess.Activity = newCareProcess.Activity;
                oldCareProcess.Date = newCareProcess.Date;
                oldCareProcess.Notes = newCareProcess.Notes;

                context.SaveChanges();
                LoadData();
                MessageBox.Show($"Care process {newCareProcess.Activity} updated successfully", "Update CareProcess");
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
                    int careProcessId = int.Parse(tbCareProcessId.Text);
                    var careProcessName = context.CareProcesses.Where(p => p.CareProcessId == careProcessId).Select(p => p.Activity).FirstOrDefault();
                    context.Database.ExecuteSqlRaw($"delete CareProcess where CareProcessID ={careProcessId}");
                    context.SaveChanges();
                    LoadData();
                    MessageBox.Show($"Care process {careProcessName} deleted successfully", "Remove Care Process");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            tbCareProcessId.Clear();
            tbProductName.Clear();
            tbDate.SelectedDate = null;
            tbActivity.Clear();
            tbNotes.Clear();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? id = null;
                if (tbCareProcessId.Text != "")
                {
                    id = int.Parse(tbCareProcessId.Text);
                }
                string name = tbProductName.Text;
                string activity = tbActivity.Text;


                // Truy vấn cơ sở dữ liệu để lấy ra các sản phẩm phù hợp với điều kiện tìm kiếm
                var query = context.CareProcesses.AsQueryable();

                if (id != null)
                {
                    query = query.Where(p => p.CareProcessId == id);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.Product.Name.Contains(name));
                }

                if (!string.IsNullOrEmpty(activity))
                {
                    query = query.Where(p => p.Activity.Contains(activity));
                }

                // Gán kết quả tìm kiếm cho ItemsSource của ListView hoặc DataGrid
                lvCareProcess.ItemsSource = query.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
