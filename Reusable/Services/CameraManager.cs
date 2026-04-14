using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reusable.Services
{
	public class CameraManager
	{
		public Rectangle WorldBounds { get; set; }
		private Vector2 location;
		public Vector2 Location
		{
			get => location;
			set
			{
				location = value;
				ClampToWorld();
			}
		}

		public Rectangle CameraBounds { get; set; }

		public Matrix TransformMatrix
		{
			get
			{ 
				var roundedLocation = new Vector2((float)Math.Round(Location.X), (float)Math.Round(Location.Y));
			
				return
					Matrix.CreateTranslation(new Vector3(-roundedLocation.X, -roundedLocation.Y, 0)) *
					Matrix.CreateTranslation(new Vector3(CameraBounds.Width * .5f, CameraBounds.Height * .5f, 0));
			}
		}

		public CameraManager(Rectangle cameraBounds, Rectangle worldBounds)
		{
			CameraBounds = cameraBounds;
			WorldBounds = worldBounds;

			ClampToWorld();
		}

		public Vector2 ScreenToWorld(Vector2 point)
		{
			return Vector2.Transform(point, Matrix.Invert(TransformMatrix));
		}

		public Vector2 WorldToScreen(Vector2 point)
		{
			return Vector2.Transform(point, TransformMatrix);
		}

		private void ClampToWorld()
		{
			float halfWidth = CameraBounds.Width * 0.5f;
			float halfHeight = CameraBounds.Height * 0.5f;

			var locationX = MathHelper.Clamp(
				location.X,
				WorldBounds.Left + halfWidth,
				WorldBounds.Right - halfWidth);

			var locationY = MathHelper.Clamp(
				location.Y,
				WorldBounds.Top + halfHeight,
				WorldBounds.Bottom - halfHeight);

			location = new Vector2(locationX, locationY);	
		}

		public Rectangle VisibleArea
		{
			get
			{
				var inverseViewMatrix = Matrix.Invert(TransformMatrix);
				var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
				var tr = Vector2.Transform(new Vector2(CameraBounds.X, 0), inverseViewMatrix);
				var bl = Vector2.Transform(new Vector2(0, CameraBounds.Y), inverseViewMatrix);
				var br = Vector2.Transform(new Vector2(CameraBounds.X, CameraBounds.Y), inverseViewMatrix);
				var min = new Vector2(
					MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
					MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
				var max = new Vector2(
					MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
					MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
				return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
			}
		}
	}
}
