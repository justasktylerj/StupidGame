using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SampleGame.Model;
using SampleGame.View;

namespace SampleGame.Model
{
	public class Boss 
	{
		// Animation representing the enemy
		public Animation bossAnimation;
		public Vector2 position;
		public bool active;
		public int health;
		public int damage;
		public int damageMin;
		public int worth;
		public float bossMoveSpeed;

		public Animation BossAnimation;

		// Position of the Player relative to the upper left side of the screen
		public Vector2 Position;

		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		// Amount of hit points that player has
		public int Health
		{
			get { return health; }
			set { health = value; }
		}

		// Get the width of the player ship

		public int Width
		{
			get { return BossAnimation.FrameWidth; } 
		}

		// Get the height of the enemy ship
		public int Height
		{
			get { return BossAnimation.FrameHeight; } 
		}

		public int Damage
		{
			get { return damage; }
			set { damage = value; }
		}

		public int DamageMin
		{
			get { return damageMin; }
			set { damageMin = value; }
		}
		// The speed at which the enemy moves
		public float BossMoveSpeed
		{
			get { return bossMoveSpeed; }
			set { bossMoveSpeed = value; }
		}

		public int Worth
		{
			get { return worth; }
			set { worth = value; }
		}

		public void Initialize(Animation animation,Vector2 position)
		{
			// Load the enemy ship texture
			this.BossAnimation = animation;

			// Set the position of the enemy
			this.Position = position;

			// We initialize the enemy to be active so it will be update in the game
			this.Active = true;


			// Set the health of the enemy
			this.Health = 500;

			// Set the amount of damage the enemy can do
			this.Damage = 75;

			this.DamageMin = 50;

			// Set how fast the enemy moves
			this.bossMoveSpeed = 1.0f;


			// Set the score value of the enemy
			this.Worth = 1000;

		}

		public void Update(GameTime gameTime)
		{ 
			// The enemy always moves to the left so decrement it's xposition
			Position.X -= bossMoveSpeed;

			// Update the position of the Animation
			BossAnimation.Position = Position;

			// Update Animation
			BossAnimation.Update(gameTime);

			// If the enemy is past the screen or its health reaches 0 then deactivateit
			if (Position.X < -Width || Health <= 0)
			{
				// By setting the Active flag to false, the game will remove this objet fromthe
				// active game list
				Active = false;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			// Draw the animation
			BossAnimation.Draw(spriteBatch);
		}

		public Boss ()
		{

		}
	}
}


