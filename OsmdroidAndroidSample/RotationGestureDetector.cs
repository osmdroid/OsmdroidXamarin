using System;
using Android.Views;

namespace OsmdroidAndroidSample
{
    public class RotationGestureDetector
    {
        public interface IRotationListener
        {
            void OnRotate(float deltaAngle);
        }

        protected float Rotation;
        private readonly IRotationListener _listener;

        public RotationGestureDetector(IRotationListener listener)
        {
            _listener = listener;
        }

        private static float CalculateRotation(MotionEvent motionEvent)
        {
            double deltaX = (motionEvent.GetX(0) - motionEvent.GetX(1));
            double deltaY = (motionEvent.GetY(0) - motionEvent.GetY(1));
            var radians = Math.Atan2(deltaY, deltaX);
            return (float) ToDegrees(radians);
        }

        private static double ToDegrees(double angrad)
        {
            return angrad*180d/Math.PI;
        }

        public void OnTouch(MotionEvent e)
        {
            if (e.PointerCount != 2)
                return;

            if (e.ActionMasked == MotionEventActions.PointerDown)
            {
                Rotation = CalculateRotation(e);
            }

            var rotation = CalculateRotation(e);
            var delta = rotation - Rotation;
            Rotation += delta;
            _listener.OnRotate(delta);
        }

    }
}