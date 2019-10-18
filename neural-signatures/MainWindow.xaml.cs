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

namespace neural_signatures
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
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
                   Bitmap img = new Bitmap(openFileDialog.FileName);
                   TesseractEngine ocr = new TesseractEngine("./tessdata", "eng+rus", EngineMode.Default);
                   var page = ocr.Process(img);
                   progressbar1.Value = 100;
                   textbox1.Text = page.GetText();
               });
                }).Start();

             
            }
        }
       
     

    }
}
