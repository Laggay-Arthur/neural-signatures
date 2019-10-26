using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace neural_signatures
{
    public partial class TrainWebWindow : Window
    {
        public TrainWebWindow() { InitializeComponent(); }


        OpenFileDialog openFileDialog = new OpenFileDialog();
        private void LoadTrain_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                loaded_img.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}