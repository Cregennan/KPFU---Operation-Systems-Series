using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Atkin_Sieve
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
            DataObject.AddPastingHandler(Number, Service.NumberField_Pasting);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long N = 0;
                if (Number.Text.Length == 0)
                {
                    MessageBox.Show(this, "Ошибка в числе", "Ошибка");
                    return;
                }
                N = long.Parse(Number.Text);
                if (N < 2) throw new Exception();

                Stopwatch generator = Stopwatch.StartNew();
                BitArray b = Service.GetPrimesUpTo(N);
                generator.Stop();
                Debug.WriteLine("Время вычисления: " + generator.Elapsed.TotalMilliseconds);

                Stopwatch output = Stopwatch.StartNew();
                Result.Text = b.AllFile(2);
                output.Stop();
                Debug.WriteLine("Время вывода: " + output.Elapsed.TotalMilliseconds);

            }catch (Exception ex)
            {
                MessageBox.Show(this, "Ошибка в числе", "Ошибка");
                return;
            }
            
        }
    }
}
