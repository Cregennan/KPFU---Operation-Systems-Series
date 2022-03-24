using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
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

namespace UniversalPrimeGenerator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow() 
        {
            InitializeComponent();
            bits.PreviewKeyDown += Cregennan.WPF.EventHandlers.Deny_Space;
            bits.PreviewTextInput += Cregennan.WPF.EventHandlers.Uint_PreviewTextInput;
            DataObject.AddPastingHandler(bits, Cregennan.WPF.EventHandlers.UInt_Pasting);
            foreach(var s in Service.Algorythms)
            {
                genAlgorytm.Items.Add(s);
                checkAlgorytm.Items.Add(s);
            }
            genAlgorytm.SelectedIndex = 0;
            checkAlgorytm.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int bit = Int32.Parse(bits.Text);
                if (bit < 2)
                {
                    throw new Exception();
                }
                BigInteger number = Cregennan.Cryptography.Numerics.Utils.GetPrimeByLength(bit, Service.VerifierFactory(genAlgorytm.SelectedIndex));
                Result.Text = number.ToString();

            }catch(Exception ex)
            {
                MessageBox.Show(this, "Ошибка в числах", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var n = BigInteger.Parse(number.Text);
                if (n < 2)
                {
                    throw new Exception();
                }
                bool res = Service.VerifierFactory(checkAlgorytm.SelectedIndex).Test(n);
                MessageBox.Show(this, res ? "Число возможно простое" : "Число составное", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
            }catch(Exception ex)
            {
                MessageBox.Show(this, "Ошибка в числах", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
