using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Application = System.Windows.Application;


namespace TarkovMap.util;

public class ShortCutHandler
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
    
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    
    public delegate void OnPressSc(Window window, NotifyIcon notifyIcon);
    
    private Window window;
    private NotifyIcon _notifyIcon;
    private OnPressSc _onPressSc;

    public ShortCutHandler(Window window, OnPressSc onPressSc)
    {
        this.window = window;
        _onPressSc = onPressSc;
        InitializeNotifyIcon();
    }
    
    private void InitializeNotifyIcon()
    {
        Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/assets/icon/tarkovmap.ico"))?.Stream;
        
        _notifyIcon = new NotifyIcon()
        {
            Icon = new Icon(iconStream),
            Visible = false,
            Text = "Tarkov Map"
        };
        _notifyIcon.DoubleClick += (sender, args) =>
        {
            window.Show();
            window.WindowState = WindowState.Normal;
            _notifyIcon.Visible = false;
        };

        ContextMenuStrip contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Restore", null, (sender, args) =>
        {
            window.Show();
            window.WindowState = WindowState.Normal;
            _notifyIcon.Visible = false;
        });
        contextMenu.Items.Add("Exit", null, (sender, args) =>
        {
            _notifyIcon.Visible = false;
            Application.Current.Shutdown();
        });
        _notifyIcon.ContextMenuStrip = contextMenu;
    }
    
    public void ShortCutRegister()
    {
        var helper = new WindowInteropHelper(window);
        IntPtr hWnd = helper.Handle;
        RegisterHotKey(hWnd, Constants.HOTKEY_ID_COMMON1, Constants.MOD_SHIFT, Constants.VK_F1);
        HwndSource source = HwndSource.FromHwnd(hWnd);
        source.AddHook(HwndHook);
    }
    
    private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        const int WM_HOTKEY = 0x0312;
        if (msg == WM_HOTKEY)
        {
            int id = wParam.ToInt32();
            if (id == Constants.HOTKEY_ID_COMMON1)
            {
                
                _onPressSc(window, _notifyIcon);
            }
        }
        return IntPtr.Zero;
    }

    public void Close()
    {
        var helper = new WindowInteropHelper(window);
        UnregisterHotKey(helper.Handle, Constants.HOTKEY_ID_COMMON1);
    }
}