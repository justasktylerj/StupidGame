// Projectile.cs
//Using declarations
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SampleGame.Model;
using SampleGame.View;

namespace SampleGame.Model
{
	public class Projectile
	{
		public Texture2D Texture;
		public Vector2 Position;
		public bool active;
		public int damage;
		public Viewport viewport;
		public float projectileMoveSpeed;

		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		public int Width
		{
			get { return Texture.Width; }
		}
		public int Height
		{
			get { return Texture.Height; }
		}

		public int Damage
		{
			get { return damage; }
			set { damage = value; }
		}

		// The speed at which the enemy moves
		public float ProjectileMoveSpeed
		{
			get { return projectileMoveSpeed; }
			set { projectileMoveSpeed = value; }
		}

		public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
		{
			Texture = texture;
			Position = position;
			this.viewport = viewport;

			Active = true;

			Damage = 3;

			ProjectileMoveSpeed = 10f;
		}

		public void Update()
		{
			// Projectiles always move to the right
			Position.X += projectileMoveSpeed;

			// Deactivate the bullet if it goes out of screen
			if (Position.X + Texture.Width / 2 > viewport.Width)
				Active = false;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Texture, Position, null, Color.White, 0f,
				new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
		}

		public Projectile ()
		{
			
		}
	}
}

