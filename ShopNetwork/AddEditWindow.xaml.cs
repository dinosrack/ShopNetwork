using ShopNetwork.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace ShopNetwork
{
    /// <summary>
    /// Логика взаимодействия для AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        public AddEditWindow()
        {
            InitializeComponent();
        }
        private string _tableName;
        private List<TextBox> _textBoxes = new List<TextBox>();
        private bool _isEditMode = false;
        private int _editId = 0;

        public AddEditWindow(string tableName)
        {
            InitializeComponent();
            _tableName = tableName;
            TableName.Text = tableName;
            CreateFieldsBasedOnTable();
        }

        public AddEditWindow(string tableName, int id)
        {
            InitializeComponent();
            _tableName = tableName;
            _isEditMode = true;
            _editId = id;
            TableName.Text = tableName;
            Header.Text = "Изменение записи";
            Title = "Изменение записи";
            AddOrEditBtn.Content = "Изменить";
            CreateFieldsBasedOnTable();
            LoadDataForEdit();
        }

        private void LoadDataForEdit()
        {
            using (ShopNetworkContext _db = new ShopNetworkContext())
            {
                switch (_tableName)
                {
                    case "Магазины":
                        var store = _db.Stores.FirstOrDefault(p => p.StoreId == _editId);
                        if (store != null)
                        {
                            _textBoxes[0].Text = store.Address;
                            _textBoxes[1].Text = store.Phone;
                            _textBoxes[2].Text = store.DirectorLastName;
                            _textBoxes[3].Text = store.EmployeeCount.ToString();
                        }
                        break;
                    case "Товары":
                        var product = _db.Products.FirstOrDefault(p => p.ProductId == _editId);
                        if (product != null)
                        {
                            _textBoxes[0].Text = product.ProductName;
                            _textBoxes[1].Text = product.Brand;
                            _textBoxes[2].Text = product.Weight.ToString();
                            _textBoxes[3].Text = product.Price.ToString();
                        }
                        break;
                    case "Наличие":
                        var inventory = _db.Inventories.FirstOrDefault(s => s.InventoryId == _editId);
                        if (inventory != null)
                        {
                            _textBoxes[0].Text = inventory.StoreId.ToString();
                            _textBoxes[1].Text = inventory.ProductId.ToString();
                            _textBoxes[2].Text = inventory.Quantity.ToString();
                        }
                        break;
                    case "Пользователи":
                        var user = _db.Users.FirstOrDefault(u => u.UserId == _editId);
                        if (user != null)
                        {
                            _textBoxes[0].Text = user.Username;
                            _textBoxes[1].Text = user.UserPassword;
                            _textBoxes[2].Text = user.UserRole;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void CreateFieldsBasedOnTable()
        {
            switch (_tableName)
            {
                case "Магазины":
                    CreateFields(new List<string> { "Адрес", "Телефон", "Директор", "Кол-во сотрудников" });
                    break;
                case "Товары":
                    CreateFields(new List<string> { "Название", "Бренд", "Вес", "Цена" });
                    break;
                case "Наличие":
                    CreateFields(new List<string> { "ID магазина", "ID товара", "Количество" });
                    break;
                case "Пользователи":
                    CreateFields(new List<string> { "Логин", "Пароль", "Роль" });
                    break;
                default:
                    break;
            }
        }

        private void CreateFields(List<string> fieldNames)
        {
            foreach (var fieldName in fieldNames)
            {
                var textBlock = new TextBlock
                {
                    Text = fieldName,
                    FontSize = 16
                };
                MainArea.Children.Add(textBlock);

                var textBox = new TextBox
                {
                    Height = 35,
                    FontSize = 16,
                    VerticalContentAlignment = VerticalAlignment.Center
                };
                MainArea.Children.Add(textBox);

                var borderAfterTextBox = new Border
                {
                    Margin = new Thickness(20)
                };
                MainArea.Children.Add(borderAfterTextBox);

                _textBoxes.Add(textBox);
            }
        }

        private void AddOrEditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_textBoxes.Any(tb => string.IsNullOrWhiteSpace(tb.Text)))
            {
                MessageBox.Show("Все поля необходимо заполнить для добавления новой строки!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (ShopNetworkContext _db = new ShopNetworkContext())
            {
                try
                {
                    if (_isEditMode)
                    {
                        switch (_tableName)
                        {
                            case "Магазины":
                                EditStore(_db);
                                break;
                            case "Товары":
                                EditProduct(_db);
                                break;
                            case "Наличие":
                                EditInventory(_db);
                                break;
                            case "Пользователи":
                                EditUser(_db);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (_tableName)
                        {
                            case "Магазины":
                                AddStore(_db);
                                break;
                            case "Товары":
                                AddProduct(_db);
                                break;
                            case "Наличие":
                                AddInventory(_db);
                                break;
                            case "Пользователи":
                                AddUser(_db);
                                break;
                            default:
                                break;
                        }
                    }

                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.ShowDialog();
        }

        private void AddStore(ShopNetworkContext _db)
        {
            var store = new Store
            {
                Address = _textBoxes[0].Text,
                Phone = _textBoxes[1].Text,
                DirectorLastName = _textBoxes[2].Text,
                EmployeeCount = int.Parse(_textBoxes[3].Text)
            };

            _db.Stores.Add(store);
        }

        private void AddProduct(ShopNetworkContext _db)
        {
            var product = new Product
            {
                ProductName = _textBoxes[0].Text,
                Brand = _textBoxes[1].Text,
                Weight = decimal.Parse(_textBoxes[2].Text),
                Price = decimal.Parse(_textBoxes[3].Text)
            };

            _db.Products.Add(product);
        }

        private void AddInventory(ShopNetworkContext _db)
        {
            var inventory = new Inventory
            {
                StoreId = int.Parse(_textBoxes[0].Text),
                ProductId = int.Parse(_textBoxes[1].Text),
                Quantity = int.Parse(_textBoxes[2].Text)
            };

            _db.Inventories.Add(inventory);
        }

        private void AddUser(ShopNetworkContext _db)
        {
            var user = new User
            {
                Username = _textBoxes[0].Text,
                UserPassword = _textBoxes[1].Text,
                UserRole = _textBoxes[2].Text
            };

            _db.Users.Add(user);
        }

        private void EditStore(ShopNetworkContext _db)
        {
            var store = _db.Stores.FirstOrDefault(p => p.StoreId == _editId);
            if (store != null)
            {
                store.Address = _textBoxes[0].Text;
                store.Phone = _textBoxes[1].Text;
                store.DirectorLastName = _textBoxes[2].Text;
                store.EmployeeCount = int.Parse(_textBoxes[3].Text);
            }
        }

        private void EditProduct(ShopNetworkContext _db)
        {
            var product = _db.Products.FirstOrDefault(p => p.ProductId == _editId);
            if (product != null)
            {
                product.ProductName = _textBoxes[0].Text;
                product.Brand = _textBoxes[1].Text;
                product.Weight = decimal.Parse(_textBoxes[2].Text);
                product.Price = decimal.Parse(_textBoxes[3].Text);
            }
        }

        private void EditInventory(ShopNetworkContext _db)
        {
            var inventory = _db.Inventories.FirstOrDefault(s => s.InventoryId == _editId);
            if (inventory != null)
            {
                inventory.StoreId = int.Parse(_textBoxes[0].Text);
                inventory.ProductId = int.Parse(_textBoxes[1].Text);
                inventory.Quantity = int.Parse(_textBoxes[2].Text);
            }
        }

        private void EditUser(ShopNetworkContext _db)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserId == _editId);
            if (user != null)
            {
                user.Username = _textBoxes[0].Text;
                user.UserPassword = _textBoxes[1].Text;
                user.UserRole = _textBoxes[2].Text;
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.ShowDialog();
        }
    }
}