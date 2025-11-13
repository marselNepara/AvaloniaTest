using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaTest.Data;
using System;
using System.Linq;

namespace AvaloniaTest;

public partial class CalculationPage : UserControl
{
    public CalculationPage()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        var productTypes = App.DbContext.ProductTypes.ToList();
        var materialTypes = App.DbContext.MaterialTypes.ToList();

        cmbProductType.ItemsSource = productTypes;
        cmbMaterialType.ItemsSource = materialTypes;

        if (productTypes.Any()) cmbProductType.SelectedIndex = 0;
        if (materialTypes.Any()) cmbMaterialType.SelectedIndex = 0;
    }

    private void Calculate_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        txtResult.Text = "";

        if (cmbProductType.SelectedItem == null || cmbMaterialType.SelectedItem == null)
        {
            txtResult.Text = "Please select product and material types";
            return;
        }

        if (string.IsNullOrWhiteSpace(txtMaterialQty.Text))
        {
            txtResult.Text = "Material quantity is required";
            return;
        }

        if (!int.TryParse(txtMaterialQty.Text, out int materialQty))
        {
            txtResult.Text = "Material quantity must be a whole number (e.g., 100)";
            return;
        }

        if (materialQty <= 0)
        {
            txtResult.Text = "Material quantity must be positive";
            return;
        }

        if (string.IsNullOrWhiteSpace(txtParam1.Text))
        {
            txtResult.Text = "Parameter 1 is required";
            return;
        }

        if (!double.TryParse(txtParam1.Text, out double param1))
        {
            txtResult.Text = "Parameter 1 must be a number (use comma for decimals, e.g., 1,5)";
            return;
        }

        if (param1 <= 0)
        {
            txtResult.Text = "Parameter 1 must be positive";
            return;
        }

        if (string.IsNullOrWhiteSpace(txtParam2.Text))
        {
            txtResult.Text = "Parameter 2 is required";
            return;
        }

        if (!double.TryParse(txtParam2.Text, out double param2))
        {
            txtResult.Text = "Parameter 2 must be a number (use comma for decimals, e.g., 1,5)";
            return;
        }

        if (param2 <= 0)
        {
            txtResult.Text = "Parameter 2 must be positive";
            return;
        }

        try
        {
            var productType = cmbProductType.SelectedItem as ProductType;
            var materialType = cmbMaterialType.SelectedItem as MaterialType;

            int result = CalculationService.CalculateProductQuantity(
                productType.Id, materialType.Id, materialQty, param1, param2);

            if (result == -1)
            {
                txtResult.Text = "Calculation error: invalid input data";
            }
            else
            {
                txtResult.Text = $"Result: {result} units of product";
            }
        }
        catch (Exception ex)
        {
            txtResult.Text = $"Error: {ex.Message}";
        }
    }
}
