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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WPF_Project
{
    /// <summary>
    /// Interaction logic for PreservationProcess.xaml
    /// </summary>
    public partial class PreservationProcessWindow : Window
    {
        public FarmManagementContext context;
        public PreservationProcessWindow()
        {
            context = new FarmManagementContext();
            InitializeComponent();
        }
        public void LoadData()
        {
            lvPreservationProcess.ItemsSource = context.PreservationProcesses.Include(x => x.Product).ToList();
        }
        private void btHome_Click_1(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            this.Hide();
            home.Show();
        }
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        public PreservationProcess GetPreservationProcess()
        {
            PreservationProcess preservationProcess = new PreservationProcess();
            if (tbPreservationProcessId.Text != "")
            {
                preservationProcess.PreservationProcessId = int.Parse(tbPreservationProcessId.Text);
            }

            // Tạo một đối tượng Product mới và gán giá trị cho thuộc tính Name
            preservationProcess.Product = new Product();
            preservationProcess.Product.Name = tbProductName.Text;
            preservationProcess.Method = tbMethod.Text;
            preservationProcess.PreservationCondition = tbPreservationCondition.Text;
            preservationProcess.ExpiryDate = tbExpiryDate.SelectedDate;
            preservationProcess.Notes = tbNotes.Text;
            return preservationProcess;
        }
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PreservationProcess preserve = GetPreservationProcess();

                Product newProduct = new Product();
                newProduct.Name = preserve.Product.Name;

                PreservationProcess addnewPreservationProcess = new PreservationProcess();
                addnewPreservationProcess.Product = newProduct;
                addnewPreservationProcess.Method = preserve.Method;
                addnewPreservationProcess.PreservationCondition = preserve.PreservationCondition;
                addnewPreservationProcess.ExpiryDate = preserve.ExpiryDate;
                addnewPreservationProcess.Notes = preserve.Notes;

                context.PreservationProcesses.Add(addnewPreservationProcess);
                context.SaveChanges();
                LoadData();
                MessageBox.Show($"Preservation Process {preserve.Method} inserted successfully", "Insert Preserve Process");
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
                PreservationProcess newPreserveProcess = GetPreservationProcess();
                newPreserveProcess.PreservationProcessId = int.Parse(tbPreservationProcessId.Text);

                PreservationProcess oldPreserveProcess = context.PreservationProcesses.FirstOrDefault(x => x.PreservationProcessId == newPreserveProcess.PreservationProcessId);
                oldPreserveProcess.Product.Name = newPreserveProcess.Product.Name;
                oldPreserveProcess.Method = newPreserveProcess.Method;
                oldPreserveProcess.PreservationCondition = newPreserveProcess.PreservationCondition;
                oldPreserveProcess.ExpiryDate = newPreserveProcess.ExpiryDate;
                oldPreserveProcess.Notes = newPreserveProcess.Notes;

                context.SaveChanges();
                LoadData();
                MessageBox.Show($"Preserve process {newPreserveProcess.Method},{newPreserveProcess.PreservationCondition} updated successfully", "Update PreservationProcess");
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
                    int preserveProcessId = int.Parse(tbPreservationProcessId.Text);
                    var preserveProcessName = context.PreservationProcesses.Where(p => p.PreservationProcessId == preserveProcessId).Select(p => p.Method).FirstOrDefault();
                    context.Database.ExecuteSqlRaw($"delete PreservationProcess where PreservationProcessID ={preserveProcessId}");
                    context.SaveChanges();
                    LoadData();
                    MessageBox.Show($"Care process {preserveProcessName} deleted successfully", "Remove Care Process");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            tbPreservationProcessId.Clear();
            tbProductName.Clear();
            tbExpiryDate.SelectedDate = null;
            tbMethod.Clear();
            tbPreservationCondition.Clear();
            tbNotes.Clear();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? id = null;
                if (tbPreservationProcessId.Text != "")
                {
                    id = int.Parse(tbPreservationProcessId.Text);
                }
                string method = tbMethod.Text;
                string condition = tbPreservationCondition.Text;


                // Truy vấn cơ sở dữ liệu để lấy ra các sản phẩm phù hợp với điều kiện tìm kiếm
                var query = context.PreservationProcesses.AsQueryable();

                if (id != null)
                {
                    query = query.Where(p => p.PreservationProcessId == id);
                }

                if (!string.IsNullOrEmpty(method))
                {
                    query = query.Where(p => p.Method.Contains(method));
                }

                if (!string.IsNullOrEmpty(condition))
                {
                    query = query.Where(p => p.PreservationCondition.Contains(condition));
                }

                // Gán kết quả tìm kiếm cho ItemsSource của ListView hoặc DataGrid
                lvPreservationProcess.ItemsSource = query.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

}   

