using GUI_Starter.classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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


namespace GUI_Starter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            race_list = new List<Race>();
            Com_Ports port = Com_Ports.Instance;
            port.DataReady += port_DataCallback;
        }

        private List<Race> race_list;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Hello World");
        }

        private void createFromFile(String FilePath)
        {
            using( StreamReader reader = new StreamReader(FilePath))
            {
                GridView gridView = (GridView) RaceList.View; // cast is needed
                RaceList.Items.Clear();// remove all old items
                while (!reader.EndOfStream)
                {
                    String line = reader.ReadLine();
                    race_list.Add(new Race(line));
                    RaceList.Items.Add(race_list.Last());
                }
                for(int i = 1; i<= 4; i++)
                {
                    GridViewColumn col = new GridViewColumn();
                    GridViewColumn delayCol = new GridViewColumn();
                    col.Header = "Bahn " + i;
                    col.DisplayMemberBinding = new Binding("Lanes[" + (i - 1) + "].LaneNumber");
                    gridView.Columns.Add(col);
                    delayCol.Header = "Zeit";
                    delayCol.DisplayMemberBinding = new Binding("Lanes[" + (i - 1) + "].delay");
                    gridView.Columns.Add(delayCol);
                }
                // maybe to some Config in Lanes here
                //RaceList.ItemsSource = race_list;

            }
        }
        private void Port_Seclect_Click(object sender, RoutedEventArgs e)
        {
            Com_Ports Bringe = Com_Ports.Instance;
            PortPopup dialog = new PortPopup();
            bool? result = dialog.ShowDialog();
            try { 
                if (result.Value == true)
                {
                    String sets = (String) dialog.cb_ports.SelectedValue;
                    Console.WriteLine(sets);
                }
                else
                {
                    Console.WriteLine("Select Window Coled without Action");
                }
            }catch(InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void onOpenClick(object sender, RoutedEventArgs e)
        {

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = " Comma-separated values|*.csv;";
            fileDialog.CheckFileExists = true;
            fileDialog.Multiselect = false;
            fileDialog.DereferenceLinks = true;
            if(fileDialog.ShowDialog() == true)
            {
                this.createFromFile(fileDialog.FileName);
            }

        }

        public void port_DataCallback(object sender, EventArgs e)
        {
            /* TODO: make bussines Logic for dispaly success */
            Console.WriteLine("Some Data Recieved from the Comport");
        }

        private void  onSendClick(object sender, RoutedEventArgs e)
        {
            String SendText = "";
            if (RaceList.SelectedItems.Count <= 0)
            {
                SendText = this.Tb_SendString.Text;
            }
            else
            {
                try
                {
                    Race item = (Race)RaceList.SelectedItem;
                    item.sendSerial();
                    return;
                }catch(InvalidCastException ex)
                {
                    Console.WriteLine(ex.Message);
                }                
            }
            Com_Ports Port = Com_Ports.Instance;
            Port.sendString(SendText);
            Console.WriteLine("btn_send Clicked");
        }
    }
}
