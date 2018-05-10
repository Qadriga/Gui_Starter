using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI_Starter.popups
{
    /// <summary>
    /// Interaktionslogik für NewPort.xaml
    /// </summary>
    public partial class NewPort : Window
    {
        public NewPort()
        {
            InitializeComponent();
        }
        
        public void SetMessageText(String Message)
        {
            this.InfoBlock.Text = Message;
        }
        private void onBtn_Oklick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
