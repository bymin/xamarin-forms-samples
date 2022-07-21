using System.Diagnostics;
using Xamarin.Forms;

namespace PinchGesture
{
    public class PinchToZoomContainer : ContentView
    {
        public PinchToZoomContainer()
        {
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
                    _offsetX = Content.TranslationX;
                    _offsetY = Content.TranslationY;

                    break;
                case GestureStatus.Running:
                    Content.TranslationX = _offsetX + e.TotalX;
                    Content.TranslationY = _offsetY + e.TotalY;
                    Debug.WriteLine($"TranslationY: {Content.TranslationY};");
                    break;

                case GestureStatus.Completed:
                    break;
            }
        }


        void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                _startScale = Content.Scale;
                _offsetX = Content.TranslationX;
                _offsetY = Content.TranslationY;

                // for 2-fingers panning
                _originX = e.ScaleOrigin.X;
                _originY = e.ScaleOrigin.Y;
            }
            if (e.Status == GestureStatus.Running)
            {
                _currentScale += (e.Scale - 1) * _startScale;

                var oldCenterX = 0.5 * Width + _offsetX;
                var pivotX = e.ScaleOrigin.X * Width;
                var dx = pivotX + (oldCenterX - pivotX) * _currentScale / _startScale - oldCenterX;
                dx += (e.ScaleOrigin.X - _originX) * Width;
                Content.TranslationX = _offsetX + dx;
                //Debug.WriteLine($"offsetX: {_offsetX}; oldCenterX: {oldCenterX}; pivotX: {pivotX}");

                var oldCenterY = 0.5 * Height + _offsetY;
                var pivotY = e.ScaleOrigin.Y * Height;
                var dy = pivotY + (oldCenterY - pivotY) * _currentScale / _startScale - oldCenterY;
                dy += (e.ScaleOrigin.Y - _originY) * Height;
                Content.TranslationY = _offsetY + dy;

                //  potential limit the scale.
                //double current = Scale + (e.Scale - 1) * StartScale;
                //Scale = Clamp(current, MIN_SCALE * (1 - OVERSHOOT), MAX_SCALE * (1 + OVERSHOOT));

                // Apply scale factor
                Content.Scale = _currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                // potential animation 
                //if (Scale > MAX_SCALE)
                //    this.ScaleTo(MAX_SCALE, 250, Easing.SpringOut);
                //else if (Scale < MIN_SCALE)
                //    this.ScaleTo(MIN_SCALE, 250, Easing.SpringOut);
            }
        }

        //private const double MIN_SCALE = 1;
        //private const double MAX_SCALE = 8;
        //private const double OVERSHOOT = 0.15;
        private double _offsetX, _offsetY;
        private double _startScale;
        double _currentScale = 1;
        double _originX, _originY;
    }
}
