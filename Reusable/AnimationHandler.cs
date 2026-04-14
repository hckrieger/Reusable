using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Reusable
{


	public class AnimationHandler
	{
		private Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
		public Animation? CurrentAnimation { get; private set; }	
		private bool resetAnimation = false;

		private Animation? previousAnimation;

		private AnimationType currentAnimationType;

		public Rectangle AnimationSourceRectangle
		{
			get
			{
				return new Rectangle(CurrentAnimation.Index * CurrentAnimation.FrameSize.X, 0, CurrentAnimation.FrameSize.X, CurrentAnimation.FrameSize.Y);
			}
		}

		public void InitializeAnimation(string name, Animation animation)
		{
			animations[name] = animation;	
		}

		public void SetAnimation(string name)
		{
			previousAnimation = CurrentAnimation;
			if (animations.ContainsKey(name))
			{
				CurrentAnimation = animations[name];
				return;
			}

			resetAnimation = true;

			throw new InvalidOperationException();	
		}
	
		public void Update(GameTime gameTime)
		{
			if (CurrentAnimation == null)
				return;

			if (resetAnimation && CurrentAnimation.Type != AnimationType.Once)
			{
				CurrentAnimation.Index = 0;
				resetAnimation = false;	
			}

			CurrentAnimation.CurrentTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;


			if (CurrentAnimation.CurrentTime < 0)
			{


				CurrentAnimation.CurrentTime = CurrentAnimation.Duration;

				if (CurrentAnimation.Index  == CurrentAnimation.FrameIndices.Length)
				{
					CurrentAnimation.Index = 0;
					if (CurrentAnimation.Type == AnimationType.Once)
					{
						//CurrentAnimation = previousAnimation;
						CurrentAnimation?.EndOfAnimationAction.Invoke();
					}

					
				}
				else if (CurrentAnimation.FrameIndices.Length > 1)
				{
					CurrentAnimation.Index++;
				}


			}

		}
	}
}
