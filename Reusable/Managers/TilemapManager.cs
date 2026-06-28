using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Reusable.Managers
{
	public class TilemapManager
	{

		public TiledMap? TiledMap { get; private set; }
		private Func<string, Texture2D> textureSource;

		public TilemapManager(string tileMapPath, Func<string, Texture2D> textureSource)
		{
			

			if (tileMapPath != null)
			{
				SetTileMapPath(tileMapPath);
			}


			this.textureSource = textureSource;

		}

		public void ForEachObject(Action<TiledLayer, TiledObject> action)
		{
			foreach (var layer in TiledMap.Layers)
			{
				if (layer.Type != "objectgroup")
					continue;

				foreach (var obj in layer.Objects)
				{
					action.Invoke(layer, obj);
				}
			}
		}

		public void SetTileMapPath(string tileDataPath)
		{
			string json = File.ReadAllText(tileDataPath);
			TiledMap = JsonSerializer.Deserialize<TiledMap>(json);
		}


		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (TiledLayer layer in TiledMap.Layers)
			{

				if (layer.Type == "objectgroup")
				{
					continue;
				}

				for (int i = 0; i < layer.Data.Count; i++)
				{

					var data = layer.Data[i];

					if (data == 0)
						continue;

					TiledTileset tileset = FindTilesetForGid(data);
					Vector2 tilePosition = GetTilePosition(i);
					Rectangle tileSource = GetTileSource(data, tileset);
					Texture2D tilesetTexture = GetTileTexture(tileset);

					spriteBatch.Draw(tilesetTexture, tilePosition, tileSource, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

				}
			}
		}

		TiledTileset FindTilesetForGid(int gid)
		{

			for (int i = TiledMap.Tilesets.Count - 1; i >= 0; i--)
			{
				if (gid >= TiledMap.Tilesets[i].FirstGid)
					return TiledMap.Tilesets[i];
			}

			throw new Exception($"No matching tileset found for GID {gid}");
		}


		private Texture2D GetTileTexture(TiledTileset tileset)
		{

			string tilesetTexturePath = Path.ChangeExtension(tileset.Image, null);



			return textureSource(tilesetTexturePath);

			
		}



		private Vector2 GetTilePosition(int index)
		{
			Vector2 coordinate = Utils.IndexToCoordinate(index, TiledMap.Width).ToVector2();

			return new Vector2(TiledMap.TileWidth * coordinate.X, TiledMap.TileHeight * coordinate.Y);
		}

		private Rectangle GetTileSource(int data, TiledTileset tileset)
		{
			int localIndex = data - tileset.FirstGid;

			Point coordinate = Utils.IndexToCoordinate(localIndex, tileset.Columns);

			return new Rectangle(coordinate.X * TiledMap.TileWidth, coordinate.Y * TiledMap.TileHeight, TiledMap.TileWidth, TiledMap.TileHeight);
		}


	}


	public class TiledMap
	{
		[JsonPropertyName("compressionlevel")]
		public int CompressionLevel { get; set; }

		[JsonPropertyName("height")]
		public int Height { get; set; }

		[JsonPropertyName("width")]
		public int Width { get; set; }

		[JsonPropertyName("infinite")]
		public bool Infinite { get; set; }

		[JsonPropertyName("layers")]
		public List<TiledLayer> Layers { get; set; } = new();

		[JsonPropertyName("nextlayerid")]
		public int NextLayerId { get; set; }

		[JsonPropertyName("nextobjectid")]
		public int NextObjectId { get; set; }

		[JsonPropertyName("orientation")]
		public string Orientation { get; set; } = string.Empty;

		[JsonPropertyName("renderorder")]
		public string RenderOrder { get; set; } = string.Empty;

		[JsonPropertyName("tiledversion")]
		public string TiledVersion { get; set; } = string.Empty;

		[JsonPropertyName("tileheight")]
		public int TileHeight { get; set; }

		[JsonPropertyName("tilesets")]
		public List<TiledTileset> Tilesets { get; set; } = new();

		[JsonPropertyName("tilewidth")]
		public int TileWidth { get; set; }

		public int WorldPixelWidth => TileWidth * Width;

		public int WorldPixelHeight => TileHeight * Height;

		[JsonPropertyName("type")]
		public string Type { get; set; } = string.Empty;

		[JsonPropertyName("version")]
		public string Version { get; set; } = string.Empty;


	}

	public class TiledLayer
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;

		[JsonPropertyName("type")]
		public string Type { get; set; } = string.Empty; // tilelayer | objectgroup

		[JsonPropertyName("opacity")]
		public float Opacity { get; set; }

		[JsonPropertyName("visible")]
		public bool Visible { get; set; }

		[JsonPropertyName("width")]
		public int Width { get; set; }

		[JsonPropertyName("height")]
		public int Height { get; set; }

		[JsonPropertyName("x")]
		public int X { get; set; }

		[JsonPropertyName("y")]
		public int Y { get; set; }

		// Tile layers only
		[JsonPropertyName("data")]
		public List<int> Data { get; set; }

		// Object layers only
		[JsonPropertyName("objects")]
		public List<TiledObject> Objects { get; set; }

		[JsonPropertyName("draworder")]
		public string DrawOrder { get; set; }

		[JsonPropertyName("properties")]
		public List<TiledProperty> Properties { get; set; }
	}

	public class TiledObject
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;

		[JsonPropertyName("type")]
		public string Type { get; set; } = string.Empty;

		[JsonPropertyName("x")]
		public float X { get; set; }

		[JsonPropertyName("y")]
		public float Y { get; set; }

		[JsonPropertyName("width")]
		public float Width { get; set; }

		[JsonPropertyName("height")]
		public float Height { get; set; }

		[JsonPropertyName("rotation")]
		public float Rotation { get; set; }

		[JsonPropertyName("visible")]
		public bool Visible { get; set; }

		[JsonPropertyName("point")]
		public bool Point { get; set; }

		[JsonPropertyName("properties")]
		public List<TiledProperty> Properties { get; set; }
	}

	public class TiledTileset
	{
		[JsonPropertyName("columns")]
		public int Columns { get; set; }

		[JsonPropertyName("firstgid")]
		public int FirstGid { get; set; }

		[JsonPropertyName("image")]
		public string Image { get; set; } = string.Empty;

		[JsonPropertyName("imageheight")]
		public int ImageHeight { get; set; }

		[JsonPropertyName("imagewidth")]
		public int ImageWidth { get; set; }

		[JsonPropertyName("margin")]
		public int Margin { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;

		[JsonPropertyName("spacing")]
		public int Spacing { get; set; }

		[JsonPropertyName("tilecount")]
		public int TileCount { get; set; }

		[JsonPropertyName("tileheight")]
		public int TileHeight { get; set; }

		[JsonPropertyName("tilewidth")]
		public int TileWidth { get; set; }
	}

	public class TiledProperty
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("value")]
		public object Value { get; set; }
	}

	public static class TiledPropertyExtensions
	{
		public static T GetValue<T>(this List<TiledProperty> propList, string name)
		{
			var prop = propList.FirstOrDefault(m => m.Name == name);

			if (prop == null)
				throw new Exception($"Property '{name}' not found");

			if (prop.Value is JsonElement element)
			{
				switch (prop.Type)
				{
					case "int":
						return (T)(object)element.GetInt32();

					case "float":
						return (T)(object)element.GetSingle();

					case "string":
						return (T)(object)element.GetString();

					case "bool":
						return (T)(object)element.GetBoolean();
				}
			}

			return (T)prop.Value;
		}
	}
}
