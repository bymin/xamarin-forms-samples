using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace PinchGesture
{
    public class PinchToZoomContainer : ContentView
    {
        //double currentScale = 1;
        //double startScale = 1;
        //double xOffset = 0;
        //double yOffset = 0;

        //      double StartX = 0;
        //      double StartY = 0;

        //      double x = 0;
        //      double y = 0;

        public PinchToZoomContainer()
        {
            //var pinchGesture = new PinchGestureRecognizer ();
            //pinchGesture.PinchUpdated += OnPinchUpdated;
            //GestureRecognizers.Add (pinchGesture);

            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(panGesture);

            var pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += OnPinchUpdated;
            GestureRecognizers.Add(pinchGesture);

        }


        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            //Content = this;
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    StartX = Content.TranslationX;
                    StartY = Content.TranslationY;

                    break;
                case GestureStatus.Running:
                    Content.TranslationX = StartX + e.TotalX;
                    Content.TranslationY = StartY + e.TotalY;
                    break;

                case GestureStatus.Completed:
                    break;
            }
        }


        //private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        //{
        //    switch (e.StatusType)
        //    {
        //        case GestureStatus.Started:
        //            StartX = (1 - AnchorX) * Width;
        //            StartY = (1 - AnchorY) * Height;
        //            break;

        //        case GestureStatus.Running:
        //            //    AnchorX = Clamp(1 - (StartX + e.TotalX) / Width, 0, 1);
        //            //    AnchorY = Clamp(1 - (StartY + e.TotalY) / Height, 0, 1);
        //            //AnchorX = 1 - (StartX + e.TotalX) / Width;
        //            //AnchorY = 1 - (StartY + e.TotalY) / Height;
        //            Content.TranslationX = (StartX + e.TotalX) ;
        //            Content.TranslationY = (StartY + e.TotalY) ;
        //            Debug.WriteLine($"TranslationX: {TranslationX}");
        //            break;
        //    }
        //}


        void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            Debug.WriteLine($"e.ScaleOrigin:{e.ScaleOrigin}; e.Scale:{e.Scale}; TranslationX: {Content.TranslationX}");

            if (e.Status == GestureStatus.Started)
            {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                startScale = Content.Scale;
                //ImageMain.AnchorX = 0;
                //ImageMain.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calculate the scale factor to be applied.
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);
                currentScale = Math.Min(currentScale, 2.5);
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
                double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

                //Debug.WriteLine($"xOffset:{xOffset}; originX:{originX}; targetX: {targetX}");

                // Apply translation based on the change in origin.
                //Content.TranslationX = targetX.Clamp(-Content.Width * (currentScale - 1), 0);
                //Content.TranslationY = targetY.Clamp(-Content.Height * (currentScale - 1), 0);
                //Content.TranslationX = targetX;
                //Content.TranslationY = targetY;



                // Apply scale factor
                Content.Scale = currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                // Store the translation delta's of the wrapped user interface element.
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
            }
        }

        //private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        //{
        //    switch (e.Status)
        //    {
        //        case GestureStatus.Started:
        //            StartScale = Scale;
        //            AnchorX = e.ScaleOrigin.X;
        //            AnchorY = e.ScaleOrigin.Y;
        //            break;

        //        case GestureStatus.Running:
        //            double current = Scale + (e.Scale - 1) * StartScale;
        //            //Scale = Clamp(current, MIN_SCALE * (1 - OVERSHOOT), MAX_SCALE * (1 + OVERSHOOT));
        //            Scale = current;
        //            break;

        //        case GestureStatus.Completed:
        //            //if (Scale > MAX_SCALE)
        //            //    this.ScaleTo(MAX_SCALE, 250, Easing.SpringOut);
        //            //else if (Scale < MIN_SCALE)
        //            //    this.ScaleTo(MIN_SCALE, 250, Easing.SpringOut);
        //            break;
        //    }
        //}

        private const double MIN_SCALE = 1;
        private const double MAX_SCALE = 8;
        private const double OVERSHOOT = 0.15;
        private double StartX, StartY;
        private double xOffset, yOffset;
        private double x, y;
        private double startScale;
        double currentScale = 1;

        private static double Clamp(double self, double min, double max)
        {
            //return Math.Min (max, Math.Max (self, min));
            return Math.Max(self, min);
        }
    }
}
