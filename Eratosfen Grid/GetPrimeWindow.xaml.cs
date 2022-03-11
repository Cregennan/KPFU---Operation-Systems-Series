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

namespace Eratosfen_Grid
{
    /// <summary>
    /// Логика взаимодействия для GetPrimeWindow.xaml
    /// </summary>
    public partial class GetPrimeWindow : Window
    {
        public GetPrimeWindow()
        {
            InitializeComponent();
            PrimeIndex.PreviewTextInput += Service.NumberField_PreviewTextInput;
            DataObject.AddPastingHandler(PrimeIndex, Service.NumberField_Pasting);
        }
    }
}
