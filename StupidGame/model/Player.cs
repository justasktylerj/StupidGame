using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using SampleGame.Model;
using SampleGame.View;

namespace SampleGame.Model
{
	public class Player 
	{
		public int score;
		public bool active;
		public int playerDamage;
		public int playerDamageMin;
		public int health;
		public int healthMax;
		public float playerMoveSpeed;

		public Animation playerRun;
		public Animation playerAttack;
		public Animation playerIdle;
		public Animation playerBlast;
		public Animation playerHeal;

		public Animation playerAnimation;

		// Animation representing the player
		public Animation PlayerAnimation
		{
			get { return playerAnimation; }
			set { playerAnimation = value; }
		}

		public Texture2D playerTexture;
		// Position of the Player relative to the upper left side of the screen
		public Vector2 Position;

		// State of the player
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		// Amount of hit points that player has
		public int PlayerDamage
		{
			get { return playerDamage; }
			set { playerDamage = value; }
		}

		public int PlayerDamageMin
		{
			get { return playerDamageMin; }
			set { playerDamageMin = value; }
		}


		public int Health
		{
			get { return health; }
			set { health = value; }
		}

		public int HealthMax
		{
			get { return healthMax; }
			set { healthMax = value; }
		}

		 //Get the width of the player ship
		public int Width
		{
			get { return PlayerAnimation.FrameWidth; }
		}

		// Get the height of the player ship
		public int Height 
		{
			get { return PlayerAnimation.FrameHeight; }

		}
		public int Score 
		{
			get { return score; }
			set { score = value; }
		}

		public float PlayerMoveSpeed
		{
			get { return playerMoveSpeed; }
			set { playerMoveSpeed = value; }
		}

		// Initialize the player
		public void Initialize(Animation animation, Vector2 position)
		{
			this.playerAnimation = playerIdle;

			// Set the starting position of the player around the middle of the screen and to the back
			this.Position = position;

			// Set the player to be active
			this.Active = true;

			this.PlayerDamageMin = 40;

			this.PlayerDamage = 50;
			// Set the player health
			this.HealthMax = 100;

			this.Health = HealthMax;

			this.score = 0;

			this.playerMoveSpeed = 2.0f;

			playerRun = new Animation();
			playerAttack = new Animation ();
			playerIdle = new Animation ();
			playerBlast = new Animation ();
			playerHeal = new Animation ();

			playerRun.Initialize(playerTexture, Vector2.Zero, 50, 60, 0, 6, 50, Color.White, 1f, true);
			playerAttack.Initialize(playerTexture, Vector2.Zero, 50, 60, 7, 18, 100, Color.White, 1f, true);
			playerIdle.Initialize(playerTexture, Vector2.Zero, 50, 60, 30, 31, 50, Color.White, 1f, true);
			playerBlast.Initialize(playerTexture, Vector2.Zero, 50, 60, 32, 42, 50, Color.White, 1f, true);
			playerHeal.Initialize(playerTexture, Vector2.Zero, 50, 60, 20, 30, 50, Color.White, 1f, true);

		}

		// Update the player animation
		public void Update(GameTime gameTime)
		{
			PlayerAnimation.Position = Position;
			PlayerAnimation.Update(gameTime);
		}

		// Draw the player
		public void Draw(SpriteBatch spriteBatch)
		{
			PlayerAnimation.Draw(spriteBatch);
		}


	}
}

