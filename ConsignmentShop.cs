using ConsignmentShopLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsignmentShopUI
{
    public partial class ConsignmentShop : Form
    {
        private Store store = new Store();
        private List<Item> shoppingCartData= new List<Item>();
        BindingSource itemsBinding = new BindingSource();
        BindingSource cartBinding = new BindingSource();
        BindingSource vendorsBinding = new BindingSource();
        private decimal storeProfit = 0;
        public ConsignmentShop()
        {
            InitializeComponent();
            SetupDate();

            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();
            itemsListBox.DataSource = itemsBinding;
            itemsListBox.DisplayMember = "Display";
            itemsListBox.ValueMember = "Display";

            cartBinding.DataSource = shoppingCartData;
            shoppingCartListBox.DataSource = cartBinding;
            shoppingCartListBox.DisplayMember = "Display";
            shoppingCartListBox.ValueMember = "Display";

            vendorsBinding.DataSource = store.Vendors;
            vendorListbox.DataSource = vendorsBinding;

            vendorListbox.DisplayMember = "Display";
            vendorListbox.ValueMember = "Display";
        }
        private void SetupDate()
        {
            store.Vendors.Add(new Vendor { FirstName = "Bill", LastName = "Smith" });
            store.Vendors.Add(new Vendor { FirstName = "Helen", LastName = "Green" });
            store.Vendors.Add(new Vendor { FirstName = "Mike", LastName = "Cooper" });
            store.Items.Add(new Item
            {
                Title = "Where The Crawdads Sing",
                Description = "Novel Fiction",
                Price = 4.0M,
                Owner = store.Vendors[0]
            });
            store.Items.Add(new Item
            {
                Title = "Harry Potter and the Saurcerer's Stone",
                Description = "Book Fantasy",
                Price = 6.5M,
                Owner = store.Vendors[0]
            });
            store.Items.Add(new Item
            {
                Title = "A Tale of two cities",
                Description = "Classic",
                Price = 5.0M,
                Owner = store.Vendors[1]
            });

            store.Items.Add(new Item
            {
                Title = "Pride and Prejudice",
                Description = "Classic",
                Price = 5.5M,
                Owner = store.Vendors[2]
            });

            store.Name = "Book Shop";
        }

        private void addToCart_Click(object sender, EventArgs e)
        {
            // Figure out what is selected from the itemList and copy that to the shopping cart
            Item selectedItem = (Item)itemsListBox.SelectedItem;
            shoppingCartData.Add(selectedItem);
            cartBinding.ResetBindings(false);
        }

        private void makePurchase_Click(object sender, EventArgs e)
        {
            // Make each item in the cart as sold and clear the cart
            foreach (Item item in shoppingCartData)
            {
                item.Sold = true;
                item.Owner.PaymentDue += (decimal)item.Owner.Commission * item.Price;
                storeProfit = (1 - (decimal)item.Owner.Commission) * item.Price;
            }
            shoppingCartData.Clear();
            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();

            storeProfitValue.Text = string.Format("€{0}", storeProfit);
            cartBinding.ResetBindings(false);
            itemsBinding.ResetBindings(false);
            vendorsBinding.ResetBindings(false);
        }
    }
}
