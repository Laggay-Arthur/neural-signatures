using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;


namespace neural_signatures
{
    public partial class SearchDocument : Window
    {
        public SearchDocument()
        {
            InitializeComponent();
            select_person.ItemsSource = DataBase.SelectFIO();
        }
        void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
        void ShowAll_Click(object sender, RoutedEventArgs e) => datagrid.ItemsSource = DataBase.SelectAllDoc();
        void Check_person_Click(object sender, RoutedEventArgs e)
        {
            if (check_person.IsChecked == true)
                select_person.Visibility = search_person.Visibility = Visibility.Visible;
            else
                select_person.Visibility = search_person.Visibility = Visibility.Hidden;
        }
        void Search_person_Click(object sender, RoutedEventArgs e)
        {
            List<TableDoc> DocPerson = new List<TableDoc>();
            DocPerson = DataBase.SelectPersonDoc(select_person.Text.ToString());
            datagrid.ItemsSource = DocPerson;
        }
    }
}