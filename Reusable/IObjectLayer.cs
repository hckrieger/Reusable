using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reusable
{
	public interface IObjectLayer
	{
		public string Name { get; }
		public void Update(GameTime gameTime);
		public void Draw(SpriteBatch spriteBatch);
	}
}
