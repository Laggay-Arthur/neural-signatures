using System;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ComboBox = System.Windows.Controls.ComboBox;


namespace neural_signatures
{
    public partial class TrainWebWindow : Window
    {
        ComboBox baseComboboxFIO;
        public TrainWebWindow(ref System.Windows.Controls.ComboBox combo)
        {
            InitializeComponent();

            baseComboboxFIO = combo;
            Task.Run(() =>
            {//Получаем список сотрудников
                Dispatcher.Invoke(() => {
                    foreach (string i in DataBase.SelectFIO())
                        comboboxFIO.Items.Add(i);
                });
                
            });
        }

        System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
        void LoadTrain_Click(object sender, RoutedEventArgs e)
        {//Загружаем подпись для обучения нейросети
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                loaded_img.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        async void Button_Click(object sender, RoutedEventArgs e)
        {//Добавление нового сотрудника в БД
            if (insertFIO.Text.Length > 0)
            {
                await Task.Run(() =>
                {
                    Dispatcher.Invoke( () =>
                    {
                        DataBase.insertFIOToDB(insertFIO.Text);

                        comboboxFIO.Items.Add(insertFIO.Text);
                        baseComboboxFIO.Items.Add(insertFIO.Text);
                    });
                });
                System.Windows.MessageBox.Show("Сотрудник добавлен в БД!");
            }
        }
    }
}
