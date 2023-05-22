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

namespace BlueprintUtils
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool ClearOwner { get; set; }        //Удалять теги Owner и BuiltBy?
        public bool CreateMultiGrid { get; set; }    //Создавать чертежи объединенных объектов?
        public bool RemoveDeformation { get; set; }  //Удалять деформации объектов?
        public bool RemoveAI { get; set; }           //Удалять автоматическое поведение?
        public bool ExtractProjectorBP { get; set; } //Извлекать чертежи из проектора?

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            ClearOwner = false;
            CreateMultiGrid = true;
            RemoveDeformation = true;
            RemoveAI = true;
            ExtractProjectorBP = true;
        }

    }
}
