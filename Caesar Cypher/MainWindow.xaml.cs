using SourceChord.FluentWPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
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

namespace Caesar_Cypher
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {


        public MainWindow()
        {
            InitializeComponent();
            foreach (var s in Service.Operations)
            {
                OperationComboBox.Items.Add(s);
            }
            OperationComboBox.SelectedIndex = 0;
            foreach (var s in Service.AlphabetDescriptions)
            {
                AlphabetComboBox.Items.Add(s);
            }
            AlphabetComboBox.SelectedIndex = 0;
        }
        
        public (bool, String, String) CheckShiftValue(String OldValue)
        {
            if (OldValue.Length == 0)
            {
                if ((bool)CheckEmptyShiftValue.IsChecked)
                {
                    return (true, "Поле для сдвига не может быть пустым", OldValue);
                }
                else
                {
                    return (false, "Всё норм", "0");
                }
            }
            else
            {
                return (false, "Всё норм", OldValue);
            }
        }

        public (bool, String) CheckMainText(String Text)
        {
            if (Text.Length == 0)
            {
                return (true, "В тексте нет символов  выбранного алфавита");
            }
            return (false, "Всё норм");
        }
        private void OperationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = OperationComboBox.SelectedIndex;
            OperationLabel.Content = Service.OperationLabelStates[index];
            LaunchButton.Content = Service.ButtonStates[index];
           
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            (bool ShiftValueStatus, String Message, String RawShiftValue) = CheckShiftValue(ShiftValueField.Text);
            if (ShiftValueStatus)
            {
                MessageBox.Show(this, Message, "Ошибка");
                return;
            }



            Regex CurrentAlhpabetRegex = Service.FilterRegices[AlphabetComboBox.SelectedIndex];
            String CurrentAlphabet = Service.Alphabets[AlphabetComboBox.SelectedIndex];
            int CurrentAlphabetLength = CurrentAlphabet.Length;
            String FilteredText = CurrentAlhpabetRegex.Replace(Regex.Replace(MainText.Text.ToLower(), @"\s+", ""), String.Empty);


            (bool TextStatus, String Message1) = CheckMainText(FilteredText);
            if (TextStatus)
            {
                MessageBox.Show(this, Message1, "Ошибка");
                return;
            }


            int ModdedShiftValue = (int)(BigInteger.Parse(ShiftValueField.Text) % CurrentAlphabetLength);
            int RealShiftValue = OperationComboBox.SelectedIndex == 0 ? ModdedShiftValue : CurrentAlphabetLength - ModdedShiftValue;
            ResultTextBox.Text = Service.Encrypt(FilteredText, RealShiftValue, CurrentAlphabet);
        }

        private void ShiftValueField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "-")
            {
                if (((TextBox)sender).Text.Length == 0)
                {
                    e.Handled = false;
                }
                else
                {
                    if (!((TextBox)sender).Text.Contains("-") && ((TextBox)sender).CaretIndex == 0)
                    {                   
                        e.Handled = false;
                    }else
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = !(new Regex(@"^[0-9]+$")).IsMatch(e.Text);
            }
        }

        private void ShiftValueField_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String TextBoxText = ((TextBox)sender).Text;
                String text = (String)e.DataObject.GetData(typeof(String));
                if (TextBoxText.Contains("-") && text.Contains("-"))
                {
                    e.CancelCommand();
                }
                if(!(TextBoxText.Length == 0)  && text.Contains("-"))
                {
                    if (!(new Regex(@"^-?\d+$")).IsMatch(text))
                    {
                        e.CancelCommand();
                    }
                }
                
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void AlphabetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).IsLoaded)
            {
                MainText.Clear();
            }
        }

        private void MainText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex currentRegex = AlphabetComboBox.SelectedIndex == 1 ? Service.TextAllowed[0] : Service.TextAllowed[1];

            if (currentRegex.IsMatch(e.Text))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void MainText_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String TextBoxText = ((TextBox)sender).Text;
                String text = (String)e.DataObject.GetData(typeof(String));
                Regex currentRegex = AlphabetComboBox.SelectedIndex == 1 ? Service.TextAllowed[0] : Service.TextAllowed[1];
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

        private void ShiftValueField_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
