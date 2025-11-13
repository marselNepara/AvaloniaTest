using Avalonia.Controls;

namespace AvaloniaTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new Materials();
        }

        private void Materials_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            MainContent.Content = new Materials();
        }

        private void Suppliers_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            MainContent.Content = new Suppliers();
        }

        private void Calculation_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            MainContent.Content = new CalculationPage();
        }
    }
}