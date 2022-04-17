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

namespace RSA
{
    /// <summary>
    /// Логика взаимодействия для TestWindow.xaml
    /// </summary>
    public partial class TestWindow
    {
        public TestWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var a = Int32.Parse(af.Text);
            var b = Int32.Parse(bf.Text);
            (var x, var y, var nod) = Cregennan.Cryptography.Numerics.Utils.ExtendedGCD(a, b);
            (xf.Text, yf.Text, gcdf.Text) = (x.ToString(), y.ToString(), nod.ToString());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var bas = Int32.Parse(base_f.Text);
            var exp = int.Parse(exp_f.Text);
            var mod = int.Parse(mod_f.Text);

            res_f.Text = System.Numerics.BigInteger.ModPow(bas, exp, mod).ToString();
        }
    }
}
