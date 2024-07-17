using System.Windows;
using WPF_Project.Models;

namespace WPF_Project
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public FarmManagementContext context;
        public Home()
        {
            context = new FarmManagementContext();
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {

        }
        private void btManageProduct_Click(object sender, RoutedEventArgs e)
        {
            ManageProduct product = new ManageProduct();
            this.Hide();
            product.Show();
        }

        private void btCareProcess_Click(object sender, RoutedEventArgs e)
        {
            CareProcessWindow care = new CareProcessWindow();
            this.Hide();
            care.Show();
        }

        private void btHarvestProcess_Click(object sender, RoutedEventArgs e)
        {
            HarvestProcessWindow harvest = new HarvestProcessWindow();
            this.Hide();
            harvest.Show();
        }

        private void btPreservationProcess_Click(object sender, RoutedEventArgs e)
        {
            PreservationProcessWindow preservation = new PreservationProcessWindow();
            this.Hide();
            preservation.Show();
        }

        private void btStaffManagement_Click(object sender, RoutedEventArgs e)
        {
            StaffManagement staffManagement = new StaffManagement();
            this.Hide();
            staffManagement.Show();
        }

        private void tbLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to sign out?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Login login = new Login();
                this.Hide();
                login.Show();
            }
        }

    }
}