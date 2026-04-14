using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reusable
{
	public enum AnimationType
	{
		Repeat,
		Once,
		PingPong
	}

	public class Animation
	{


		public AnimationType Type;
		public string TextureName;
		public int[] FrameIndices;
		public int Frames;
		public int Index = 0;
		public float Duration;
		public float CurrentTime;
		public Point FrameSize;
		public Action EndOfAnimationAction;

		public Animation(string textureName, Point frameSize, float duration, int[] frameIndices, AnimationType animationType = AnimationType.Repeat, Action? endOfAnimationAction = null)
		{
			TextureName = textureName;
			FrameIndices = frameIndices;
			FrameSize = frameSize;
			Duration = duration;
			Type = animationType;
			EndOfAnimationAction = (endOfAnimationAction != null) ? endOfAnimationAction : () => { };
		}
	}
}
