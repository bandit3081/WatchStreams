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
using System.Collections;

namespace WatchStreams
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ArrayList Streamerlist = new ArrayList();

        public MainWindow()
        {
            InitializeComponent();
            loadList();
        }

        public void OpenCMD()
        {
            string streamid = streamerlistbox.SelectedItem.ToString();
            string strCmdText;
            strCmdText = "livestreamer http://www.twitch.tv/" + streamid + " " + qualityBox.Text;
            //System.Diagnostics.Process.Start("CMD.exe", strCmdText);

            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + strCmdText);

            // The following commands are needed to redirect the standard output.
            // This means that it will be redirected to the Process.StandardOutput StreamReader.
            procStartInfo.RedirectStandardOutput = false;
            procStartInfo.UseShellExecute = false;
            // Do not create the black window.
            if (consoleCheck.IsChecked == true)
            {
                procStartInfo.CreateNoWindow = false;
            }
            else
            {
                procStartInfo.CreateNoWindow = true;
            }
            // Now we create a process, assign its ProcessStartInfo and start it
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            if (streamerlistbox.SelectedItem.ToString() != "")
            {
                printToConsoleBox("Starting Stream...");
                printToConsoleBox("(if streamer is not live, stream will not start...)");
                proc.Start();
            }
            
        }

        private void OpenStream(object sender, RoutedEventArgs e)
        {
            OpenCMD();
        }

        private void saveList()
        {
            try
            {
                //Save listBox Items           
                //Properties.Settings.Default.StreamerList.Clear();
                Streamerlist.Clear();
                foreach (object item in streamerlistbox.Items)
                {
                    Streamerlist.Add(item);
                }
                printToConsoleBox("Saving list...");
                Properties.Settings.Default.StreamerList = Streamerlist;

                Properties.Settings.Default.Quality = qualityBox.SelectedIndex;

                Properties.Settings.Default.Save();
            }
            catch (Exception e)
            {
                printToConsoleBox(e.ToString());

            }
        }
        private void loadList()
        {
            try
            {
                if (Properties.Settings.Default.StreamerList.Count == 0)
                {
                    printToConsoleBox("Preparing list...");
                    printToConsoleBox("Will save list on exit!");
                }
                else
                {
                    //Restore listBox Items
                    streamerlistbox.Items.Clear();
                    foreach (object item in Properties.Settings.Default.StreamerList)
                    {
                        streamerlistbox.Items.Add(item);
                    }
                    printToConsoleBox("Loading list...");
                }
            }
            catch (Exception e)
            {
                printToConsoleBox(e.ToString());
            }

            qualityBox.SelectedIndex = Properties.Settings.Default.Quality;
        }

        private void addToList(object sender, RoutedEventArgs e)
        {
            string streamertxt = streamerID.Text;
            streamerlistbox.Items.Add(streamertxt);
            printToConsoleBox("Added *" + streamertxt + "* to list");
            streamerID.Text = "";
        }

        private void removeFromList(object sender, RoutedEventArgs e)
        {
            string streamertxt1 = streamerlistbox.SelectedItem.ToString();
            streamerlistbox.Items.Remove(streamerlistbox.SelectedItem);
            printToConsoleBox("Removed *" + streamertxt1 + "* from list");
        }


        public void check()
        {
            foreach (object item in streamerlistbox.Items)
            {
                printToConsoleBox(item.ToString());
            }
        }


        public void printToConsoleBox(string txt)
        {
            consoleBox.AppendText(txt + "\n");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            saveList();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //check();
            //saveList();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            loadList();
        }

        private void openChatBtn_Click(object sender, RoutedEventArgs e)
        {
            Chat chatwindow = new Chat();
            chatwindow.Show();
        }

        private void streamerlistbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;

            while (obj != null && obj != streamerlistbox)
            {
                if (obj.GetType() == typeof(ListBoxItem))
                {
                    OpenCMD();
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }
    }
}
