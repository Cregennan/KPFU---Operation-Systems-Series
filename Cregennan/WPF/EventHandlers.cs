using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cregennan.WPF
{
    public class EventHandlers
    {
        public static void Uint_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = (new Regex(@"[^0-9]+$")).IsMatch(e.Text);
        }
        public static void UInt_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String TextBoxText = ((TextBox)sender).Text;
                String text = (String)e.DataObject.GetData(typeof(String));
                if (new Regex(@"[^0-9]+$").IsMatch(text))
                {
                    e.CancelCommand();
                    return;
                }
            }
            else
            {
                e.CancelCommand();
                return;
            }
        }
            
        public static void Deny_Space(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
