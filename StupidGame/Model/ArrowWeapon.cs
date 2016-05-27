using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SampleGame.Model;
using SampleGame.View;

namespace SampleGame.Model
{
	public class ArrowWeapon
	{
		public Texture2D texture;
		public Vector2 position;
		public int damage;
		public Viewport viewport;
		public float boomBoxSpeed;

		public int Damage
		{
			get { return damage; }
			set { damage = value; }
		}

		public Texture2D Texture
		{
			get { return texture; }
			set { texture = value; }
		}
		public Vector2 Position 
		{
			get { return position; }
			set { position = value; }
		}

		// The speed at which the enemy moves
		public float BoomBoxSpeed
		{
			get { return boomBoxSpeed; }
			set { boomBoxSpeed = value; }
		}

		public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
		{
			this.texture = texture;
			this.position = position;
			this.damage = 5000;
			this.boomBoxSpeed = .2f;
					
				
		}

		public void Update()
		{
			// Projectiles always move to the right
			//Position.X += boomBoxSpeed;
			//Position.Y += .25f;

		}

		public void Draw(SpriteBatch spriteBatch)
		{
			//spriteBatch.Draw(Texture, Position, null, Color.White, 0f,
			//	new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
		}
			
	}
}



