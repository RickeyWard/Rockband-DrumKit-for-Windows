using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Rockband_Drum_Kit
{
   public class MessageDialog : Form
    {

       public MessageDialog()
       {
           this.FormBorderStyle = FormBorderStyle.None;
           this.Size = Properties.Resources.RB_messagedialog.Size;
           this.StartPosition = FormStartPosition.CenterScreen;
           this.TopMost = true;
           this.ShowInTaskbar = false;
           this.MouseDown +=new MouseEventHandler(MainForm_MouseDown);
           this.Paint += new PaintEventHandler(MessageDialog_Paint);

           //initialize components
           // 
           // btnClose
           // 
           MicrosoftStore.IsoTool.ImageButton btnClose = new MicrosoftStore.IsoTool.ImageButton();

           btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
           btnClose.BackColor = System.Drawing.Color.Transparent;
           btnClose.ButtonImage = Properties.Resources.custom_dialog_cosebtn;
           btnClose.ButtonImageOffset = new System.Drawing.Point(0, 0);
           btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
           btnClose.FlatAppearance.BorderSize = 0;
           btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
           btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
           btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
           btnClose.Location = new System.Drawing.Point(370, 5);
           btnClose.Name = "btnClose";
           btnClose.Size = new System.Drawing.Size(25, 25);
           btnClose.TabIndex = 0;
           btnClose.TabStop = false;
           btnClose.UseVisualStyleBackColor = false;
           btnClose.Click += new EventHandler(btnClose_Click);

           this.Controls.Add(btnClose);
       }

       void btnClose_Click(object sender, EventArgs e)
       {
           this.Close();
       }

       void MessageDialog_Paint(object sender, PaintEventArgs e)
       {
           e.Graphics.DrawImage(Properties.Resources.RB_messagedialog, new Point(0, 0));
           e.Graphics.DrawString(this.Text, new Font("Arial", 10f), Brushes.Gray, new PointF(150,10));
       }


       #region regular form stuff

        /// <summary>
        /// Event handler for the mouse click to be able to drag the entire window.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            // --Allow entire form to be dragable//
            // allow form to be movable using the titlebar
            Rectangle dragrect = new Rectangle(0, 0, this.Width, 30);
            if (e.Button == MouseButtons.Left)
            {
                if (dragrect.Contains(e.Location))
                {
                    NativeMethods.ReleaseCapture();
                    NativeMethods.SendMessage(this.Handle, NativeMethods.WM_NCLBUTTONDOWN, new IntPtr(NativeMethods.HT_CAPTION), new IntPtr(0));
                }
            }
        }

        /// <summary>
        /// Native methods for interacting with Win32 methods.
        /// </summary>
        private static class NativeMethods
        {
            public const int PBT_APMQUERYSUSPEND = 0x0;
            public const int BROADCAST_QUERY_DENY = 0x424D5144;
            public const int WM_NCLBUTTONDOWN = 0xA1;
            public const int HT_CAPTION = 0x2;

            public static readonly int QueryCancelAutoPlay = RegisterWindowMessage("QueryCancelAutoPlay");
            public static readonly int WM_POWERBROADCAST = RegisterWindowMessage("WM_POWERBROADCAST");

            /// <summary>
            /// Exectuion state enum for disabling standby.
            /// </summary>
            [Flags]
            public enum EXECUTION_STATE : uint
            {
                ES_AWAYMODE_REQUIRED = 0x00000040,
                ES_CONTINUOUS = 0x80000000,
                ES_DISPLAY_REQUIRED = 0x00000002,
                ES_SYSTEM_REQUIRED = 0x00000001,
                ES_USER_PRESENT = 0x00000004,
            }

            /// <summary>
            /// The send message method for allowing the window to be dragable.
            /// </summary>
            /// <param name="hWnd">The window handle.</param>
            /// <param name="msg">The message to send.</param>
            /// <param name="wParam">The wParam value.</param>
            /// <param name="lParam">The lParam value.</param>
            /// <returns>The hResult of the call.</returns>
            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            /// <summary>
            /// Releases the window caputre.
            /// </summary>
            /// <returns>True on success.</returns>
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ReleaseCapture();

            /// <summary>
            /// Sets the execution state for allowing the tool to disable standby (Vista and higher).
            /// </summary>
            /// <param name="esFlags">The flags indicating the thread execution state.</param>
            /// <returns>The execution state that was set.</returns>
            [DllImport("kernel32.dll")]
            public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

            /// <summary>
            /// Gets the integer value for the given window message.
            /// </summary>
            /// <param name="msgString">The window message to lookup.</param>
            /// <returns>The integer value for the message.</returns>
            [DllImport("user32", CharSet = CharSet.Auto)]
            private static extern int RegisterWindowMessage([In, MarshalAs(UnmanagedType.LPWStr)] string msgString);
        }

        #endregion

        #region Win32

        const int AW_HIDE = 0X10000;
        const int AW_ACTIVATE = 0X20000;
        const int AW_HOR_POSITIVE = 0X1;
        const int AW_HOR_NEGATIVE = 0X2;
        const int AW_SLIDE = 0X40000;
        const int AW_BLEND = 0X80000;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int AnimateWindow
        (IntPtr hwand, int dwTime, int dwFlags);

        #endregion

        #region Variables

        public enum AnimateWindowFlags
        {
            AW_HOR_POSITIVE = 0x00000001,
            AW_HOR_NEGATIVE = 0x00000002,
            AW_VER_POSITIVE = 0x00000004,
            AW_VER_NEGATIVE = 0x00000008,
            AW_CENTER = 0x00000010,
            AW_HIDE = 0x00010000,
            AW_ACTIVATE = 0x00020000,
            AW_SLIDE = 0x00040000,
            AW_BLEND = 0x00080000
        }

        public enum AnimateWindowDirections
        {
            Right = 0x00000001,
            Left = 0x00000002,
            Down = 0x00000004,
            Up = 0x00000008,
            Center = 0x00000010
        }

        #endregion

        #region Overrides

        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
            AnimateWindow(this.Handle, 175, (int)AnimateWindowFlags.AW_BLEND | (int)AnimateWindowFlags.AW_ACTIVATE);


        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnLoad(e);
            AnimateWindow(this.Handle, 175, (int)AnimateWindowFlags.AW_BLEND | (int)AnimateWindowFlags.AW_HIDE);


        }

        #endregion

        
    }
}
