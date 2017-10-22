using Assets.Scripts.ScratchPad;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Util
{
    public static class CameraAdjust
    {
        private static Transform CameraTransform
        {
            get
            {
                return Camera.main.transform;
            }
        }

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
            CurrentZoom = Mathf.Clamp(CurrentZoom, 0, yscale / 2 + Mathf.Sqrt(yscale / 2));

            // Limit panning to the edge of the canvas
            CameraTransform.position = new Vector3
            {
                x = Mathf.Clamp(CameraTransform.position.x, -xscale / 2, xscale / 2),
                y = Mathf.Clamp(CameraTransform.position.y, -yscale / 2, yscale / 2),
                z = CameraTransform.position.z
            };
        }

        public static void Pan(Vector2 delta)
        {
            CameraTransform.position += (Vector3)delta;
        }

        public static void Zoom(float zoomLevelDelta, Vector2 anchor)
        {
            // The spec for zooming with an anchor is that the position of the anchor in
            // screen coordinates before the zoom must be equal to the position of the
            // anchor in screen coordinates after the zoom.

            // Here's the math:
            // 1. Save the position of the anchor in screen coordinates.
            // 2. Do a regular zoom anchored on the origin.
            // 3. Convert the saved screen coordinates back to world coordinates,
            //    using the modified camera coordinate frame.
            // 4. Calculate the difference between the anchor and the converted
            //    coordinates.
            // 5. Shift the camera by this difference.

            Vector2 originalPosition = Camera.main.WorldToScreenPoint(anchor);
            CurrentZoom = Mathf.Pow(Mathf.Sqrt(CurrentZoom) - Mathf.Sqrt(Math.Abs(zoomLevelDelta)) * Math.Sign(zoomLevelDelta), 2);
            Vector2 convertedPosition = Camera.main.ScreenToWorldPoint(originalPosition);
            Vector2 delta = anchor - convertedPosition;
            CameraTransform.position += (Vector3)delta;
        }
    }
}
