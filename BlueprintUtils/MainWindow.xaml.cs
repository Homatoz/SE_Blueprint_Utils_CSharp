using System;
using System.Collections.Generic;
using System.IO;
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


namespace BlueprintUtils
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btnCurrent.IsEnabled = File.Exists(Environment.CurrentDirectory + @"\SANDBOX_0_0_0_.sbs");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Converter converter = new Converter((sender as Button).Name.Replace("btn",""));
        }
    }
}
