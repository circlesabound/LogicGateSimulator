using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class Util
    {
        /// <summary>
        /// The position of the mouse cursor in world coordinates.
        /// Uses the main camera.
        /// </summary>
        public static Vector2 MouseWorldCoordinates
        {
            get
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                return new Vector2(pos.x, pos.y);
            }
        }
    }
}
