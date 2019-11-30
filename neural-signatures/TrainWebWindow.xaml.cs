using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace neural_signatures
{
    public partial class TrainWebWindow : Window
    {
        List<string> FIOList = new List<string>(1);
        public MainWindow f1 { get; set; }
        public TrainWebWindow() { InitializeComponent();

         new Thread(() => {
                Action action = () =>
                {
                    DataBase db = new DataBase();
                    FIOList = db.SelectFIO();
                    foreach (string i in FIOList)
                    {
                        comboboxFIO.Items.Add(i);
                    }

                };
                Dispatcher.Invoke(action);

            }
                ).Start();
        }

       

        OpenFileDialog openFileDialog = new OpenFileDialog();
        private void LoadTrain_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                loaded_img.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            DataBase db = new DataBase();
          
            {
                db.insertToFIO(insertFIO.Text);
            }
           
            System.Windows.MessageBox.Show("Сотрудник добавлен в БД!");
            comboboxFIO.Items.Clear();
            new Thread(() => {
                Action action = () =>
                {
                    
                    FIOList = db.SelectFIO();
                    foreach (string i in FIOList)
                    {
                        comboboxFIO.Items.Add(i);
                    }

                };
                Dispatcher.Invoke(action);

            }
              ).Start();
            f1.comboboxFIO.Items.Add(insertFIO.Text);
        }
    }
    }