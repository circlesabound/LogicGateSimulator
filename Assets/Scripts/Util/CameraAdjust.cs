using System;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class CameraAdjust
    {
        private const float TimeDuration = 0.5F;
        private static float StartZoom;
        private static float TargetZoom = 5F; // surely there's a better way to do this
        private static float CurrentTime;

        private static float CurrentZoom
        {
            get
            {
                return Camera.main.orthographicSize;
            }
            set
            {
                Camera.main.orthographicSize = value;
            }
        }

        /// <summary>
        /// Non-smooth zooming
        /// </summary>
        /// <param name="zoomLevelDelta"></param>
        public static void SimpleZoom(float zoomLevelDelta)
        {
            CurrentZoom = (float)Math.Pow(Math.Sqrt(CurrentZoom) - Math.Sqrt(Math.Abs(zoomLevelDelta)) * Math.Sign(zoomLevelDelta), 2);
        }

        public static void Pan(Vector2 delta)
        {
            Camera.main.transform.Translate(delta);
        }

        /// <summary>
        /// Smooth zooming
        /// </summary>
        /// <param name="zoomLevelDelta"></param>
        public static void Zoom(float zoomLevelDelta)
        {
            if (zoomLevelDelta != 0)
            {
                // TODO this causes a non-smooth velocity curve, how to fix
                // Reset the timer to an intermediate value if a zoom occurs during another one
                if (CurrentZoom != TargetZoom)
                {
                    CurrentTime = 0F;
                }
                // Set the new start
                StartZoom = CurrentZoom;
                // Modify the target
                TargetZoom = (float)Math.Pow(Math.Sqrt(TargetZoom) - Math.Sqrt(Math.Abs(zoomLevelDelta)) * Math.Sign(zoomLevelDelta), 2);
                if (TargetZoom < 0.1F) TargetZoom = 0.1F;
            }
            if (CurrentZoom != TargetZoom)
            {
                // Calculate parameter t
                float t = CurrentTime / TimeDuration;
                // Apply zoom
                CurrentZoom = Mathf.SmoothStep(StartZoom, TargetZoom, t);
                // Increment the timer
                CurrentTime += Time.deltaTime;
                // If we're done, reset values
                if (CurrentTime > TimeDuration)
                {
                    StartZoom = CurrentZoom;
                    TargetZoom = CurrentZoom;
                    CurrentTime = 0;
                }
            }
        }
    }
}
