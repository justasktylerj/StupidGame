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
		public Animation enemyAnimation;
		public Vector2 position;
		public bool active;
		public int health;
		public int damage;
		public int worth;
		public float enemyMoveSpeed;

		public Animation EnemyAnimation;

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
			get { return EnemyAnimation.FrameWidth; } 
		}

		// Get the height of the enemy ship
		public int Height
		{
			get { return EnemyAnimation.FrameHeight; } 
		}

		public int Damage
		{
			get { return damage; }
			set { damage = value; }
		}

		// The speed at which the enemy moves
		public float EnemyMoveSpeed
		{
			get { return enemyMoveSpeed; }
			set { enemyMoveSpeed = value; }
		}

		public int Worth
		{
			get { return worth; }
			set { worth = value; }
		}

		public void Initialize(Animation animation,Vector2 position)
		{
			// Load the enemy ship texture
			EnemyAnimation = animation;

			// Set the position of the enemy
			Position = position;

			// We initialize the enemy to be active so it will be update in the game
			Active = true;


			// Set the health of the enemy
			Health = 10;

			// Set the amount of damage the enemy can do
			Damage = 10;

			// Set how fast the enemy moves
			enemyMoveSpeed = 3.0f;


			// Set the score value of the enemy
			Worth = 100;

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

		public Enemy ()
		{

		}
	}
}



