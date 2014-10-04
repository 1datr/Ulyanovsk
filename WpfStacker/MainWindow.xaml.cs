using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BR.AN.PviServices;

namespace WpfStacker
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

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            /*
            CollectionViewSource adViewSource = (CollectionViewSource)FindResource("VCellContentCount");
            //System.Data.Objects.ObjectQuery<WpfStacker.CellContent> cellContentsQuery = this.GetCellContentsQuery(cellcontentEntities1);
            var counts = from CellContents in cellcontentEntities1.CellContents.GroupBy(c => c.Cell).Select(G => new 
                              { 
                                 cellno = G.Key.cellno,                                  
                                 cCount = G.Count()                                
                              } 
                           )                                                                         
                         join tCells in cellcontentEntities1.Cells on CellContents.Cell equals tCells.id
                         select new { cell = tCells.cellno, count = CellContents.cCount };*/
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
       
            
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StatusBar_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {

        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            stacker1.AddProduct(full_prod_filter.Text);
            stacker2.refresh();
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            
        }

        private void prods_grid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //prods_grid.SelectedItem 
            var editing = e.EditingElement as TextBox;
            stacker1.EditProduct(prods_grid.SelectedItem, editing.Text);
            stacker2.refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            stacker1.DeleteProduct(prods_grid.SelectedItem);
            stacker2.refresh();
        }
                
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            switch (Stackers)
            { 
                case 0:
                    dgSearched.ItemsSource = stacker1.FindByName(filter_productname.Text);
                    break;
                case 1:
                    {
                        List<int> l = new List<int>();
                        l.Add(1);
                        dgSearched.ItemsSource = stacker1.FindByName(filter_productname.Text, l);
                    } break;
                case 2:
                    {
                        List<int> l = new List<int>();
                        l.Add(2);
                        dgSearched.ItemsSource = stacker1.FindByName(filter_productname.Text, l);
                    } break;
            }
            
        }

        private int fStackers;

        public static readonly DependencyProperty StackersDP =
        DependencyProperty.Register(
            "Stackers",
            typeof(int),
            typeof(MainWindow),
            new FrameworkPropertyMetadata(
                0));
        public int Stackers { get { return (int)GetValue(StackersDP); } set { SetValue(StackersDP,value); } }

        public static readonly DependencyProperty EditProductsDP =
        DependencyProperty.Register(
            "EditProducts",
            typeof(bool),
            typeof(MainWindow),
            new FrameworkPropertyMetadata(
                false));
        public bool EditProducts { get { return (bool)GetValue(EditProductsDP); } set { SetValue(EditProductsDP, value); } }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // take a product from cell
            //   dgSearched.SelectedItem
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            // move to first load cell
        }

        


        

        

        

       

       
    }
}
