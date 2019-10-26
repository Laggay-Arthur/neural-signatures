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
        private string SafeFileName = "";
        public MainWindow() { InitializeComponent(); }

        OpenFileDialog openFileDialog = new OpenFileDialog();
        async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == true)
            {
                SafeFileName = openFileDialog.SafeFileName;
                string currDir = Environment.CurrentDirectory;
                string docDir = currDir + "\\documents\\";

                if (!Directory.Exists(docDir)) Directory.CreateDirectory(docDir);
                if (openFileDialog.FileName != (docDir + openFileDialog.SafeFileName))
                {
                    if (!File.Exists(docDir + openFileDialog.SafeFileName))
                        File.Copy(openFileDialog.FileName, docDir + openFileDialog.SafeFileName);

                    else
                    {
                        MessageBox.Show("Файл с таким именем уже существует в проекте в папке /debug/documents. Скорее всего вы уже его загружали. Проверьте, если файлы действительно разные, то переименуйте его! Если файлы одинаковые, то запускайте из папки documents");
                        progressbar1.Value = 0;
                        return;
                    }
                }
                //loaded_img.Source = new BitmapImage(new Uri(openFileDialog.FileName));

                string radio_lang = "rus";
                if ((bool)radio_rus.IsChecked)
                { radio_lang = "rus"; }
                else if ((bool)radio_eng.IsChecked)
                { radio_lang = "eng"; }
                else if ((bool)radio_rus_eng.IsChecked)
                { radio_lang = "rus+eng"; }

                Bitmap img = new Bitmap(openFileDialog.FileName);

                string s = "";
                await Task.Run(() =>
                {
                    TesseractEngine ocr = new TesseractEngine("./tessdata", radio_lang, EngineMode.CubeOnly);
                    var page = ocr.Process(img, PageSegMode.Auto);
                    s = page.GetText();
                });
                btn_insert_to_db.Visibility = Visibility.Visible;
                textbox1.Text = s;
            }
        }

        void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();
        SetPositionByHand spbh = null;
        void set_by_hand_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.FileName.Length > 0)
            {
                spbh = new SetPositionByHand(openFileDialog.FileName);
                spbh.Show();
            }
            else { MessageBox.Show("Укажите какой скан загружать"); }
        }
        TrainWebWindow tww = null;
        void TrainWeb_Click(object sender, RoutedEventArgs e)
        {
            if (tww != null)
            {
                tww.Close();
            }
            tww = new TrainWebWindow();
            tww.Show();
        }

        private void Btn_insert_to_db_Click(object sender, RoutedEventArgs e)
        {
            DataBase db = new DataBase();

            db.insert(SafeFileName, "Вася", textbox1.Text);
            MessageBox.Show("Документ добавлен!");
        }
    }
}