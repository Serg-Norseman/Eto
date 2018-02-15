using System;
using Eto.Forms;
using Eto.Drawing;
using Eto.Mac.Drawing;

#if XAMMAC2
using AppKit;
using Foundation;
using CoreGraphics;
using ObjCRuntime;
using CoreAnimation;
using CoreImage;
#else
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using MonoMac.ObjCRuntime;
using MonoMac.CoreAnimation;
using MonoMac.CoreImage;
#if Mac64
using nfloat = System.Double;
using nint = System.Int64;
using nuint = System.UInt64;
#else
using nfloat = System.Single;
using nint = System.Int32;
using nuint = System.UInt32;
#endif
#if SDCOMPAT
using CGSize = System.Drawing.SizeF;
using CGRect = System.Drawing.RectangleF;
using CGPoint = System.Drawing.PointF;
#endif
#endif

namespace Eto.Mac.Forms.Controls
{
	public class GroupBoxHandler : MacPanel<NSBox, GroupBox, GroupBox.ICallback>, GroupBox.IHandler
	{
		Font font;

		public class EtoBox : NSBox, IMacControl
		{
			public WeakReference WeakHandler { get; set; }

			public object Handler
			{ 
				get { return WeakHandler.Target; }
				set { WeakHandler = new WeakReference(value); } 
			}

			public EtoBox()
			{
				Title = string.Empty;
				ContentView = new NSView();
			}
		}

		protected override NSBox CreateControl()
		{
			return new EtoBox();
		}

		protected override void Initialize()
		{
			Enabled = true;
			base.Initialize();
		}

		public override NSView ContainerControl
		{
			get { return Control; }
		}

		public override NSView ContentControl
		{
			get { return (NSView)Control.ContentView; }
		}

		public override bool Enabled { get; set; }

		public override Size ClientSize
		{
			get
			{
				var view = Control.ContentView as NSView;
				return view.Frame.Size.ToEtoSize();
			}
			set
			{
				Control.SetFrameFromContentFrame(new CGRect(0, 0, value.Width, value.Height));
			}
		}

		public Font Font
		{
			get
			{
				return font ?? (font = new Font(new FontHandler(Control.TitleFont)));
			}
			set
			{
				font = value;
				Control.TitleFont = font == null ? null : ((FontHandler)font.Handler).Control;
				LayoutIfNeeded();
			}
		}

		public virtual string Text
		{
			get { return Control.Title; }
			set { Control.Title = value; }
		}

		public override SizeF GetPreferredSize(SizeF availableSize)
		{
			var boundsSize = new SizeF(14, (float)TitleCell.CellSize.Height + 8);
			availableSize -= boundsSize;

			return base.GetPreferredSize(availableSize) + boundsSize;
		}

		public override void SetContentSize(CGSize contentSize)
		{
			Control.SetFrameFromContentFrame(new CGRect(0, 0, contentSize.Width, contentSize.Height));
		}

		NSTextFieldCell TitleCell { get { return (NSTextFieldCell)Control.TitleCell; } }

		public Color TextColor
		{
			get { return TitleCell.TextColor.ToEto(); }
			set
			{ 
				TitleCell.TextColor = value.ToNSUI(); 
				Control.SetNeedsDisplay();
			}
		}
	}
}
