using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CoolerGamma
{
    public partial class MainWindow
    {
        public Service.KeyTypes SelectedKeyType = Service.KeyTypes.AUTO;

        private bool ValidateKeyInput(String t)
        {
            switch (SelectedKeyType)
            {
                case Service.KeyTypes.AUTO:
                    return Service.isValueHex(t);
                case Service.KeyTypes.BINARY:
                    return Service.isValueBinary(t);
                case Service.KeyTypes.HEX:
                    return Service.isValueHex(t);
                default:
                    return false;
            }
        }

        public Service.KeyTypes Detect
        {
            get
            {
                return Service.DetectKeyType(SourceTextBox.Text, KeyTextBox.Text, SelectedKeyType);
            }
            private set { }
        }



        private (String, Brush) GetCurrentLabelState(String source, String key)
        {
            Service.KeyTypes detectedType = Service.DetectKeyType(source, key, SelectedKeyType);
            if (SelectedKeyType == Service.KeyTypes.AUTO)
            {
                switch (detectedType)
                {
                    case Service.KeyTypes.AUTO:
                    case Service.KeyTypes.UNDEFINED:
                        return ("", Service.Colors.Transparent);
                    default:
                        return (Service.KeyTypeLabelValues[(int)detectedType - 1], Service.Colors.DefaultDetectedKeyType);
                }

            }
            else
            {
                return ("", Service.Colors.Transparent);
            }

        }
        private void UpdateLabelState(String source, String key)
        {
            (String text, Brush br) = GetCurrentLabelState(source, key);
            KeyTypeLabel.Content = text;
            KeyTypeLabel.Foreground = br;
        }

        public MainWindow()
        {
            InitializeComponent();
            ClearOutput.Click += (object sender, RoutedEventArgs e) => ResultTextBox.Clear();

            foreach (var t in Service.Operations)
            {
                OperationComboBox.Items.Add(t);
            }
            foreach (var t in Service.KeyTypeDescriptions)
            {
                KeyType.Items.Add(t);
            }
            KeyTypeLabel.Foreground = Service.GetColor("#00000000");

            KeyType.SelectedIndex = 0;
            OperationComboBox.SelectedIndex = 0;

            //Debug.WriteLine("Ты гей".ConvertToBytes().ToHexString()); 
            foreach (var t in "Ты гей")
            {
                Debug.WriteLine(((byte)t));
            }
            foreach (byte t in "Ты гей".ConvertToInts())
            {
                Debug.WriteLine(t);
            }
        }

        private (bool, String) VerifyKeyWithText(String text, String key, Service.KeyTypes type)
        {
            if (type == Service.KeyTypes.BINARY)
            {
                bool state = key.Length == text.Length * 8;
                return (state, state ? "Всё ок" : "Каждой букве текста должно соответствовать 8 битное двоичное число");
            }
            if (type == Service.KeyTypes.HEX)
            {
                bool state = key.Length == text.Length * 2;
                return (state, state ? "Всё ок" : "Каждой букве текста должно соответствовать двухзначное шестадцатеричное число");
            }
            return (false, "Ошибка в ключе");
        }

        private void KeyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedKeyType = (Service.KeyTypes)((ComboBox)sender).SelectedIndex;


            if (SelectedKeyType == Service.KeyTypes.BINARY && !Service.isValueBinary(KeyTextBox.Text))
            {
                KeyTextBox.Clear();
            }
            UpdateLabelState(SourceTextBox.Text, KeyTextBox.Text);
        }

        private void KeyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidateKeyInput(e.Text);
        }

        private void KeyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateLabelState(SourceTextBox.Text, KeyTextBox.Text);
            Debug.WriteLine(Service.isValueBinary(SourceTextBox.Text));
        }

        private void KeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Service.KeyTypes DetectedType = Service.DetectKeyType(SourceTextBox.Text, KeyTextBox.Text, SelectedKeyType);
            (bool status, string message) = VerifyKeyWithText(SourceTextBox.Text, KeyTextBox.Text, DetectedType);
            //if (!status)
            //{
            //    MessageBox.Show(this, message, "Ошибка", icon: MessageBoxImage.Error, button: MessageBoxButton.OKCancel);
            //    return;
            //}

            if (OperationComboBox.SelectedIndex == 0)
            {
                ResultTextBox.Text = Service.Cypher(SourceTextBox.Text, KeyTextBox.Text, DetectedType);
            }
            else
            {
                ResultTextBox.Text = Service.Decypher(SourceTextBox.Text, KeyTextBox.Text, DetectedType);
            }
        }

        private void RandomKey_Click(object sender, RoutedEventArgs e)
        {
            KeyTextBox.Text = Service.GenerateKey(SourceTextBox.Text, SelectedKeyType);
        }
    }
}
