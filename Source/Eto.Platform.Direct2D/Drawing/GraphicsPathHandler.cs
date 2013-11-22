using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s = SharpDX;
using sd = SharpDX.Direct2D1;
using sw = SharpDX.DirectWrite;
using Eto.Drawing;

namespace Eto.Platform.Direct2D.Drawing
{
	/// <summary>
	/// Handler for <see cref="IGraphicsPath"/>
	/// </summary>
	/// <copyright>(c) 2013 by Vivek Jhaveri</copyright>
	/// <license type="BSD-3">See LICENSE for full terms</license>
	public class GraphicsPathHandler : IGraphicsPathHandler
	{
		sd.PathGeometry Control { get; set; }
		private sd.GeometrySink sink;
		public PointF CurrentPoint { get; private set; }
		public object ControlObject { get { return this.Control; } }
		public RectangleF Bounds { get { return Control.GetBounds().ToEto(); } }

		#region Constructors

		public GraphicsPathHandler()
		{
			this.Control = new sd.PathGeometry(SDFactory.D2D1Factory);
			this.sink = this.Control.Open();
		}

		#endregion

		public void AddBezier(PointF pt1, PointF pt2, PointF pt3, PointF pt4)
		{
			sink.AddLine(pt1.ToWpf());

			sink.AddBezier(new sd.BezierSegment
			{
				Point1 = pt2.ToWpf(),
				Point2 = pt3.ToWpf(),
				Point3 = pt4.ToWpf()
			});
		}

		public void AddBeziers(Point[] p)
		{
			if (p != null &&
				p.Length > 3)
			{
				sink.AddLine(p[0].ToWpf());

				var i = 1;
				while (p.Length > i + 2)
				{
					sink.AddBezier(new sd.BezierSegment
					{
						Point1 = p[i].ToWpf(),
						Point2 = p[i + 1].ToWpf(),
						Point3 = p[i + 2].ToWpf()
					});

					i = i + 3;
				}
			}
		}

		public void AddCurve(IEnumerable<PointF> points, float tension = 0.5f)
		{
			throw new NotImplementedException();
		}

		public void AddPath(IGraphicsPath path, bool connect)
		{
			throw new NotImplementedException();
		}

		public IGraphicsPath Clone()
		{
			throw new NotImplementedException();
		}

		public FillMode FillMode
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public bool IsEmpty
		{
			get { throw new NotImplementedException(); }
		}

		public void StartFigure()
		{
			throw new NotImplementedException();
			//sink.BeginFigure(
		}

		public void CloseFigure()
		{
			sink.EndFigure(sd.FigureEnd.Closed);
		}

		public void Translate(PointF point)
		{
			throw new NotImplementedException();
		}

		public void Transform(IMatrix matrix)
		{
			throw new NotImplementedException();
		}

		public void AddLine(float startX, float startY, float endX, float endY)
		{
			sink.AddLines(
				new s.DrawingPointF[]
                {
					new s.DrawingPointF(startX, startY),
					new s.DrawingPointF(endX, endY)  
                });
		}

		public void LineTo(float x, float y)
		{
			throw new NotImplementedException();
		}

		public void MoveTo(float x, float y)
		{
			sink.BeginFigure(
				new s.DrawingPointF(x, y),
				sd.FigureBegin.Filled); // is this correct?
		}

		public void AddArc(float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			throw new NotImplementedException();
		}

		public void AddEllipse(float x, float y, float width, float height)
		{
			throw new NotImplementedException();
		}

		public void AddRectangle(float x, float y, float width, float height)
		{
			var left = x;
			var top = y;
			var right = x + width - 1;
			var bottom = y + height - 1;

			sink.AddLines(
				new s.DrawingPointF[]
                {
                   new s.DrawingPointF(left, top),
                   new s.DrawingPointF(right, top),
                   new s.DrawingPointF(right, bottom),
                   new s.DrawingPointF(left, bottom),
                   new s.DrawingPointF(left, top),
                });
		}

		public void AddLines(IEnumerable<PointF> points)
		{
			sink.AddLines(points.ToArray().ToDx());
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
