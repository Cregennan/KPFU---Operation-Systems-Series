using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vigeneer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static String Multiply(String source, int count)
        {
            String res = ""; 
            for(int i = 0; i < count; i++)
                res += source; 
            return res;
        }
        public (bool, String) CheckText(String Text)
        {
            if (Text.Length == 0)
            {
                return (true, "В тексте нет символов  выбранного алфавита");
            }
            return (false, "Всё норм");
        }
        public (bool, String) CheckShift(String Text)
        {
            if (Text.Length == 0)
            {
                return (true, "В тексте нет символов  выбранного алфавита");
            }
            return (false, "Всё норм");
        }
        public MainWindow()
        {
            InitializeComponent();
            foreach (var t in Service.AlphabetDescriptions)
            {
                AlphabetComboBox.Items.Add(t);
            }
            foreach (var t in Service.Operations)
            {
                OperationComboBox.Items.Add(t);
            }
            OperationComboBox.SelectedIndex = 0;
            AlphabetComboBox.SelectedIndex = 0;
        }

        private void SourceTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String TextBoxText = ((TextBox)sender).Text;
                String text = (String)e.DataObject.GetData(typeof(String));
                Regex currentRegex = Service.ShiftRegices[AlphabetComboBox.SelectedIndex];
                if (!currentRegex.IsMatch(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void SourceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Lang = Service.TextAllowed[AlphabetComboBox.SelectedIndex];
            if (!Lang.IsMatch(e.Text))
            {
                e.Handled = true;
                return;
            }
            e.Handled = false;
        }

        private void KeyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Lang = Service.ShiftRegices[AlphabetComboBox.SelectedIndex];
            if (!Lang.IsMatch(e.Text))
            {
                e.Handled = true;
                return;
            }
            e.Handled = false;
        }

        private void KeyTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String TextBoxText = ((TextBox)sender).Text;
                String text = (String)e.DataObject.GetData(typeof(String));
                Regex currentRegex = Service.ShiftRegices[AlphabetComboBox.SelectedIndex];
                if (!currentRegex.IsMatch(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Regex CurrentAlphabetRegex = Service.FilterRegices[AlphabetComboBox.SelectedIndex];
                String CurrentAlphabet = Service.Alphabets[AlphabetComboBox.SelectedIndex];

                String FilteredText = CurrentAlphabetRegex.Replace(Regex.Replace(SourceTextBox.Text.ToLower(), @"\s+", ""), String.Empty);
                String FilteredShift = KeyTextBox.Text.ToLower();

                (bool TextCheckResult, String TextCheckError) = CheckText(FilteredText);
                if (TextCheckResult)
                {
                    MessageBox.Show(this, TextCheckError, "Ошибка");
                    return;
                }
                (bool ShiftCheckResult, String ShiftCheckError) = CheckShift(FilteredShift);
                if (ShiftCheckResult)
                {
                    MessageBox.Show(this, ShiftCheckError, "Ошибка");
                    return;
                }

                String PreparedKey = "";

                if (FilteredShift.Length > FilteredText.Length)
                {
                    PreparedKey = FilteredShift.Substring(0, FilteredText.Length);
                }
                else
                {
                    PreparedKey = Multiply(FilteredShift, FilteredText.Length / FilteredShift.Length + 1).Substring(0, FilteredText.Length);
                }

                String Result = Service.Encrypt(FilteredText, PreparedKey, CurrentAlphabet, OperationComboBox.SelectedIndex == 0);

                ResultTextBox.Text = Result;
            }catch (Exception ex)
            {
                MessageBox.Show(this, "Проверьте правильность заполнения полей", "Ошибка");
                return;
            }
            

        }

        private void KeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled=true;
            }
        }

        private void AlphabetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlphabetComboBox.IsLoaded)
            {
                KeyTextBox.Clear();
                SourceTextBox.Clear();
            }
        }

        private void ResultTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(ResultTextBox.Text);
        }

        private void OperationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = OperationComboBox.SelectedIndex;
            SourceTextLabel.Content = Service.OperationLabelStates[i];
            Start.Content = Service.ButtonStates[i];
        }

        private void ClearOutput_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Clear();
        }
    }
}
