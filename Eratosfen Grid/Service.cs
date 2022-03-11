using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Eratosfen_Grid
{
    public static class Service
    {
        public static bool Check(this int i)
        {
            return (i % 2 != 0 || i == 2) && (i % 3 != 0 || i == 3) && (i % 5 != 0 || i == 5);
        }
        public static String All(this bool[] ts, int lower)
        {
            StringBuilder sb = new StringBuilder();    
            for (int i = 2; i < ts.Length; i++)
            {
                if (ts[i] && i > lower)
                {
                   sb.Append(i);
                   sb.Append(" ");
                }
            }
            return sb.ToString();
        }
        public static void AllFile(this bool[] ts, int lower)
        {
            StreamWriter file = new StreamWriter("Result.txt", false);
            for (int i = lower; i < ts.Length; i++)
            {
                if (ts[i])
                {
                    file.WriteLine(i);
                }
            }
            file.Close();
            Process.Start("Result.txt");
        }



        public static String Alls(this bool[] ts, int lower)
        {
            return String.Join(", ", ts.Select( (x, i) => (x && i >= lower) ? i.ToString() : "").Where(x => x != ""));
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
