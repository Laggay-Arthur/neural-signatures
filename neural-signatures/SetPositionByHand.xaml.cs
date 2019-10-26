using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace neural_signatures
{
    public partial class SetPositionByHand : Window
    {
        public SetPositionByHand(string fileName)
        {
            InitializeComponent();
            loaded_img.Source = new BitmapImage(new Uri(fileName));
        }

        void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}