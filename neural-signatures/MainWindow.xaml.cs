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
using Tesseract;
using Microsoft.Win32;
using System.Drawing;
using System.Threading;
using System.Windows.Threading;
using System.IO;

namespace neural_signatures
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

       private void Button_Click(object sender, RoutedEventArgs e)
        {
            

            progressbar1.Value = 0;
            OpenFileDialog openFileDialog = new OpenFileDialog();
           if( openFileDialog.ShowDialog() == true)
            {
                
                while (progressbar1.Value < 50)
                {
                    progressbar1.Value++;
                    
                }


                /*Thread thread = new Thread(new ParameterizedThreadStart(UpdateTextWrong));
                thread.Start(openFileDialog);*/
                new Thread(() =>
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
               (ThreadStart)delegate ()
               {
                   //OpenFileDialog openFileDialog = (OpenFileDialog)obj;

                   string radio_lang="rus";
                   bool radio_bool = true;
                   Bitmap img = new Bitmap(openFileDialog.FileName);
                   string path = AppDomain.CurrentDomain.BaseDirectory;
                   string PathFolder1 = System.IO.Path.GetDirectoryName(path);
                   string PathFolder2 = System.IO.Path.GetDirectoryName(PathFolder1);
                   PathFolder1 = System.IO.Path.GetDirectoryName(PathFolder2);
                   PathFolder1 = PathFolder1.ToString() + @"\documents\"+ openFileDialog.SafeFileName;
                   if (!File.Exists(PathFolder1))
                   {
                       File.Copy(openFileDialog.FileName.ToString(), PathFolder1, true);
                   }
                   else if (!(openFileDialog.FileName.ToString() == PathFolder1))
                   {
                       MessageBox.Show("Файл с таким именем уже существует в проекте в папке /debug/documents. Скорее всего вы уже его загружали. Проверьте, если файлы действительно разные, то переименуйте его! Если файлы одинаковые, то запускайте из папки documents");
                       progressbar1.Value = 0;
                       return;
                   }
                   while (radio_bool) {
                       if ((bool)radio_rus.IsChecked) {
                           radio_lang = "rus";
                           radio_bool = false;
                       } else if ((bool)radio_eng.IsChecked)
                       {
                           radio_lang = "eng";
                           radio_bool = false;
                       }else if ((bool)radio_rus_eng.IsChecked)
                       {
                           radio_lang = "rus+eng";
                           radio_bool = false;
                       }
                   }
                   TesseractEngine ocr = new TesseractEngine("./tessdata", radio_lang, EngineMode.Default);
                   var page = ocr.Process(img);
                   progressbar1.Value = 100;
                   textbox1.Text = page.GetText();


                 
               });
                }).Start();

             
            }
        }
       
     

    }
}
