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
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;

namespace Miller_Rabin_Test
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
       
        public MainWindow()
        {
            InitializeComponent();
            Number.PreviewTextInput += Service.NumberField_PreviewTextInput;
            Rounds.PreviewTextInput += Service.NumberField_PreviewTextInput;
            DataObject.AddPastingHandler(Number, Service.NumberField_Pasting);
            DataObject.AddPastingHandler(Rounds, Service.NumberField_Pasting);
            Rounds.Text = "5";
            Status.Text = "Ожидание...";
        }

        public async void SetStatus(String text)
        {
            Status.Text = text;
            await Task.Delay(1);
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (Rounds.Text.Length == 0)
                {
                    throw new Exception("Неверное число раундов");
                }
                if (Number.Text.Length == 0) {
                    throw new Exception("Неверное число для проверки");
                }
                BigInteger num = BigInteger.Parse(Number.Text);
                if (num < 2)
                {
                    throw new Exception("Неверное число для проверки");
                }
                int rounds = Int32.Parse(Rounds.Text);

                SetStatus("Вычисление...");
                bool result = num.MillerRabin(rounds);
                Debug.WriteLine(1 - Math.Pow(4, -rounds));

                Status.Text = result ? "Число простое с вероятностью " + Math.Truncate( (1 - Math.Pow(4, -rounds)) * 100) +"%" : "Число составное";
                
            }catch(Exception ex) { 

                MessageBox.Show(this, ex.Message.Length > 0 ? ex.Message : "Проверьте правильность данных", "Ошибка");
                return;
            }
        }
    }
}
