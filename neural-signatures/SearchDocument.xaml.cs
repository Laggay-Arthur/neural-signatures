using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System;

namespace neural_signatures
{
    public partial class SearchDocument : Window
    {
        DateTime? date_validity;
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

        private void DatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            List<TableDoc> doc_date = new List<TableDoc>();
            //List<TableDoc> doc_date2 = new List<TableDoc>();
            date_validity = (DateTime)datePicker.SelectedDate;
            doc_date = DataBase.SelectAllDocDate(date_validity);
            datagrid.ItemsSource = doc_date;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<TableDoc> doc_string = new List<TableDoc>();
            doc_string = DataBase.SelectAllDocString(textbox.Text);
            datagrid.ItemsSource = doc_string;
        }
    }
}