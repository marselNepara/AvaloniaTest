using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaTest.Data;
using System;
using System.Linq;

namespace AvaloniaTest
{
    public partial class AddEditSupplierWindow : Window
    {
        private Supplier currentSupplier = new Supplier();

        public AddEditSupplierWindow()
        {
            InitializeComponent();
            LoadTypes();
        }

        public AddEditSupplierWindow(Supplier supplier)
        {
            InitializeComponent();
            currentSupplier = supplier;
            DataContext = currentSupplier;
            LoadTypes();

            // Установка значений в поля
            nameSupplier.Text = supplier.Name;
            innSupplier.Text = supplier.Inn;
            ratingSupplier.Text = supplier.Rating?.ToString() ?? "";

            // Если в модели DateOnly, преобразуем в DateTime, затем в DateTimeOffset для SelectedDate
            if (supplier.StartDate.HasValue)
            {
                var dt = supplier.StartDate.Value.ToDateTime(TimeOnly.MinValue); // DateTime
                startDateSupplier.SelectedDate = new DateTimeOffset(dt);        // DateTimeOffset
            }

            if (supplier.SupplierTypeId.HasValue)
            {
                var type = App.DbContext.SupplierTypes.FirstOrDefault(t => t.Id == supplier.SupplierTypeId.Value);
                typeSupplier.SelectedItem = type;
            }
        }

        private void LoadTypes()
        {
            typeSupplier.ItemsSource = App.DbContext.SupplierTypes.ToList();
        }

        private void Close_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Click(object? sender, RoutedEventArgs e)
        {
            currentSupplier.Name = nameSupplier.Text!;
            currentSupplier.Inn = innSupplier.Text!;
            currentSupplier.Rating = int.TryParse(ratingSupplier.Text, out int r) ? r : null;

            // Безопасное чтение SelectedDate: берём DateTime из DateTimeOffset
            if (startDateSupplier.SelectedDate.HasValue)
            {
                var dto = startDateSupplier.SelectedDate.Value; // DateTimeOffset
                currentSupplier.StartDate = DateOnly.FromDateTime(dto.DateTime);
            }
            else
            {
                currentSupplier.StartDate = null;
            }

            if (typeSupplier.SelectedItem is SupplierType selectedType)
            {
                currentSupplier.SupplierTypeId = selectedType.Id;
            }

            if (currentSupplier.Id == 0)
                App.DbContext.Suppliers.Add(currentSupplier);

            App.DbContext.SaveChanges();
            Close();
        }
    }
}
