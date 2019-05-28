using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// Utilities, Math and Extensions.

namespace imageDeCap
{
    public static class Utilities
    {
        public static bool IsWindows10()
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            string productName = (string)reg.GetValue("ProductName");
            return productName.StartsWith("Windows 10");
        }

        public static void playSound(string soundName)
        {
            if (!imageDeCap.Preferences.DisableSoundEffects)
            {
                SoundPlayer sp = null;
                string soundPath = "imageDeCap.Sounds." + soundName;
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream soundResource = assembly.GetManifestResourceStream(soundPath);
                sp = new SoundPlayer(soundResource);
                
                if(sp != null)
                {
                    sp.Play();
                }
            }
        }

        // Utility function for displaying bubble notifications.
        // The most recent call to this gets to choose what function is called when you click it.
        public delegate void Target(object o, EventArgs e);
        static EventHandler CurrentTarget;
        public static void BubbleNotification(string Text, Target OnClickFunction = null, System.Windows.Forms.ToolTipIcon Icon = System.Windows.Forms.ToolTipIcon.Info, string Title = "ImageDeCap", int Timeout = 500)
        {
            Program.ImageDeCap.BubbleNotification.ShowBalloonTip(Timeout, Title, Text, Icon);
            if(CurrentTarget != null)
            {
                Program.ImageDeCap.BubbleNotification.BalloonTipClicked -= CurrentTarget;
            }
            if(OnClickFunction != null)
            {
                CurrentTarget = new EventHandler(OnClickFunction);
                Program.ImageDeCap.BubbleNotification.BalloonTipClicked += CurrentTarget;
            }
        }
    }

    // Little vector class because Points drive me insane
    public struct Vector2
    {
        // Members
        public float X;
        public float Y;

        // Ctor
        public Vector2(Vector2 vector)
        {
            this.X = vector.X;
            this.Y = vector.Y;
        }
        public Vector2(Point vector)
        {
            this.X = vector.X;
            this.Y = vector.Y;
        }
        public Vector2(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        // Utility
        public Point ToPoint()
        {
            return new Point((int)X, (int)Y);
        }
        // Math
        public static float Magnitude(Vector2 P1)
        {
            return (float)Math.Sqrt(P1.X * P1.X + P1.Y * P1.Y);
        }
        public static float Distance(Vector2 P1, Vector2 P2)
        {
            return Vector2.Magnitude(P2 - P1);
        }
        public static Vector2 Normalize(Vector2 P1)
        {
            float magn = Magnitude(P1);
            if (magn == 0)
            {
                return new Vector2(0, 0);
            }
            return P1 / magn;
        }
        public static Vector2 MoveTowards(Vector2 P1, Vector2 P2, float MaxDistance)
        {
            Vector2 DeltaVector = P2 - P1;
            float Distance = Magnitude(DeltaVector);
            Distance = Math.Min(Distance, MaxDistance);
            return P1 + Normalize(DeltaVector) * Distance;
        }
        public static Vector2 OrthagonalLeft(Vector2 P1)
        {
            return new Vector2(-P1.Y, P1.X);
        }
        public static Vector2 OrthagonalRight(Vector2 P1)
        {
            return new Vector2(P1.Y, -P1.X);
        }
        public static Vector2 fromAtoB(Vector2 A, Vector2 B)
        {
            return B - A;
        }
        public static Vector2 Lerp(Vector2 A, Vector2 B, float t)
        {
            return (A + (B - A) * t);
        }

        // Operators
        public static Vector2 operator +(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }
        public static Vector2 operator -(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.X - vector2.X, vector1.Y - vector2.Y);
        }
        public static Vector2 operator *(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.X * vector2.X, vector1.Y * vector2.Y);
        }
        public static Vector2 operator /(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.X / vector2.X, vector1.Y / vector2.Y);
        }

        public static Vector2 operator +(Vector2 vector1, float vector2)
        {
            return new Vector2(vector1.X + vector2, vector1.Y + vector2);
        }
        public static Vector2 operator -(Vector2 vector1, float vector2)
        {
            return new Vector2(vector1.X - vector2, vector1.Y - vector2);
        }
        public static Vector2 operator *(Vector2 vector1, float vector2)
        {
            return new Vector2(vector1.X * vector2, vector1.Y * vector2);
        }
        public static Vector2 operator /(Vector2 vector1, float vector2)
        {
            return new Vector2(vector1.X / vector2, vector1.Y / vector2);
        }
    }
}
