using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rockband_Drum_Kit
{

public class TransparentPanel : Panel

{

public TransparentPanel()

{
    this.Paint += new PaintEventHandler(TransparentPanel_Paint);
}

void TransparentPanel_Paint(object sender, PaintEventArgs e)
{
    e.Graphics.DrawImage(Rockband_Drum_Kit.Properties.Resources.Drumset, new System.Drawing.Point(0,0));
}

//This method makes sure that the background is what is directly behind the control

//and not what is behind the form...



protected override CreateParams CreateParams

{

get

{

CreateParams cp = base.CreateParams;

cp.ExStyle |= 0x20;

return cp;

}

}

override protected void OnPaintBackground(PaintEventArgs e)

{

// do nothing

}

}

}
