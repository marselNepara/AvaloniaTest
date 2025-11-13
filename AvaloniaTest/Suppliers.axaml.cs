using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using AvaloniaTest.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AvaloniaTest
{
    public partial class Suppliers : UserControl
    {
        public Suppliers()
        {
            InitializeComponent();
            LoadData();
        }

        // Загрузка данных в таблицу
        private void LoadData()
        {
            dgSuppliers.ItemsSource = App.DbContext.Suppliers
                .Include(s => s.SupplierType)
                .ToList()
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    SupplierTypeName = s.SupplierType != null ? s.SupplierType.Name : "—",
                    s.Inn,
                    s.Rating,
                    StartDate = s.StartDate.HasValue ? s.StartDate.Value.ToString("dd.MM.yyyy") : ""
                })
                .ToList();
        }

        // Добавление нового поставщика
        private async void Add_Click(object? sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditSupplierWindow();
            var parent = this.VisualRoot as Window;

            await addWindow.ShowDialog(parent);
            LoadData(); // обновляем таблицу после закрытия окна
        }

        // Редактирование поставщика
        private async void Edit_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is not null)
            {
                // получаем Id из объекта строки
                var idProp = button.DataContext.GetType().GetProperty("Id");
                if (idProp == null) return;

                int id = (int)idProp.GetValue(button.DataContext)!;

                var supplier = App.DbContext.Suppliers
                    .Include(s => s.SupplierType)
                    .FirstOrDefault(s => s.Id == id);

                if (supplier != null)
                {
                    var editWindow = new AddEditSupplierWindow(supplier);
                    var parent = this.VisualRoot as Window;

                    await editWindow.ShowDialog(parent);
                    LoadData(); // обновляем данные
                }
            }
        }

        // Удаление поставщика
        private void Delete_Click(object? sender, RoutedEventArgs e)
        {
            var anonymousObject = (sender as Button)?.Tag;
            if (anonymousObject == null)
                return;

            var idProperty = anonymousObject.GetType().GetProperty("Id");
            if (idProperty == null)
                return;

            var id = (int)idProperty.GetValue(anonymousObject)!;

            // Находим поставщика и удаляем
            var supplier = App.DbContext.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier != null)
            {
                App.DbContext.Suppliers.Remove(supplier);
                App.DbContext.SaveChanges();
                LoadData();
            }
        }
    }
}
