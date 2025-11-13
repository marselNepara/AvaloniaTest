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
    public void LoadData()
    {
        dgMaterials.ItemsSource = App.DbContext.Materials.Include(m => m.MaterialType).ToList();
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