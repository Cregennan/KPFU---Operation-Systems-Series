using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Atkin_Sieve
{
    public static class Service
    {
        public static bool Check(this int i)
        {
            return (i % 2 != 0 || i == 2) && (i % 3 != 0 || i == 3) && (i % 5 != 0 || i == 5);
        }
        public static String AllFile(this bool[] ts, int lower)
        {

            using (StreamWriter file = new StreamWriter("Result.txt", false))
            {
                for (int i = lower; i < ts.Length; i++)
                {
                    if (ts[i])
                    {
                        file.Write(i);
                        file.Write(" ");
                    }
                }
                file.Close();
            }

            StreamReader input = new StreamReader("Result.txt");
            String t = input.ReadToEnd();
            input.Close();
            return t;
        }


        public static void NumberField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = (new Regex(@"[^0-9]+$")).IsMatch(e.Text);
        }
        public static void NumberField_Pasting(object sender, DataObjectPastingEventArgs e)
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


    }
}
