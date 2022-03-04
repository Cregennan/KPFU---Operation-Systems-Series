using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Vigeneer
{
    public class Service
    {
        public static readonly String[] OperationLabelStates = {
            "Текст для шифрования",
            "Текст для дешифрования"
        };
        public static readonly String[] Operations = {
            "Шифрование",
            "Дешифрование"
        };
        public static readonly String[] AlphabetDescriptions = {
            "Русский",
            "Английский"
        };
        public static readonly String[] ButtonStates = {
            "Зашифровать",
            "Расшифровать"
        };
        public static readonly String[] ResultWindowTitles = { 
            "Результат шифрования",
            "Результат дешифрования"
        };
        public static readonly Regex OnlyNumbers = new Regex("^[0-9]+$");
        public static readonly Regex[] FilterRegices =
        {
             new Regex("[^а-я0-9ё]"),
             new Regex("[^a-z0-9]")
        };
        public static readonly Regex[] ShiftRegices =
        {
             new Regex("[а-я0-9ё]"),
             new Regex("[a-z0-9]")
        };
        public static readonly Regex[] TextAllowed = {
            new Regex(@"^[а-яА-я0-9ёЁ \!\?\n\r.`,\!\?\-\+=()*:;{}—]+$"),
            new Regex(@"^[A-Za-z0-9 \n\r.`,\!\?\-\+=()*:;{}—]+$")
        };



        public static readonly String[] Alphabets =
        {
            "абвгдеёжзийклмнопрстуфхцчшщъыьэюя0123456789",
            "abcdefghijklmnopqrstuvwxyz0123456789",

        };
        public static readonly Regex RussianLetters = new Regex(@"^[а-яё]+$");
        public static readonly Regex EnglishLetters = new Regex(@"^[a-z]+$");

        public static String Encrypt(String text, String shift, String alphabet, bool type)
        {
            return text.Select((c,i) => alphabet[(alphabet.IndexOf(text[i]) + (type ? alphabet.IndexOf(shift[i]) : alphabet.Length - alphabet.IndexOf(shift[i]))  ) % alphabet.Length]).Aggregate("", (x, y) => x + y);
        }


    }
}
