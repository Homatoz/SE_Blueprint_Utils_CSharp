using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml;

namespace BlueprintUtils {
    /// <summary>
    /// Логика взаимодействия для Converter.xaml
    /// </summary>
    public partial class Converter : Window {
        static readonly System.Windows.Forms.OpenFileDialog FileBrowser = new();
        static readonly System.Windows.Forms.FolderBrowserDialog FolderBrowserRead = new();
        static readonly System.Windows.Forms.FolderBrowserDialog FolderBrowserSave = new();

        public string ConvertType { get; set; }

        public string PathToExtracted { get; set; }

        //Шаблон XML для чертежей
        string BPTemplate = "<?xml version=\"1.0\"?>" +
        "<Definitions xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
        "<ShipBlueprints>" +
        "<ShipBlueprint xsi:type=\"MyObjectBuilder_ShipBlueprintDefinition\">" +
        "<Id Type=\"MyObjectBuilder_ShipBlueprintDefinition\"/>" +
        "<CubeGrids>" +
        "</CubeGrids>" +
        "</ShipBlueprint>" +
        "</ShipBlueprints>" +
        "</Definitions>";

        //Переменные для работы c SelectNodes
        string XPathCubeGrid = "/MyObjectBuilder_Sector/SectorObjects/MyObjectBuilder_EntityBase[@xsi:type=\"MyObjectBuilder_CubeGrid\"] | /MyObjectBuilder_Sector/SectorObjects/MyObjectBuilder_EntityBase[@xsi:type=\"MyObjectBuilder_ProxyAntenna\"]/ComponentContainer/Components/ComponentData/Component[@xsi:type=\"MyObjectBuilder_UpdateTrigger\"]/SerializedPirateStation";
        string XPathProjector = "./CubeBlocks/MyObjectBuilder_CubeBlock/ProjectedGrid | ./CubeBlocks/MyObjectBuilder_CubeBlock/ProjectedGrids/MyObjectBuilder_CubeGrid";

        //Объекты для работы с XML
        XmlDocument XMLSandbox = new();
        XmlDocument XMLSave = new();
        XmlDocument XMLExtracted = new();

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
            InitializeComponent();
        }

        private void Logging(string message) {
            tbLog.Text = tbLog.Text + message + "\n";
        }

        //Функция для удаления всякого трэша
        private void RemoveNodes(XmlDocument XML, string Name) {
            foreach (XmlNode node in XML.SelectNodes("//" + Name)) {
                node.ParentNode.RemoveChild(node);
            }
        }

        //Функция для выгрызания чертежей из проекторов
        private void ExtractProjectorGrid(XmlNode Node, string FileName) {
            int Projector = 0;
            foreach (XmlElement node in Node.SelectNodes(XPathProjector)) {
                Projector += 1;
                node.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                XMLSave.LoadXml(node.OuterXml);
                XMLSave.Save(PathToExtracted + FileName + "p" + Projector);
                ExtractProjectorGrid(node, FileName + "p" + Projector);
            }
        }

        private void ExtractGrid(string PathToSandbox, string BPPath) {
            //Проверяем наличие файла SANDBOX_0_0_0_.sbs
            if (!File.Exists(PathToSandbox)) {
                Logging("Файл SANDBOX_0_0_0_.sbs отсутствует, обработка не может быть произведена");
                return;
            }

            //Инициализируем переменные
            int CubeGridFile = 0;           //Используется как имя файла для извлеченных CubeGrid
            int CountEntity = 0;            //Используется для отображения процесса при проверке связей. Процесс иногда слишком долгий...
            PathToExtracted = BPPath + @"\extracted\";

            //Создаем папку для выгрызенных объектов
            Directory.CreateDirectory(PathToExtracted);

            //Обрабатываем файл Sandbox
            Logging("Загружаем и чистим Sandbox");
            Logging("");

            XMLSandbox.Load(PathToSandbox);
            XmlNamespaceManager SENamespace = new(XMLSandbox.NameTable);
            SENamespace.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            if (Settings.ClearOwner) {
                RemoveNodes(XMLSandbox, "Owner");
                RemoveNodes(XMLSandbox, "BuiltBy");
            }

            if (Settings.RemoveDeformation) {
                RemoveNodes(XMLSandbox, "Skeleton");
            }

            if (Settings.RemoveAI) {
                RemoveNodes(XMLSandbox, "AutomaticBehaviour");
            }

            Logging("Выгрызаем CubeGrid из Sandbox");
            Logging("");

            foreach (XmlNode node in XMLSandbox.SelectNodes(XPathCubeGrid, SENamespace)) {
                CubeGridFile += 1;
                XMLSave.LoadXml(node.OuterXml);
                XMLSave.Save(PathToExtracted + CubeGridFile.ToString().PadLeft(4, '0'));
                if (Settings.ExtractProjectorBP && node.SelectSingleNode(XPathProjector) != null) {
                    ExtractProjectorGrid(node, CubeGridFile.ToString().PadLeft(4, '0'));
                }
            }

            //Обрабатываем полученные объекты CubeGrid
            Logging("Создаем чертежи");

            string[] ExtractedFiles = Directory.GetFiles(PathToExtracted);

            foreach (string file in ExtractedFiles) {
                //Забираем файл
                XMLExtracted.Load(file);
                //Получаем название для чертежа из DisplayName
                string DisplayName = XMLExtracted.SelectSingleNode("*/DisplayName").InnerText;
                DisplayName = Regex.Replace(DisplayName, "[\"?]", "_");
                //Создаем чертеж
                XMLSave.LoadXml(BPTemplate);
                XmlNode CubeGridsNode = XMLSave.SelectSingleNode("//CubeGrids");
                XmlNode CubeGridNewNode = XMLSave.CreateElement("CubeGrid");
                CubeGridsNode.AppendChild(CubeGridNewNode);
                CubeGridNewNode.InnerXml = XMLExtracted.FirstChild.InnerXml;
                (XMLSave.SelectSingleNode("//Id[@Type=\"MyObjectBuilder_ShipBlueprintDefinition\"]") as XmlElement).SetAttribute("Subtype", DisplayName);
                //Создаем папку чертежа и сохраняем в нее чертеж
                string Path = file.Replace(@"\extracted", "") + "_" + DisplayName;
                string BPFile = Path + @"\bp.sbc";
                Directory.CreateDirectory(Path);
                XMLSave.Save(BPFile);
                //Выводим название чертежа и, при наличии тега AutomaticBehaviour, предупреждаем
                XmlNode AutomaticBehaviour = XMLSave.SelectSingleNode("//AutomaticBehaviour");
                if (AutomaticBehaviour != null) {
                    Logging(DisplayName + " (присутствует AutomaticBehaviour)");
                }
                else {
                    Logging(DisplayName);
                }
            }
            Logging("");
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
                            MessageBox.Show("Не выбрана папка для сохранения чертежа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
