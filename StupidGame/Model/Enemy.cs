using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SampleGame.Model;
using SampleGame.View;

namespace SampleGame.Model
{
	public class Enemy 
	{
		// Animation representing the enemy
		public Texture2D enemyTexture;

		public Texture2D EnemyTexture;

		public Animation enemyAnimation;

		public Animation EnemyAnimation
		{
			get { return enemyAnimation; }
			set { enemyAnimation = value; }
		}
		// Position of the Player relative to the upper left side of the screen
		public Vector2 position;

		public Vector2 Position;

		public bool active;

		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		// Amount of hit points that player has
		public int health;

		public int Health
		{
			get { return health; }
			set { health = value; }
		}

		// Get the width of the player ship

		public int Width
		{
			get { return EnemyAnimation.FrameWidth; } 
		}

		// Get the height of the enemy ship
		public int Height
		{
			get { return EnemyAnimation.FrameHeight; } 
		}

		public int damage;

		public int Damage
		{
			get { return damage; }
			set { damage = value; }
		}

		public int damageMin;

		public int DamageMin
		{
			get { return damageMin; }
			set { damageMin = value; }
		}
		// The speed at which the enemy moves
		public float enemyMoveSpeed;

		public float EnemyMoveSpeed
		{
			get { return enemyMoveSpeed; }
			set { enemyMoveSpeed = value; }
		}

		public int worth;

		public int Worth
		{
			get { return worth; }
			set { worth = value; }
		}

		public void Initialize(Animation enemyAnimation, Vector2 position,
			bool active, int health, int damageMin, int damage,
			float enemyMoveSpeed, int worth)
		{
			// Keep a local copy of the values passed in
			this.enemyAnimation = enemyAnimation;
			this.position = position;
			this.active = true;
			this.health = 20;
			this.damageMin = 5;
			this.damage = 10;
			this.enemyMoveSpeed = 1.0f; // = enemyMoveSpeed
			this.worth = 20; // = worth
		}



		public void Update(GameTime gameTime)
		{ 
			// The enemy always moves to the left so decrement it's xposition
			Position.X -= enemyMoveSpeed;

			// Update the position of the Animation
			EnemyAnimation.Position = Position;

			// Update Animation
			EnemyAnimation.Update(gameTime);

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
			EnemyAnimation.Draw(spriteBatch);
		}
			
	}
}



