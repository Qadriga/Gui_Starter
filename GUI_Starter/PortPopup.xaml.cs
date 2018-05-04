using GUI_Starter.classes;
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
using System.Windows.Shapes;

namespace GUI_Starter
{
    /// <summary>
    /// Interaktionslogik für PortPopup.xaml
    /// </summary>
    public partial class PortPopup : Window
    {
        public PortPopup()
        {
            InitializeComponent();
            Com_Ports ports = Com_Ports.Instance;
            cb_ports.Items.Clear(); // clear the list if exists something
            foreach(String port in ports.Port_names)
            {
                cb_ports.Items.Add(port);
            }
            cb_ports.SelectedIndex = ports.Port_names.ToList().IndexOf(ports.portname);
            
        }
        private void Select_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
        private void Cancle_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        
    }
}
