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
    /// Interaction logic for HarvestProcess.xaml
    /// </summary>
    public partial class HarvestProcessWindow : Window
    {
        public FarmManagementContext context;
        public HarvestProcessWindow()
        {
            context = new FarmManagementContext();
            InitializeComponent();
        }

        private void btHome_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            this.Hide();
            home.Show();
        }
        private void LoadData()
        {
            lvHarvestProcess.ItemsSource = context.HarvestProcesses.Include(x=>x.Product).ToList();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        public HarvestProcess GetHarvestProcess()
        {
            HarvestProcess harvestProcess = new HarvestProcess();

            if (tbHarvestProcessId.Text != "")
            {
                harvestProcess.HarvestProcessId = int.Parse(tbHarvestProcessId.Text);
            }

            // Tạo một đối tượng Product mới và gán giá trị cho thuộc tính Name
            harvestProcess.Product = new Product();
            harvestProcess.Product.Name = tbProductName.Text;
            harvestProcess.QuantityHarvested = int.Parse(tbQuantity.Text);
            harvestProcess.Date = tbDate.SelectedDate;
            harvestProcess.Notes = tbNotes.Text;
          
        
            return harvestProcess;
        }
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HarvestProcess harvest = GetHarvestProcess();

                Product newProduct = new Product();
                newProduct.Name = harvest.Product.Name;

                HarvestProcess addNewHarvestProcess = new HarvestProcess();
                addNewHarvestProcess.Product = newProduct;

                addNewHarvestProcess.Date = harvest.Date;
                addNewHarvestProcess.QuantityHarvested = harvest.QuantityHarvested;
                addNewHarvestProcess.Notes = harvest.Notes;

                context.HarvestProcesses.Add(addNewHarvestProcess);
                context.SaveChanges();
                LoadData();
                MessageBox.Show($"Harvest {harvest.QuantityHarvested} kg {harvest.Product.Name} successfully", "Insert HarvestProcess");
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
                HarvestProcess newHarvestProcess = GetHarvestProcess();
                newHarvestProcess.HarvestProcessId = int.Parse(tbHarvestProcessId.Text);

                HarvestProcess oldHarvestProcess = context.HarvestProcesses.FirstOrDefault(x => x.HarvestProcessId == newHarvestProcess.HarvestProcessId);
                oldHarvestProcess.Product.Name = newHarvestProcess.Product.Name;
                oldHarvestProcess.QuantityHarvested = newHarvestProcess.QuantityHarvested;
                oldHarvestProcess.Date = newHarvestProcess.Date;
                oldHarvestProcess.Notes = newHarvestProcess.Notes;

                context.SaveChanges();
                LoadData();
                MessageBox.Show($"Harvest process {newHarvestProcess.Product.Name} updated successfully", "Update HarvestProcess");
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
                    int harvestProcessId = int.Parse(tbHarvestProcessId.Text);
                    var harvestProcessName = context.HarvestProcesses.Where(p => p.HarvestProcessId == harvestProcessId).Select(p => p.Product.Name).FirstOrDefault();
                    context.Database.ExecuteSqlRaw($"delete HarvestProcess where HarvestProcessId ={harvestProcessId}");
                    context.SaveChanges();
                    LoadData();
                    MessageBox.Show($"Care process {harvestProcessName} deleted successfully", "Remove Harvest Process");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            tbHarvestProcessId.Clear();
            tbProductName.Clear();
            tbDate.SelectedDate = null;
            tbQuantity.Clear();
            tbNotes.Clear();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? id = null;
                if (tbHarvestProcessId.Text != "")
                {
                    id = int.Parse(tbHarvestProcessId.Text);
                }
                int? quantity = null;
                if(tbQuantity.Text != "")
                {
                    quantity = int.Parse(tbQuantity.Text);
                }
                string name = tbProductName.Text;
               


                // Truy vấn cơ sở dữ liệu để lấy ra các sản phẩm phù hợp với điều kiện tìm kiếm
                var query = context.HarvestProcesses.AsQueryable();

                if (id != null)
                {
                    query = query.Where(p => p.HarvestProcessId == id);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.Product.Name.Contains(name));
                }

                if (quantity != null)
                {
                    query = query.Where(p => p.QuantityHarvested == quantity);
                }

                // Gán kết quả tìm kiếm cho ItemsSource của ListView hoặc DataGrid
                lvHarvestProcess.ItemsSource = query.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    
}
