#region Using

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.VisualBasic;
using System.Data;
using System.Diagnostics;

#endregion

#region WC_Blue
#region  Button

public class WC_Blue_Button : Control
{

    #region  Variables

    private int MouseState;
    private bool Enable;
    private GraphicsPath Shape;
    private LinearGradientBrush InactiveGB;
    private LinearGradientBrush HoverGB;
    private LinearGradientBrush PressedGB;
    private LinearGradientBrush DisabledGB;
    private Rectangle R1;
    private Pen B1;
    private Pen B3;
    private Image _Image;
    private Size _ImageSize;
    private StringAlignment _TextAlignment = StringAlignment.Center;
    private Color _TextColor;
    private ContentAlignment _ImageAlign = ContentAlignment.MiddleLeft;

    #endregion
    #region  Image Designer

    private static PointF ImageLocation(StringFormat SF, SizeF Area, SizeF ImageArea)
    {
        PointF MyPoint = new PointF();
        switch (SF.Alignment)
        {
            case StringAlignment.Center:
                MyPoint.X = (float)((Area.Width - ImageArea.Width) / 2);
                break;
            case StringAlignment.Near:
                MyPoint.X = 2;
                break;
            case StringAlignment.Far:
                MyPoint.X = Area.Width - ImageArea.Width - 2;
                break;

        }

        switch (SF.LineAlignment)
        {
            case StringAlignment.Center:
                MyPoint.Y = (float)((Area.Height - ImageArea.Height) / 2);
                break;
            case StringAlignment.Near:
                MyPoint.Y = 2;
                break;
            case StringAlignment.Far:
                MyPoint.Y = Area.Height - ImageArea.Height - 2;
                break;
        }
        return MyPoint;
    }

    private StringFormat GetStringFormat(ContentAlignment _ContentAlignment)
    {
        StringFormat SF = new StringFormat();
        switch (_ContentAlignment)
        {
            case ContentAlignment.MiddleCenter:
                SF.LineAlignment = StringAlignment.Center;
                SF.Alignment = StringAlignment.Center;
                break;
            case ContentAlignment.MiddleLeft:
                SF.LineAlignment = StringAlignment.Center;
                SF.Alignment = StringAlignment.Near;
                break;
            case ContentAlignment.MiddleRight:
                SF.LineAlignment = StringAlignment.Center;
                SF.Alignment = StringAlignment.Far;
                break;
            case ContentAlignment.TopCenter:
                SF.LineAlignment = StringAlignment.Near;
                SF.Alignment = StringAlignment.Center;
                break;
            case ContentAlignment.TopLeft:
                SF.LineAlignment = StringAlignment.Near;
                SF.Alignment = StringAlignment.Near;
                break;
            case ContentAlignment.TopRight:
                SF.LineAlignment = StringAlignment.Near;
                SF.Alignment = StringAlignment.Far;
                break;
            case ContentAlignment.BottomCenter:
                SF.LineAlignment = StringAlignment.Far;
                SF.Alignment = StringAlignment.Center;
                break;
            case ContentAlignment.BottomLeft:
                SF.LineAlignment = StringAlignment.Far;
                SF.Alignment = StringAlignment.Near;
                break;
            case ContentAlignment.BottomRight:
                SF.LineAlignment = StringAlignment.Far;
                SF.Alignment = StringAlignment.Far;
                break;
        }
        return SF;
    }

    #endregion
    #region  Properties

    public Image Image
    {
        get
        {
            return _Image;
        }
        set
        {
            if (value == null)
            {
                _ImageSize = Size.Empty;
            }
            else
            {
                _ImageSize = value.Size;
            }

            _Image = value;
            Invalidate();
        }
    }

    protected Size ImageSize
    {
        get
        {
            return _ImageSize;
        }
    }

    public ContentAlignment ImageAlign
    {
        get
        {
            return _ImageAlign;
        }
        set
        {
            _ImageAlign = value;
            Invalidate();
        }
    }

    public StringAlignment TextAlignment
    {
        get
        {
            return this._TextAlignment;
        }
        set
        {
            this._TextAlignment = value;
            this.Invalidate();
        }
    }

    public override Color ForeColor
    {
        get
        {
            return this._TextColor;
        }
        set
        {
            this._TextColor = value;
            this.Invalidate();
        }
    }


    #endregion
    #region  EventArgs

    protected override void OnMouseUp(MouseEventArgs e)
    {
        MouseState = 0;
        Invalidate();
        base.OnMouseUp(e);
    }
    protected override void OnMouseDown(MouseEventArgs e)
    {
        MouseState = 1;
        Focus();
        Invalidate();
        base.OnMouseDown(e);
    }

    protected override void OnMouseEnter(System.EventArgs e)
    {
        MouseState = 2;
        Invalidate();
        base.OnMouseEnter(e);
    }
    protected override void OnMouseLeave(EventArgs e)
    {
        MouseState = 0;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        base.OnEnabledChanged(e);
        if (this.Enabled)
        {
            Enable = true;
            Invalidate();
        }
        else
        {
            Enable = false;
            Invalidate();
        }
    }
    protected override void OnTextChanged(System.EventArgs e)
    {
        Invalidate();
        base.OnTextChanged(e);
    }

    #endregion

    public WC_Blue_Button()
    {
        SetStyle((System.Windows.Forms.ControlStyles)(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint), true);

        BackColor = Color.Transparent;
        DoubleBuffered = true;
        Font = new Font("Roboto", 9);
        ForeColor = Color.FromArgb(255, 255, 255);

        Size = new Size(112, 28);
        _TextAlignment = StringAlignment.Center;

        B1 = new Pen(Color.FromArgb(76, 123, 206)); // B1 = Border color
        B3 = new Pen(Color.FromArgb(156, 158, 161)); // B3 = Border color    
    }

    protected override void OnResize(System.EventArgs e)
    {
        base.OnResize(e);
        if (Width > 0 && Height > 0)
        {

            Shape = new GraphicsPath();
            R1 = new Rectangle(0, 0, Width, Height);

            InactiveGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(110, 161, 252), Color.FromArgb(110, 161, 252), 90.0F);
            HoverGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(129, 174, 252), Color.FromArgb(129, 174, 252), 90.0F);
            PressedGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(104, 154, 244), Color.FromArgb(104, 154, 244), 90.0F);
            DisabledGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(218, 221, 225), Color.FromArgb(218, 221, 225), 90.0F);
        }

        Shape.AddArc(0, 0, 3, 3, 180, 90);
        Shape.AddArc(Width - 4, 0, 3, 3, -90, 90);
        Shape.AddArc(Width - 4, Height - 4, 3, 3, 0, 90);
        Shape.AddArc(0, Height - 4, 3, 3, 90, 90);
        Shape.CloseAllFigures();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var G = e.Graphics;
        G.SmoothingMode = SmoothingMode.HighQuality;
        PointF ipt = ImageLocation(GetStringFormat(ImageAlign), Size, ImageSize);

        Font font = new Font("Roboto", 9);

        if (Enabled == true)
        {
            switch (MouseState)
            {
                case 0:
                    G.FillPath(InactiveGB, Shape);
                    // Fill button body with InactiveGB color gradient
                    G.DrawPath(B1, Shape);
                    // Draw button border [InactiveGB]
                    if ((Image == null))
                    {
                        G.DrawString(Text, font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        G.DrawString(Text, font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
                case 1:
                    G.FillPath(PressedGB, Shape);
                    // Fill button body with PressedGB color gradient
                    G.DrawPath(B1, Shape);
                    // Draw button border [PressedGB]

                    if ((Image == null))
                    {
                        G.DrawString(Text, font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        G.DrawString(Text, font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
                case 2:
                    G.FillPath(HoverGB, Shape);
                    // Fill button body with InactiveGB color gradient
                    G.DrawPath(B1, Shape);
                    // Draw button border [InactiveGB]
                    if ((Image == null))
                    {
                        G.DrawString(Text, font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        G.DrawString(Text, font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
            }
        }
        else
        {
            G.FillPath(DisabledGB, Shape);
            // Fill button body with InactiveGB color gradient
            G.DrawPath(B3, Shape);
            // Draw button border [InactiveGB]
            if ((Image == null))
            {
                G.DrawString(Text, font, new SolidBrush(Color.FromArgb(113, 118, 124)), R1, new StringFormat
                {
                    Alignment = _TextAlignment,
                    LineAlignment = StringAlignment.Center
                });
            }
            else
            {
                G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                G.DrawString(Text, font, new SolidBrush(Color.FromArgb(113, 118, 124)), R1, new StringFormat
                {
                    Alignment = _TextAlignment,
                    LineAlignment = StringAlignment.Center
                });
            }
        }
        base.OnPaint(e);
    }
}

#endregion
#region  TextBox

[DefaultEvent("TextChanged")]
public class WC_Blue_TextBox : Control
{

    #region  Variables

    public TextBox BlueTB = new TextBox();
    private int _maxchars = 32767;
    private bool _ReadOnly;
    private bool _Multiline;
    private Image _Image;
    private Size _ImageSize;
    private HorizontalAlignment ALNType;
    private bool isPasswordMasked = false;
    private Pen P1;
    private SolidBrush B1;
    private GraphicsPath Shape;

    #endregion
    #region  Properties

    public HorizontalAlignment TextAlignment
    {
        get
        {
            return ALNType;
        }
        set
        {
            ALNType = value;
            Invalidate();
        }
    }
    public int MaxLength
    {
        get
        {
            return _maxchars;
        }
        set
        {
            _maxchars = value;
            BlueTB.MaxLength = MaxLength;
            Invalidate();
        }
    }

    public bool UseSystemPasswordChar
    {
        get
        {
            return isPasswordMasked;
        }
        set
        {
            BlueTB.UseSystemPasswordChar = UseSystemPasswordChar;
            isPasswordMasked = value;
            Invalidate();
        }
    }
    public bool ReadOnly
    {
        get
        {
            return _ReadOnly;
        }
        set
        {
            _ReadOnly = value;
            if (BlueTB != null)
            {
                BlueTB.ReadOnly = value;
            }
        }
    }
    public bool Multiline
    {
        get
        {
            return _Multiline;
        }
        set
        {
            _Multiline = value;
            if (BlueTB != null)
            {
                BlueTB.Multiline = value;

                if (value)
                {
                    BlueTB.Height = Height - 23;
                }
                else
                {
                    Height = BlueTB.Height + 23;
                }
            }
        }
    }

    public Image Image
    {
        get
        {
            return _Image;
        }
        set
        {
            if (value == null)
            {
                _ImageSize = Size.Empty;
            }
            else
            {
                _ImageSize = value.Size;
            }

            _Image = value;

            if (Image == null)
            {
                BlueTB.Location = new Point(3, 3);
            }
            else
            {
                BlueTB.Location = new Point(35, 11);
            }
            Invalidate();
        }
    }

    protected Size ImageSize
    {
        get
        {
            return _ImageSize;
        }
    }

    #endregion
    #region  EventArgs

    private void _Enter(object Obj, EventArgs e)
    {
        P1 = new Pen(Color.FromArgb(146, 192, 224));
        Refresh();
    }

    private void _Leave(object Obj, EventArgs e)
    {
        P1 = new Pen(Color.FromArgb(172, 172, 172));
        Refresh();
    }

    private void _OnKeyDown(object Obj, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.A)
        {
            BlueTB.SelectAll();
            e.SuppressKeyPress = true;
        }
        if (e.Control && e.KeyCode == Keys.C)
        {
            BlueTB.Copy();
            e.SuppressKeyPress = true;
        }
    }

    public void TextChngTxtBox(System.Object sender, System.EventArgs e)
    {
        Text = BlueTB.Text;
    }

    public void TextChng(System.Object sender, System.EventArgs e)
    {
        BlueTB.Text = Text;
    }

    protected override void OnTextChanged(System.EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        P1 = new Pen(Color.FromArgb(172, 172, 172));
        base.OnEnabledChanged(e);
    }

    protected override void OnForeColorChanged(System.EventArgs e)
    {
        base.OnForeColorChanged(e);
        BlueTB.ForeColor = ForeColor;
        Invalidate();
    }

    protected override void OnFontChanged(System.EventArgs e)
    {
        base.OnFontChanged(e);
        BlueTB.Font = Font;
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        base.OnPaintBackground(e);
    }

    protected override void OnResize(System.EventArgs e)
    {
        base.OnResize(e);
        if (_Multiline)
        {
            BlueTB.Height = Height - 23;
        }
        else
        {
            Height = BlueTB.Height + 6;
        }

        Shape = new GraphicsPath();
        Shape.AddArc(0, 0, 3, 3, 180, 90);
        Shape.AddArc(Width - 4, 0, 3, 3, -90, 90);
        Shape.AddArc(Width - 4, Height - 4, 3, 3, 0, 90);
        Shape.AddArc(0, Height - 4, 3, 3, 90, 90);
        Shape.CloseAllFigures();
    }

    protected override void OnGotFocus(System.EventArgs e)
    {
        base.OnGotFocus(e);
        BlueTB.Focus();
    }

    #endregion

    public void AddTextBox()
    {
        BlueTB.Location = new Point(3, 3);
        BlueTB.Size = new Size(140, 20);
        BlueTB.Text = Text;

        BlueTB.BorderStyle = BorderStyle.None;
        BlueTB.TextAlign = HorizontalAlignment.Left;
        BlueTB.Font = new Font("Tahoma", 10);
        BlueTB.UseSystemPasswordChar = UseSystemPasswordChar;
        BlueTB.Multiline = false;
        BlueTB.BackColor = Color.FromArgb(255, 255, 255);
        BlueTB.ForeColor = Color.FromArgb(138, 133, 133);
        BlueTB.ScrollBars = ScrollBars.None;

        BlueTB.KeyDown += _OnKeyDown;
        BlueTB.Enter += _Enter;
        BlueTB.Leave += _Leave;
    }

    public WC_Blue_TextBox()
        : base()
    {

        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);

        AddTextBox();
        Controls.Add(BlueTB);

        P1 = new Pen(Color.FromArgb(172, 172, 172));
        B1 = new SolidBrush(Color.FromArgb(255, 255, 255));

        BackColor = Color.Transparent;
        ForeColor = Color.FromArgb(138, 133, 133);

        Text = null;
        Font = new Font("Tahoma", 10);
        Size = new Size(140, 20);
        DoubleBuffered = true;
        BlueTB.TextChanged += new EventHandler(TextChngTxtBox);
        base.TextChanged += new EventHandler(TextChng);
    }

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        base.OnPaint(e);
        Bitmap B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);

        G.SmoothingMode = SmoothingMode.AntiAlias;

        if (Image == null)
        {
            BlueTB.Width = Width - 6;
        }
        else
        {
            BlueTB.Width = Width - 45;
        }

        BlueTB.TextAlign = TextAlignment;
        BlueTB.UseSystemPasswordChar = UseSystemPasswordChar;

        G.Clear(Color.Transparent);

        G.FillPath(B1, Shape);
        G.DrawPath(P1, Shape);

        if (Image != null)
        {
            G.DrawImage(_Image, 5, 8, 24, 24);
        }

        e.Graphics.DrawImage((Image)(B.Clone()), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}

#endregion
#region  Radio Button

[DefaultEvent("CheckedChanged")]
public class WC_Blue_RadioButton : Control
{

    #region  Variables

    private int MouseState;
    private int X;
    private bool _Checked;

    #endregion
    #region  Properties

    public bool Checked
    {
        get
        {
            return _Checked;
        }
        set
        {
            _Checked = value;
            InvalidateControls();
            if (CheckedChangedEvent != null)
                CheckedChangedEvent(this);
            Invalidate();
        }
    }

    #endregion
    #region  EventArgs

    public delegate void CheckedChangedEventHandler(object sender);
    private CheckedChangedEventHandler CheckedChangedEvent;
    public event CheckedChangedEventHandler CheckedChanged
    {
        add
        {
            CheckedChangedEvent = (CheckedChangedEventHandler)System.Delegate.Combine(CheckedChangedEvent, value);
        }
        remove
        {
            CheckedChangedEvent = (CheckedChangedEventHandler)System.Delegate.Remove(CheckedChangedEvent, value);
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        MouseState = 0;
        Invalidate();
        base.OnMouseUp(e);
    }
    protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
    {
        MouseState = 2;
        Invalidate();

        if (!_Checked)
        {
            @Checked = true;
        }

        Focus();
        base.OnMouseDown(e);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        MouseState = 1;
        Invalidate();
        base.OnMouseEnter(e);
    }
    protected override void OnMouseLeave(EventArgs e)
    {
        MouseState = 0;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.X;
        Invalidate();
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }
    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        this.Height = 19;

        Invalidate();
    }

    #endregion

    public WC_Blue_RadioButton()
    {

        Width = 139;
        Height = 19;
        DoubleBuffered = true;
    }

    private void InvalidateControls()
    {
        if (!IsHandleCreated || !_Checked)
        {
            return;
        }

        foreach (Control _Control in Parent.Controls)
        {
            if (_Control != this && _Control is WC_Blue_RadioButton)
            {
                ((WC_Blue_RadioButton)_Control).Checked = false;
            }
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
        e.Graphics.DrawString(Text, new Font("Roboto", 10), new SolidBrush(Color.FromArgb(138, 138, 138)), new Point(19, 0));

        if (this.Enabled)
        {
            if (!_Checked)
            {
                switch (MouseState)
                {
                    case 0:
                        e.Graphics.DrawString("a", new Font("bluesymbols", 13), new SolidBrush(Color.FromArgb(172, 172, 172)), new Point(-3, 1));
                        break;
                    case 1:
                        e.Graphics.DrawString("a", new Font("bluesymbols", 13), new SolidBrush(Color.FromArgb(110, 161, 252)), new Point(-3, 1));
                        break;
                    case 2:
                        e.Graphics.DrawString("b", new Font("bluesymbols", 13), new SolidBrush(Color.FromArgb(110, 161, 252)), new Point(-3, 1));
                        break;
                }
            }
            else
            {
                e.Graphics.DrawString("b", new Font("bluesymbols", 13), new SolidBrush(Color.FromArgb(110, 161, 252)), new Point(-3, 1));
            }
        }
        else
        {
            if (_Checked)
            {
                e.Graphics.DrawString("b", new Font("bluesymbols", 13), new SolidBrush(Color.FromArgb(172, 172, 172)), new Point(-3, 1));
            }
            else
            {
                e.Graphics.DrawString("a", new Font("bluesymbols", 13), new SolidBrush(Color.FromArgb(172, 172, 172)), new Point(-3, 1));
            }
        }
    }
}

#endregion
#region  CheckBox

[DefaultEvent("CheckedChanged")]
public class WC_Blue_Checkbox : Control
{

    #region  Variables

    private int MouseState;
    private int X;
    private bool _Checked = false;

    #endregion
    #region  Properties

    public bool Checked
    {
        get
        {
            return _Checked;
        }
        set
        {
            _Checked = value;
            Invalidate();
        }
    }

    #endregion
    #region  EventArgs

    public delegate void CheckedChangedEventHandler(object sender);
    private CheckedChangedEventHandler CheckedChangedEvent;

    public event CheckedChangedEventHandler CheckedChanged
    {
        add
        {
            CheckedChangedEvent = (CheckedChangedEventHandler)System.Delegate.Combine(CheckedChangedEvent, value);
        }
        remove
        {
            CheckedChangedEvent = (CheckedChangedEventHandler)System.Delegate.Remove(CheckedChangedEvent, value);
        }
    }


    protected override void OnMouseUp(MouseEventArgs e)
    {
        MouseState = 0;
        Invalidate();
        base.OnMouseUp(e);
    }
    protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
    {
        _Checked = !_Checked;
        Focus();
        if (CheckedChangedEvent != null)
            CheckedChangedEvent(this);
        base.OnMouseDown(e);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        MouseState = 1;
        Invalidate();
        base.OnMouseEnter(e);
    }
    protected override void OnMouseLeave(EventArgs e)
    {
        MouseState = 0;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.Location.X;
        Invalidate();
    }
    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        this.Height = 19;
        Invalidate();
    }

    #endregion

    public WC_Blue_Checkbox()
    {
        Width = 115;
        Height = 16;

        Font = new Font("Roboto", 10);
        DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

        e.Graphics.DrawString(Text, new Font("Roboto", 10), new SolidBrush(Color.FromArgb(138, 138, 138)), new Point(19, 0));

        if (this.Enabled)
        {
            if (!_Checked)
            {
                switch (MouseState)
                {
                    case 0:

                        e.Graphics.DrawString("u", new Font("bluesymbols", 13), new SolidBrush(Color.FromArgb(172, 172, 172)), new Point(-3, 1));

                        break;
                    case 1:

                        e.Graphics.DrawString("u", new Font("bluesymbols", 13), new SolidBrush(Color.FromArgb(110, 161, 252)), new Point(-3, 1));

                        break;
                    case 2:

                        e.Graphics.DrawString("c", new Font("bluesymbols", 13), new SolidBrush(Color.FromArgb(110, 161, 252)), new Point(-3, 1));

                        break;
                }
            }
            else
            {

                e.Graphics.DrawString("c", new Font("bluesymbols", 13), new SolidBrush(Color.FromArgb(110, 161, 252)), new Point(-3, 1));

            }
        }
        else
        {
            if (_Checked)
            {
                e.Graphics.DrawString("c", new Font("bluesymbols", 13), new SolidBrush(Color.FromArgb(172, 172, 172)), new Point(-3, 1));

            }
            else
            {
                e.Graphics.DrawString("u", new Font("bluesymbols", 13), new SolidBrush(Color.FromArgb(172, 172, 172)), new Point(-3, 1));

            }
        }
    }
}
#endregion
#region  Label

public class WC_Blue_Label : Label
{
    public WC_Blue_Label()
    {
        Font = new Font("Roboto", 10);
        ForeColor = Color.FromArgb(68, 76, 99);
        BackColor = Color.Transparent;
    }
}

#endregion
#region  CircularProgressBar
public class WC_Blue_CircularProgressBar : Control
{
    private float P = 10;

    #region  Properties
    [Description("The current percentage for the ProgressBar, in the range specified by minimum and maximum"), Category("Behavior")]
    public float Percentage
    {
        get { return P; }
        set
        {
            P = value;
            Invalidate();
        }
    }

    [Browsable(false)]
    public bool AllowDrop
    {
        get { return base.AllowDrop; }
        set { base.AllowDrop = value; }
    }
    [Browsable(false)]
    public ImeMode ImeMode
    {
        get { return base.ImeMode; }
        set { base.ImeMode = value; }
    }
    #endregion

    public WC_Blue_CircularProgressBar()
    {
        Font = new Font("Roboto", 10);
        DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        Width = 71;
        Height = 71;

        Rectangle rect = new Rectangle(5, 5, 60, 60);
        Graphics g = e.Graphics;
        float percentage;

        if (Percentage <= 100 && Percentage >= 0)
        {
            percentage = P;
        }
        else
        {
            percentage = 10;
            Percentage = 10;
            MessageBox.Show("Wrong value...!", "Blue Theme", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        dynamic progressAngle = Convert.ToSingle((percentage * 360) / 100);
        dynamic remainderAngle = 360 - progressAngle;

        using (Pen progressPen = new Pen(new SolidBrush(Color.FromArgb(110, 161, 252)), 3))
        {
            using (Pen remainderPen = new Pen(Color.LightGray, 3))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawArc(progressPen, rect, -90, progressAngle);
                g.DrawArc(remainderPen, rect, progressAngle - 90, remainderAngle);
            }
        }

        using (Font fnt = new Font("Roboto", 14))
        {
            string text = percentage.ToString() + "%";
            dynamic textSize = g.MeasureString(text, fnt);
            Point textPoint = new Point(Convert.ToInt32(rect.Left + (rect.Width / 2) - (textSize.Width / 2)), Convert.ToInt32(rect.Top + (rect.Height / 2) - (textSize.Height / 2)));

            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.DrawString(text, fnt, new SolidBrush(Color.FromArgb(172, 172, 172)), textPoint);
        }
    }
}
#endregion
#region ProgressBar
public class WC_Blue_ProgressBar : Control
{
    #region variables

    private Rectangle BGShape;
    private int _Value = 10;
    private Pen P = new Pen(Color.FromArgb(184, 186, 191));

    #endregion
    #region Properties
    [Category("Behavior")]
    public int Value
    {
        get { return _Value; }
        set
        {
            _Value = value;
            Invalidate();
        }
    }
    #endregion

    public WC_Blue_ProgressBar()
    {
        Width = 300;
        Height = 13;

        Font = new Font("Roboto", 10);
        DoubleBuffered = true;
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);

        BGShape = new Rectangle(0, 0, Width - 1, Height - 1);
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics G = e.Graphics;

        HatchBrush Hatch = new HatchBrush(HatchStyle.WideUpwardDiagonal, Color.FromArgb(78, 131, 224), Color.FromArgb(110, 161, 252));

        if (_Value <= 100 && _Value >= 0)
        {
            G.DrawRectangle(P, BGShape);
            G.FillRectangle(Hatch, 2, 2, (Width * _Value) / (100 - 0) - 4, Height - 4);
        }
        else
        {
            _Value = 10;
            MessageBox.Show("Wrong value...!", "Blue Theme", MessageBoxButtons.OK, MessageBoxIcon.Information);

            G.DrawRectangle(P, BGShape);
            G.FillRectangle(Hatch, 2, 2, (Width * _Value) / (100 - 0) - 4, Height - 4);
        }
    }
}
#endregion
#region TabControl


public class WC_Blue_TabControl : TabControl
{

    #region variables

    Bitmap B;
    Graphics G;

    #endregion
    #region EventArgs

    protected override void CreateHandle()
    {
        base.CreateHandle();

        base.DoubleBuffered = true;
        SizeMode = TabSizeMode.Fixed;
        Appearance = TabAppearance.Normal;
        Alignment = TabAlignment.Left;
    }
    protected override void OnControlAdded(ControlEventArgs e)
    {
        base.OnControlAdded(e);
        if (e.Control is TabPage)
        {
            IEnumerator enumerator;
            try
            {
                enumerator = this.Controls.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    TabPage current = (TabPage)enumerator.Current;
                    current = new TabPage();
                }
            }
            finally
            {
                e.Control.BackColor = Color.FromArgb(255, 255, 255);
            }
        }
    }

    #endregion

    public WC_Blue_TabControl()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true);

        DoubleBuffered = true;
        SizeMode = TabSizeMode.Fixed;
        ItemSize = new Size(35, 161);
        DrawMode = TabDrawMode.OwnerDrawFixed;

        foreach (TabPage Page in this.TabPages)
        {
            Page.BackColor = Color.FromArgb(255, 255, 255);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        B = new Bitmap(Width, Height);
        G = Graphics.FromImage(B);

        G.Clear(Color.FromArgb(255, 255, 255));
        G.SmoothingMode = SmoothingMode.HighSpeed;
        G.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
        G.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

        G.FillRectangle(new SolidBrush(Color.FromArgb(68, 76, 99)), new Rectangle(2, 0, ItemSize.Height, Height));

        for (int i = 0; i <= TabCount - 1; i++)
        {
            if (i == SelectedIndex)
            {
                //SelectedTab BG
                G.FillRectangle(new SolidBrush(Color.FromArgb(47, 52, 70)), GetTabRect(i).Location.X, GetTabRect(i).Location.Y - 2, ItemSize.Height, ItemSize.Width);
                //TabHighlighter
                G.FillRectangle(new SolidBrush(Color.FromArgb(110, 161, 252)), GetTabRect(i).X + 156, GetTabRect(i).Location.Y - 2, 5, GetTabRect(i).Height);
            }

            G.DrawString(TabPages[i].Text, new Font("Roboto", 10), Brushes.White, new Rectangle(GetTabRect(i).Left + 47, GetTabRect(i).Top + 6, GetTabRect(i).Width, GetTabRect(i).Height));
        }

        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
        base.OnPaint(e);
    }
}

#endregion
#region GroupBox


public class WC_Blue_GroupBox : ContainerControl
{

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        Refresh();
    }

    public WC_Blue_GroupBox()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.Transparent;
        DoubleBuffered = true;
        Size = new Size(195, 104);
        MinimumSize = new Size(136, 50);
        Padding = new Padding(5, 28, 5, 5);
    }

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        base.OnPaint(e);
        Bitmap B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        int StringWidth = new int();

        StringWidth = (int)G.MeasureString(Text, new Font("Roboto", 10)).Width;
        Rectangle TitleBox = new Rectangle(14, 2, StringWidth + 5, 15);

        G.Clear(Color.Transparent);
        G.SmoothingMode = SmoothingMode.HighQuality;

        //BG line && BG of the title
        G.DrawLine(new Pen(Color.Gray), 0, 10, Width, 10);
        G.FillRectangle(Brushes.White, TitleBox);

        //The string of the groupbox
        G.DrawString(Text, new Font("Roboto", 10), new SolidBrush(Color.Gray), TitleBox, new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        });

        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}

#endregion
#endregion

#region WC_Linux
namespace WC_Linux
{


    #region RoundRectangle

    static class RoundRectangle
    {
        public static GraphicsPath RoundRect(Rectangle Rectangle, int Curve)
        {
            GraphicsPath P = new GraphicsPath();
            int ArcRectangleWidth = Curve * 2;
            P.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90);
            P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90);
            P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90);
            P.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90);
            P.AddLine(new Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), new Point(Rectangle.X, Curve + Rectangle.Y));
            return P;
        }
        public static GraphicsPath RoundRect(int X, int Y, int Width, int Height, int Curve)
        {
            Rectangle Rectangle = new Rectangle(X, Y, Width, Height);
            GraphicsPath P = new GraphicsPath();
            int ArcRectangleWidth = Curve * 2;
            P.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90);
            P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90);
            P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90);
            P.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90);
            P.AddLine(new Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), new Point(Rectangle.X, Curve + Rectangle.Y));
            return P;
        }
        public static GraphicsPath RoundedTopRect(Rectangle Rectangle, int Curve)
        {
            GraphicsPath P = new GraphicsPath();
            int ArcRectangleWidth = Curve * 2;
            P.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90);
            P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90);
            P.AddLine(new Point(Rectangle.X + Rectangle.Width, Rectangle.Y + ArcRectangleWidth), new Point(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height - 1));
            P.AddLine(new Point(Rectangle.X, Rectangle.Height - 1 + Rectangle.Y), new Point(Rectangle.X, Rectangle.Y + Curve));
            return P;
        }
    }

    #endregion
    #region  ControlBox

    public class WC_Linux_ControlBox : Control
    {

        #region  Enums

        public enum MouseState
        {
            None = 0,
            Over = 1,
            Down = 2
        }

        #endregion
        #region  MouseStates
        MouseState State = MouseState.None;
        int X;
        Rectangle CloseBtn = new Rectangle(3, 2, 17, 17);
        Rectangle MinBtn = new Rectangle(23, 2, 17, 17);
        Rectangle MaxBtn = new Rectangle(43, 2, 17, 17);

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);

            State = MouseState.Down;
            Invalidate();
        }
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (X > 3 && X < 20)
            {
                FindForm().Close();
            }
            else if (X > 23 && X < 40)
            {
                FindForm().WindowState = FormWindowState.Minimized;
            }
            else if (X > 43 && X < 60)
            {
                if (_EnableMaximize == true)
                {
                    if (FindForm().WindowState == FormWindowState.Maximized)
                    {
                        FindForm().WindowState = FormWindowState.Minimized;
                        FindForm().WindowState = FormWindowState.Normal;
                    }
                    else
                    {
                        FindForm().WindowState = FormWindowState.Minimized;
                        FindForm().WindowState = FormWindowState.Maximized;
                    }
                }
            }
            State = MouseState.Over;
            Invalidate();
        }
        protected override void OnMouseEnter(System.EventArgs e)
        {
            base.OnMouseEnter(e);
            State = MouseState.Over;
            Invalidate();
        }
        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);
            State = MouseState.None;
            Invalidate();
        }
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            X = e.Location.X;
            Invalidate();
        }
        #endregion
        #region  Properties

        bool _EnableMaximize = true;
        public bool EnableMaximize
        {
            get
            {
                return _EnableMaximize;
            }
            set
            {
                _EnableMaximize = value;
                if (_EnableMaximize == true)
                {
                    this.Size = new Size(64, 22);
                }
                else
                {
                    this.Size = new Size(44, 22);
                }
                Invalidate();
            }
        }

        #endregion

        public WC_Linux_ControlBox()
        {
            SetStyle((System.Windows.Forms.ControlStyles)(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer), true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Font = new Font("Marlett", 7);
            //Anchor = (System.Windows.Forms.AnchorStyles)(AnchorStyles.Top | AnchorStyles.Right);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_EnableMaximize == true)
            {
                this.Size = new Size(64, 22);
            }
            else
            {
                this.Size = new Size(44, 22);
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            // Auto-decide control location on the theme container
            // Location = new Point(5, 13);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);

            base.OnPaint(e);
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            LinearGradientBrush LGBClose = new LinearGradientBrush(CloseBtn, Color.FromArgb(242, 132, 99), Color.FromArgb(224, 82, 33), 90);
            G.FillEllipse(LGBClose, CloseBtn);
            G.DrawEllipse(new Pen(Color.FromArgb(57, 56, 53)), CloseBtn);
            G.DrawString("r", new Font("Marlett", 7), new SolidBrush(Color.FromArgb(52, 50, 46)), new Rectangle((int)6.5, 8, 0, 0));

            LinearGradientBrush LGBMinimize = new LinearGradientBrush(MinBtn, Color.FromArgb(130, 129, 123), Color.FromArgb(103, 102, 96), 90);
            G.FillEllipse(LGBMinimize, MinBtn);
            G.DrawEllipse(new Pen(Color.FromArgb(57, 56, 53)), MinBtn);
            G.DrawString("0", new Font("Marlett", 7), new SolidBrush(Color.FromArgb(52, 50, 46)), new Rectangle(26, (int)4.4, 0, 0));

            if (_EnableMaximize == true)
            {
                LinearGradientBrush LGBMaximize = new LinearGradientBrush(MaxBtn, Color.FromArgb(130, 129, 123), Color.FromArgb(103, 102, 96), 90);
                G.FillEllipse(LGBMaximize, MaxBtn);
                G.DrawEllipse(new Pen(Color.FromArgb(57, 56, 53)), MaxBtn);
                G.DrawString("1", new Font("Marlett", 7), new SolidBrush(Color.FromArgb(52, 50, 46)), new Rectangle(46, 7, 0, 0));
            }

            switch (State)
            {
                case MouseState.None:
                    LinearGradientBrush xLGBClose_1 = new LinearGradientBrush(CloseBtn, Color.FromArgb(242, 132, 99), Color.FromArgb(224, 82, 33), 90);
                    G.FillEllipse(xLGBClose_1, CloseBtn);
                    G.DrawEllipse(new Pen(Color.FromArgb(57, 56, 53)), CloseBtn);
                    G.DrawString("r", new Font("Marlett", 7), new SolidBrush(Color.FromArgb(52, 50, 46)), new Rectangle((int)6.5, 8, 0, 0));

                    LinearGradientBrush xLGBMinimize_1 = new LinearGradientBrush(MinBtn, Color.FromArgb(130, 129, 123), Color.FromArgb(103, 102, 96), 90);
                    G.FillEllipse(xLGBMinimize_1, MinBtn);
                    G.DrawEllipse(new Pen(Color.FromArgb(57, 56, 53)), MinBtn);
                    G.DrawString("0", new Font("Marlett", 7), new SolidBrush(Color.FromArgb(52, 50, 46)), new Rectangle(26, (int)4.4, 0, 0));

                    if (_EnableMaximize == true)
                    {
                        LinearGradientBrush xLGBMaximize = new LinearGradientBrush(MaxBtn, Color.FromArgb(130, 129, 123), Color.FromArgb(103, 102, 96), 90);
                        G.FillEllipse(xLGBMaximize, MaxBtn);
                        G.DrawEllipse(new Pen(Color.FromArgb(57, 56, 53)), MaxBtn);
                        G.DrawString("1", new Font("Marlett", 7), new SolidBrush(Color.FromArgb(52, 50, 46)), new Rectangle(46, 7, 0, 0));
                    }
                    break;
                case MouseState.Over:
                    if (X > 3 && X < 20)
                    {
                        LinearGradientBrush xLGBClose = new LinearGradientBrush(CloseBtn, Color.FromArgb(248, 152, 124), Color.FromArgb(231, 92, 45), 90);
                        G.FillEllipse(xLGBClose, CloseBtn);
                        G.DrawEllipse(new Pen(Color.FromArgb(57, 56, 53)), CloseBtn);
                        G.DrawString("r", new Font("Marlett", 7), new SolidBrush(Color.FromArgb(52, 50, 46)), new Rectangle((int)6.5, 8, 0, 0));
                    }
                    else if (X > 23 && X < 40)
                    {
                        LinearGradientBrush xLGBMinimize = new LinearGradientBrush(MinBtn, Color.FromArgb(196, 196, 196), Color.FromArgb(173, 173, 173), 90);
                        G.FillEllipse(xLGBMinimize, MinBtn);
                        G.DrawEllipse(new Pen(Color.FromArgb(57, 56, 53)), MinBtn);
                        G.DrawString("0", new Font("Marlett", 7), new SolidBrush(Color.FromArgb(52, 50, 46)), new Rectangle(26, (int)4.4, 0, 0));
                    }
                    else if (X > 43 && X < 60)
                    {
                        if (_EnableMaximize == true)
                        {
                            LinearGradientBrush xLGBMaximize = new LinearGradientBrush(MaxBtn, Color.FromArgb(196, 196, 196), Color.FromArgb(173, 173, 173), 90);
                            G.FillEllipse(xLGBMaximize, MaxBtn);
                            G.DrawEllipse(new Pen(Color.FromArgb(57, 56, 53)), MaxBtn);
                            G.DrawString("1", new Font("Marlett", 7), new SolidBrush(Color.FromArgb(52, 50, 46)), new Rectangle(46, 7, 0, 0));
                        }
                    }
                    break;
            }

            e.Graphics.DrawImage((Image)(B.Clone()), 0, 0);
            G.Dispose();
            B.Dispose();
        }
    }

    #endregion
    #region Button 1

    class WC_Linux_Button_1 : Control
    {

        #region Variables

        private int MouseState;
        private GraphicsPath Shape;
        private LinearGradientBrush InactiveGB;
        private LinearGradientBrush PressedGB;
        private LinearGradientBrush PressedContourGB;
        private Rectangle R1;
        private Pen P1;
        private Pen P3;
        private Image _Image;
        private Size _ImageSize;
        private StringAlignment _TextAlignment = StringAlignment.Center;
        private Color _TextColor = Color.FromArgb(150, 150, 150);
        private ContentAlignment _ImageAlign = ContentAlignment.MiddleLeft;

        #endregion
        #region Image Designer

        private static PointF ImageLocation(StringFormat SF, SizeF Area, SizeF ImageArea)
        {
            PointF MyPoint = default(PointF);
            switch (SF.Alignment)
            {
                case StringAlignment.Center:
                    MyPoint.X = Convert.ToSingle((Area.Width - ImageArea.Width) / 2);
                    break;
                case StringAlignment.Near:
                    MyPoint.X = 2;
                    break;
                case StringAlignment.Far:
                    MyPoint.X = Area.Width - ImageArea.Width - 2;

                    break;
            }

            switch (SF.LineAlignment)
            {
                case StringAlignment.Center:
                    MyPoint.Y = Convert.ToSingle((Area.Height - ImageArea.Height) / 2);
                    break;
                case StringAlignment.Near:
                    MyPoint.Y = 2;
                    break;
                case StringAlignment.Far:
                    MyPoint.Y = Area.Height - ImageArea.Height - 2;
                    break;
            }
            return MyPoint;
        }

        private StringFormat GetStringFormat(ContentAlignment _ContentAlignment)
        {
            StringFormat SF = new StringFormat();
            switch (_ContentAlignment)
            {
                case ContentAlignment.MiddleCenter:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleLeft:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleRight:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.TopCenter:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.TopLeft:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomRight:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Far;
                    break;
            }
            return SF;
        }

        #endregion
        #region Properties

        public Image Image
        {
            get { return _Image; }
            set
            {
                if (value == null)
                {
                    _ImageSize = Size.Empty;
                }
                else
                {
                    _ImageSize = value.Size;
                }

                _Image = value;
                Invalidate();
            }
        }

        protected Size ImageSize
        {
            get { return _ImageSize; }
        }

        public ContentAlignment ImageAlign
        {
            get { return _ImageAlign; }
            set
            {
                _ImageAlign = value;
                Invalidate();
            }
        }

        public StringAlignment TextAlignment
        {
            get { return this._TextAlignment; }
            set
            {
                this._TextAlignment = value;
                this.Invalidate();
            }
        }

        public override Color ForeColor
        {
            get { return this._TextColor; }
            set
            {
                this._TextColor = value;
                this.Invalidate();
            }
        }

        #endregion
        #region EventArgs

        protected override void OnMouseUp(MouseEventArgs e)
        {
            MouseState = 0;
            Invalidate();
            base.OnMouseUp(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            MouseState = 1;
            Focus();
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = 0;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }

        #endregion

        public WC_Linux_Button_1()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);

            BackColor = Color.Transparent;
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 12);
            ForeColor = Color.FromArgb(76, 76, 76);
            Size = new Size(177, 30);
            _TextAlignment = StringAlignment.Center;
            P1 = new Pen(Color.FromArgb(180, 180, 180));
            // P1 = Border color
        }

        protected override void OnResize(System.EventArgs e)
        {

            if (Width > 0 && Height > 0)
            {
                Shape = new GraphicsPath();
                R1 = new Rectangle(0, 0, Width, Height);

                InactiveGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(253, 252, 252), Color.FromArgb(239, 237, 236), 90f);
                PressedGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(226, 226, 226), Color.FromArgb(237, 237, 237), 90f);
                PressedContourGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(167, 167, 167), Color.FromArgb(167, 167, 167), 90f);

                P3 = new Pen(PressedContourGB);
            }

            var MyDrawer = Shape;
            MyDrawer.AddArc(0, 0, 10, 10, 180, 90);
            MyDrawer.AddArc(Width - 11, 0, 10, 10, -90, 90);
            MyDrawer.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            MyDrawer.AddArc(0, Height - 11, 10, 10, 90, 90);
            MyDrawer.CloseAllFigures();
            Invalidate();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
            G.SmoothingMode = SmoothingMode.HighQuality;
            PointF ipt = ImageLocation(GetStringFormat(ImageAlign), Size, ImageSize);

            switch (MouseState)
            {
                case 0:
                    //Inactive
                    G.FillPath(InactiveGB, Shape);
                    // Fill button body with InactiveGB color gradient
                    G.DrawPath(P1, Shape);
                    // Draw button border [InactiveGB]
                    if ((Image == null))
                    {
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
                case 1:
                    //Pressed
                    G.FillPath(PressedGB, Shape);
                    // Fill button body with PressedGB color gradient
                    G.DrawPath(P3, Shape);
                    // Draw button border [PressedGB]

                    if ((Image == null))
                    {
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
            }
            base.OnPaint(e);
        }
    }

    #endregion
    #region Button 2

    class WC_Linux_Button_2 : Control
    {

        #region Variables

        private int MouseState;
        private GraphicsPath Shape;
        private LinearGradientBrush InactiveGB;
        private LinearGradientBrush PressedGB;
        private LinearGradientBrush PressedContourGB;
        private Rectangle R1;
        private Pen P1;
        private Pen P3;
        private Image _Image;
        private Size _ImageSize;
        private StringAlignment _TextAlignment = StringAlignment.Center;
        private Color _TextColor = Color.FromArgb(150, 150, 150);
        private ContentAlignment _ImageAlign = ContentAlignment.MiddleLeft;

        #endregion
        #region Image Designer

        private static PointF ImageLocation(StringFormat SF, SizeF Area, SizeF ImageArea)
        {
            PointF MyPoint = default(PointF);
            switch (SF.Alignment)
            {
                case StringAlignment.Center:
                    MyPoint.X = Convert.ToSingle((Area.Width - ImageArea.Width) / 2);
                    break;
                case StringAlignment.Near:
                    MyPoint.X = 2;
                    break;
                case StringAlignment.Far:
                    MyPoint.X = Area.Width - ImageArea.Width - 2;

                    break;
            }

            switch (SF.LineAlignment)
            {
                case StringAlignment.Center:
                    MyPoint.Y = Convert.ToSingle((Area.Height - ImageArea.Height) / 2);
                    break;
                case StringAlignment.Near:
                    MyPoint.Y = 2;
                    break;
                case StringAlignment.Far:
                    MyPoint.Y = Area.Height - ImageArea.Height - 2;
                    break;
            }
            return MyPoint;
        }

        private StringFormat GetStringFormat(ContentAlignment _ContentAlignment)
        {
            StringFormat SF = new StringFormat();
            switch (_ContentAlignment)
            {
                case ContentAlignment.MiddleCenter:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleLeft:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleRight:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.TopCenter:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.TopLeft:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomRight:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Far;
                    break;
            }
            return SF;
        }

        #endregion
        #region Properties

        public Image Image
        {
            get { return _Image; }
            set
            {
                if (value == null)
                {
                    _ImageSize = Size.Empty;
                }
                else
                {
                    _ImageSize = value.Size;
                }

                _Image = value;
                Invalidate();
            }
        }

        protected Size ImageSize
        {
            get { return _ImageSize; }
        }

        public ContentAlignment ImageAlign
        {
            get { return _ImageAlign; }
            set
            {
                _ImageAlign = value;
                Invalidate();
            }
        }

        public StringAlignment TextAlignment
        {
            get { return this._TextAlignment; }
            set
            {
                this._TextAlignment = value;
                this.Invalidate();
            }
        }

        public override Color ForeColor
        {
            get { return this._TextColor; }
            set
            {
                this._TextColor = value;
                this.Invalidate();
            }
        }

        #endregion
        #region EventArgs

        protected override void OnMouseUp(MouseEventArgs e)
        {
            MouseState = 0;
            Invalidate();
            base.OnMouseUp(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            MouseState = 1;
            Focus();
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = 0;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }

        #endregion

        public WC_Linux_Button_2()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);

            BackColor = Color.Transparent;
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            ForeColor = Color.FromArgb(76, 76, 76);
            Size = new Size(177, 30);
            _TextAlignment = StringAlignment.Center;
            P1 = new Pen(Color.FromArgb(162, 120, 101));
            // P1 = Border color
        }

        protected override void OnResize(System.EventArgs e)
        {

            if (Width > 0 && Height > 0)
            {
                Shape = new GraphicsPath();
                R1 = new Rectangle(0, 0, Width, Height);

                InactiveGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(253, 175, 143), Color.FromArgb(244, 146, 106), 90f);
                PressedGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(244, 146, 106), Color.FromArgb(244, 146, 106), 90f);
                PressedContourGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(162, 120, 101), Color.FromArgb(162, 120, 101), 90f);

                P3 = new Pen(PressedContourGB);
            }

            var MyDrawer = Shape;
            MyDrawer.AddArc(0, 0, 10, 10, 180, 90);
            MyDrawer.AddArc(Width - 11, 0, 10, 10, -90, 90);
            MyDrawer.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            MyDrawer.AddArc(0, Height - 11, 10, 10, 90, 90);
            MyDrawer.CloseAllFigures();
            Invalidate();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
            G.SmoothingMode = SmoothingMode.HighQuality;
            PointF ipt = ImageLocation(GetStringFormat(ImageAlign), Size, ImageSize);

            switch (MouseState)
            {
                case 0:
                    //Inactive
                    G.FillPath(InactiveGB, Shape);
                    // Fill button body with InactiveGB color gradient
                    G.DrawPath(P1, Shape);
                    // Draw button border [InactiveGB]
                    if ((Image == null))
                    {
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
                case 1:
                    //Pressed
                    G.FillPath(PressedGB, Shape);
                    // Fill button body with PressedGB color gradient
                    G.DrawPath(P3, Shape);
                    // Draw button border [PressedGB]

                    if ((Image == null))
                    {
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
            }
            base.OnPaint(e);
        }
    }

    #endregion
    #region Label

    class WC_Linux_Label : Label
    {

        public WC_Linux_Label()
        {
            Font = new Font("Segoe UI", 11);
            ForeColor = Color.FromArgb(76, 76, 77);
            BackColor = Color.Transparent;
        }
    }

    #endregion
    #region Link Label
    class WC_Linux_LinkLabel : LinkLabel
    {

        public WC_Linux_LinkLabel()
        {
            Font = new Font("Segoe UI", 11, FontStyle.Regular);
            BackColor = Color.Transparent;
            LinkColor = Color.FromArgb(240, 119, 70);
            ActiveLinkColor = Color.FromArgb(221, 72, 20);
            VisitedLinkColor = Color.FromArgb(240, 119, 70);
            LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
        }
    }

    #endregion
    #region Header Label

    class WC_Linux_HeaderLabel : Label
    {

        public WC_Linux_HeaderLabel()
        {
            Font = new Font("Segoe UI", 11, FontStyle.Bold);
            ForeColor = Color.FromArgb(76, 76, 77);
            BackColor = Color.Transparent;
        }
    }

    #endregion
    #region Separator

    public class WC_Linux_Separator : Control
    {

        public WC_Linux_Separator()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            this.Size = new Size(120, 10);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(224, 222, 220)), 0, 5, Width, 5);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(250, 249, 249)), 0, 6, Width, 6);
        }
    }

    #endregion
    #region ProgressBar

    public class WC_Linux_ProgressBar : Control
    {

        #region Enums

        public enum Alignment
        {
            Right,
            Center
        }

        #endregion
        #region Variables

        private int _Minimum;
        private int _Maximum = 100;
        private int _Value = 0;
        private Alignment ALN;
        private bool _DrawHatch;

        private bool _ShowPercentage;
        private GraphicsPath GP1;
        private GraphicsPath GP2;
        private GraphicsPath GP3;
        private Rectangle R1;
        private Rectangle R2;
        private LinearGradientBrush GB1;
        private LinearGradientBrush GB2;
        private int I1;

        #endregion
        #region Properties

        public int Maximum
        {
            get { return _Maximum; }
            set
            {
                if (value < 1)
                    value = 1;
                if (value < _Value)
                    _Value = value;
                _Maximum = value;
                Invalidate();
            }
        }

        public int Minimum
        {
            get { return _Minimum; }
            set
            {
                _Minimum = value;

                if (value > _Maximum)
                    _Maximum = value;
                if (value > _Value)
                    _Value = value;

                Invalidate();
            }
        }

        public int Value
        {
            get { return _Value; }
            set
            {
                if (value > _Maximum)
                    value = Maximum;
                _Value = value;
                Invalidate();
            }
        }

        public Alignment ValueAlignment
        {
            get { return ALN; }
            set
            {
                ALN = value;
                Invalidate();
            }
        }

        public bool DrawHatch
        {
            get { return _DrawHatch; }
            set
            {
                _DrawHatch = value;
                Invalidate();
            }
        }

        public bool ShowPercentage
        {
            get { return _ShowPercentage; }
            set
            {
                _ShowPercentage = value;
                Invalidate();
            }
        }

        #endregion
        #region EventArgs

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Height = 20;
            Size minimumSize = new Size(58, 20);
            this.MinimumSize = minimumSize;
        }

        #endregion

        public WC_Linux_ProgressBar()
        {
            _Maximum = 100;
            _ShowPercentage = true;
            _DrawHatch = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
            DoubleBuffered = true;
        }

        public void Increment(int value)
        {
            this._Value += value;
            Invalidate();
        }

        public void Deincrement(int value)
        {
            this._Value -= value;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);

            G.Clear(Color.Transparent);
            G.SmoothingMode = SmoothingMode.HighQuality;

            GP1 = RoundRectangle.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 4);
            GP2 = RoundRectangle.RoundRect(new Rectangle(1, 1, Width - 3, Height - 3), 4);

            R1 = new Rectangle(0, 2, Width - 1, Height - 1);
            GB1 = new LinearGradientBrush(R1, Color.FromArgb(255, 255, 255), Color.FromArgb(230, 230, 230), 90f);

            // Draw inside background
            G.FillRectangle(new SolidBrush(Color.FromArgb(244, 241, 243)), R1);
            G.SetClip(GP1);
            G.FillPath(new SolidBrush(Color.FromArgb(244, 241, 243)), RoundRectangle.RoundRect(new Rectangle(1, 1, Width - 3, Height / 2 - 2), 4));


            I1 = (int)Math.Round(((double)(this._Value - this._Minimum) / (double)(this._Maximum - this._Minimum)) * (double)(this.Width - 3));
            if (I1 > 1)
            {
                GP3 = RoundRectangle.RoundRect(new Rectangle(1, 1, I1, Height - 3), 4);

                R2 = new Rectangle(1, 1, I1, Height - 3);
                GB2 = new LinearGradientBrush(R2, Color.FromArgb(214, 89, 37), Color.FromArgb(223, 118, 75), 90f);

                // Fill the value with its gradient
                G.FillPath(GB2, GP3);

                // Draw diagonal lines
                if (_DrawHatch == true)
                {
                    for (var i = 0; i <= (Width - 1) * _Maximum / _Value; i += 20)
                    {
                        G.DrawLine(new Pen(new SolidBrush(Color.FromArgb(25, Color.White)), 10.0F), new Point(System.Convert.ToInt32(i), 0), new Point((int)(i - 10), Height));
                    }
                }

                G.SetClip(GP3);
                G.SmoothingMode = SmoothingMode.None;
                G.SmoothingMode = SmoothingMode.AntiAlias;
                G.ResetClip();
            }

            // Draw value as a string
            string DrawString = Convert.ToString(Convert.ToInt32(Value)) + "%";
            int textX = (int)(this.Width - G.MeasureString(DrawString, Font).Width - 1);
            int textY = (int)((this.Height / 2) - (System.Convert.ToInt32(G.MeasureString(DrawString, Font).Height / 2) - 2));

            if (_ShowPercentage == true)
            {
                switch (ValueAlignment)
                {
                    case Alignment.Right:
                        G.DrawString(DrawString, new Font("Segoe UI", 8), Brushes.DimGray, new Point(textX, textY));
                        break;
                    case Alignment.Center:
                        G.DrawString(DrawString, new Font("Segoe UI", 8), Brushes.DimGray, new Rectangle(0, 0, Width, Height + 2), new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        });
                        break;
                }
            }

            // Draw border
            G.DrawPath(new Pen(Color.FromArgb(180, 180, 180)), GP2);

            e.Graphics.DrawImage((Image)(B.Clone()), 0, 0);
            G.Dispose();
            B.Dispose();
        }
    }

    #endregion
    #region Progress Indicator

    class WC_Linux_ProgressIndicator : Control
    {

        #region Variables

        private readonly SolidBrush BaseColor = new SolidBrush(Color.FromArgb(76, 76, 76));
        private readonly SolidBrush AnimationColor = new SolidBrush(Color.Gray);

        private readonly Timer AnimationSpeed = new Timer();
        private PointF[] FloatPoint;
        private BufferedGraphics BuffGraphics;
        private int IndicatorIndex;
        private readonly BufferedGraphicsContext GraphicsContext = BufferedGraphicsManager.Current;

        #endregion
        #region Custom Properties

        public Color P_BaseColor
        {
            get { return BaseColor.Color; }
            set { BaseColor.Color = value; }
        }

        public Color P_AnimationColor
        {
            get { return AnimationColor.Color; }
            set { AnimationColor.Color = value; }
        }

        public int P_AnimationSpeed
        {
            get { return AnimationSpeed.Interval; }
            set { AnimationSpeed.Interval = value; }
        }

        #endregion
        #region EventArgs

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetStandardSize();
            UpdateGraphics();
            SetPoints();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            AnimationSpeed.Enabled = this.Enabled;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AnimationSpeed.Tick += AnimationSpeed_Tick;
            AnimationSpeed.Start();
        }

        private void AnimationSpeed_Tick(object sender, EventArgs e)
        {
            if (IndicatorIndex.Equals(0))
            {
                IndicatorIndex = FloatPoint.Length - 1;
            }
            else
            {
                IndicatorIndex -= 1;
            }
            this.Invalidate(false);
        }

        #endregion

        public WC_Linux_ProgressIndicator()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            Size = new Size(80, 80);
            Text = string.Empty;
            MinimumSize = new Size(80, 80);
            SetPoints();
            AnimationSpeed.Interval = 100;
        }

        private void SetStandardSize()
        {
            int _Size = Math.Max(Width, Height);
            Size = new Size(_Size, _Size);
        }

        private void SetPoints()
        {
            Stack<PointF> stack = new Stack<PointF>();
            PointF startingFloatPoint = new PointF(((float)this.Width) / 2f, ((float)this.Height) / 2f);
            for (float i = 0f; i < 360f; i += 45f)
            {
                this.SetValue(startingFloatPoint, (int)Math.Round((double)((((double)this.Width) / 2.0) - 15.0)), (double)i);
                PointF endPoint = this.EndPoint;
                endPoint = new PointF(endPoint.X - 7.5f, endPoint.Y - 7.5f);
                stack.Push(endPoint);
            }
            this.FloatPoint = stack.ToArray();
        }

        private void UpdateGraphics()
        {
            if ((this.Width > 0) && (this.Height > 0))
            {
                Size size2 = new Size(this.Width + 1, this.Height + 1);
                this.GraphicsContext.MaximumBuffer = size2;
                this.BuffGraphics = this.GraphicsContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
                this.BuffGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.BuffGraphics.Graphics.Clear(this.BackColor);
            int num2 = this.FloatPoint.Length - 1;
            for (int i = 0; i <= num2; i++)
            {
                if (this.IndicatorIndex == i)
                {
                    this.BuffGraphics.Graphics.FillEllipse(this.AnimationColor, this.FloatPoint[i].X, this.FloatPoint[i].Y, 15f, 15f);
                }
                else
                {
                    this.BuffGraphics.Graphics.FillEllipse(this.BaseColor, this.FloatPoint[i].X, this.FloatPoint[i].Y, 15f, 15f);
                }
            }
            this.BuffGraphics.Render(e.Graphics);
        }


        private double Rise;
        private double Run;
        private PointF _StartingFloatPoint;

        private X AssignValues<X>(ref X Run, X Length)
        {
            Run = Length;
            return Length;
        }

        private void SetValue(PointF StartingFloatPoint, int Length, double Angle)
        {
            double CircleRadian = Math.PI * Angle / 180.0;

            _StartingFloatPoint = StartingFloatPoint;
            Rise = AssignValues(ref Run, Length);
            Rise = Math.Sin(CircleRadian) * Rise;
            Run = Math.Cos(CircleRadian) * Run;
        }

        private PointF EndPoint
        {
            get
            {
                float LocationX = Convert.ToSingle(_StartingFloatPoint.Y + Rise);
                float LocationY = Convert.ToSingle(_StartingFloatPoint.X + Run);

                return new PointF(LocationY, LocationX);
            }
        }
    }

    #endregion
    #region  Toggle Button

    [DefaultEvent("ToggledChanged")]
    public class WC_Linux_Toggle : Control
    {

        #region  Enums

        public enum _Type
        {
            OnOff,
            YesNo,
            IO
        }

        #endregion
        #region  Variables

        public delegate void ToggledChangedEventHandler();
        private ToggledChangedEventHandler ToggledChangedEvent;

        public event ToggledChangedEventHandler ToggledChanged
        {
            add
            {
                ToggledChangedEvent = (ToggledChangedEventHandler)System.Delegate.Combine(ToggledChangedEvent, value);
            }
            remove
            {
                ToggledChangedEvent = (ToggledChangedEventHandler)System.Delegate.Remove(ToggledChangedEvent, value);
            }
        }

        private bool _Toggled;
        private _Type ToggleType;
        private Rectangle Bar;
        private Size cHandle = new Size(15, 20);

        #endregion
        #region  Properties

        public bool Toggled
        {
            get
            {
                return _Toggled;
            }
            set
            {
                _Toggled = value;
                Invalidate();
                if (ToggledChangedEvent != null)
                    ToggledChangedEvent();
            }
        }

        public _Type Type
        {
            get
            {
                return ToggleType;
            }
            set
            {
                ToggleType = value;
                Invalidate();
            }
        }

        #endregion
        #region  EventArgs

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Width = 79;
            Height = 27;
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Toggled = !Toggled;
            Focus();
        }

        #endregion

        public WC_Linux_Toggle()
        {
            SetStyle((System.Windows.Forms.ControlStyles)(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint), true);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;

            G.SmoothingMode = SmoothingMode.HighQuality;
            G.Clear(Parent.BackColor);

            int SwitchXLoc = 3;
            Rectangle ControlRectangle = new Rectangle(0, 0, Width - 1, Height - 1);
            GraphicsPath ControlPath = RoundRectangle.RoundRect(ControlRectangle, 4);

            LinearGradientBrush BackgroundLGB = default(LinearGradientBrush);
            if (_Toggled)
            {
                SwitchXLoc = 37;
                BackgroundLGB = new LinearGradientBrush(ControlRectangle, Color.FromArgb(231, 108, 58), Color.FromArgb(236, 113, 63), 90.0F);
            }
            else
            {
                SwitchXLoc = 0;
                BackgroundLGB = new LinearGradientBrush(ControlRectangle, Color.FromArgb(208, 208, 208), Color.FromArgb(226, 226, 226), 90.0F);
            }

            // Fill inside background gradient
            G.FillPath(BackgroundLGB, ControlPath);

            // Draw string
            switch (ToggleType)
            {
                case _Type.OnOff:
                    if (Toggled)
                    {
                        G.DrawString("ON", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.WhiteSmoke, Bar.X + 18, (float)(Bar.Y + 13.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.DrawString("OFF", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.DimGray, Bar.X + 59, (float)(Bar.Y + 13.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    break;
                case _Type.YesNo:
                    if (Toggled)
                    {
                        G.DrawString("YES", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.WhiteSmoke, Bar.X + 18, (float)(Bar.Y + 13.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.DrawString("NO", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.DimGray, Bar.X + 59, (float)(Bar.Y + 13.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    break;
                case _Type.IO:
                    if (Toggled)
                    {
                        G.DrawString("I", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.WhiteSmoke, Bar.X + 18, (float)(Bar.Y + 13.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.DrawString("O", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.DimGray, Bar.X + 59, (float)(Bar.Y + 13.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    break;
            }

            Rectangle SwitchRectangle = new Rectangle(SwitchXLoc, 0, Width - 38, Height);
            GraphicsPath SwitchPath = RoundRectangle.RoundRect(SwitchRectangle, 4);
            LinearGradientBrush SwitchButtonLGB = new LinearGradientBrush(SwitchRectangle, Color.FromArgb(253, 253, 253), Color.FromArgb(240, 238, 237), LinearGradientMode.Vertical);

            // Fill switch background gradient
            G.FillPath(SwitchButtonLGB, SwitchPath);

            // Draw borders
            if (_Toggled == true)
            {
                G.DrawPath(new Pen(Color.FromArgb(185, 89, 55)), SwitchPath);
                G.DrawPath(new Pen(Color.FromArgb(185, 89, 55)), ControlPath);
            }
            else
            {
                G.DrawPath(new Pen(Color.FromArgb(181, 181, 181)), SwitchPath);
                G.DrawPath(new Pen(Color.FromArgb(181, 181, 181)), ControlPath);
            }
        }
    }

    #endregion
    #region CheckBox

    [DefaultEvent("CheckedChanged")]
    class WC_Linux_CheckBox : Control
    {

        #region Variables

        private GraphicsPath Shape;
        private LinearGradientBrush GB;
        private Rectangle R1;
        private Rectangle R2;
        private bool _Checked;
        public event CheckedChangedEventHandler CheckedChanged;
        public delegate void CheckedChangedEventHandler(object sender);

        #endregion
        #region Properties

        public bool Checked
        {
            get { return _Checked; }
            set
            {
                _Checked = value;
                if (CheckedChanged != null)
                {
                    CheckedChanged(this);
                }
                Invalidate();
            }
        }

        #endregion

        public WC_Linux_CheckBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);

            BackColor = Color.Transparent;
            DoubleBuffered = true;
            // Reduce control flicker
            Font = new Font("Segoe UI", 12);
            Size = new Size(171, 26);
        }

        protected override void OnClick(EventArgs e)
        {
            _Checked = !_Checked;
            if (CheckedChanged != null)
            {
                CheckedChanged(this);
            }
            Focus();
            Invalidate();
            base.OnClick(e);
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }

        protected override void OnResize(System.EventArgs e)
        {
            if (Width > 0 && Height > 0)
            {
                Shape = new GraphicsPath();

                R1 = new Rectangle(17, 0, Width, Height + 1);
                R2 = new Rectangle(0, 0, Width, Height);
                GB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(253, 175, 143), Color.FromArgb(244, 146, 106), 90f);

                var MyDrawer = Shape;
                MyDrawer.AddArc(0, 0, 7, 7, 180, 90);
                MyDrawer.AddArc(7, 0, 7, 7, -90, 90);
                MyDrawer.AddArc(7, 7, 7, 7, 0, 90);
                MyDrawer.AddArc(0, 7, 7, 7, 90, 90);
                MyDrawer.CloseAllFigures();
                Height = 15;
            }

            Invalidate();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var MyDrawer = e.Graphics;
            MyDrawer.Clear(Parent.BackColor);
            MyDrawer.SmoothingMode = SmoothingMode.AntiAlias;

            MyDrawer.FillPath(GB, Shape);
            // Fill the body of the CheckBox
            MyDrawer.DrawPath(new Pen(Color.FromArgb(182, 88, 55)), Shape);
            // Draw the border

            MyDrawer.DrawString(Text, Font, new SolidBrush(Color.FromArgb(128,128,128)), new Rectangle(17, 0, Width, Height - 1), new StringFormat { LineAlignment = StringAlignment.Center });

            if (Checked)
            {
                MyDrawer.DrawString("ü", new Font("Wingdings", 12), new SolidBrush(Color.FromArgb(50,50,50)), new Rectangle(-2, 1, Width, Height + 2), new StringFormat { LineAlignment = StringAlignment.Center });
            }
            e.Dispose();
        }
    }

    #endregion
    #region RadioButton

    [DefaultEvent("CheckedChanged")]
    class WC_Linux_RadioButton : Control
    {

        #region Enums

        public enum MouseState : byte
        {
            None = 0,
            Over = 1,
            Down = 2,
            Block = 3
        }

        #endregion
        #region Variables

        private bool _Checked;
        public event CheckedChangedEventHandler CheckedChanged;
        public delegate void CheckedChangedEventHandler(object sender);

        #endregion
        #region Properties

        public bool Checked
        {
            get { return _Checked; }
            set
            {
                _Checked = value;
                InvalidateControls();
                if (CheckedChanged != null)
                {
                    CheckedChanged(this);
                }
                Invalidate();
            }
        }

        #endregion
        #region EventArgs

        protected override void OnTextChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 15;
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (!_Checked)
                Checked = true;
            base.OnMouseDown(e);
            Focus();
        }

        #endregion

        public WC_Linux_RadioButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
            Font = new Font("Segoe UI", 12);
            Width = 193;
        }

        private void InvalidateControls()
        {
            if (!IsHandleCreated || !_Checked)
                return;

            foreach (Control _Control in Parent.Controls)
            {
                if (!object.ReferenceEquals(_Control, this) && _Control is WC_Linux_RadioButton)
                {
                    ((WC_Linux_RadioButton)_Control).Checked = false;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var MyDrawer = e.Graphics;

            MyDrawer.Clear(Parent.BackColor);
            MyDrawer.SmoothingMode = SmoothingMode.AntiAlias;

            // Fill the body of the ellipse with a gradient
            LinearGradientBrush LGB = new LinearGradientBrush(new Rectangle(new Point(0, 0), new Size(14, 14)), Color.FromArgb(213, 85, 32), Color.FromArgb(224, 123, 82), 90);
            MyDrawer.FillEllipse(LGB, new Rectangle(new Point(0, 0), new Size(14, 14)));

            GraphicsPath GP = new GraphicsPath();
            GP.AddEllipse(new Rectangle(0, 0, 14, 14));
            MyDrawer.SetClip(GP);
            MyDrawer.ResetClip();

            // Draw ellipse border
            MyDrawer.DrawEllipse(new Pen(Color.FromArgb(182, 88, 55)), new Rectangle(new Point(0, 0), new Size(14, 14)));

            // Draw an ellipse inside the body
            if (_Checked)
            {
                SolidBrush EllipseColor = new SolidBrush(Color.FromArgb(255, 255, 255));
                MyDrawer.FillEllipse(EllipseColor, new Rectangle(new Point(4, 4), new Size(6, 6)));
            }
            MyDrawer.DrawString(Text, Font, new SolidBrush(Color.FromArgb(76, 76, 95)), 16, 7, new StringFormat { LineAlignment = StringAlignment.Center });
            e.Dispose();
        }
    }

    #endregion
    #region  ComboBox

    public class WC_Linux_ComboBox : ComboBox
    {

        #region  Variables

        private int _StartIndex = 0;
        private Color _HoverSelectionColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.

        #endregion
        #region  Custom Properties

        public int StartIndex
        {
            get
            {
                return _StartIndex;
            }
            set
            {
                _StartIndex = value;
                try
                {
                    base.SelectedIndex = value;
                }
                catch
                {
                }
                Invalidate();
            }
        }

        public Color HoverSelectionColor
        {
            get
            {
                return _HoverSelectionColor;
            }
            set
            {
                _HoverSelectionColor = value;
                Invalidate();
            }
        }

        #endregion
        #region  EventArgs

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            LinearGradientBrush LGB = new LinearGradientBrush(e.Bounds, Color.FromArgb(246, 132, 85), Color.FromArgb(231, 108, 57), 90.0F);

            if (System.Convert.ToInt32((e.State & DrawItemState.Selected)) == (int)DrawItemState.Selected)
            {
                if (!(e.Index == -1))
                {
                    e.Graphics.FillRectangle(LGB, e.Bounds);
                    e.Graphics.DrawString(GetItemText(Items[e.Index]), e.Font, Brushes.WhiteSmoke, e.Bounds);
                }
            }
            else
            {
                if (!(e.Index == -1))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(242, 241, 240)), e.Bounds);
                    e.Graphics.DrawString(GetItemText(Items[e.Index]), e.Font, Brushes.DimGray, e.Bounds);
                }
            }
            LGB.Dispose();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            SuspendLayout();
            Update();
            ResumeLayout();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (!Focused)
            {
                SelectionLength = 0;
            }
        }

        #endregion

        public WC_Linux_ComboBox()
        {
            SetStyle((ControlStyles)(139286), true);
            SetStyle(ControlStyles.Selectable, false);

            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;

            BackColor = Color.FromArgb(246, 246, 246);
            ForeColor = Color.FromArgb(142, 142, 142);
            Size = new Size(135, 26);
            ItemHeight = 20;
            DropDownHeight = 100;
            Font = new Font("Segoe UI", 10, FontStyle.Regular);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            LinearGradientBrush LGB = default(LinearGradientBrush);
            GraphicsPath GP = default(GraphicsPath);

            e.Graphics.Clear(Parent.BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Create a curvy border
            GP = RoundRectangle.RoundRect(0, 0, Width - 1, Height - 1, 5);
            // Fills the body of the rectangle with a gradient
            LGB = new LinearGradientBrush(ClientRectangle, Color.FromArgb(253, 252, 252), Color.FromArgb(239, 237, 236), 90.0F);

            e.Graphics.SetClip(GP);
            e.Graphics.FillRectangle(LGB, ClientRectangle);
            e.Graphics.ResetClip();

            // Draw rectangle border
            e.Graphics.DrawPath(new Pen(Color.FromArgb(180, 180, 180)), GP);
            // Draw string
            e.Graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(76, 76, 97)), new Rectangle(3, 0, Width - 20, Height), new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near
            });
            e.Graphics.DrawString("6", new Font("Marlett", 13, FontStyle.Regular), new SolidBrush(Color.FromArgb(119, 119, 118)), new Rectangle(3, 0, Width - 4, Height), new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Far
            });
            e.Graphics.DrawLine(new Pen(Color.FromArgb(224, 222, 220)), Width - 24, 4, Width - 24, this.Height - 5);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(250, 249, 249)), Width - 25, 4, Width - 25, this.Height - 5);

            GP.Dispose();
            LGB.Dispose();
        }
    }

    #endregion
    #region  NumericUpDown

    public class WC_Linux_NumericUpDown : Control
    {

        #region  Enums

        public enum _TextAlignment
        {
            Near,
            Center
        }

        #endregion
        #region  Variables

        private GraphicsPath Shape;
        private Pen P1;

        private long _Value;
        private long _Minimum;
        private long _Maximum;
        private int Xval;
        private bool KeyboardNum;
        private _TextAlignment MyStringAlignment;

        private Timer LongPressTimer = new Timer();

        #endregion
        #region  Properties

        public long Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (value <= _Maximum & value >= _Minimum)
                {
                    _Value = value;
                }
                Invalidate();
            }
        }

        public long Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {
                if (value < _Maximum)
                {
                    _Minimum = value;
                }
                if (_Value < _Minimum)
                {
                    _Value = Minimum;
                }
                Invalidate();
            }
        }

        public long Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                if (value > _Minimum)
                {
                    _Maximum = value;
                }
                if (_Value > _Maximum)
                {
                    _Value = _Maximum;
                }
                Invalidate();
            }
        }

        public _TextAlignment TextAlignment
        {
            get
            {
                return MyStringAlignment;
            }
            set
            {
                MyStringAlignment = value;
                Invalidate();
            }
        }

        #endregion
        #region  EventArgs

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            Height = 28;
            MinimumSize = new Size(93, 28);
            Shape = new GraphicsPath();
            Shape.AddArc(0, 0, 10, 10, 180, 90);
            Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            Shape.CloseAllFigures();
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Xval = e.Location.X;
            Invalidate();

            if (e.X < Width - 50)
            {
                Cursor = Cursors.IBeam;
            }
            else
            {
                Cursor = Cursors.Default;
            }
            if (e.X > this.Width - 25 && e.X < this.Width - 10)
            {
                Cursor = Cursors.Hand;
            }
            if (e.X > this.Width - 44 && e.X < this.Width - 33)
            {
                Cursor = Cursors.Hand;
            }
        }

        private void ClickButton()
        {
            if (Xval > this.Width - 25 && Xval < this.Width - 10)
            {
                if ((Value + 1) <= _Maximum)
                {
                    _Value++;
                }
            }
            else
            {
                if (Xval > this.Width - 44 && Xval < this.Width - 33)
                {
                    if ((Value - 1) >= _Minimum)
                    {
                        _Value--;
                    }
                }
                KeyboardNum = !KeyboardNum;
            }
            Focus();
            Invalidate();
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseClick(e);
            ClickButton();
            LongPressTimer.Start();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            LongPressTimer.Stop();
        }
        private void LongPressTimer_Tick(object sender, EventArgs e)
        {
            ClickButton();
        }
        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            try
            {
                if (KeyboardNum == true)
                {
                    _Value = long.Parse((_Value).ToString() + e.KeyChar.ToString().ToString());
                }
                if (_Value > _Maximum)
                {
                    _Value = _Maximum;
                }
            }
            catch (Exception)
            {
            }
        }

        protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Back)
            {
                string TemporaryValue = _Value.ToString();
                TemporaryValue = TemporaryValue.Remove(Convert.ToInt32(TemporaryValue.Length - 1));
                if (TemporaryValue.Length == 0)
                {
                    TemporaryValue = "0";
                }
                _Value = Convert.ToInt32(TemporaryValue);
            }
            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
            {
                if ((Value + 1) <= _Maximum)
                {
                    _Value++;
                }
                Invalidate();
            }
            else
            {
                if ((Value - 1) >= _Minimum)
                {
                    _Value--;
                }
                Invalidate();
            }
        }

        #endregion

        public WC_Linux_NumericUpDown()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            P1 = new Pen(Color.FromArgb(180, 180, 180));
            BackColor = Color.Transparent;
            ForeColor = Color.FromArgb(76, 76, 76);
            _Minimum = 0;
            _Maximum = 100;
            Font = new Font("Tahoma", 11);
            Size = new Size(70, 28);
            MinimumSize = new Size(62, 28);
            DoubleBuffered = true;

            LongPressTimer.Tick += LongPressTimer_Tick;
            LongPressTimer.Interval = 300;
        }

        public void Increment(int Value)
        {
            this._Value += Value;
            Invalidate();
        }

        public void Decrement(int Value)
        {
            this._Value -= Value;
            Invalidate();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);
            LinearGradientBrush BackgroundLGB = default(LinearGradientBrush);

            BackgroundLGB = new LinearGradientBrush(ClientRectangle, Color.FromArgb(246, 246, 246), Color.FromArgb(254, 254, 254), 90.0F);

            G.SmoothingMode = SmoothingMode.AntiAlias;

            G.Clear(Color.Transparent); // Set control background color
            G.FillPath(BackgroundLGB, Shape); // Draw background
            G.DrawPath(P1, Shape); // Draw border

            G.DrawString("+", new Font("Tahoma", 14), new SolidBrush(Color.FromArgb(75, 75, 75)), new Rectangle(Width - 25, 1, 19, 30));
            G.DrawLine(new Pen(Color.FromArgb(229, 228, 227)), Width - 28, 1, Width - 28, this.Height - 2);
            G.DrawString("-", new Font("Tahoma", 14), new SolidBrush(Color.FromArgb(75, 75, 75)), new Rectangle(Width - 44, 1, 19, 30));
            G.DrawLine(new Pen(Color.FromArgb(229, 228, 227)), Width - 48, 1, Width - 48, this.Height - 2);

            switch (MyStringAlignment)
            {
                case _TextAlignment.Near:
                    G.DrawString(System.Convert.ToString(Value), Font, new SolidBrush(ForeColor), new Rectangle(5, 0, Width - 1, Height - 1), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    break;
                case _TextAlignment.Center:
                    G.DrawString(System.Convert.ToString(Value), Font, new SolidBrush(ForeColor), new Rectangle(0, 0, Width - 1, Height - 1), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    break;
            }
            e.Graphics.DrawImage((Image)(B.Clone()), 0, 0);
            G.Dispose();
            B.Dispose();
        }
    }

    #endregion
    #region  TrackBar

    [DefaultEvent("ValueChanged")]
    public class WC_Linux_TrackBar : Control
    {

        #region  Enums

        public enum ValueDivisor
        {
            By1 = 1,
            By10 = 10,
            By100 = 100,
            By1000 = 1000
        }

        #endregion
        #region  Variables

        private GraphicsPath PipeBorder;
        private GraphicsPath FillValue;
        private Rectangle TrackBarHandleRect;
        private bool Cap;
        private int ValueDrawer;

        private Size ThumbSize = new Size(15, 15);
        private Rectangle TrackThumb;

        private int _Minimum = 0;
        private int _Maximum = 10;
        private int _Value = 0;

        private bool _DrawValueString = false;
        private bool _JumpToMouse = false;
        private ValueDivisor DividedValue = ValueDivisor.By1;

        #endregion
        #region  Properties

        public int Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {

                if (value >= _Maximum)
                {
                    value = _Maximum - 10;
                }
                if (_Value < value)
                {
                    _Value = value;
                }

                _Minimum = value;
                Invalidate();
            }
        }

        public int Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {

                if (value <= _Minimum)
                {
                    value = _Minimum + 10;
                }
                if (_Value > value)
                {
                    _Value = value;
                }

                _Maximum = value;
                Invalidate();
            }
        }

        public delegate void ValueChangedEventHandler();
        private ValueChangedEventHandler ValueChangedEvent;

        public event ValueChangedEventHandler ValueChanged
        {
            add
            {
                ValueChangedEvent = (ValueChangedEventHandler)System.Delegate.Combine(ValueChangedEvent, value);
            }
            remove
            {
                ValueChangedEvent = (ValueChangedEventHandler)System.Delegate.Remove(ValueChangedEvent, value);
            }
        }

        public int Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_Value != value)
                {
                    if (value < _Minimum)
                    {
                        _Value = _Minimum;
                    }
                    else
                    {
                        if (value > _Maximum)
                        {
                            _Value = _Maximum;
                        }
                        else
                        {
                            _Value = value;
                        }
                    }
                    Invalidate();
                    if (ValueChangedEvent != null)
                        ValueChangedEvent();
                }
            }
        }

        public ValueDivisor ValueDivison
        {
            get
            {
                return DividedValue;
            }
            set
            {
                DividedValue = value;
                Invalidate();
            }
        }

        [Browsable(false)]
        public float ValueToSet
        {
            get
            {
                return _Value / (int)DividedValue;
            }
            set
            {
                Value = (int)(value * (int)DividedValue);
            }
        }

        public bool JumpToMouse
        {
            get
            {
                return _JumpToMouse;
            }
            set
            {
                _JumpToMouse = value;
                Invalidate();
            }
        }

        public bool DrawValueString
        {
            get
            {
                return _DrawValueString;
            }
            set
            {
                _DrawValueString = value;
                if (_DrawValueString == true)
                {
                    Height = 35;
                }
                else
                {
                    Height = 22;
                }
                Invalidate();
            }
        }

        #endregion
        #region  EventArgs

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            checked
            {
                bool flag = this.Cap && e.X > -1 && e.X < this.Width + 1;
                if (flag)
                {
                    this.Value = this._Minimum + (int)Math.Round((double)(this._Maximum - this._Minimum) * ((double)e.X / (double)this.Width));
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            bool flag = e.Button == MouseButtons.Left;
            checked
            {
                if (flag)
                {
                    this.ValueDrawer = (int)Math.Round(((double)(this._Value - this._Minimum) / (double)(this._Maximum - this._Minimum)) * (double)(this.Width - 11));
                    this.TrackBarHandleRect = new Rectangle(this.ValueDrawer, 0, 25, 25);
                    this.Cap = this.TrackBarHandleRect.Contains(e.Location);
                    this.Focus();
                    flag = this._JumpToMouse;
                    if (flag)
                    {
                        this.Value = this._Minimum + (int)Math.Round((double)(this._Maximum - this._Minimum) * ((double)e.X / (double)this.Width));
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Cap = false;
        }

        #endregion

        public WC_Linux_TrackBar()
        {
            SetStyle((System.Windows.Forms.ControlStyles)(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer), true);

            Size = new Size(80, 22);
            MinimumSize = new Size(47, 22);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_DrawValueString == true)
            {
                Height = 35;
            }
            else
            {
                Height = 22;
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;

            G.Clear(Parent.BackColor);
            G.SmoothingMode = SmoothingMode.AntiAlias;
            TrackThumb = new Rectangle(8, 10, Width - 16, 2);
            PipeBorder = RoundRectangle.RoundRect(1, 8, Width - 3, 5, 2);

            try
            {
                this.ValueDrawer = (int)Math.Round(((double)(this._Value - this._Minimum) / (double)(this._Maximum - this._Minimum)) * (double)(this.Width - 11));
            }
            catch (Exception)
            {
            }

            TrackBarHandleRect = new Rectangle(ValueDrawer, 0, 10, 20);

            G.SetClip(PipeBorder); // Set the clipping region of this Graphics to the specified GraphicsPath
            G.FillPath(new SolidBrush(Color.FromArgb(221, 221, 221)), PipeBorder);
            FillValue = RoundRectangle.RoundRect(1, 8, TrackBarHandleRect.X + TrackBarHandleRect.Width - 4, 5, 2);

            G.ResetClip(); // Reset the clip region of this Graphics to an infinite region

            G.SmoothingMode = SmoothingMode.HighQuality;
            G.DrawPath(new Pen(Color.FromArgb(200, 200, 200)), PipeBorder); // Draw pipe border
            G.FillPath(new SolidBrush(Color.FromArgb(217, 99, 50)), FillValue);

            G.FillEllipse(new SolidBrush(Color.FromArgb(244, 244, 244)), this.TrackThumb.X + (int)Math.Round(unchecked((double)this.TrackThumb.Width * ((double)this.Value / (double)this.Maximum))) - (int)Math.Round((double)this.ThumbSize.Width / 2.0), this.TrackThumb.Y + (int)Math.Round((double)this.TrackThumb.Height / 2.0) - (int)Math.Round((double)this.ThumbSize.Height / 2.0), this.ThumbSize.Width, this.ThumbSize.Height);
            G.DrawEllipse(new Pen(Color.FromArgb(180, 180, 180)), this.TrackThumb.X + (int)Math.Round(unchecked((double)this.TrackThumb.Width * ((double)this.Value / (double)this.Maximum))) - (int)Math.Round((double)this.ThumbSize.Width / 2.0), this.TrackThumb.Y + (int)Math.Round((double)this.TrackThumb.Height / 2.0) - (int)Math.Round((double)this.ThumbSize.Height / 2.0), this.ThumbSize.Width, this.ThumbSize.Height);

            if (_DrawValueString == true)
            {
                G.DrawString(System.Convert.ToString(ValueToSet), Font, Brushes.DimGray, 1, 20);
            }
        }
    }

    #endregion
    #region  Panel

    public class WC_Linux_Panel : ContainerControl
    {
        public WC_Linux_Panel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, false);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics G = e.Graphics;

            this.Font = new Font("Tahoma", 9);
            this.BackColor = Color.White;
            G.SmoothingMode = SmoothingMode.AntiAlias;
            G.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, Width, Height));
            G.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, Width - 1, Height - 1));
            G.DrawRectangle(new Pen(Color.FromArgb(211, 208, 205)), 0, 0, Width - 1, Height - 1);
        }
    }

    #endregion
    #region TextBox

    [DefaultEvent("TextChanged")]
    class WC_Linux_TextBox : Control
    {
        #region Variables

        public TextBox AmbianceTB = new TextBox();
        private GraphicsPath Shape;
        private int _maxchars = 32767;
        private bool _ReadOnly;
        private bool _Multiline;
        private HorizontalAlignment ALNType;
        private bool isPasswordMasked = false;
        private Pen P1;
        private SolidBrush B1;

        #endregion
        #region Properties

        public HorizontalAlignment TextAlignment
        {
            get { return ALNType; }
            set
            {
                ALNType = value;
                Invalidate();
            }
        }
        public int MaxLength
        {
            get { return _maxchars; }
            set
            {
                _maxchars = value;
                AmbianceTB.MaxLength = MaxLength;
                Invalidate();
            }
        }

        public bool UseSystemPasswordChar
        {
            get { return isPasswordMasked; }
            set
            {
                AmbianceTB.UseSystemPasswordChar = UseSystemPasswordChar;
                isPasswordMasked = value;
                Invalidate();
            }
        }
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                _ReadOnly = value;
                if (AmbianceTB != null)
                {
                    AmbianceTB.ReadOnly = value;
                }
            }
        }
        public bool Multiline
        {
            get { return _Multiline; }
            set
            {
                _Multiline = value;
                if (AmbianceTB != null)
                {
                    AmbianceTB.Multiline = value;

                    if (value)
                    {
                        AmbianceTB.Height = Height - 10;
                    }
                    else
                    {
                        Height = AmbianceTB.Height + 10;
                    }
                }
            }
        }

        #endregion
        #region EventArgs

        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            AmbianceTB.Text = Text;
            Invalidate();
        }

        private void OnBaseTextChanged(object s, EventArgs e)
        {
            Text = AmbianceTB.Text;
        }

        protected override void OnForeColorChanged(System.EventArgs e)
        {
            base.OnForeColorChanged(e);
            AmbianceTB.ForeColor = ForeColor;
            Invalidate();
        }

        protected override void OnFontChanged(System.EventArgs e)
        {
            base.OnFontChanged(e);
            AmbianceTB.Font = Font;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        private void _OnKeyDown(object Obj, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                AmbianceTB.SelectAll();
                e.SuppressKeyPress = true;
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                AmbianceTB.Copy();
                e.SuppressKeyPress = true;
            }
        }

        private void _Enter(object Obj, EventArgs e)
        {
            P1 = new Pen(Color.FromArgb(205, 87, 40));
            Refresh();
        }

        private void _Leave(object Obj, EventArgs e)
        {
            P1 = new Pen(Color.FromArgb(180, 180, 180));
            Refresh();
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            if (_Multiline)
            {
                AmbianceTB.Height = Height - 10;
            }
            else
            {
                Height = AmbianceTB.Height + 10;
            }

            Shape = new GraphicsPath();
            var _with1 = Shape;
            _with1.AddArc(0, 0, 10, 10, 180, 90);
            _with1.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _with1.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _with1.AddArc(0, Height - 11, 10, 10, 90, 90);
            _with1.CloseAllFigures();
        }

        protected override void OnGotFocus(System.EventArgs e)
        {
            base.OnGotFocus(e);
            AmbianceTB.Focus();
        }

        #endregion
        public void AddTextBox()
        {
            var _TB = AmbianceTB;
            _TB.Size = new Size(Width - 10, 33);
            _TB.Location = new Point(7, 4);
            _TB.Text = String.Empty;
            _TB.BorderStyle = BorderStyle.None;
            _TB.TextAlign = HorizontalAlignment.Left;
            _TB.Font = new Font("Tahoma", 11);
            _TB.UseSystemPasswordChar = UseSystemPasswordChar;
            _TB.Multiline = false;
            AmbianceTB.KeyDown += _OnKeyDown;
            AmbianceTB.Enter += _Enter;
            AmbianceTB.Leave += _Leave;
            AmbianceTB.TextChanged += OnBaseTextChanged;

        }

        public WC_Linux_TextBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            AddTextBox();
            Controls.Add(AmbianceTB);

            P1 = new Pen(Color.FromArgb(180, 180, 180)); // P1 = Border color
            B1 = new SolidBrush(Color.White); // B1 = Rect Background color
            BackColor = Color.Transparent;
            ForeColor = Color.DimGray;

            Text = null;
            Font = new Font("Tahoma", 11);
            Size = new Size(135, 33);
            DoubleBuffered = true;
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);

            G.SmoothingMode = SmoothingMode.AntiAlias;

            var _TB = AmbianceTB;
            _TB.Width = Width - 10;
            _TB.TextAlign = TextAlignment;
            _TB.UseSystemPasswordChar = UseSystemPasswordChar;

            G.Clear(Color.Transparent);
            G.FillPath(B1, Shape); // Draw background
            G.DrawPath(P1, Shape); // Draw border

            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            G.Dispose();
            B.Dispose();
        }

    }

    #endregion
    #region RichTextBox

    [DefaultEvent("TextChanged")]
    class WC_Linux_RichTextBox : Control
    {

        #region Variables

        public RichTextBox AmbianceRTB = new RichTextBox();
        private bool _ReadOnly;
        private bool _WordWrap;
        private bool _AutoWordSelection;
        private GraphicsPath Shape;
        private Pen P1;

        #endregion
        #region Properties

        public override string Text
        {
            get { return AmbianceRTB.Text; }
            set
            {
                AmbianceRTB.Text = value;
                Invalidate();
            }
        }
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                _ReadOnly = value;
                if (AmbianceRTB != null)
                {
                    AmbianceRTB.ReadOnly = value;
                }
            }
        }
        public bool WordWrap
        {
            get { return _WordWrap; }
            set
            {
                _WordWrap = value;
                if (AmbianceRTB != null)
                {
                    AmbianceRTB.WordWrap = value;
                }
            }
        }
        public bool AutoWordSelection
        {
            get { return _AutoWordSelection; }
            set
            {
                _AutoWordSelection = value;
                if (AmbianceRTB != null)
                {
                    AmbianceRTB.AutoWordSelection = value;
                }
            }
        }
        #endregion
        #region EventArgs

        protected override void OnForeColorChanged(System.EventArgs e)
        {
            base.OnForeColorChanged(e);
            AmbianceRTB.ForeColor = ForeColor;
            Invalidate();
        }

        protected override void OnFontChanged(System.EventArgs e)
        {
            base.OnFontChanged(e);
            AmbianceRTB.Font = Font;
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        protected override void OnSizeChanged(System.EventArgs e)
        {
            base.OnSizeChanged(e);
            AmbianceRTB.Size = new Size(Width - 13, Height - 11);
        }

        private void _Enter(object Obj, EventArgs e)
        {
            P1 = new Pen(Color.FromArgb(205, 87, 40));
            Refresh();
        }

        private void _Leave(object Obj, EventArgs e)
        {
            P1 = new Pen(Color.FromArgb(180, 180, 180));
            Refresh();
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);

            Shape = new GraphicsPath();
            var _Shape = Shape;
            _Shape.AddArc(0, 0, 10, 10, 180, 90);
            _Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            _Shape.CloseAllFigures();
        }

        public void _TextChanged(object s, EventArgs e)
        {
            AmbianceRTB.Text = Text;
        }

        #endregion

        public void AddRichTextBox()
        {
            var _RTB = AmbianceRTB;
            //_RTB.BackColor = Color.White;
            _RTB.Size = new Size(Width - 10, 100);
            _RTB.Location = new Point(7, 5);
            _RTB.Text = string.Empty;
            _RTB.BorderStyle = BorderStyle.None;
            _RTB.Font = new Font("Tahoma", 10);
            _RTB.Multiline = true;
        }

        public WC_Linux_RichTextBox()
            : base()
        {

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            AddRichTextBox();
            Controls.Add(AmbianceRTB);
            BackColor = Color.Transparent;
            ForeColor = Color.FromArgb(76, 76, 76);

            P1 = new Pen(Color.FromArgb(180, 180, 180));
            Text = null;
            Font = new Font("Tahoma", 10);
            Size = new Size(150, 100);
            WordWrap = true;
            AutoWordSelection = false;
            DoubleBuffered = true;

            AmbianceRTB.Enter += _Enter;
            AmbianceRTB.Leave += _Leave;
            TextChanged += _TextChanged;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(this.Width, this.Height);
            Graphics G = Graphics.FromImage(B);
            G.SmoothingMode = SmoothingMode.AntiAlias;
            G.Clear(Color.Transparent);
            G.FillPath(Brushes.White, this.Shape);
            G.DrawPath(P1, this.Shape);
            G.Dispose();
            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            B.Dispose();
        }
    }

    #endregion
    #region  ListBox

    public class WC_Linux_ListBox : ListBox
    {

        public WC_Linux_ListBox()
        {
            this.SetStyle((System.Windows.Forms.ControlStyles)(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint), true);
            this.DrawMode = DrawMode.OwnerDrawFixed;
            IntegralHeight = false;
            ItemHeight = 18;
            Font = new Font("Seoge UI", 11, FontStyle.Regular);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            e.DrawBackground();
            LinearGradientBrush LGB = new LinearGradientBrush(e.Bounds, Color.FromArgb(246, 132, 85), Color.FromArgb(231, 108, 57), 90.0F);
            if (System.Convert.ToInt32((e.State & DrawItemState.Selected)) == (int)DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(LGB, e.Bounds);
            }
            using (SolidBrush b = new SolidBrush(e.ForeColor))
            {
                if (base.Items.Count == 0)
                {
                    return;
                }
                else
                {
                    e.Graphics.DrawString(base.GetItemText(base.Items[e.Index]), e.Font, b, e.Bounds);
                }
            }

            LGB.Dispose();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Region MyRegion = new Region(e.ClipRectangle);
            e.Graphics.FillRegion(new SolidBrush(this.BackColor), MyRegion);

            if (this.Items.Count > 0)
            {
                for (int i = 0; i <= this.Items.Count - 1; i++)
                {
                    System.Drawing.Rectangle RegionRect = this.GetItemRectangle(i);
                    if (e.ClipRectangle.IntersectsWith(RegionRect))
                    {
                        if ((this.SelectionMode == SelectionMode.One && this.SelectedIndex == i) || (this.SelectionMode == SelectionMode.MultiSimple && this.SelectedIndices.Contains(i)) || (this.SelectionMode == SelectionMode.MultiExtended && this.SelectedIndices.Contains(i)))
                        {
                            OnDrawItem(new DrawItemEventArgs(e.Graphics, this.Font, RegionRect, i, DrawItemState.Selected, this.ForeColor, this.BackColor));
                        }
                        else
                        {
                            OnDrawItem(new DrawItemEventArgs(e.Graphics, this.Font, RegionRect, i, DrawItemState.Default, Color.FromArgb(60, 60, 60), this.BackColor));
                        }
                        MyRegion.Complement(RegionRect);
                    }
                }
            }
        }
    }

    #endregion
    #region  TabControl

    public class WC_Linux_TabControl : TabControl
    {

        public WC_Linux_TabControl()
        {
            SetStyle((System.Windows.Forms.ControlStyles)(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint), true);
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();

            ItemSize = new Size(80, 24);
            Alignment = TabAlignment.Top;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics G = e.Graphics;
            Rectangle ItemBoundsRect = new Rectangle();
            G.Clear(Parent.BackColor);
            for (int TabIndex = 0; TabIndex <= TabCount - 1; TabIndex++)
            {
                ItemBoundsRect = GetTabRect(TabIndex);
                if (!(TabIndex == SelectedIndex))
                {
                    G.DrawString(TabPages[TabIndex].Text, new Font(Font.Name, Font.Size - 2, FontStyle.Bold), new SolidBrush(Color.FromArgb(80, 76, 76)), new Rectangle(GetTabRect(TabIndex).Location, GetTabRect(TabIndex).Size), new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    });
                }
            }

            // Draw container rectangle
            G.FillPath(new SolidBrush(Color.FromArgb(247, 246, 246)), RoundRectangle.RoundRect(0, 23, Width - 1, Height - 24, 2));
            G.DrawPath(new Pen(Color.FromArgb(201, 198, 195)), RoundRectangle.RoundRect(0, 23, Width - 1, Height - 24, 2));

            for (int ItemIndex = 0; ItemIndex <= TabCount - 1; ItemIndex++)
            {
                ItemBoundsRect = GetTabRect(ItemIndex);
                if (ItemIndex == SelectedIndex)
                {

                    // Draw header tabs
                    G.DrawPath(new Pen(Color.FromArgb(201, 198, 195)), RoundRectangle.RoundedTopRect(new Rectangle(new Point(ItemBoundsRect.X - 2, ItemBoundsRect.Y - 2), new Size(ItemBoundsRect.Width + 3, ItemBoundsRect.Height)), 7));
                    G.FillPath(new SolidBrush(Color.FromArgb(247, 246, 246)), RoundRectangle.RoundedTopRect(new Rectangle(new Point(ItemBoundsRect.X - 1, ItemBoundsRect.Y - 1), new Size(ItemBoundsRect.Width + 2, ItemBoundsRect.Height)), 7));

                    try
                    {
                        G.DrawString(TabPages[ItemIndex].Text, new Font(Font.Name, Font.Size - 1, FontStyle.Bold), new SolidBrush(Color.FromArgb(80, 76, 76)), new Rectangle(GetTabRect(ItemIndex).Location, GetTabRect(ItemIndex).Size), new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        });
                        TabPages[ItemIndex].BackColor = Color.FromArgb(247, 246, 246);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }

    #endregion

}
#endregion

#region WC_Theme_2
namespace WC_Theme_2
{

    #region  RoundRectangle

    sealed class RoundRectangle
    {
        public static GraphicsPath RoundRect(Rectangle Rectangle, int Curve)
        {
            GraphicsPath P = new GraphicsPath();
            int ArcRectangleWidth = Curve * 2;
            P.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90);
            P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90);
            P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90);
            P.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90);
            P.AddLine(new Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), new Point(Rectangle.X, Curve + Rectangle.Y));
            return P;
        }
    }

    #endregion
    #region ControlBox

    class WC_2_ControlBox : Control
    {

        #region Enums

        public enum ButtonHoverState
        {
            Minimize,
            Maximize,
            Close,
            None
        }

        #endregion
        #region Variables

        private ButtonHoverState ButtonHState = ButtonHoverState.None;

        #endregion
        #region Properties

        private bool _EnableMaximize = true;
        public bool EnableMaximizeButton
        {
            get { return _EnableMaximize; }
            set
            {
                _EnableMaximize = value;
                Invalidate();
            }
        }

        private bool _EnableMinimize = true;
        public bool EnableMinimizeButton
        {
            get { return _EnableMinimize; }
            set
            {
                _EnableMinimize = value;
                Invalidate();
            }
        }

        private bool _EnableHoverHighlight = false;
        public bool EnableHoverHighlight
        {
            get { return _EnableHoverHighlight; }
            set
            {
                _EnableHoverHighlight = value;
                Invalidate();
            }
        }

        #endregion
        #region EventArgs

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Size = new Size(100, 25);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int X = e.Location.X;
            int Y = e.Location.Y;
            if (Y > 0 && Y < (Height - 2))
            {
                if (X > 0 && X < 34)
                {
                    ButtonHState = ButtonHoverState.Minimize;
                }
                else if (X > 33 && X < 65)
                {
                    ButtonHState = ButtonHoverState.Maximize;
                }
                else if (X > 64 && X < Width)
                {
                    ButtonHState = ButtonHoverState.Close;
                }
                else
                {
                    ButtonHState = ButtonHoverState.None;
                }
            }
            else
            {
                ButtonHState = ButtonHoverState.None;
            }
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            switch (ButtonHState)
            {
                case ButtonHoverState.Close:
                    Parent.FindForm().Close();
                    break;
                case ButtonHoverState.Minimize:
                    if (_EnableMinimize == true)
                    {
                        Parent.FindForm().WindowState = FormWindowState.Minimized;
                    }
                    break;
                case ButtonHoverState.Maximize:
                    if (_EnableMaximize == true)
                    {
                        if (Parent.FindForm().WindowState == FormWindowState.Normal)
                        {
                            Parent.FindForm().WindowState = FormWindowState.Maximized;
                        }
                        else
                        {
                            Parent.FindForm().WindowState = FormWindowState.Normal;
                        }
                    }
                    break;
            }
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ButtonHState = ButtonHoverState.None;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
        }

        #endregion

        public WC_2_ControlBox()
            : base()
        {
            DoubleBuffered = true;
            Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            try
            {
                Location = new Point(Parent.Width - 112, 15);
            }
            catch (Exception)
            {
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;
            G.Clear(Color.FromArgb(181, 41, 42));

            if (_EnableHoverHighlight == true)
            {
                switch (ButtonHState)
                {
                    case ButtonHoverState.None:
                        G.Clear(Color.FromArgb(181, 41, 42));
                        break;
                    case ButtonHoverState.Minimize:
                        if (_EnableMinimize == true)
                        {
                            G.FillRectangle(new SolidBrush(Color.FromArgb(156, 35, 35)), new Rectangle(3, 0, 30, Height));
                        }
                        break;
                    case ButtonHoverState.Maximize:
                        if (_EnableMaximize == true)
                        {
                            G.FillRectangle(new SolidBrush(Color.FromArgb(156, 35, 35)), new Rectangle(35, 0, 30, Height));
                        }
                        break;
                    case ButtonHoverState.Close:
                        G.FillRectangle(new SolidBrush(Color.FromArgb(156, 35, 35)), new Rectangle(66, 0, 35, Height));
                        break;
                }
            }

            //Close
            G.DrawString("r", new Font("Marlett", 12), new SolidBrush(Color.FromArgb(255, 254, 255)), new Point(Width - 16, 8), new StringFormat { Alignment = StringAlignment.Center });

            //Maximize
            switch (Parent.FindForm().WindowState)
            {
                case FormWindowState.Maximized:
                    if (_EnableMaximize == true)
                    {
                        G.DrawString("2", new Font("Marlett", 12), new SolidBrush(Color.FromArgb(255, 254, 255)), new Point(51, 7), new StringFormat { Alignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.DrawString("2", new Font("Marlett", 12), new SolidBrush(Color.LightGray), new Point(51, 7), new StringFormat { Alignment = StringAlignment.Center });
                    }
                    break;
                case FormWindowState.Normal:
                    if (_EnableMaximize == true)
                    {
                        G.DrawString("1", new Font("Marlett", 12), new SolidBrush(Color.FromArgb(255, 254, 255)), new Point(51, 7), new StringFormat { Alignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.DrawString("1", new Font("Marlett", 12), new SolidBrush(Color.LightGray), new Point(51, 7), new StringFormat { Alignment = StringAlignment.Center });
                    }
                    break;
            }

            //Minimize
            if (_EnableMinimize == true)
            {
                G.DrawString("0", new Font("Marlett", 12), new SolidBrush(Color.FromArgb(255, 254, 255)), new Point(20, 7), new StringFormat { Alignment = StringAlignment.Center });
            }
            else
            {
                G.DrawString("0", new Font("Marlett", 12), new SolidBrush(Color.LightGray), new Point(20, 7), new StringFormat { Alignment = StringAlignment.Center });
            }
        }
    }

    #endregion
    #region  Button

    public class WC_2_Button : Control
    {

        #region  Variables

        private int MouseState;
        private GraphicsPath Shape;
        private LinearGradientBrush InactiveGB;
        private LinearGradientBrush PressedGB;
        private Rectangle R1;
        private Pen P1;
        private Pen P3;
        private Image _Image;
        private Size _ImageSize;
        private StringAlignment _TextAlignment = StringAlignment.Center;
        private Color _TextColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        private ContentAlignment _ImageAlign = ContentAlignment.MiddleLeft;

        #endregion
        #region  Image Designer

        private static PointF ImageLocation(StringFormat SF, SizeF Area, SizeF ImageArea)
        {
            PointF MyPoint = new PointF();
            switch (SF.Alignment)
            {
                case StringAlignment.Center:
                    MyPoint.X = (float)((Area.Width - ImageArea.Width) / 2);
                    break;
                case StringAlignment.Near:
                    MyPoint.X = 2;
                    break;
                case StringAlignment.Far:
                    MyPoint.X = Area.Width - ImageArea.Width - 2;
                    break;

            }

            switch (SF.LineAlignment)
            {
                case StringAlignment.Center:
                    MyPoint.Y = (float)((Area.Height - ImageArea.Height) / 2);
                    break;
                case StringAlignment.Near:
                    MyPoint.Y = 2;
                    break;
                case StringAlignment.Far:
                    MyPoint.Y = Area.Height - ImageArea.Height - 2;
                    break;
            }
            return MyPoint;
        }

        private StringFormat GetStringFormat(ContentAlignment _ContentAlignment)
        {
            StringFormat SF = new StringFormat();
            switch (_ContentAlignment)
            {
                case ContentAlignment.MiddleCenter:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleLeft:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleRight:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.TopCenter:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.TopLeft:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomRight:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Far;
                    break;
            }
            return SF;
        }

        #endregion
        #region  Properties

        public Image Image
        {
            get
            {
                return _Image;
            }
            set
            {
                if (value == null)
                {
                    _ImageSize = Size.Empty;
                }
                else
                {
                    _ImageSize = value.Size;
                }

                _Image = value;
                Invalidate();
            }
        }

        protected Size ImageSize
        {
            get
            {
                return _ImageSize;
            }
        }

        public ContentAlignment ImageAlign
        {
            get
            {
                return _ImageAlign;
            }
            set
            {
                _ImageAlign = value;
                Invalidate();
            }
        }

        public StringAlignment TextAlignment
        {
            get
            {
                return this._TextAlignment;
            }
            set
            {
                this._TextAlignment = value;
                this.Invalidate();
            }
        }

        public override Color ForeColor
        {
            get
            {
                return this._TextColor;
            }
            set
            {
                this._TextColor = value;
                this.Invalidate();
            }
        }

        #endregion
        #region  EventArgs

        protected override void OnMouseUp(MouseEventArgs e)
        {
            MouseState = 0;
            Invalidate();
            base.OnMouseUp(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            MouseState = 1;
            Focus();
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = 0;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }

        #endregion

        public WC_2_Button()
        {
            SetStyle((System.Windows.Forms.ControlStyles)(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint), true);

            BackColor = Color.Transparent;
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 12);
            ForeColor = Color.FromArgb(255, 255, 255);
            Size = new Size(146, 41);
            _TextAlignment = StringAlignment.Center;
            P1 = new Pen(Color.FromArgb(181, 41, 42)); // P1 = Border color
            P3 = new Pen(Color.FromArgb(165, 37, 37)); // P3 = Border color when pressed
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            if (Width > 0 && Height > 0)
            {

                Shape = new GraphicsPath();
                R1 = new Rectangle(0, 0, Width, Height);

                InactiveGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(181, 41, 42), Color.FromArgb(181, 41, 42), 90.0F);
                PressedGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(165, 37, 37), Color.FromArgb(165, 37, 37), 90.0F);
            }

            Shape.AddArc(0, 0, 10, 10, 180, 90);
            Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            Shape.CloseAllFigures();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
            G.SmoothingMode = SmoothingMode.HighQuality;
            PointF ipt = ImageLocation(GetStringFormat(ImageAlign), Size, ImageSize);

            switch (MouseState)
            {
                case 0:
                    //Inactive
                    G.FillPath(InactiveGB, Shape);
                    // Fill button body with InactiveGB color gradient
                    G.DrawPath(P1, Shape);
                    // Draw button border [InactiveGB]
                    if ((Image == null))
                    {
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
                case 1:
                    //Pressed
                    G.FillPath(PressedGB, Shape);
                    // Fill button body with PressedGB color gradient
                    G.DrawPath(P3, Shape);
                    // Draw button border [PressedGB]

                    if ((Image == null))
                    {
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
            }
            base.OnPaint(e);
        }
    }

    #endregion
    #region  Social Button

    public class WC_2_SocialButton : Control
    {

        #region  Variables

        private Image _Image;
        private Size _ImageSize;
        private Color EllipseColor; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.

        #endregion
        #region  Properties

        public Image Image
        {
            get
            {
                return _Image;
            }
            set
            {
                if (value == null)
                {
                    _ImageSize = Size.Empty;
                }
                else
                {
                    _ImageSize = value.Size;
                }

                _Image = value;
                Invalidate();
            }
        }

        protected Size ImageSize
        {
            get
            {
                return _ImageSize;
            }
        }

        #endregion
        #region  EventArgs

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Size = new Size(54, 54);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            EllipseColor = Color.FromArgb(181, 41, 42);
            Refresh();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            EllipseColor = Color.FromArgb(66, 76, 85);
            Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            EllipseColor = Color.FromArgb(153, 34, 34);
            Focus();
            Refresh();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            EllipseColor = Color.FromArgb(181, 41, 42);
            Refresh();
        }

        #endregion
        #region  Image Designer

        private static PointF ImageLocation(StringFormat SF, SizeF Area, SizeF ImageArea)
        {
            PointF MyPoint = new PointF();
            switch (SF.Alignment)
            {
                case StringAlignment.Center:
                    MyPoint.X = (float)((Area.Width - ImageArea.Width) / 2);
                    break;
            }

            switch (SF.LineAlignment)
            {
                case StringAlignment.Center:
                    MyPoint.Y = (float)((Area.Height - ImageArea.Height) / 2);
                    break;
            }
            return MyPoint;
        }

        private StringFormat GetStringFormat(ContentAlignment _ContentAlignment)
        {
            StringFormat SF = new StringFormat();
            switch (_ContentAlignment)
            {
                case ContentAlignment.MiddleCenter:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Center;
                    break;
            }
            return SF;
        }

        #endregion

        public WC_2_SocialButton()
        {
            DoubleBuffered = true;
            EllipseColor = Color.FromArgb(66, 76, 85);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics G = e.Graphics;
            G.Clear(Parent.BackColor);
            G.SmoothingMode = SmoothingMode.HighQuality;

            PointF ImgPoint = ImageLocation(GetStringFormat(ContentAlignment.MiddleCenter), Size, ImageSize);
            G.FillEllipse(new SolidBrush(EllipseColor), new Rectangle(0, 0, 53, 53));

            // HINTS:
            // The best size for the drawn image is 32x32\
            // The best matching color of drawn image is (RGB: 31, 40, 49)
            if (Image != null)
            {
                G.DrawImage(_Image, (int)ImgPoint.X, (int)ImgPoint.Y, ImageSize.Width, ImageSize.Height);
            }
        }
    }

    #endregion
    #region  Label

    public class WC_2_Label : Label
    {

        public WC_2_Label()
        {
            Font = new Font("Segoe UI", 9);
            ForeColor = Color.FromArgb(116, 125, 132);
            BackColor = Color.Transparent;
        }
    }

    #endregion
    #region  Link Label
    public class WC_2_LinkLabel : LinkLabel
    {

        public WC_2_LinkLabel()
        {
            Font = new Font("Segoe UI", 9, FontStyle.Regular);
            BackColor = Color.Transparent;
            LinkColor = Color.FromArgb(181, 41, 42);
            ActiveLinkColor = Color.FromArgb(153, 34, 34);
            VisitedLinkColor = Color.FromArgb(181, 41, 42);
            LinkBehavior = LinkBehavior.NeverUnderline;
        }
    }

    #endregion
    #region  Header Label

    public class WC_2_HeaderLabel : Label
    {

        public WC_2_HeaderLabel()
        {
            Font = new Font("Segoe UI", 11, FontStyle.Bold);
            ForeColor = Color.FromArgb(255, 255, 255);
            BackColor = Color.Transparent;
        }
    }

    #endregion
    #region  Toggle Button

    [DefaultEvent("ToggledChanged")]
    public class WC_2_Toggle : Control
    {

        #region  Enums

        public enum _Type
        {
            CheckMark,
            OnOff,
            YesNo,
            IO
        }

        #endregion
        #region  Variables

        public delegate void ToggledChangedEventHandler();
        private ToggledChangedEventHandler ToggledChangedEvent;

        public event ToggledChangedEventHandler ToggledChanged
        {
            add
            {
                ToggledChangedEvent = (ToggledChangedEventHandler)System.Delegate.Combine(ToggledChangedEvent, value);
            }
            remove
            {
                ToggledChangedEvent = (ToggledChangedEventHandler)System.Delegate.Remove(ToggledChangedEvent, value);
            }
        }

        private bool _Toggled;
        private _Type ToggleType;
        private Rectangle Bar;
        private int _Width;
        private int _Height;

        #endregion
        #region  Properties

        public bool Toggled
        {
            get
            {
                return _Toggled;
            }
            set
            {
                _Toggled = value;
                Invalidate();
                if (ToggledChangedEvent != null)
                    ToggledChangedEvent();
            }
        }

        public _Type Type
        {
            get
            {
                return ToggleType;
            }
            set
            {
                ToggleType = value;
                Invalidate();
            }
        }

        #endregion
        #region  EventArgs

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Size = new Size(76, 33);
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Toggled = !Toggled;
            Focus();
        }

        #endregion

        public WC_2_Toggle()
        {
            SetStyle((System.Windows.Forms.ControlStyles)(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint), true);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            System.Drawing.Graphics G = e.Graphics;

            G.SmoothingMode = SmoothingMode.HighQuality;
            G.Clear(Parent.BackColor);
            _Width = Width - 1;
            _Height = Height - 1;

            GraphicsPath GP = default(GraphicsPath);
            GraphicsPath GP2 = new GraphicsPath();
            Rectangle BaseRect = new Rectangle(0, 0, _Width, _Height);
            Rectangle ThumbRect = new Rectangle(_Width / 2, 0, 38, _Height);

            G.SmoothingMode = (System.Drawing.Drawing2D.SmoothingMode)2;
            G.PixelOffsetMode = (System.Drawing.Drawing2D.PixelOffsetMode)2;
            G.TextRenderingHint = (System.Drawing.Text.TextRenderingHint)5;
            G.Clear(BackColor);

            GP = RoundRectangle.RoundRect(BaseRect, 4);
            ThumbRect = new Rectangle(4, 4, 36, _Height - 8);
            GP2 = RoundRectangle.RoundRect(ThumbRect, 4);
            G.FillPath(new SolidBrush(Color.FromArgb(66, 76, 85)), GP);
            G.FillPath(new SolidBrush(Color.FromArgb(32, 41, 50)), GP2);

            if (_Toggled)
            {
                GP = RoundRectangle.RoundRect(BaseRect, 4);
                ThumbRect = new Rectangle((_Width / 2) - 2, 4, 36, _Height - 8);
                GP2 = RoundRectangle.RoundRect(ThumbRect, 4);
                G.FillPath(new SolidBrush(Color.FromArgb(181, 41, 42)), GP);
                G.FillPath(new SolidBrush(Color.FromArgb(32, 41, 50)), GP2);
            }

            // Draw string
            switch (ToggleType)
            {
                case _Type.CheckMark:
                    if (Toggled)
                    {
                        G.DrawString("ü", new Font("Wingdings", 18, FontStyle.Regular), Brushes.WhiteSmoke, Bar.X + 18, Bar.Y + 19, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.DrawString("r", new Font("Marlett", 14, FontStyle.Regular), Brushes.DimGray, Bar.X + 59, Bar.Y + 18, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    break;
                case _Type.OnOff:
                    if (Toggled)
                    {
                        G.DrawString("ON", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.WhiteSmoke, Bar.X + 18, Bar.Y + 16, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.DrawString("OFF", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.DimGray, Bar.X + 57, Bar.Y + 16, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    break;
                case _Type.YesNo:
                    if (Toggled)
                    {
                        G.DrawString("YES", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.WhiteSmoke, Bar.X + 19, Bar.Y + 16, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.DrawString("NO", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.DimGray, Bar.X + 56, Bar.Y + 16, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    break;
                case _Type.IO:
                    if (Toggled)
                    {
                        G.DrawString("I", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.WhiteSmoke, Bar.X + 18, Bar.Y + 16, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        G.DrawString("O", new Font("Segoe UI", 12, FontStyle.Regular), Brushes.DimGray, Bar.X + 57, Bar.Y + 16, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    break;
            }
        }
    }

    #endregion
    #region  CheckBox

    [DefaultEvent("CheckedChanged")]
    public class WC_2_CheckBox : Control
    {

        #region  Variables

        private int X;
        private bool _Checked = false;
        private GraphicsPath Shape;

        #endregion
        #region  Properties

        public bool Checked
        {
            get
            {
                return _Checked;
            }
            set
            {
                _Checked = value;
                Invalidate();
            }
        }

        #endregion
        #region  EventArgs

        public delegate void CheckedChangedEventHandler(object sender);
        private CheckedChangedEventHandler CheckedChangedEvent;

        public event CheckedChangedEventHandler CheckedChanged
        {
            add
            {
                CheckedChangedEvent = (CheckedChangedEventHandler)System.Delegate.Combine(CheckedChangedEvent, value);
            }
            remove
            {
                CheckedChangedEvent = (CheckedChangedEventHandler)System.Delegate.Remove(CheckedChangedEvent, value);
            }
        }


        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            X = e.Location.X;
            Invalidate();
        }
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            _Checked = !_Checked;
            Focus();
            if (CheckedChangedEvent != null)
                CheckedChangedEvent(this);
            base.OnMouseDown(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.Height = 16;

            Shape = new GraphicsPath();
            Shape.AddArc(0, 0, 10, 10, 180, 90);
            Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            Shape.CloseAllFigures();
            Invalidate();
        }

        #endregion

        public WC_2_CheckBox()
        {
            Width = 148;
            Height = 16;
            Font = new Font("Microsoft Sans Serif", 9);
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;
            G.Clear(Parent.BackColor);

            if (_Checked)
            {
                G.FillRectangle(new SolidBrush(Color.FromArgb(66, 76, 85)), new Rectangle(0, 0, 16, 16));
                G.FillRectangle(new SolidBrush(Color.FromArgb(66, 76, 85)), new Rectangle(1, 1, 16 - 2, 16 - 2));
            }
            else
            {
                G.FillRectangle(new SolidBrush(Color.FromArgb(66, 76, 85)), new Rectangle(0, 0, 16, 16));
                G.FillRectangle(new SolidBrush(Color.FromArgb(66, 76, 85)), new Rectangle(1, 1, 16 - 2, 16 - 2));
            }

            if (Enabled == true)
            {
                if (_Checked)
                {
                    G.DrawString("a", new Font("Marlett", 16), new SolidBrush(Color.FromArgb(181, 41, 42)), new Point(-5, -3));
                }
            }
            else
            {
                if (_Checked)
                {
                    G.DrawString("a", new Font("Marlett", 16), new SolidBrush(Color.Gray), new Point(-5, -3));
                }
            }

            G.DrawString(Text, Font, new SolidBrush(Color.FromArgb(116, 125, 132)), new Point(20, 0));
        }
    }
    #endregion
    #region  Radio Button

    [DefaultEvent("CheckedChanged")]
    public class WC_2_RadioButton : Control
    {

        #region  Variables

        private int X;
        private bool _Checked;

        #endregion
        #region  Properties

        public bool Checked
        {
            get
            {
                return _Checked;
            }
            set
            {
                _Checked = value;
                InvalidateControls();
                if (CheckedChangedEvent != null)
                    CheckedChangedEvent(this);
                Invalidate();
            }
        }

        #endregion
        #region  EventArgs

        public delegate void CheckedChangedEventHandler(object sender);
        private CheckedChangedEventHandler CheckedChangedEvent;

        public event CheckedChangedEventHandler CheckedChanged
        {
            add
            {
                CheckedChangedEvent = (CheckedChangedEventHandler)System.Delegate.Combine(CheckedChangedEvent, value);
            }
            remove
            {
                CheckedChangedEvent = (CheckedChangedEventHandler)System.Delegate.Remove(CheckedChangedEvent, value);
            }
        }


        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (!_Checked)
            {
                @Checked = true;
            }
            Focus();
            base.OnMouseDown(e);
        }
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            X = e.X;
            Invalidate();
        }
        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            int textSize = 0;
            textSize = (int)(this.CreateGraphics().MeasureString(Text, Font).Width);
            this.Width = 28 + textSize;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Height = 17;
        }

        #endregion

        public WC_2_RadioButton()
        {
            Width = 159;
            Height = 17;
            DoubleBuffered = true;
        }

        private void InvalidateControls()
        {
            if (!IsHandleCreated || !_Checked)
            {
                return;
            }

            foreach (Control _Control in Parent.Controls)
            {
                if (_Control != this && _Control is WC_2_RadioButton)
                {
                    ((WC_2_RadioButton)_Control).Checked = false;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;
            G.Clear(Parent.BackColor);
            G.SmoothingMode = SmoothingMode.HighQuality;

            G.FillEllipse(new SolidBrush(Color.FromArgb(66, 76, 85)), new Rectangle(0, 0, 16, 16));

            if (_Checked)
            {
                G.DrawString("a", new Font("Marlett", 15), new SolidBrush(Color.FromArgb(181, 41, 42)), new Point(-3, -2));
            }

            G.DrawString(Text, Font, new SolidBrush(Color.FromArgb(116, 125, 132)), new Point(20, 0));
        }
    }

    #endregion
    #region  TextBox

    [DefaultEvent("TextChanged")]
    public class WC_2_TextBox : Control
    {

        #region  Variables

        public TextBox WC_Theme_3TB = new TextBox();
        private int _maxchars = 32767;
        private bool _ReadOnly;
        private bool _Multiline;
        private Image _Image;
        private Size _ImageSize;
        private HorizontalAlignment ALNType;
        private bool isPasswordMasked = false;
        private Pen P1;
        private SolidBrush B1;
        private GraphicsPath Shape;

        #endregion
        #region  Properties

        public HorizontalAlignment TextAlignment
        {
            get
            {
                return ALNType;
            }
            set
            {
                ALNType = value;
                Invalidate();
            }
        }
        public int MaxLength
        {
            get
            {
                return _maxchars;
            }
            set
            {
                _maxchars = value;
                WC_Theme_3TB.MaxLength = MaxLength;
                Invalidate();
            }
        }

        public bool UseSystemPasswordChar
        {
            get
            {
                return isPasswordMasked;
            }
            set
            {
                WC_Theme_3TB.UseSystemPasswordChar = UseSystemPasswordChar;
                isPasswordMasked = value;
                Invalidate();
            }
        }
        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
                if (WC_Theme_3TB != null)
                {
                    WC_Theme_3TB.ReadOnly = value;
                }
            }
        }
        public bool Multiline
        {
            get
            {
                return _Multiline;
            }
            set
            {
                _Multiline = value;
                if (WC_Theme_3TB != null)
                {
                    WC_Theme_3TB.Multiline = value;

                    if (value)
                    {
                        WC_Theme_3TB.Height = Height - 23;
                    }
                    else
                    {
                        Height = WC_Theme_3TB.Height + 23;
                    }
                }
            }
        }

        public Image Image
        {
            get
            {
                return _Image;
            }
            set
            {
                if (value == null)
                {
                    _ImageSize = Size.Empty;
                }
                else
                {
                    _ImageSize = value.Size;
                }

                _Image = value;

                if (Image == null)
                {
                    WC_Theme_3TB.Location = new Point(8, 10);
                }
                else
                {
                    WC_Theme_3TB.Location = new Point(35, 11);
                }
                Invalidate();
            }
        }

        protected Size ImageSize
        {
            get
            {
                return _ImageSize;
            }
        }

        #endregion
        #region  EventArgs

        private void _Enter(object Obj, EventArgs e)
        {
            P1 = new Pen(Color.FromArgb(181, 41, 42));
            Refresh();
        }

        private void _Leave(object Obj, EventArgs e)
        {
            P1 = new Pen(Color.FromArgb(32, 41, 50));
            Refresh();
        }

        private void OnBaseTextChanged(object s, EventArgs e)
        {
            Text = WC_Theme_3TB.Text;
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            WC_Theme_3TB.Text = Text;
            Invalidate();
        }

        protected override void OnForeColorChanged(System.EventArgs e)
        {
            base.OnForeColorChanged(e);
            WC_Theme_3TB.ForeColor = ForeColor;
            Invalidate();
        }

        protected override void OnFontChanged(System.EventArgs e)
        {
            base.OnFontChanged(e);
            WC_Theme_3TB.Font = Font;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        private void _OnKeyDown(object Obj, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                WC_Theme_3TB.SelectAll();
                e.SuppressKeyPress = true;
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                WC_Theme_3TB.Copy();
                e.SuppressKeyPress = true;
            }
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            if (_Multiline)
            {
                WC_Theme_3TB.Height = Height - 23;
            }
            else
            {
                Height = WC_Theme_3TB.Height + 23;
            }

            Shape = new GraphicsPath();
            Shape.AddArc(0, 0, 10, 10, 180, 90);
            Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            Shape.CloseAllFigures();
        }

        protected override void OnGotFocus(System.EventArgs e)
        {
            base.OnGotFocus(e);
            WC_Theme_3TB.Focus();
        }

        public void _TextChanged(System.Object sender, System.EventArgs e)
        {
            Text = WC_Theme_3TB.Text;
        }

        public void _BaseTextChanged(System.Object sender, System.EventArgs e)
        {
            WC_Theme_3TB.Text = Text;
        }

        #endregion

        public void AddTextBox()
        {
            WC_Theme_3TB.Location = new Point(8, 10);
            WC_Theme_3TB.Text = String.Empty;
            WC_Theme_3TB.BorderStyle = BorderStyle.None;
            WC_Theme_3TB.TextAlign = HorizontalAlignment.Left;
            WC_Theme_3TB.Font = new Font("Tahoma", 11);
            WC_Theme_3TB.UseSystemPasswordChar = UseSystemPasswordChar;
            WC_Theme_3TB.Multiline = false;
            WC_Theme_3TB.BackColor = Color.FromArgb(66, 76, 85);
            WC_Theme_3TB.ScrollBars = ScrollBars.None;
            WC_Theme_3TB.KeyDown += _OnKeyDown;
            WC_Theme_3TB.Enter += _Enter;
            WC_Theme_3TB.Leave += _Leave;
            WC_Theme_3TB.TextChanged += OnBaseTextChanged;
        }

        public WC_2_TextBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            AddTextBox();
            Controls.Add(WC_Theme_3TB);

            P1 = new Pen(Color.FromArgb(32, 41, 50));
            B1 = new SolidBrush(Color.FromArgb(66, 76, 85));
            BackColor = Color.Transparent;
            ForeColor = Color.FromArgb(176, 183, 191);

            Text = null;
            Font = new Font("Tahoma", 11);
            Size = new Size(135, 43);
            DoubleBuffered = true;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);

            G.SmoothingMode = SmoothingMode.AntiAlias;


            if (Image == null)
            {
                WC_Theme_3TB.Width = Width - 18;
            }
            else
            {
                WC_Theme_3TB.Width = Width - 45;
            }

            WC_Theme_3TB.TextAlign = TextAlignment;
            WC_Theme_3TB.UseSystemPasswordChar = UseSystemPasswordChar;

            G.Clear(Color.Transparent);

            G.FillPath(B1, Shape);
            G.DrawPath(P1, Shape);

            if (Image != null)
            {
                G.DrawImage(_Image, 5, 8, 24, 24);
                // 24x24 is the perfect size of the image
            }

            e.Graphics.DrawImage((Image)(B.Clone()), 0, 0);
            G.Dispose();
            B.Dispose();
        }
    }

    #endregion
    #region  Panel

    public class WC_2_Panel : ContainerControl
    {

        private GraphicsPath Shape;

        public WC_2_Panel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            BackColor = Color.FromArgb(39, 51, 63);
            this.Size = new Size(187, 117);
            Padding = new Padding(5, 5, 5, 5);
            DoubleBuffered = true;
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);

            Shape = new GraphicsPath();
            Shape.AddArc(0, 0, 10, 10, 180, 90);
            Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            Shape.CloseAllFigures();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            var G = Graphics.FromImage(B);

            G.SmoothingMode = SmoothingMode.HighQuality;

            G.Clear(Color.FromArgb(32, 41, 50)); // Set control background to transparent
            G.FillPath(new SolidBrush(Color.FromArgb(39, 51, 63)), Shape); // Draw RTB background
            G.DrawPath(new Pen(Color.FromArgb(39, 51, 63)), Shape); // Draw border

            G.Dispose();
            e.Graphics.DrawImage((Image)(B.Clone()), 0, 0);
            B.Dispose();
        }
    }

    #endregion
    #region  Separator

    public class WC_2_Separator : Control
    {

        public WC_2_Separator()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            this.Size = (System.Drawing.Size)(new Point(120, 10));
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(45, 57, 68)), 0, 5, Width, 5);
        }
    }

    #endregion
    #region  TrackBar

    [DefaultEvent("ValueChanged")]
    public class WC_2_TrackBar : Control
    {

        #region  Enums

        public enum ValueDivisor
        {
            By1 = 1,
            By10 = 10,
            By100 = 100,
            By1000 = 1000
        }

        #endregion
        #region  Variables

        private Rectangle FillValue;
        private Rectangle PipeBorder;
        private Rectangle TrackBarHandleRect;
        private bool Cap;
        private int ValueDrawer;

        private Size ThumbSize = new Size(14, 14);
        private Rectangle TrackThumb;

        private int _Minimum = 0;
        private int _Maximum = 10;
        private int _Value = 0;

        private bool _JumpToMouse = false;
        private ValueDivisor DividedValue = ValueDivisor.By1;

        #endregion
        #region  Properties

        public int Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {

                if (value >= _Maximum)
                {
                    value = _Maximum - 10;
                }
                if (_Value < value)
                {
                    _Value = value;
                }

                _Minimum = value;
                Invalidate();
            }
        }

        public int Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {

                if (value <= _Minimum)
                {
                    value = _Minimum + 10;
                }
                if (_Value > value)
                {
                    _Value = value;
                }

                _Maximum = value;
                Invalidate();
            }
        }

        public delegate void ValueChangedEventHandler();
        private ValueChangedEventHandler ValueChangedEvent;

        public event ValueChangedEventHandler ValueChanged
        {
            add
            {
                ValueChangedEvent = (ValueChangedEventHandler)System.Delegate.Combine(ValueChangedEvent, value);
            }
            remove
            {
                ValueChangedEvent = (ValueChangedEventHandler)System.Delegate.Remove(ValueChangedEvent, value);
            }
        }

        public int Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_Value != value)
                {
                    if (value < _Minimum)
                    {
                        _Value = _Minimum;
                    }
                    else
                    {
                        if (value > _Maximum)
                        {
                            _Value = _Maximum;
                        }
                        else
                        {
                            _Value = value;
                        }
                    }
                    Invalidate();
                    if (ValueChangedEvent != null)
                        ValueChangedEvent();
                }
            }
        }

        public ValueDivisor ValueDivison
        {
            get
            {
                return DividedValue;
            }
            set
            {
                DividedValue = value;
                Invalidate();
            }
        }

        [Browsable(false)]
        public float ValueToSet
        {
            get
            {
                return _Value / (int)DividedValue;
            }
            set
            {
                Value = (int)(value * (int)DividedValue);
            }
        }

        public bool JumpToMouse
        {
            get
            {
                return _JumpToMouse;
            }
            set
            {
                _JumpToMouse = value;
                Invalidate();
            }
        }

        #endregion
        #region  EventArgs

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            checked
            {
                bool flag = this.Cap && e.X > -1 && e.X < this.Width + 1;
                if (flag)
                {
                    this.Value = this._Minimum + (int)Math.Round((double)(this._Maximum - this._Minimum) * ((double)e.X / (double)this.Width));
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.ValueDrawer = (int)Math.Round(((double)(this._Value - this._Minimum) / (double)(this._Maximum - this._Minimum)) * (double)(this.Width - 11));
                TrackBarHandleRect = new Rectangle(ValueDrawer, 0, 25, 25);
                Cap = TrackBarHandleRect.Contains(e.Location);
                Focus();
                if (_JumpToMouse)
                {
                    this.Value = this._Minimum + (int)Math.Round((double)(this._Maximum - this._Minimum) * ((double)e.X / (double)this.Width));
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Cap = false;
        }

        #endregion

        public WC_2_TrackBar()
        {
            SetStyle((System.Windows.Forms.ControlStyles)(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer), true);

            Size = new Size(80, 22);
            MinimumSize = new Size(47, 22);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 22;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;

            G.Clear(Parent.BackColor);
            G.SmoothingMode = SmoothingMode.AntiAlias;
            TrackThumb = new Rectangle(7, 10, Width - 16, 2);
            PipeBorder = new Rectangle(1, 10, Width - 3, 2);

            try
            {
                this.ValueDrawer = (int)Math.Round(((double)(this._Value - this._Minimum) / (double)(this._Maximum - this._Minimum)) * (double)(this.Width));
            }
            catch (Exception)
            {
            }

            TrackBarHandleRect = new Rectangle(ValueDrawer, 0, 3, 20);

            G.FillRectangle(new SolidBrush(Color.FromArgb(124, 131, 137)), PipeBorder);
            FillValue = new Rectangle(0, 10, TrackBarHandleRect.X + TrackBarHandleRect.Width - 4, 3);

            G.ResetClip();

            G.SmoothingMode = SmoothingMode.Default;
            G.DrawRectangle(new Pen(Color.FromArgb(124, 131, 137)), PipeBorder); // Draw pipe border
            G.FillRectangle(new SolidBrush(Color.FromArgb(181, 41, 42)), FillValue);

            G.ResetClip();

            G.SmoothingMode = SmoothingMode.HighQuality;

            G.FillEllipse(new SolidBrush(Color.FromArgb(181, 41, 42)), this.TrackThumb.X + (int)Math.Round(unchecked((double)this.TrackThumb.Width * ((double)this.Value / (double)this.Maximum))) - (int)Math.Round((double)this.ThumbSize.Width / 2.0), this.TrackThumb.Y + (int)Math.Round((double)this.TrackThumb.Height / 2.0) - (int)Math.Round((double)this.ThumbSize.Height / 2.0), this.ThumbSize.Width, this.ThumbSize.Height);
            G.DrawEllipse(new Pen(Color.FromArgb(181, 41, 42)), this.TrackThumb.X + (int)Math.Round(unchecked((double)this.TrackThumb.Width * ((double)this.Value / (double)this.Maximum))) - (int)Math.Round((double)this.ThumbSize.Width / 2.0), this.TrackThumb.Y + (int)Math.Round((double)this.TrackThumb.Height / 2.0) - (int)Math.Round((double)this.ThumbSize.Height / 2.0), this.ThumbSize.Width, this.ThumbSize.Height);
        }
    }

    #endregion
    #region  NotificationBox

    public class WC_2_NotificationBox : Control
    {

        #region  Variables

        private Point CloseCoordinates;
        private bool IsOverClose;
        private int _BorderCurve = 8;
        private GraphicsPath CreateRoundPath;
        private string NotificationText = null;
        private Type _NotificationType;
        private bool _RoundedCorners;
        private bool _ShowCloseButton;
        private Image _Image;
        private Size _ImageSize;

        #endregion
        #region  Enums

        // Create a list of Notification Types
        public enum Type
        {
            @Notice,
            @Success,
            @Warning,
            @Error
        }

        #endregion
        #region  Custom Properties

        // Create a NotificationType property and add the Type enum to it
        public Type NotificationType
        {
            get
            {
                return _NotificationType;
            }
            set
            {
                _NotificationType = value;
                Invalidate();
            }
        }
        // Boolean value to determine whether the control should use border radius
        public bool RoundCorners
        {
            get
            {
                return _RoundedCorners;
            }
            set
            {
                _RoundedCorners = value;
                Invalidate();
            }
        }
        // Boolean value to determine whether the control should draw the close button
        public bool ShowCloseButton
        {
            get
            {
                return _ShowCloseButton;
            }
            set
            {
                _ShowCloseButton = value;
                Invalidate();
            }
        }
        // Integer value to determine the curve level of the borders
        public int BorderCurve
        {
            get
            {
                return _BorderCurve;
            }
            set
            {
                _BorderCurve = value;
                Invalidate();
            }
        }
        // Image value to determine whether the control should draw an image before the header
        public Image Image
        {
            get
            {
                return _Image;
            }
            set
            {
                if (value == null)
                {
                    _ImageSize = Size.Empty;
                }
                else
                {
                    _ImageSize = value.Size;
                }

                _Image = value;
                Invalidate();
            }
        }
        // Size value - returns the image size
        protected Size ImageSize
        {
            get
            {
                return _ImageSize;
            }
        }

        #endregion
        #region  EventArgs

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // Decides the location of the drawn ellipse. If mouse is over the correct coordinates, "IsOverClose" boolean will be triggered to draw the ellipse
            if (e.X >= Width - 19 && e.X <= Width - 10 && e.Y > CloseCoordinates.Y && e.Y < CloseCoordinates.Y + 12)
            {
                IsOverClose = true;
            }
            else
            {
                IsOverClose = false;
            }
            // Updates the control
            Invalidate();
        }
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);

            // Disposes the control when the close button is clicked
            if (_ShowCloseButton == true)
            {
                if (IsOverClose)
                {
                    Dispose();
                }
            }
        }

        #endregion

        internal GraphicsPath CreateRoundRect(Rectangle r, int curve)
        {
            // Draw a border radius
            try
            {
                CreateRoundPath = new GraphicsPath(FillMode.Winding);
                CreateRoundPath.AddArc(r.X, r.Y, curve, curve, 180.0F, 90.0F);
                CreateRoundPath.AddArc(r.Right - curve, r.Y, curve, curve, 270.0F, 90.0F);
                CreateRoundPath.AddArc(r.Right - curve, r.Bottom - curve, curve, curve, 0.0F, 90.0F);
                CreateRoundPath.AddArc(r.X, r.Bottom - curve, curve, curve, 90.0F, 90.0F);
                CreateRoundPath.CloseFigure();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine + "Value must be either \'1\' or higher", "Invalid Integer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Return to the default border curve if the parameter is less than "1"
                _BorderCurve = 8;
                BorderCurve = 8;
            }
            return CreateRoundPath;
        }

        public WC_2_NotificationBox()
        {
            SetStyle((System.Windows.Forms.ControlStyles)(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw), true);

            Font = new Font("Tahoma", 9);
            this.MinimumSize = new Size(100, 40);
            RoundCorners = false;
            ShowCloseButton = true;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            // Declare Graphics to draw the control
            Graphics GFX = e.Graphics;
            // Declare Color to paint the control's Text, Background and Border
            Color ForeColor = new Color();
            Color BackgroundColor = new Color();
            Color BorderColor = new Color();
            // Determine the header Notification Type font
            Font TypeFont = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
            // Decalre a new rectangle to draw the control inside it
            Rectangle MainRectangle = new Rectangle(0, 0, Width - 1, Height - 1);
            // Declare a GraphicsPath to create a border radius
            GraphicsPath CrvBorderPath = CreateRoundRect(MainRectangle, _BorderCurve);

            GFX.SmoothingMode = SmoothingMode.HighQuality;
            GFX.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            GFX.Clear(Parent.BackColor);

            switch (_NotificationType)
            {
                case Type.Notice:
                    BackgroundColor = Color.FromArgb(111, 177, 199);
                    BorderColor = Color.FromArgb(111, 177, 199);
                    ForeColor = Color.White;
                    break;
                case Type.Success:
                    BackgroundColor = Color.FromArgb(91, 195, 162);
                    BorderColor = Color.FromArgb(91, 195, 162);
                    ForeColor = Color.White;
                    break;
                case Type.Warning:
                    BackgroundColor = Color.FromArgb(254, 209, 108);
                    BorderColor = Color.FromArgb(254, 209, 108);
                    ForeColor = Color.DimGray;
                    break;
                case Type.Error:
                    BackgroundColor = Color.FromArgb(217, 103, 93);
                    BorderColor = Color.FromArgb(217, 103, 93);
                    ForeColor = Color.White;
                    break;
            }

            if (_RoundedCorners == true)
            {
                GFX.FillPath(new SolidBrush(BackgroundColor), CrvBorderPath);
                GFX.DrawPath(new Pen(BorderColor), CrvBorderPath);
            }
            else
            {
                GFX.FillRectangle(new SolidBrush(BackgroundColor), MainRectangle);
                GFX.DrawRectangle(new Pen(BorderColor), MainRectangle);
            }

            switch (_NotificationType)
            {
                case Type.Notice:
                    NotificationText = "NOTICE";
                    break;
                case Type.Success:
                    NotificationText = "SUCCESS";
                    break;
                case Type.Warning:
                    NotificationText = "WARNING";
                    break;
                case Type.Error:
                    NotificationText = "ERROR";
                    break;
            }

            if (Image == null)
            {
                GFX.DrawString(NotificationText, TypeFont, new SolidBrush(ForeColor), new Point(10, 5));
                GFX.DrawString(Text, Font, new SolidBrush(ForeColor), new Rectangle(10, 21, Width - 17, Height - 5));
            }
            else
            {
                GFX.DrawImage(_Image, 12, 4, 16, 16);
                GFX.DrawString(NotificationText, TypeFont, new SolidBrush(ForeColor), new Point(30, 5));
                GFX.DrawString(Text, Font, new SolidBrush(ForeColor), new Rectangle(10, 21, Width - 17, Height - 5));
            }

            CloseCoordinates = new Point(Width - 26, 4);

            if (_ShowCloseButton == true)
            {
                // Draw the close button
                GFX.DrawString("r", new Font("Marlett", 7, FontStyle.Regular), new SolidBrush(Color.FromArgb(130, 130, 130)), new Rectangle(Width - 20, 10, Width, Height), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
            }

            CrvBorderPath.Dispose();
        }
    }

    #endregion

}
#endregion

#region 3vil_Theme_Block
static class ThemeModule
{

    static ThemeModule()
    {
        TextBitmap = new Bitmap(1, 1);
        TextGraphics = Graphics.FromImage(TextBitmap);
    }

    private static Bitmap TextBitmap;

    private static Graphics TextGraphics;
    static internal SizeF MeasureString(string text, Font font)
    {
        return TextGraphics.MeasureString(text, font);
    }

    static internal SizeF MeasureString(string text, Font font, int width)
    {
        return TextGraphics.MeasureString(text, font, width, StringFormat.GenericTypographic);
    }

    private static GraphicsPath CreateRoundPath;

    private static Rectangle CreateRoundRectangle;
    static internal GraphicsPath CreateRound(int x, int y, int width, int height, int slope)
    {
        CreateRoundRectangle = new Rectangle(x, y, width, height);
        return CreateRound(CreateRoundRectangle, slope);
    }

    static internal GraphicsPath CreateRound(Rectangle r, int slope)
    {
        CreateRoundPath = new GraphicsPath(FillMode.Winding);
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0f, 90f);
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90f, 90f);
        CreateRoundPath.CloseFigure();
        return CreateRoundPath;
    }

}

#region 3vil_Theme
class __________3vil_Theme : ThemeContainer154
{

    private int _AccentOffset = 0;
    public int AccentOffset
    {
        get { return _AccentOffset; }
        set
        {
            _AccentOffset = value;
            Invalidate();
        }
    }

    public __________3vil_Theme()
    {
        Header = 30;
        BackColor = Color.FromArgb(50, 50, 50);

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(60, 60, 60));

        B1 = new SolidBrush(Color.FromArgb(50, 50, 50));
    }


    protected override void ColorHook()
    {
    }


    private Rectangle R1;
    private Pen P1;
    private Pen P2;

    private SolidBrush B1;

    private int Pad;
    protected override void PaintHook()
    {
        G.Clear(BackColor);
        DrawBorders(P2, 1);

        G.DrawLine(P1, 0, 26, Width, 26);
        G.DrawLine(P2, 0, 25, Width, 25);

        Pad = Math.Max(Measure().Width + 20, 80);
        R1 = new Rectangle(Pad, 17, Width - (Pad * 2) + _AccentOffset, 8);

        G.DrawRectangle(P2, R1);
        G.DrawRectangle(P1, R1.X + 1, R1.Y + 1, R1.Width - 2, R1.Height);

        G.DrawLine(P1, 0, 29, Width, 29);
        G.DrawLine(P2, 0, 30, Width, 30);

        DrawText(Brushes.Black, HorizontalAlignment.Left, 8, 1);
        DrawText(Brushes.White, HorizontalAlignment.Left, 7, 0);

        G.FillRectangle(B1, 0, 27, Width, 2);
        DrawBorders(Pens.Black);
    }

}
#endregion
#region 3vil_Button
class _3vil_Button : Control
{

    public _3vil_Button()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(65, 65, 65));
    }


    private bool IsMouseDown;
    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private SizeF SZ1;

    private PointF PT1;
    private Pen P1;

    private Pen P2;
    private PathGradientBrush PB1;

    private LinearGradientBrush GB1;

    private Graphics G;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        if (IsMouseDown)
        {
            PB1 = new PathGradientBrush(GP1);
            PB1.CenterColor = Color.FromArgb(60, 60, 60);
            PB1.SurroundColors = new Color[] { Color.FromArgb(55, 55, 55) };
            PB1.FocusScales = new PointF(0.8f, 0.5f);

            G.FillPath(PB1, GP1);
        }
        else
        {
            GB1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(60, 60, 60), Color.FromArgb(55, 55, 55), 90f);
            G.FillPath(GB1, GP1);
        }

        G.DrawPath(P1, GP1);
        G.DrawPath(P2, GP2);

        SZ1 = G.MeasureString(Text, Font);
        PT1 = new PointF(5, Height / 2 - SZ1.Height / 2);

        if (IsMouseDown)
        {
            PT1.X += 1f;
            PT1.Y += 1f;
        }

        G.DrawString(Text, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
        G.DrawString(Text, Font, Brushes.White, PT1);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        IsMouseDown = true;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        IsMouseDown = false;
        Invalidate();
    }

}
#endregion
#region 3vil_ProgressBar
class _3vil_ProgressBar : Control
{

    private int _Minimum;
    public int Minimum
    {
        get { return _Minimum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Minimum = value;
            if (value > _Value)
                _Value = value;
            if (value > _Maximum)
                _Maximum = value;
            Invalidate();
        }
    }

    private int _Maximum = 100;
    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Maximum = value;
            if (value < _Value)
                _Value = value;
            if (value < _Minimum)
                _Minimum = value;
            Invalidate();
        }
    }

    private int _Value;
    public int Value
    {
        get { return _Value; }
        set
        {
            if (value > _Maximum || value < _Minimum)
            {
                throw new Exception("Property value is not valid.");
            }

            _Value = value;
            Invalidate();
        }
    }

    private void Increment(int amount)
    {
        Value += amount;
    }

    public _3vil_ProgressBar()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(55, 55, 55));
        B1 = new SolidBrush(Color.FromArgb(200, 160, 0));
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;

    private GraphicsPath GP3;
    private Rectangle R1;

    private Rectangle R2;
    private Pen P1;
    private Pen P2;
    private SolidBrush B1;
    private LinearGradientBrush GB1;

    private LinearGradientBrush GB2;

    private int I1;
    private Graphics G;

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        R1 = new Rectangle(0, 2, Width - 1, Height - 1);
        GB1 = new LinearGradientBrush(R1, Color.FromArgb(45, 45, 45), Color.FromArgb(50, 50, 50), 90f);

        G.SetClip(GP1);
        G.FillRectangle(GB1, R1);

        I1 = Convert.ToInt32((_Value - _Minimum) / (_Maximum - _Minimum) * (Width - 3));

        if (I1 > 1)
        {
            GP3 = ThemeModule.CreateRound(1, 1, I1, Height - 3, 7);

            R2 = new Rectangle(1, 1, I1, Height - 3);
            GB2 = new LinearGradientBrush(R2, Color.FromArgb(205, 150, 0), Color.FromArgb(150, 110, 0), 90f);

            G.FillPath(GB2, GP3);
            G.DrawPath(P1, GP3);

            G.SetClip(GP3);
            G.SmoothingMode = SmoothingMode.None;

            G.FillRectangle(B1, R2.X, R2.Y + 1, R2.Width, R2.Height / 2);

            G.SmoothingMode = SmoothingMode.AntiAlias;
            G.ResetClip();
        }

        G.DrawPath(P2, GP1);
        G.DrawPath(P1, GP2);
    }

}
#endregion
#region 3vil_Label
class _3vil_Label : Control
{

    public _3vil_Label()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Font = new Font("Segoe UI", 11.25f, FontStyle.Bold);

        B1 = new SolidBrush(Color.FromArgb(205, 150, 0));
    }

    private string _Value1 = "NET";
    public string Value1
    {
        get { return _Value1; }
        set
        {
            _Value1 = value;
            Invalidate();
        }
    }

    private string _Value2 = "SEAL";
    public string Value2
    {
        get { return _Value2; }
        set
        {
            _Value2 = value;
            Invalidate();
        }
    }


    private SolidBrush B1;
    private PointF PT1;
    private PointF PT2;
    private SizeF SZ1;

    private SizeF SZ2;
    private Graphics G;

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);

        SZ1 = G.MeasureString(Value1, Font, Width, StringFormat.GenericTypographic);
        SZ2 = G.MeasureString(Value2, Font, Width, StringFormat.GenericTypographic);

        PT1 = new PointF(0, Height / 2 - SZ1.Height / 2);
        PT2 = new PointF(SZ1.Width + 1, Height / 2 - SZ1.Height / 2);

        G.DrawString(Value1, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
        G.DrawString(Value1, Font, Brushes.White, PT1);

        G.DrawString(Value2, Font, Brushes.Black, PT2.X + 1, PT2.Y + 1);
        G.DrawString(Value2, Font, B1, PT2);
    }

}
#endregion
#region 3vil_TextBox
[DefaultEvent("TextChanged")]
class _3vil_TextBox : Control
{

    private HorizontalAlignment _TextAlign = HorizontalAlignment.Left;
    public HorizontalAlignment TextAlign
    {
        get { return _TextAlign; }
        set
        {
            _TextAlign = value;
            if (Base != null)
            {
                Base.TextAlign = value;
            }
        }
    }

    private int _MaxLength = 32767;
    public int MaxLength
    {
        get { return _MaxLength; }
        set
        {
            _MaxLength = value;
            if (Base != null)
            {
                Base.MaxLength = value;
            }
        }
    }

    private bool _ReadOnly;
    public bool ReadOnly
    {
        get { return _ReadOnly; }
        set
        {
            _ReadOnly = value;
            if (Base != null)
            {
                Base.ReadOnly = value;
            }
        }
    }

    private bool _UseSystemPasswordChar;
    public bool UseSystemPasswordChar
    {
        get { return _UseSystemPasswordChar; }
        set
        {
            _UseSystemPasswordChar = value;
            if (Base != null)
            {
                Base.UseSystemPasswordChar = value;
            }
        }
    }

    private bool _Multiline;
    public bool Multiline
    {
        get { return _Multiline; }
        set
        {
            _Multiline = value;
            if (Base != null)
            {
                Base.Multiline = value;

                if (value)
                {
                    Base.Height = Height - 11;
                }
                else
                {
                    Height = Base.Height + 11;
                }
            }
        }
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            if (Base != null)
            {
                Base.Text = value;
            }
        }
    }

    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            if (Base != null)
            {
                Base.Font = value;
                Base.Location = new Point(5, 5);
                Base.Width = Width - 8;

                if (!_Multiline)
                {
                    Height = Base.Height + 11;
                }
            }
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        if (!Controls.Contains(Base))
        {
            Controls.Add(Base);
        }

        base.OnHandleCreated(e);
    }

    public TextBox Base;
    public _3vil_TextBox()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, true);

        Cursor = Cursors.IBeam;

        Base = new TextBox();
        Base.Font = Font;
        Base.Text = Text;
        Base.MaxLength = _MaxLength;
        Base.Multiline = _Multiline;
        Base.ReadOnly = _ReadOnly;
        Base.UseSystemPasswordChar = _UseSystemPasswordChar;
        Base.ScrollBars = ScrollBars.None;


        Base.ForeColor = Color.White;
        Base.BackColor = Color.FromArgb(50, 50, 50);

        Base.BorderStyle = BorderStyle.None;

        Base.Location = new Point(5, 5);
        Base.Width = Width - 14;

        if (_Multiline)
        {
            Base.Height = Height - 11;
        }
        else
        {
            Height = Base.Height + 11;
        }

        Base.TextChanged += OnBaseTextChanged;
        Base.KeyDown += OnBaseKeyDown;

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(55, 55, 55));
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private Pen P1;
    private Pen P2;

    private PathGradientBrush PB1;
    private Graphics G;

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        PB1 = new PathGradientBrush(GP1);
        PB1.CenterColor = Color.FromArgb(50, 50, 50);
        PB1.SurroundColors = new Color[] { Color.FromArgb(45, 45, 45) };
        PB1.FocusScales = new PointF(0.9f, 0.5f);

        G.FillPath(PB1, GP1);

        G.DrawPath(P2, GP1);
        G.DrawPath(P1, GP2);
    }

    private void OnBaseTextChanged(object s, EventArgs e)
    {
        Text = Base.Text;
    }

    private void OnBaseKeyDown(object s, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.A)
        {
            Base.SelectAll();
            e.SuppressKeyPress = true;
        }
    }

    protected override void OnResize(EventArgs e)
    {
        Base.Location = new Point(5, 5);

        Base.Width = Width - 10;
        Base.Height = Height - 11;

        base.OnResize(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Base.Focus();
        base.OnMouseDown(e);
    }

    protected override void OnEnter(EventArgs e)
    {
        Base.Focus();
        Invalidate();
        base.OnEnter(e);
    }

    protected override void OnLeave(EventArgs e)
    {
        Invalidate();
        base.OnLeave(e);
    }

}
#endregion
#region 3vil_CheckCox
[DefaultEvent("CheckedChanged")]
class _3vil_CheckBox : Control
{

    public event CheckedChangedEventHandler CheckedChanged;
    public delegate void CheckedChangedEventHandler(object sender);

    public _3vil_CheckBox()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        P11 = new Pen(Color.FromArgb(55, 55, 55));
        P22 = new Pen(Color.FromArgb(35, 35, 35));
        P3 = new Pen(Color.Black, 2f);
        P4 = new Pen(Color.White, 2f);
    }

    private bool _Checked;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            if (CheckedChanged != null)
            {
                CheckedChanged(this);
            }

            Invalidate();
        }
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private SizeF SZ1;

    private PointF PT1;
    private Pen P11;
    private Pen P22;
    private Pen P3;

    private Pen P4;

    private PathGradientBrush PB1;
    private Graphics G;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 2, Height - 5, Height - 5, 5);
        GP2 = ThemeModule.CreateRound(1, 3, Height - 7, Height - 7, 5);

        PB1 = new PathGradientBrush(GP1);
        PB1.CenterColor = Color.FromArgb(50, 50, 50);
        PB1.SurroundColors = new Color[] { Color.FromArgb(45, 45, 45) };
        PB1.FocusScales = new PointF(0.3f, 0.3f);

        G.FillPath(PB1, GP1);
        G.DrawPath(P11, GP1);
        G.DrawPath(P22, GP2);

        if (_Checked)
        {
            G.DrawLine(P3, 5, Height - 9, 8, Height - 7);
            G.DrawLine(P3, 7, Height - 7, Height - 8, 7);

            G.DrawLine(P4, 4, Height - 10, 7, Height - 8);
            G.DrawLine(P4, 6, Height - 8, Height - 9, 6);
        }

        SZ1 = G.MeasureString(Text, Font);
        PT1 = new PointF(Height - 3, Height / 2 - SZ1.Height / 2);

        G.DrawString(Text, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
        G.DrawString(Text, Font, Brushes.White, PT1);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Checked = !Checked;
    }

}
#endregion
#region 3vil_RadioButton
[DefaultEvent("CheckedChanged")]
class _3vil_RadioButton : Control
{

    public event CheckedChangedEventHandler CheckedChanged;
    public delegate void CheckedChangedEventHandler(object sender);

    public _3vil_RadioButton()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        P1 = new Pen(Color.FromArgb(55, 55, 55));
        P2 = new Pen(Color.FromArgb(35, 35, 35));
    }

    private bool _Checked;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;

            if (_Checked)
            {
                InvalidateParent();
            }

            if (CheckedChanged != null)
            {
                CheckedChanged(this);
            }
            Invalidate();
        }
    }

    private void InvalidateParent()
    {
        if (Parent == null)
            return;

        foreach (Control C in Parent.Controls)
        {
            if ((!object.ReferenceEquals(C, this)) && (C is _3vil_RadioButton))
            {
                ((_3vil_RadioButton)C).Checked = false;
            }
        }
    }


    private GraphicsPath GP1;
    private SizeF SZ1;

    private PointF PT1;
    private Pen P1;

    private Pen P2;

    private PathGradientBrush PB1;
    private Graphics G;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = new GraphicsPath();
        GP1.AddEllipse(0, 2, Height - 5, Height - 5);

        PB1 = new PathGradientBrush(GP1);
        PB1.CenterColor = Color.FromArgb(50, 50, 50);
        PB1.SurroundColors = new Color[] { Color.FromArgb(45, 45, 45) };
        PB1.FocusScales = new PointF(0.3f, 0.3f);

        G.FillPath(PB1, GP1);

        G.DrawEllipse(P1, 0, 2, Height - 5, Height - 5);
        G.DrawEllipse(P2, 1, 3, Height - 7, Height - 7);

        if (_Checked)
        {
            G.FillEllipse(Brushes.Black, 6, 8, Height - 15, Height - 15);
            G.FillEllipse(Brushes.White, 5, 7, Height - 15, Height - 15);
        }

        SZ1 = G.MeasureString(Text, Font);
        PT1 = new PointF(Height - 3, Height / 2 - SZ1.Height / 2);

        G.DrawString(Text, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
        G.DrawString(Text, Font, Brushes.White, PT1);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Checked = true;
        base.OnMouseDown(e);
    }

}
#endregion
#region 3vil_ComboBox
class _3vil_ComboBox : ComboBox
{

    public _3vil_ComboBox()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        DropDownStyle = ComboBoxStyle.DropDownList;

        BackColor = Color.FromArgb(50, 50, 50);
        ForeColor = Color.White;

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.White, 2f);
        P3 = new Pen(Brushes.Black, 2f);
        P4 = new Pen(Color.FromArgb(65, 65, 65));

        B1 = new SolidBrush(Color.FromArgb(65, 65, 65));
        B2 = new SolidBrush(Color.FromArgb(55, 55, 55));
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private SizeF SZ1;

    private PointF PT1;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private Pen P4;
    private SolidBrush B1;

    private SolidBrush B2;

    private LinearGradientBrush GB1;
    private Graphics G;
    protected override void OnPaint(PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        GB1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(60, 60, 60), Color.FromArgb(55, 55, 55), 90f);
        G.SetClip(GP1);
        G.FillRectangle(GB1, ClientRectangle);
        G.ResetClip();

        G.DrawPath(P1, GP1);
        G.DrawPath(P4, GP2);

        SZ1 = G.MeasureString(Text, Font);
        PT1 = new PointF(5, Height / 2 - SZ1.Height / 2);

        G.DrawString(Text, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
        G.DrawString(Text, Font, Brushes.White, PT1);

        G.DrawLine(P3, Width - 15, 10, Width - 11, 13);
        G.DrawLine(P3, Width - 7, 10, Width - 11, 13);
        G.DrawLine(Pens.Black, Width - 11, 13, Width - 11, 14);

        G.DrawLine(P2, Width - 16, 9, Width - 12, 12);
        G.DrawLine(P2, Width - 8, 9, Width - 12, 12);
        G.DrawLine(Pens.White, Width - 12, 12, Width - 12, 13);

        G.DrawLine(P1, Width - 22, 0, Width - 22, Height);
        G.DrawLine(P4, Width - 23, 1, Width - 23, Height - 2);
        G.DrawLine(P4, Width - 21, 1, Width - 21, Height - 2);
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
        {
            e.Graphics.FillRectangle(B1, e.Bounds);
        }
        else
        {
            e.Graphics.FillRectangle(B2, e.Bounds);
        }

        if (!(e.Index == -1))
        {
            e.Graphics.DrawString(GetItemText(Items[e.Index]), e.Font, Brushes.White, e.Bounds);
        }
    }

}
#endregion
#region 3vil_TabControl
class _3vil_TabControl : TabControl
{

    public _3vil_TabControl()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        SizeMode = TabSizeMode.Fixed;
        Alignment = TabAlignment.Left;
        ItemSize = new Size(28, 115);

        DrawMode = TabDrawMode.OwnerDrawFixed;

        P1 = new Pen(Color.FromArgb(55, 55, 55));
        P2 = new Pen(Color.FromArgb(35, 35, 35));
        P3 = new Pen(Color.FromArgb(45, 45, 45), 2);

        B1 = new SolidBrush(Color.FromArgb(50, 50, 50));
        B2 = new SolidBrush(Color.FromArgb(35, 35, 35));
        B3 = new SolidBrush(Color.FromArgb(205, 150, 0));
        B4 = new SolidBrush(Color.FromArgb(65, 65, 65));

        SF1 = new StringFormat();
        SF1.LineAlignment = StringAlignment.Center;
    }

    protected override void OnControlAdded(ControlEventArgs e)
    {
        if (e.Control is TabPage)
        {
            e.Control.BackColor = Color.FromArgb(50, 50, 50);
        }

        base.OnControlAdded(e);
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;
    private GraphicsPath GP3;

    private GraphicsPath GP4;
    private Rectangle R1;

    private Rectangle R2;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private SolidBrush B1;
    private SolidBrush B2;
    private SolidBrush B3;

    private SolidBrush B4;

    private PathGradientBrush PB1;
    private TabPage TP1;

    private StringFormat SF1;
    private int Offset;

    private int ItemHeight;
    private Graphics G;

    protected override void OnPaint(PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(Color.FromArgb(50, 50, 50));
        G.SmoothingMode = SmoothingMode.AntiAlias;

        ItemHeight = ItemSize.Height + 2;

        GP1 = ThemeModule.CreateRound(0, 0, ItemHeight + 3, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, ItemHeight + 3, Height - 3, 7);

        PB1 = new PathGradientBrush(GP1);
        PB1.CenterColor = Color.FromArgb(50, 50, 50);
        PB1.SurroundColors = new Color[] { Color.FromArgb(45, 45, 45) };
        PB1.FocusScales = new PointF(0.8f, 0.95f);

        G.FillPath(PB1, GP1);

        G.DrawPath(P1, GP1);
        G.DrawPath(P2, GP2);

        for (int I = 0; I <= TabCount - 1; I++)
        {
            R1 = GetTabRect(I);
            R1.Y += 2;
            R1.Height -= 3;
            R1.Width += 1;
            R1.X -= 1;

            TP1 = TabPages[I];
            Offset = 0;

            if (SelectedIndex == I)
            {
                G.FillRectangle(B1, R1);

                for (int J = 0; J <= 1; J++)
                {
                    G.FillRectangle(B2, R1.X + 5 + (J * 5), R1.Y + 6, 2, R1.Height - 9);

                    G.SmoothingMode = SmoothingMode.None;
                    G.FillRectangle(B3, R1.X + 5 + (J * 5), R1.Y + 5, 2, R1.Height - 9);
                    G.SmoothingMode = SmoothingMode.AntiAlias;

                    Offset += 5;
                }

                G.DrawRectangle(P3, R1.X + 1, R1.Y - 1, R1.Width, R1.Height + 2);
                G.DrawRectangle(P1, R1.X + 1, R1.Y + 1, R1.Width - 2, R1.Height - 2);
                G.DrawRectangle(P2, R1);
            }
            else
            {
                for (int J = 0; J <= 1; J++)
                {
                    G.FillRectangle(B2, R1.X + 5 + (J * 5), R1.Y + 6, 2, R1.Height - 9);

                    G.SmoothingMode = SmoothingMode.None;
                    G.FillRectangle(B4, R1.X + 5 + (J * 5), R1.Y + 5, 2, R1.Height - 9);
                    G.SmoothingMode = SmoothingMode.AntiAlias;

                    Offset += 5;
                }
            }

            R1.X += 5 + Offset;

            R2 = R1;
            R2.Y += 1;
            R2.X += 1;

            G.DrawString(TP1.Text, Font, Brushes.Black, R2, SF1);
            G.DrawString(TP1.Text, Font, Brushes.White, R1, SF1);
        }

        GP3 = ThemeModule.CreateRound(ItemHeight, 0, Width - ItemHeight - 1, Height - 1, 7);
        GP4 = ThemeModule.CreateRound(ItemHeight + 1, 1, Width - ItemHeight - 3, Height - 3, 7);

        G.DrawPath(P2, GP3);
        G.DrawPath(P1, GP4);
    }

}
#endregion
#region 3vil_OnOffBox
[DefaultEvent("CheckedChanged")]
class _3vil_OnOffBox : Control
{

    public event CheckedChangedEventHandler CheckedChanged;
    public delegate void CheckedChangedEventHandler(object sender);

    public _3vil_OnOffBox()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        P1 = new Pen(Color.FromArgb(55, 55, 55));
        P2 = new Pen(Color.FromArgb(35, 35, 35));
        P3 = new Pen(Color.FromArgb(65, 65, 65));

        B1 = new SolidBrush(Color.FromArgb(35, 35, 35));
        B2 = new SolidBrush(Color.FromArgb(85, 85, 85));
        B3 = new SolidBrush(Color.FromArgb(65, 65, 65));
        B4 = new SolidBrush(Color.FromArgb(205, 150, 0));
        B5 = new SolidBrush(Color.FromArgb(40, 40, 40));

        SF1 = new StringFormat();
        SF1.LineAlignment = StringAlignment.Center;
        SF1.Alignment = StringAlignment.Near;

        SF2 = new StringFormat();
        SF2.LineAlignment = StringAlignment.Center;
        SF2.Alignment = StringAlignment.Far;

        Size = new Size(56, 24);
        MinimumSize = Size;
        MaximumSize = Size;
    }

    private bool _Checked;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            if (CheckedChanged != null)
            {
                CheckedChanged(this);
            }

            Invalidate();
        }
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;
    private GraphicsPath GP3;

    private GraphicsPath GP4;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private SolidBrush B1;
    private SolidBrush B2;
    private SolidBrush B3;
    private SolidBrush B4;

    private SolidBrush B5;
    private PathGradientBrush PB1;

    private LinearGradientBrush GB1;
    private Rectangle R1;
    private Rectangle R2;
    private Rectangle R3;
    private StringFormat SF1;

    private StringFormat SF2;

    private int Offset;
    private Graphics G;

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        PB1 = new PathGradientBrush(GP1);
        PB1.CenterColor = Color.FromArgb(50, 50, 50);
        PB1.SurroundColors = new Color[] { Color.FromArgb(45, 45, 45) };
        PB1.FocusScales = new PointF(0.3f, 0.3f);

        G.FillPath(PB1, GP1);
        G.DrawPath(P1, GP1);
        G.DrawPath(P2, GP2);

        R1 = new Rectangle(5, 0, Width - 10, Height + 2);
        R2 = new Rectangle(6, 1, Width - 10, Height + 2);

        R3 = new Rectangle(1, 1, (Width / 2) - 1, Height - 3);

        if (_Checked)
        {
            G.DrawString("On", Font, Brushes.Black, R2, SF1);
            G.DrawString("On", Font, Brushes.White, R1, SF1);

            R3.X += (Width / 2) - 1;
        }
        else
        {
            G.DrawString("Off", Font, B1, R2, SF2);
            G.DrawString("Off", Font, B2, R1, SF2);
        }

        GP3 = ThemeModule.CreateRound(R3, 7);
        GP4 = ThemeModule.CreateRound(R3.X + 1, R3.Y + 1, R3.Width - 2, R3.Height - 2, 7);

        GB1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(60, 60, 60), Color.FromArgb(55, 55, 55), 90f);

        G.FillPath(GB1, GP3);
        G.DrawPath(P2, GP3);
        G.DrawPath(P3, GP4);

        Offset = R3.X + (R3.Width / 2) - 3;

        for (int I = 0; I <= 1; I++)
        {
            if (_Checked)
            {
                G.FillRectangle(B1, Offset + (I * 5), 7, 2, Height - 14);
            }
            else
            {
                G.FillRectangle(B3, Offset + (I * 5), 7, 2, Height - 14);
            }

            G.SmoothingMode = SmoothingMode.None;

            if (_Checked)
            {
                G.FillRectangle(B4, Offset + (I * 5), 7, 2, Height - 14);
            }
            else
            {
                G.FillRectangle(B5, Offset + (I * 5), 7, 2, Height - 14);
            }

            G.SmoothingMode = SmoothingMode.AntiAlias;
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Checked = !Checked;
        base.OnMouseDown(e);
    }

}
#endregion
#region 3vil_ControlButton
class _3vil_ControlButton : Control
{

    public enum Button : byte
    {
        None = 0,
        Minimize = 1,
        MaximizeRestore = 2,
        Close = 3
    }

    private Button _ControlButton = Button.Close;
    public Button ControlButton
    {
        get { return _ControlButton; }
        set
        {
            _ControlButton = value;
            Invalidate();
        }
    }

    public _3vil_ControlButton()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Anchor = AnchorStyles.Top | AnchorStyles.Right;

        Width = 18;
        Height = 20;

        MinimumSize = Size;
        MaximumSize = Size;

        Margin = new Padding(0);
    }

    private Graphics G;
    protected override void OnPaint(PaintEventArgs e)
    {
        G = e.Graphics;
        G.Clear(BackColor);

        switch (_ControlButton)
        {
            case Button.Minimize:
                DrawMinimize(3, 10);
                break;
            case Button.MaximizeRestore:
                if (FindForm().WindowState == FormWindowState.Normal)
                {
                    DrawMaximize(3, 5);
                }
                else
                {
                    DrawRestore(3, 4);
                }
                break;
            case Button.Close:
                DrawClose(4, 5);
                break;
        }
    }

    private void DrawMinimize(int x, int y)
    {
        G.FillRectangle(Brushes.White, x, y, 12, 5);
        G.DrawRectangle(Pens.Black, x, y, 11, 4);
    }

    private void DrawMaximize(int x, int y)
    {
        G.DrawRectangle(new Pen(Color.White, 2), x + 2, y + 2, 8, 6);
        G.DrawRectangle(Pens.Black, x, y, 11, 9);
        G.DrawRectangle(Pens.Black, x + 3, y + 3, 5, 3);
    }

    private void DrawRestore(int x, int y)
    {
        G.FillRectangle(Brushes.White, x + 3, y + 1, 8, 4);
        G.FillRectangle(Brushes.White, x + 7, y + 5, 4, 4);
        G.DrawRectangle(Pens.Black, x + 2, y + 0, 9, 9);

        G.FillRectangle(Brushes.White, x + 1, y + 3, 2, 6);
        G.FillRectangle(Brushes.White, x + 1, y + 9, 8, 2);
        G.DrawRectangle(Pens.Black, x, y + 2, 9, 9);
        G.DrawRectangle(Pens.Black, x + 3, y + 5, 3, 3);
    }

    private GraphicsPath ClosePath;
    private void DrawClose(int x, int y)
    {
        if (ClosePath == null)
        {
            ClosePath = new GraphicsPath();
            ClosePath.AddLine(x + 1, y, x + 3, y);
            ClosePath.AddLine(x + 5, y + 2, x + 7, y);
            ClosePath.AddLine(x + 9, y, x + 10, y + 1);
            ClosePath.AddLine(x + 7, y + 4, x + 7, y + 5);
            ClosePath.AddLine(x + 10, y + 8, x + 9, y + 9);
            ClosePath.AddLine(x + 7, y + 9, x + 5, y + 7);
            ClosePath.AddLine(x + 3, y + 9, x + 1, y + 9);
            ClosePath.AddLine(x + 0, y + 8, x + 3, y + 5);
            ClosePath.AddLine(x + 3, y + 4, x + 0, y + 1);
        }

        G.FillPath(Brushes.White, ClosePath);
        G.DrawPath(Pens.Black, ClosePath);
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {

        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            Form F = FindForm();

            switch (_ControlButton)
            {
                case Button.Minimize:
                    F.WindowState = FormWindowState.Minimized;
                    break;
                case Button.MaximizeRestore:
                    if (F.WindowState == FormWindowState.Normal)
                    {
                        F.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        F.WindowState = FormWindowState.Normal;
                    }
                    break;
                case Button.Close:
                    F.Close();
                    break;
            }

        }

        Invalidate();
        base.OnMouseClick(e);
    }

}
#endregion
#region 3vil_GroupBox
class _3vil_GroupBox : ContainerControl
{

    private bool _DrawSeperator;
    public bool DrawSeperator
    {
        get { return _DrawSeperator; }
        set
        {
            _DrawSeperator = value;
            Invalidate();
        }
    }

    private string _Title = "GroupBox";
    public string Title
    {
        get { return _Title; }
        set
        {
            _Title = value;
            Invalidate();
        }
    }

    private string _SubTitle = "Details";
    public string SubTitle
    {
        get { return _SubTitle; }
        set
        {
            _SubTitle = value;
            Invalidate();
        }
    }

    private Font _TitleFont;

    private Font _SubTitleFont;
    public _3vil_GroupBox()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        _TitleFont = new Font("Verdana", 10f);
        _SubTitleFont = new Font("Verdana", 6.5f);

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(55, 55, 55));

        B1 = new SolidBrush(Color.FromArgb(205, 150, 0));
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private PointF PT1;
    private SizeF SZ1;

    private SizeF SZ2;
    private Pen P1;
    private Pen P2;

    private SolidBrush B1;
    private Graphics G;

    protected override void OnPaint(PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        G.DrawPath(P1, GP1);
        G.DrawPath(P2, GP2);

        SZ1 = G.MeasureString(_Title, _TitleFont, Width, StringFormat.GenericTypographic);
        SZ2 = G.MeasureString(_SubTitle, _SubTitleFont, Width, StringFormat.GenericTypographic);

        G.DrawString(_Title, _TitleFont, Brushes.Black, 6, 6);
        G.DrawString(_Title, _TitleFont, B1, 5, 5);

        PT1 = new PointF(6f, SZ1.Height + 4f);

        G.DrawString(_SubTitle, _SubTitleFont, Brushes.Black, PT1.X + 1, PT1.Y + 1);
        G.DrawString(_SubTitle, _SubTitleFont, Brushes.White, PT1.X, PT1.Y);

        if (_DrawSeperator)
        {
            int Y = Convert.ToInt32(PT1.Y + SZ2.Height) + 8;

            G.DrawLine(P1, 4, Y, Width - 5, Y);
            G.DrawLine(P2, 4, Y + 1, Width - 5, Y + 1);
        }
    }

}
#endregion
#region 3vil_Seperator
class _3vil_Seperator : Control
{

    public _3vil_Seperator()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Height = 10;

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(55, 55, 55));
    }

    private Pen P1;

    private Pen P2;
    private Graphics G;

    protected override void OnPaint(PaintEventArgs e)
    {
        G = e.Graphics;
        G.Clear(BackColor);

        G.DrawLine(P1, 0, 5, Width, 5);
        G.DrawLine(P2, 0, 6, Width, 6);
    }

}
#endregion
#region 3vil_TrackBar
[DefaultEvent("Scroll")]
class _3vil_TrackBar : Control
{

    public event ScrollEventHandler Scroll;
    public delegate void ScrollEventHandler(object sender);

    private int _Minimum;
    public int Minimum
    {
        get { return _Minimum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Minimum = value;
            if (value > _Value)
                _Value = value;
            if (value > _Maximum)
                _Maximum = value;
            Invalidate();
        }
    }

    private int _Maximum = 10;
    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Maximum = value;
            if (value < _Value)
                _Value = value;
            if (value < _Minimum)
                _Minimum = value;
            Invalidate();
        }
    }

    private int _Value;
    public int Value
    {
        get { return _Value; }
        set
        {
            if (value == _Value)
                return;

            if (value > _Maximum || value < _Minimum)
            {
                throw new Exception("Property value is not valid.");
            }

            _Value = value;
            Invalidate();

            if (Scroll != null)
            {
                Scroll(this);
            }
        }
    }

    public _3vil_TrackBar()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Height = 17;

        P1 = new Pen(Color.FromArgb(150, 110, 0), 2);
        P2 = new Pen(Color.FromArgb(55, 55, 55));
        P3 = new Pen(Color.FromArgb(35, 35, 35));
        P4 = new Pen(Color.FromArgb(65, 65, 65));
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;
    private GraphicsPath GP3;

    private GraphicsPath GP4;
    private Rectangle R1;
    private Rectangle R2;
    private Rectangle R3;

    private int I1;
    private Pen P1;
    private Pen P2;
    private Pen P3;

    private Pen P4;
    private LinearGradientBrush GB1;
    private LinearGradientBrush GB2;

    private LinearGradientBrush GB3;
    private Graphics G;

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 5, Width - 1, 10, 5);
        GP2 = ThemeModule.CreateRound(1, 6, Width - 3, 8, 5);

        R1 = new Rectangle(0, 7, Width - 1, 5);
        GB1 = new LinearGradientBrush(R1, Color.FromArgb(45, 45, 45), Color.FromArgb(50, 50, 50), 90f);

        I1 = Convert.ToInt32((double)(_Value - _Minimum) / (double)(_Maximum - _Minimum) * (Width - 11));
        R2 = new Rectangle(I1, 0, 10, 20);

        G.SetClip(GP2);
        G.FillRectangle(GB1, R1);

        R3 = new Rectangle(1, 7, R2.X + R2.Width - 2, 8);
        GB2 = new LinearGradientBrush(R3, Color.FromArgb(205, 150, 0), Color.FromArgb(150, 110, 0), 90f);

        G.SmoothingMode = SmoothingMode.None;
        G.FillRectangle(GB2, R3);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        for (int I = 0; I <= R3.Width - 15; I += 5)
        {
            G.DrawLine(P1, I, 0, I + 15, Height);
        }

        G.ResetClip();

        G.DrawPath(P2, GP1);
        G.DrawPath(P3, GP2);

        GP3 = ThemeModule.CreateRound(R2, 5);
        GP4 = ThemeModule.CreateRound(R2.X + 1, R2.Y + 1, R2.Width - 2, R2.Height - 2, 5);
        GB3 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(60, 60, 60), Color.FromArgb(55, 55, 55), 90f);

        G.FillPath(GB3, GP3);
        G.DrawPath(P3, GP3);
        G.DrawPath(P4, GP4);
    }

    private bool TrackDown;
    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            I1 = Convert.ToInt32((double)(_Value - _Minimum) / (double)(_Maximum - _Minimum) * (Width - 11));
            R2 = new Rectangle(I1, 0, 10, 20);

            TrackDown = R2.Contains(e.Location);
        }

        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (TrackDown && e.X > -1 && e.X < (Width + 1))
        {
            Value = _Minimum + Convert.ToInt32((_Maximum - _Minimum) * ((double)e.X / (double)Width));
        }

        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        TrackDown = false;
        base.OnMouseUp(e);
    }

}
#endregion
#region 3vil_RandomPool
[DefaultEvent("ValueChanged")]
class _3vil_RandomPool : Control
{

    public event ValueChangedEventHandler ValueChanged;
    public delegate void ValueChangedEventHandler(object sender);

    private StringBuilder _Value = new StringBuilder();
    public string Value
    {
        get { return _Value.ToString(); }
    }

    public string FullValue
    {
        get { return BitConverter.ToString(Table).Replace("-", ""); }
    }


    private Random RNG = new Random();
    private int ItemSize = 9;

    private int DrawSize = 8;

    private Rectangle WA;
    private int RowSize;

    private int ColumnSize;
    public _3vil_RandomPool()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        P1 = new Pen(Color.FromArgb(55, 55, 55));
        P2 = new Pen(Color.FromArgb(35, 35, 35));

        B1 = new SolidBrush(Color.FromArgb(30, 30, 30));
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        UpdateTable();
        base.OnHandleCreated(e);
    }

    private byte[] Table;
    private void UpdateTable()
    {
        WA = new Rectangle(5, 5, Width - 10, Height - 10);

        RowSize = WA.Width / ItemSize;
        ColumnSize = WA.Height / ItemSize;

        WA.Width = RowSize * ItemSize;
        WA.Height = ColumnSize * ItemSize;

        WA.X = (Width / 2) - (WA.Width / 2);
        WA.Y = (Height / 2) - (WA.Height / 2);

        Table = new byte[(RowSize * ColumnSize)];

        for (int I = 0; I <= Table.Length - 1; I++)
        {
            Table[I] = Convert.ToByte(RNG.Next(100));
        }

        Invalidate();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        UpdateTable();
    }

    private int Index1 = -1;

    private int Index2;

    private bool InvertColors;
    protected override void OnMouseMove(MouseEventArgs e)
    {
        HandleDraw(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        HandleDraw(e);
        base.OnMouseDown(e);
    }

    private void HandleDraw(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left || e.Button == System.Windows.Forms.MouseButtons.Right)
        {
            if (!WA.Contains(e.Location))
                return;

            InvertColors = (e.Button == System.Windows.Forms.MouseButtons.Right);

            Index1 = GetIndex(e.X, e.Y);
            if (Index1 == Index2)
                return;

            bool L = !(Index1 % RowSize == 0);
            bool R = !(Index1 % RowSize == (RowSize - 1));

            Randomize(Index1 - RowSize);
            if (L)
                Randomize(Index1 - 1);
            Randomize(Index1);
            if (R)
                Randomize(Index1 + 1);
            Randomize(Index1 + RowSize);

            _Value.Append(Table[Index1].ToString("X"));
            if (_Value.Length > 32)
                _Value.Remove(0, 2);

            if (ValueChanged != null)
            {
                ValueChanged(this);
            }

            Index2 = Index1;
            Invalidate();
        }
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private Pen P1;
    private Pen P2;
    private SolidBrush B1;

    private SolidBrush B2;

    private PathGradientBrush PB1;
    private Graphics G;

    protected override void OnPaint(PaintEventArgs e)
    {
        G = e.Graphics;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        PB1 = new PathGradientBrush(GP1);
        PB1.CenterColor = Color.FromArgb(50, 50, 50);
        PB1.SurroundColors = new Color[] { Color.FromArgb(45, 45, 45) };
        PB1.FocusScales = new PointF(0.9f, 0.5f);

        G.FillPath(PB1, GP1);

        G.DrawPath(P1, GP1);
        G.DrawPath(P2, GP2);

        G.SmoothingMode = SmoothingMode.None;

        for (int I = 0; I <= Table.Length - 1; I++)
        {
            int C = Math.Max(Table[I], (byte)75);

            int X = ((I % RowSize) * ItemSize) + WA.X;
            int Y = ((I / RowSize) * ItemSize) + WA.Y;

            B2 = new SolidBrush(Color.FromArgb(C, C, C));

            G.FillRectangle(B1, X + 1, Y + 1, DrawSize, DrawSize);
            G.FillRectangle(B2, X, Y, DrawSize, DrawSize);

            B2.Dispose();
        }

    }

    private int GetIndex(int x, int y)
    {
        return (((y - WA.Y) / ItemSize) * RowSize) + ((x - WA.X) / ItemSize);
    }

    private void Randomize(int index)
    {
        if (index > -1 && index < Table.Length)
        {
            if (InvertColors)
            {
                Table[index] = Convert.ToByte(RNG.Next(100));
            }
            else
            {
                Table[index] = Convert.ToByte(RNG.Next(100, 256));
            }
        }
    }

}
#endregion
#region 3vil_Keyboard
class _3vil_Keyboard : Control
{

    private Bitmap TextBitmap;

    private Graphics TextGraphics;
    const string LowerKeys = "1234567890-=qwertyuiop[]asdfghjkl\\;'zxcvbnm,./`";

    const string UpperKeys = "!@#$%^&*()_+QWERTYUIOP{}ASDFGHJKL|:\"ZXCVBNM<>?~";
    public _3vil_Keyboard()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Font = new Font("Verdana", 8.25f);

        TextBitmap = new Bitmap(1, 1);
        TextGraphics = Graphics.FromImage(TextBitmap);

        MinimumSize = new Size(386, 162);
        MaximumSize = new Size(386, 162);

        Lower = LowerKeys.ToCharArray();
        Upper = UpperKeys.ToCharArray();

        PrepareCache();

        P1 = new Pen(Color.FromArgb(45, 45, 45));
        P2 = new Pen(Color.FromArgb(65, 65, 65));
        P3 = new Pen(Color.FromArgb(35, 35, 35));

        B1 = new SolidBrush(Color.FromArgb(100, 100, 100));
    }

    private Control _Target;
    public Control Target
    {
        get { return _Target; }
        set { _Target = value; }
    }


    private bool Shift;
    private int Pressed = -1;

    private Rectangle[] Buttons;
    private char[] Lower;
    private char[] Upper;
    private string[] Other = {
		"Shift",
		"Space",
		"Back"

	};
    private PointF[] UpperCache;

    private PointF[] LowerCache;
    private void PrepareCache()
    {
        Buttons = new Rectangle[51];
        UpperCache = new PointF[Upper.Length];
        LowerCache = new PointF[Lower.Length];

        int I = 0;

        SizeF S = default(SizeF);
        Rectangle R = default(Rectangle);

        for (int Y = 0; Y <= 3; Y++)
        {
            for (int X = 0; X <= 11; X++)
            {
                I = (Y * 12) + X;
                R = new Rectangle(X * 32, Y * 32, 32, 32);

                Buttons[I] = R;

                if (!(I == 47) && !char.IsLetter(Upper[I]))
                {
                    S = TextGraphics.MeasureString(Upper[I].ToString(), Font);
                    UpperCache[I] = new PointF(R.X + (R.Width / 2 - S.Width / 2), R.Y + R.Height - S.Height - 2);

                    S = TextGraphics.MeasureString(Lower[I].ToString(), Font);
                    LowerCache[I] = new PointF(R.X + (R.Width / 2 - S.Width / 2), R.Y + R.Height - S.Height - 2);
                }
            }
        }

        Buttons[48] = new Rectangle(0, 4 * 32, 2 * 32, 32);
        Buttons[49] = new Rectangle(Buttons[48].Right, 4 * 32, 8 * 32, 32);
        Buttons[50] = new Rectangle(Buttons[49].Right, 4 * 32, 2 * 32, 32);
    }


    private GraphicsPath GP1;
    private SizeF SZ1;

    private PointF PT1;
    private Pen P1;
    private Pen P2;
    private Pen P3;

    private SolidBrush B1;
    private PathGradientBrush PB1;

    private LinearGradientBrush GB1;
    private Graphics G;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);

        Rectangle R = default(Rectangle);

        int Offset = 0;
        G.DrawRectangle(P1, 0, 0, (12 * 32) + 1, (5 * 32) + 1);

        for (int I = 0; I <= Buttons.Length - 1; I++)
        {
            R = Buttons[I];

            Offset = 0;
            if (I == Pressed)
            {
                Offset = 1;

                GP1 = new GraphicsPath();
                GP1.AddRectangle(R);

                PB1 = new PathGradientBrush(GP1);
                PB1.CenterColor = Color.FromArgb(60, 60, 60);
                PB1.SurroundColors = new Color[] { Color.FromArgb(55, 55, 55) };
                PB1.FocusScales = new PointF(0.8f, 0.5f);

                G.FillPath(PB1, GP1);
            }
            else
            {
                GB1 = new LinearGradientBrush(R, Color.FromArgb(60, 60, 60), Color.FromArgb(55, 55, 55), 90f);
                G.FillRectangle(GB1, R);
            }

            switch (I)
            {
                case 48:
                case 49:
                case 50:
                    SZ1 = G.MeasureString(Other[I - 48], Font);
                    G.DrawString(Other[I - 48], Font, Brushes.Black, R.X + (R.Width / 2 - SZ1.Width / 2) + Offset + 1, R.Y + (R.Height / 2 - SZ1.Height / 2) + Offset + 1);
                    G.DrawString(Other[I - 48], Font, Brushes.White, R.X + (R.Width / 2 - SZ1.Width / 2) + Offset, R.Y + (R.Height / 2 - SZ1.Height / 2) + Offset);
                    break;
                case 47:
                    DrawArrow(Color.Black, R.X + Offset + 1, R.Y + Offset + 1);
                    DrawArrow(Color.White, R.X + Offset, R.Y + Offset);
                    break;
                default:
                    if (Shift)
                    {
                        G.DrawString(Upper[I].ToString(), Font, Brushes.Black, R.X + 3 + Offset + 1, R.Y + 2 + Offset + 1);
                        G.DrawString(Upper[I].ToString(), Font, Brushes.White, R.X + 3 + Offset, R.Y + 2 + Offset);

                        if (!char.IsLetter(Lower[I]))
                        {
                            PT1 = LowerCache[I];
                            G.DrawString(Lower[I].ToString(), Font, B1, PT1.X + Offset, PT1.Y + Offset);
                        }
                    }
                    else
                    {
                        G.DrawString(Lower[I].ToString(), Font, Brushes.Black, R.X + 3 + Offset + 1, R.Y + 2 + Offset + 1);
                        G.DrawString(Lower[I].ToString(), Font, Brushes.White, R.X + 3 + Offset, R.Y + 2 + Offset);

                        if (!char.IsLetter(Upper[I]))
                        {
                            PT1 = UpperCache[I];
                            G.DrawString(Upper[I].ToString(), Font, B1, PT1.X + Offset, PT1.Y + Offset);
                        }
                    }
                    break;
            }

            G.DrawRectangle(P2, R.X + 1 + Offset, R.Y + 1 + Offset, R.Width - 2, R.Height - 2);
            G.DrawRectangle(P3, R.X + Offset, R.Y + Offset, R.Width, R.Height);

            if (I == Pressed)
            {
                G.DrawLine(P1, R.X, R.Y, R.Right, R.Y);
                G.DrawLine(P1, R.X, R.Y, R.X, R.Bottom);
            }
        }
    }

    private void DrawArrow(Color color, int rx, int ry)
    {
        Rectangle R = new Rectangle(rx + 8, ry + 8, 16, 16);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        Pen P = new Pen(color, 1);
        AdjustableArrowCap C = new AdjustableArrowCap(3, 2);
        P.CustomEndCap = C;

        G.DrawArc(P, R, 0f, 290f);

        P.Dispose();
        C.Dispose();
        G.SmoothingMode = SmoothingMode.None;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        int Index = ((e.Y / 32) * 12) + (e.X / 32);

        if (Index > 47)
        {
            for (int I = 48; I <= Buttons.Length - 1; I++)
            {
                if (Buttons[I].Contains(e.X, e.Y))
                {
                    Pressed = I;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
        }
        else
        {
            Pressed = Index;
        }

        HandleKey();
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        Pressed = -1;
        Invalidate();
    }

    private void HandleKey()
    {
        if (_Target == null)
            return;
        if (Pressed == -1)
            return;

        switch (Pressed)
        {
            case 47:
                _Target.Text = string.Empty;
                break;
            case 48:
                Shift = !Shift;
                break;
            case 49:
                _Target.Text += " ";
                break;
            case 50:
                if (!(_Target.Text.Length == 0))
                {
                    _Target.Text = _Target.Text.Remove(_Target.Text.Length - 1);
                }
                break;
            default:
                if (Shift)
                {
                    _Target.Text += Upper[Pressed];
                }
                else
                {
                    _Target.Text += Lower[Pressed];
                }
                break;
        }
    }

}
#endregion
#region 3vil_Paginator
[DefaultEvent("SelectedIndexChanged")]
class _3vil_Paginator : Control
{

    public event SelectedIndexChangedEventHandler SelectedIndexChanged;
    public delegate void SelectedIndexChangedEventHandler(object sender, EventArgs e);

    private Bitmap TextBitmap;

    private Graphics TextGraphics;
    public _3vil_Paginator()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Size = new Size(202, 26);

        TextBitmap = new Bitmap(1, 1);
        TextGraphics = Graphics.FromImage(TextBitmap);

        InvalidateItems();

        B1 = new SolidBrush(Color.FromArgb(50, 50, 50));
        B2 = new SolidBrush(Color.FromArgb(55, 55, 55));

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(55, 55, 55));
        P3 = new Pen(Color.FromArgb(65, 65, 65));
    }

    private int _SelectedIndex;
    public int SelectedIndex
    {
        get { return _SelectedIndex; }
        set
        {
            _SelectedIndex = Math.Max(Math.Min(value, MaximumIndex), 0);
            Invalidate();
        }
    }

    private int _NumberOfPages;
    public int NumberOfPages
    {
        get { return _NumberOfPages; }
        set
        {
            _NumberOfPages = value;
            _SelectedIndex = Math.Max(Math.Min(_SelectedIndex, MaximumIndex), 0);
            Invalidate();
        }
    }

    public int MaximumIndex
    {
        get { return NumberOfPages - 1; }
    }


    private int ItemWidth;
    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;

            InvalidateItems();
            Invalidate();
        }
    }

    private void InvalidateItems()
    {
        Size S = TextGraphics.MeasureString("000 ..", Font).ToSize();
        ItemWidth = S.Width + 10;
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;

    private Rectangle R1;
    private Size SZ1;

    private Point PT1;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private SolidBrush B1;

    private SolidBrush B2;
    private Graphics G;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        bool LeftEllipse = false;
        bool RightEllipse = false;

        if (_SelectedIndex < 4)
        {
            for (int I = 0; I <= Math.Min(MaximumIndex, 4); I++)
            {
                RightEllipse = (I == 4) && (MaximumIndex > 4);
                DrawBox(I * ItemWidth, I, false, RightEllipse);
            }
        }
        else if (_SelectedIndex > 3 && _SelectedIndex < (MaximumIndex - 3))
        {
            for (int I = 0; I <= 4; I++)
            {
                LeftEllipse = (I == 0);
                RightEllipse = (I == 4);
                DrawBox(I * ItemWidth, _SelectedIndex + I - 2, LeftEllipse, RightEllipse);
            }
        }
        else
        {
            for (int I = 0; I <= 4; I++)
            {
                LeftEllipse = (I == 0) && (MaximumIndex > 4);
                DrawBox(I * ItemWidth, MaximumIndex - (4 - I), LeftEllipse, false);
            }
        }
    }

    private void DrawBox(int x, int index, bool leftEllipse, bool rightEllipse)
    {
        R1 = new Rectangle(x, 0, ItemWidth - 4, Height - 1);

        GP1 = ThemeModule.CreateRound(R1, 7);
        GP2 = ThemeModule.CreateRound(R1.X + 1, R1.Y + 1, R1.Width - 2, R1.Height - 2, 7);

        string T = Convert.ToString(index + 1);

        if (leftEllipse)
            T = ".. " + T;
        if (rightEllipse)
            T = T + " ..";

        SZ1 = G.MeasureString(T, Font).ToSize();
        PT1 = new Point(R1.X + (R1.Width / 2 - SZ1.Width / 2), R1.Y + (R1.Height / 2 - SZ1.Height / 2));

        if (index == _SelectedIndex)
        {
            G.FillPath(B1, GP1);

            Font F = new Font(Font, FontStyle.Underline);
            G.DrawString(T, F, Brushes.Black, PT1.X + 1, PT1.Y + 1);
            G.DrawString(T, F, Brushes.White, PT1);
            F.Dispose();

            G.DrawPath(P1, GP2);
            G.DrawPath(P2, GP1);
        }
        else
        {
            G.FillPath(B2, GP1);

            G.DrawString(T, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
            G.DrawString(T, Font, Brushes.White, PT1);

            G.DrawPath(P3, GP2);
            G.DrawPath(P1, GP1);
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            int NewIndex = 0;
            int OldIndex = _SelectedIndex;

            if (_SelectedIndex < 4)
            {
                NewIndex = (e.X / ItemWidth);
            }
            else if (_SelectedIndex > 3 && _SelectedIndex < (MaximumIndex - 3))
            {
                NewIndex = (e.X / ItemWidth);

                if (NewIndex == 2)
                {
                    NewIndex = OldIndex;
                }
                else if (NewIndex < 2)
                {
                    NewIndex = OldIndex - (2 - NewIndex);
                }
                else if (NewIndex > 2)
                {
                    NewIndex = OldIndex + (NewIndex - 2);
                }
            }
            else
            {
                NewIndex = MaximumIndex - (4 - (e.X / ItemWidth));
            }

            if ((NewIndex < _NumberOfPages) && (!(NewIndex == OldIndex)))
            {
                SelectedIndex = NewIndex;
                if (SelectedIndexChanged != null)
                {
                    SelectedIndexChanged(this, null);
                }
            }
        }

        base.OnMouseDown(e);
    }

}
#endregion
#region 3vil_ScrollBar_V
[DefaultEvent("Scroll")]
class _3vil_VScrollBar : Control
{

    public event ScrollEventHandler Scroll;
    public delegate void ScrollEventHandler(object sender);

    private int _Minimum;
    public int Minimum
    {
        get { return _Minimum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Minimum = value;
            if (value > _Value)
                _Value = value;
            if (value > _Maximum)
                _Maximum = value;

            InvalidateLayout();
        }
    }

    private int _Maximum = 100;
    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value < 1)
                value = 1;

            _Maximum = value;
            if (value < _Value)
                _Value = value;
            if (value < _Minimum)
                _Minimum = value;

            InvalidateLayout();
        }
    }

    private int _Value;
    public int Value
    {
        get
        {
            if (!ShowThumb)
                return _Minimum;
            return _Value;
        }
        set
        {
            if (value == _Value)
                return;

            if (value > _Maximum || value < _Minimum)
            {
                throw new Exception("Property value is not valid.");
            }

            _Value = value;
            InvalidatePosition();

            if (Scroll != null)
            {
                Scroll(this);
            }
        }
    }

    public double _Percent { get; set; }
    public double Percent
    {
        get
        {
            if (!ShowThumb)
                return 0;
            return GetProgress();
        }
    }

    private int _SmallChange = 1;
    public int SmallChange
    {
        get { return _SmallChange; }
        set
        {
            if (value < 1)
            {
                throw new Exception("Property value is not valid.");
            }

            _SmallChange = value;
        }
    }

    private int _LargeChange = 10;
    public int LargeChange
    {
        get { return _LargeChange; }
        set
        {
            if (value < 1)
            {
                throw new Exception("Property value is not valid.");
            }

            _LargeChange = value;
        }
    }

    private int ButtonSize = 16;
    // 14 minimum
    private int ThumbSize = 24;

    private Rectangle TSA;
    private Rectangle BSA;
    private Rectangle Shaft;

    private Rectangle Thumb;
    private bool ShowThumb;

    private bool ThumbDown;
    public _3vil_VScrollBar()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Width = 18;

        B1 = new SolidBrush(Color.FromArgb(55, 55, 55));
        B2 = new SolidBrush(Color.FromArgb(35, 35, 35));

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(65, 65, 65));
        P3 = new Pen(Color.FromArgb(55, 55, 55));
        P4 = new Pen(Color.FromArgb(40, 40, 40));
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;
    private GraphicsPath GP3;

    private GraphicsPath GP4;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private Pen P4;
    private SolidBrush B1;

    private SolidBrush B2;

    int I1;
    private Graphics G;

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.Clear(BackColor);

        GP1 = DrawArrow(4, 6, false);
        GP2 = DrawArrow(5, 7, false);

        G.FillPath(B1, GP2);
        G.FillPath(B2, GP1);

        GP3 = DrawArrow(4, Height - 11, true);
        GP4 = DrawArrow(5, Height - 10, true);

        G.FillPath(B1, GP4);
        G.FillPath(B2, GP3);

        if (ShowThumb)
        {
            G.FillRectangle(B1, Thumb);
            G.DrawRectangle(P1, Thumb);
            G.DrawRectangle(P2, Thumb.X + 1, Thumb.Y + 1, Thumb.Width - 2, Thumb.Height - 2);

            int Y = 0;
            int LY = Thumb.Y + (Thumb.Height / 2) - 3;

            for (int I = 0; I <= 2; I++)
            {
                Y = LY + (I * 3);

                G.DrawLine(P1, Thumb.X + 5, Y, Thumb.Right - 5, Y);
                G.DrawLine(P2, Thumb.X + 5, Y + 1, Thumb.Right - 5, Y + 1);
            }
        }

        G.DrawRectangle(P3, 0, 0, Width - 1, Height - 1);
        G.DrawRectangle(P4, 1, 1, Width - 3, Height - 3);
    }

    private GraphicsPath DrawArrow(int x, int y, bool flip)
    {
        GraphicsPath GP = new GraphicsPath();

        int W = 9;
        int H = 5;

        if (flip)
        {
            GP.AddLine(x + 1, y, x + W + 1, y);
            GP.AddLine(x + W, y, x + H, y + H - 1);
        }
        else
        {
            GP.AddLine(x, y + H, x + W, y + H);
            GP.AddLine(x + W, y + H, x + H, y);
        }

        GP.CloseFigure();
        return GP;
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        InvalidateLayout();
    }

    private void InvalidateLayout()
    {
        TSA = new Rectangle(0, 0, Width, ButtonSize);
        BSA = new Rectangle(0, Height - ButtonSize, Width, ButtonSize);
        Shaft = new Rectangle(0, TSA.Bottom + 1, Width, Height - (ButtonSize * 2) - 1);

        ShowThumb = ((_Maximum - _Minimum) > Shaft.Height);

        if (ShowThumb)
        {
            //ThumbSize = Math.Max(0, 14) 'TODO: Implement this.
            Thumb = new Rectangle(1, 0, Width - 3, ThumbSize);
        }

        if (Scroll != null)
        {
            Scroll(this);
        }
        InvalidatePosition();
    }

    private void InvalidatePosition()
    {
        Thumb.Y = Convert.ToInt32(GetProgress() * (Shaft.Height - ThumbSize)) + TSA.Height;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left && ShowThumb)
        {
            if (TSA.Contains(e.Location))
            {
                I1 = _Value - _SmallChange;
            }
            else if (BSA.Contains(e.Location))
            {
                I1 = _Value + _SmallChange;
            }
            else
            {
                if (Thumb.Contains(e.Location))
                {
                    ThumbDown = true;
                    base.OnMouseDown(e);
                    return;
                }
                else
                {
                    if (e.Y < Thumb.Y)
                    {
                        I1 = _Value - _LargeChange;
                    }
                    else
                    {
                        I1 = _Value + _LargeChange;
                    }
                }
            }

            Value = Math.Min(Math.Max(I1, _Minimum), _Maximum);
            InvalidatePosition();
        }

        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (ThumbDown && ShowThumb)
        {
            int ThumbPosition = e.Y - TSA.Height - (ThumbSize / 2);
            int ThumbBounds = Shaft.Height - ThumbSize;

            I1 = Convert.ToInt32(((double)ThumbPosition / (double)ThumbBounds) * (_Maximum - _Minimum)) + _Minimum;

            Value = Math.Min(Math.Max(I1, _Minimum), _Maximum);
            InvalidatePosition();
        }

        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        ThumbDown = false;
        base.OnMouseUp(e);
    }

    private double GetProgress()
    {
        return (double)(_Value - _Minimum) / (double)(_Maximum - _Minimum);
    }

}
#endregion
#region 3vil_ScrollBar_H
[DefaultEvent("Scroll")]
class _3vil_HScrollBar : Control
{

    public event ScrollEventHandler Scroll;
    public delegate void ScrollEventHandler(object sender);

    private int _Minimum;
    public int Minimum
    {
        get { return _Minimum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Minimum = value;
            if (value > _Value)
                _Value = value;
            if (value > _Maximum)
                _Maximum = value;

            InvalidateLayout();
        }
    }

    private int _Maximum = 100;
    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Maximum = value;
            if (value < _Value)
                _Value = value;
            if (value < _Minimum)
                _Minimum = value;

            InvalidateLayout();
        }
    }

    private int _Value;
    public int Value
    {
        get
        {
            if (!ShowThumb)
                return _Minimum;
            return _Value;
        }
        set
        {
            if (value == _Value)
                return;

            if (value > _Maximum || value < _Minimum)
            {
                throw new Exception("Property value is not valid.");
            }

            _Value = value;
            InvalidatePosition();

            if (Scroll != null)
            {
                Scroll(this);
            }
        }
    }

    private int _SmallChange = 1;
    public int SmallChange
    {
        get { return _SmallChange; }
        set
        {
            if (value < 1)
            {
                throw new Exception("Property value is not valid.");
            }

            _SmallChange = value;
        }
    }

    private int _LargeChange = 10;
    public int LargeChange
    {
        get { return _LargeChange; }
        set
        {
            if (value < 1)
            {
                throw new Exception("Property value is not valid.");
            }

            _LargeChange = value;
        }
    }

    private int ButtonSize = 16;
    // 14 minimum
    private int ThumbSize = 24;

    private Rectangle LSA;
    private Rectangle RSA;
    private Rectangle Shaft;

    private Rectangle Thumb;
    private bool ShowThumb;

    private bool ThumbDown;
    public _3vil_HScrollBar()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Height = 18;

        B1 = new SolidBrush(Color.FromArgb(55, 55, 55));
        B2 = new SolidBrush(Color.FromArgb(35, 35, 35));

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(65, 65, 65));
        P3 = new Pen(Color.FromArgb(55, 55, 55));
        P4 = new Pen(Color.FromArgb(40, 40, 40));
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;
    private GraphicsPath GP3;

    private GraphicsPath GP4;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private Pen P4;
    private SolidBrush B1;

    private SolidBrush B2;

    int I1;
    private Graphics G;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.Clear(BackColor);

        GP1 = DrawArrow(6, 4, false);
        GP2 = DrawArrow(7, 5, false);

        G.FillPath(B1, GP2);
        G.FillPath(B2, GP1);

        GP3 = DrawArrow(Width - 11, 4, true);
        GP4 = DrawArrow(Width - 10, 5, true);

        G.FillPath(B1, GP4);
        G.FillPath(B2, GP3);

        if (ShowThumb)
        {
            G.FillRectangle(B1, Thumb);
            G.DrawRectangle(P1, Thumb);
            G.DrawRectangle(P2, Thumb.X + 1, Thumb.Y + 1, Thumb.Width - 2, Thumb.Height - 2);

            int X = 0;
            int LX = Thumb.X + (Thumb.Width / 2) - 3;

            for (int I = 0; I <= 2; I++)
            {
                X = LX + (I * 3);

                G.DrawLine(P1, X, Thumb.Y + 5, X, Thumb.Bottom - 5);
                G.DrawLine(P2, X + 1, Thumb.Y + 5, X + 1, Thumb.Bottom - 5);
            }
        }

        G.DrawRectangle(P3, 0, 0, Width - 1, Height - 1);
        G.DrawRectangle(P4, 1, 1, Width - 3, Height - 3);
    }

    private GraphicsPath DrawArrow(int x, int y, bool flip)
    {
        GraphicsPath GP = new GraphicsPath();

        int W = 5;
        int H = 9;

        if (flip)
        {
            GP.AddLine(x, y + 1, x, y + H + 1);
            GP.AddLine(x, y + H, x + W - 1, y + W);
        }
        else
        {
            GP.AddLine(x + W, y, x + W, y + H);
            GP.AddLine(x + W, y + H, x + 1, y + W);
        }

        GP.CloseFigure();
        return GP;
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        InvalidateLayout();
    }

    private void InvalidateLayout()
    {
        LSA = new Rectangle(0, 0, ButtonSize, Height);
        RSA = new Rectangle(Width - ButtonSize, 0, ButtonSize, Height);
        Shaft = new Rectangle(LSA.Right + 1, 0, Width - (ButtonSize * 2) - 1, Height);

        ShowThumb = ((_Maximum - _Minimum) > Shaft.Width);

        if (ShowThumb)
        {
            //ThumbSize = Math.Max(0, 14) 'TODO: Implement this.
            Thumb = new Rectangle(0, 1, ThumbSize, Height - 3);
        }

        if (Scroll != null)
        {
            Scroll(this);
        }
        InvalidatePosition();
    }

    private void InvalidatePosition()
    {
        Thumb.X = Convert.ToInt32(GetProgress() * (Shaft.Width - ThumbSize)) + LSA.Width;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left && ShowThumb)
        {
            if (LSA.Contains(e.Location))
            {
                I1 = _Value - _SmallChange;
            }
            else if (RSA.Contains(e.Location))
            {
                I1 = _Value + _SmallChange;
            }
            else
            {
                if (Thumb.Contains(e.Location))
                {
                    ThumbDown = true;
                    base.OnMouseDown(e);
                    return;
                }
                else
                {
                    if (e.X < Thumb.X)
                    {
                        I1 = _Value - _LargeChange;
                    }
                    else
                    {
                        I1 = _Value + _LargeChange;
                    }
                }
            }

            Value = Math.Min(Math.Max(I1, _Minimum), _Maximum);
            InvalidatePosition();
        }

        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (ThumbDown && ShowThumb)
        {
            int ThumbPosition = e.X - LSA.Width - (ThumbSize / 2);
            int ThumbBounds = Shaft.Width - ThumbSize;

            I1 = Convert.ToInt32(((double)ThumbPosition / (double)ThumbBounds) * (_Maximum - _Minimum)) + _Minimum;

            Value = Math.Min(Math.Max(I1, _Minimum), _Maximum);
            InvalidatePosition();
        }

        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        ThumbDown = false;
        base.OnMouseUp(e);
    }

    private double GetProgress()
    {
        return (double)(_Value - _Minimum) / (double)(_Maximum - _Minimum);
    }

}
#endregion

#region 3vil_ColorTable
class _3vil_ColorTable : ProfessionalColorTable
{


    private Color BackColor = Color.FromArgb(55, 55, 55);
    public override Color ButtonSelectedBorder
    {
        get { return BackColor; }
    }

    public override Color CheckBackground
    {
        get { return BackColor; }
    }

    public override Color CheckPressedBackground
    {
        get { return BackColor; }
    }

    public override Color CheckSelectedBackground
    {
        get { return BackColor; }
    }

    public override Color ImageMarginGradientBegin
    {
        get { return BackColor; }
    }

    public override Color ImageMarginGradientEnd
    {
        get { return BackColor; }
    }

    public override Color ImageMarginGradientMiddle
    {
        get { return BackColor; }
    }

    public override Color MenuBorder
    {
        get { return Color.FromArgb(25, 25, 25); }
    }

    public override Color MenuItemBorder
    {
        get { return BackColor; }
    }

    public override Color MenuItemSelected
    {
        get { return Color.FromArgb(65, 65, 65); }
    }

    public override Color SeparatorDark
    {
        get { return Color.FromArgb(35, 35, 35); }
    }

    public override Color ToolStripDropDownBackground
    {
        get { return BackColor; }
    }

}
#endregion
#region 3vil_ListView
class _3vil_ListView : Control
{

    public class _3vil_ListViewItem
    {
        public string Text { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<NSListViewSubItem> SubItems { get; set; }


        protected Guid UniqueId;
        public _3vil_ListViewItem()
        {
            UniqueId = Guid.NewGuid();
        }

        public override string ToString()
        {
            return Text;
        }

        public override bool Equals(object obj)
        {
            if (obj is _3vil_ListViewItem)
            {
                return (((_3vil_ListViewItem)obj).UniqueId == UniqueId);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

    public class NSListViewSubItem
    {
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    public class NSListViewColumnHeader
    {
        public string Text { get; set; }
        public int Width { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    private List<_3vil_ListViewItem> _Items = new List<_3vil_ListViewItem>();
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public _3vil_ListViewItem[] Items
    {
        get { return _Items.ToArray(); }
        set
        {
            _Items = new List<_3vil_ListViewItem>(value);
            InvalidateScroll();
        }
    }

    private List<_3vil_ListViewItem> _SelectedItems = new List<_3vil_ListViewItem>();
    public _3vil_ListViewItem[] SelectedItems
    {
        get { return _SelectedItems.ToArray(); }
    }

    private List<NSListViewColumnHeader> _Columns = new List<NSListViewColumnHeader>();
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public NSListViewColumnHeader[] Columns
    {
        get { return _Columns.ToArray(); }
        set
        {
            _Columns = new List<NSListViewColumnHeader>(value);
            InvalidateColumns();
        }
    }

    private bool _MultiSelect = true;
    public bool MultiSelect
    {
        get { return _MultiSelect; }
        set
        {
            _MultiSelect = value;

            if (_SelectedItems.Count > 1)
            {
                _SelectedItems.RemoveRange(1, _SelectedItems.Count - 1);
            }

            Invalidate();
        }
    }

    private int ItemHeight = 24;
    public override Font Font
    {
        get { return base.Font; }
        set
        {
            ItemHeight = Convert.ToInt32(Graphics.FromHwnd(Handle).MeasureString("@", Font).Height) + 6;

            if (VS != null)
            {
                VS.SmallChange = ItemHeight;
                VS.LargeChange = ItemHeight;
            }

            base.Font = value;
            InvalidateLayout();
        }
    }

    #region " Item Helper Methods "

    //Ok, you've seen everything of importance at this point; I am begging you to spare yourself. You must not read any further!

    public void AddItem(string text, params string[] subItems)
    {
        List<NSListViewSubItem> Items = new List<NSListViewSubItem>();
        foreach (string I in subItems)
        {
            NSListViewSubItem SubItem = new NSListViewSubItem();
            SubItem.Text = I;
            Items.Add(SubItem);
        }

        _3vil_ListViewItem Item = new _3vil_ListViewItem();
        Item.Text = text;
        Item.SubItems = Items;

        _Items.Add(Item);
        InvalidateScroll();
    }

    public void RemoveItemAt(int index)
    {
        _Items.RemoveAt(index);
        InvalidateScroll();
    }

    public void RemoveItem(_3vil_ListViewItem item)
    {
        _Items.Remove(item);
        InvalidateScroll();
    }

    public void RemoveItems(_3vil_ListViewItem[] items)
    {
        foreach (_3vil_ListViewItem I in items)
        {
            _Items.Remove(I);
        }

        InvalidateScroll();
    }

    #endregion


    private _3vil_VScrollBar VS;
    public _3vil_ListView()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, true);

        P1 = new Pen(Color.FromArgb(55, 55, 55));
        P2 = new Pen(Color.FromArgb(35, 35, 35));
        P3 = new Pen(Color.FromArgb(65, 65, 65));

        B1 = new SolidBrush(Color.FromArgb(62, 62, 62));
        B2 = new SolidBrush(Color.FromArgb(65, 65, 65));
        B3 = new SolidBrush(Color.FromArgb(47, 47, 47));
        B4 = new SolidBrush(Color.FromArgb(50, 50, 50));

        VS = new _3vil_VScrollBar();
        VS.SmallChange = ItemHeight;
        VS.LargeChange = ItemHeight;

        VS.Scroll += HandleScroll;
        VS.MouseDown += VS_MouseDown;
        Controls.Add(VS);

        InvalidateLayout();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        InvalidateLayout();
        base.OnSizeChanged(e);
    }

    private void HandleScroll(object sender)
    {
        Invalidate();
    }

    private void InvalidateScroll()
    {
        VS.Maximum = (_Items.Count * ItemHeight);
        Invalidate();
    }

    private void InvalidateLayout()
    {
        VS.Location = new Point(Width - VS.Width - 1, 1);
        VS.Size = new Size(18, Height - 2);

        Invalidate();
    }

    private int[] ColumnOffsets;
    private void InvalidateColumns()
    {
        int Width = 3;
        ColumnOffsets = new int[_Columns.Count];

        for (int I = 0; I <= _Columns.Count - 1; I++)
        {
            ColumnOffsets[I] = Width;
            Width += Columns[I].Width;
        }

        Invalidate();
    }

    private void VS_MouseDown(object sender, MouseEventArgs e)
    {
        Focus();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Focus();

        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            int Offset = Convert.ToInt32(VS.Percent * (VS.Maximum - (Height - (ItemHeight * 2))));
            int Index = ((e.Y + Offset - ItemHeight) / ItemHeight);

            if (Index > _Items.Count - 1)
                Index = -1;

            if (!(Index == -1))
            {
                //TODO: Handle Shift key

                if (ModifierKeys == Keys.Control && _MultiSelect)
                {
                    if (_SelectedItems.Contains(_Items[Index]))
                    {
                        _SelectedItems.Remove(_Items[Index]);
                    }
                    else
                    {
                        _SelectedItems.Add(_Items[Index]);
                    }
                }
                else
                {
                    _SelectedItems.Clear();
                    _SelectedItems.Add(_Items[Index]);
                }
            }

            Invalidate();
        }

        base.OnMouseDown(e);
    }

    private Pen P1;
    private Pen P2;
    private Pen P3;
    private SolidBrush B1;
    private SolidBrush B2;
    private SolidBrush B3;
    private SolidBrush B4;

    private LinearGradientBrush GB1;
    //I am so sorry you have to witness this. I tried warning you. ;.;

    private Graphics G;
    protected override void OnPaint(PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);

        int X = 0;
        int Y = 0;
        float H = 0;

        G.DrawRectangle(P1, 1, 1, Width - 3, Height - 3);

        Rectangle R1 = default(Rectangle);
        _3vil_ListViewItem CI = null;

        int Offset = Convert.ToInt32(VS.Percent * (VS.Maximum - (Height - (ItemHeight * 2))));

        int StartIndex = 0;
        if (Offset == 0)
            StartIndex = 0;
        else
            StartIndex = (Offset / ItemHeight);

        int EndIndex = Math.Min(StartIndex + (Height / ItemHeight), _Items.Count - 1);

        for (int I = StartIndex; I <= EndIndex; I++)
        {
            CI = Items[I];

            R1 = new Rectangle(0, ItemHeight + (I * ItemHeight) + 1 - Offset, Width, ItemHeight - 1);

            H = G.MeasureString(CI.Text, Font).Height;
            Y = R1.Y + Convert.ToInt32((ItemHeight / 2) - (H / 2));

            if (_SelectedItems.Contains(CI))
            {
                if (I % 2 == 0)
                {
                    G.FillRectangle(B1, R1);
                }
                else
                {
                    G.FillRectangle(B2, R1);
                }
            }
            else
            {
                if (I % 2 == 0)
                {
                    G.FillRectangle(B3, R1);
                }
                else
                {
                    G.FillRectangle(B4, R1);
                }
            }

            G.DrawLine(P2, 0, R1.Bottom, Width, R1.Bottom);

            if (Columns.Length > 0)
            {
                R1.Width = Columns[0].Width;
                G.SetClip(R1);
            }

            //TODO: Ellipse text that overhangs seperators.
            G.DrawString(CI.Text, Font, Brushes.Black, 10, Y + 1);
            G.DrawString(CI.Text, Font, Brushes.White, 9, Y);

            if (CI.SubItems != null)
            {
                for (int I2 = 0; I2 <= Math.Min(CI.SubItems.Count, _Columns.Count) - 1; I2++)
                {
                    X = ColumnOffsets[I2 + 1] + 4;

                    R1.X = X;
                    R1.Width = Columns[I2].Width;
                    G.SetClip(R1);

                    G.DrawString(CI.SubItems[I2].Text, Font, Brushes.Black, X + 1, Y + 1);
                    G.DrawString(CI.SubItems[I2].Text, Font, Brushes.White, X, Y);
                }
            }

            G.ResetClip();
        }

        R1 = new Rectangle(0, 0, Width, ItemHeight);

        GB1 = new LinearGradientBrush(R1, Color.FromArgb(60, 60, 60), Color.FromArgb(55, 55, 55), 90f);
        G.FillRectangle(GB1, R1);
        G.DrawRectangle(P3, 1, 1, Width - 22, ItemHeight - 2);

        int LH = Math.Min(VS.Maximum + ItemHeight - Offset, Height);

        NSListViewColumnHeader CC = null;
        for (int I = 0; I <= _Columns.Count - 1; I++)
        {
            CC = Columns[I];

            H = G.MeasureString(CC.Text, Font).Height;
            Y = Convert.ToInt32((ItemHeight / 2) - (H / 2));
            X = ColumnOffsets[I];

            G.DrawString(CC.Text, Font, Brushes.Black, X + 1, Y + 1);
            G.DrawString(CC.Text, Font, Brushes.White, X, Y);

            G.DrawLine(P2, X - 3, 0, X - 3, LH);
            G.DrawLine(P3, X - 2, 0, X - 2, ItemHeight);
        }

        G.DrawRectangle(P2, 0, 0, Width - 1, Height - 1);

        G.DrawLine(P2, 0, ItemHeight, Width, ItemHeight);
        G.DrawLine(P2, VS.Location.X - 1, 0, VS.Location.X - 1, Height);
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        int Move = -((e.Delta * SystemInformation.MouseWheelScrollLines / 120) * (ItemHeight / 2));

        int Value = Math.Max(Math.Min(VS.Value + Move, VS.Maximum), VS.Minimum);
        VS.Value = Value;

        base.OnMouseWheel(e);
    }

}
#endregion
#endregion

#region "WC_Theme_1"



class __________WC_Theme_1 : ThemeContainer154
{

    #region WC_Theme_1
    public __________WC_Theme_1()
    {
        TransparencyKey = Color.Fuchsia;
    }

    protected override void ColorHook()
    {

    }

    protected override void PaintHook()
    {
        G.Clear(Color.FromArgb(12, 12, 12));
        Pen P = new Pen(Color.FromArgb(32, 32, 32));
        G.DrawLine(P, 11, 31, Width - 12, 31);
        G.DrawLine(P, 11, 8, Width - 12, 8);
        G.FillRectangle(new LinearGradientBrush(new Rectangle(8, 38, Width - 16, Height - 46), Color.FromArgb(12, 12, 12), Color.FromArgb(18, 18, 18), LinearGradientMode.BackwardDiagonal), 8, 38, Width - 16, Height - 46);
        DrawText(Brushes.White, HorizontalAlignment.Left, 25, 6);
        DrawBorders(new Pen(Color.FromArgb(60, 60, 60)), 1);
        DrawBorders(Pens.Black);

        P = new Pen(Color.FromArgb(25, 25, 25));
        G.DrawLine(Pens.Black, 6, 0, 6, Height - 6);
        G.DrawLine(Pens.Black, Width - 6, 0, Width - 6, Height - 6);
        G.DrawLine(P, 6, 0, 6, Height - 6);
        G.DrawLine(P, Width - 8, 0, Width - 8, Height - 6);

        G.DrawRectangle(Pens.Black, 11, 4, Width - 23, 22);
        G.DrawLine(P, 6, Height - 6, Width - 8, Height - 6);
        G.DrawLine(Pens.Black, 6, Height - 8, Width - 8, Height - 8);
        DrawCorners(Color.Fuchsia);
    }

}
    #endregion
#region "WC_1_TabControl"
class WC_1_TabControl : TabControl
{
    int OldIndex;
    private int _Speed = 10;
    public int Speed
    {
        get { return _Speed; }
        set
        {
            if (value > 20 | value < -20)
            {
                MessageBox.Show("Speed needs to be in between -20 and 20.");
            }
            else
            {
                _Speed = value;
            }
        }
    }
    private Color LightBlack = Color.FromArgb(18, 18, 18);
    private Color LighterBlack = Color.FromArgb(21, 21, 21);
    private LinearGradientBrush DrawGradientBrush;
    private LinearGradientBrush DrawGradientBrush2;
    private LinearGradientBrush DrawGradientBrushPen;
    private LinearGradientBrush DrawGradientBrushTab;
    private Color _ControlBColor;
    public Color TabTextColor
    {
        get { return _ControlBColor; }
        set
        {
            _ControlBColor = value;
            Invalidate();
        }
    }

    public WC_1_TabControl()
        : base()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        TabTextColor = Color.White;
    }
    public void DoAnimationScrollDown(Control Control1, Control Control2)
    {
        Graphics G = Control1.CreateGraphics();
        Bitmap P1 = new Bitmap(Control1.Width, Control1.Height);
        Bitmap P2 = new Bitmap(Control2.Width, Control2.Height);
        Control1.DrawToBitmap(P1, new Rectangle(0, 0, Control1.Width, Control1.Height));
        Control2.DrawToBitmap(P2, new Rectangle(0, 0, Control2.Width, Control2.Height));
        foreach (Control c in Control1.Controls)
        {
            c.Hide();
        }
        int Slide = Control1.Height - (Control1.Height % _Speed);
        int a = 0;
        for (a = 0; a <= Slide; a += _Speed)
        {
            G.DrawImage(P1, new Rectangle(0, a, Control1.Width, Control1.Height));
            G.DrawImage(P2, new Rectangle(0, a - Control2.Height, Control2.Width, Control2.Height));
        }
        a = Control1.Width;
        G.DrawImage(P1, new Rectangle(0, a, Control1.Width, Control1.Height));
        G.DrawImage(P2, new Rectangle(0, a - Control2.Height, Control2.Width, Control2.Height));
        SelectedTab = (TabPage)Control2;
        foreach (Control c in Control2.Controls)
        {
            c.Show();
        }
        foreach (Control c in Control1.Controls)
        {
            c.Show();
        }
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Rectangle r2 = new Rectangle(2, 0, Width, 25);
        Rectangle r3 = new Rectangle(2, 0, Width, 25);
        e.Graphics.Clear(Color.FromArgb(18, 18, 18));
        Rectangle ItemBounds = default(Rectangle);
        SolidBrush TextBrush = new SolidBrush(Color.Empty);
        SolidBrush TabBrush = new SolidBrush(Color.FromArgb(15, 15, 15));
        DrawGradientBrush2 = new LinearGradientBrush(r3, Color.FromArgb(25, 25, 25), Color.FromArgb(42, 42, 42), LinearGradientMode.ForwardDiagonal);
        e.Graphics.FillRectangle(DrawGradientBrush2, r2);
        for (int TabItemIndex = 0; TabItemIndex <= this.TabCount - 1; TabItemIndex++)
        {
            ItemBounds = this.GetTabRect(TabItemIndex);

            if (Convert.ToBoolean(TabItemIndex & 1))
            {
                TabBrush.Color = Color.Transparent;
            }
            else
            {
                TabBrush.Color = Color.Transparent;
            }
            e.Graphics.FillRectangle(TabBrush, ItemBounds);
            Pen BorderPen = null;
            if (TabItemIndex == SelectedIndex)
            {
                BorderPen = new Pen(Color.Black, 1);
            }
            else
            {
                BorderPen = new Pen(Color.Black, 1);
            }
            Rectangle rPen = new Rectangle(ItemBounds.Location.X + 3, ItemBounds.Location.Y + 0, ItemBounds.Width - 4, ItemBounds.Height - 2);
            e.Graphics.DrawRectangle(BorderPen, rPen);
            DrawGradientBrushPen = new LinearGradientBrush(rPen, Color.FromArgb(5, 5, 5), Color.FromArgb(24, 24, 24), LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(DrawGradientBrushPen, rPen);
            BorderPen.Dispose();
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            if (this.SelectedIndex == TabItemIndex)
            {
                TextBrush.Color = TabTextColor;
            }
            else
            {
                TextBrush.Color = Color.Gray;
            }
            //Rectangle.(this.GetTabRect(TabItemIndex));
            //RectangleF.
            e.Graphics.DrawString(this.TabPages[TabItemIndex].Text, this.Font, TextBrush, Rectangle.Ceiling(this.GetTabRect(TabItemIndex)), sf);
            //G.DrawString(Text, Font, Brushes.White, 22, 2);
            try
            {
                this.TabPages[TabItemIndex].BackColor = Color.FromArgb(15, 15, 15);

            }
            catch
            {
            }
        }
        try
        {
            foreach (TabPage Page in this.TabPages)
            {
                Page.BorderStyle = BorderStyle.None;
            }
        }
        catch
        {
        }
        e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb(255, Color.Black))), 2, 0, Width - 3, Height - 3);
        e.Graphics.DrawRectangle(new Pen(new SolidBrush(LighterBlack)), new Rectangle(3, 24, Width - 5, Height - 28));
        e.Graphics.DrawLine(new Pen(new SolidBrush(Color.FromArgb(255, Color.Black))), 2, 23, Width - 2, 23);
        e.Graphics.DrawLine(new Pen(new SolidBrush(Color.FromArgb(35, 35, 35))), 0, 0, 1, 1);
        e.Graphics.DrawLine(new Pen(new SolidBrush(Color.FromArgb(70, 70, 70))), 2, Height - 2, Width + 1, Height - 2);

    }
    protected override void OnSelecting(System.Windows.Forms.TabControlCancelEventArgs e)
    {
        if (OldIndex < e.TabPageIndex)
        {
            DoAnimationScrollUp(TabPages[OldIndex], TabPages[e.TabPageIndex]);
        }
        else
        {
            DoAnimationScrollDown(TabPages[OldIndex], TabPages[e.TabPageIndex]);
        }
    }

    protected override void OnDeselecting(System.Windows.Forms.TabControlCancelEventArgs e)
    {
        OldIndex = e.TabPageIndex;
    }
    public void DoAnimationScrollUp(Control Control1, Control Control2)
    {
        Graphics G = Control1.CreateGraphics();
        Bitmap P1 = new Bitmap(Control1.Width, Control1.Height);
        Bitmap P2 = new Bitmap(Control2.Width, Control2.Height);
        Control1.DrawToBitmap(P1, new Rectangle(0, 0, Control1.Width, Control1.Height));
        Control2.DrawToBitmap(P2, new Rectangle(0, 0, Control2.Width, Control2.Height));

        foreach (Control c in Control1.Controls)
        {
            c.Hide();
        }
        int Slide = Control1.Height - (Control1.Height % _Speed);
        int a = 0;
        for (a = 0; a >= -Slide; a += -_Speed)
        {
            G.DrawImage(P1, new Rectangle(0, a, Control1.Width, Control1.Height));
            G.DrawImage(P2, new Rectangle(0, a + Control2.Height, Control2.Width, Control2.Height));
        }
        a = Control1.Width;
        G.DrawImage(P1, new Rectangle(0, a, Control1.Width, Control1.Height));
        G.DrawImage(P2, new Rectangle(0, a + Control2.Height, Control2.Width, Control2.Height));

        SelectedTab = (TabPage)Control2;

        foreach (Control c in Control2.Controls)
        {
            c.Show();
        }

        foreach (Control c in Control1.Controls)
        {
            c.Show();
        }
    }
}
#endregion
#region "WC_1_GroupBox"
class WC_1_GroupBox : ThemeContainer154
{

    public WC_1_GroupBox()
    {
        ControlMode = true;
    }

    protected override void ColorHook()
    {

    }

    protected override void PaintHook()
    {
        G.Clear(Color.Transparent);
        DrawBorders(new Pen(Color.FromArgb(36, 36, 36)), 1);
        DrawBorders(Pens.Black);
        G.DrawLine(new Pen(Color.FromArgb(48, 48, 48)), 1, 1, Width - 2, 1);
        G.FillRectangle(new LinearGradientBrush(new Rectangle(8, 8, Width - 16, Height - 16), Color.FromArgb(12, 12, 12), Color.FromArgb(18, 18, 18), LinearGradientMode.BackwardDiagonal), 8, 8, Width - 16, Height - 16);
        DrawBorders(new Pen(Color.FromArgb(36, 36, 36)), 7);
    }
}
#endregion
#region "WC_1_CheckBox"
class WC_1_CheckBox : ThemeControl154
{
    private Pen P1;
    private Brush B1;
    private Brush B2;
    private Brush B3;
    private bool _Checked = false;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            Invalidate();
        }
    }
    public WC_1_CheckBox()
    {
        Click += changeChecked;
        BackColor = Color.Transparent;
        Transparent = true;
        Size = new Size(150, 16);
    }
    public void changeChecked(object sender, EventArgs e)
    {
        switch (_Checked)
        {
            case false:
                _Checked = true;
                break;
            case true:
                _Checked = false;
                break;
        }
    }
    protected override void ColorHook()
    {
        P1 = new Pen(Color.FromArgb(0, 0, 0));
        B1 = new SolidBrush(Color.FromArgb(15, Color.FromArgb(26, 26, 26)));
        B2 = new SolidBrush(Color.White);
        B3 = new SolidBrush(Color.FromArgb(0, 0, 0));
    }

    protected override void PaintHook()
    {
        G.Clear(BackColor);
        G.FillRectangle(B3, 0, 0, 45, 15);
        G.DrawRectangle(P1, 0, 0, 45, 15);
        if ((_Checked == false))
        {
            G.DrawString("OFF", Font, Brushes.Gray, 3, 1);
            G.FillRectangle(new LinearGradientBrush(new Rectangle(29, -1, 13, 16), Color.FromArgb(35, 35, 35), Color.FromArgb(25, 25, 25), 90), 29, -1, 13, 16);
        }
        else
        {
            DrawGradient(Color.FromArgb(10, 10, 10), Color.FromArgb(20, 20, 20), 15, 2, 28, 11, 90);
            G.DrawString("ON", Font, Brushes.White, 18, 0);
            G.FillRectangle(new LinearGradientBrush(new Rectangle(2, -1, 13, 16), Color.FromArgb(80, 80, 80), Color.FromArgb(60, 60, 60), 90), 2, -1, 13, 16);
        }
        G.FillRectangle(B1, 2, 2, 41, 11);
        DrawText(B2, HorizontalAlignment.Left, 50, 0);
    }
}
#endregion
#region "WC_1_Radiobutton"
class WC_1_Radiobutton : ThemeControl154
{
    public WC_1_Radiobutton()
    {
        BackColor = Color.Transparent;
        Transparent = true;
        Size = new Size(50, 17);
    }
    private bool _Checked;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            Invalidate();
        }
    }

    protected override void OnClick(System.EventArgs e)
    {
        base.OnClick(e);
        foreach (Control C in Parent.Controls)
        {

            if (C.GetType().ToString() == C.ToString().Replace(Application.ProductName.ToString(), " ") + ".WC_Radiobutton_1")
            {
                WC_1_Radiobutton CC = null;
                CC = (WC_1_Radiobutton)C;
                CC.Checked = false;
            }
        }
        _Checked = true;
    }

    protected override void ColorHook()
    {

    }

    protected override void OnTextChanged(System.EventArgs e)
    {
        base.OnTextChanged(e);
        int textSize = 0;
        textSize = (int)this.CreateGraphics().MeasureString(Text, Font).Width;
        this.Width = 25 + textSize;
    }

    protected override void PaintHook()
    {
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        if (_Checked == false)
        {
            G.FillEllipse(new SolidBrush(Color.Black), 0, 0, 16, 16);
            LinearGradientBrush Gbrush = new LinearGradientBrush(new Rectangle(1, 1, 14, 14), Color.FromArgb(24, 30, 36), Color.FromArgb(25, 25, 25), 90f);
            G.FillEllipse(Gbrush, new Rectangle(1, 1, 14, 14));
            Gbrush = new LinearGradientBrush(new Rectangle(2, 2, 12, 12), Color.FromArgb(12, 12, 12), Color.FromArgb(25, 25, 25), 90f);
            G.FillEllipse(Gbrush, new Rectangle(2, 2, 12, 12));
        }
        else
        {
            G.FillEllipse(new SolidBrush(Color.Black), 0, 0, 16, 16);
            LinearGradientBrush Gbrush = new LinearGradientBrush(new Rectangle(1, 1, 14, 14), Color.FromArgb(45, 45, 45), Color.FromArgb(10, 10, 10), 90f);
            G.FillEllipse(Gbrush, new Rectangle(1, 1, 14, 14));
            Gbrush = new LinearGradientBrush(new Rectangle(2, 2, 12, 12), Color.FromArgb(25, 25, 25), Color.FromArgb(20, 20, 20), 90f);
            G.FillEllipse(Gbrush, new Rectangle(2, 2, 12, 12));
            G.FillEllipse(Brushes.Black, new Rectangle(5, 6, 5, 5));
            LinearGradientBrush Gbrush2 = new LinearGradientBrush(new Rectangle(1, 1, 14, 14), Color.FromArgb(130, 130, 130), Color.FromArgb(20, 20, 20), LinearGradientMode.ForwardDiagonal);
            G.FillEllipse(Gbrush2, new Rectangle(3, 3, 10, 10));
        }
        G.DrawString(Text, Font, Brushes.White, 22, 2);
    }


}
#endregion
#region "WC_1_Button"
class WC_1_Button : ThemeControl154
{

    protected override void ColorHook()
    {

    }

    protected override void PaintHook()
    {
        DrawBorders(new Pen(Color.FromArgb(16, 16, 16)), 1);
        G.FillRectangle(new SolidBrush(Color.FromArgb(5, 5, 5)), 0, 0, Width, 8);
        DrawBorders(Pens.Black, 3);
        DrawBorders(new Pen(Color.FromArgb(24, 24, 24)));

        if (State == MouseState.Over)
        {
            G.FillRectangle(new SolidBrush(Color.FromArgb(25, 25, 25)), 3, 3, Width - 6, Height - 6);
            DrawBorders(new Pen(Color.FromArgb(0, 0, 0)), 2);
        }
        else if (State == MouseState.Down)
        {
            G.FillRectangle(new LinearGradientBrush(new Rectangle(3, 3, Width - 6, Height - 6), Color.FromArgb(12, 12, 12), Color.FromArgb(30, 30, 30), LinearGradientMode.BackwardDiagonal), 3, 3, Width - 6, Height - 6);
            DrawBorders(new Pen(Color.FromArgb(0, 0, 0)), 2);
        }
        else
        {
            G.FillRectangle(new LinearGradientBrush(new Rectangle(3, 3, Width - 6, Height - 6), Color.FromArgb(9, 9, 9), Color.FromArgb(18, 18, 18), LinearGradientMode.Vertical), 3, 3, Width - 6, Height - 6);
            DrawBorders(new Pen(Color.FromArgb(32, 32, 32)), 2);
        }
        if (State == MouseState.Down)
        {
            DrawText(Brushes.White, HorizontalAlignment.Center, 2, 2);
        }
        else
        {
            DrawText(Brushes.White, HorizontalAlignment.Center, 0, 0);
        }
    }

}
#endregion
#region "WC_1_TextBox"
class WC_1_TextBox : ThemeControl154
{
    private TextBox withEventsField_Txt = new TextBox();
    public TextBox Txt
    {
        get { return withEventsField_Txt; }
        set
        {
            if (withEventsField_Txt != null)
            {
                withEventsField_Txt.TextChanged -= TextChngTxtBox;
            }
            withEventsField_Txt = value;
            if (withEventsField_Txt != null)
            {
                withEventsField_Txt.TextChanged += TextChngTxtBox;
            }
        }
    }

    private bool _Mulitline;
    public bool Multiline
    {
        get { return _Mulitline; }
        set { _Mulitline = value; }
    }
    private bool _PassMask;
    public bool UsePasswordMask
    {
        get { return _PassMask; }
        set
        {
            _PassMask = value;
            Txt.UseSystemPasswordChar = value;
        }
    }
    private int _maxchars;
    public int MaxCharacters
    {
        get { return _maxchars; }
        set
        {
            _maxchars = value;
            Txt.MaxLength = value;
        }
    }

    public WC_1_TextBox()
    {
        TextChanged += TextChng;

        Txt.TextAlign = HorizontalAlignment.Left;
        Txt.BorderStyle = BorderStyle.None;
        Txt.Location = new Point(10, 6);
        Txt.Font = new Font("Verdana", 8);
        Controls.Add(Txt);
        Text = "";
        Txt.Text = "";
        Size = new Size(150, 25);
    }

    protected override void ColorHook()
    {
        Txt.ForeColor = Color.White;
        Txt.BackColor = Color.FromArgb(15, 15, 15);
    }

    protected override void PaintHook()
    {
        G.Clear(Color.FromArgb(15, 15, 15));
        Txt.Size = new Size(Width - 20, Height - 10);
        switch (Multiline)
        {
            case true:
                Size = new Size(Width, Height);
                break;
            case false:
                Size = new Size(Width, 25);
                break;
        }

        G.FillRectangle(new SolidBrush(Color.FromArgb(15, 15, 15)), new Rectangle(1, 1, Width - 2, Height - 2));
        DrawBorders(new Pen(new SolidBrush(Color.FromArgb(32, 32, 32))), 1);
        DrawBorders(new Pen(new SolidBrush(Color.Black)));
        DrawCorners(Color.FromArgb(15, 15, 15));
        DrawCorners(Color.FromArgb(15, 15, 15), new Rectangle(1, 1, Width - 2, Height - 2));
    }
    public void TextChngTxtBox(object sender, EventArgs e)
    {
        Text = Txt.Text;
    }
    public void TextChng(object sender, EventArgs e)
    {
        Txt.Text = Text;
    }
#endregion

}


#endregion

#region WC_Theme_3
namespace WC_Theme_3
{
    #region ThemeSetup
    #region RoundRect

    static class RoundRectangle
    {
        public static GraphicsPath RoundRect(Rectangle Rectangle, int Curve)
        {
            GraphicsPath GP = new GraphicsPath();
            int EndArcWidth = Curve * 2;
            GP.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, EndArcWidth, EndArcWidth), -180, 90);
            GP.AddArc(new Rectangle(Rectangle.Width - EndArcWidth + Rectangle.X, Rectangle.Y, EndArcWidth, EndArcWidth), -90, 90);
            GP.AddArc(new Rectangle(Rectangle.Width - EndArcWidth + Rectangle.X, Rectangle.Height - EndArcWidth + Rectangle.Y, EndArcWidth, EndArcWidth), 0, 90);
            GP.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - EndArcWidth + Rectangle.Y, EndArcWidth, EndArcWidth), 90, 90);
            GP.AddLine(new Point(Rectangle.X, Rectangle.Height - EndArcWidth + Rectangle.Y), new Point(Rectangle.X, Curve + Rectangle.Y));
            return GP;
        }

        public static GraphicsPath RoundRect(int X, int Y, int Width, int Height, int Curve)
        {
            Rectangle Rectangle = new Rectangle(X, Y, Width, Height);
            GraphicsPath GP = new GraphicsPath();
            int EndArcWidth = Curve * 2;
            GP.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, EndArcWidth, EndArcWidth), -180, 90);
            GP.AddArc(new Rectangle(Rectangle.Width - EndArcWidth + Rectangle.X, Rectangle.Y, EndArcWidth, EndArcWidth), -90, 90);
            GP.AddArc(new Rectangle(Rectangle.Width - EndArcWidth + Rectangle.X, Rectangle.Height - EndArcWidth + Rectangle.Y, EndArcWidth, EndArcWidth), 0, 90);
            GP.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - EndArcWidth + Rectangle.Y, EndArcWidth, EndArcWidth), 90, 90);
            GP.AddLine(new Point(Rectangle.X, Rectangle.Height - EndArcWidth + Rectangle.Y), new Point(Rectangle.X, Curve + Rectangle.Y));
            return GP;
        }
    }

    #endregion

    #region  Control Renderer

    #region  Color Table

    public abstract class xColorTable
    {
        public abstract Color TextColor { get; }
        public abstract Color Background { get; }
        public abstract Color SelectionBorder { get; }
        public abstract Color SelectionTopGradient { get; }
        public abstract Color SelectionMidGradient { get; }
        public abstract Color SelectionBottomGradient { get; }
        public abstract Color PressedBackground { get; }
        public abstract Color CheckedBackground { get; }
        public abstract Color CheckedSelectedBackground { get; }
        public abstract Color DropdownBorder { get; }
        public abstract Color Arrow { get; }
        public abstract Color OverflowBackground { get; }
    }

    public abstract class ColorTable
    {
        public abstract xColorTable CommonColorTable { get; }
        public abstract Color BackgroundTopGradient { get; }
        public abstract Color BackgroundBottomGradient { get; }
        public abstract Color DroppedDownItemBackground { get; }
        public abstract Color DropdownTopGradient { get; }
        public abstract Color DropdownBottomGradient { get; }
        public abstract Color Separator { get; }
        public abstract Color ImageMargin { get; }
    }

    public class MSColorTable : ColorTable
    {

        private xColorTable _CommonColorTable;

        public MSColorTable()
        {
            _CommonColorTable = new DefaultCColorTable();
        }

        public override xColorTable CommonColorTable
        {
            get
            {
                return _CommonColorTable;
            }
        }

        public override System.Drawing.Color BackgroundTopGradient
        {
            get
            {
                return Color.FromArgb(246, 246, 246);
            }
        }

        public override System.Drawing.Color BackgroundBottomGradient
        {
            get
            {
                return Color.FromArgb(226, 226, 226);
            }
        }

        public override System.Drawing.Color DropdownTopGradient
        {
            get
            {
                return Color.FromArgb(246, 246, 246);
            }
        }

        public override System.Drawing.Color DropdownBottomGradient
        {
            get
            {
                return Color.FromArgb(246, 246, 246);
            }
        }

        public override System.Drawing.Color DroppedDownItemBackground
        {
            get
            {
                return Color.FromArgb(240, 240, 240);
            }
        }

        public override System.Drawing.Color Separator
        {
            get
            {
                return Color.FromArgb(190, 195, 203);
            }
        }

        public override System.Drawing.Color ImageMargin
        {
            get
            {
                return Color.FromArgb(240, 240, 240);
            }
        }
    }

    public class DefaultCColorTable : xColorTable
    {

        public override System.Drawing.Color CheckedBackground
        {
            get
            {
                return Color.FromArgb(230, 230, 230);
            }
        }

        public override System.Drawing.Color CheckedSelectedBackground
        {
            get
            {
                return Color.FromArgb(230, 230, 230);
            }
        }

        public override System.Drawing.Color SelectionBorder
        {
            get
            {
                return Color.FromArgb(180, 180, 180);
            }
        }

        public override System.Drawing.Color SelectionTopGradient
        {
            get
            {
                return Color.FromArgb(240, 240, 240);
            }
        }

        public override System.Drawing.Color SelectionMidGradient
        {
            get
            {
                return Color.FromArgb(235, 235, 235);
            }
        }

        public override System.Drawing.Color SelectionBottomGradient
        {
            get
            {
                return Color.FromArgb(230, 230, 230);
            }
        }

        public override System.Drawing.Color PressedBackground
        {
            get
            {
                return Color.FromArgb(232, 232, 232);
            }
        }

        public override System.Drawing.Color TextColor
        {
            get
            {
                return Color.FromArgb(80, 80, 80);
            }
        }

        public override System.Drawing.Color Background
        {
            get
            {
                return Color.FromArgb(188, 199, 216);
            }
        }

        public override System.Drawing.Color DropdownBorder
        {
            get
            {
                return Color.LightGray;
            }
        }

        public override System.Drawing.Color Arrow
        {
            get
            {
                return Color.Black;
            }
        }

        public override System.Drawing.Color OverflowBackground
        {
            get
            {
                return Color.FromArgb(213, 220, 232);
            }
        }
    }

    #endregion
    #region  Renderer

    public class ControlRenderer : ToolStripProfessionalRenderer
    {

        public ControlRenderer()
            : this(new MSColorTable())
        {
        }

        public ControlRenderer(ColorTable ColorTable)
        {
            this.ColorTable = ColorTable;
        }

        private ColorTable _ColorTable;
        public new ColorTable ColorTable
        {
            get
            {
                if (_ColorTable == null)
                {
                    _ColorTable = new MSColorTable();
                }
                return _ColorTable;
            }
            set
            {
                _ColorTable = value;
            }
        }

        protected override void OnRenderToolStripBackground(System.Windows.Forms.ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBackground(e);

            // Menu strip bar gradient
            using (LinearGradientBrush LGB = new LinearGradientBrush(e.AffectedBounds, this.ColorTable.BackgroundTopGradient, this.ColorTable.BackgroundBottomGradient, LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(LGB, e.AffectedBounds);
            }

        }

        protected override void OnRenderToolStripBorder(System.Windows.Forms.ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip.Parent == null)
            {
                // Draw border around the menu drop-down
                Rectangle Rect = new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
                using (Pen P1 = new Pen(this.ColorTable.CommonColorTable.DropdownBorder))
                {
                    e.Graphics.DrawRectangle(P1, Rect);
                }


                // Fill the gap between menu drop-down and owner item
                using (SolidBrush B1 = new SolidBrush(this.ColorTable.DroppedDownItemBackground))
                {
                    e.Graphics.FillRectangle(B1, e.ConnectedArea);
                }

            }
        }

        protected override void OnRenderMenuItemBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Enabled)
            {
                if (e.Item.Selected)
                {
                    if (!e.Item.IsOnDropDown)
                    {
                        Rectangle SelRect = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1);
                        RectDrawing.DrawSelection(e.Graphics, this.ColorTable.CommonColorTable, SelRect);
                    }
                    else
                    {
                        Rectangle SelRect = new Rectangle(2, 0, e.Item.Width - 4, e.Item.Height - 1);
                        RectDrawing.DrawSelection(e.Graphics, this.ColorTable.CommonColorTable, SelRect);
                    }
                }

                if (((ToolStripMenuItem)e.Item).DropDown.Visible && !e.Item.IsOnDropDown)
                {
                    Rectangle BorderRect = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height);
                    // Fill the background
                    Rectangle BackgroundRect = new Rectangle(1, 1, e.Item.Width - 2, e.Item.Height + 2);
                    using (SolidBrush B1 = new SolidBrush(this.ColorTable.DroppedDownItemBackground))
                    {
                        e.Graphics.FillRectangle(B1, BackgroundRect);
                    }


                    // Draw border
                    using (Pen P1 = new Pen(this.ColorTable.CommonColorTable.DropdownBorder))
                    {
                        RectDrawing.DrawRoundedRectangle(e.Graphics, P1, System.Convert.ToSingle(BorderRect.X), System.Convert.ToSingle(BorderRect.Y), System.Convert.ToSingle(BorderRect.Width), System.Convert.ToSingle(BorderRect.Height), 2);
                    }

                }
                e.Item.ForeColor = this.ColorTable.CommonColorTable.TextColor;
            }
        }

        protected override void OnRenderItemText(System.Windows.Forms.ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = this.ColorTable.CommonColorTable.TextColor;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderItemCheck(System.Windows.Forms.ToolStripItemImageRenderEventArgs e)
        {
            base.OnRenderItemCheck(e);

            Rectangle rect = new Rectangle(3, 1, e.Item.Height - 3, e.Item.Height - 3);
            Color c = default(Color);

            if (e.Item.Selected)
            {
                c = this.ColorTable.CommonColorTable.CheckedSelectedBackground;
            }
            else
            {
                c = this.ColorTable.CommonColorTable.CheckedBackground;
            }

            using (SolidBrush b = new SolidBrush(c))
            {
                e.Graphics.FillRectangle(b, rect);
            }


            using (Pen p = new Pen(this.ColorTable.CommonColorTable.SelectionBorder))
            {
                e.Graphics.DrawRectangle(p, rect);
            }


            e.Graphics.DrawString("ü", new Font("Wingdings", 13, FontStyle.Regular), Brushes.Black, new Point(4, 2));
        }

        protected override void OnRenderSeparator(System.Windows.Forms.ToolStripSeparatorRenderEventArgs e)
        {
            base.OnRenderSeparator(e);
            int PT1 = 28;
            int PT2 = System.Convert.ToInt32(e.Item.Width);
            int Y = 3;
            using (Pen P1 = new Pen(this.ColorTable.Separator))
            {
                e.Graphics.DrawLine(P1, PT1, Y, PT2, Y);
            }

        }

        protected override void OnRenderImageMargin(System.Windows.Forms.ToolStripRenderEventArgs e)
        {
            base.OnRenderImageMargin(e);

            Rectangle BackgroundRect = new Rectangle(0, -1, e.ToolStrip.Width, e.ToolStrip.Height + 1);
            using (LinearGradientBrush LGB = new LinearGradientBrush(BackgroundRect,
                    this.ColorTable.DropdownTopGradient,
                    this.ColorTable.DropdownBottomGradient,
                    LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(LGB, BackgroundRect);
            }


            using (SolidBrush B1 = new SolidBrush(this.ColorTable.ImageMargin))
            {
                e.Graphics.FillRectangle(B1, e.AffectedBounds);
            }

        }

        protected override void OnRenderButtonBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1);
            bool @checked = System.Convert.ToBoolean(((ToolStripButton)e.Item).Checked);
            bool drawBorder = false;

            if (@checked)
            {
                drawBorder = true;

                if (e.Item.Selected && !e.Item.Pressed)
                {
                    using (SolidBrush b = new SolidBrush(this.ColorTable.CommonColorTable.CheckedSelectedBackground))
                    {
                        e.Graphics.FillRectangle(b, rect);
                    }

                }
                else
                {
                    using (SolidBrush b = new SolidBrush(this.ColorTable.CommonColorTable.CheckedBackground))
                    {
                        e.Graphics.FillRectangle(b, rect);
                    }

                }

            }
            else
            {

                if (e.Item.Pressed)
                {
                    drawBorder = true;
                    using (SolidBrush b = new SolidBrush(this.ColorTable.CommonColorTable.PressedBackground))
                    {
                        e.Graphics.FillRectangle(b, rect);
                    }

                }
                else if (e.Item.Selected)
                {
                    drawBorder = true;
                    RectDrawing.DrawSelection(e.Graphics, this.ColorTable.CommonColorTable, rect);
                }

            }

            if (drawBorder)
            {
                using (Pen p = new Pen(this.ColorTable.CommonColorTable.SelectionBorder))
                {
                    e.Graphics.DrawRectangle(p, rect);
                }

            }
        }

        protected override void OnRenderDropDownButtonBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1);
            bool drawBorder = false;

            if (e.Item.Pressed)
            {
                drawBorder = true;
                using (SolidBrush b = new SolidBrush(this.ColorTable.CommonColorTable.PressedBackground))
                {
                    e.Graphics.FillRectangle(b, rect);
                }

            }
            else if (e.Item.Selected)
            {
                drawBorder = true;
                RectDrawing.DrawSelection(e.Graphics, this.ColorTable.CommonColorTable, rect);
            }

            if (drawBorder)
            {
                using (Pen p = new Pen(this.ColorTable.CommonColorTable.SelectionBorder))
                {
                    e.Graphics.DrawRectangle(p, rect);
                }

            }
        }

        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderSplitButtonBackground(e);
            bool drawBorder = false;
            bool drawSeparator = true;
            ToolStripSplitButton item = (ToolStripSplitButton)e.Item;
            checked
            {
                Rectangle btnRect = new Rectangle(0, 0, item.ButtonBounds.Width - 1, item.ButtonBounds.Height - 1);
                Rectangle borderRect = new Rectangle(0, 0, item.Bounds.Width - 1, item.Bounds.Height - 1);
                bool flag = item.DropDownButtonPressed;
                if (flag)
                {
                    drawBorder = true;
                    drawSeparator = false;
                    SolidBrush b = new SolidBrush(this.ColorTable.CommonColorTable.PressedBackground);
                    try
                    {
                        e.Graphics.FillRectangle(b, borderRect);
                    }
                    finally
                    {
                        flag = (b != null);
                        if (flag)
                        {
                            ((IDisposable)b).Dispose();
                        }
                    }
                }
                else
                {
                    flag = item.DropDownButtonSelected;
                    if (flag)
                    {
                        drawBorder = true;
                        RectDrawing.DrawSelection(e.Graphics, this.ColorTable.CommonColorTable, borderRect);
                    }
                }
                flag = item.ButtonPressed;
                if (flag)
                {
                    SolidBrush b2 = new SolidBrush(this.ColorTable.CommonColorTable.PressedBackground);
                    try
                    {
                        e.Graphics.FillRectangle(b2, btnRect);
                    }
                    finally
                    {
                        flag = (b2 != null);
                        if (flag)
                        {
                            ((IDisposable)b2).Dispose();
                        }
                    }
                }
                flag = drawBorder;
                if (flag)
                {
                    Pen p = new Pen(this.ColorTable.CommonColorTable.SelectionBorder);
                    try
                    {
                        e.Graphics.DrawRectangle(p, borderRect);
                        flag = drawSeparator;
                        if (flag)
                        {
                            e.Graphics.DrawRectangle(p, btnRect);
                        }
                    }
                    finally
                    {
                        flag = (p != null);
                        if (flag)
                        {
                            ((IDisposable)p).Dispose();
                        }
                    }
                    this.DrawCustomArrow(e.Graphics, item);
                }
            }
        }


        private void DrawCustomArrow(Graphics g, ToolStripSplitButton item)
        {
            int dropWidth = System.Convert.ToInt32(item.DropDownButtonBounds.Width - 1);
            int dropHeight = System.Convert.ToInt32(item.DropDownButtonBounds.Height - 1);
            float triangleWidth = dropWidth / 2.0F + 1;
            float triangleLeft = System.Convert.ToSingle(item.DropDownButtonBounds.Left + (dropWidth - triangleWidth) / 2.0F);
            float triangleHeight = triangleWidth / 2.0F;
            float triangleTop = System.Convert.ToSingle(item.DropDownButtonBounds.Top + (dropHeight - triangleHeight) / 2.0F + 1);
            RectangleF arrowRect = new RectangleF(triangleLeft, triangleTop, triangleWidth, triangleHeight);

            this.DrawCustomArrow(g, item, Rectangle.Round(arrowRect));
        }

        private void DrawCustomArrow(Graphics g, ToolStripItem item, Rectangle rect)
        {
            ToolStripArrowRenderEventArgs arrowEventArgs = new ToolStripArrowRenderEventArgs(g, item, rect, this.ColorTable.CommonColorTable.Arrow, ArrowDirection.Down);
            base.OnRenderArrow(arrowEventArgs);
        }

        protected override void OnRenderOverflowButtonBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
        {
            Rectangle rect = default(Rectangle);
            Rectangle rectEnd = default(Rectangle);
            rect = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 2);
            rectEnd = new Rectangle(rect.X - 5, rect.Y, rect.Width - 5, rect.Height);

            if (e.Item.Pressed)
            {
                using (SolidBrush b = new SolidBrush(this.ColorTable.CommonColorTable.PressedBackground))
                {
                    e.Graphics.FillRectangle(b, rect);
                }

            }
            else if (e.Item.Selected)
            {
                RectDrawing.DrawSelection(e.Graphics, this.ColorTable.CommonColorTable, rect);
            }
            else
            {
                using (SolidBrush b = new SolidBrush(this.ColorTable.CommonColorTable.OverflowBackground))
                {
                    e.Graphics.FillRectangle(b, rect);
                }

            }

            using (Pen P1 = new Pen(this.ColorTable.CommonColorTable.Background))
            {
                RectDrawing.DrawRoundedRectangle(e.Graphics, P1, System.Convert.ToSingle(rectEnd.X), System.Convert.ToSingle(rectEnd.Y), System.Convert.ToSingle(rectEnd.Width), System.Convert.ToSingle(rectEnd.Height), 3);
            }


            // Icon
            int w = System.Convert.ToInt32(rect.Width - 1);
            int h = System.Convert.ToInt32(rect.Height - 1);
            float triangleWidth = w / 2.0F + 1;
            float triangleLeft = System.Convert.ToSingle(rect.Left + (w - triangleWidth) / 2.0F + 3);
            float triangleHeight = triangleWidth / 2.0F;
            float triangleTop = System.Convert.ToSingle(rect.Top + (h - triangleHeight) / 2.0F + 7);
            RectangleF arrowRect = new RectangleF(triangleLeft, triangleTop, triangleWidth, triangleHeight);
            this.DrawCustomArrow(e.Graphics, e.Item, Rectangle.Round(arrowRect));

            using (Pen p = new Pen(this.ColorTable.CommonColorTable.Arrow))
            {
                e.Graphics.DrawLine(p, triangleLeft + 2, triangleTop - 2, triangleLeft + triangleWidth - 2, triangleTop - 2);
            }

        }
    }

    #endregion
    #region  Drawing

    public class RectDrawing
    {

        public static void DrawSelection(Graphics G, xColorTable ColorTable, Rectangle Rect)
        {
            Rectangle TopRect = default(Rectangle);
            Rectangle BottomRect = default(Rectangle);
            Rectangle FillRect = new Rectangle(Rect.X + 1, Rect.Y + 1, Rect.Width - 1, Rect.Height - 1);

            TopRect = FillRect;
            TopRect.Height -= System.Convert.ToInt32(TopRect.Height / 2);
            BottomRect = new Rectangle(TopRect.X, TopRect.Bottom, TopRect.Width, FillRect.Height - TopRect.Height);

            // Top gradient
            using (LinearGradientBrush LGB = new LinearGradientBrush(TopRect, ColorTable.SelectionTopGradient, ColorTable.SelectionMidGradient, LinearGradientMode.Vertical))
            {
                G.FillRectangle(LGB, TopRect);
            }


            // Bottom
            using (SolidBrush B1 = new SolidBrush(ColorTable.SelectionBottomGradient))
            {
                G.FillRectangle(B1, BottomRect);
            }


            // Border
            using (Pen P1 = new Pen(ColorTable.SelectionBorder))
            {
                RectDrawing.DrawRoundedRectangle(G, P1, System.Convert.ToSingle(Rect.X), System.Convert.ToSingle(Rect.Y), System.Convert.ToSingle(Rect.Width), System.Convert.ToSingle(Rect.Height), 2);
            }

        }

        public static void DrawRoundedRectangle(Graphics G, Pen P, float X, float Y, float W, float H, float Rad)
        {

            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddLine(X + Rad, Y, X + W - (Rad * 2), Y);
                gp.AddArc(X + W - (Rad * 2), Y, Rad * 2, Rad * 2, 270, 90);
                gp.AddLine(X + W, Y + Rad, X + W, Y + H - (Rad * 2));
                gp.AddArc(X + W - (Rad * 2), Y + H - (Rad * 2), Rad * 2, Rad * 2, 0, 90);
                gp.AddLine(X + W - (Rad * 2), Y + H, X + Rad, Y + H);
                gp.AddArc(X, Y + H - (Rad * 2), Rad * 2, Rad * 2, 90, 90);
                gp.AddLine(X, Y + H - (Rad * 2), X, Y + Rad);
                gp.AddArc(X, Y, Rad * 2, Rad * 2, 180, 90);
                gp.CloseFigure();

                G.SmoothingMode = SmoothingMode.AntiAlias;
                G.DrawPath(P, gp);
                G.SmoothingMode = SmoothingMode.Default;
            }

        }
    }

    #endregion

    #endregion
    #endregion
    #region ThemeContainer

    public class __________WC_3_ForumTheme : ContainerControl
    {


        #region  Variables

        private Point MouseP = new Point(0, 0);
        private bool Cap = false;
        private int MoveHeight;
        private string _TextBottom = null;
        const int BorderCurve = 7;
        protected MouseState State;
        private bool HasShown;
        private Rectangle HeaderRect;

        #endregion
        #region  Enums

        public enum MouseState
        {
            None = 0,
            Over = 1,
            Down = 2
        }

        #endregion
        #region  Properties

        private bool _Sizable = true;
        public bool Sizable
        {
            get
            {
                return _Sizable;
            }
            set
            {
                _Sizable = value;
            }
        }

        private bool _SmartBounds = false;
        public bool SmartBounds
        {
            get
            {
                return _SmartBounds;
            }
            set
            {
                _SmartBounds = value;
            }
        }

        private bool _IsParentForm;
        protected bool IsParentForm
        {
            get
            {
                return _IsParentForm;
            }
        }

        protected bool IsParentMdi
        {
            get
            {
                if (Parent == null)
                {
                    return false;
                }
                return Parent.Parent != null;
            }
        }

        private bool _ControlMode;
        protected bool ControlMode
        {
            get
            {
                return _ControlMode;
            }
            set
            {
                _ControlMode = value;
                Invalidate();
            }
        }

        private FormStartPosition _StartPosition;
        public FormStartPosition StartPosition
        {
            get
            {
                if (_IsParentForm && !_ControlMode)
                {
                    return ParentForm.StartPosition;
                }
                else
                {
                    return _StartPosition;
                }
            }
            set
            {
                _StartPosition = value;

                if (_IsParentForm && !_ControlMode)
                {
                    ParentForm.StartPosition = value;
                }
            }
        }

        #endregion
        #region  EventArgs

        protected sealed override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent == null)
            {
                return;
            }
            _IsParentForm = Parent is Form;

            if (!_ControlMode)
            {
                InitializeMessages();

                if (_IsParentForm)
                {
                    this.ParentForm.FormBorderStyle = FormBorderStyle.None;
                    this.ParentForm.TransparencyKey = Color.Fuchsia;

                    if (!DesignMode)
                    {
                        ParentForm.Shown += FormShown;
                    }
                }
                Parent.BackColor = BackColor;
                Parent.MinimumSize = new Size(126, 39);
            }
        }

        protected sealed override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (!_ControlMode)
            {
                HeaderRect = new Rectangle(0, 0, Width - 14, MoveHeight - 7);
            }
            Invalidate();
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                SetState(MouseState.Down);
            }
            if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized || _ControlMode))
            {
                if (HeaderRect.Contains(e.Location))
                {
                    Capture = false;
                    WM_LMBUTTONDOWN = true;
                    DefWndProc(ref Messages[0]);
                }
                else if (_Sizable && !(Previous == 0))
                {
                    Capture = false;
                    WM_LMBUTTONDOWN = true;
                    DefWndProc(ref Messages[Previous]);
                }
            }
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Cap = false;
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized))
            {
                if (_Sizable && !_ControlMode)
                {
                    InvalidateMouse();
                }
            }
            if (Cap)
            {
                Parent.Location = (System.Drawing.Point)((object)(System.Convert.ToDouble(MousePosition) - System.Convert.ToDouble(MouseP)));
            }
        }

        protected override void OnInvalidated(System.Windows.Forms.InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
            ParentForm.Text = Text;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        private void FormShown(object sender, EventArgs e)
        {
            if (_ControlMode || HasShown)
            {
                return;
            }

            if (_StartPosition == FormStartPosition.CenterParent || _StartPosition == FormStartPosition.CenterScreen)
            {
                Rectangle SB = Screen.PrimaryScreen.Bounds;
                Rectangle CB = ParentForm.Bounds;
                ParentForm.Location = new Point(SB.Width / 2 - CB.Width / 2, SB.Height / 2 - CB.Width / 2);
            }
            HasShown = true;
        }

        #endregion
        #region  Mouse & Size

        private void SetState(MouseState current)
        {
            State = current;
            Invalidate();
        }

        private Point GetIndexPoint;
        private bool B1x;
        private bool B2x;
        private bool B3;
        private bool B4;
        private int GetIndex()
        {
            GetIndexPoint = PointToClient(MousePosition);
            B1x = GetIndexPoint.X < 7;
            B2x = GetIndexPoint.X > Width - 7;
            B3 = GetIndexPoint.Y < 7;
            B4 = GetIndexPoint.Y > Height - 7;

            if (B1x && B3)
            {
                return 4;
            }
            if (B1x && B4)
            {
                return 7;
            }
            if (B2x && B3)
            {
                return 5;
            }
            if (B2x && B4)
            {
                return 8;
            }
            if (B1x)
            {
                return 1;
            }
            if (B2x)
            {
                return 2;
            }
            if (B3)
            {
                return 3;
            }
            if (B4)
            {
                return 6;
            }
            return 0;
        }

        private int Current;
        private int Previous;
        private void InvalidateMouse()
        {
            Current = GetIndex();
            if (Current == Previous)
            {
                return;
            }

            Previous = Current;
            switch (Previous)
            {
                case 0:
                    Cursor = Cursors.Default;
                    break;
                case 6:
                    Cursor = Cursors.SizeNS;
                    break;
                case 8:
                    Cursor = Cursors.SizeNWSE;
                    break;
                case 7:
                    Cursor = Cursors.SizeNESW;
                    break;
            }
        }

        private Message[] Messages = new Message[9];
        private void InitializeMessages()
        {
            Messages[0] = Message.Create(Parent.Handle, 161, new IntPtr(2), IntPtr.Zero);
            for (int I = 1; I <= 8; I++)
            {
                Messages[I] = Message.Create(Parent.Handle, 161, new IntPtr(I + 9), IntPtr.Zero);
            }
        }

        private void CorrectBounds(Rectangle bounds)
        {
            if (Parent.Width > bounds.Width)
            {
                Parent.Width = bounds.Width;
            }
            if (Parent.Height > bounds.Height)
            {
                Parent.Height = bounds.Height;
            }

            int X = Parent.Location.X;
            int Y = Parent.Location.Y;

            if (X < bounds.X)
            {
                X = bounds.X;
            }
            if (Y < bounds.Y)
            {
                Y = bounds.Y;
            }

            int Width = bounds.X + bounds.Width;
            int Height = bounds.Y + bounds.Height;

            if (X + Parent.Width > Width)
            {
                X = Width - Parent.Width;
            }
            if (Y + Parent.Height > Height)
            {
                Y = Height - Parent.Height;
            }

            Parent.Location = new Point(X, Y);
        }

        private bool WM_LMBUTTONDOWN;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (WM_LMBUTTONDOWN && m.Msg == 513)
            {
                WM_LMBUTTONDOWN = false;

                SetState(MouseState.Over);
                if (!_SmartBounds)
                {
                    return;
                }

                if (IsParentMdi)
                {
                    CorrectBounds(new Rectangle(Point.Empty, Parent.Parent.Size));
                }
                else
                {
                    CorrectBounds(Screen.FromControl(Parent).WorkingArea);
                }
            }
        }

        #endregion

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.ParentForm.FormBorderStyle = FormBorderStyle.None;
            this.ParentForm.TransparencyKey = Color.Fuchsia;
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
        }

        public __________WC_3_ForumTheme()
        {
            SetStyle((ControlStyles)(139270), true);
            Dock = DockStyle.Fill;
            MoveHeight = 25;
            Padding = new Padding(3, 28, 3, 28);
            Font = new Font("Segoe UI", 8, FontStyle.Regular);
            ForeColor = Color.FromArgb(142, 142, 142);
            BackColor = Color.FromArgb(246, 246, 246);
            DoubleBuffered = true;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);
            Rectangle ClientRectangle = new Rectangle(0, 0, Width - 1, Height - 1);
            Color TransparencyKey = this.ParentForm.TransparencyKey;

            G.SmoothingMode = SmoothingMode.Default;
            G.Clear(TransparencyKey);

            // Draw the container borders
            G.FillPath(new SolidBrush(Color.FromArgb(52, 52, 52)), RoundRectangle.RoundRect(ClientRectangle, BorderCurve));
            // Draw a rectangle in which the controls should be added on
            G.FillPath(new SolidBrush(Color.FromArgb(246, 246, 246)), RoundRectangle.RoundRect(new Rectangle(2, 20, Width - 5, Height - 42), BorderCurve));

            // Patch the header with a rectangle that has a curve so its border will remain within container bounds
            G.FillPath(new SolidBrush(Color.FromArgb(52, 52, 52)), RoundRectangle.RoundRect(new Rectangle(2, 2, (int)(Width / 2 + 2), 16), BorderCurve));
            G.FillPath(new SolidBrush(Color.FromArgb(52, 52, 52)), RoundRectangle.RoundRect(new Rectangle((int)(Width / 2 - 3), 2, (int)(Width / 2), 16), BorderCurve));
            // Fill the header rectangle below the patch
            G.FillRectangle(new SolidBrush(Color.FromArgb(52, 52, 52)), new Rectangle(2, 15, Width - 5, 10));

            // Increase the thickness of the container borders
            G.DrawPath(new Pen(Color.FromArgb(52, 52, 52)), RoundRectangle.RoundRect(new Rectangle(2, 2, Width - 5, Height - 5), BorderCurve));
            G.DrawPath(new Pen(Color.FromArgb(52, 52, 52)), RoundRectangle.RoundRect(ClientRectangle, BorderCurve));

            // Draw the string from the specified 'Text' property on the header rectangle
            G.DrawString(Text, new Font("Trebuchet MS", 10, FontStyle.Bold), new SolidBrush(Color.FromArgb(221, 221, 221)), new Rectangle(BorderCurve, BorderCurve - 4, Width - 1, 22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near });


            // Draws a rectangle at the bottom of the container
            G.FillRectangle(new SolidBrush(Color.FromArgb(52, 52, 52)), 0, Height - 25, Width - 3, 22 - 2);
            G.DrawLine(new Pen(Color.FromArgb(52, 52, 52)), 5, Height - 5, Width - 6, Height - 5);
            G.DrawLine(new Pen(Color.FromArgb(52, 52, 52)), 7, Height - 4, Width - 7, Height - 4);

            G.DrawString(_TextBottom, new Font("Trebuchet MS", 10, FontStyle.Bold), new SolidBrush(Color.FromArgb(221, 221, 221)), 5, Height - 23);

            e.Graphics.DrawImage((Image)(B.Clone()), 0, 0);
            G.Dispose();
            B.Dispose();
        }
    }

    #endregion
    #region ControlBox

    public class WC_3_ControlBox : Control
    {

        #region Enums

        public enum MouseState : byte
        {
            None = 0,
            Over = 1,
            Down = 2,
            Block = 3
        }

        #endregion
        #region Variables

        MouseState State = MouseState.None;
        int i;
        Rectangle CloseRect = new Rectangle(28, 0, 47, 18);
        Rectangle MinimizeRect = new Rectangle(0, 0, 28, 18);

        #endregion
        #region EventArgs

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (i > 0 & i < 28)
            {
                this.FindForm().WindowState = FormWindowState.Minimized;
            }
            else if (i > 30 & i < 75)
            {
                this.FindForm().Close();
            }

            State = MouseState.Down;
        }

        protected override void OnMouseEnter(System.EventArgs e)
        {
            base.OnMouseEnter(e);
            State = MouseState.Over;
            Invalidate();
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);
            State = MouseState.None;
            Invalidate();
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            State = MouseState.Over;
            Invalidate();
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            i = e.Location.X;
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Width = 77;
            Height = 19;
        }

        #endregion

        public WC_3_ControlBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
            DoubleBuffered = true;
            Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Point location = new Point(checked(this.FindForm().Width - 81), 4);
            this.Location = location;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);
            GraphicsPath GP_MinimizeRect = new GraphicsPath();
            GraphicsPath GP_CloseRect = new GraphicsPath();

            GP_MinimizeRect.AddRectangle(MinimizeRect);
            GP_CloseRect.AddRectangle(CloseRect);
            G.Clear(BackColor);

            switch (State)
            {
                case MouseState.None:
                NonePoint:
                    LinearGradientBrush MinimizeGradient = new LinearGradientBrush(MinimizeRect, Color.FromArgb(73, 73, 73), Color.FromArgb(58, 58, 58), 90);
                    G.FillPath(MinimizeGradient, GP_MinimizeRect);
                    G.DrawPath(new Pen(Color.FromArgb(40, 40, 40)), GP_MinimizeRect);
                    G.DrawString("0", new Font("Marlett", 11, FontStyle.Regular), new SolidBrush(Color.FromArgb(221, 221, 221)), MinimizeRect.Width - 22, MinimizeRect.Height - 16);

                    LinearGradientBrush CloseGradient = new LinearGradientBrush(CloseRect, Color.FromArgb(73, 73, 73), Color.FromArgb(58, 58, 58), 90);
                    G.FillPath(CloseGradient, GP_CloseRect);
                    G.DrawPath(new Pen(Color.FromArgb(40, 40, 40)), GP_CloseRect);
                    G.DrawString("r", new Font("Marlett", 11, FontStyle.Regular), new SolidBrush(Color.FromArgb(221, 221, 221)), CloseRect.Width - 4, CloseRect.Height - 16);
                    break;
                case MouseState.Over:
                    if (i > 0 & i < 28)
                    {
                        LinearGradientBrush xMinimizeGradient = new LinearGradientBrush(this.MinimizeRect, Color.FromArgb(76, 76, 76, 76), Color.FromArgb(48, 48, 48), 90f);
                        G.FillPath(xMinimizeGradient, GP_MinimizeRect);
                        G.DrawPath(new Pen(Color.FromArgb(40, 40, 40)), GP_MinimizeRect);
                        G.DrawString("0", new Font("Marlett", 11, FontStyle.Regular), new SolidBrush(Color.FromArgb(221, 221, 221)), MinimizeRect.Width - 22, MinimizeRect.Height - 16);

                        LinearGradientBrush xCloseGradient = new LinearGradientBrush(CloseRect, Color.FromArgb(73, 73, 73), Color.FromArgb(58, 58, 58), 90);
                        G.FillPath(xCloseGradient, GP_CloseRect);
                        G.DrawPath(new Pen(Color.FromArgb(40, 40, 40)), GP_CloseRect);
                        G.DrawString("r", new Font("Marlett", 11, FontStyle.Regular), new SolidBrush(Color.FromArgb(221, 221, 221)), CloseRect.Width - 4, CloseRect.Height - 16);
                    }
                    else if (i > 30 & i < 75)
                    {
                        LinearGradientBrush xCloseGradient = new LinearGradientBrush(CloseRect, Color.FromArgb(76, 76, 76, 76), Color.FromArgb(48, 48, 48), 90);
                        G.FillPath(xCloseGradient, GP_CloseRect);
                        G.DrawPath(new Pen(Color.FromArgb(40, 40, 40)), GP_CloseRect);
                        G.DrawString("r", new Font("Marlett", 11, FontStyle.Regular), new SolidBrush(Color.FromArgb(221, 221, 221)), CloseRect.Width - 4, CloseRect.Height - 16);

                        LinearGradientBrush xMinimizeGradient = new LinearGradientBrush(MinimizeRect, Color.FromArgb(73, 73, 73), Color.FromArgb(58, 58, 58), 90);
                        G.FillPath(xMinimizeGradient, RoundRectangle.RoundRect(MinimizeRect, 1));
                        G.DrawPath(new Pen(Color.FromArgb(40, 40, 40)), GP_MinimizeRect);
                        G.DrawString("0", new Font("Marlett", 11, FontStyle.Regular), new SolidBrush(Color.FromArgb(221, 221, 221)), MinimizeRect.Width - 22, MinimizeRect.Height - 16);
                    }
                    else
                    {
                        goto NonePoint; // Return to [MouseState = None]     
                    }
                    break;
            }

            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            G.Dispose();
            GP_CloseRect.Dispose();
            GP_MinimizeRect.Dispose();
            B.Dispose();
        }
    }

    #endregion
    #region Button 1

    class WC_3_Button_1 : Control
    {

        #region Variables

        private int MouseState;
        private GraphicsPath Shape;
        private LinearGradientBrush InactiveGB;
        private LinearGradientBrush PressedGB;
        private LinearGradientBrush PressedContourGB;
        private Rectangle R1;
        private Pen P1;
        private Pen P3;
        private Image _Image;
        private Size _ImageSize;
        private StringAlignment _TextAlignment = StringAlignment.Center;
        private Color _TextColor = Color.FromArgb(150, 150, 150);
        private ContentAlignment _ImageAlign = ContentAlignment.MiddleLeft;

        #endregion
        #region Image Designer

        private static PointF ImageLocation(StringFormat SF, SizeF Area, SizeF ImageArea)
        {
            PointF MyPoint = default(PointF);
            switch (SF.Alignment)
            {
                case StringAlignment.Center:
                    MyPoint.X = Convert.ToSingle((Area.Width - ImageArea.Width) / 2);
                    break;
                case StringAlignment.Near:
                    MyPoint.X = 2;
                    break;
                case StringAlignment.Far:
                    MyPoint.X = Area.Width - ImageArea.Width - 2;

                    break;
            }

            switch (SF.LineAlignment)
            {
                case StringAlignment.Center:
                    MyPoint.Y = Convert.ToSingle((Area.Height - ImageArea.Height) / 2);
                    break;
                case StringAlignment.Near:
                    MyPoint.Y = 2;
                    break;
                case StringAlignment.Far:
                    MyPoint.Y = Area.Height - ImageArea.Height - 2;
                    break;
            }
            return MyPoint;
        }

        private StringFormat GetStringFormat(ContentAlignment _ContentAlignment)
        {
            StringFormat SF = new StringFormat();
            switch (_ContentAlignment)
            {
                case ContentAlignment.MiddleCenter:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleLeft:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleRight:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.TopCenter:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.TopLeft:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomRight:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Far;
                    break;
            }
            return SF;
        }

        #endregion
        #region Properties

        public Image Image
        {
            get { return _Image; }
            set
            {
                if (value == null)
                {
                    _ImageSize = Size.Empty;
                }
                else
                {
                    _ImageSize = value.Size;
                }

                _Image = value;
                Invalidate();
            }
        }

        protected Size ImageSize
        {
            get { return _ImageSize; }
        }

        public ContentAlignment ImageAlign
        {
            get { return _ImageAlign; }
            set
            {
                _ImageAlign = value;
                Invalidate();
            }
        }

        public StringAlignment TextAlignment
        {
            get { return this._TextAlignment; }
            set
            {
                this._TextAlignment = value;
                this.Invalidate();
            }
        }

        public override Color ForeColor
        {
            get { return this._TextColor; }
            set
            {
                this._TextColor = value;
                this.Invalidate();
            }
        }

        #endregion
        #region EventArgs

        protected override void OnMouseUp(MouseEventArgs e)
        {
            MouseState = 0;
            Invalidate();
            base.OnMouseUp(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            MouseState = 1;
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = 0;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }

        #endregion

        public WC_3_Button_1()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);

            BackColor = Color.Transparent;
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 12);
            ForeColor = Color.FromArgb(150, 150, 150);
            Size = new Size(166, 40);
            _TextAlignment = StringAlignment.Center;
            P1 = new Pen(Color.FromArgb(190, 190, 190)); // P1 = Border color
        }

        protected override void OnResize(System.EventArgs e)
        {
            if (Width > 0 && Height > 0)
            {
                Shape = new GraphicsPath();
                R1 = new Rectangle(0, 0, Width, Height);

                InactiveGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(251, 251, 251), Color.FromArgb(225, 225, 225), 90f);
                PressedGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(235, 235, 235), Color.FromArgb(223, 223, 223), 90f);
                PressedContourGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(167, 167, 167), Color.FromArgb(167, 167, 167), 90f);

                P3 = new Pen(PressedContourGB);
            }

            var _Shape = Shape;
            _Shape.AddArc(0, 0, 10, 10, 180, 90);
            _Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            _Shape.CloseAllFigures();

            Invalidate();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var _G = e.Graphics;
            _G.SmoothingMode = SmoothingMode.HighQuality;
            PointF ipt = ImageLocation(GetStringFormat(ImageAlign), Size, ImageSize);

            switch (MouseState)
            {
                case 0:
                    _G.FillPath(InactiveGB, Shape);
                    _G.DrawPath(P1, Shape);
                    if ((Image == null))
                    {
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        _G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
                case 1:
                    _G.FillPath(PressedGB, Shape);
                    _G.DrawPath(P3, Shape);

                    if ((Image == null))
                    {
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        _G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
            }
            base.OnPaint(e);
        }
    }

    #endregion
    #region Button 2

    class WC_3_Button_2 : Control
    {

        #region Variables

        private int MouseState;
        private GraphicsPath Shape;
        private LinearGradientBrush InactiveGB;
        private LinearGradientBrush PressedGB;
        private LinearGradientBrush PressedContourGB;
        private Rectangle R1;
        private Pen P1;
        private Pen P3;
        private Image _Image;
        private Size _ImageSize;
        private StringAlignment _TextAlignment = StringAlignment.Center;
        private ContentAlignment _ImageAlign = ContentAlignment.MiddleLeft;

        #endregion
        #region Image Designer

        private static PointF ImageLocation(StringFormat SF, SizeF Area, SizeF ImageArea)
        {
            PointF MyPoint = default(PointF);
            switch (SF.Alignment)
            {
                case StringAlignment.Center:
                    MyPoint.X = Convert.ToSingle((Area.Width - ImageArea.Width) / 2);
                    break;
                case StringAlignment.Near:
                    MyPoint.X = 2;
                    break;
                case StringAlignment.Far:
                    MyPoint.X = Area.Width - ImageArea.Width - 2;
                    break;
            }

            switch (SF.LineAlignment)
            {
                case StringAlignment.Center:
                    MyPoint.Y = Convert.ToSingle((Area.Height - ImageArea.Height) / 2);
                    break;
                case StringAlignment.Near:
                    MyPoint.Y = 2;
                    break;
                case StringAlignment.Far:
                    MyPoint.Y = Area.Height - ImageArea.Height - 2;
                    break;
            }
            return MyPoint;
        }

        private StringFormat GetStringFormat(ContentAlignment _ContentAlignment)
        {
            StringFormat SF = new StringFormat();
            switch (_ContentAlignment)
            {
                case ContentAlignment.MiddleCenter:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleLeft:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleRight:
                    SF.LineAlignment = StringAlignment.Center;
                    SF.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.TopCenter:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.TopLeft:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    SF.LineAlignment = StringAlignment.Near;
                    SF.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomRight:
                    SF.LineAlignment = StringAlignment.Far;
                    SF.Alignment = StringAlignment.Far;
                    break;
            }
            return SF;
        }

        #endregion
        #region Properties

        public Image Image
        {
            get { return _Image; }
            set
            {
                if (value == null)
                {
                    _ImageSize = Size.Empty;
                }
                else
                {
                    _ImageSize = value.Size;
                }

                _Image = value;
                Invalidate();
            }
        }

        public StringAlignment TextAlignment
        {
            get { return this._TextAlignment; }
            set
            {
                this._TextAlignment = value;
                this.Invalidate();
            }
        }

        protected Size ImageSize
        {
            get { return _ImageSize; }
        }

        public ContentAlignment ImageAlign
        {
            get { return _ImageAlign; }
            set
            {
                _ImageAlign = value;
                Invalidate();
            }
        }

        #endregion
        #region EventArgs

        protected override void OnMouseUp(MouseEventArgs e)
        {
            MouseState = 0;
            Invalidate();
            base.OnMouseUp(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            MouseState = 1;
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            MouseState = 0;
            // [Inactive]
            Invalidate();
            // Update control
            base.OnMouseLeave(e);
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }

        #endregion

        public WC_3_Button_2()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);

            BackColor = Color.Transparent;
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 14);
            ForeColor = Color.White;
            Size = new Size(166, 40);
            _TextAlignment = StringAlignment.Center;
            P1 = new Pen(Color.FromArgb(0, 118, 176));
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            if (Width > 0 && Height > 0)
            {
                Shape = new GraphicsPath();
                R1 = new Rectangle(0, 0, Width, Height);

                InactiveGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(0, 176, 231), Color.FromArgb(0, 152, 224), 90f);
                PressedGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(0, 118, 176), Color.FromArgb(0, 149, 222), 90f);
                PressedContourGB = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(0, 118, 176), Color.FromArgb(0, 118, 176), 90f);

                P3 = new Pen(PressedContourGB);
            }

            var _Shape = Shape;
            _Shape.AddArc(0, 0, 10, 10, 180, 90);
            _Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            _Shape.CloseAllFigures();

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var _G = e.Graphics;
            _G.SmoothingMode = SmoothingMode.HighQuality;

            PointF ipt = ImageLocation(GetStringFormat(ImageAlign), Size, ImageSize);

            switch (MouseState)
            {
                case 0:
                    _G.FillPath(InactiveGB, Shape);
                    _G.DrawPath(P1, Shape);
                    if ((Image == null))
                    {
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        _G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
                case 1:
                    _G.FillPath(PressedGB, Shape);
                    _G.DrawPath(P3, Shape);
                    if ((Image == null))
                    {
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    else
                    {
                        _G.DrawImage(_Image, ipt.X, ipt.Y, ImageSize.Width, ImageSize.Height);
                        _G.DrawString(Text, Font, new SolidBrush(ForeColor), R1, new StringFormat
                        {
                            Alignment = _TextAlignment,
                            LineAlignment = StringAlignment.Center
                        });
                    }
                    break;
            }
            base.OnPaint(e);
        }
    }

    #endregion
    #region Toggle Button

    [DefaultEvent("ToggledChanged")]
    class WC_3_Toggle : Control
    {

        #region Designer

        public class PillStyle
        {
            public bool Left;
            public bool Right;
        }

        public GraphicsPath Pill(Rectangle Rectangle, PillStyle PillStyle)
        {
            GraphicsPath functionReturnValue = default(GraphicsPath);
            functionReturnValue = new GraphicsPath();

            if (PillStyle.Left)
            {
                functionReturnValue.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Height, Rectangle.Height), -270, 180);
            }
            else
            {
                functionReturnValue.AddLine(Rectangle.X, Rectangle.Y + Rectangle.Height, Rectangle.X, Rectangle.Y);
            }

            if (PillStyle.Right)
            {
                functionReturnValue.AddArc(new Rectangle(Rectangle.X + Rectangle.Width - Rectangle.Height, Rectangle.Y, Rectangle.Height, Rectangle.Height), -90, 180);
            }
            else
            {
                functionReturnValue.AddLine(Rectangle.X + Rectangle.Width, Rectangle.Y, Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height);
            }

            functionReturnValue.CloseAllFigures();
            return functionReturnValue;
        }

        public object Pill(int X, int Y, int Width, int Height, PillStyle PillStyle)
        {
            return Pill(new Rectangle(X, Y, Width, Height), PillStyle);
        }

        #endregion
        #region Enums

        public enum _Type
        {
            YesNo,
            OnOff,
            IO
        }

        #endregion
        #region Variables

        private Timer AnimationTimer = new Timer { Interval = 1 };
        private int ToggleLocation = 0;
        public event ToggledChangedEventHandler ToggledChanged;
        public delegate void ToggledChangedEventHandler();
        private bool _Toggled;
        private _Type ToggleType;
        private Rectangle Bar;
        private Size cHandle = new Size(15, 20);

        #endregion
        #region Properties

        public bool Toggled
        {
            get { return _Toggled; }
            set
            {
                _Toggled = value;
                Invalidate();

                if (ToggledChanged != null)
                {
                    ToggledChanged();
                }
            }
        }

        public _Type Type
        {
            get { return ToggleType; }
            set
            {
                ToggleType = value;
                Invalidate();
            }
        }

        #endregion
        #region EventArgs

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Width = 41;
            Height = 23;
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Toggled = !Toggled;
        }

        #endregion

        public WC_3_Toggle()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            AnimationTimer.Tick += new EventHandler(AnimationTimer_Tick);
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AnimationTimer.Start();
        }

        void AnimationTimer_Tick(object sender, EventArgs e)
        {
            //  Create a slide animation when toggled on/off
            if ((_Toggled == true))
            {
                if ((ToggleLocation < 100))
                {
                    ToggleLocation += 10;
                    this.Invalidate(false);
                }
            }
            else if ((ToggleLocation > 0))
            {
                ToggleLocation -= 10;
                this.Invalidate(false);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;
            G.Clear(Parent.BackColor);
            checked
            {
                Point point = new Point(0, (int)Math.Round(unchecked((double)this.Height / 2.0 - (double)this.cHandle.Height / 2.0)));
                Point arg_A8_0 = point;
                Point point2 = new Point(0, (int)Math.Round(unchecked((double)this.Height / 2.0 + (double)this.cHandle.Height / 2.0)));
                LinearGradientBrush Gradient = new LinearGradientBrush(arg_A8_0, point2, Color.FromArgb(250, 250, 250), Color.FromArgb(240, 240, 240));
                this.Bar = new Rectangle(8, 10, this.Width - 21, this.Height - 21);

                G.SmoothingMode = SmoothingMode.AntiAlias;
                G.FillPath(Gradient, (GraphicsPath)this.Pill(0, (int)Math.Round(unchecked((double)this.Height / 2.0 - (double)this.cHandle.Height / 2.0)), this.Width - 1, this.cHandle.Height - 5, new WC_3_Toggle.PillStyle
                {
                    Left = true,
                    Right = true
                }));
                G.DrawPath(new Pen(Color.FromArgb(177, 177, 176)), (GraphicsPath)this.Pill(0, (int)Math.Round(unchecked((double)this.Height / 2.0 - (double)this.cHandle.Height / 2.0)), this.Width - 1, this.cHandle.Height - 5, new WC_3_Toggle.PillStyle
                {
                    Left = true,
                    Right = true
                }));
                Gradient.Dispose();
                switch (this.ToggleType)
                {
                    case WC_3_Toggle._Type.YesNo:
                        {
                            bool toggled = this.Toggled;
                            if (toggled)
                            {
                                G.DrawString("Yes", new Font("Segoe UI", 7f, FontStyle.Regular), Brushes.Gray, (float)(this.Bar.X + 7), (float)this.Bar.Y, new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                });
                            }
                            else
                            {
                                G.DrawString("No", new Font("Segoe UI", 7f, FontStyle.Regular), Brushes.Gray, (float)(this.Bar.X + 18), (float)this.Bar.Y, new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                });
                            }
                            break;
                        }
                    case WC_3_Toggle._Type.OnOff:
                        {
                            bool toggled = this.Toggled;
                            if (toggled)
                            {
                                G.DrawString("On", new Font("Segoe UI", 7f, FontStyle.Regular), Brushes.Gray, (float)(this.Bar.X + 7), (float)this.Bar.Y, new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                });
                            }
                            else
                            {
                                G.DrawString("Off", new Font("Segoe UI", 7f, FontStyle.Regular), Brushes.Gray, (float)(this.Bar.X + 18), (float)this.Bar.Y, new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                });
                            }
                            break;
                        }
                    case WC_3_Toggle._Type.IO:
                        {
                            bool toggled = this.Toggled;
                            if (toggled)
                            {
                                G.DrawString("I", new Font("Segoe UI", 7f, FontStyle.Regular), Brushes.Gray, (float)(this.Bar.X + 7), (float)this.Bar.Y, new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                });
                            }
                            else
                            {
                                G.DrawString("O", new Font("Segoe UI", 7f, FontStyle.Regular), Brushes.Gray, (float)(this.Bar.X + 18), (float)this.Bar.Y, new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                });
                            }
                            break;
                        }
                }
                G.FillEllipse(new SolidBrush(Color.FromArgb(249, 249, 249)), this.Bar.X + (int)Math.Round(unchecked((double)this.Bar.Width * ((double)this.ToggleLocation / 80.0))) - (int)Math.Round((double)this.cHandle.Width / 2.0), this.Bar.Y + (int)Math.Round((double)this.Bar.Height / 2.0) - (int)Math.Round(unchecked((double)this.cHandle.Height / 2.0 - 1.0)), this.cHandle.Width, this.cHandle.Height - 5);
                G.DrawEllipse(new Pen(Color.FromArgb(177, 177, 176)), this.Bar.X + (int)Math.Round(unchecked((double)this.Bar.Width * ((double)this.ToggleLocation / 80.0) - (double)checked((int)Math.Round((double)this.cHandle.Width / 2.0)))), this.Bar.Y + (int)Math.Round((double)this.Bar.Height / 2.0) - (int)Math.Round(unchecked((double)this.cHandle.Height / 2.0 - 1.0)), this.cHandle.Width, this.cHandle.Height - 5);
            }
        }
    }
    #endregion
    #region Label

    class WC_3_Label : Label
    {

        public WC_3_Label()
        {
            Font = new Font("Segoe UI", 8);
            ForeColor = Color.FromArgb(142, 142, 142);
            BackColor = Color.Transparent;
        }
    }

    #endregion
    #region Link Label

    class WC_3_LinkLabel : LinkLabel
    {

        public WC_3_LinkLabel()
        {
            Font = new Font("Segoe UI", 8, FontStyle.Regular);
            BackColor = Color.Transparent;
            LinkColor = Color.FromArgb(51, 153, 225);
            ActiveLinkColor = Color.FromArgb(0, 101, 202);
            VisitedLinkColor = Color.FromArgb(0, 101, 202);
            LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
        }
    }

    #endregion
    #region Header Label

    class WC_3_HeaderLabel : Label
    {

        public WC_3_HeaderLabel()
        {
            Font = new Font("Segoe UI", 25, FontStyle.Regular);
            ForeColor = Color.FromArgb(80, 80, 80);
            BackColor = Color.Transparent;
        }
    }

    #endregion
    #region Big TextBox

    [DefaultEvent("TextChanged")]
    class WC_3_TextBox_Big : Control
    {
        #region Variables

        public TextBox iTalkTB = new TextBox();
        private GraphicsPath Shape;
        private int _maxchars = 32767;
        private bool _ReadOnly;
        private bool _Multiline;
        private Image _Image;
        private Size _ImageSize;
        private HorizontalAlignment ALNType;
        private bool isPasswordMasked = false;
        private Pen P1;
        private SolidBrush B1;

        #endregion
        #region Properties

        public HorizontalAlignment TextAlignment
        {
            get { return ALNType; }
            set
            {
                ALNType = value;
                Invalidate();
            }
        }
        public int MaxLength
        {
            get { return _maxchars; }
            set
            {
                _maxchars = value;
                iTalkTB.MaxLength = MaxLength;
                Invalidate();
            }
        }

        public bool UseSystemPasswordChar
        {
            get { return isPasswordMasked; }
            set
            {
                iTalkTB.UseSystemPasswordChar = UseSystemPasswordChar;
                isPasswordMasked = value;
                Invalidate();
            }
        }
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                _ReadOnly = value;
                if (iTalkTB != null)
                {
                    iTalkTB.ReadOnly = value;
                }
            }
        }
        public bool Multiline
        {
            get { return _Multiline; }
            set
            {
                _Multiline = value;
                if (iTalkTB != null)
                {
                    iTalkTB.Multiline = value;

                    if (value)
                    {
                        iTalkTB.Height = Height - 23;
                    }
                    else
                    {
                        Height = iTalkTB.Height + 23;
                    }
                }

            }
        }

        public Image Image
        {
            get
            {
                return _Image;
            }
            set
            {
                if (value == null)
                {
                    _ImageSize = Size.Empty;
                }
                else
                {
                    _ImageSize = value.Size;
                }

                _Image = value;

                if (Image == null)
                {
                    iTalkTB.Location = new Point(8, 10);
                }
                else
                {
                    iTalkTB.Location = new Point(35, 11);
                }
                Invalidate();
            }
        }

        protected Size ImageSize
        {
            get
            {
                return _ImageSize;
            }
        }

        #endregion
        #region EventArgs

        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            iTalkTB.Text = Text;
            Invalidate();
        }

        private void OnBaseTextChanged(object s, EventArgs e)
        {
            Text = iTalkTB.Text;
        }

        protected override void OnForeColorChanged(System.EventArgs e)
        {
            base.OnForeColorChanged(e);
            iTalkTB.ForeColor = ForeColor;
            Invalidate();
        }

        protected override void OnFontChanged(System.EventArgs e)
        {
            base.OnFontChanged(e);
            iTalkTB.Font = Font;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        private void _OnKeyDown(object Obj, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                iTalkTB.SelectAll();
                e.SuppressKeyPress = true;
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                iTalkTB.Copy();
                e.SuppressKeyPress = true;
            }
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            if (_Multiline)
            {
                iTalkTB.Height = Height - 23;
            }
            else
            {
                Height = iTalkTB.Height + 23;
            }

            Shape = new GraphicsPath();
            var _with1 = Shape;
            _with1.AddArc(0, 0, 10, 10, 180, 90);
            _with1.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _with1.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _with1.AddArc(0, Height - 11, 10, 10, 90, 90);
            _with1.CloseAllFigures();
        }

        protected override void OnGotFocus(System.EventArgs e)
        {
            base.OnGotFocus(e);
            iTalkTB.Focus();
        }

        #endregion
        public void AddTextBox()
        {
            var _TB = iTalkTB;
            _TB.Location = new Point(7, 10);
            _TB.Text = string.Empty;
            _TB.BorderStyle = BorderStyle.None;
            _TB.TextAlign = HorizontalAlignment.Left;
            _TB.Font = new Font("Tahoma", 11);
            _TB.UseSystemPasswordChar = UseSystemPasswordChar;
            _TB.Multiline = false;
            iTalkTB.KeyDown += _OnKeyDown;
            iTalkTB.TextChanged += OnBaseTextChanged;
        }

        public WC_3_TextBox_Big()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            AddTextBox();
            Controls.Add(iTalkTB);

            P1 = new Pen(Color.FromArgb(180, 180, 180)); // P1 = Border color
            B1 = new SolidBrush(Color.White); // B1 = Rect Background color
            BackColor = Color.Transparent;
            ForeColor = Color.DimGray;

            Text = null;
            Font = new Font("Tahoma", 11);
            Size = new Size(135, 43);
            DoubleBuffered = true;
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);

            G.SmoothingMode = SmoothingMode.AntiAlias;

            if (Image == null)
            {
                iTalkTB.Width = Width - 18;
            }
            else
            {
                iTalkTB.Width = Width - 45;
            }

            iTalkTB.TextAlign = TextAlignment;
            iTalkTB.UseSystemPasswordChar = UseSystemPasswordChar;

            G.Clear(Color.Transparent);
            G.FillPath(B1, Shape); // Draw background
            G.DrawPath(P1, Shape); // Draw border


            if (Image != null)
            {
                G.DrawImage(_Image, 5, 8, 24, 24);
                // 24x24 is the perfect size of the image
            }

            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            G.Dispose();
            B.Dispose();
        }
    }

    #endregion
    #region Small TextBox

    [DefaultEvent("TextChanged")]
    class WC_3_TextBox_Small : Control
    {
        #region Variables

        public TextBox iTalkTB = new TextBox();
        private GraphicsPath Shape;
        private int _maxchars = 32767;
        private bool _ReadOnly;
        private bool _Multiline;
        private HorizontalAlignment ALNType;
        private bool isPasswordMasked = false;
        private Pen P1;
        private SolidBrush B1;

        #endregion
        #region Properties

        public HorizontalAlignment TextAlignment
        {
            get { return ALNType; }
            set
            {
                ALNType = value;
                Invalidate();
            }
        }
        public int MaxLength
        {
            get { return _maxchars; }
            set
            {
                _maxchars = value;
                iTalkTB.MaxLength = MaxLength;
                Invalidate();
            }
        }

        public bool UseSystemPasswordChar
        {
            get { return isPasswordMasked; }
            set
            {
                iTalkTB.UseSystemPasswordChar = UseSystemPasswordChar;
                isPasswordMasked = value;
                Invalidate();
            }
        }
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                _ReadOnly = value;
                if (iTalkTB != null)
                {
                    iTalkTB.ReadOnly = value;
                }
            }
        }
        public bool Multiline
        {
            get { return _Multiline; }
            set
            {
                _Multiline = value;
                if (iTalkTB != null)
                {
                    iTalkTB.Multiline = value;

                    if (value)
                    {
                        iTalkTB.Height = Height - 10;
                    }
                    else
                    {
                        Height = iTalkTB.Height + 10;
                    }
                }
            }
        }

        #endregion
        #region EventArgs

        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            iTalkTB.Text = Text;
            Invalidate();
        }

        private void OnBaseTextChanged(object s, EventArgs e)
        {
            Text = iTalkTB.Text;
        }

        protected override void OnForeColorChanged(System.EventArgs e)
        {
            base.OnForeColorChanged(e);
            iTalkTB.ForeColor = ForeColor;
            Invalidate();
        }

        protected override void OnFontChanged(System.EventArgs e)
        {
            base.OnFontChanged(e);
            iTalkTB.Font = Font;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        private void _OnKeyDown(object Obj, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                iTalkTB.SelectAll();
                e.SuppressKeyPress = true;
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                iTalkTB.Copy();
                e.SuppressKeyPress = true;
            }
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            if (_Multiline)
            {
                iTalkTB.Height = Height - 10;
            }
            else
            {
                Height = iTalkTB.Height + 10;
            }

            Shape = new GraphicsPath();
            var _with1 = Shape;
            _with1.AddArc(0, 0, 10, 10, 180, 90);
            _with1.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _with1.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _with1.AddArc(0, Height - 11, 10, 10, 90, 90);
            _with1.CloseAllFigures();
        }

        protected override void OnGotFocus(System.EventArgs e)
        {
            base.OnGotFocus(e);
            iTalkTB.Focus();
        }

        #endregion
        public void AddTextBox()
        {
            var _TB = iTalkTB;
            _TB.Size = new Size(Width - 10, 33);
            _TB.Location = new Point(7, 5);
            _TB.Text = string.Empty;
            _TB.BorderStyle = BorderStyle.None;
            _TB.TextAlign = HorizontalAlignment.Left;
            _TB.Font = new Font("Tahoma", 11);
            _TB.UseSystemPasswordChar = UseSystemPasswordChar;
            _TB.Multiline = false;
            iTalkTB.KeyDown += _OnKeyDown;
            iTalkTB.TextChanged += OnBaseTextChanged;
        }

        public WC_3_TextBox_Small()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            AddTextBox();
            Controls.Add(iTalkTB);

            P1 = new Pen(Color.FromArgb(180, 180, 180)); // P1 = Border color
            B1 = new SolidBrush(Color.White); // B1 = Rect Background color
            BackColor = Color.Transparent;
            ForeColor = Color.DimGray;

            Text = null;
            Font = new Font("Tahoma", 11);
            Size = new Size(135, 33);
            DoubleBuffered = true;
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);

            G.SmoothingMode = SmoothingMode.AntiAlias;

            var _TB = iTalkTB;
            _TB.Width = Width - 10;
            _TB.TextAlign = TextAlignment;
            _TB.UseSystemPasswordChar = UseSystemPasswordChar;

            G.Clear(Color.Transparent);
            G.FillPath(B1, Shape); // Draw background
            G.DrawPath(P1, Shape); // Draw border

            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            G.Dispose();
            B.Dispose();
        }

    }

    #endregion
    #region RichTextBox

    [DefaultEvent("TextChanged")]
    class WC_3_RichTextBox : Control
    {

        #region Variables

        public RichTextBox iTalkRTB = new RichTextBox();
        private bool _ReadOnly;
        private bool _WordWrap;
        private bool _AutoWordSelection;
        private GraphicsPath Shape;

        #endregion
        #region Properties

        public override string Text
        {
            get { return iTalkRTB.Text; }
            set
            {
                iTalkRTB.Text = value;
                Invalidate();
            }
        }
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                _ReadOnly = value;
                if (iTalkRTB != null)
                {
                    iTalkRTB.ReadOnly = value;
                }
            }
        }
        public bool WordWrap
        {
            get { return _WordWrap; }
            set
            {
                _WordWrap = value;
                if (iTalkRTB != null)
                {
                    iTalkRTB.WordWrap = value;
                }
            }
        }
        public bool AutoWordSelection
        {
            get { return _AutoWordSelection; }
            set
            {
                _AutoWordSelection = value;
                if (iTalkRTB != null)
                {
                    iTalkRTB.AutoWordSelection = value;
                }
            }
        }
        #endregion
        #region EventArgs

        protected override void OnForeColorChanged(System.EventArgs e)
        {
            base.OnForeColorChanged(e);
            iTalkRTB.ForeColor = ForeColor;
            Invalidate();
        }

        protected override void OnFontChanged(System.EventArgs e)
        {
            base.OnFontChanged(e);
            iTalkRTB.Font = Font;
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        protected override void OnSizeChanged(System.EventArgs e)
        {
            base.OnSizeChanged(e);
            iTalkRTB.Size = new Size(Width - 13, Height - 11);
        }


        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);

            Shape = new GraphicsPath();
            var _Shape = Shape;
            _Shape.AddArc(0, 0, 10, 10, 180, 90);
            _Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            _Shape.CloseAllFigures();
        }

        public void _TextChanged(object s, EventArgs e)
        {
            iTalkRTB.Text = Text;
        }

        #endregion

        public void AddRichTextBox()
        {
            var _RTB = iTalkRTB;
            _RTB.BackColor = Color.White;
            _RTB.Size = new Size(Width - 10, 100);
            _RTB.Location = new Point(7, 5);
            _RTB.Text = string.Empty;
            _RTB.BorderStyle = BorderStyle.None;
            _RTB.Font = new Font("Tahoma", 10);
            _RTB.Multiline = true;
        }

        public WC_3_RichTextBox()
            : base()
        {

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            AddRichTextBox();
            Controls.Add(iTalkRTB);
            BackColor = Color.Transparent;
            ForeColor = Color.DimGray;

            Text = null;
            Font = new Font("Tahoma", 10);
            Size = new Size(150, 100);
            WordWrap = true;
            AutoWordSelection = false;
            DoubleBuffered = true;

            TextChanged += _TextChanged;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(this.Width, this.Height);
            Graphics G = Graphics.FromImage(B);
            G.SmoothingMode = SmoothingMode.AntiAlias;
            G.Clear(Color.Transparent);
            G.FillPath(Brushes.White, this.Shape);
            G.DrawPath(new Pen(Color.FromArgb(180, 180, 180)), this.Shape);
            G.Dispose();
            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            B.Dispose();
        }
    }

    #endregion
    #region NumericUpDown

    public class WC_3_NumericUpDown : Control
    {

        #region  Enums

        public enum _TextAlignment
        {
            Near,
            Center
        }

        #endregion
        #region  Variables

        private GraphicsPath Shape;
        private Pen P1;
        private SolidBrush B1;

        private long _Value;
        private long _Minimum;
        private long _Maximum;
        private int Xval;
        private int Yval;
        private bool KeyboardNum;
        private _TextAlignment MyStringAlignment;

        #endregion
        #region  Properties

        public long Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (value <= _Maximum & value >= _Minimum)
                {
                    _Value = value;
                }
                Invalidate();
            }
        }

        public long Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {
                if (value < _Maximum)
                {
                    _Minimum = value;
                }
                if (_Value < _Minimum)
                {
                    _Value = Minimum;
                }
                Invalidate();
            }
        }

        public long Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                if (value > _Minimum)
                {
                    _Maximum = value;
                }
                if (_Value > _Maximum)
                {
                    _Value = _Maximum;
                }
                Invalidate();
            }
        }

        public _TextAlignment TextAlignment
        {
            get
            {
                return MyStringAlignment;
            }
            set
            {
                MyStringAlignment = value;
                Invalidate();
            }
        }

        #endregion
        #region  EventArgs

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            Height = 28;
            Shape = new GraphicsPath();
            Shape.AddArc(0, 0, 10, 10, 180, 90);
            Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            Shape.AddArc(0, Height - 11, 10, 10, 90, 90);
            Shape.CloseAllFigures();
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Xval = e.Location.X;
            Yval = e.Location.Y;
            Invalidate();

            if (e.X < Width - 24)
            {
                Cursor = Cursors.IBeam;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (Xval > this.Width - 23 && Xval < this.Width - 3)
            {
                if (Yval < 15)
                {
                    if ((Value + 1) <= _Maximum)
                    {
                        _Value++;
                    }
                }
                else
                {
                    if ((Value - 1) >= _Minimum)
                    {
                        _Value--;
                    }
                }
            }
            else
            {
                KeyboardNum = !KeyboardNum;
                Focus();
            }
            Invalidate();
        }

        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            try
            {
                if (KeyboardNum == true)
                {
                    _Value = long.Parse((_Value).ToString() + e.KeyChar.ToString().ToString());
                }
                if (_Value > _Maximum)
                {
                    _Value = _Maximum;
                }
            }
            catch (Exception)
            {
            }
        }

        protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Back)
            {
                string TemporaryValue = _Value.ToString();
                TemporaryValue = TemporaryValue.Remove(Convert.ToInt32(TemporaryValue.Length - 1));
                if (TemporaryValue.Length == 0)
                {
                    TemporaryValue = "0";
                }
                _Value = Convert.ToInt32(TemporaryValue);
            }
            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
            {
                if ((Value + 1) <= _Maximum)
                {
                    _Value++;
                }
                Invalidate();
            }
            else
            {
                if ((Value - 1) >= _Minimum)
                {
                    _Value--;
                }
                Invalidate();
            }
        }

        #endregion

        public WC_3_NumericUpDown()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            P1 = new Pen(Color.FromArgb(180, 180, 180)); // P1 = Border color
            B1 = new SolidBrush(Color.White); // B1 = Rect Background color
            BackColor = Color.Transparent;
            ForeColor = Color.DimGray;

            _Minimum = 0;
            _Maximum = 100;

            Font = new Font("Tahoma", 11);
            Size = new Size(70, 28);
            MinimumSize = new Size(62, 28);
            DoubleBuffered = true;
        }

        public void Increment(int Value)
        {
            this._Value += Value;
            Invalidate();
        }

        public void Decrement(int Value)
        {
            this._Value -= Value;
            Invalidate();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);

            G.SmoothingMode = SmoothingMode.AntiAlias;

            G.Clear(Color.Transparent); // Set control background color
            G.FillPath(B1, Shape); // Draw background
            G.DrawPath(P1, Shape); // Draw border

            LinearGradientBrush ColorGradient = new LinearGradientBrush(new Rectangle(Width - 23, 4, 19, 19), Color.FromArgb(241, 241, 241), Color.FromArgb(241, 241, 241), 90.0F);
            G.FillRectangle(ColorGradient, ColorGradient.Rectangle); // Fills the body of the rectangle

            G.DrawRectangle(new Pen(Color.FromArgb(252, 252, 252)), new Rectangle(Width - 22, 5, 17, 17));
            G.DrawRectangle(new Pen(Color.FromArgb(180, 180, 180)), new Rectangle(Width - 23, 4, 19, 19));

            G.DrawLine(new Pen(Color.FromArgb(250, 252, 250)), new Point(Width - 22, Height - 16), new Point(Width - 5, Height - 16));
            G.DrawLine(new Pen(Color.FromArgb(180, 180, 180)), new Point(Width - 22, Height - 15), new Point(Width - 5, Height - 15));
            G.DrawLine(new Pen(Color.FromArgb(250, 250, 250)), new Point(Width - 22, Height - 14), new Point(Width - 5, Height - 14));

            G.DrawString("+", new Font("Tahoma", 8), Brushes.Gray, Width - 19, Height - 26);
            G.DrawString("-", new Font("Tahoma", 12), Brushes.Gray, Width - 19, Height - 20);

            switch (MyStringAlignment)
            {
                case _TextAlignment.Near:
                    G.DrawString(System.Convert.ToString(Value), Font, new SolidBrush(ForeColor), new Rectangle(5, 0, Width - 1, Height - 1), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    break;
                case _TextAlignment.Center:
                    G.DrawString(System.Convert.ToString(Value), Font, new SolidBrush(ForeColor), new Rectangle(0, 0, Width - 1, Height - 1), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    break;
            }

            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            G.Dispose();
            B.Dispose();
        }
    }

    #endregion
    #region Left Chat Bubble

    public class WC_3_ChatBubble_L : Control
    {

        #region Variables

        private GraphicsPath Shape;
        private Color _TextColor = Color.FromArgb(52, 52, 52);
        private Color _BubbleColor = Color.FromArgb(217, 217, 217);
        private bool _DrawBubbleArrow = true;

        #endregion
        #region Properties

        public override Color ForeColor
        {
            get { return this._TextColor; }
            set
            {
                this._TextColor = value;
                this.Invalidate();
            }
        }

        public Color BubbleColor
        {
            get { return this._BubbleColor; }
            set
            {
                this._BubbleColor = value;
                this.Invalidate();
            }
        }

        public bool DrawBubbleArrow
        {
            get { return _DrawBubbleArrow; }
            set
            {
                _DrawBubbleArrow = value;
                Invalidate();
            }
        }

        #endregion

        public WC_3_ChatBubble_L()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            Size = new Size(152, 38);
            BackColor = Color.Transparent;
            ForeColor = Color.FromArgb(52, 52, 52);
            Font = new Font("Segoe UI", 10);
        }

        protected override void OnResize(System.EventArgs e)
        {
            Shape = new GraphicsPath();

            var _Shape = Shape;
            _Shape.AddArc(9, 0, 10, 10, 180, 90);
            _Shape.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _Shape.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _Shape.AddArc(9, Height - 11, 10, 10, 90, 90);
            _Shape.CloseAllFigures();

            Invalidate();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(this.Width, this.Height);
            Graphics G = Graphics.FromImage(B);
            var _G = G;
            _G.SmoothingMode = SmoothingMode.HighQuality;
            _G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _G.Clear(BackColor);

            // Fill the body of the bubble with the specified color
            _G.FillPath(new SolidBrush(_BubbleColor), Shape);
            // Draw the string specified in 'Text' property
            _G.DrawString(Text, Font, new SolidBrush(ForeColor), new Rectangle(15, 4, Width - 17, Height - 5));

            // Draw a polygon on the right side of the bubble
            if (_DrawBubbleArrow == true)
            {
                Point[] p = {
                            new Point(9, Height - 19),
                            new Point(0, Height - 25),
                            new Point(9, Height - 30)
                        };
                _G.FillPolygon(new SolidBrush(_BubbleColor), p);
                _G.DrawPolygon(new Pen(new SolidBrush(_BubbleColor)), p);
            }
            G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(B, 0, 0);
            B.Dispose();
        }
    }

    #endregion
    #region Right Chat Bubble

    public class WC_3_ChatBubble_R : Control
    {

        #region Variables

        private GraphicsPath Shape;
        private Color _TextColor = Color.FromArgb(52, 52, 52);
        private Color _BubbleColor = Color.FromArgb(192, 206, 215);
        private bool _DrawBubbleArrow = true;

        #endregion
        #region Properties

        public override Color ForeColor
        {
            get { return this._TextColor; }
            set
            {
                this._TextColor = value;
                this.Invalidate();
            }
        }

        public Color BubbleColor
        {
            get { return this._BubbleColor; }
            set
            {
                this._BubbleColor = value;
                this.Invalidate();
            }
        }

        public bool DrawBubbleArrow
        {
            get { return _DrawBubbleArrow; }
            set
            {
                _DrawBubbleArrow = value;
                Invalidate();
            }
        }

        #endregion

        public WC_3_ChatBubble_R()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            Size = new Size(152, 38);
            BackColor = Color.Transparent;
            ForeColor = Color.FromArgb(52, 52, 52);
            Font = new Font("Segoe UI", 10);
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            Shape = new GraphicsPath();

            var _with1 = Shape;
            _with1.AddArc(0, 0, 10, 10, 180, 90);
            _with1.AddArc(Width - 18, 0, 10, 10, -90, 90);
            _with1.AddArc(Width - 18, Height - 11, 10, 10, 0, 90);
            _with1.AddArc(0, Height - 11, 10, 10, 90, 90);
            _with1.CloseAllFigures();

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(this.Width, this.Height);
            Graphics G = Graphics.FromImage(B);

            var _G = G;
            _G.SmoothingMode = SmoothingMode.HighQuality;
            _G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _G.Clear(BackColor);

            // Fill the body of the bubble with the specified color
            _G.FillPath(new SolidBrush(_BubbleColor), Shape);
            // Draw the string specified in 'Text' property
            _G.DrawString(Text, Font, new SolidBrush(ForeColor), (new Rectangle(6, 4, Width - 15, Height)));

            // Draw a polygon on the right side of the bubble
            if (_DrawBubbleArrow == true)
            {
                Point[] p = {
            new Point(Width - 8, Height - 19),
            new Point(Width, Height - 25),
            new Point(Width - 8, Height - 30)
        };
                _G.FillPolygon(new SolidBrush(_BubbleColor), p);
                _G.DrawPolygon(new Pen(new SolidBrush(_BubbleColor)), p);
            }

            G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(B, 0, 0);
            B.Dispose();
        }
    }

    #endregion
    #region Separator

    public class WC_3_Separator : Control
    {

        public WC_3_Separator()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            this.Size = new Size(120, 10);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawLine(new Pen(Color.FromArgb(184, 183, 188)), 0, 5, Width, 5);
        }
    }

    #endregion
    #region Panel

    class WC_3_Panel : ContainerControl
    {


        private GraphicsPath Shape;
        public WC_3_Panel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            BackColor = Color.Transparent;
            this.Size = new Size(187, 117);
            Padding = new Padding(5, 5, 5, 5);
            DoubleBuffered = true;
        }

        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);

            Shape = new GraphicsPath();
            var _with1 = Shape;
            _with1.AddArc(0, 0, 10, 10, 180, 90);
            _with1.AddArc(Width - 11, 0, 10, 10, -90, 90);
            _with1.AddArc(Width - 11, Height - 11, 10, 10, 0, 90);
            _with1.AddArc(0, Height - 11, 10, 10, 90, 90);
            _with1.CloseAllFigures();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);

            G.SmoothingMode = SmoothingMode.HighQuality;

            G.Clear(Color.Transparent);
            G.FillPath(Brushes.White, Shape); // Draw RTB background
            G.DrawPath(new Pen(Color.FromArgb(180, 180, 180)), Shape); // Draw border

            G.Dispose();
            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            B.Dispose();
        }
    }

    #endregion
    #region GroupBox

    public class WC_3_GroupBox : ContainerControl
    {

        public WC_3_GroupBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            DoubleBuffered = true;
            this.Size = new Size(212, 104);
            this.MinimumSize = new Size(136, 50);
            this.Padding = new Padding(5, 28, 5, 5);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);
            Rectangle TitleBox = new Rectangle(51, 3, Width - 103, 18);
            Rectangle box = new Rectangle(0, 0, Width - 1, Height - 10);

            G.Clear(Color.Transparent);
            G.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the body of the GroupBox
            G.FillPath(Brushes.White, RoundRectangle.RoundRect(new Rectangle(1, 12, Width - 3, box.Height - 1), 8));
            // Draw the border of the GroupBox
            G.DrawPath(new Pen(Color.FromArgb(159, 159, 161)), RoundRectangle.RoundRect(new Rectangle(1, 12, Width - 3, Height - 13), 8));

            // Draw the background of the title box
            G.FillPath(Brushes.White, RoundRectangle.RoundRect(TitleBox, 1));
            // Draw the border of the title box
            G.DrawPath(new Pen(Color.FromArgb(182, 180, 186)), RoundRectangle.RoundRect(TitleBox, 4));
            // Draw the specified string from 'Text' property inside the title box
            G.DrawString(Text, new Font("Tahoma", 9, FontStyle.Regular), new SolidBrush(Color.FromArgb(53, 53, 53)), TitleBox, new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            });

            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            G.Dispose();
            B.Dispose();
        }
    }

    #endregion
    #region CheckBox

    [DefaultEvent("CheckedChanged")]
    class WC_3_CheckBox : Control
    {

        #region Variables

        private GraphicsPath Shape;
        private LinearGradientBrush GB;
        private Rectangle R1;
        private Rectangle R2;
        private bool _Checked;
        public event CheckedChangedEventHandler CheckedChanged;
        public delegate void CheckedChangedEventHandler(object sender);

        #endregion
        #region Properties

        public bool Checked
        {
            get { return _Checked; }
            set
            {
                _Checked = value;
                if (CheckedChanged != null)
                {
                    CheckedChanged(this);
                }
                Invalidate();
            }
        }

        #endregion

        public WC_3_CheckBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);

            BackColor = Color.Transparent;
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 10);
            Size = new Size(120, 26);
        }

        protected override void OnClick(EventArgs e)
        {
            _Checked = !_Checked;
            if (CheckedChanged != null)
            {
                CheckedChanged(this);
            }
            Invalidate();
            base.OnClick(e);
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }

        protected override void OnResize(System.EventArgs e)
        {
            if (Width > 0 && Height > 0)
            {
                Shape = new GraphicsPath();

                R1 = new Rectangle(17, 0, Width, Height + 1);
                R2 = new Rectangle(0, 0, Width, Height);
                GB = new LinearGradientBrush(new Rectangle(0, 0, 25, 25), Color.FromArgb(250, 250, 250), Color.FromArgb(240, 240, 240), 90);

                var _Shape = Shape;
                _Shape.AddArc(0, 0, 7, 7, 180, 90);
                _Shape.AddArc(7, 0, 7, 7, -90, 90);
                _Shape.AddArc(7, 7, 7, 7, 0, 90);
                _Shape.AddArc(0, 7, 7, 7, 90, 90);
                _Shape.CloseAllFigures();
                Height = 15;
            }

            Invalidate();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var _G = e.Graphics;
            _G.Clear(Color.FromArgb(246, 246, 246));
            _G.SmoothingMode = SmoothingMode.AntiAlias;
            // Fill the body of the CheckBox
            _G.FillPath(GB, Shape);
            // Draw the border
            _G.DrawPath(new Pen(Color.FromArgb(160, 160, 160)), Shape);
            // Draw the string
            _G.DrawString(Text, Font, new SolidBrush(Color.FromArgb(142, 142, 142)), R1, new StringFormat { LineAlignment = StringAlignment.Center });

            if (Checked)
            {
                _G.DrawString("ü", new Font("Wingdings", 14), new SolidBrush(Color.FromArgb(142, 142, 142)), new Rectangle(-2, 1, Width, Height), new StringFormat { LineAlignment = StringAlignment.Center });
            }
            e.Dispose();
        }
    }

    #endregion
    #region RadioButton

    [DefaultEvent("CheckedChanged")]
    class WC_3_RadioButton : Control
    {

        #region Enums

        public enum MouseState : byte
        {
            None = 0,
            Over = 1,
            Down = 2,
            Block = 3
        }

        #endregion
        #region Variables

        private bool _Checked;
        public event CheckedChangedEventHandler CheckedChanged;
        public delegate void CheckedChangedEventHandler(object sender);

        #endregion
        #region Properties

        public bool Checked
        {
            get { return _Checked; }
            set
            {
                _Checked = value;
                InvalidateControls();
                if (CheckedChanged != null)
                {
                    CheckedChanged(this);
                }
                Invalidate();
            }
        }

        #endregion
        #region EventArgs

        protected override void OnTextChanged(System.EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 15;
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (!_Checked)
                Checked = true;
            base.OnMouseDown(e);
        }

        #endregion

        public WC_3_RadioButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
            Font = new Font("Segoe UI", 10);
            Width = 132;
        }

        private void InvalidateControls()
        {
            if (!IsHandleCreated || !_Checked)
                return;

            foreach (Control _Control in Parent.Controls)
            {
                if (!object.ReferenceEquals(_Control, this) && _Control is WC_3_RadioButton)
                {
                    ((WC_3_RadioButton)_Control).Checked = false;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var _G = e.Graphics;

            _G.Clear(Color.FromArgb(246, 246, 246));
            _G.SmoothingMode = SmoothingMode.AntiAlias;

            LinearGradientBrush LGB = new LinearGradientBrush(new Rectangle(new Point(0, 0), new Size(14, 14)), Color.FromArgb(250, 250, 250), Color.FromArgb(240, 240, 240), 90);
            _G.FillEllipse(LGB, new Rectangle(new Point(0, 0), new Size(14, 14)));

            GraphicsPath GP = new GraphicsPath();
            GP.AddEllipse(new Rectangle(0, 0, 14, 14));
            _G.SetClip(GP);
            _G.ResetClip();

            // Draw ellipse border
            _G.DrawEllipse(new Pen(Color.FromArgb(160, 160, 160)), new Rectangle(new Point(0, 0), new Size(14, 14)));

            // Draw an ellipse inside the body
            if (_Checked)
            {
                SolidBrush EllipseColor = new SolidBrush(Color.FromArgb(142, 142, 142));
                _G.FillEllipse(EllipseColor, new Rectangle(new Point(4, 4), new Size(6, 6)));
            }
            // Draw the string specified in 'Text' property
            _G.DrawString(Text, Font, new SolidBrush(Color.FromArgb(142, 142, 142)), 16, 8, new StringFormat { LineAlignment = StringAlignment.Center });

            e.Dispose();
        }
    }

    #endregion
    #region Notification Number

    class WC_3_NotificationNumber : Control
    {
        #region Variables

        private int _Value = 0;
        private int _Maximum = 99;

        #endregion
        #region Properties

        public int Value
        {
            get
            {
                if (this._Value == 0)
                {
                    return 0;
                }
                return this._Value;
            }
            set
            {
                if (value > this._Maximum)
                {
                    value = this._Maximum;
                }
                this._Value = value;
                this.Invalidate();
            }
        }

        public int Maximum
        {
            get
            {
                return this._Maximum;
            }
            set
            {
                if (value < this._Value)
                {
                    this._Value = value;
                }
                this._Maximum = value;
                this.Invalidate();
            }
        }



        #endregion

        public WC_3_NotificationNumber()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            Text = null;
            DoubleBuffered = true;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 20;
            Width = 20;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var _G = e.Graphics;
            string myString = _Value.ToString();
            _G.Clear(BackColor);
            _G.SmoothingMode = SmoothingMode.AntiAlias;
            LinearGradientBrush LGB = new LinearGradientBrush(new Rectangle(new Point(0, 0), new Size(18, 20)), Color.FromArgb(197, 69, 68), Color.FromArgb(176, 52, 52), 90f);

            // Fills the body with LGB gradient
            _G.FillEllipse(LGB, new Rectangle(new Point(0, 0), new Size(18, 18)));
            // Draw border
            _G.DrawEllipse(new Pen(Color.FromArgb(205, 70, 66)), new Rectangle(new Point(0, 0), new Size(18, 18)));
            _G.DrawString(myString, new Font("Segoe UI", 8, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 253)), new Rectangle(0, 0, Width - 2, Height), new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            });
            e.Dispose();
        }

    }

    #endregion
    #region ListView

    class WC_3_Listview : ListView
    {

        [DllImport("uxtheme", CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string textSubAppName, string textSubIdList);

        public WC_3_Listview()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
            HeaderStyle = ColumnHeaderStyle.Nonclickable;
            BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            WC_3_Listview.SetWindowTheme(this.Handle, "explorer", null);
            base.OnHandleCreated(e);
        }
    }

    #endregion
    #region ComboBox

    class WC_3_ComboBox : ComboBox
    {

        #region Variables

        private int _StartIndex = 0;
        private Color _HoverSelectionColor = Color.FromArgb(241, 241, 241);

        #endregion
        #region Custom Properties

        public int StartIndex
        {
            get { return _StartIndex; }
            set
            {
                _StartIndex = value;
                try
                {
                    base.SelectedIndex = value;
                }
                catch
                {
                }
                Invalidate();
            }
        }

        public Color HoverSelectionColor
        {
            get { return _HoverSelectionColor; }
            set
            {
                _HoverSelectionColor = value;
                Invalidate();
            }
        }

        #endregion
        #region EventArgs

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(_HoverSelectionColor), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            }

            if (!(e.Index == -1))
            {
                e.Graphics.DrawString(GetItemText(Items[e.Index]), e.Font, Brushes.DimGray, e.Bounds);
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            SuspendLayout();
            Update();
            ResumeLayout();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        #endregion

        public WC_3_ComboBox()
        {
            SetStyle((ControlStyles)139286, true);
            SetStyle(ControlStyles.Selectable, false);

            DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;

            BackColor = Color.FromArgb(246, 246, 246);
            ForeColor = Color.FromArgb(142, 142, 142);
            Size = new Size(135, 26);
            ItemHeight = 20;
            DropDownHeight = 100;
            Font = new Font("Segoe UI", 10, FontStyle.Regular);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            LinearGradientBrush LGB = default(LinearGradientBrush);
            GraphicsPath GP = default(GraphicsPath);

            e.Graphics.Clear(BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Create a curvy border
            GP = RoundRectangle.RoundRect(0, 0, Width - 1, Height - 1, 5);
            // Fills the body of the rectangle with a gradient
            LGB = new LinearGradientBrush(ClientRectangle, Color.FromArgb(241, 241, 241), Color.FromArgb(241, 241, 241), 90f);

            e.Graphics.SetClip(GP);
            e.Graphics.FillRectangle(LGB, ClientRectangle);
            e.Graphics.ResetClip();

            // Draw rectangle border
            e.Graphics.DrawPath(new Pen(Color.FromArgb(204, 204, 204)), GP);
            // Draw string
            e.Graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(142, 142, 142)), new Rectangle(3, 0, Width - 20, Height), new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near
            });

            // Draw the dropdown arrow
            e.Graphics.DrawLine(new Pen(Color.FromArgb(160, 160, 160), 2), new Point(Width - 18, 10), new Point(Width - 14, 14));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(160, 160, 160), 2), new Point(Width - 14, 14), new Point(Width - 10, 10));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(160, 160, 160)), new Point(Width - 14, 15), new Point(Width - 14, 14));

            GP.Dispose();
            LGB.Dispose();
        }
    }

    #endregion
    #region Circular ProgressBar

    public class WC_3_ProgressBar : Control
    {

        #region Enums

        public enum _ProgressShape
        {
            Round,
            Flat
        }

        #endregion
        #region Variables

        private long _Value;
        private long _Maximum = 100;
        private Color _ProgressColor1 = Color.FromArgb(92, 92, 92);
        private Color _ProgressColor2 = Color.FromArgb(92, 92, 92);
        private _ProgressShape ProgressShapeVal;

        #endregion
        #region Custom Properties

        public long Value
        {
            get { return _Value; }
            set
            {
                if (value > _Maximum)
                    value = _Maximum;
                _Value = value;
                Invalidate();
            }
        }

        public long Maximum
        {
            get { return _Maximum; }
            set
            {
                if (value < 1)
                    value = 1;
                _Maximum = value;
                Invalidate();
            }
        }

        public Color ProgressColor1
        {
            get { return _ProgressColor1; }
            set
            {
                _ProgressColor1 = value;
                Invalidate();
            }
        }

        public Color ProgressColor2
        {
            get { return _ProgressColor2; }
            set
            {
                _ProgressColor2 = value;
                Invalidate();
            }
        }

        public _ProgressShape ProgressShape
        {
            get { return ProgressShapeVal; }
            set
            {
                ProgressShapeVal = value;
                Invalidate();
            }
        }

        #endregion
        #region EventArgs

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetStandardSize();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetStandardSize();
        }

        protected override void OnPaintBackground(PaintEventArgs p)
        {
            base.OnPaintBackground(p);
        }

        #endregion

        public WC_3_ProgressBar()
        {
            Size = new Size(130, 130);
            Font = new Font("Segoe UI", 15);
            MinimumSize = new Size(100, 100);
            DoubleBuffered = true;
        }

        private void SetStandardSize()
        {
            int _Size = Math.Max(Width, Height);
            Size = new Size(_Size, _Size);
        }

        public void Increment(int Val)
        {
            this._Value += Val;
            Invalidate();
        }

        public void Decrement(int Val)
        {
            this._Value -= Val;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Bitmap bitmap = new Bitmap(this.Width, this.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.Clear(this.BackColor);
                    using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, this._ProgressColor1, this._ProgressColor2, LinearGradientMode.ForwardDiagonal))
                    {
                        using (Pen pen = new Pen(brush, 14f))
                        {
                            switch (this.ProgressShapeVal)
                            {
                                case _ProgressShape.Round:
                                    pen.StartCap = LineCap.Round;
                                    pen.EndCap = LineCap.Round;
                                    break;

                                case _ProgressShape.Flat:
                                    pen.StartCap = LineCap.Flat;
                                    pen.EndCap = LineCap.Flat;
                                    break;
                            }
                            graphics.DrawArc(pen, 0x12, 0x12, (this.Width - 0x23) - 2, (this.Height - 0x23) - 2, -90, (int)Math.Round((double)((360.0 / ((double)this._Maximum)) * this._Value)));
                        }
                    }
                    using (LinearGradientBrush brush2 = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0x34, 0x34, 0x34), Color.FromArgb(0x34, 0x34, 0x34), LinearGradientMode.Vertical))
                    {
                        graphics.FillEllipse(brush2, 0x18, 0x18, (this.Width - 0x30) - 1, (this.Height - 0x30) - 1);
                    }
                    SizeF MS = graphics.MeasureString(Convert.ToString(Convert.ToInt32((100 / _Maximum) * _Value)), Font);
                    graphics.DrawString(Convert.ToString(Convert.ToInt32((100 / _Maximum) * _Value)), Font, Brushes.White, Convert.ToInt32(Width / 2 - MS.Width / 2), Convert.ToInt32(Height / 2 - MS.Height / 2));
                    e.Graphics.DrawImage(bitmap, 0, 0);
                    graphics.Dispose();
                    bitmap.Dispose();
                }
            }
        }
    }

    #endregion
    #region Progress Indicator

    class WC_3_ProgressIndicator : Control
    {

        #region Variables

        private readonly SolidBrush BaseColor = new SolidBrush(Color.DarkGray);
        private readonly SolidBrush AnimationColor = new SolidBrush(Color.DimGray);

        private readonly Timer AnimationSpeed = new Timer();
        private PointF[] FloatPoint;
        private BufferedGraphics BuffGraphics;
        private int IndicatorIndex;
        private readonly BufferedGraphicsContext GraphicsContext = BufferedGraphicsManager.Current;

        #endregion
        #region Custom Properties

        public Color P_BaseColor
        {
            get { return BaseColor.Color; }
            set { BaseColor.Color = value; }
        }

        public Color P_AnimationColor
        {
            get { return AnimationColor.Color; }
            set { AnimationColor.Color = value; }
        }

        public int P_AnimationSpeed
        {
            get { return AnimationSpeed.Interval; }
            set { AnimationSpeed.Interval = value; }
        }

        #endregion
        #region EventArgs

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetStandardSize();
            UpdateGraphics();
            SetPoints();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            AnimationSpeed.Enabled = this.Enabled;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AnimationSpeed.Tick += AnimationSpeed_Tick;
            AnimationSpeed.Start();
        }

        private void AnimationSpeed_Tick(object sender, EventArgs e)
        {
            if (IndicatorIndex.Equals(0))
            {
                IndicatorIndex = FloatPoint.Length - 1;
            }
            else
            {
                IndicatorIndex -= 1;
            }
            this.Invalidate(false);
        }

        #endregion

        public WC_3_ProgressIndicator()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            Size = new Size(80, 80);
            Text = string.Empty;
            MinimumSize = new Size(80, 80);
            SetPoints();
            AnimationSpeed.Interval = 100;
        }

        private void SetStandardSize()
        {
            int _Size = Math.Max(Width, Height);
            Size = new Size(_Size, _Size);
        }

        private void SetPoints()
        {
            Stack<PointF> stack = new Stack<PointF>();
            PointF startingFloatPoint = new PointF(((float)this.Width) / 2f, ((float)this.Height) / 2f);
            for (float i = 0f; i < 360f; i += 45f)
            {
                this.SetValue(startingFloatPoint, (int)Math.Round((double)((((double)this.Width) / 2.0) - 15.0)), (double)i);
                PointF endPoint = this.EndPoint;
                endPoint = new PointF(endPoint.X - 7.5f, endPoint.Y - 7.5f);
                stack.Push(endPoint);
            }
            this.FloatPoint = stack.ToArray();
        }

        private void UpdateGraphics()
        {
            if ((this.Width > 0) && (this.Height > 0))
            {
                Size size2 = new Size(this.Width + 1, this.Height + 1);
                this.GraphicsContext.MaximumBuffer = size2;
                this.BuffGraphics = this.GraphicsContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
                this.BuffGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.BuffGraphics.Graphics.Clear(this.BackColor);
            int num2 = this.FloatPoint.Length - 1;
            for (int i = 0; i <= num2; i++)
            {
                if (this.IndicatorIndex == i)
                {
                    this.BuffGraphics.Graphics.FillEllipse(this.AnimationColor, this.FloatPoint[i].X, this.FloatPoint[i].Y, 15f, 15f);
                }
                else
                {
                    this.BuffGraphics.Graphics.FillEllipse(this.BaseColor, this.FloatPoint[i].X, this.FloatPoint[i].Y, 15f, 15f);
                }
            }
            this.BuffGraphics.Render(e.Graphics);
        }


        private double Rise;
        private double Run;
        private PointF _StartingFloatPoint;

        private X AssignValues<X>(ref X Run, X Length)
        {
            Run = Length;
            return Length;
        }

        private void SetValue(PointF StartingFloatPoint, int Length, double Angle)
        {
            double CircleRadian = Math.PI * Angle / 180.0;

            _StartingFloatPoint = StartingFloatPoint;
            Rise = AssignValues(ref Run, Length);
            Rise = Math.Sin(CircleRadian) * Rise;
            Run = Math.Cos(CircleRadian) * Run;
        }

        private PointF EndPoint
        {
            get
            {
                float LocationX = Convert.ToSingle(_StartingFloatPoint.Y + Rise);
                float LocationY = Convert.ToSingle(_StartingFloatPoint.X + Run);

                return new PointF(LocationY, LocationX);
            }
        }
    }

    #endregion
    #region TabControl

    class WC_3_TabControl : TabControl
    {

        // NOTE: For best quality icons/images on the TabControl; from the associated ImageList, set
        // the image size (24,24) so it can fit in the tab rectangle. However, to ensure a
        // high-quality image drawing, make sure you only add (32,32) images and not (24,24) as
        // determined in the ImageList

        // INFO: A free, non-commercial icon list that would fit in perfectly with the TabControl is
        // Wireframe Toolbar Icons by Gentleface. Licensed under Creative Commons Attribution.
        // Check it out from here: http://www.gentleface.com/free_icon_set.html

        public WC_3_TabControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true);

            DoubleBuffered = true;
            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(44, 135);
            DrawMode = TabDrawMode.OwnerDrawFixed;

            foreach (TabPage Page in this.TabPages)
            {
                Page.BackColor = Color.FromArgb(246, 246, 246);
            }
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();

            base.DoubleBuffered = true;
            SizeMode = TabSizeMode.Fixed;
            Appearance = TabAppearance.Normal;
            Alignment = TabAlignment.Left;
        }


        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control is TabPage)
            {
                IEnumerator enumerator;
                try
                {
                    enumerator = this.Controls.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        TabPage current = (TabPage)enumerator.Current;
                        current = new TabPage();
                    }
                }
                finally
                {
                    e.Control.BackColor = Color.FromArgb(246, 246, 246);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Bitmap B = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(B);

            var _Graphics = G;

            _Graphics.Clear(Color.FromArgb(246, 246, 246));
            _Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            _Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            _Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            // Draw tab selector background
            _Graphics.FillRectangle(new SolidBrush(Color.FromArgb(54, 57, 64)), new Rectangle(-5, 0, ItemSize.Height + 4, Height));
            // Draw vertical line at the end of the tab selector rectangle
            _Graphics.DrawLine(new Pen(Color.FromArgb(25, 26, 28)), ItemSize.Height - 1, 0, ItemSize.Height - 1, Height);

            for (int TabIndex = 0; TabIndex <= TabCount - 1; TabIndex++)
            {
                if (TabIndex == SelectedIndex)
                {
                    Rectangle TabRect = new Rectangle(new Point(GetTabRect(TabIndex).Location.X - 2, GetTabRect(TabIndex).Location.Y - 2), new Size(GetTabRect(TabIndex).Width + 3, GetTabRect(TabIndex).Height - 8));

                    // Draw background of the selected tab
                    _Graphics.FillRectangle(new SolidBrush(Color.FromArgb(35, 36, 38)), TabRect.X, TabRect.Y, TabRect.Width - 4, TabRect.Height + 3);
                    // Draw a tab highlighter on the background of the selected tab
                    Rectangle TabHighlighter = new Rectangle(new Point(GetTabRect(TabIndex).X - 2, GetTabRect(TabIndex).Location.Y - (TabIndex == 0 ? 1 : 1)), new Size(4, GetTabRect(TabIndex).Height - 7));
                    _Graphics.FillRectangle(new SolidBrush(Color.FromArgb(89, 169, 222)), TabHighlighter);
                    // Draw tab text
                    _Graphics.DrawString(TabPages[TabIndex].Text, new Font(Font.FontFamily, Font.Size, FontStyle.Bold), new SolidBrush(Color.FromArgb(254, 255, 255)), new Rectangle(TabRect.Left + 40, TabRect.Top + 12, TabRect.Width - 40, TabRect.Height), new StringFormat { Alignment = StringAlignment.Near });

                    if (this.ImageList != null)
                    {
                        int Index = TabPages[TabIndex].ImageIndex;
                        if (!(Index == -1))
                        {
                            _Graphics.DrawImage(ImageList.Images[TabPages[TabIndex].ImageIndex], TabRect.X + 9, TabRect.Y + 6, 24, 24);
                        }
                    }
                }
                else
                {
                    Rectangle TabRect = new Rectangle(new Point(GetTabRect(TabIndex).Location.X - 2, GetTabRect(TabIndex).Location.Y - 2), new Size(GetTabRect(TabIndex).Width + 3, GetTabRect(TabIndex).Height - 8));
                    _Graphics.DrawString(TabPages[TabIndex].Text, new Font(Font.FontFamily, Font.Size, FontStyle.Bold), new SolidBrush(Color.FromArgb(159, 162, 167)), new Rectangle(TabRect.Left + 40, TabRect.Top + 12, TabRect.Width - 40, TabRect.Height), new StringFormat { Alignment = StringAlignment.Near });

                    if (this.ImageList != null)
                    {
                        int Index = TabPages[TabIndex].ImageIndex;
                        if (!(Index == -1))
                        {
                            _Graphics.DrawImage(ImageList.Images[TabPages[TabIndex].ImageIndex], TabRect.X + 9, TabRect.Y + 6, 24, 24);
                        }
                    }

                }
            }
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            G.Dispose();
            B.Dispose();
        }
    }

    #endregion
    #region TrackBar

    [DefaultEvent("ValueChanged")]
    class WC_3_TrackBar : Control
    {

        #region Enums

        public enum ValueDivisor
        {
            By1 = 1,
            By10 = 10,
            By100 = 100,
            By1000 = 1000
        }

        #endregion
        #region Variables

        private GraphicsPath PipeBorder;
        private GraphicsPath TrackBarHandle;
        private Rectangle TrackBarHandleRect;
        private Rectangle ValueRect;
        private LinearGradientBrush VlaueLGB;
        private LinearGradientBrush TrackBarHandleLGB;
        private bool Cap;

        private int ValueDrawer;
        private int _Minimum = 0;
        private int _Maximum = 10;
        private int _Value = 0;
        private Color _ValueColour = Color.FromArgb(224, 224, 224);
        private bool _DrawHatch = true;
        private bool _DrawValueString = false;
        private bool _JumpToMouse = false;
        private ValueDivisor DividedValue = ValueDivisor.By1;

        #endregion
        #region Custom Properties

        public int Minimum
        {
            get { return _Minimum; }

            set
            {
                if (value >= _Maximum)
                    value = _Maximum - 10;
                if (_Value < value)
                    _Value = value;

                _Minimum = value;
                Invalidate();
            }
        }

        public int Maximum
        {
            get { return _Maximum; }

            set
            {
                if (value <= _Minimum)
                    value = _Minimum + 10;
                if (_Value > value)
                    _Value = value;

                _Maximum = value;
                Invalidate();
            }
        }

        public event ValueChangedEventHandler ValueChanged;
        public delegate void ValueChangedEventHandler();
        public int Value
        {
            get { return _Value; }
            set
            {
                if (_Value != value)
                {
                    if (value < _Minimum)
                    {
                        _Value = _Minimum;
                    }
                    else
                    {
                        if (value > _Maximum)
                        {
                            _Value = _Maximum;
                        }
                        else
                        {
                            _Value = value;
                        }
                    }
                    Invalidate();
                    if (ValueChanged != null)
                    {
                        ValueChanged();
                    }
                }
            }
        }

        public ValueDivisor ValueDivison
        {
            get
            {
                return this.DividedValue;
            }
            set
            {
                this.DividedValue = value;
                this.Invalidate();
            }
        }

        [Browsable(false)]
        public float ValueToSet
        {
            get
            {
                return (float)(((double)this._Value) / ((double)this.DividedValue));
            }
            set
            {
                this.Value = (int)Math.Round((double)(value * ((float)this.DividedValue)));
            }
        }

        public Color ValueColour
        {
            get { return _ValueColour; }
            set
            {
                _ValueColour = value;
                Invalidate();
            }
        }

        public bool DrawHatch
        {
            get { return _DrawHatch; }
            set
            {
                _DrawHatch = value;
                Invalidate();
            }
        }

        public bool DrawValueString
        {
            get { return _DrawValueString; }
            set
            {
                _DrawValueString = value;
                if (_DrawValueString == true)
                {
                    Height = 40;
                }
                else
                {
                    Height = 22;
                }
                Invalidate();
            }
        }

        public bool JumpToMouse
        {
            get
            {
                return this._JumpToMouse;
            }
            set
            {
                this._JumpToMouse = value;
            }
        }

        #endregion
        #region EventArgs

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if ((this.Cap && (e.X > -1)) && (e.X < (this.Width + 1)))
            {
                this.Value = this._Minimum + ((int)Math.Round((double)((this._Maximum - this._Minimum) * (((double)e.X) / ((double)this.Width)))));
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.ValueDrawer = (int)Math.Round((double)((((double)(this._Value - this._Minimum)) / ((double)(this._Maximum - this._Minimum))) * (this.Width - 11)));
                this.TrackBarHandleRect = new Rectangle(this.ValueDrawer, 0, 10, 20);
                this.Cap = this.TrackBarHandleRect.Contains(e.Location);
                if (this._JumpToMouse)
                {
                    this.Value = this._Minimum + ((int)Math.Round((double)((this._Maximum - this._Minimum) * (((double)e.X) / ((double)this.Width)))));
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.Cap = false;
        }


        #endregion

        public WC_3_TrackBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true);

            _DrawHatch = true;
            Size = new Size(80, 22);
            MinimumSize = new Size(37, 22);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_DrawValueString == true)
            {
                Height = 40;
            }
            else
            {
                Height = 22;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics G = e.Graphics;
            HatchBrush Hatch = new HatchBrush(HatchStyle.WideDownwardDiagonal, Color.FromArgb(20, Color.Black), Color.Transparent);
            G.Clear(Parent.BackColor);
            G.SmoothingMode = SmoothingMode.AntiAlias;
            checked
            {
                this.PipeBorder = RoundRectangle.RoundRect(1, 6, this.Width - 3, 8, 3);
                try
                {
                    this.ValueDrawer = (int)Math.Round(unchecked(checked((double)(this._Value - this._Minimum) / (double)(this._Maximum - this._Minimum)) * (double)checked(this.Width - 11)));
                }
                catch (Exception)
                {
                }
                this.TrackBarHandleRect = new Rectangle(this.ValueDrawer, 0, 10, 20);
                G.SetClip(this.PipeBorder);
                this.ValueRect = new Rectangle(1, 7, this.TrackBarHandleRect.X + this.TrackBarHandleRect.Width - 2, 7);
                this.VlaueLGB = new LinearGradientBrush(this.ValueRect, this._ValueColour, this._ValueColour, 90f);
                G.FillRectangle(this.VlaueLGB, this.ValueRect);

                if (_DrawHatch == true)
                {
                    G.FillRectangle(Hatch, this.ValueRect);
                }

                G.ResetClip();
                G.SmoothingMode = SmoothingMode.AntiAlias;
                G.DrawPath(new Pen(Color.FromArgb(180, 180, 180)), this.PipeBorder);
                this.TrackBarHandle = RoundRectangle.RoundRect(this.TrackBarHandleRect, 3);
                this.TrackBarHandleLGB = new LinearGradientBrush(this.ClientRectangle, SystemColors.Control, SystemColors.Control, 90f);
                G.FillPath(this.TrackBarHandleLGB, this.TrackBarHandle);
                G.DrawPath(new Pen(Color.FromArgb(180, 180, 180)), this.TrackBarHandle);

                if (_DrawValueString == true)
                {
                    G.DrawString(System.Convert.ToString(ValueToSet), Font, Brushes.Gray, 0, 25);
                }
            }
        }
    }

    #endregion

    public class WC_3_MenuStrip : MenuStrip
    {

        public WC_3_MenuStrip()
        {
            this.Renderer = new ControlRenderer();
        }

        public new ControlRenderer Renderer
        {
            get { return (ControlRenderer)base.Renderer; }
            set { base.Renderer = value; }
        }

    }

#endregion

}

#region "Themebase154"

abstract class ThemeContainer154 : ContainerControl
{

    #region " Initialization "

    protected Graphics G;
    protected Bitmap B;

    public ThemeContainer154()
    {
        SetStyle((ControlStyles)139270, true);

        _ImageSize = Size.Empty;
        Font = new Font("Verdana", 8);

        MeasureBitmap = new Bitmap(1, 1);
        MeasureGraphics = Graphics.FromImage(MeasureBitmap);

        DrawRadialPath = new GraphicsPath();

        InvalidateCustimization();
    }

    protected override sealed void OnHandleCreated(EventArgs e)
    {
        if (DoneCreation)
            InitializeMessages();

        InvalidateCustimization();
        ColorHook();

        if (!(_LockWidth == 0))
            Width = _LockWidth;
        if (!(_LockHeight == 0))
            Height = _LockHeight;
        if (!_ControlMode)
            base.Dock = DockStyle.Fill;

        Transparent = _Transparent;
        if (_Transparent && _BackColor)
            BackColor = Color.Transparent;

        base.OnHandleCreated(e);
    }

    private bool DoneCreation;
    protected override sealed void OnParentChanged(EventArgs e)
    {
        base.OnParentChanged(e);

        if (Parent == null)
            return;
        _IsParentForm = Parent is Form;

        if (!_ControlMode)
        {
            InitializeMessages();

            if (_IsParentForm)
            {
                ParentForm.FormBorderStyle = _BorderStyle;
                ParentForm.TransparencyKey = _TransparencyKey;

                if (!DesignMode)
                {
                    ParentForm.Shown += FormShown;
                }
            }

            Parent.BackColor = BackColor;
        }

        OnCreation();
        DoneCreation = true;
        InvalidateTimer();
    }

    #endregion

    private void DoAnimation(bool i)
    {
        OnAnimation();
        if (i)
            Invalidate();
    }

    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;

        if (_Transparent && _ControlMode)
        {
            PaintHook();
            e.Graphics.DrawImage(B, 0, 0);
        }
        else
        {
            G = e.Graphics;
            PaintHook();
        }
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        ThemeShare.RemoveAnimationCallback(DoAnimation);
        base.OnHandleDestroyed(e);
    }

    private bool HasShown;
    private void FormShown(object sender, EventArgs e)
    {
        if (_ControlMode || HasShown)
            return;

        if (_StartPosition == FormStartPosition.CenterParent || _StartPosition == FormStartPosition.CenterScreen)
        {
            Rectangle SB = Screen.PrimaryScreen.Bounds;
            Rectangle CB = ParentForm.Bounds;
            ParentForm.Location = new Point(SB.Width / 2 - CB.Width / 2, SB.Height / 2 - CB.Width / 2);
        }

        HasShown = true;
    }


    #region " Size Handling "

    private Rectangle Frame;
    protected override sealed void OnSizeChanged(EventArgs e)
    {
        if (_Movable && !_ControlMode)
        {
            Frame = new Rectangle(7, 7, Width - 14, _Header - 7);
        }

        InvalidateBitmap();
        Invalidate();

        base.OnSizeChanged(e);
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        if (!(_LockWidth == 0))
            width = _LockWidth;
        if (!(_LockHeight == 0))
            height = _LockHeight;
        base.SetBoundsCore(x, y, width, height, specified);
    }

    #endregion

    #region " State Handling "

    protected MouseState State;
    private void SetState(MouseState current)
    {
        State = current;
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized))
        {
            if (_Sizable && !_ControlMode)
                InvalidateMouse();
        }

        base.OnMouseMove(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        if (Enabled)
            SetState(MouseState.None);
        else
            SetState(MouseState.Block);
        base.OnEnabledChanged(e);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        SetState(MouseState.Over);
        base.OnMouseEnter(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        SetState(MouseState.Over);
        base.OnMouseUp(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        SetState(MouseState.None);

        if (GetChildAtPoint(PointToClient(MousePosition)) != null)
        {
            if (_Sizable && !_ControlMode)
            {
                Cursor = Cursors.Default;
                Previous = 0;
            }
        }

        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
            SetState(MouseState.Down);

        if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized || _ControlMode))
        {
            if (_Movable && Frame.Contains(e.Location))
            {
                Capture = false;
                WM_LMBUTTONDOWN = true;
                DefWndProc(ref Messages[0]);
            }
            else if (_Sizable && !(Previous == 0))
            {
                Capture = false;
                WM_LMBUTTONDOWN = true;
                DefWndProc(ref Messages[Previous]);
            }
        }

        base.OnMouseDown(e);
    }

    private bool WM_LMBUTTONDOWN;
    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        if (WM_LMBUTTONDOWN && m.Msg == 513)
        {
            WM_LMBUTTONDOWN = false;

            SetState(MouseState.Over);
            if (!_SmartBounds)
                return;

            if (IsParentMdi)
            {
                CorrectBounds(new Rectangle(Point.Empty, Parent.Parent.Size));
            }
            else
            {
                CorrectBounds(Screen.FromControl(Parent).WorkingArea);
            }
        }
    }

    private Point GetIndexPoint;
    private bool B1;
    private bool B2;
    private bool B3;
    private bool B4;
    private int GetIndex()
    {
        GetIndexPoint = PointToClient(MousePosition);
        B1 = GetIndexPoint.X < 7;
        B2 = GetIndexPoint.X > Width - 7;
        B3 = GetIndexPoint.Y < 7;
        B4 = GetIndexPoint.Y > Height - 7;

        if (B1 && B3)
            return 4;
        if (B1 && B4)
            return 7;
        if (B2 && B3)
            return 5;
        if (B2 && B4)
            return 8;
        if (B1)
            return 1;
        if (B2)
            return 2;
        if (B3)
            return 3;
        if (B4)
            return 6;
        return 0;
    }

    private int Current;
    private int Previous;
    private void InvalidateMouse()
    {
        Current = GetIndex();
        if (Current == Previous)
            return;

        Previous = Current;
        switch (Previous)
        {
            case 0:
                Cursor = Cursors.Default;
                break;
            case 1:
            case 2:
                Cursor = Cursors.SizeWE;
                break;
            case 3:
            case 6:
                Cursor = Cursors.SizeNS;
                break;
            case 4:
            case 8:
                Cursor = Cursors.SizeNWSE;
                break;
            case 5:
            case 7:
                Cursor = Cursors.SizeNESW;
                break;
        }
    }

    private Message[] Messages = new Message[9];
    private void InitializeMessages()
    {
        Messages[0] = Message.Create(Parent.Handle, 161, new IntPtr(2), IntPtr.Zero);
        for (int I = 1; I <= 8; I++)
        {
            Messages[I] = Message.Create(Parent.Handle, 161, new IntPtr(I + 9), IntPtr.Zero);
        }
    }

    private void CorrectBounds(Rectangle bounds)
    {
        if (Parent.Width > bounds.Width)
            Parent.Width = bounds.Width;
        if (Parent.Height > bounds.Height)
            Parent.Height = bounds.Height;

        int X = Parent.Location.X;
        int Y = Parent.Location.Y;

        if (X < bounds.X)
            X = bounds.X;
        if (Y < bounds.Y)
            Y = bounds.Y;

        int Width = bounds.X + bounds.Width;
        int Height = bounds.Y + bounds.Height;

        if (X + Parent.Width > Width)
            X = Width - Parent.Width;
        if (Y + Parent.Height > Height)
            Y = Height - Parent.Height;

        Parent.Location = new Point(X, Y);
    }

    #endregion


    #region " Base Properties "

    public override DockStyle Dock
    {
        get { return base.Dock; }
        set
        {
            if (!_ControlMode)
                return;
            base.Dock = value;
        }
    }

    private bool _BackColor;
    [Category("Misc")]
    public override Color BackColor
    {
        get { return base.BackColor; }
        set
        {
            if (value == base.BackColor)
                return;

            if (!IsHandleCreated && _ControlMode && value == Color.Transparent)
            {
                _BackColor = true;
                return;
            }

            base.BackColor = value;
            if (Parent != null)
            {
                if (!_ControlMode)
                    Parent.BackColor = value;
                ColorHook();
            }
        }
    }

    public override Size MinimumSize
    {
        get { return base.MinimumSize; }
        set
        {
            base.MinimumSize = value;
            if (Parent != null)
                Parent.MinimumSize = value;
        }
    }

    public override Size MaximumSize
    {
        get { return base.MaximumSize; }
        set
        {
            base.MaximumSize = value;
            if (Parent != null)
                Parent.MaximumSize = value;
        }
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            Invalidate();
        }
    }

    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            Invalidate();
        }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Color ForeColor
    {
        get { return Color.Empty; }
        set { }
    }
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Image BackgroundImage
    {
        get { return null; }
        set { }
    }
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override ImageLayout BackgroundImageLayout
    {
        get { return ImageLayout.None; }
        set { }
    }

    #endregion

    #region " Public Properties "

    private bool _SmartBounds = true;
    public bool SmartBounds
    {
        get { return _SmartBounds; }
        set { _SmartBounds = value; }
    }

    private bool _Movable = true;
    public bool Movable
    {
        get { return _Movable; }
        set { _Movable = value; }
    }

    private bool _Sizable = true;
    public bool Sizable
    {
        get { return _Sizable; }
        set { _Sizable = value; }
    }

    private Color _TransparencyKey;
    public Color TransparencyKey
    {
        get
        {
            if (_IsParentForm && !_ControlMode)
                return ParentForm.TransparencyKey;
            else
                return _TransparencyKey;
        }
        set
        {
            if (value == _TransparencyKey)
                return;
            _TransparencyKey = value;

            if (_IsParentForm && !_ControlMode)
            {
                ParentForm.TransparencyKey = value;
                ColorHook();
            }
        }
    }

    private FormBorderStyle _BorderStyle;
    public FormBorderStyle BorderStyle
    {
        get
        {
            if (_IsParentForm && !_ControlMode)
                return ParentForm.FormBorderStyle;
            else
                return _BorderStyle;
        }
        set
        {
            _BorderStyle = value;

            if (_IsParentForm && !_ControlMode)
            {
                ParentForm.FormBorderStyle = value;

                if (!(value == FormBorderStyle.None))
                {
                    Movable = false;
                    Sizable = false;
                }
            }
        }
    }

    private FormStartPosition _StartPosition;
    public FormStartPosition StartPosition
    {
        get
        {
            if (_IsParentForm && !_ControlMode)
                return ParentForm.StartPosition;
            else
                return _StartPosition;
        }
        set
        {
            _StartPosition = value;

            if (_IsParentForm && !_ControlMode)
            {
                ParentForm.StartPosition = value;
            }
        }
    }

    private bool _NoRounding;
    public bool NoRounding
    {
        get { return _NoRounding; }
        set
        {
            _NoRounding = value;
            Invalidate();
        }
    }

    private Image _Image;
    public Image Image
    {
        get { return _Image; }
        set
        {
            if (value == null)
                _ImageSize = Size.Empty;
            else
                _ImageSize = value.Size;

            _Image = value;
            Invalidate();
        }
    }

    private Dictionary<string, Color> Items = new Dictionary<string, Color>();
    public Bloom[] Colors
    {
        get
        {
            List<Bloom> T = new List<Bloom>();
            Dictionary<string, Color>.Enumerator E = Items.GetEnumerator();

            while (E.MoveNext())
            {
                T.Add(new Bloom(E.Current.Key, E.Current.Value));
            }

            return T.ToArray();
        }
        set
        {
            foreach (Bloom B in value)
            {
                if (Items.ContainsKey(B.Name))
                    Items[B.Name] = B.Value;
            }

            InvalidateCustimization();
            ColorHook();
            Invalidate();
        }
    }

    private string _Customization;
    public string Customization
    {
        get { return _Customization; }
        set
        {
            if (value == _Customization)
                return;

            byte[] Data = null;
            Bloom[] Items = Colors;

            try
            {
                Data = Convert.FromBase64String(value);
                for (int I = 0; I <= Items.Length - 1; I++)
                {
                    Items[I].Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4));
                }
            }
            catch
            {
                return;
            }

            _Customization = value;

            Colors = Items;
            ColorHook();
            Invalidate();
        }
    }

    private bool _Transparent;
    public bool Transparent
    {
        get { return _Transparent; }
        set
        {
            _Transparent = value;
            if (!(IsHandleCreated || _ControlMode))
                return;

            if (!value && !(BackColor.A == 255))
            {
                throw new Exception("Unable to change value to false while a transparent BackColor is in use.");
            }

            SetStyle(ControlStyles.Opaque, !value);
            SetStyle(ControlStyles.SupportsTransparentBackColor, value);

            InvalidateBitmap();
            Invalidate();
        }
    }

    #endregion

    #region " Private Properties "

    private Size _ImageSize;
    protected Size ImageSize
    {
        get { return _ImageSize; }
    }

    private bool _IsParentForm;
    protected bool IsParentForm
    {
        get { return _IsParentForm; }
    }

    protected bool IsParentMdi
    {
        get
        {
            if (Parent == null)
                return false;
            return Parent.Parent != null;
        }
    }

    private int _LockWidth;
    protected int LockWidth
    {
        get { return _LockWidth; }
        set
        {
            _LockWidth = value;
            if (!(LockWidth == 0) && IsHandleCreated)
                Width = LockWidth;
        }
    }

    private int _LockHeight;
    protected int LockHeight
    {
        get { return _LockHeight; }
        set
        {
            _LockHeight = value;
            if (!(LockHeight == 0) && IsHandleCreated)
                Height = LockHeight;
        }
    }

    private int _Header = 24;
    protected int Header
    {
        get { return _Header; }
        set
        {
            _Header = value;

            if (!_ControlMode)
            {
                Frame = new Rectangle(7, 7, Width - 14, value - 7);
                Invalidate();
            }
        }
    }

    private bool _ControlMode;
    protected bool ControlMode
    {
        get { return _ControlMode; }
        set
        {
            _ControlMode = value;

            Transparent = _Transparent;
            if (_Transparent && _BackColor)
                BackColor = Color.Transparent;

            InvalidateBitmap();
            Invalidate();
        }
    }

    private bool _IsAnimated;
    protected bool IsAnimated
    {
        get { return _IsAnimated; }
        set
        {
            _IsAnimated = value;
            InvalidateTimer();
        }
    }

    #endregion


    #region " Property Helpers "

    protected Pen GetPen(string name)
    {
        return new Pen(Items[name]);
    }
    protected Pen GetPen(string name, float width)
    {
        return new Pen(Items[name], width);
    }

    protected SolidBrush GetBrush(string name)
    {
        return new SolidBrush(Items[name]);
    }

    protected Color GetColor(string name)
    {
        return Items[name];
    }

    protected void SetColor(string name, Color value)
    {
        if (Items.ContainsKey(name))
            Items[name] = value;
        else
            Items.Add(name, value);
    }
    protected void SetColor(string name, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(r, g, b));
    }
    protected void SetColor(string name, byte a, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(a, r, g, b));
    }
    protected void SetColor(string name, byte a, Color value)
    {
        SetColor(name, Color.FromArgb(a, value));
    }

    private void InvalidateBitmap()
    {
        if (_Transparent && _ControlMode)
        {
            if (Width == 0 || Height == 0)
                return;
            B = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
            G = Graphics.FromImage(B);
        }
        else
        {
            G = null;
            B = null;
        }
    }

    private void InvalidateCustimization()
    {
        MemoryStream M = new MemoryStream(Items.Count * 4);

        foreach (Bloom B in Colors)
        {
            M.Write(BitConverter.GetBytes(B.Value.ToArgb()), 0, 4);
        }

        M.Close();
        _Customization = Convert.ToBase64String(M.ToArray());
    }

    private void InvalidateTimer()
    {
        if (DesignMode || !DoneCreation)
            return;

        if (_IsAnimated)
        {
            ThemeShare.AddAnimationCallback(DoAnimation);
        }
        else
        {
            ThemeShare.RemoveAnimationCallback(DoAnimation);
        }
    }

    #endregion


    #region " User Hooks "

    protected abstract void ColorHook();
    protected abstract void PaintHook();

    protected virtual void OnCreation()
    {
    }

    protected virtual void OnAnimation()
    {
    }

    #endregion


    #region " Offset "

    private Rectangle OffsetReturnRectangle;
    protected Rectangle Offset(Rectangle r, int amount)
    {
        OffsetReturnRectangle = new Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2));
        return OffsetReturnRectangle;
    }

    private Size OffsetReturnSize;
    protected Size Offset(Size s, int amount)
    {
        OffsetReturnSize = new Size(s.Width + amount, s.Height + amount);
        return OffsetReturnSize;
    }

    private Point OffsetReturnPoint;
    protected Point Offset(Point p, int amount)
    {
        OffsetReturnPoint = new Point(p.X + amount, p.Y + amount);
        return OffsetReturnPoint;
    }

    #endregion

    #region " Center "

    private Point CenterReturn;

    protected Point Center(Rectangle p, Rectangle c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X + c.X, (p.Height / 2 - c.Height / 2) + p.Y + c.Y);
        return CenterReturn;
    }
    protected Point Center(Rectangle p, Size c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X, (p.Height / 2 - c.Height / 2) + p.Y);
        return CenterReturn;
    }

    protected Point Center(Rectangle child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }
    protected Point Center(Size child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }
    protected Point Center(int childWidth, int childHeight)
    {
        return Center(Width, Height, childWidth, childHeight);
    }

    protected Point Center(Size p, Size c)
    {
        return Center(p.Width, p.Height, c.Width, c.Height);
    }

    protected Point Center(int pWidth, int pHeight, int cWidth, int cHeight)
    {
        CenterReturn = new Point(pWidth / 2 - cWidth / 2, pHeight / 2 - cHeight / 2);
        return CenterReturn;
    }

    #endregion

    #region " Measure "

    private Bitmap MeasureBitmap;
    private Graphics MeasureGraphics;

    protected Size Measure()
    {
        lock (MeasureGraphics)
        {
            return MeasureGraphics.MeasureString(Text, Font, Width).ToSize();
        }
    }
    protected Size Measure(string text)
    {
        lock (MeasureGraphics)
        {
            return MeasureGraphics.MeasureString(text, Font, Width).ToSize();
        }
    }

    #endregion


    #region " DrawPixel "

    private SolidBrush DrawPixelBrush;

    protected void DrawPixel(Color c1, int x, int y)
    {
        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
        }
        else
        {
            DrawPixelBrush = new SolidBrush(c1);
            G.FillRectangle(DrawPixelBrush, x, y, 1, 1);
        }
    }

    #endregion

    #region " DrawCorners "

    private SolidBrush DrawCornersBrush;

    protected void DrawCorners(Color c1, int offset)
    {
        DrawCorners(c1, 0, 0, Width, Height, offset);
    }
    protected void DrawCorners(Color c1, Rectangle r1, int offset)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset);
    }
    protected void DrawCorners(Color c1, int x, int y, int width, int height, int offset)
    {
        DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawCorners(Color c1)
    {
        DrawCorners(c1, 0, 0, Width, Height);
    }
    protected void DrawCorners(Color c1, Rectangle r1)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height);
    }
    protected void DrawCorners(Color c1, int x, int y, int width, int height)
    {
        if (_NoRounding)
            return;

        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
            B.SetPixel(x + (width - 1), y, c1);
            B.SetPixel(x, y + (height - 1), c1);
            B.SetPixel(x + (width - 1), y + (height - 1), c1);
        }
        else
        {
            DrawCornersBrush = new SolidBrush(c1);
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1);
        }
    }

    #endregion

    #region " DrawBorders "

    protected void DrawBorders(Pen p1, int offset)
    {
        DrawBorders(p1, 0, 0, Width, Height, offset);
    }
    protected void DrawBorders(Pen p1, Rectangle r, int offset)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset);
    }
    protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset)
    {
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawBorders(Pen p1)
    {
        DrawBorders(p1, 0, 0, Width, Height);
    }
    protected void DrawBorders(Pen p1, Rectangle r)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height);
    }
    protected void DrawBorders(Pen p1, int x, int y, int width, int height)
    {
        G.DrawRectangle(p1, x, y, width - 1, height - 1);
    }

    #endregion

    #region " DrawText "

    private Point DrawTextPoint;
    private Size DrawTextSize;

    protected void DrawText(Brush b1, HorizontalAlignment a, int x, int y)
    {
        DrawText(b1, Text, a, x, y);
    }
    protected void DrawText(Brush b1, string text, HorizontalAlignment a, int x, int y)
    {
        if (text.Length == 0)
            return;

        DrawTextSize = Measure(text);
        DrawTextPoint = new Point(Width / 2 - DrawTextSize.Width / 2, Header / 2 - DrawTextSize.Height / 2);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y);
                break;
        }
    }

    protected void DrawText(Brush b1, Point p1)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, p1);
    }
    protected void DrawText(Brush b1, int x, int y)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, x, y);
    }

    #endregion

    #region " DrawImage "

    private Point DrawImagePoint;

    protected void DrawImage(HorizontalAlignment a, int x, int y)
    {
        DrawImage(_Image, a, x, y);
    }
    protected void DrawImage(Image image, HorizontalAlignment a, int x, int y)
    {
        if (image == null)
            return;
        DrawImagePoint = new Point(Width / 2 - image.Width / 2, Header / 2 - image.Height / 2);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Center:
                G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Right:
                G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
        }
    }

    protected void DrawImage(Point p1)
    {
        DrawImage(_Image, p1.X, p1.Y);
    }
    protected void DrawImage(int x, int y)
    {
        DrawImage(_Image, x, y);
    }

    protected void DrawImage(Image image, Point p1)
    {
        DrawImage(image, p1.X, p1.Y);
    }
    protected void DrawImage(Image image, int x, int y)
    {
        if (image == null)
            return;
        G.DrawImage(image, x, y, image.Width, image.Height);
    }

    #endregion

    #region " DrawGradient "

    private LinearGradientBrush DrawGradientBrush;
    private Rectangle DrawGradientRectangle;

    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle);
    }
    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(ColorBlend blend, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, 90f);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }
    protected void DrawGradient(ColorBlend blend, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, angle);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }


    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle);
    }
    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(Color c1, Color c2, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillRectangle(DrawGradientBrush, r);
    }
    protected void DrawGradient(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, angle);
        G.FillRectangle(DrawGradientBrush, r);
    }

    #endregion

    #region " DrawRadial "

    private GraphicsPath DrawRadialPath;
    private PathGradientBrush DrawRadialBrush1;
    private LinearGradientBrush DrawRadialBrush2;
    private Rectangle DrawRadialRectangle;

    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, width / 2, height / 2);
    }
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, Point center)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, center.X, center.Y);
    }
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, int cx, int cy)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, cx, cy);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r)
    {
        DrawRadial(blend, r, r.Width / 2, r.Height / 2);
    }
    public void DrawRadial(ColorBlend blend, Rectangle r, Point center)
    {
        DrawRadial(blend, r, center.X, center.Y);
    }
    public void DrawRadial(ColorBlend blend, Rectangle r, int cx, int cy)
    {
        DrawRadialPath.Reset();
        DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1);

        DrawRadialBrush1 = new PathGradientBrush(DrawRadialPath);
        DrawRadialBrush1.CenterPoint = new Point(r.X + cx, r.Y + cy);
        DrawRadialBrush1.InterpolationColors = blend;

        if (G.SmoothingMode == SmoothingMode.AntiAlias)
        {
            G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3);
        }
        else
        {
            G.FillEllipse(DrawRadialBrush1, r);
        }
    }


    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawGradientRectangle);
    }
    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawGradientRectangle, angle);
    }

    protected void DrawRadial(Color c1, Color c2, Rectangle r)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillRectangle(DrawGradientBrush, r);
    }
    protected void DrawRadial(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, angle);
        G.FillEllipse(DrawGradientBrush, r);
    }

    #endregion

    #region " CreateRound "

    private GraphicsPath CreateRoundPath;
    private Rectangle CreateRoundRectangle;

    public GraphicsPath CreateRound(int x, int y, int width, int height, int slope)
    {
        CreateRoundRectangle = new Rectangle(x, y, width, height);
        return CreateRound(CreateRoundRectangle, slope);
    }

    public GraphicsPath CreateRound(Rectangle r, int slope)
    {
        CreateRoundPath = new GraphicsPath(FillMode.Winding);
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0f, 90f);
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90f, 90f);
        CreateRoundPath.CloseFigure();
        return CreateRoundPath;
    }

    #endregion

}

abstract class ThemeControl154 : Control
{


    #region " Initialization "

    protected Graphics G;
    protected Bitmap B;

    public ThemeControl154()
    {
        SetStyle((ControlStyles)139270, true);

        _ImageSize = Size.Empty;
        Font = new Font("Verdana", 8);

        MeasureBitmap = new Bitmap(1, 1);
        MeasureGraphics = Graphics.FromImage(MeasureBitmap);

        DrawRadialPath = new GraphicsPath();

        InvalidateCustimization();
        //Remove?
    }

    protected override sealed void OnHandleCreated(EventArgs e)
    {
        InvalidateCustimization();
        ColorHook();

        if (!(_LockWidth == 0))
            Width = _LockWidth;
        if (!(_LockHeight == 0))
            Height = _LockHeight;

        Transparent = _Transparent;
        if (_Transparent && _BackColor)
            BackColor = Color.Transparent;

        base.OnHandleCreated(e);
    }

    private bool DoneCreation;
    protected override sealed void OnParentChanged(EventArgs e)
    {
        if (Parent != null)
        {
            OnCreation();
            DoneCreation = true;
            InvalidateTimer();
        }

        base.OnParentChanged(e);
    }

    #endregion

    private void DoAnimation(bool i)
    {
        OnAnimation();
        if (i)
            Invalidate();
    }

    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;

        if (_Transparent)
        {
            PaintHook();
            e.Graphics.DrawImage(B, 0, 0);
        }
        else
        {
            G = e.Graphics;
            PaintHook();
        }
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        ThemeShare.RemoveAnimationCallback(DoAnimation);
        base.OnHandleDestroyed(e);
    }

    #region " Size Handling "

    protected override sealed void OnSizeChanged(EventArgs e)
    {
        if (_Transparent)
        {
            InvalidateBitmap();
        }

        Invalidate();
        base.OnSizeChanged(e);
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        if (!(_LockWidth == 0))
            width = _LockWidth;
        if (!(_LockHeight == 0))
            height = _LockHeight;
        base.SetBoundsCore(x, y, width, height, specified);
    }

    #endregion

    #region " State Handling "

    private bool InPosition;
    protected override void OnMouseEnter(EventArgs e)
    {
        InPosition = true;
        SetState(MouseState.Over);
        base.OnMouseEnter(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (InPosition)
            SetState(MouseState.Over);
        base.OnMouseUp(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
            SetState(MouseState.Down);
        base.OnMouseDown(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        InPosition = false;
        SetState(MouseState.None);
        base.OnMouseLeave(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        if (Enabled)
            SetState(MouseState.None);
        else
            SetState(MouseState.Block);
        base.OnEnabledChanged(e);
    }

    protected MouseState State;
    private void SetState(MouseState current)
    {
        State = current;
        Invalidate();
    }

    #endregion


    #region " Base Properties "

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Color ForeColor
    {
        get { return Color.Empty; }
        set { }
    }
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Image BackgroundImage
    {
        get { return null; }
        set { }
    }
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override ImageLayout BackgroundImageLayout
    {
        get { return ImageLayout.None; }
        set { }
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            Invalidate();
        }
    }
    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            Invalidate();
        }
    }

    private bool _BackColor;
    [Category("Misc")]
    public override Color BackColor
    {
        get { return base.BackColor; }
        set
        {
            if (!IsHandleCreated && value == Color.Transparent)
            {
                _BackColor = true;
                return;
            }

            base.BackColor = value;
            if (Parent != null)
                ColorHook();
        }
    }

    #endregion

    #region " Public Properties "

    private bool _NoRounding;
    public bool NoRounding
    {
        get { return _NoRounding; }
        set
        {
            _NoRounding = value;
            Invalidate();
        }
    }

    private Image _Image;
    public Image Image
    {
        get { return _Image; }
        set
        {
            if (value == null)
            {
                _ImageSize = Size.Empty;
            }
            else
            {
                _ImageSize = value.Size;
            }

            _Image = value;
            Invalidate();
        }
    }

    private bool _Transparent;
    public bool Transparent
    {
        get { return _Transparent; }
        set
        {
            _Transparent = value;
            if (!IsHandleCreated)
                return;

            if (!value && !(BackColor.A == 255))
            {
                throw new Exception("Unable to change value to false while a transparent BackColor is in use.");
            }

            SetStyle(ControlStyles.Opaque, !value);
            SetStyle(ControlStyles.SupportsTransparentBackColor, value);

            if (value)
                InvalidateBitmap();
            else
                B = null;
            Invalidate();
        }
    }

    private Dictionary<string, Color> Items = new Dictionary<string, Color>();
    public Bloom[] Colors
    {
        get
        {
            List<Bloom> T = new List<Bloom>();
            Dictionary<string, Color>.Enumerator E = Items.GetEnumerator();

            while (E.MoveNext())
            {
                T.Add(new Bloom(E.Current.Key, E.Current.Value));
            }

            return T.ToArray();
        }
        set
        {
            foreach (Bloom B in value)
            {
                if (Items.ContainsKey(B.Name))
                    Items[B.Name] = B.Value;
            }

            InvalidateCustimization();
            ColorHook();
            Invalidate();
        }
    }

    private string _Customization;
    public string Customization
    {
        get { return _Customization; }
        set
        {
            if (value == _Customization)
                return;

            byte[] Data = null;
            Bloom[] Items = Colors;

            try
            {
                Data = Convert.FromBase64String(value);
                for (int I = 0; I <= Items.Length - 1; I++)
                {
                    Items[I].Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4));
                }
            }
            catch
            {
                return;
            }

            _Customization = value;

            Colors = Items;
            ColorHook();
            Invalidate();
        }
    }

    #endregion

    #region " Private Properties "

    private Size _ImageSize;
    protected Size ImageSize
    {
        get { return _ImageSize; }
    }

    private int _LockWidth;
    protected int LockWidth
    {
        get { return _LockWidth; }
        set
        {
            _LockWidth = value;
            if (!(LockWidth == 0) && IsHandleCreated)
                Width = LockWidth;
        }
    }

    private int _LockHeight;
    protected int LockHeight
    {
        get { return _LockHeight; }
        set
        {
            _LockHeight = value;
            if (!(LockHeight == 0) && IsHandleCreated)
                Height = LockHeight;
        }
    }

    private bool _IsAnimated;
    protected bool IsAnimated
    {
        get { return _IsAnimated; }
        set
        {
            _IsAnimated = value;
            InvalidateTimer();
        }
    }

    #endregion


    #region " Property Helpers "

    protected Pen GetPen(string name)
    {
        return new Pen(Items[name]);
    }
    protected Pen GetPen(string name, float width)
    {
        return new Pen(Items[name], width);
    }

    protected SolidBrush GetBrush(string name)
    {
        return new SolidBrush(Items[name]);
    }

    protected Color GetColor(string name)
    {
        return Items[name];
    }

    protected void SetColor(string name, Color value)
    {
        if (Items.ContainsKey(name))
            Items[name] = value;
        else
            Items.Add(name, value);
    }
    protected void SetColor(string name, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(r, g, b));
    }
    protected void SetColor(string name, byte a, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(a, r, g, b));
    }
    protected void SetColor(string name, byte a, Color value)
    {
        SetColor(name, Color.FromArgb(a, value));
    }

    private void InvalidateBitmap()
    {
        if (Width == 0 || Height == 0)
            return;
        B = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
        G = Graphics.FromImage(B);
    }

    private void InvalidateCustimization()
    {
        MemoryStream M = new MemoryStream(Items.Count * 4);

        foreach (Bloom B in Colors)
        {
            M.Write(BitConverter.GetBytes(B.Value.ToArgb()), 0, 4);
        }

        M.Close();
        _Customization = Convert.ToBase64String(M.ToArray());
    }

    private void InvalidateTimer()
    {
        if (DesignMode || !DoneCreation)
            return;

        if (_IsAnimated)
        {
            ThemeShare.AddAnimationCallback(DoAnimation);
        }
        else
        {
            ThemeShare.RemoveAnimationCallback(DoAnimation);
        }
    }
    #endregion


    #region " User Hooks "

    protected abstract void ColorHook();
    protected abstract void PaintHook();

    protected virtual void OnCreation()
    {
    }

    protected virtual void OnAnimation()
    {
    }

    #endregion


    #region " Offset "

    private Rectangle OffsetReturnRectangle;
    protected Rectangle Offset(Rectangle r, int amount)
    {
        OffsetReturnRectangle = new Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2));
        return OffsetReturnRectangle;
    }

    private Size OffsetReturnSize;
    protected Size Offset(Size s, int amount)
    {
        OffsetReturnSize = new Size(s.Width + amount, s.Height + amount);
        return OffsetReturnSize;
    }

    private Point OffsetReturnPoint;
    protected Point Offset(Point p, int amount)
    {
        OffsetReturnPoint = new Point(p.X + amount, p.Y + amount);
        return OffsetReturnPoint;
    }

    #endregion

    #region " Center "

    private Point CenterReturn;

    protected Point Center(Rectangle p, Rectangle c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X + c.X, (p.Height / 2 - c.Height / 2) + p.Y + c.Y);
        return CenterReturn;
    }
    protected Point Center(Rectangle p, Size c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X, (p.Height / 2 - c.Height / 2) + p.Y);
        return CenterReturn;
    }

    protected Point Center(Rectangle child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }
    protected Point Center(Size child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }
    protected Point Center(int childWidth, int childHeight)
    {
        return Center(Width, Height, childWidth, childHeight);
    }

    protected Point Center(Size p, Size c)
    {
        return Center(p.Width, p.Height, c.Width, c.Height);
    }

    protected Point Center(int pWidth, int pHeight, int cWidth, int cHeight)
    {
        CenterReturn = new Point(pWidth / 2 - cWidth / 2, pHeight / 2 - cHeight / 2);
        return CenterReturn;
    }

    #endregion

    #region " Measure "

    private Bitmap MeasureBitmap;
    //TODO: Potential issues during multi-threading.
    private Graphics MeasureGraphics;

    protected Size Measure()
    {
        return MeasureGraphics.MeasureString(Text, Font, Width).ToSize();
    }
    protected Size Measure(string text)
    {
        return MeasureGraphics.MeasureString(text, Font, Width).ToSize();
    }

    #endregion


    #region " DrawPixel "

    private SolidBrush DrawPixelBrush;

    protected void DrawPixel(Color c1, int x, int y)
    {
        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
        }
        else
        {
            DrawPixelBrush = new SolidBrush(c1);
            G.FillRectangle(DrawPixelBrush, x, y, 1, 1);
        }
    }

    #endregion

    #region " DrawCorners "

    private SolidBrush DrawCornersBrush;

    protected void DrawCorners(Color c1, int offset)
    {
        DrawCorners(c1, 0, 0, Width, Height, offset);
    }
    protected void DrawCorners(Color c1, Rectangle r1, int offset)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset);
    }
    protected void DrawCorners(Color c1, int x, int y, int width, int height, int offset)
    {
        DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawCorners(Color c1)
    {
        DrawCorners(c1, 0, 0, Width, Height);
    }
    protected void DrawCorners(Color c1, Rectangle r1)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height);
    }
    protected void DrawCorners(Color c1, int x, int y, int width, int height)
    {
        if (_NoRounding)
            return;

        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
            B.SetPixel(x + (width - 1), y, c1);
            B.SetPixel(x, y + (height - 1), c1);
            B.SetPixel(x + (width - 1), y + (height - 1), c1);
        }
        else
        {
            DrawCornersBrush = new SolidBrush(c1);
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1);
        }
    }

    #endregion

    #region " DrawBorders "

    protected void DrawBorders(Pen p1, int offset)
    {
        DrawBorders(p1, 0, 0, Width, Height, offset);
    }
    protected void DrawBorders(Pen p1, Rectangle r, int offset)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset);
    }
    protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset)
    {
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawBorders(Pen p1)
    {
        DrawBorders(p1, 0, 0, Width, Height);
    }
    protected void DrawBorders(Pen p1, Rectangle r)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height);
    }
    protected void DrawBorders(Pen p1, int x, int y, int width, int height)
    {
        G.DrawRectangle(p1, x, y, width - 1, height - 1);
    }

    #endregion

    #region " DrawText "

    private Point DrawTextPoint;
    private Size DrawTextSize;

    protected void DrawText(Brush b1, HorizontalAlignment a, int x, int y)
    {
        DrawText(b1, Text, a, x, y);
    }
    protected void DrawText(Brush b1, string text, HorizontalAlignment a, int x, int y)
    {
        if (text.Length == 0)
            return;

        DrawTextSize = Measure(text);
        DrawTextPoint = Center(DrawTextSize);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y);
                break;
        }
    }

    protected void DrawText(Brush b1, Point p1)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, p1);
    }
    protected void DrawText(Brush b1, int x, int y)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, x, y);
    }

    #endregion

    #region " DrawImage "

    private Point DrawImagePoint;

    protected void DrawImage(HorizontalAlignment a, int x, int y)
    {
        DrawImage(_Image, a, x, y);
    }
    protected void DrawImage(Image image, HorizontalAlignment a, int x, int y)
    {
        if (image == null)
            return;
        DrawImagePoint = Center(image.Size);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Center:
                G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Right:
                G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
        }
    }

    protected void DrawImage(Point p1)
    {
        DrawImage(_Image, p1.X, p1.Y);
    }
    protected void DrawImage(int x, int y)
    {
        DrawImage(_Image, x, y);
    }

    protected void DrawImage(Image image, Point p1)
    {
        DrawImage(image, p1.X, p1.Y);
    }
    protected void DrawImage(Image image, int x, int y)
    {
        if (image == null)
            return;
        G.DrawImage(image, x, y, image.Width, image.Height);
    }

    #endregion

    #region " DrawGradient "

    private LinearGradientBrush DrawGradientBrush;
    private Rectangle DrawGradientRectangle;

    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle);
    }
    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(ColorBlend blend, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, 90f);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }
    protected void DrawGradient(ColorBlend blend, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, angle);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }


    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle);
    }
    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(Color c1, Color c2, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillRectangle(DrawGradientBrush, r);
    }
    protected void DrawGradient(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, angle);
        G.FillRectangle(DrawGradientBrush, r);
    }

    #endregion

    #region " DrawRadial "

    private GraphicsPath DrawRadialPath;
    private PathGradientBrush DrawRadialBrush1;
    private LinearGradientBrush DrawRadialBrush2;
    private Rectangle DrawRadialRectangle;

    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, width / 2, height / 2);
    }
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, Point center)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, center.X, center.Y);
    }
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, int cx, int cy)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, cx, cy);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r)
    {
        DrawRadial(blend, r, r.Width / 2, r.Height / 2);
    }
    public void DrawRadial(ColorBlend blend, Rectangle r, Point center)
    {
        DrawRadial(blend, r, center.X, center.Y);
    }
    public void DrawRadial(ColorBlend blend, Rectangle r, int cx, int cy)
    {
        DrawRadialPath.Reset();
        DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1);

        DrawRadialBrush1 = new PathGradientBrush(DrawRadialPath);
        DrawRadialBrush1.CenterPoint = new Point(r.X + cx, r.Y + cy);
        DrawRadialBrush1.InterpolationColors = blend;

        if (G.SmoothingMode == SmoothingMode.AntiAlias)
        {
            G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3);
        }
        else
        {
            G.FillEllipse(DrawRadialBrush1, r);
        }
    }


    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawRadialRectangle);
    }
    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawRadialRectangle, angle);
    }

    protected void DrawRadial(Color c1, Color c2, Rectangle r)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillEllipse(DrawRadialBrush2, r);
    }
    protected void DrawRadial(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, angle);
        G.FillEllipse(DrawRadialBrush2, r);
    }

    #endregion

    #region " CreateRound "

    private GraphicsPath CreateRoundPath;
    private Rectangle CreateRoundRectangle;

    public GraphicsPath CreateRound(int x, int y, int width, int height, int slope)
    {
        CreateRoundRectangle = new Rectangle(x, y, width, height);
        return CreateRound(CreateRoundRectangle, slope);
    }

    public GraphicsPath CreateRound(Rectangle r, int slope)
    {
        CreateRoundPath = new GraphicsPath(FillMode.Winding);
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0f, 90f);
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90f, 90f);
        CreateRoundPath.CloseFigure();
        return CreateRoundPath;
    }

    #endregion

}

static class ThemeShare
{

    #region " Animation "

    private static int Frames;
    private static bool Invalidate;
    public static PrecisionTimer ThemeTimer = new PrecisionTimer();

    //1000 / 50 = 20 FPS
    private const int FPS = 50;
    private const int Rate = 10;

    public delegate void AnimationDelegate(bool invalidate);

    private static List<AnimationDelegate> Callbacks = new List<AnimationDelegate>();

    private static void HandleCallbacks(IntPtr state, bool reserve)
    {
        Invalidate = (Frames >= FPS);
        if (Invalidate)
            Frames = 0;

        lock (Callbacks)
        {
            for (int I = 0; I <= Callbacks.Count - 1; I++)
            {
                Callbacks[I].Invoke(Invalidate);
            }
        }

        Frames += Rate;
    }

    private static void InvalidateThemeTimer()
    {
        if (Callbacks.Count == 0)
        {
            ThemeTimer.Delete();
        }
        else
        {
            ThemeTimer.Create(0, Rate, HandleCallbacks);
        }
    }

    public static void AddAnimationCallback(AnimationDelegate callback)
    {
        lock (Callbacks)
        {
            if (Callbacks.Contains(callback))
                return;

            Callbacks.Add(callback);
            InvalidateThemeTimer();
        }
    }

    public static void RemoveAnimationCallback(AnimationDelegate callback)
    {
        lock (Callbacks)
        {
            if (!Callbacks.Contains(callback))
                return;

            Callbacks.Remove(callback);
            InvalidateThemeTimer();
        }
    }

    #endregion

}

enum MouseState : byte
{
    None = 0,
    Over = 1,
    Down = 2,
    Block = 3
}

struct Bloom
{

    public string _Name;
    public string Name
    {
        get { return _Name; }
    }

    private Color _Value;
    public Color Value
    {
        get { return _Value; }
        set { _Value = value; }
    }

    public string ValueHex
    {
        get { return string.Concat("#", _Value.R.ToString("X2", null), _Value.G.ToString("X2", null), _Value.B.ToString("X2", null)); }
        set
        {
            try
            {
                _Value = ColorTranslator.FromHtml(value);
            }
            catch
            {
                return;
            }
        }
    }


    public Bloom(string name, Color value)
    {
        _Name = name;
        _Value = value;
    }
}

class PrecisionTimer : IDisposable
{

    private bool _Enabled;
    public bool Enabled
    {
        get { return _Enabled; }
    }

    private IntPtr Handle;
    private TimerDelegate TimerCallback;

    [DllImport("kernel32.dll", EntryPoint = "CreateTimerQueueTimer")]
    private static extern bool CreateTimerQueueTimer(ref IntPtr handle, IntPtr queue, TimerDelegate callback, IntPtr state, uint dueTime, uint period, uint flags);

    [DllImport("kernel32.dll", EntryPoint = "DeleteTimerQueueTimer")]
    private static extern bool DeleteTimerQueueTimer(IntPtr queue, IntPtr handle, IntPtr callback);

    public delegate void TimerDelegate(IntPtr r1, bool r2);

    public void Create(uint dueTime, uint period, TimerDelegate callback)
    {
        if (_Enabled)
            return;

        TimerCallback = callback;
        bool Success = CreateTimerQueueTimer(ref Handle, IntPtr.Zero, TimerCallback, IntPtr.Zero, dueTime, period, 0);

        if (!Success)
            ThrowNewException("CreateTimerQueueTimer");
        _Enabled = Success;
    }

    public void Delete()
    {
        if (!_Enabled)
            return;
        bool Success = DeleteTimerQueueTimer(IntPtr.Zero, Handle, IntPtr.Zero);

        if (!Success && !(Marshal.GetLastWin32Error() == 997))
        {
            ThrowNewException("DeleteTimerQueueTimer");
        }

        _Enabled = !Success;
    }

    private void ThrowNewException(string name)
    {
        throw new Exception(string.Format("{0} failed. Win32Error: {1}", name, Marshal.GetLastWin32Error()));
    }

    public void Dispose()
    {
        Delete();
    }
}

#endregion