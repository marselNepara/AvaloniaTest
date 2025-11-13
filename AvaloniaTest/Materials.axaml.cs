using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaTest.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AvaloniaTest;

public partial class Materials : UserControl
{
    public Materials()
    {
        InitializeComponent();
        LoadData();
    }
    private void LoadData()
    {
        var materials = App.DbContext.Materials
            .Include(m => m.MaterialType)
            .ToList()
            .Select(m => new
            {
                m.Id,
                m.Name,
                MaterialType = m.MaterialType,
                m.UnitPrice,
                m.QuantityInStock,
                m.MinimumQuantity,
                m.QuantityInPackage,
                m.UnitOfMeasure,
                TotalCost = CalculatePurchaseCost(m)
            })
            .ToList();

        dgMaterials.ItemsSource = materials;
    }
    private string CalculatePurchaseCost(Material material)
    {
        if (material.QuantityInStock >= material.MinimumQuantity)
            return "Not needed";

        var deficit = material.MinimumQuantity - material.QuantityInStock;
        var packagesNeeded = (int)System.Math.Ceiling(deficit / material.QuantityInPackage);
        var quantityToBuy = packagesNeeded * material.QuantityInPackage;
        var cost = quantityToBuy * material.UnitPrice;

        return $"{cost:F2} руб.";
    }
    private void Delete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var anonymousObject = (sender as Button).Tag;
        var idProperty = anonymousObject.GetType().GetProperty("Id");
        var id = (int)idProperty.GetValue(anonymousObject);

        var material = App.DbContext.Materials.FirstOrDefault(m => m.Id == id);

        if (material != null)
        {
            App.DbContext.Remove(material);
            App.DbContext.SaveChanges();
            LoadData();
        }
    }

    private async void Edit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
       
        var anonymousObject = (sender as Button).Tag;
        var idProperty = anonymousObject.GetType().GetProperty("Id");
        var id = (int)idProperty.GetValue(anonymousObject);

        var material = App.DbContext.Materials
            .Include(m => m.MaterialType)
            .FirstOrDefault(m => m.Id == id);

        if (material != null)   
        {
            var editWindow = new AddEditMaterialWindow(material);
            var parent = this.VisualRoot as Window;
            await editWindow.ShowDialog(parent);

            LoadData();
        }
    }

    private async void Add_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var addWindow = new AddEditMaterialWindow();
        var parent = this.VisualRoot as Window;
        await addWindow.ShowDialog(parent);

        LoadData();
    }
}