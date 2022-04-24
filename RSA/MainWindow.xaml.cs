using Cregennan.Core;
using Cregennan.Core.Exceptions;
using Cregennan.Cryptography.Algorithms.RSA;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;



namespace RSA
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            keyLength.PreviewTextInput += Cregennan.WPF.EventHandlers.Uint_PreviewTextInput;
            DataObject.AddPastingHandler(keyLength, Cregennan.WPF.EventHandlers.UInt_Pasting);
            keyLength.PreviewKeyDown += Cregennan.WPF.EventHandlers.Deny_Space;
        }

        private void generatePair_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                (var pub, var priv) = RSACryptoService.GenerateKeyPair(Int32.Parse(keyLength.Text));
                
                Debug.WriteLine("Данные открытого  ключа:");
                Debug.WriteLine("\tЧисло N: " + pub.KeyPair.Item1);
                Debug.WriteLine("\tОткрытая экспонента: " + pub.KeyPair.Item2  + '\n');
                Debug.WriteLine("Данные закрытого ключа: ");
                Debug.WriteLine("\tЧисло N: " + priv.KeyPair.Item1);
                Debug.WriteLine("\tЗакрытая экспонента: " + priv.KeyPair.Item2 + '\n');

                privateKey.Text = priv.Encode();
                publicKey.Text = pub.Encode();
            }
            catch(RSAInvalidKeyLengthException ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButton.OK);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Ошибка в данных", "Ошибка", MessageBoxButton.OK);
                return;
            }

        }

        private void encrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RSAPublicKey pub = RSAKey.TryFromEncoded<RSAPublicKey>(publicKey.Text);

                if(keyLength.Text.Length == 0)
                {
                    keyLength.Text = "10";
                }

                int keyL = Int32.Parse(keyLength.Text);

                if (keyL < 10) {
                    keyLength.Text = "10";
                    keyL = 10;
                }

                List<byte[]> messages = originalText.Text.Split(keyL / 4).Select(x => x.Indexes(Cregennan.Core.Definitions.GlobalAlphabet).ToArray()).ToList();

                string[] encryptedChunks = messages.Select(x => RSACryptoService.Encrypt(x, pub)).Select(x => Convert.ToBase64String(x)).ToArray();  

                encryptedText.Text = String.Join("%", encryptedChunks);

            }
            catch (RSAInvalidKeyException ex)
            {
                MessageBox.Show(this, "Невалидный открытый ключ", "Ошибка");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Ошибка в данных" + (ex.Message.Length > 0? ": "+ex.Message : ""), "Ошибка");
                return;
            }
        }

        private void decrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                RSAPrivateKey priv = RSAKey.TryFromEncoded<RSAPrivateKey>(privateKey.Text);

                string[] chunks = !encryptedText.Text.Contains("%") ? new string[] { encryptedText.Text } : encryptedText.Text.Split(new char[] { '%' } );

                IEnumerable<byte[]> crypted = chunks.Select(x => Convert.FromBase64String(x));

                IEnumerable<byte[]> decrypted = crypted.Select(x => RSACryptoService.Decrypt(x, priv).ToArray());

                IEnumerable<string> textchunks = decrypted.Select(x => new string(x.FromIndexes(Cregennan.Core.Definitions.GlobalAlphabet).ToArray()));

                decryptedText.Text = String.Join(String.Empty, textchunks);
            }
            catch (RSAInvalidKeyException ex)
            {
                MessageBox.Show(this, "Невалидный закрытый ключ", "Ошибка");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Ошибка в данных", "Ошибка");
                return;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TestWindow w = new TestWindow();
            w.ShowDialog();

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                (var pub, var priv) = RSACryptoService.GenerateKeyPairAsync(Int32.Parse(keyLength.Text));

                Debug.WriteLine("Данные открытого  ключа:");
                Debug.WriteLine("\tЧисло N: " + pub.KeyPair.Item1);
                Debug.WriteLine("\tОткрытая экспонента: " + pub.KeyPair.Item2 + '\n');
                Debug.WriteLine("Данные закрытого ключа: ");
                Debug.WriteLine("\tЧисло N: " + priv.KeyPair.Item1);
                Debug.WriteLine("\tЗакрытая экспонента: " + priv.KeyPair.Item2 + '\n');

                privateKey.Text = priv.Encode();
                publicKey.Text = pub.Encode();
            }
            catch (RSAInvalidKeyLengthException ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButton.OK);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Ошибка в данных", "Ошибка", MessageBoxButton.OK);
                return;
            }
        }
    }
}
