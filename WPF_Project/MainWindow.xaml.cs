using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Project.Models;

namespace WPF_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FarmManagementContext context;
        public MainWindow()
        {
            context = new FarmManagementContext();
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            harvestProcessListView.ItemsSource = context.HarvestProcesses.ToList();
            produceListView.ItemsSource = context.Produces.ToList();
            careProcessListView.ItemsSource = context.CareProcesses.ToList();
            preservationProcessListView.ItemsSource = context.PreservationProcesses.ToList();
        }
    }
}