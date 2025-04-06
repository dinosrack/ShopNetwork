using ShopNetwork.Models;
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

namespace ShopNetwork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _userrole;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string userrole)
        {
            InitializeComponent();
            _userrole = userrole;
            CheckUserAccess();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDBInDataGrid();
        }

        void LoadDBInDataGrid()
        {
            using (ShopNetworkContext _db = new ShopNetworkContext())
            {
                Table.ItemsSource = _db.Stores.ToList();

                Table2.ItemsSource = _db.Products.ToList();

                Table3.ItemsSource = _db.Inventories.ToList();

                Table4.ItemsSource = _db.Users.ToList();
            }
        }

        private void CheckUserAccess()
        {
            if (_userrole != "admin")
            {
                // Скрываем вкладку "Пользователи"
                var usersTabItem = Tab.Items
                    .OfType<TabItem>()
                    .FirstOrDefault(t => t.Header.ToString() == "Пользователи");

                if (usersTabItem != null)
                {
                    usersTabItem.Visibility = Visibility.Collapsed;
                }

                var addButton = FindName("Add") as Button;
                var editButton = FindName("Edit") as Button;
                var removeButton = FindName("Remove") as Button;
                var borderMargin1 = FindName("BorderMargin1") as Border;
                var borderMargin2 = FindName("BorderMargin2") as Border;
                var borderMargin3 = FindName("BorderMargin3") as Border;

                if (addButton != null)
                {
                    addButton.Visibility = Visibility.Collapsed;
                }

                if (editButton != null)
                {
                    editButton.Visibility = Visibility.Collapsed;
                }

                if (removeButton != null)
                {
                    removeButton.Visibility = Visibility.Collapsed;
                }

                if (borderMargin1 != null)
                {
                    borderMargin1.Visibility = Visibility.Collapsed;
                }

                if (borderMargin2 != null)
                {
                    borderMargin2.Visibility = Visibility.Collapsed;
                }

                if (borderMargin3 != null)
                {
                    borderMargin3.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            DataGrid activeDataGrid = null;
            string tableName = "";

            switch (Tab.SelectedIndex)
            {
                case 0:
                    activeDataGrid = Table;
                    tableName = "Магазины";
                    break;
                case 1:
                    activeDataGrid = Table2;
                    tableName = "Товары";
                    break;
                case 2:
                    activeDataGrid = Table3;
                    tableName = "Наличие";
                    break;
                case 3:
                    activeDataGrid = Table4;
                    tableName = "Пользователи";
                    break;
                default:
                    break;
            }

            if (activeDataGrid != null)
            {
                var selectedItem = activeDataGrid.SelectedItem;
                if (selectedItem != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Вы уверены, что хотите удалить выбранную запись?",
                        "Подтверждение удаления",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        using (ShopNetworkContext _db = new ShopNetworkContext())
                        {
                            try
                            {
                                _db.Entry(selectedItem).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

                                switch (tableName)
                                {
                                    case "Stores":
                                        var storeId = ((Store)selectedItem).StoreId;
                                        var relatedInventory = _db.Inventories.Where(i => i.StoreId == storeId).ToList();
                                        _db.Inventories.RemoveRange(relatedInventory);
                                        _db.Stores.Remove((Store)selectedItem);
                                        break;

                                    case "Products":
                                        var productId = ((Product)selectedItem).ProductId;
                                        var relatedInventoryForProduct = _db.Inventories.Where(i => i.ProductId == productId).ToList();
                                        _db.Inventories.RemoveRange(relatedInventoryForProduct);
                                        _db.Products.Remove((Product)selectedItem);
                                        break;

                                    case "Inventory":
                                        var inventoryId = ((Inventory)selectedItem).InventoryId;
                                        var inventoryToDelete = _db.Inventories.FirstOrDefault(i => i.InventoryId == inventoryId);
                                        if (inventoryToDelete != null)
                                        {
                                            _db.Inventories.Remove(inventoryToDelete);
                                        }
                                        break;

                                    case "Users":
                                        var userId = ((User)selectedItem).UserId;
                                        var relatedRoles = _db.Users.Where(u => u.UserId == userId).ToList();

                                        _db.Users.Remove((User)selectedItem);
                                        break;

                                    default:
                                        break;
                                }
                                _db.SaveChanges();
                                LoadDBInDataGrid();

                                MessageBox.Show("Запись успешно удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка при удалении: {ex.Message}\nВнутреннее исключение: {ex.InnerException?.Message}");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Необходимо выбрать строку для выполнения дальнейшего действия!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var selectedTab = Tab.SelectedItem as TabItem;

            if (selectedTab != null)
            {
                string tableName = selectedTab.Header.ToString();

                AddEditWindow addEditWindow = new AddEditWindow(tableName);
                this.Close();
                addEditWindow.ShowDialog();

                LoadDBInDataGrid();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            DataGrid activeDataGrid = null;
            string tableName = "";

            switch (Tab.SelectedIndex)
            {
                case 0:
                    activeDataGrid = Table;
                    tableName = "Магазины";
                    break;
                case 1:
                    activeDataGrid = Table2;
                    tableName = "Товары";
                    break;
                case 2:
                    activeDataGrid = Table3;
                    tableName = "Наличие";
                    break;
                case 3:
                    activeDataGrid = Table4;
                    tableName = "Пользователи";
                    break;
                default:
                    break;
            }

            if (activeDataGrid != null)
            {
                var selectedItem = activeDataGrid.SelectedItem;
                if (selectedItem != null)
                {
                    int id = 0;

                    switch (tableName)
                    {
                        case "Магазины":
                            id = ((Store)selectedItem).StoreId;
                            break;
                        case "Товары":
                            id = ((Product)selectedItem).ProductId;
                            break;
                        case "Наличие":
                            id = ((Inventory)selectedItem).InventoryId;
                            break;
                        case "Пользователи":
                            id = ((User)selectedItem).UserId;
                            break;
                        default:
                            break;
                    }

                    var editWindow = new AddEditWindow(tableName, id);
                    this.Close();
                    editWindow.ShowDialog();

                    LoadDBInDataGrid();
                }
                else
                {
                    MessageBox.Show("Необходимо выбрать строку для выполнения дальнейшего действия!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Find_Click(object sender, RoutedEventArgs e)
        {
            DataGrid activeDataGrid = null;
            string idPropertyName = null;

            switch (Tab.SelectedIndex)
            {
                case 0:
                    activeDataGrid = Table;
                    idPropertyName = "StoreId";
                    break;
                case 1:
                    activeDataGrid = Table2;
                    idPropertyName = "ProductId";
                    break;
                case 2:
                    activeDataGrid = Table3;
                    idPropertyName = "InventoryId";
                    break;
                case 3:
                    activeDataGrid = Table4;
                    idPropertyName = "UserId";
                    break;
                default:
                    return;
            }

            if (activeDataGrid != null && idPropertyName != null)
            {
                var listItem = activeDataGrid.ItemsSource as IEnumerable<object>;

                if (listItem != null)
                {
                    var filtered = listItem.Cast<object>().Where(item =>
                    {
                        var prop = item.GetType().GetProperty(idPropertyName);
                        if (prop != null)
                        {
                            var value = prop.GetValue(item)?.ToString();
                            return value == ID.Text;
                        }
                        return false;
                    }).ToList();

                    activeDataGrid.SelectedItems.Clear();

                    if (filtered.Any())
                    {
                        foreach (var item in filtered)
                        {
                            activeDataGrid.SelectedItems.Add(item);
                            activeDataGrid.ScrollIntoView(item);
                        }

                        activeDataGrid.Focus();
                    }
                    else
                    {
                        MessageBox.Show("ID, который вы искали, не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            this.Close();
            authWindow.ShowDialog();
        }
    }
}