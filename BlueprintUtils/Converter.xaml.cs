using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using static System.Net.WebRequestMethods;

namespace BlueprintUtils {
    /// <summary>
    /// Логика взаимодействия для Converter.xaml
    /// </summary>
    public partial class Converter : Window {
        static readonly System.Windows.Forms.OpenFileDialog FileBrowser = new();
        static readonly System.Windows.Forms.FolderBrowserDialog FolderBrowserRead = new();
        static readonly System.Windows.Forms.FolderBrowserDialog FolderBrowserSave = new();

        public string ConvertType { get; set; }

        static Converter() {
            FileBrowser.Filter = "Файл мира|SANDBOX_0_0_0_.sbs";
            FileBrowser.Title = "Выберите файл мира";
            FolderBrowserRead.Description = "Выберите папку с файлами мира";
            FolderBrowserRead.UseDescriptionForTitle = true;
            FolderBrowserSave.InitialDirectory = Environment.ExpandEnvironmentVariables("%APPDATA%") + @"\SpaceEngineers\Blueprints\local\";
            FolderBrowserSave.Description = "Выберите папку для сохранения чертежей";
            FolderBrowserSave.UseDescriptionForTitle = true;
        }
        public Converter(string Type) {
            ConvertType = Type;
            //MessageBox.Show(ConvertType);
            InitializeComponent();
        }

        private void ExtractGrid(string PathToSandbox, string BPPath) {
            throw new NotImplementedException();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            switch (ConvertType) {
                case "Current":
                    ExtractGrid(Environment.CurrentDirectory + @"\SANDBOX_0_0_0_.sbs", Environment.CurrentDirectory);
                    break;
                case "Single":
                    ExtractGrid(FileBrowser.FileName, FolderBrowserSave.SelectedPath);
                    break;
                case "Multi":
                    string ReadPath = FolderBrowserRead.SelectedPath;
                    string SavePath = FolderBrowserSave.SelectedPath;
                    string[] SandboxAllFiles = Directory.GetFiles(ReadPath, "SANDBOX_0_0_0_.sbs", SearchOption.AllDirectories);
                    foreach (string SandboxFile in SandboxAllFiles) {
                        string BPSavePath = Directory.GetParent(SandboxFile).FullName.Replace(ReadPath, SavePath);
                        ExtractGrid(SandboxFile, BPSavePath);
                    }
                    break;
            }

        }

        private void Window_Initialized(object sender, EventArgs e) {
            switch (ConvertType) {
                case "Current":
                    this.ShowDialog();
                    break;
                case "Single":
                    if (FileBrowser.ShowDialog().ToString() == "OK") {
                        FileBrowser.InitialDirectory = Directory.GetParent(FileBrowser.FileName).FullName;
                        FolderBrowserSave.SelectedPath = "";
                        if (FolderBrowserSave.ShowDialog().ToString() == "OK") {
                            this.ShowDialog();
                        }
                        else {
                            MessageBox.Show("Не выбрана папка для сохранения чертежа","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                            this.Close();
                        }
                    }
                    else {
                        MessageBox.Show("Не выбран файл мира", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        this.Close();
                    }
                    break;
                case "Multi":
                    FolderBrowserRead.SelectedPath = "";
                    FolderBrowserSave.SelectedPath = "";
                    if (FolderBrowserRead.ShowDialog().ToString() == "OK") {
                        FolderBrowserRead.InitialDirectory = FolderBrowserRead.SelectedPath;
                        if (FolderBrowserSave.ShowDialog().ToString() == "OK") {
                            this.ShowDialog();
                        }
                        else {
                            MessageBox.Show("Не выбрана папка для сохранения чертежа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            this.Close();
                        }
                    }
                    else {
                        MessageBox.Show("Не выбрана папка с файлами мира", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        this.Close();
                    }
                    break;
            }
        }
    }
}
