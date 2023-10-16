using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace TCP
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        SimpleTcpClient client = new SimpleTcpClient("192.168.178.46:7891");

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            client.Events.Connected += Events_Connected;
            client.Events.DataReceived += Events_DataReceived;
            client.Events.Disconnected += Events_Disconnected;
            btnSend.IsEnabled = false;
 
        }

        private void Events_Connected(object sender, ConnectionEventArgs e)
        {
            txtInfo.Text += $"Server connected.{Environment.NewLine}";
        }

        private void Events_Disconnected(object sender, ConnectionEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                txtInfo.Text += $"Server disconnected.{Environment.NewLine}";
            });
        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            txtInfo.Text += $"Server connected.{Environment.NewLine}";
         }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Add a handler to the Loaded event
            AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(Window_Loaded));
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.Connect();
                btnSend.IsEnabled=true;
                btnConnect.IsEnabled=false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (client.IsConnected)
            {
                if(!string.IsNullOrEmpty(txtMessage.Text)) 
                { 
                    client.Send(txtMessage.Text);
                    txtInfo.Text += $"Me: {txtMessage.Text}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
            }
        }
    }
}
