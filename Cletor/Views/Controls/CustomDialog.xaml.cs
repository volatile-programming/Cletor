using Cletor.Views.Helpers;
using ControlzEx.Theming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Cletor.Views.Controls
{
    /// <summary>
    /// Interaction logic for CustomDialog.xaml
    /// </summary>
    public partial class CustomDialog
    {
        private static CustomDialog _lastInstance;
        private Dictionary<PropertyInfo, TextBox> _virtualFrom;
        private MainWindow _window;

        public CustomDialog(MainWindow window)
        {
            InitializeComponent();
            _window = window;
            _virtualFrom = new Dictionary<PropertyInfo, TextBox>();
            Loaded += OnLoad;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            ThemeManager.Current.ChangeTheme(this, $"{ConfigurationHandler.Current.Theme}.Blue");
        }

        private void OnUserInput(object sender, RoutedEventArgs e) =>
            DialogResult = sender.Equals(AcceptButton);

        public static Task<T> Show<T>(MainWindow window, string title) where T : class
        {
            _lastInstance = new CustomDialog(window);
            var dataType = typeof(T);

            var properties = _lastInstance.GetProperties(dataType);
            _lastInstance.CreateUiFromProperties(title, properties);
            var result = _lastInstance.GetUserInput<T>(properties);

            return Task.FromResult(result);
        }

        private List<PropertyInfo> GetProperties(Type dataType)
        {
            var properties = dataType
                .GetProperties()
                .ToList();

            return properties;
        }

        private void CreateUiFromProperties(string title, List<PropertyInfo> properties)
        {
            Title = title;

            foreach (PropertyInfo property in properties)
            {
                var control = CreateFormControl(property);
                Container.Children.Add(control);
            }
        }

        private UIElement CreateFormControl(PropertyInfo property)
        {
            var headerText = property.Name.All(x => char.IsUpper(x)) ?
                property.Name :
                Regex.Replace(property.Name, "(\\B[A-Z])", " $1");

            var header = new TextBlock() { Text = headerText };
            var input = new TextBox();
            var stackPanel = new StackPanel() { Margin = new Thickness(0, 0, 0, 20) };

            stackPanel.Children.Add(header);
            stackPanel.Children.Add(input);
            _virtualFrom.Add(property, input);

            return stackPanel;
        }

        private T GetUserInput<T>(List<PropertyInfo> properties) where T : class
        {
            var result = ShowDialog();
            if (result == false)
                return null;

            var data = Activator.CreateInstance<T>();
            foreach (var property in properties)
                property.SetValue(data, _virtualFrom[property].Text);

            return data;
        }
    }
}
