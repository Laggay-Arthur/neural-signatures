using System;
using Tesseract;
using System.IO;
using System.Windows;
using System.Drawing;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;

namespace neural_signatures
{
    public partial class MainWindow : Window
    {
        private string SafeFileName = "";
        DateTime? date_validity=null;
      public static List<string> FIOList = new List<string>(1);
        public MainWindow() { InitializeComponent();

            /*new Thread(() => {
                Action action = () =>
                {
                    DataBase db = new DataBase(ref comboboxFIO);
                    FIOList = db.SelectFIO();
                    foreach (string i in FIOList)
                    {
                        comboboxFIO.Items.Add(i);
                    }

                };
                Dispatcher.Invoke(action);
            }
                ).Start();*/

            FIOAsync(comboboxFIO);
           
        }

       public static async void FIOAsync(ComboBox comboboxFIO)
        {
            await Task.Run(() =>
              {

                  DataBase db = new DataBase();
                  FIOList = db.SelectFIO();
                  
              }
           
               );
            foreach (string i in FIOList)
            {
                comboboxFIO.Items.Add(i);
            }
        }
    
        OpenFileDialog openFileDialog = new OpenFileDialog();
        async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == true)
            {
                SafeFileName = openFileDialog.SafeFileName;
                string currDir = Environment.CurrentDirectory;
                string docDir = currDir + "\\documents\\";

                if (!Directory.Exists(docDir)) Directory.CreateDirectory(docDir);

                if (!File.Exists(docDir + openFileDialog.SafeFileName))
                    File.Copy(openFileDialog.FileName, docDir + openFileDialog.SafeFileName);
                
                //else
                //{
                //    MessageBox.Show("Файл с таким именем уже существует в проекте в папке /debug/documents. Скорее всего вы уже его загружали. Проверьте, если файлы действительно разные, то переименуйте его! Если файлы одинаковые, то запускайте из папки documents");
                //    progressbar1.Value = 0;
                //    return;
                //}

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
                    TesseractEngine ocr = new TesseractEngine("./tessdata", radio_lang);
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
            tww.f1 = this;
            tww.Show();
        }

        private void Btn_insert_to_db_Click(object sender, RoutedEventArgs e)
        {
            if ((date_validity == null) && (date_have.IsChecked==true))
            {
                MessageBox.Show("Пожалуйста, проверьте дату окончания документа, если она есть в договоре!");
                return;
            }
            DataBase db = new DataBase();
            if (date_have.IsChecked == false)
            {
                db.insert(SafeFileName, comboboxFIO.Text, textbox1.Text);
            }
            else db.insert(SafeFileName, comboboxFIO.Text, textbox1.Text, (DateTime)date_validity);
            MessageBox.Show("Документ добавлен!");
        }

        private void DatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            date_validity = (DateTime)DatePicker.SelectedDate;
        }

      public string setFIOBox
        {
            set { string i = value;
                comboboxFIO.Items.Add(i);
            }
            
        }
  
    }
}