using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DetectHIDDeviceConnect
{
   public class JoystickConnectNotification
    {
       public enum JoyConEventType
        {
            Connected,
            Disconnected
        }

       WindowWatcher ww;
       Form FMain;
        public JoystickConnectNotification(Form Fmain)
        {
            FMain = Fmain;
            ww = new WindowWatcher();
            ww.AssignHandle(Fmain.Handle);
            ww.JoyStickNotifier = this;
            RegisterHidNotification();
        }

        void RegisterHidNotification()
        {
            Win32.DEV_BROADCAST_DEVICEINTERFACE dbi = new
            Win32.DEV_BROADCAST_DEVICEINTERFACE();
            int size = Marshal.SizeOf(dbi);
            dbi.dbcc_size = size;
            dbi.dbcc_devicetype = Win32.DBT_DEVTYP_DEVICEINTERFACE;
            dbi.dbcc_reserved = 0;
            dbi.dbcc_classguid = Win32.GUID_DEVINTERFACE_HID;
            dbi.dbcc_name = 0;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(dbi, buffer, true);
            IntPtr r = Win32.RegisterDeviceNotification(FMain.Handle, buffer,
            Win32.DEVICE_NOTIFY_WINDOW_HANDLE);
            if (r == IntPtr.Zero)
                MessageBox.Show(Win32.GetLastError().ToString());
        }

        //create event for hotkeyevent
        public delegate void JoyConnectDel(JoyConnectedArgs ConArgs);

        public event JoyConnectDel OnJoystickConnected;
        //call OnHotKey(this, EventArgs.Empty); //to throw event. have his called by native window class
        public void throwOnJoystickConnected(JoyConnectedArgs e)
        {
            if (OnJoystickConnected != null)
            {
                OnJoystickConnected(e);
            }
        }
    }

    class WindowWatcher : NativeWindow
    {

        private JoystickConnectNotification joystickNotification;

        public JoystickConnectNotification JoyStickNotifier
        {
            get
            {
                return joystickNotification;
            }
            set
            {
                joystickNotification = value;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Win32.WM_DEVICECHANGE: OnDeviceChange(ref m); break;
            }
            base.WndProc(ref m);
        }

        void OnDeviceChange(ref Message msg)
        {
            int wParam = (int)msg.WParam;
            if (wParam == Win32.DBT_DEVICEARRIVAL) joystickNotification.throwOnJoystickConnected(new JoyConnectedArgs(JoystickConnectNotification.JoyConEventType.Connected));
            else if (wParam == Win32.DBT_DEVICEREMOVECOMPLETE) joystickNotification.throwOnJoystickConnected(new JoyConnectedArgs(JoystickConnectNotification.JoyConEventType.Disconnected));
        }
    }

    public class JoyConnectedArgs : EventArgs
    {
        private JoystickConnectNotification.JoyConEventType conType;

        public JoyConnectedArgs(JoystickConnectNotification.JoyConEventType ConnectType)
        {
            conType = ConnectType;
        }

        public JoystickConnectNotification.JoyConEventType ConnectType
        {
            get
            {
                return conType;
            }
        }

    }

    class Win32
    {
        public const int
        WM_DEVICECHANGE = 0x0219;
        public const int
        DBT_DEVICEARRIVAL = 0x8000,
        DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const int
        DEVICE_NOTIFY_WINDOW_HANDLE = 0,
        DEVICE_NOTIFY_SERVICE_HANDLE = 1;
        public const int
        DBT_DEVTYP_DEVICEINTERFACE = 5;
        public static Guid
        GUID_DEVINTERFACE_HID = new
        Guid("4D1E55B2-F16F-11CF-88CB-001111000030");

        [StructLayout(LayoutKind.Sequential)]
        public class DEV_BROADCAST_DEVICEINTERFACE
        {
            public int dbcc_size;
            public int dbcc_devicetype;
            public int dbcc_reserved;
            public Guid dbcc_classguid;
            public short dbcc_name;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr RegisterDeviceNotification(
        IntPtr hRecipient,
        IntPtr NotificationFilter,
        Int32 Flags);

        [DllImport("kernel32.dll")]
        public static extern int GetLastError();
    }
}
