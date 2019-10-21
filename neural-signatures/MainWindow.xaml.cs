using System;
using Tesseract;
using System.IO;
using System.Windows;
using System.Drawing;
using Microsoft.Win32;
using System.Threading.Tasks;


namespace neural_signatures
{
    public partial class MainWindow : Window
    {
        public MainWindow() { InitializeComponent(); }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string radio_lang = "rus";

                Bitmap img = new Bitmap(openFileDialog.FileName);
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string PathFolder1 = Path.GetDirectoryName(path);
                string PathFolder2 = Path.GetDirectoryName(PathFolder1);
                PathFolder1 = Path.GetDirectoryName(PathFolder2);
                PathFolder1 = PathFolder1.ToString() + @"\documents\" + openFileDialog.SafeFileName;
                if (!File.Exists(PathFolder1))
                {
                    File.Copy(openFileDialog.FileName, PathFolder1, true);
                }
                else if (!(openFileDialog.FileName == PathFolder1))
                {
                    MessageBox.Show("Файл с таким именем уже существует в проекте в папке /debug/documents. Скорее всего вы уже его загружали. Проверьте, если файлы действительно разные, то переименуйте его! Если файлы одинаковые, то запускайте из папки documents");
                    progressbar1.Value = 0;
                    return;
                }

                if ((bool)radio_rus.IsChecked)
                { radio_lang = "rus"; }
                else if ((bool)radio_eng.IsChecked)
                { radio_lang = "eng"; }
                else if ((bool)radio_rus_eng.IsChecked)
                { radio_lang = "rus+eng"; }

                string s = "";
                await Task.Run(() =>
                {
                    TesseractEngine ocr = new TesseractEngine("./tessdata", radio_lang, EngineMode.CubeOnly);
                    var page = ocr.Process(img, PageSegMode.Auto);
                    s = page.GetText();
                });
                textbox1.Text = s;
            }
        }
    }
}