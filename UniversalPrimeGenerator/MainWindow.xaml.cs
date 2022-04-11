using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Cregennan.Cryptography.Numerics;
namespace UniversalPrimeGenerator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        AllWindow child;
        

        public MainWindow() 
        {
            InitializeComponent();
            child = new AllWindow(this);
            this.Closing += (object sender, System.ComponentModel.CancelEventArgs e) =>
            {
                System.Windows.Application.Current.Shutdown();
            };


            bits.PreviewKeyDown += Cregennan.WPF.EventHandlers.Deny_Space;
            bits.PreviewTextInput += Cregennan.WPF.EventHandlers.Uint_PreviewTextInput;

            number.PreviewKeyDown += Cregennan.WPF.EventHandlers.Deny_Space;
            number.PreviewTextInput += Cregennan.WPF.EventHandlers.Uint_PreviewTextInput;


            DataObject.AddPastingHandler(bits, Cregennan.WPF.EventHandlers.UInt_Pasting);
            DataObject.AddPastingHandler(number, Cregennan.WPF.EventHandlers.UInt_Pasting);



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
                Stopwatch sw = Stopwatch.StartNew();
                var verifier = Service.VerifierFactory(checkAlgorytm.SelectedIndex);
                bool res = verifier.Test(n);
                sw.Stop();
                Debug.WriteLine(sw.Elapsed.TotalMilliseconds);

                result.Text = res ? "Число простое с вероятностью " + verifier.TestAccuracy(n) : "Число составное";
                result.Text += "\r\n Время проверки: " + sw.Elapsed.TotalMilliseconds + "мс";
            }catch(Exception ex)
            {
                MessageBox.Show(this, "Ошибка в числах", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool state = false;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            state = !state;
            if (state)
            {
                SourceChord.FluentWPF.ResourceDictionaryEx.GlobalTheme = SourceChord.FluentWPF.ElementTheme.Light;
                Тема.Content = "Светлая тема";
            }
            else
            {
                SourceChord.FluentWPF.ResourceDictionaryEx.GlobalTheme = SourceChord.FluentWPF.ElementTheme.Dark;
                Тема.Content = "Тёмная тема";
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            child.Show();
            this.Hide();
        }

    }
}
