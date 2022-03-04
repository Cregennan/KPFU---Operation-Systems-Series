using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Gamma
{
    public static class Service
    {
        public static class Colors{

            public static Brush GetColor(String hex) => new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));

            public static readonly Brush Transparent = GetColor("#00000000");
            public static readonly Brush Error = GetColor("#44AB1F1F");
            public static readonly Brush Success = GetColor("#AA2A9927");
            public static readonly Brush Light = GetColor("#AAFFFFFF");
            public static readonly Brush Dark = GetColor("#FF111111");
            public static readonly Brush Basic = GetColor("#88FFFFFF");

        }


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

        public static readonly String[] KeyTypeDescriptions = {
            "Ручной",
            "Псевдослучайная гамма"
        };
        public static readonly Regex OnlyNumbers = new Regex("^[0-9]+$");
        public static readonly Regex[] FilterRegices =
        {
             new Regex("[^а-я0-9ё] "),
             new Regex("[^a-z0-9] ")
        };
        public static readonly Regex[] TextAllowed = {
            new Regex(@"^[A-Za-z0-9 \n\r.`,\!\?\-\+=()*:;{}—]+$"),
            new Regex(@"^[а-яА-я0-9ёЁ \!\?\n\r.`,\!\?\-\+=()*:;{}—]+$")
        };


        public static readonly String[] Alphabets =
        {
            "абвгдеёжзийклмнопрстуфхцчшщъыьэюя0123456789 ",
            "abcdefghijklmnopqrstuvwxyz0123456789 "
        };
        public static int[] DecodeAlphabet(this String text, int alphabet) => text.Select(x => Alphabets[alphabet].IndexOf(x)).ToArray();

        public static int[] DecodeKey(this String text) => Regex.Matches(text, @".{1,8}").Cast<Match>().Select(x => Convert.ToInt32(x.Value, 2)).ToArray();


        //public static String Filter(this String text, int alphabet) => FilterRegices[alphabet].Replace(Regex.Replace(text.ToLower(), @"\s+", ""), String.Empty);
        public static String Filter(this String text, int alphabet) => FilterRegices[alphabet].Replace(text.ToLower().Replace("\n", "").Replace("\r", ""), String.Empty);

        public static String GenerateRandomGamma(int l)
        {
            Random random = new Random();

            char[] result = new string('0', l).ToCharArray();
            int c = 0;
            while (c < l / 2)
            {
                int pos = random.Next(0, l );
                if (result[pos] == '0')
                {
                    result[pos] = '1';
                    c++;
                }
            }
            return result.Aggregate("", (x,y) => x + y);
        }

        public static String ToBinary(this byte number) => Convert.ToString(number, 2).PadLeft(8, '0');
  
        public static String[] ToBinary(this byte[] array) => array.Select(x => x.ToBinary()).ToArray();

        public static byte[] ToBytes(this int[] array) => array.Select(x => (byte)x).ToArray();

        public static String ToBinaryString(this int[] array) => array.ToBytes().ToArray().ToBinary().Aggregate("", (x, y) => x + y);

        public static int[] Xor(this int[] one, int[] two) => one.Select((item, index) => one[index] ^ two[index]).ToArray();

        public static String ConvertToText(this int[] one, int alphabet) => one.Select(x => Alphabets[alphabet][x]).Aggregate("", (x, y) => x + "" + y);




        public static readonly Regex RussianLetters = new Regex(@"^[а-яё]+$");
        public static readonly Regex EnglishLetters = new Regex(@"^[a-z]+$");

      
    }
}
