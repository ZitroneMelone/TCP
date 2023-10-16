using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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

        SimpleTcpServer server;

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            server.Start();
            txtInfo.Text += $"Startung...{Environment.NewLine}";
            btnStart.IsEnabled = false;
            btnSend.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnSend.IsEnabled = false;
            server = new SimpleTcpServer(txtIP.Text);
            server.Events.ClientConnected += Events_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived;
        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            byte[] bytes = new byte[e.Data.Count];
            Array.Copy(e.Data.Array, e.Data.Offset, bytes, 0, e.Data.Count);

            Dispatcher.Invoke(() =>
            {
                txtInfo.Text += $"Server: {Encoding.UTF8.GetString(e.Data.Array, e.Data.Offset, e.Data.Count)}{Environment.NewLine}";
            });
        }

        private void Events_ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                lstClientIP.Items.Remove(e.IpPort);
            });
        }

        private void Events_ClientConnected(object sender, ConnectionEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                txtInfo.Text += $"Server connected.{Environment.NewLine}";
            });
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Add a handler to the Loaded event
            AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(Window_Loaded));
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if(server.IsListening)
            {
                if(!string.IsNullOrEmpty(txtMessage.Text) && lstClientIP.SelectedItems != null) {
                    server.Send(lstClientIP.SelectedItems.ToString(), txtMessage.Text);
                    txtInfo.Text += $"Server: {txtMessage.Text}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
            }
        }
    }
}
