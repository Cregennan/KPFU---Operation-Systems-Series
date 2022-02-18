using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Caesar_Cypher
{
    public class Service
    {
        public static readonly String[] OperationLabelStates = {
            "Текст для шифрования (регистр не учитывается)",
            "Текст для дешифрования (регистр не учитывается)"
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
        public static readonly Regex[] TextAllowed = {
            new Regex(@"^[a-zA-Z0-9 \n\r.,-]+$"),
            new Regex(@"^[а-яА-Я0-9 \n\r.`,-]+$")
    };


        public static readonly String[] Alphabets =
        {
            "абвгдеёжзийклмнопрстуфхцчшщъыьэюя0123456789",
            "abcdefghijklmnopqrstuvwxyz0123456789"
        };
        public static readonly Regex RussianLetters = new Regex(@"^[а-яё]+$");
        public static readonly Regex EnglishLetters = new Regex(@"^[a-z]+$");   

        public static String Encrypt(String text, int shift, String alphabet)
        {
            return text.Select(c => alphabet[(alphabet.IndexOf(c) + shift) % alphabet.Length]).Aggregate("",(x,y) => x + y);

        }

        public static bool CheckBothAlphabets(String text)
        {
            bool ru = false;
            bool en = false;
            foreach (var l in text)
            {
                if (!ru && l >= 'а' && l <= 'я')
                {
                    ru = true;
                }
                if (!en && l >= 'a' && l <= 'z')
                {
                    en = true;
                }
                if (en && ru)
                {

                    return false;
                }
            }
            return true;
        }


    }
}
