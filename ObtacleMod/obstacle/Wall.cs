﻿using System;
using System.Drawing;
using BaseLibrary;
using BaseLibrary.battlefield;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;
using BaseLibrary.utils.euclidianSpaceStruct;
using Point = BaseLibrary.utils.euclidianSpaceStruct.Point;
using PointF = System.Drawing.PointF;

namespace ObstacleMod.obstacle {
    /// <summary>
    /// Wall represent wall on map. It is not possible to shoot through it neither move through it.
    /// </summary>
    [ModDescription()]
    public class Wall : IMoveInfluence, IShotInfluence {

        /// <inheritdoc />
        public bool Standable => false;

        public const string COMMAND_NAME = "WALL";
        private static readonly AObstacleFactory FACTORY = new ObstacleFactory();
        private class ObstacleFactory : AObstacleFactory {
            internal ObstacleFactory() {}
            public override bool IsDeserializeable(string s) {
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, COMMAND_NAME, out rest)) {
                    int x, y;
                    if (rest.Length == 2 && int.TryParse(rest[0], out x) && int.TryParse(rest[1], out y)) {
                        cache.Cached(s, new Wall(x,y));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(IObstacle c) {
                Wall c2 = c as Wall;
                if (c2 != null) {
                    cache.Cached(c, c2);
                    return true;
                }
                return false;
            }

            public override bool IsSerializable(IObstacle c) {
                Wall c2 = c as Wall;
                if (c2 != null) {
                    string serialized = ProtocolV1_0Utils.SerializeParams(COMMAND_NAME, c2.Y, c2.X);
                    cacheForSerialize.Cached(c, serialized);
                    return true;
                }
                return false;
            }
        }

        private static readonly Cache<PointF, Image> CACHE = new Cache<PointF, Image>(true);
        static Wall() {
            ObstaclesAroundRobot.OBSTACLE_FACTORIES.RegisterCommand(FACTORY);
        }

        public string TypeName => this.GetType().ToString();

        public int X { get; }
        public int Y { get; }
        public bool Used { get; set; }

        public Wall(int x, int y) {
            X = x;
            Y = y;
        }

        /// <inheritdoc />
        void IMoveInfluence.Change(Robot robot, int turn, ref double fromX, ref double fromY, ref double toX, ref double toY) {
            robot.HitPoints -= (int) Math.Max(1, Math.Round(5 * robot.Power * robot.Motor.MAX_SPEED / 100));
            robot.Power = 0;
            toY = robot.Y;
            toX = robot.X;
            fromX = robot.X;
            fromY = robot.Y;
        }

        /// <inheritdoc />
        bool IShotInfluence.Change(int turn, double fromX, double fromY, ref double toX, ref double toY) {
            Segment shotSegment = new Segment(fromX, fromY, toX, toY);

            Point intersect = EuclideanSpaceUtils.GetNearestIntersect(shotSegment, this.Segments());
            toY = intersect.Y;
            toX = intersect.X;

            return true;
        }

        public void Draw(Graphics graphics, float xScale, float yScale) {
            PointF leftUpCorner = new PointF(X * xScale, Y * yScale);
            PointF scale = new PointF(xScale, yScale);
            Image icon;
            if (!CACHE.TryGetCached(scale, out icon)) {
                icon = drawIcon();
                icon = new Bitmap(icon, new Size((int)xScale, (int)yScale));
                CACHE.Cached(scale, icon);
            }
            graphics.DrawImage(icon, leftUpCorner);
        }

        private Image drawIcon() {
            Image icon = new Bitmap(11, 11);
            Graphics graphics = Graphics.FromImage(icon);
            graphics.FillRectangle(new SolidBrush(Color.Red), 0, 0, icon.Width, icon.Height);
            Pen black = new Pen(Color.Black, 1);

            int x = 0;

            for (int y = 0; y < icon.Height; y += 3) {
                graphics.DrawLine(black, 0, y, icon.Width, y);
                for (x = x % icon.Width; x < icon.Width; x += 4) {
                    graphics.DrawLine(black, x, y + 0, x, y + 3);
                }
            }
            return icon;
        }
    }
}
