﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using SampleGame.Model;
using SampleGame.View;

namespace SampleGame.Controller
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class StupidGame : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		private Player player;

		public int level;

		public Boolean vulerable;

		//public Animation currentAnimation;
		// Keyboard states used to determine key presses
		private KeyboardState currentKeyboardState;
		private KeyboardState previousKeyboardState;

		// Gamepad states used to determine button presses
		private GamePadState currentGamePadState;
		private GamePadState previousGamePadState; 

		// A movement speed for the player
		private float playerMoveSpeed;

		// Image used to display the static background
		private Texture2D mainBackground;

		// Parallaxing Layers
		private ParallaxingBackground bgLayer1;
		private ParallaxingBackground bgLayer2;

		// Enemies
		private Texture2D enemyTexture;

		private List<Enemy> enemies;

		// The rate at which the enemies appear
		TimeSpan enemySpawnTime;
		TimeSpan previousSpawnTime;

		// A random number generator
		Random random;

		Texture2D projectileTexture;
		List<Projectile> projectiles;

		// The rate of fire of the player laser
		TimeSpan fireTime;
		TimeSpan previousFireTime;

		Texture2D explosionTexture;
		List<Animation> explosions;

		// The sound that is played when a laser is fired
		SoundEffect laserSound;

		// The sound used when the player or an enemy dies
		SoundEffect explosionSound;

		// The music played during gameplay
		Song gameplayMusic;

		//Number that holds the player score
		int score;
		// The font used to display UI elements
		SpriteFont font;

		public StupidGame ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here

			player = new Player ();

			playerMoveSpeed = 2.0f;

			bgLayer1 = new ParallaxingBackground();
			bgLayer2 = new ParallaxingBackground();

			// Initialize the enemies list
			enemies = new List<Enemy> ();
			// Set the time keepers to zero
			previousSpawnTime = TimeSpan.Zero;

			// Used to determine how fast enemy respawns
			enemySpawnTime = TimeSpan.FromSeconds(1.0f);

			// Initialize our random number generator
			random = new Random();

			base.Initialize ();

			projectiles = new List<Projectile>();

			// Set the laser to fire every quarter second
			fireTime = TimeSpan.FromSeconds(.25f);

			explosions = new List<Animation>();

			//Set player's score to zero
			score = 0;

			level = 1;

			vulerable = true;

		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);

			Texture2D playerTexture = Content.Load<Texture2D>("Animation/Imported Paladin");

			Vector2 playerPosition = new Vector2 (GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y
				+ GraphicsDevice.Viewport.TitleSafeArea.Height / 2);

			player.Initialize(player.playerIdle, playerPosition);

			// Load the parallaxing background
			bgLayer1.Initialize(Content, "Texture/bgLayer1", GraphicsDevice.Viewport.Width, -1);
			bgLayer2.Initialize(Content, "Texture/bgLayer2", GraphicsDevice.Viewport.Width, -2);

			enemyTexture = Content.Load<Texture2D>("Animation/imported goblin clone");

			projectileTexture = Content.Load<Texture2D>("Texture/laser");

			explosionTexture = Content.Load<Texture2D>("Animation/dead");

			// Load the music
			gameplayMusic = Content.Load<Song>("Sound/Kingdom Rush Soundtrack - Forest Battle (PC&app)");

			// Load the laser and explosion sound effect
			laserSound = Content.Load<SoundEffect>("Sound/laserFire");
			explosionSound = Content.Load<SoundEffect>("Sound/explosion");

			// Start the music right away
			PlayMusic(gameplayMusic);

			// Load the score font
			font = Content.Load<SpriteFont>("Font/gameFont");

			mainBackground = Content.Load<Texture2D>("Texture/mainBackground");

		}

		private void UpdatePlayer(GameTime gameTime)
		{

			player.Update(gameTime);
			// Get Thumbstick Controls
			player.Position.X += currentGamePadState.ThumbSticks.Left.X *playerMoveSpeed;
			player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y *playerMoveSpeed;

			// Use the Keyboard / Dpad
			if (currentKeyboardState.IsKeyDown(Keys.Left) ||
				currentGamePadState.DPad.Left == ButtonState.Pressed)
			{
				player.Position.X -= playerMoveSpeed;
				player.playerAnimation = player.playerRun;
			}

			if (currentKeyboardState.IsKeyDown(Keys.Right) ||
				currentGamePadState.DPad.Right == ButtonState.Pressed)
			{
				player.Position.X += playerMoveSpeed;
				player.playerAnimation = player.playerRun;
			}

			if (currentKeyboardState.IsKeyDown(Keys.Up) ||
				currentGamePadState.DPad.Up == ButtonState.Pressed)
			{
				player.Position.Y -= playerMoveSpeed;
				player.playerAnimation = player.playerRun;
			}

			if (currentKeyboardState.IsKeyDown(Keys.Down) ||
				currentGamePadState.DPad.Down == ButtonState.Pressed)
			{
				player.Position.Y += playerMoveSpeed;
				player.playerAnimation = player.playerRun;
			}

			if (currentKeyboardState.IsKeyUp(Keys.Down)
				&& currentKeyboardState.IsKeyUp(Keys.Up)
				&& currentKeyboardState.IsKeyUp(Keys.Left)
				&& currentKeyboardState.IsKeyUp(Keys.Right)
				&& currentKeyboardState.IsKeyUp(Keys.Space)
				&& currentKeyboardState.IsKeyUp(Keys.A)
				&& currentKeyboardState.IsKeyUp(Keys.H))
			{
				player.playerAnimation = player.playerIdle;
			}


			// Make sure that the player does not go out of bounds
			player.Position.X = MathHelper.Clamp(player.Position.X, 0,GraphicsDevice.Viewport.Width - player.Width);
			player.Position.Y = MathHelper.Clamp(player.Position.Y, 0,GraphicsDevice.Viewport.Height - player.Height);

			// Fire only every interval we set as the fireTime
			if (currentKeyboardState.IsKeyDown(Keys.Space))
			{
				if (gameTime.TotalGameTime - previousFireTime > fireTime)
				{
					// Reset our current time
					previousFireTime = gameTime.TotalGameTime;

					player.playerAnimation = player.playerBlast;

					// Add the projectile, but add it to the front and center of the player
					AddProjectile(player.Position + new Vector2(player.Width / 2, 0));

					// Play the laser sound
					laserSound.Play();
				}


			}

			if (currentKeyboardState.IsKeyDown(Keys.W) ||
				currentGamePadState.DPad.Right == ButtonState.Pressed)
			{
				player.playerAnimation = player.playerAttack;
				vulerable = false;
			}
			else
			{
				vulerable = true;
			}

			if (currentKeyboardState.IsKeyDown(Keys.H))
			{
				player.playerAnimation = player.playerHeal;

				player.Health += 50;
				if(player.Health > player.HealthMax)
				{
					player.Health = player.HealthMax;
				}
				score = score / 4;
				
			}

			if (player.Health <= 0)
			{
				player.Health = 100;
				player.HealthMax = 100;
				score = 0;
			}

		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>

		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__ &&  !__TVOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState ().IsKeyDown (Keys.Escape))
				Exit ();
			#endif

			// TODO: Add your update logic here
			// Save the previous state of the keyboard and game pad so we can determinesingle key/button presses
			previousGamePadState = currentGamePadState;
			previousKeyboardState = currentKeyboardState;

			// Read the current state of the keyboard and gamepad and store it
			currentKeyboardState = Keyboard.GetState();
			currentGamePadState = GamePad.GetState(PlayerIndex.One);


			//Update the player
			UpdatePlayer(gameTime);

			UpdateEnemies(gameTime);
	
			// Update the parallaxing background
			bgLayer1.Update();
			bgLayer2.Update();

			// Update the collision
			UpdateCollision();

			UpdateProjectiles();

			UpdateExplosions(gameTime);

			if(score >= 200)
			{
				level = 2;
			}

			base.Update (gameTime);
		}

		private void UpdateCollision()
		{
			// Use the Rectangle's built-in intersect function to 
			// determine if two objects are overlapping
			Rectangle rectangle1;
			Rectangle rectangle2;


			// Only create the rectangle once for the player
			rectangle1 = new Rectangle ((int)player.Position.X,
				(int)player.Position.Y,
				player.Width / 2
				, player.Height / 2);

			// Do the collision between the player and the enemies
			for (int i = 0; i < enemies.Count; i++) 
			{
				rectangle2 = new Rectangle ((int)enemies [i].Position.X,
					(int)enemies [i].Position.Y,
					enemies [i].Width,
					enemies [i].Height);

				// Determine if the two objects collided with each
				// other
				if (rectangle1.Intersects (rectangle2)) 
				{
					// Subtract the health from the player based on
					// the enemy damage
					if (vulerable == true) 
					{
						player.Health -= enemies [i].Damage; //Random.Next(enemies [i].DamageMin, enemies [i].Damage);
					}

					// Since the enemy collided with the player
					// destroy it
					enemies [i].Health -= player.PlayerDamage;
		
					// If the player health is less than zero we died
					if (player.Health <= 0) 
					{
						player.Active = false; 
					}
						
				}
			}
				
			// Projectile vs Enemy Collision
			for (int i = 0; i < projectiles.Count; i++)
			{
				for (int j = 0; j < enemies.Count; j++) 
				{
					// Create the rectangles we need to determine if we collided with each other
					rectangle1 = new Rectangle ((int)projectiles [i].Position.X -
					projectiles [i].Width / 2, (int)projectiles [i].Position.Y -
					projectiles [i].Height / 2, projectiles [i].Width, projectiles [i].Height);
				
					rectangle2 = new Rectangle ((int)enemies [j].Position.X - enemies [j].Width / 2,
						(int)enemies [j].Position.Y - enemies [j].Height / 2,
						enemies [j].Width, enemies [j].Height);

					// Determine if the two objects collided with each other
					if (rectangle1.Intersects (rectangle2)) 
					{
						enemies [j].Health -= projectiles [i].Damage;
						projectiles [i].Active = false;
					}

				}


			}


		}

		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.LavenderBlush);

			//TODO: Add your drawing code here
			// Start drawing
			spriteBatch.Begin();

			// Draw the Player
			spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);

			// Draw the moving background
			bgLayer1.Draw(spriteBatch);
			bgLayer2.Draw(spriteBatch);

			// Draw the Enemies
			for (int i = 0; i < enemies.Count; i++)
			{
				enemies[i].Draw(spriteBatch);
			}

			// Draw the Projectiles
			for (int i = 0; i < projectiles.Count; i++)
			{
				projectiles[i].Draw(spriteBatch);
			}

			// Draw the explosions
			for (int i = 0; i < explosions.Count; i++)
			{
				explosions[i].Draw(spriteBatch);
			}

			// Draw the score
			spriteBatch.DrawString(font, "score: " + score, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y), Color.White);
			// Draw the player health
			spriteBatch.DrawString(font, "health: " + player.Health + "/" + player.HealthMax, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + 30), Color.White);

			player.Draw(spriteBatch);

			// Stop drawing
			spriteBatch.End();

			base.Draw (gameTime);
		}

		private void AddEnemy()
		{ 
			// Create the animation object
			Animation enemyAnimation = new Animation();

			// Initialize the animation with the correct animation information
			enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 35, 30, 0, 9, 50,Color.White, 1f, true);

			// Randomly generate the position of the enemy
			Vector2 position = new Vector2(GraphicsDevice.Viewport.Width +enemyTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height -100));

			// Create an enemy
			Enemy enemy = new Enemy();

			// Initialize the enemy
			enemy.Initialize(enemyAnimation, position, enemy.active, enemy.health, enemy.damageMin, enemy.damage, enemy.enemyMoveSpeed, enemy.worth); 

			// Add the enemy to the active enemies list
			enemies.Add(enemy);
		}

		private void AddProjectile(Vector2 position)
		{
			Projectile projectile = new Projectile(); 
			projectile.Initialize(GraphicsDevice.Viewport, projectileTexture,position); 
			projectiles.Add(projectile);
		}

		private void UpdateProjectiles()
		{
			for (int i = projectiles.Count - 1; i >= 0; i--)
			{
				projectiles [i].Update ();

				if (projectiles[i].Active == false)
				{
					projectiles.RemoveAt(i);
				}

			}
		}

		private void UpdateEnemies(GameTime gameTime)
		{
			// Spawn a new enemy enemy every 1.5 seconds
			if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime) 
			{
				previousSpawnTime = gameTime.TotalGameTime;

				AddEnemy ();
			}

			// Update the Enemies
			for (int i = enemies.Count - 1; i >= 0; i--) 
			{
				enemies[i].Update(gameTime);

				if (enemies[i].Active == false)
				{
					if (enemies[i].Health <= 0)
					{

						if(vulerable == true)
						{
							// Add an explosion


							//Add to the player's score
							score += enemies[i].Worth;

							player.Health += player.Health / 20;
							if (player.Health > player.HealthMax)
							{
								player.Health = player.HealthMax;
							}

						}

						AddExplosion(enemies[i].Position);

					}
					enemies.RemoveAt(i);
				} 
			}

		}
			

		private void AddExplosion(Vector2 position)
		{
			Animation explosion = new Animation();
			explosion.Initialize(explosionTexture,position, 28, 15, 0, 12, 45, Color.White, 1f,false);
			explosions.Add(explosion);
		}

		private void UpdateExplosions(GameTime gameTime)
		{
			for (int i = explosions.Count - 1; i >= 0; i--)
			{
				explosions[i].Update(gameTime);
				if (explosions[i].Active == false)
				{
					explosions.RemoveAt(i);
				}
			}
		}

		private void PlayMusic(Song song)
		{
			// Due to the way the MediaPlayer plays music,
			// we have to catch the exception. Music will play when the game is not tethered
			try
			{
				// Play the music
				MediaPlayer.Play(song);

				// Loop the currently playing song
				MediaPlayer.IsRepeating = true;
			}
			catch { }
		}
	}
}
