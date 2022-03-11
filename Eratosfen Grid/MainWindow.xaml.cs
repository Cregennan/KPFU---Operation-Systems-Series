using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Eratosfen_Grid
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            HigherBound.PreviewTextInput += (s, e) => Service.NumberField_PreviewTextInput(s, e);
            LowerBound.PreviewTextInput += (s, e) => Service.NumberField_PreviewTextInput(s, e);
            DataObject.AddPastingHandler(HigherBound, Service.NumberField_Pasting);
            DataObject.AddPastingHandler(LowerBound, Service.NumberField_Pasting);
        }


        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (LowerBound.Text.Length > 6 || HigherBound.Text.Length > 6)
            //{
            //    MessageBox.Show(this, "Ошибка", "Введено слишком большое число. Возможно переполнение памяти");
            //    return;
            //}
            int lower = Int32.Parse(LowerBound.Text);
            int higher = Int32.Parse(HigherBound.Text);
            if (lower > higher)
            {
                (LowerBound.Text, HigherBound.Text) = (HigherBound.Text, LowerBound.Text);
                (lower, higher) = (higher, lower);
            }
            if (lower < 2)
            {
                lower = 2;
                LowerBound.Text = "2";
            }

            bool[] A = new bool[higher + 1];
            Stopwatch gen = Stopwatch.StartNew();
            for (int i = 0; i < A.Length; i++)
            {
                A[i] = i.Check();
            }
            gen.Stop();
            Stopwatch comp = Stopwatch.StartNew();

            for (int i = 2; i <= Math.Sqrt(A.Length - 1); i++)
            {
                if (A[i])
                {
                    int j = i * i;
                    while (j <= A.Length - 1)
                    {
                        A[j] = false;
                        j += i;
                    }
                }
            }

            comp.Stop();

            Debug.WriteLine("Время генерации: " + gen.Elapsed.TotalMilliseconds + " мс");
            Debug.WriteLine("Время вычислений: " + comp.Elapsed.TotalMilliseconds + " мс");
            Debug.WriteLine("Количество: " + higher);

            A.AllFile(lower);
        }




     
    }
}
