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
using NetworkHelper;
using System.Data;


namespace WPF_miniwebshopDG
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<products> termekek = new List<products>();
        private int selectedId;

        public MainWindow()
        {
            InitializeComponent();
            DataGridFeltolt();
            kategoriaFeltolt();
        }
        private void DataGridFeltolt()
        {
            string url = "http://localhost:3000/products";
            termekek = Backend.GET(url).Send().As<List<products>>();
            ProductsGrid.ItemsSource = termekek;
            

        }
        private void kategoriaFeltolt()
        {
            string url = "http://localhost:3000/category";
            List<products> kategoria = Backend.GET(url).Send().As<List<products>>();
            combobox1.Items.Clear();
            combobox1.Items.Add("Összes");
            //combobox1.ItemsSource = kategoria;
            foreach (var item in kategoria)
            {
                combobox1.Items.Add(item.category);
            }
            combobox1.SelectedIndex = 0;
            combobox2.ItemsSource = kategoria;
            combobox2.DisplayMemberPath = "category";
            combobox2.SelectedValuePath = "category";
        }
        private void btn_newAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbx_nev.Text) || string.IsNullOrWhiteSpace(tbx_ar.Text) )
            {
                MessageBox.Show("Hiba!", "Minden mezőt ki kell tölteni!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newProduct = new
            {
                name = tbx_nev.Text,
                price = tbx_ar.Text,
                category = combobox2.SelectedItem,
                inStock = raktaron.IsChecked,
                

            };
            string url = "http://localhost:3000/newProduct";
            try
            {
                Backend.POST(url).Body(newProduct).Send();
                MessageBox.Show("Sikeres!", "Sikeres regisztráció!", MessageBoxButton.OK, MessageBoxImage.Information);
                tbx_nev.Text = "";
                tbx_ar.Text = "";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba!", $"Sikertelen regisztráció! {ex}", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }

        private void btn_modifySave_Click(object sender, RoutedEventArgs e)
        {
            products selectedUser = ProductsGrid.SelectedItem as products;
            if (selectedUser == null)
            {
                MessageBox.Show("Válassz ki egy terméket.");
                return;
            }
            //egy val
            if (string.IsNullOrWhiteSpace(tbx_nev.Text) || string.IsNullOrWhiteSpace(tbx_ar.Text))
            {
                MessageBox.Show("Hiba!", "Minden mezőt ki kell tölteni!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //friss. adatok össz.
            var newUser = new
            {
                name = tbx_nev.Text,
                price = tbx_ar.Text,
                category = combobox2.SelectedItem,
                inStock = raktaron.IsChecked,
            };
            string url = $"http://localhost:3000/modifyProduct/{selectedUser.id}";
            try
            {
                Backend.PUT(url).Body(newUser).Send();
                MessageBox.Show("Sikeres a módosítás!");
                DataGridFeltolt();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a módosítás során! {ex}", $"Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            ProductsGrid.ItemsSource = "";
            tbx_nev.Clear();
            tbx_ar.Clear();
            combobox1.Items.Clear();
            //combobox2.Items.Clear();
            

            DataGridFeltolt();
            kategoriaFeltolt();
        }

        private void csakRaktaron_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void raktaron_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void combobox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Category = termekek.Select(x => x.category).ToString();
           combobox1.Text = Category;
        }

        private void ProductsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            products selectedUser = ProductsGrid.SelectedItem as products;
            if (selectedUser != null)
            {
                tbx_nev.Text = selectedUser.name;
                tbx_ar.Text = selectedUser.price;
                combobox2.Text = selectedUser.category;
                raktaron.IsChecked = selectedUser.inStock == 1;
                
            }

            
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            products selectedProduct = ProductsGrid.SelectedItem as products;

            if (selectedProduct == null)
            {
                MessageBox.Show("Válassz ki egy terméket a táblázatból!");
                return;
            }

            MessageBoxResult confirm = MessageBox.Show(
                $"Biztosan törlöd ezt a terméket?\n\nNév: {selectedProduct.name}\nÁr: {selectedProduct.price}",
                "Törlés megerősítése",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (confirm != MessageBoxResult.Yes)
                return;


            string url = $"http://localhost:3000/deleteProduct/{selectedProduct.id}";

            try
            {
                Backend.DELETE(url).Send();

                MessageBox.Show("Felhasználó sikeresen törölve!");
                DataGridFeltolt();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt a törlés során:\n" + ex.Message);
            }
        }
    }
}
