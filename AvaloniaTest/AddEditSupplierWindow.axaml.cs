using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaTest.Data;
using System;
using System.Linq;
using System.Text.RegularExpressions;

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

            nameSupplier.Text = supplier.Name;
            innSupplier.Text = supplier.Inn;
            ratingSupplier.Text = supplier.Rating?.ToString() ?? "";

            if (supplier.StartDate.HasValue)
            {
                var dt = supplier.StartDate.Value.ToDateTime(TimeOnly.MinValue);
                startDateSupplier.SelectedDate = new DateTimeOffset(dt);
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
            ClearErrors();
            bool isValid = true;
            SupplierType? selectedType = null;

            if (string.IsNullOrWhiteSpace(nameSupplier.Text))
            {
                errorName.Text = "Название не может быть пустым.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(innSupplier.Text) ||
                !Regex.IsMatch(innSupplier.Text, @"^\d{10}(\d{2})?$"))
            {
                errorInn.Text = "ИНН должен содержать 10 или 12 цифр.";
                isValid = false;
            }

            if (!int.TryParse(ratingSupplier.Text, out int rating) || rating < 1 || rating > 10)
            {
                errorRating.Text = "Рейтинг должен быть числом от 1 до 10.";
                isValid = false;
            }

            if (!startDateSupplier.SelectedDate.HasValue)
            {
                errorDate.Text = "Укажите дату начала сотрудничества.";
                isValid = false;
            }

            if (typeSupplier.SelectedItem is SupplierType type)
            {
                selectedType = type;
            }
            else
            {
                errorType.Text = "Выберите тип поставщика.";
                isValid = false;
            }

            if (!isValid)
                return;

            currentSupplier.Name = nameSupplier.Text!.Trim();
            currentSupplier.Inn = innSupplier.Text!.Trim();
            currentSupplier.Rating = rating;
            currentSupplier.StartDate = DateOnly.FromDateTime(startDateSupplier.SelectedDate!.Value.DateTime);
            currentSupplier.SupplierTypeId = selectedType!.Id;

            if (currentSupplier.Id == 0)
                App.DbContext.Suppliers.Add(currentSupplier);

            App.DbContext.SaveChanges();
            Close();
        }

        private void ClearErrors()
        {
            errorName.Text = "";
            errorInn.Text = "";
            errorRating.Text = "";
            errorDate.Text = "";
            errorType.Text = "";
        }
    }
}
