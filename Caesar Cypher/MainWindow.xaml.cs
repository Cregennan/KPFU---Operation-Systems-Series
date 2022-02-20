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
            ClearOut.Click += (object k, RoutedEventArgs t) => ResultTextBox.Clear();
        }
        
        public (bool, String, String) CheckShiftValue(String OldValue)
        {
            if (OldValue.Length == 0)
            {
                if (!(bool)CheckEmptyShiftValue.IsChecked)
                {
                    return (true, "Поле для сдвига не может быть пустым", OldValue);
                }
                else
                {
                    ShiftValueField.Text = "0";
                    return (false, "Всё норм", "0");
                }
            }
            if (OldValue == "-")
            {
                return (true, "Поле сдвига содержит только минус, возможно вы хотели ввести отрицательное число", OldValue);
            }
            if (!(new Regex(@"^-?[0-9]+$")).IsMatch(OldValue))
            {
                return (true, "Ошибка в поле сдвига", OldValue);
            }

            return (false, "Всё норм", OldValue);
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
            try
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


                int ModdedShiftValue = (int)(BigInteger.Parse(RawShiftValue) % CurrentAlphabetLength);
                Debug.WriteLine("Подсчет ключа завершен");

                int RealShiftValue = 0;
                if (ModdedShiftValue >= 0)
                {
                    RealShiftValue = OperationComboBox.SelectedIndex == 0 ? ModdedShiftValue : CurrentAlphabetLength - ModdedShiftValue;
                }
                else
                {
                    RealShiftValue = OperationComboBox.SelectedIndex == 0 ? CurrentAlphabetLength + ModdedShiftValue : -ModdedShiftValue;
                }



                ResultTextBox.Text = Service.Encrypt(FilteredText, RealShiftValue, CurrentAlphabet);
            }catch(Exception ex)
            {
                MessageBox.Show(this, "Ошибка в данных, перепроверьте введенную информацию", "Ошибка");
            }
        }

        private void ShiftValueField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
           TextBox tb = (TextBox)sender;

            if (tb.SelectionLength > 0)
            {
                if(tb.SelectionStart == 0)
                {
                    if(e.Text == "-")
                    {
                        e.Handled = false;
                        return;
                    }
                    else
                    {
                        e.Handled = !(new Regex(@"^[0-9]+$")).IsMatch(e.Text);
                        return;
                    }
                    
                }   
            }
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
                if(((TextBox)sender).CaretIndex == 0 && ((TextBox)sender).Text.Contains("-"))
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = !(new Regex(@"^[0-9]+$")).IsMatch(e.Text);
                }
               
            }
        }

        private void ShiftValueField_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                TextBox tb = (TextBox)sender;
                String Text = ((TextBox)sender).Text;
                String text = (String)e.DataObject.GetData(typeof(String));

                if (tb.SelectionLength > 0)
                {
                    if (tb.SelectionStart == 0) 
                    {
                        if (text == "-")
                        {
                            return;
                        }
                        if (tb.Text.Contains("-"))
                        {
                            if (!(new Regex(@"^-?[0-9]+$")).IsMatch(text))
                            {
                                e.CancelCommand();
                                return;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (!(new Regex(@"^[0-9]+$")).IsMatch(text))
                        {
                            e.CancelCommand();
                            return;
                        }
                    }
                }
                if (Text.Contains("-") && text.Contains("-"))
                {
                    e.CancelCommand();
                    return;
                }
                if (tb.CaretIndex == 0)
                {
                    if (Text.Contains("-"))
                    {
                        e.CancelCommand();
                        return;
                    }
                    if (text == "-")
                    {
                        //Можно
                        return;
                    }
                    else if (!(new Regex(@"^-?[0-9]+$")).IsMatch(text))
                    {
                        e.CancelCommand();
                    }
                }
                else
                {
                    if (!(new Regex(@"^[0-9]+$")).IsMatch(text))
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
