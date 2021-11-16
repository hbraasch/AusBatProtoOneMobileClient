﻿using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PinchGesture
{
	public class PinchToZoomContainer : ContentView
	{
		private const double MIN_SCALE = 1;
		private const double MAX_SCALE = 8;
		double currentScale = 1;
		double startScale = 1;
		double xOffset = 0;
		double yOffset = 0;
		double x, y;
		double startX, startY;
		double width, height;

		public PinchToZoomContainer ()
		{
			var pinchGesture = new PinchGestureRecognizer ();
			pinchGesture.PinchUpdated += OnPinchUpdated;
			GestureRecognizers.Add (pinchGesture);

			var panGesture = new PanGestureRecognizer();
			panGesture.PanUpdated += OnPanUpdated;
			GestureRecognizers.Add(panGesture);

			var doubletapGesture = new TapGestureRecognizer();
			doubletapGesture.NumberOfTapsRequired = 2;
			doubletapGesture.Tapped += OnTapped;
			GestureRecognizers.Add(doubletapGesture);
		}


		void OnPinchUpdated (object sender, PinchGestureUpdatedEventArgs e)
		{
			if (e.Status == GestureStatus.Started)
			{
				// Store the current scale factor applied to the wrapped user interface element,


				// and zero the components for the center point of the translate transform.
				startScale = Content.Scale;
				Content.AnchorX = 0;
				Content.AnchorY = 0;
			}

			if (e.Status == GestureStatus.Running)
			{
				// Calculate the scale factor to be applied.
				currentScale += (e.Scale - 1) * startScale;
				currentScale = Math.Max(1, currentScale);

				// The ScaleOrigin is in relative coordinates to the wrapped user interface element,
				// so get the X pixel coordinate.
				double renderedX = Content.X + xOffset;
				double deltaX = renderedX / Width;
				double deltaWidth = Width / (Content.Width * startScale);
				double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

				// The ScaleOrigin is in relative coordinates to the wrapped user interface element,
				// so get the Y pixel coordinate.
				double renderedY = Content.Y + yOffset;
				double deltaY = renderedY / Height;
				double deltaHeight = Height / (Content.Height * startScale);
				double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

				// Calculate the transformed element pixel coordinates.
				double targetX = xOffset - (originX * Content.Width) * (currentScale -
				startScale);
				double targetY = yOffset - (originY * Content.Height) * (currentScale -
				startScale);

				// Apply translation based on the change in origin.
				Content.TranslationX = targetX.Clamp(-Content.Width* (currentScale - 1), 0);
				Content.TranslationY = targetY.Clamp(-Content.Height* (currentScale - 1), 0);

				// Apply scale factor.
				Content.Scale = currentScale;
				width = Content.Width* currentScale;
				height = Content.Height* currentScale;
			}
			if (e.Status == GestureStatus.Completed)
			{
				// Store the translation delta's of the wrapped user interface element.
				xOffset = Content.TranslationX;
				yOffset = Content.TranslationY;
				x = Content.TranslationX;
				y = Content.TranslationY;
			}

		}

		void OnPanUpdated(object sender, PanUpdatedEventArgs e)
		{
			if (!width.Equals(Content.Width) && !height.Equals(Content.Height))
			{
				switch (e.StatusType)
				{
					case GestureStatus.Started:
						startX = Content.TranslationX;
						startY = Content.TranslationY;
						break;
					case GestureStatus.Running:
						if (!width.Equals(0))
						{
							Content.TranslationX = Math.Max(Math.Min(0, x + e.TotalX), -Math.Abs(Content.Width - width));// App.ScreenWidth));
						}
						if (!height.Equals(0))
						{
							Content.TranslationY = Math.Max(Math.Min(0, y + e.TotalY), -Math.Abs(Content.Height - height)); //App.ScreenHeight));    
						}
						break;
					case GestureStatus.Completed:
						// Store the translation applied during the pan
						x = Content.TranslationX;
						y = Content.TranslationY;
						xOffset = Content.TranslationX;
						yOffset = Content.TranslationY;
						break;
				}
			}
		}

		private void OnTapped(object sender, EventArgs e)
		{
			
		}
	}
}
