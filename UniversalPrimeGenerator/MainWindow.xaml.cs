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
        }
    }
}
