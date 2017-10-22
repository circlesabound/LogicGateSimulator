using Assets.Scripts.ScratchPad;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Util
{
    public static class CameraAdjust
    {
        private const float TimeDuration = 0.5F;
        private static float StartZoom;
        private static float TargetZoom = 5F; // surely there's a better way to do this
        private static float CurrentTime;

        public static float CurrentZoom
        {
            get
            {
                return Camera.main.orthographicSize;
            }
            private set
            {
                Camera.main.orthographicSize = value;
            }
        }

        public static void Clamp()
        {
            var canvas = GameObject.FindObjectOfType<SPCanvas>();
            Assert.IsNotNull(canvas);
            var xscale = canvas.gameObject.transform.localScale.x;
            var yscale = canvas.gameObject.transform.localScale.y;

            // Limit zoom to a sensible amount
            CurrentZoom = Mathf.Clamp(CurrentZoom, 0, yscale / 20 + Mathf.Sqrt(yscale / 20));

            // Limit panning to the edge of the canvas
            Camera.main.transform.position = new Vector3
            {
                x = Mathf.Clamp(Camera.main.transform.position.x, -xscale / 2, xscale / 2),
                y = Mathf.Clamp(Camera.main.transform.position.y, -yscale / 2, yscale / 2),
                z = Camera.main.transform.position.z
            };
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
