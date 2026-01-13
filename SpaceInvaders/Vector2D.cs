using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Vector2D
    {
        public double x { get; set; }
        public double y { get; set; }
        public Vector2D(double X, double Y)
        {
            x = X;
            y = Y;
        }
        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x + b.x, a.y + b.y);
        }

        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x - b.x, a.y - b.y);
        }

        public static Vector2D operator -(Vector2D v)
        {
            return new Vector2D(-v.x, -v.y);
        }

        public static Vector2D operator *(Vector2D v, double scalar)
        {
            return new Vector2D(v.x * scalar, v.y * scalar);
        }

        public static Vector2D operator *(double scalar, Vector2D v)
        {
            return new Vector2D(v.x * scalar, v.y * scalar);
        }

        public static Vector2D operator /(Vector2D v, double scalar)
        {
            return new Vector2D(v.x / scalar, v.y / scalar);
        }
    }
}

