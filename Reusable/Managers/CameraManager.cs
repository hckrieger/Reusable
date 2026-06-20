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

		public Vector2 ScreenOffset { get; set; } = Vector2.Zero;

		public Matrix TransformMatrix
		{
			get
			{ 
				var roundedLocation = new Vector2((float)Math.Round(Location.X), (float)Math.Round(Location.Y));
			
				return
					Matrix.CreateTranslation(new Vector3(-roundedLocation.X, -roundedLocation.Y, 0)) *
					Matrix.CreateTranslation(new Vector3(CameraBounds.Width * .5f + ScreenOffset.X, CameraBounds.Height * .5f + ScreenOffset.Y, 0));
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

			float offsetX = ScreenOffset.X;
			float offsetY = ScreenOffset.Y;

			var locationX = MathHelper.Clamp(
				location.X,
				WorldBounds.Left + halfWidth + offsetX,
				WorldBounds.Right - halfWidth + offsetX);

			var locationY = MathHelper.Clamp(
				location.Y,
				WorldBounds.Top + halfHeight + offsetY,
				WorldBounds.Bottom - halfHeight + offsetY);

			location = new Vector2(locationX, locationY);	
		}

		public Rectangle VisibleArea
		{
			get
			{
				int left = (int)(Location.X - CameraBounds.Width * 0.05f);
				int top = (int)(Location.Y - CameraBounds.Height * 0.05f);

				return new Rectangle(left, top, CameraBounds.Width, CameraBounds.Height);
			}
		}
	}
}
