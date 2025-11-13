using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaTest.Data;
using System;
using System.Linq;

namespace AvaloniaTest;

public partial class AddEditMaterialWindow : Window
{
    Material currentMaterial = new Material();

    public AddEditMaterialWindow()
    {
        InitializeComponent();
        LoadTypes();
    }

    public AddEditMaterialWindow(Material material)
    {
        InitializeComponent();
        this.currentMaterial = material;
        DataContext = this.currentMaterial;
        LoadTypes();

        if (material.MaterialTypeId.HasValue)
        {
            var type = App.DbContext.MaterialTypes.FirstOrDefault(t => t.Id == material.MaterialTypeId.Value);
            typeMaterial.SelectedItem = type;
        }
    }

    private void LoadTypes()
    {
        var types = App.DbContext.MaterialTypes.ToList();
        typeMaterial.ItemsSource = types;
    }

    private void Close_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }

    private void Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        currentMaterial.Name = nameMaterial.Text!;
        currentMaterial.UnitPrice = decimal.Parse(priceMaterial.Text);
        currentMaterial.QuantityInStock = decimal.Parse(stockMaterial.Text);
        currentMaterial.MinimumQuantity = decimal.Parse(minQuantityMaterial.Text);
        currentMaterial.QuantityInPackage = int.Parse(packageMaterial.Text);
        currentMaterial.UnitOfMeasure = unitMaterial.Text!;

        if (typeMaterial.SelectedItem is MaterialType selectedType)
        {
            currentMaterial.MaterialTypeId = selectedType.Id;
        }

        if (currentMaterial.Id == 0)
        {
            App.DbContext.Materials.Add(currentMaterial);
        }

        App.DbContext.SaveChanges();
        Close();
    }
}
