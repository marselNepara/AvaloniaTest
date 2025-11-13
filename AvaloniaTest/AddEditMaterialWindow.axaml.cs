using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaTest.Data;
using System;
using System.Linq;

namespace AvaloniaTest
{
    public partial class AddEditMaterialWindow : Window
    {
        private Material currentMaterial = new Material();

        public AddEditMaterialWindow()
        {
            InitializeComponent();
            LoadTypes();
        }

        public AddEditMaterialWindow(Material material)
        {
            InitializeComponent();
            currentMaterial = material;
            DataContext = currentMaterial;
            LoadTypes();

            nameMaterial.Text = material.Name;
            priceMaterial.Text = material.UnitPrice.ToString();
            stockMaterial.Text = material.QuantityInStock.ToString();
            minQuantityMaterial.Text = material.MinimumQuantity.ToString();
            packageMaterial.Text = material.QuantityInPackage.ToString();
            unitMaterial.Text = material.UnitOfMeasure;

            if (material.MaterialTypeId.HasValue)
            {
                var type = App.DbContext.MaterialTypes.FirstOrDefault(t => t.Id == material.MaterialTypeId.Value);
                typeMaterial.SelectedItem = type;
            }
        }

        private void LoadTypes()
        {
            typeMaterial.ItemsSource = App.DbContext.MaterialTypes.ToList();
        }

        private void Close_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Click(object? sender, RoutedEventArgs e)
        {
            ClearErrors();
            bool isValid = true;
            MaterialType? selectedType = null;

            if (string.IsNullOrWhiteSpace(nameMaterial.Text))
            {
                errorName.Text = "Наименование не может быть пустым.";
                isValid = false;
            }

            if (typeMaterial.SelectedItem is MaterialType type)
                selectedType = type;
            else
            {
                errorType.Text = "Выберите тип материала.";
                isValid = false;
            }

            if (!decimal.TryParse(priceMaterial.Text, out decimal price) || price <= 0)
            {
                errorPrice.Text = "Цена должна быть положительным числом.";
                isValid = false;
            }

            if (!decimal.TryParse(stockMaterial.Text, out decimal stock) || stock < 0)
            {
                errorStock.Text = "Количество на складе должно быть числом ≥ 0.";
                isValid = false;
            }

            if (!decimal.TryParse(minQuantityMaterial.Text, out decimal minQty) || minQty < 0)
            {
                errorMinQuantity.Text = "Минимальное количество должно быть числом ≥ 0.";
                isValid = false;
            }

            if (!int.TryParse(packageMaterial.Text, out int package) || package <= 0)
            {
                errorPackage.Text = "Количество в упаковке должно быть целым положительным числом.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(unitMaterial.Text))
            {
                errorUnit.Text = "Укажите единицу измерения.";
                isValid = false;
            }

            if (!isValid)
                return;

            currentMaterial.Name = nameMaterial.Text.Trim();
            currentMaterial.UnitPrice = price;
            currentMaterial.QuantityInStock = stock;
            currentMaterial.MinimumQuantity = minQty;
            currentMaterial.QuantityInPackage = package;
            currentMaterial.UnitOfMeasure = unitMaterial.Text.Trim();
            currentMaterial.MaterialTypeId = selectedType!.Id;

            if (currentMaterial.Id == 0)
                App.DbContext.Materials.Add(currentMaterial);

            App.DbContext.SaveChanges();
            Close();
        }

        private void ClearErrors()
        {
            errorName.Text = "";
            errorType.Text = "";
            errorPrice.Text = "";
            errorStock.Text = "";
            errorMinQuantity.Text = "";
            errorPackage.Text = "";
            errorUnit.Text = "";
        }
    }
}
