﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SampleGame.View
{
	public class Animation
	{
		public Animation ()
		{
			// The image representing the collection of images used for animation
			Texture2D spriteStrip;

			// The scale used to display the sprite strip
			float scale;

			// The time since we last updated the frame
			int elapsedTime;

			// The time we display a frame until the next one
			int frameTime;

			// The number of frames that the animation contains
			int frameCount;

			// The index of the current frame we are displaying
			int currentFrame;

			// The color of the frame we will be displaying
			Color color;

			// The area of the image strip we want to display
			Rectangle sourceRect = new Rectangle();

			// The area where we want to display the image strip in the game
			Rectangle destinationRect = new Rectangle();

			// Width of a given frame
			public int FrameWidth;

			// Height of a given frame
			public int FrameHeight;

			// The state of the Animation
			public bool Active;

			// Determines if the animation will keep playing or deactivate after one run
			public bool Looping;

			// Width of a given frame
			public Vector2 Position;

			public void Initialize(Texture2D texture, Vector2 position,
				int frameWidth, int frameHeight, int frameCount,
				int frametime, Color color, float scale, bool looping)
			{
				// Keep a local copy of the values passed in
				this.color = color;
				this.FrameWidth = frameWidth;
				this.FrameHeight = frameHeight;
				this.frameCount = frameCount;
				this.frameTime = frametime;
				this.scale = scale;

				Looping = looping;
				Position = position;
				spriteStrip = texture;

				// Set the time to zero
				elapsedTime = 0;
				currentFrame = 0;

				// Set the Animation to active by default
				Active = true;
			}
			public void Update()
			{
				
			}
			public void Draw()
			{
				
			}
		}
	}
}

