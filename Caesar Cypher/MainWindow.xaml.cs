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
            foreach(var s in Service.AlphabetDescriptions)
            {
                AlphabetComboBox.Items.Add(s);
            }
            AlphabetComboBox.SelectedIndex = 0;
        }

        private void OperationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = OperationComboBox.SelectedIndex;
            OperationLabel.Content = Service.OperationLabelStates[index];
            LaunchButton.Content = Service.ButtonStates[index];
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            String RawShiftValue = Regex.Replace(ShiftValueField.Text, @"\s+", "");

            if (!Service.OnlyNumbers.IsMatch(RawShiftValue))
            {
                AcrylicMessageBox.Show(this, "Ошибка в поле сдвига", "Ошибка");
                return;
            }

            var index = AlphabetComboBox.SelectedIndex;
            String RawText = MainText.Text.ToLower();
            Regex CurrentAlhpabetRegex = Service.AlphabetRegices[index];
            String CurrentAlphabet = Service.Alphabets[index];
            int CurrentAlphabetLength = CurrentAlphabet.Length;


            //Понижаем регистр
            //String FilteredText = RawText.ToLower();
            ////Убираем пустое место
            //FilteredText = Regex.Replace(RawText.ToLower(), @"\s+", "");
            ////Убираем неалфавитные символы
            ///
            String FilteredText = CurrentAlhpabetRegex.Replace(Regex.Replace(RawText, @"\s+", ""), String.Empty);

            if ((bool)ForeignAlphabetCheckBox.IsChecked)
            {
                bool ru = false;
                bool en = false;
                foreach (var l in RawText)
                {
                    if (!ru && l >= 'а' && l <= 'я')
                    {
                        ru = true;
                    }
                    if (!en && l >= 'a' && l <= 'z')
                    {
                        en = true;
                    }
                    if (en && ru)
                    {
                        MessageBox.Show(this, "В тексте обнаружены символы обоих алфавитов. Выключите проверку, или исправьте текст.", "Ошибка");
                        return;
                    }
                }
            }
            
            Debug.WriteLine(RawText);
            if (FilteredText.Length == 0)
            {
                MessageBox.Show(this, "В заданном тексте нет символов выбранного алфавита", "Ошибка");
                return;
            }
            



            int ModdedShiftValue = (int)BigInteger.ModPow(BigInteger.Parse(RawShiftValue), 1, CurrentAlphabetLength);
            int RealShiftValue = OperationComboBox.SelectedIndex == 0 ? ModdedShiftValue : CurrentAlphabetLength - ModdedShiftValue;

            String result = "";
            foreach(var c in FilteredText)
            {
                result += CurrentAlphabet[(CurrentAlphabet.IndexOf(c) + RealShiftValue) % CurrentAlphabetLength];
            }

            String ResultWindowTitle = Service.ResultWindowTitles[OperationComboBox.SelectedIndex];
            (new ResultWindow(result, ResultWindowTitle)).ShowDialog();
        }
    }
}
