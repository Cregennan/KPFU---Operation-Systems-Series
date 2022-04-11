using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Numerics;
using Cregennan.Cryptography.Numerics;
using System.Diagnostics;

namespace UniversalPrimeGenerator
{
    /// <summary>
    /// Логика взаимодействия для AllWindow.xaml
    /// </summary>
    public partial class AllWindow
    {
        SourceChord.FluentWPF.AcrylicWindow parentWindow;



        public AllWindow(SourceChord.FluentWPF.AcrylicWindow parent)
        {
            InitializeComponent();
            parentWindow = parent;
            this.Closing += (object sender, System.ComponentModel.CancelEventArgs e) =>
            {
                System.Windows.Application.Current.Shutdown();
            };
            number.PreviewTextInput += Cregennan.WPF.EventHandlers.Uint_PreviewTextInput;
            DataObject.AddPastingHandler(number, Cregennan.WPF.EventHandlers.UInt_Pasting);
            number.PreviewKeyDown += Cregennan.WPF.EventHandlers.Deny_Space;

            bits.PreviewKeyDown += Cregennan.WPF.EventHandlers.Deny_Space;
            bits.PreviewTextInput += Cregennan.WPF.EventHandlers.Uint_PreviewTextInput;
            bits.PreviewKeyDown += Cregennan.WPF.EventHandlers.Deny_Space;
        }
        void ShowResult(PrimeNumberVerifier verifier, TextBlock target)
        {
            
                Stopwatch st = Stopwatch.StartNew();
                var result = verifier.Test(BigInteger.Parse(number.Text));
                st.Stop();
                var probability = verifier.TestAccuracy(BigInteger.Parse(number.Text));
                target.Text = result ? "Число простое с вероятностью " + probability : "Число составное";
                target.Text += "\r\n Время проверки: " + st.Elapsed.TotalMilliseconds + "мс";
          
        }

        void Generate(PrimeNumberVerifier verifier)
        {
            Stopwatch gen = Stopwatch.StartNew();
            BigInteger t = Cregennan.Cryptography.Numerics.Utils.GetPrimeByLength(Int32.Parse(bits.Text), verifier);
            gen.Stop();

            genResult.Text = t.ToString();
            generatorTime.Content = "Сгенерировано за " + gen.Elapsed.TotalMilliseconds + "мс.";
        }

        void Average()
        {

            var rounds = 50; //Количество чисел для генерации

            var verifiers = new List<PrimeNumberVerifier>()
            {
                new FermatVerifier(),
                new MillerRabinVerifier(),
                new SoloveySchtrassenVerifier()
            };
            var desc = new List<String>()
            {
                "Тест Ферма",
                "Тест Миллера-Рабина",
                "Тест Соловея-Штрассена"
            };
            String s = "";

            for(int k = 0; k < verifiers.Count; k++)
            {
                double ms = 0d;                
                for (int i = 0; i < rounds; i++)
                {
                    Stopwatch st = Stopwatch.StartNew();
                    var n = Cregennan.Cryptography.Numerics.Utils.GetPrimeByLength(Int32.Parse((bits.Text)), verifiers[k]);
                    st.Stop();
                    ms += st.Elapsed.TotalMilliseconds;
                }
                s += desc[k] + ": " + ms / rounds + "мс\r\n";
            }
            SourceChord.FluentWPF.AcrylicMessageBox.Show(this, s);
        }





        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                ShowResult(new FermatVerifier(), Result1);
                ShowResult(new MillerRabinVerifier(), Result2);
                ShowResult(new SoloveySchtrassenVerifier(), Result3);
            } catch(Exception ex)
            {
                MessageBox.Show("Ошибка в числе для проверки");
            }
        }

        private void Test1_Click(object sender, RoutedEventArgs e)
        {
            Generate(new FermatVerifier());
        }

        private void Test2_Click(object sender, RoutedEventArgs e)
        {
            Generate(new MillerRabinVerifier());
        }

        private void Test3_Click(object sender, RoutedEventArgs e)
        {
            Generate(new SoloveySchtrassenVerifier());
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Average();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Hide();
            parentWindow.Show();
        }
    }
}
