using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Graphics;

namespace Moyeu
{
	public class ChronometerView : View
	{
		Color baseQuadrantColor;
		Color chromeColor;

		TimeSpan time;

		public ChronometerView (Context context) :
			base (context)
		{
			Initialize ();
		}

		public ChronometerView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public ChronometerView (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
			baseQuadrantColor = Color.Rgb (0x33, 0xb5, 0xe5);
			chromeColor = Color.Rgb (0x33, 0x33, 0x33);
		}

		public TimeSpan Time {
			get {
				return time;
			}
			set {
				time = value;
				Invalidate ();
			}
		}

		protected override void OnDraw (Android.Graphics.Canvas canvas)
		{
			var radius = (Height / 2) - 4;
			var middleX = Width / 2;
			var middleY = Height / 2;

			var minutes = (int)time.TotalMinutes;
			var rotation = minutes / 60;
			var color = baseQuadrantColor;

			for (int i = 0; i <= rotation; i++) {
				var angle = minutes > 60 ? 360 : (int)((minutes * 360f) / 60);

				canvas.DrawArc (new RectF (middleX - radius, middleY - radius, middleX + radius, middleY + radius), -90, angle, true, new Paint {
					Color = color,
					AntiAlias = true,
				});

				minutes -= 60;
				color = Color.Rgb ((byte)Math.Max (0, color.R - 30),
				                   (byte)Math.Max (0, color.G - 30),
				                   (byte)Math.Max (0, color.B - 30));
			}

			// Draw chrome
			Paint chromeBorder = new Paint {
				AntiAlias = true,
				Color = chromeColor,
				StrokeWidth = 1.ToPixels ()
			};
			chromeBorder.SetStyle (Paint.Style.Stroke);
			canvas.DrawCircle (middleX, middleY, radius, chromeBorder);

			// Draw markers
			var markerPaint = new Paint {
				Color = chromeColor,
				AntiAlias = true
			};
			var innerRadius = radius - 6;
			for (int i = 0; i < 8; i++) {
				var stepAngle = i * Math.PI / 4;
				canvas.DrawCircle (middleX - (int)Math.Round (Math.Sin (stepAngle) * innerRadius),
				                   middleY - (int)Math.Round (Math.Cos (stepAngle) * innerRadius),
				                   1, markerPaint);
			}
		}
	}
}

