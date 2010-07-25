using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace Rockband_Drum_Kit.Controls
{
   public class BasicHorizontalSlider : Control
   {
       #region Initialize control variables

       private int _minValue = 0;
       private int _maxValue = 100;
       private int _value = 50;
       private bool _dragModeEnabled = false;
       private Point _startDragPoint;
       private const int setheight = 25; //setheight, prevents change
       private bool _ready = false;  //improvised on load event

       private Rectangle KnobRect;
       private Rectangle SlotRect;

       #endregion

       public BasicHorizontalSlider()
       {
           //set user paint style
           this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
           ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor,
           true);

           this.MouseDown += new MouseEventHandler(knob_MouseDown);
           this.MouseMove += new MouseEventHandler(knob_MouseMove);
           this.MouseUp += new MouseEventHandler(knob_MouseUp);
       }

       [Category("Action"), Description("Occurs when the slider is moved")]
       public event EventHandler ValueChanged;

       protected override void OnPaint(PaintEventArgs e)
       {
           //there is no onload event for a control, so this takes its place
           if (!_ready)
           {
               //set up knob rect here
               KnobRect = new Rectangle(this.ClientRectangle.Width / 3, this.ClientRectangle.Height / 2 - 10, 6, 20);

               SlotRect = new Rectangle(6, this.ClientRectangle.Height / 3, this.ClientRectangle.Width - 12, this.ClientRectangle.Height /3);

               this.MoveKnob();
               _ready = true;
           }

           e.Graphics.FillRectangle(Brushes.DarkGray, SlotRect);
           e.Graphics.DrawRectangle(Pens.Black, SlotRect);

           e.Graphics.FillRectangle(Brushes.Gray, KnobRect);
           e.Graphics.DrawRectangle(Pens.Black, KnobRect);
       }

       #region Public Properties
       [Bindable(true), Category("Behavior"),
       DefaultValue(0), Description("The minimum value of the slider")]
       public int Minimum
       {
           get { return this._minValue; }
           set
           {
               this._minValue = value;

               if (this._value < this._minValue)
                   this.Value = this._minValue;
               else
               {
                   this.MoveKnob();
                   this.Invalidate();
               }
           }
       }

       [Bindable(true), Category("Behavior"),
       DefaultValue(0), Description("The value of the slider")]
       public int Value
       {
           get { return this._value; }
           set
           {
               if (value < this._minValue || value > this._maxValue)
               {
                   MessageBox.Show("Value out of bounds");
                   return;
               }

               if (value == this._value)
                   return;

               // int tmp = this._value;
               this._value = value;
               this.MoveKnob();

               if (ValueChanged != null)
                   ValueChanged(this, new EventArgs());

               this.Invalidate();
           }
       }

       [Bindable(true), Category("Behavior"),
       DefaultValue(100), Description("The maximum value of the slider")]
       public int Maximum
       {
           get { return this._maxValue; }
           set
           {
               this._maxValue = value;

               if (this._value > this._maxValue)
                   this.Value = this._maxValue;
               else
               {
                   this.MoveKnob();
                   this.Invalidate();
               }
           }
       }


       public new int Height
       {
           get
           {
               return base.Height;
           }
           set
           {
               base.Height = setheight;
           }
       }


       #endregion

       #region Movement Functions

       int lastClickX = 0;

       private void MoveKnob(int delta)
       {
           // Move the slider and make sure it stays in the bounds of the control  
           if (delta < 0 && (lastClickX + delta) <= SlotRect.X)
               this.KnobRect.X = SlotRect.X;
           else if (delta > 0 && (lastClickX + delta) >= SlotRect.X+SlotRect.Width-KnobRect.Width)
               this.KnobRect.X = SlotRect.X+SlotRect.Width - KnobRect.Width;
           else
               this.KnobRect.X = lastClickX + delta;

           this.Refresh();

           this.CalculateSliderValue();
       }

       private void MoveKnob()
       {
           // distance between min and max values
           int distance = Math.Abs(this._maxValue) - Math.Abs(this._minValue);

           // percentage of control travelled  - subtract min val to put us back at 0 then devide by the total travelable distance to get the percentage of it traveled.
           float percent = (float)(this._value - Math.Abs(_minValue)) / (float)distance;

           // New Knob location  
           this.KnobRect.X = Convert.ToInt32(percent * (float)(SlotRect.Width - this.KnobRect.Width)) + SlotRect.X;

           this.Invalidate();
       }

       private void CalculateSliderValue()
       {
           // distance the knob can travel
           int distance = SlotRect.Width - KnobRect.Width;


           // percentage of control travelled by the knob   
           float percent = (float)(this.KnobRect.X - SlotRect.X) / (float)distance;

           // Slider movement in points  
           int movement = Convert.ToInt32(percent * (float)(Math.Abs(this._maxValue) - Math.Abs(this._minValue)));

           // New value  
           this._value = (this._maxValue >= 0) ? this._minValue + movement : this._minValue - movement;

           // Raise the ValueChanged event  
           if (ValueChanged != null)
               ValueChanged(this, new EventArgs());


       }

       private void knob_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
       {
           if (KnobRect.Contains(e.Location))
           {
               // Set the drag flag for the mousemove event
               this._dragModeEnabled = true;

               // Record the start point for the slider movement
               this._startDragPoint = new Point(e.X, e.Y);

               lastClickX = KnobRect.X;

           }
       }

       private void knob_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
       {
           // User isn't dragging the slider, so dont bother moving it  
           if (this._dragModeEnabled == false)
               return;

           // Calculate the distance the mouse moved  
           int delta = e.X - this._startDragPoint.X;

           if (delta == 0)
               return;

           this.MoveKnob(delta);
       }

       private void knob_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
       {
           // Reset the drag flag
           this._dragModeEnabled = false;
       }

       #endregion
    }
}
