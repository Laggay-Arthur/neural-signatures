using System;
using Tesseract;
using System.IO;
using System.Windows;
using System.Drawing;
using Microsoft.Win32;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.Generic;


namespace neural_signatures
{
    public partial class MainWindow : Window
    {
        private string SafeFileName = "";
        DateTime? date_validity;
        public MainWindow()
        {
            InitializeComponent();
            DataBase.Init(ref this.comboboxFIO);//Передаём элементы в который можно добавить нового сотрудника
            getFIOAsync();
        }

        async void getFIOAsync()
        {//Получаем список сотрудников и выводим их списом
            await Task.Run(() =>
            {
                foreach (string i in DataBase.SelectFIO())
                    comboboxFIO.Items.Add(i);
            });
        }

        OpenFileDialog openFileDialog = new OpenFileDialog();
        void Button_Click(object sender, RoutedEventArgs e)
        {//Нажата кнопка загрузки скана
            if (openFileDialog.ShowDialog() == true)
            {
                SafeFileName = openFileDialog.SafeFileName;
                string currDir = Environment.CurrentDirectory;
                string docDir = currDir + "\\documents\\";

                if (!Directory.Exists(docDir)) Directory.CreateDirectory(docDir);

                if (!File.Exists(docDir + openFileDialog.SafeFileName))
                    File.Copy(openFileDialog.FileName, docDir + openFileDialog.SafeFileName);
                set_by_hand.Visibility = Visibility.Visible;
            }
        }

        void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();
        SetPositionByHand spbh;//Окно, в котором можно вручную указать расположение подписи
        void set_by_hand_Click(object sender, RoutedEventArgs e)
        {//Открывает окно для ручного указания расположения подписи
            if (openFileDialog.FileName.Length > 0)
            {
                spbh = new SetPositionByHand(openFileDialog.FileName);
                spbh.Show();
            }
            else { MessageBox.Show("Укажите какой скан загружать"); }
        }
        TrainWebWindow tww;//Окно, в котором можно обучать нейросеть
        void TrainWeb_Click(object sender, RoutedEventArgs e)
        {//Открывает окно для обучения нейросети
            if (tww != null)
            {
                tww = null;
            }
            tww = new TrainWebWindow(this.comboboxFIO)
            tww.Show();
        }

        void Btn_insert_to_db_Click(object sender, RoutedEventArgs e)
        {
            if ((date_validity == null) && (date_have.IsChecked == true))
            {
                MessageBox.Show("Пожалуйста, укажите дату окончания документа!");
                return;
            }
            if (date_have.IsChecked == false)
                DataBase.insert(SafeFileName, comboboxFIO.Text, textbox1.Text);
            else DataBase.insert(SafeFileName, comboboxFIO.Text, textbox1.Text, (DateTime)date_validity);
            MessageBox.Show("Документ добавлен!");
        }

        void DatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) => date_validity = (DateTime)datePicker.SelectedDate;

        void date_have_Click(object sender, RoutedEventArgs e)
        {//Скрываем/показываем выбор даты окончания
            if (date_have.IsChecked == true)
                datePicker.Visibility = Visibility.Visible;

            else
                datePicker.Visibility = Visibility.Hidden;
        }

        async void getTextFromImage_Click(object sender, RoutedEventArgs e)
        {//Считывание текста со скана
            if (openFileDialog.FileName.Length == 0) return;
            progressbar1.Value = 0;
            string radio_lang = (bool)radio_rus.IsChecked ? "rus" : (bool)radio_eng.IsChecked ? "eng" : (bool)radio_rus_eng.IsChecked ? "rus+eng" : "";

            Bitmap img = new Bitmap(openFileDialog.FileName);

            string s = "";
            Task progress;
            await Task.Run(() =>
            {//Запускаем в отдельном потоке увеличение прогрессбара и распознавание текста
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                progress = Dispatcher.Invoke(async () =>
                    {
                        while (progressbar1.Value < 100)
                        {
                            progressbar1.Value++;

                            await Task.Delay(100);
                        }
                    }, System.Windows.Threading.DispatcherPriority.Normal, token);

                TesseractEngine ocr = new TesseractEngine("./tessdata", radio_lang);
                var page = ocr.Process(img, PageSegMode.Auto);
                s = page.GetText();
                if (!progress.IsCompleted)
                {
                    token.ThrowIfCancellationRequested(); Dispatcher.Invoke(() => progressbar1.Value = 100);
                }
            });
            btn_insert_to_db.Visibility = Visibility.Visible;
            textbox1.Text = s;
            img.Dispose();
        }

        void getGignatures_Click(object sender, RoutedEventArgs e)
        {//Автоопределение положения подписи

        }
    }
}
