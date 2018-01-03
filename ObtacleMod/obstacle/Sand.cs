﻿using System;
using System.Drawing;
using BaseLibrary;
using BaseLibrary.battlefield;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace ObstacleMod.obstacle {
    [ModDescription()]
    public class Sand : IMoveInfluence {
        public bool Standable => true;

        public const string COMMAND_NAME = "SAND";
        public static readonly IFactory<IObstacle, IObstacle> FACTORY = new ObtacleFactory();
        private sealed class ObtacleFactory : AObstacleFactory {
            internal ObtacleFactory() { }
            public override bool IsDeserializeable(string s) {
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, COMMAND_NAME, out rest)) {
                    if (rest.Length == 2) {
                        int x, y;
                        if (int.TryParse(rest[0], out x) && int.TryParse(rest[1], out y)) {
                            cache.Cached(s, new Sand(x, y));
                            return true;
                        }
                    }
                }
                return false;
            }

            public override bool IsTransferable(IObstacle c) {
                var c2 = c as Sand;
                if (c2 != null) {
                    cache.Cached(c, new Sand(c2.X, c2.Y));
                    return true;
                }
                return false;
            }

            public override bool IsSerializable(IObstacle c) {
                Sand c2 = c as Sand;
                if (c2 != null) {
                    cacheForSerialize.Cached(c, ProtocolV1_0Utils.SerializeParams(COMMAND_NAME, c2.X, c2.Y));
                    return true;
                }
                return false;
            }
        }

        private static readonly Image icon = new Bitmap(30, 30);
        private static readonly RectangleF iconRectangle = new RectangleF(0, 0, icon.Width, icon.Height);

        static Sand() {
            ObstaclesAroundRobot.OBSTACLE_FACTORIES.RegisterCommand(FACTORY);

            Graphics graphics = Graphics.FromImage(icon);

            graphics.FillRectangle(new SolidBrush(Color.Yellow), iconRectangle);
            Pen black = new Pen(Color.Black, 1);
            int sandNotchSize = 2;
            Random random = new Random();
            for (int i = 0; i < 10; i++) {
                int x = random.Next(icon.Width);
                int y = random.Next(icon.Height);
                graphics.FillEllipse(black.Brush, x, y, sandNotchSize, sandNotchSize);
            }
        }

        public string TypeName => this.GetType().ToString();

        public int X { get; }
        public int Y { get; }
        public bool Used { get; set; }

        public Sand(int x, int y) {
            X = x;
            Y = y;
        }

        public void Change(Robot robot, int turn, ref double fromX, ref double fromY, ref double toX, ref double toY) {
            // TODO can not leave sand
            double speedX = RobotUtils.GetSpeedX(robot);
            double speedY = RobotUtils.GetSpeedX(robot);

            double dX = toX - robot.X;
            double dY = toY - robot.Y;
            if (dX == dY && dY == 0) {
                fromX = robot.X;
                fromY = robot.Y;
                toX = robot.X;
                toY = robot.Y;
            } else if (Math.Abs(dY) > Math.Abs(dX)) {
                double leaveSendY = Y + 1;
                double leaveSendX = (leaveSendY - robot.Y) * dX / dY + robot.X;

                double timeInSendX = (leaveSendX - robot.X) / (speedX / 2);
                double timeInSendY = (leaveSendY - robot.Y) / (speedY / 2);
                double origTimeY = dY / speedY;
                double origTimeX = dX / speedX;


                toY = leaveSendY + speedY * (origTimeY - timeInSendY);
                toX = leaveSendX + speedX * (origTimeX - timeInSendX);

                fromY = leaveSendY;
                fromX = leaveSendX;
            } else {
                double leaveSendX = X + 1;
                double leaveSendY = (leaveSendX - robot.X) * dY / dX + robot.Y;

                double timeInSendY = (leaveSendY - robot.Y) / (speedY / 2);
                double timeInSendX = (leaveSendX - robot.X) / (speedX / 2);
                double origTimeX = dX / speedX;
                double origTimeY = dY / speedY;


                toX = leaveSendX + speedX * (origTimeX - timeInSendX);
                toY = leaveSendY + speedY * (origTimeY - timeInSendY);

                fromX = leaveSendX;
                fromY = leaveSendY;
            }
        }

        public void Draw(Graphics graphics, float xScale, float yScale) {
            PointF leftUpCorner = new PointF(X * xScale, Y * yScale);
            graphics.DrawImage(icon, new RectangleF(leftUpCorner.X, leftUpCorner.Y, xScale, yScale), iconRectangle, GraphicsUnit.Pixel);
        }
    }
}
