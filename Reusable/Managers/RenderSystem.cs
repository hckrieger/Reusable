using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Jewely
{
	public class RenderSystem
	{

		private RenderableDataStore data;
		private ContentManager Content;
		private int nth = 0;
		public RenderSystem(Game game, int entityCount)
		{
			Content = game.Services.GetService<ContentManager>();
			data = new RenderableDataStore(entityCount);
		}

		public int AddDataEntity(RenderableDataInstance dataInstance)
		{
			var id = nth++;
			data.TextureKey[id] = dataInstance.TextureKey;
			data.Position[id] = dataInstance.Position;
			data.SourceRectangle[id] = dataInstance.SourceRectangle;
			data.Color[id] = dataInstance.Color;
			data.Rotation[id] = dataInstance.Rotation;
			data.Origin[id] = dataInstance.Origin;
			data.Scale[id] = dataInstance.Scale;
			data.SpriteEffects[id] = dataInstance.SpriteEffects;
			data.LayerDepth[id] = dataInstance.LayerDepth;
			return id;
		}


		public void ClearData()
		{
			data.TextureKey = [];
			data.Position = [];
			data.SourceRectangle = [];
			data.Color = [];
			data.Rotation = [];
			data.Origin = [];
			data.Scale = [];
			data.SpriteEffects = [];
			data.LayerDepth = [];
			nth = 0;
		}

		public RenderableDataStore Data => data;

		public int CurrentId;


		public void Draw(SpriteBatch spriteBatch, Func<string, Texture2D> textureSource)
		{
			for (int i = 0; i < nth; i++) 
			{
				spriteBatch.Draw(
					textureSource(data.TextureKey[i]),
					data.Position[i],
					data.SourceRectangle[i],
					data.Color[i],
					data.Rotation[i],
					data.Origin[i],
					data.Scale[i],
					data.SpriteEffects[i],
					data.LayerDepth[i]
					);
			}
		}
	
	}

	public struct RenderableDataStore(int count)
	{
		public string[] TextureKey = new string[count];
		public Vector2[] Position = new Vector2[count];
		public Rectangle[] SourceRectangle = new Rectangle[count];
		public Color[] Color = new Color[count];
		public float[] Rotation = new float[count];
		public Vector2[] Origin = new Vector2[count];
		public Vector2[] Scale = new Vector2[count];
		public SpriteEffects[] SpriteEffects = new SpriteEffects[count];
		public float[] LayerDepth = new float[count];
		
	}

	public struct RenderableDataInstance()
	{
		public string TextureKey = string.Empty;
		public Vector2 Position;
		public Rectangle SourceRectangle;
		public Color Color = Color.White;
		public float Rotation;
		public Vector2 Origin = Vector2.Zero;
		public Vector2 Scale = Vector2.One;
		public SpriteEffects SpriteEffects = SpriteEffects.None;
		public float LayerDepth = 1f;
		
	}


}
