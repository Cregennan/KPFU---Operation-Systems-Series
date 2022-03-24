using System;
using System.Collections;
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
        public static String AllFile(this BitArray ts, int lower)
        {

            using (StreamWriter file = new StreamWriter("Result.txt", false))
            {
                for (int i = lower; i < ts.Length; i++)
                {
                    if (ts[i])
                    {
                        file.WriteLine(i);
                    }
                }
                file.Close();
            }

            StreamReader input = new StreamReader("Result.txt");
            String t = input.ReadToEnd();
            input.Close();
            return t;
        }

        public static BitArray GetPrimesUpTo(int limit)
        {
            var sieve = new BitArray(limit + 1);
            for (long x2 = 1, dx2 = 3; x2 < limit; x2 += dx2, dx2 += 2)
                for (long y2 = 1, dy2 = 3, n; y2 < limit; y2 += dy2, dy2 += 2)
                {
                    n = (x2 << 2) + y2;
                    if (n <= limit && (n % 12 == 1 || n % 12 == 5))
                        sieve[(int)n] ^= true;
                    n -= x2;
                    if (n <= limit && n % 12 == 7)
                        sieve[(int)n] ^= true;
                    if (x2 > y2)
                    {
                        n -= y2 << 1;
                        if (n <= limit && n % 12 == 1)
                            sieve[(int)n] ^= true;
                    }
                }
                        int r = 5;
            for (long r2 = r * r, dr2 = (r << 1) + 1; r2 < limit; ++r, r2 += dr2, dr2 += 2)
                if (sieve[r])
                    for (long mr2 = r2; mr2 < limit; mr2 += r2)
                        sieve[(int)mr2] = false;


            if (limit > 2)
                sieve[2] = true;
            if (limit > 3)
                sieve[3] = true;
            return sieve;
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
