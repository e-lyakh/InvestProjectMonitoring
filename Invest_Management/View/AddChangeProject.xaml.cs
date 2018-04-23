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
using Invest_Management;
using Invest_Management.ViewModel;
using System.Collections;
using System.Windows.Markup;
using System.Globalization;

namespace Invest_Management.View
{
    /// <summary>
    /// Interaction logic for AddChangeProject.xaml
    /// </summary>
    public partial class AddChangeProject : Window
    {
        public AddChangeProject(AddChangeViewModel acwm)
        {
            InitializeComponent();
            
            this.DataContext = acwm;
            this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
        }              
    }    
}
