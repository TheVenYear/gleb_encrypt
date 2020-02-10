using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gleb_encrypt
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            var enc = new Encrypter();
            output.Text = enc.Encrypt(input.Text, out var key);
            outputVars.Text = key;
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            var enc = new Encrypter();
            input.Text = enc.Decrypt(output.Text, outputVars.Text);
        }
    }
}
