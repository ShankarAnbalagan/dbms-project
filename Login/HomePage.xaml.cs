using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace MockSAP
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        private DatabaseConnection database;
        private DataTable purchaseTable;
        private DataTable vendorTable;
        private DataTable materialTable;
        private DataTable buyerTable;
        private DataTable salesTable;
        private DataTable productTable;
        private VendorList vendorList;
        private PurchasesList purchasesList;
        private MaterialList materialList;
        private BuyerList buyerList;
        private SalesList salesList;
        private ProductList productList;
        private NewPurchase newPurchase;
        private NewVendor newVendor;
        private ModifyVendor modifyVendor;
        private NewSale newSale;
        private NewBuyer newBuyer;
        private ModifyBuyer modifyBuyer;
        private Manufacture manufacture;
        public HomePage()
        {
            InitializeComponent();
        }

        public void StartPage(String uname, DatabaseConnection database)
        {
            this.database = database;
            this.Show();
            Loggedon_name.Text = "User ID : "+this.database.getUserId(uname)+"   Username : " + uname;
            purchaseTable = new DataTable();
            purchasesList = new PurchasesList(purchaseTable, database);
            vendorTable = new DataTable();
            vendorList = new VendorList(vendorTable, database);
            materialTable = new DataTable();
            materialList = new MaterialList(materialTable, database);
            buyerTable = new DataTable();
            buyerList = new BuyerList(buyerTable, database);
            salesTable = new DataTable();
            salesList = new SalesList(salesTable, database);
            productTable = new DataTable();
            productList = new ProductList(productTable, database);
            PopulateGridViews();
        }

        private void PopulateGridViews()
        {
            VendorListGrid.ItemsSource = vendorList.AddColums().DefaultView;
            VendorListGrid.ItemsSource = vendorList.AddRows().DefaultView;
            PurchasesGrid.ItemsSource = purchasesList.AddColums().DefaultView;
            PurchasesGrid.ItemsSource = purchasesList.AddRows().DefaultView;
            MaterialDataGrid.ItemsSource = materialList.AddColums().DefaultView;
            MaterialDataGrid.ItemsSource = materialList.AddRows().DefaultView;
            BuyerListGrid.ItemsSource = buyerList.AddColums().DefaultView;
            BuyerListGrid.ItemsSource = buyerList.AddRows().DefaultView;
            SalesGrid.ItemsSource = salesList.AddColums().DefaultView;
            SalesGrid.ItemsSource = salesList.AddRows().DefaultView;
            ProductsGrid.ItemsSource = productList.AddColums().DefaultView;
            ProductsGrid.ItemsSource = productList.AddRows().DefaultView;
        }

        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void new_purchase_Click(object sender, RoutedEventArgs e)
        {
            newPurchase = new NewPurchase();
            this.IsEnabled = false;
            newPurchase.StartPage(this.database,this);
        }

        public void RefreshPurchaseList()
        {
            purchaseTable.Clear();
            PurchasesGrid.ItemsSource = purchasesList.AddRows().DefaultView;
            materialTable.Clear();
            MaterialDataGrid.ItemsSource = materialList.AddRows().DefaultView;
        }

        public void RefreshSalesList()
        {
            salesTable.Clear();
            SalesGrid.ItemsSource = salesList.AddRows().DefaultView;
            materialTable.Clear();
            MaterialDataGrid.ItemsSource = materialList.AddRows().DefaultView;
            productTable.Clear();
            ProductsGrid.ItemsSource = productList.AddRows().DefaultView;
        }

        public void RefreshProductList()
        {
            productTable.Clear();
            ProductsGrid.ItemsSource = productList.AddRows().DefaultView;
            materialTable.Clear();
            MaterialDataGrid.ItemsSource = materialList.AddRows().DefaultView;
        }

        public void RefreshVendorList()
        {
            vendorTable.Clear();
            VendorListGrid.ItemsSource =vendorList.AddRows().DefaultView;
        }

        public void RefreshBuyerList()
        {
            buyerTable.Clear();
            BuyerListGrid.ItemsSource = buyerList.AddRows().DefaultView;
        }

        private void new_vendor_Click(object sender, RoutedEventArgs e)
        {
            newVendor = new NewVendor();
            this.IsEnabled = false;
            newVendor.StartPage(this.database,this);
        }

        private void modify_vendor_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = VendorListGrid.SelectedItem as DataRowView;
            if (item == null)
                MessageBox.Show("No Vendor Selected");
            else
            {
                modifyVendor = new ModifyVendor();
                this.IsEnabled = false;
                modifyVendor.StartPage(database,this, item.Row[0].ToString());
            }
        }        

        private void delete_vendor_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = VendorListGrid.SelectedItem as DataRowView;
            if (item == null)
                MessageBox.Show("No Vendor Selected");
            else
            {
                MessageBoxResult result= MessageBox.Show("Are you sure you want to remove vendor with ID "
                    +item.Row[0].ToString()+" ?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    database.DeleteVendor(item.Row[0].ToString());
                    RefreshVendorList();
                    MessageBox.Show("Vendor Successfully Deleted");
                }
                else
                {
                    return;
                }
            }
        }

        private void new_sale_Click(object sender, RoutedEventArgs e)
        {
            newSale = new NewSale();
            this.IsEnabled = false;
            newSale.StartPage(this.database, this);
        }

        private void new_buyer_Click(object sender, RoutedEventArgs e)
        {
            newBuyer = new NewBuyer();
            this.IsEnabled = false;
            newBuyer.StartPage(this.database, this);
        }

        private void delete_buyer_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = BuyerListGrid.SelectedItem as DataRowView;
            if (item == null)
                MessageBox.Show("No Buyer Selected");
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to remove Buyer with ID "
                    + item.Row[0].ToString() + " ?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    database.DeleteBuyer(item.Row[0].ToString());
                    RefreshBuyerList();
                    MessageBox.Show("Buyer Successfully Deleted");
                }
                else
                {
                    return;
                }
            }
        }

        private void modify_buyer_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = BuyerListGrid.SelectedItem as DataRowView;
            if (item == null)
                MessageBox.Show("No Buyer Selected");
            else
            {
                modifyBuyer = new ModifyBuyer();
                this.IsEnabled = false;
                modifyBuyer.StartPage(database, this, item.Row[0].ToString());
            }
        }

        private void manufacture_button_Click(object sender, RoutedEventArgs e)
        {
            manufacture = new Manufacture();
            this.IsEnabled = false;
            manufacture.StartPage(this.database, this);
        }
    }
}
