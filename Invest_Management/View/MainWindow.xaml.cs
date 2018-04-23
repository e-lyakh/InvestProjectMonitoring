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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Invest_Management.ViewModel;
using Invest_Management.View;
using Invest_Management.Model;

namespace Invest_Management
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel mvm;

        public MainWindow()
        {
            InitializeComponent();            

            mvm = new MainViewModel();
            this.DataContext = mvm;            
        }                  

        private void tvTypes_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            mvm.SelectedType = tvTypes.SelectedItem.ToString();
        }        
    }
}
