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
using System.Diagnostics;

namespace Gamma
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
           
            foreach (var s in Service.AlphabetDescriptions)
            {
                AlphabetComboBox.Items.Add(s);
            }
            AlphabetComboBox.SelectedIndex = 0;

            foreach(var s in Service.KeyTypeDescriptions)
            {
                KeyType.Items.Add(s);
            }
            KeyType.SelectedIndex = 0;
        }

   
        
        public (bool, String) CheckKey(String Key, String Text)
        {
            if (Key.Length != Text.Length * 8)
            {
                return (false, "Неверная длина ключа");
            }
            return (true, "Всё норм");
        }

        public void UpdateKeyColor()
        {
            String text = MainText.Text.Filter(AlphabetComboBox.SelectedIndex);
            String key = ShiftValueField.Text;
            (bool status, String m) = CheckKey(key, text);
            Brush color = status ? Service.Colors.Success : Service.Colors.Error;
            Brush fore = status ? Service.Colors.Dark : Service.Colors.Light;
            if (key.Length == 0)
            {
                ShiftValueField.Background = Service.Colors.Basic;
            }
            else
            {
                ShiftValueField.Background = color;
                ShiftValueField.Foreground = fore;
            }
            
        }



        public (bool, String) CheckMainText(String Text)
        {
            if (Text.Length == 0)
            {
                return (false, "В тексте нет символов выбранного алфавита");
            }
            return (true, "Всё норм");
        }
        
        private void ShiftValueField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex(@"[^0-1]+$").IsMatch(e.Text);
        }

        private void ShiftValueField_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                TextBox tb = (TextBox)sender;
                String Text = ((TextBox)sender).Text;
                String text = (String)e.DataObject.GetData(typeof(String));

                if (new Regex(@"[^0-1]+$").IsMatch(text))
                {
                    e.CancelCommand();
                    return;
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

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EncodedGamma.Text.Length == 0)
                {
                    DecodedText.Text = "";
                    return;
                }
                String RawText = EncodedGamma.Text;
                String RawKey = ShiftValueField.Text;
                if (RawText.Length != RawKey.Length)
                {
                    MessageBox.Show(this, "Длина ключа не соответствует длине сообщения", "Ошибка");
                    return;
                }

                int[] text = RawText.DecodeKey();
                int[] key = RawKey.DecodeKey();
                int[] result = text.Xor(key);
                DecodedText.Text = result.ConvertToText(AlphabetComboBox.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Ошибка в данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                int alphabet = AlphabetComboBox.SelectedIndex;

                String FilteredText = MainText.Text.Filter(alphabet);
                String RawKey = ShiftValueField.Text;
                (bool keyStatus, String keyError) = CheckKey(RawKey, FilteredText);

                if (!keyStatus)
                {
                    MessageBox.Show(this, keyError, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                     return;
                }
                (bool textStatus, String textError) = CheckMainText(FilteredText);
                if (!textStatus)
                {
                    MessageBox.Show(this, textError, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int[] key = RawKey.DecodeKey();
                int[] text = FilteredText.DecodeAlphabet(alphabet);
                
                int[] result = text.Xor(key);

                BinaryRepresentation.Text = text.ToBinaryString();
                EncodedGamma.Text = result.ToBinaryString();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(this, "Ошибка в данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }
        private void KeyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var origin = (ComboBox)sender;
            if (origin.IsLoaded)
            {
                if (origin.SelectedIndex == 0)
                {
                    Generate.IsEnabled = false;
                    ShiftValueField.IsReadOnly = false;
                }
                else
                {
                    Generate.IsEnabled = true;
                    ShiftValueField.IsReadOnly = true;
                    ShiftValueField.Clear();
                }
            }
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            ShiftValueField.Text = Service.GenerateRandomGamma(MainText.Text.Filter(AlphabetComboBox.SelectedIndex).Length * 8);
        }

        private void MainText_TextChanged(object sender, TextChangedEventArgs e) => UpdateKeyColor();

        private void ShiftValueField_TextChanged(object sender, TextChangedEventArgs e) => UpdateKeyColor();
    }
}
