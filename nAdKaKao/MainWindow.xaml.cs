using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace nAdKaKao
{
    public partial class MainWindow : Window
    {
        public TaskbarIcon notifyIcon = new TaskbarIcon();
        private readonly string[] WINDOW_STRING_KAKOTALK = { "카카오톡", "KakaoTalk" };
        private const string CLASS_NAME_VIEWER = "EVA_ChildWindow";
        private const string CLASS_NAME_AD = "EVA_Window";
        private const string CLASS_NAME_ADPOPUP = "FAKE_WND_REACHPOP";

        private int HANDLE_WND, HANDLE_AD, HANDLE_ADP, HANDLE_VW;

        private WinAPI.RECT wndRect = new WinAPI.RECT();

        public MainWindow()
        {
            InitializeComponent();
            notifyIcon.Icon = Properties.Resources.Icon;
            notifyIcon.TrayMouseDoubleClick += NotifyIcon_TrayMouseDoubleClick;
            BackgroundTask();
        }

        private async void BackgroundTask()
        {
            try
            {
                await Task.Run(() =>
                {
                    while(true)
                    {
                        System.Threading.Thread.Sleep(100);
                        FindHandles();

                        if ((HANDLE_WND != 0) & (HANDLE_VW != 0))
                        {
                            if (RemoveAd())
                                FitVwWindow();
                        }
                        if (HANDLE_ADP != 0)
                            CollapseAdp();
                    }
                });
            }
            catch (Exception) { }
        }

        private void FindHandles()
        {
            foreach(string str in WINDOW_STRING_KAKOTALK)
            {
                int id = WinAPI.FindWindow(null, str);

                if (id != 0)
                    HANDLE_WND = id;
            }
            HANDLE_AD = WinAPI.FindWindowEx(HANDLE_WND, 0, CLASS_NAME_AD, null);
            HANDLE_ADP = WinAPI.FindWindowEx(0, 0, CLASS_NAME_ADPOPUP, null);
            HANDLE_VW = WinAPI.FindWindowEx(HANDLE_WND, 0, CLASS_NAME_VIEWER, null);
        }

        private bool RemoveAd()
        {
            return WinAPI.SetWindowPos(HANDLE_AD, 0, 0, 0, 0, 0, 0x0080);
        }

        private bool CollapseAdp()
        {
            return WinAPI.SetWindowPos(HANDLE_ADP, 0, 0, 0, 0, 0, 0x0080);
        }

        private bool FitVwWindow()
        {
            WinAPI.GetClientRect(HANDLE_WND, out wndRect);

            return WinAPI.SetWindowPos(HANDLE_VW, 0, 0, 0, wndRect.Right - 2, wndRect.Bottom - 30, 0x0002);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void NotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Visible;
            notifyIcon.Visibility = Visibility.Hidden;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            notifyIcon.Visibility = Visibility.Visible;
        }
    }
}
