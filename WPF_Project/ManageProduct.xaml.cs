using Microsoft.EntityFrameworkCore;
using System.Windows;
using WPF_Project.Models;

namespace WPF_Project
{
    /// <summary>
    /// Interaction logic for ManageProduct.xaml
    /// </summary>
    public partial class ManageProduct : Window
    {
        public FarmManagementContext context;
        public ManageProduct()
        {
            context = new FarmManagementContext();
            InitializeComponent();
            List<string> seasons = new List<string>
            {
                "Mùa xuân", "Mùa hè","Mùa thu","Mùa đông"
            };
            cbSeason.ItemsSource = seasons;
        }
        private void LoadData()
        {
            lvProducts.ItemsSource = context.Products.ToList();
        }

        private void btHome_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            this.Hide();
            home.Show();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public Product GetProductFromUI()
        {
            Product product = new Product();
            if (tbProductId.Text != "")
            {
                product.ProductId = int.Parse(tbProductId.Text);
            }
            product.Name = tbProductName.Text;
            product.Description = tbDescription.Text;
            product.Type = tbType.Text;
            product.Season = cbSeason.SelectedValue.ToString();
            product.Quantity = int.Parse(tbQuantity.Text);
            return product;
        }
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product p = GetProductFromUI();
                Product productToAdd = new Product();
                productToAdd.Name = p.Name;
                productToAdd.Type = p.Type;
                productToAdd.Description = p.Description;
                productToAdd.Season = p.Season;
                productToAdd.Quantity = p.Quantity;

                context.Products.Add(productToAdd);
                context.SaveChanges();
                LoadData();
                MessageBox.Show($"Product {p.Name} inserted successfully", "Insert Product");
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
                Product newProduct = GetProductFromUI();
                newProduct.ProductId = int.Parse(tbProductId.Text);

                Product oldProduct = context.Products.FirstOrDefault(x => x.ProductId == newProduct.ProductId);
                oldProduct.Name = newProduct.Name;
                oldProduct.Type = newProduct.Type;
                oldProduct.Quantity = newProduct.Quantity;
                oldProduct.Season = newProduct.Season;
                oldProduct.Quantity = newProduct.Quantity;
                oldProduct.Description = newProduct.Description;
                context.SaveChanges();
                LoadData();
                MessageBox.Show($"Product {newProduct.Name} updated successfully", "Update Product");
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
                    int proID = int.Parse(tbProductId.Text);
                    var productName = context.Products.Where(p => p.ProductId == proID).Select(p => p.Name).FirstOrDefault();
                    context.Database.ExecuteSqlRaw($"delete Product where ProductID ={proID}");
                    context.SaveChanges();
                    LoadData();
                    MessageBox.Show($"Product {productName} deleted successfully", "Remove Product");
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
                if (tbProductId.Text != "")
                {
                    id = int.Parse(tbProductId.Text);
                }
                string name = tbProductName.Text;
                string type = tbType.Text;
                string season = cbSeason.Text;

                // Truy vấn cơ sở dữ liệu để lấy ra các sản phẩm phù hợp với điều kiện tìm kiếm
                var query = context.Products.AsQueryable();

                if (id != null)
                {
                    query = query.Where(p => p.ProductId == id);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.Name.Contains(name));
                }

                if (!string.IsNullOrEmpty(type))
                {
                    query = query.Where(p => p.Type.Contains(type));
                }

                if (!string.IsNullOrEmpty(season))
                {
                    query = query.Where(p => p.Season.Contains(season));
                }

                // Gán kết quả tìm kiếm cho ItemsSource của ListView hoặc DataGrid
                lvProducts.ItemsSource = query.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

            tbProductId.Clear();
            tbProductName.Clear();
            tbDescription.Clear();
            tbType.Clear();
            tbQuantity.Clear();
            cbSeason.SelectedItem = null;
        }
    }
}
