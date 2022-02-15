using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        public static readonly Regex[] AlphabetRegices =
        {
             new Regex("[^а-я0-9]"),
             new Regex("[^a-z0-9]")
        };
        public static readonly String[] Alphabets =
        {
            "абвгдеёжзийклмнопрстуфхцчшщъыьэюя0123456789",
            "abcdefghijklmnopqrstuvwxyz0123456789"
        };
        public static readonly Regex RussianLetters = new Regex(@"^[а-я]+$");
        public static readonly Regex EnglishLetters = new Regex(@"^[a-z]+$");   



    }
}
