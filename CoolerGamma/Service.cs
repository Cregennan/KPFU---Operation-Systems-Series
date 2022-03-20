using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;


namespace CoolerGamma
{
    public static class Service
    {
        public enum KeyTypes
        {
            AUTO = 0,
            BINARY = 1,
            HEX = 2,
            UNDEFINED = 3,
        }

        
        public static String Cypher(String text, String key, KeyTypes type)
        {
            byte[] keybytes = Service.ConvertKeyTo(key, type);
            byte[] textbytes = text.ConvertToBytes();
            Debug.WriteLine(textbytes.ToHexString());
            Debug.WriteLine(textbytes.Length);
            Debug.WriteLine(keybytes.ToHexString());
            byte[] result = new byte[textbytes.Length];

            for (int i = 0; i < textbytes.Length; i++)
            {
                result[i] = (byte)((int)textbytes[i] ^ (int)keybytes[i]);
            }
            return result.ToHexString();
        }
        public static String Decypher(String text, String key, KeyTypes type)
        {
            byte[] keybytes = Service.ConvertKeyTo(key, type);
            byte[] textbytes = text.ConvertKeyFromHex();
            Debug.WriteLine(textbytes.ToHexString());
            Debug.WriteLine(textbytes.Length);
            Debug.WriteLine(keybytes.ToHexString());
            String result = "";
            for (int i = 0; i < textbytes.Length; i++)
            {
                result += (char)(byte)((int)textbytes[i] ^ (int)keybytes[i]);
            }
            return result;
        }
        public static Service.KeyTypes DetectKeyType(String message, String key, Service.KeyTypes type)
        {
            if (key.Length == 0)
            {
                return Service.KeyTypes.UNDEFINED;
            }
            switch (type)
            {
                case Service.KeyTypes.AUTO:
                    if (isValueBinary(key))
                    {
                        if (key.Length == message.Length * 2)
                        {
                            return Service.KeyTypes.HEX;
                        }
                        else
                        {
                            return Service.KeyTypes.BINARY;
                        }
                    }
                    else if (isValueHex(key))
                    {
                        return Service.KeyTypes.HEX;
                    }
                    return Service.KeyTypes.UNDEFINED;

                default:
                    return type;
            }
        }
        public static bool isValueBinary(String t)
        {
            foreach (char c in t)
            {
                if (c != '0' && c != '1')
                {
                    return false;
                }

            }
            return true;

        }

        public static bool isValueHex(String t) => Service.HEXRegex.IsMatch(t);

        private static byte[] ConvertKeyFromBinary(this String t)
        {
            var bytesAsStrings =
                t.Select((c, i) => new { Char = c, Index = i })
                     .GroupBy(x => x.Index / 8)
                     .Select(g => new string(g.Select(x => x.Char).ToArray()));
            return bytesAsStrings.Select(s => Convert.ToByte(s, 2)).ToArray();
        }
        private static byte[] ConvertKeyFromHex(this String hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("Ключ не может иметь нечетное количество знаков");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }
        private static int GetHexVal(char hex) => (int)hex - ((int)hex < 58 ? 48 : ((int)hex < 97 ? 55 : 87));

        public static byte[] ConvertKeyTo(String t, KeyTypes type)
        {
            switch (type)
            {
                case KeyTypes.BINARY:
                    return ConvertKeyFromBinary(t);
                case KeyTypes.HEX:
                    return ConvertKeyFromHex(t);
                default:
                    throw new Exception("Неверный тип ключа");
            }
        }
        public static byte[] ConvertToBytes(this String t)
        {
            byte[] s = new byte[t.Length];
            for (int i = 0; i < t.Length; i++)
            {
                s[i] = (byte)(int)t[i];
            }
            return s;

        }
        public static int[] ConvertToInts(this String t)
        {
            int[] s = new int[t.Length];
            for (int i = 0; i < t.Length; i++)
            {
                s[i] = (int)t[i];

            }
            return s;
        }
        public static String ToHexString(this byte[] t) => BitConverter.ToString(t).Replace("-", "");

        public static String GenerateKey(String text, KeyTypes type)
        {
            String result = "";
            Random r = new Random();
            if (text.Length == 0)
            {
                return "";
            }
            switch (type)
            {
                case KeyTypes.BINARY:
                    for (int i = 0; i < text.Length * 8; i++)
                    {
                        result += r.Next(2);
                    }
                    return result;
                default:
                    byte[] vs = new byte[text.Length];
                    for (int i = 0; i < text.Length; i++)
                    {
                        vs[i] = (byte)r.Next(256);
                    }

                    return vs.ToHexString();
            }
        }

        public class Colors
        {
            public static readonly SolidColorBrush Transparent = GetColor("#00000000");
            public static readonly SolidColorBrush DefaultDetectedKeyType = GetColor("#FF7D7878");
        }

        public static readonly String[] OperationLabelStates = {
            "Текст для шифрования",
            "Гамма"
        };
        public static readonly String[] Operations = {
            "Шифрование",
            "Дешифрование"
        };
        public static readonly String[] ButtonStates = {
            "Зашифровать",
            "Расшифровать"
        };
        public static readonly String[] ResultWindowTitles = {
            "Гамма",
            "Результат дешифрования"
        };

        public static readonly String[] KeyTypeDescriptions = {
            "Автоматически",
            "Двоичный",
            "Шеснадцатеричный"
        };
        public static readonly String[] KeyTypeLabelValues =
        {
            "Определено: двоичный",
            "Определено: шестандцатеричный"
        };


        public static readonly Regex OnlyNumbers = new Regex("^[0-9]+$");


        public static readonly Regex BinaryFilter = new Regex(@"[^0-1]+$");
        public static readonly Regex BinaryRegex = new Regex(@"[0-1]+$");
        public static readonly Regex HEXRegex = new Regex(@"[0-9A-Fa-f]+$");

        public static SolidColorBrush GetColor(String hex)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFrom(hex);
        }


    }
}
