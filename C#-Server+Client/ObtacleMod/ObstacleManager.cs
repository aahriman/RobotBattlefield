using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseLibrary.battlefield;
using BaseLibrary.utils;
using BaseLibrary.utils.euclidianSpaceStruct;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ObstacleMod {

    public class ObstacleManager {
        public static IObstacle GetObtacle(string obstacleName, int x, int y) {
            Type type = Type.GetType(obstacleName);
            if (type != null)
                return (IObstacle)Activator.CreateInstance(type, x, y);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies()) {
                type = asm.GetType(obstacleName);
                if (type != null)
                    return (IObstacle)Activator.CreateInstance(type, x, y);
            }
            return null;
        }

        public static void SaveObstaclesToFile(IEnumerable<IObstacle> obstacles, string filename) {
            SaveObstaclesToFile(obstacles.ToArray(), filename);
        }

        public static void SaveObstaclesToFile(IObstacle[] obstacles, string filename) {
            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine(JsonConvert.SerializeObject(obstacles));
            writer.Close();
        }

        public static List<IObstacle> LoadObstaclesFromFile(string filename) {

            List<IObstacle> obstacles = new List<IObstacle>();
            StreamReader reader = new StreamReader(filename);
            IEnumerable<JObject> deserializedObject = JsonConvert.DeserializeObject<IEnumerable<JObject>>(reader.ReadLine());
            foreach (var obstacle in deserializedObject) {
                JToken x = obstacle["X"];
                JToken y = obstacle["Y"];
                JToken typeName = obstacle["TypeName"];
                obstacles.Add(GetObtacle(typeName.ToString(), x.ToObject<int>(), y.ToObject<int>()));
            }
            reader.Close();
            return obstacles;
        }

        private readonly Random random;

        private Dictionary<Point, IMoveInfluence> movementObstacles = new Dictionary<Point, IMoveInfluence>();
        private Dictionary<Point, IShotInfluence> shotObstacles = new Dictionary<Point, IShotInfluence>();
        private Dictionary<Point, IScanInfluence> scanObstacles = new Dictionary<Point, IScanInfluence>();

        public ObstacleManager(IEnumerable<IObstacle> obstacles, int? randomSeed ) {
            IEnumerable<IMoveInfluence> movementObstacles = from o in obstacles
                                                           where o is IMoveInfluence
                                                           select o as IMoveInfluence;
            IEnumerable<IShotInfluence> shotObstacles = from o in obstacles
                                                       where o is IShotInfluence
                                                       select o as IShotInfluence;

            IEnumerable<IScanInfluence> scanObstacles = from o in obstacles
                                                       where o is IScanInfluence
                                                       select o as IScanInfluence;

            foreach (var obstacle in movementObstacles) {
                this.movementObstacles.Add(new Point(obstacle.X, obstacle.Y), obstacle);
            }

            foreach (var obstacle in shotObstacles) {
                this.shotObstacles.Add(new Point(obstacle.X, obstacle.Y), obstacle);
            }

            foreach (var obstacle in scanObstacles) {
                this.scanObstacles.Add(new Point(obstacle.X, obstacle.Y), obstacle);
            }

            if (randomSeed == null) {
                random = new Random();
            } else {
                random = new Random((int)randomSeed);
            }
        }

        public void AddMovementObstacle(IMoveInfluence obstacle) {
            this.movementObstacles.Add(new Point(obstacle.X, obstacle.Y), obstacle);
        }

        public void AddShotObstacle(IShotInfluence obstacle) {
            this.shotObstacles.Add(new Point(obstacle.X, obstacle.Y), obstacle);
        }

        public void AddScanObstacle(IScanInfluence obstacle) {
            this.scanObstacles.Add(new Point(obstacle.X, obstacle.Y), obstacle);
        }

        public IObstacle[] GetObstaclesInPoints(Point[] points) {
            List<IObstacle> obstacles = new List<IObstacle>();
            foreach (var point in points) {
                if (movementObstacles.TryGetValue(point, out IMoveInfluence moveInfluenceObstacle)) {
                    obstacles.Add(moveInfluenceObstacle);
                } else if (scanObstacles.TryGetValue(point, out IScanInfluence scanInfluenceObstacle)) {
                    obstacles.Add(scanInfluenceObstacle);
                } else if (shotObstacles.TryGetValue(point, out IShotInfluence shootInfluenceObstacle)) {
                    obstacles.Add(shootInfluenceObstacle);
                }
            }
            return obstacles.ToArray();
        }

        public Point StartRobotPosition(int maxX, int maxY) {
            for (int i = 0; i < 10; i++) {
                int generatedX = random.Next(0, maxX);
                int generatedY = random.Next(0, maxY);

                for (int x = generatedX; x >= 0; x--) {
                    Point position = new Point(x, generatedY);
                    if (!movementObstacles.TryGetValue(position, out IMoveInfluence moveObstacle) || !moveObstacle.Standable) {
                        return new Point((position.X + random.NextDouble()),
                            (position.Y + random.NextDouble()));
                    }
                }

                for (int x = generatedX; x < maxX; x++) {
                    Point position = new Point(x, generatedY);
                    if (!movementObstacles.TryGetValue(position, out IMoveInfluence moveObstacle) || !moveObstacle.Standable) {
                        return new Point((position.X + random.NextDouble()),
                            (position.Y + random.NextDouble()));
                    }
                }

                for (int y = generatedY; y >= 0; y--) {
                    Point position = new Point(generatedX, y);
                    if (!movementObstacles.TryGetValue(position, out IMoveInfluence moveObstacle) || !moveObstacle.Standable) {
                        return new Point((position.X + random.NextDouble()),
                            (position.Y + random.NextDouble()));
                    }
                }

                for (int y = generatedY; y < maxY; y++) {
                    Point position = new Point(generatedX, y);
                    if (!movementObstacles.TryGetValue(position, out IMoveInfluence moveObstacle) || !moveObstacle.Standable) {
                        return new Point((position.X + random.NextDouble()),
                            (position.Y + random.NextDouble()));
                    }
                }
            }

            return manualRobotPosition(maxX, maxY);
        }

        private Point manualRobotPosition(int maxX, int maxY) {
            Console.WriteLine(
                $"Cannot generate start position for robot. Please write it manually (first x, second y split by space, decimal point is '.'). X in [0, {maxX}). Y in [0,{maxY})");
            while (true) {
                string row = Console.ReadLine();
                string[] splitRow = row.Split(' ');
                if (splitRow.Length == 2) {
                    if (double.TryParse(splitRow[0], out double x) && double.TryParse(splitRow[1], out double y)) {
                        return new Point(x, y);
                    }
                }
                Console.WriteLine("Use right format");
            }
        }  

        private IEnumerable<T> getEnumerator<T>(IDictionary<Point, T> dictionary, double fromX, double fromY, double toX,
            double toY) where T : IObstacle {
            // TODO test this method
            T obstacle;
            if (fromX.DEquals(toX) && fromY.DEquals(toY)) {
                if (dictionary.TryGetValue(new Point(fromX, fromY), out obstacle)) {
                    yield return obstacle;
                } else {
                    yield break;
                }
            }

            // průchod po přímce a vracení obstacle na daném bodě
            double dX = toX - fromX;
            double dY = toY - fromY;
            double x = fromX;
            double y = fromY;
            double stepY = dY / Math.Max(dX, dY);
            double stepX = dX / Math.Max(dX, dY);
            double prevX = x;
            double prevY = y;
            Segment directrix = new Segment(fromX, fromY, toX, toY);

            while (x <= toX && y <= toY) {
                int prevLeftBorder = (int) prevX;
                int prevRightBorder = (int) prevX + 1;
                int prevUpBorder = (int) prevY;
                int prevDownBorder = (int) prevY - 1;
                
                double actualSpaceToPrevLeftBorder = x - prevLeftBorder; // < 0 maybe cross left
                double actualSpaceToPrevRightBorder = prevRightBorder - x; // < 0 maybe cross right
                double actualSpaceToPrevUpBorder = y - prevUpBorder; // < 0 maybe cross up
                double actualSpaceToPrevDownBorder = y - prevDownBorder; // < 0 maybe cross down

                bool crossLeftOrRigh = false;
                bool crossUpOrDown = false;

                if (actualSpaceToPrevLeftBorder.CompareTo(0) < 0) { // maybe cross left
                    crossLeftOrRigh |= EuclideanSpaceUtils.FindIntersection(directrix, new Segment(prevLeftBorder, prevUpBorder, prevLeftBorder, prevDownBorder));
                    // can cross also up or down (throught corner)
                    if (actualSpaceToPrevUpBorder.CompareTo(0) < 0) { // maybe cross up
                        crossUpOrDown |= EuclideanSpaceUtils.FindIntersection(directrix, new Segment(prevLeftBorder, prevUpBorder, prevRightBorder, prevUpBorder));
                    } else if (actualSpaceToPrevDownBorder.CompareTo(0) < 0) { // maybe cross down
                        crossUpOrDown |= EuclideanSpaceUtils.FindIntersection(directrix,
                            new Segment(prevLeftBorder, prevDownBorder, prevRightBorder, prevDownBorder));
                    } else {
                        crossLeftOrRigh = false; // cannot cross any more then (x, y)
                    }
                } else if (actualSpaceToPrevRightBorder.CompareTo(0) < 0) { // maybe cross right
                    crossLeftOrRigh |= EuclideanSpaceUtils.FindIntersection(directrix, new Segment(prevRightBorder, prevUpBorder, prevRightBorder, prevDownBorder));
                    // can cross also up or down (throught corner)
                    if (actualSpaceToPrevUpBorder.CompareTo(0) < 0) { // maybe cross up
                        crossUpOrDown |= EuclideanSpaceUtils.FindIntersection(directrix, new Segment(prevLeftBorder, prevUpBorder, prevRightBorder, prevUpBorder));
                    } else if (actualSpaceToPrevDownBorder.CompareTo(0) < 0) { // maybe cross down
                        crossUpOrDown |= EuclideanSpaceUtils.FindIntersection(directrix,
                            new Segment(prevLeftBorder, prevDownBorder, prevRightBorder, prevDownBorder));
                    } else {
                        crossLeftOrRigh = false; // cannot cross any more then (x, y)
                    }
                }
                

                if (crossLeftOrRigh) {
                    Point crossedSqueare = new Point((int) x, (int) prevY);
                    if (dictionary.TryGetValue(crossedSqueare, out obstacle) && obstacle.Used == false) {
                        yield return obstacle;
                    }
                }

                if (crossUpOrDown) {
                    Point crossedSqueare = new Point((int) prevX, (int) y);
                    if (dictionary.TryGetValue(crossedSqueare, out obstacle) && obstacle.Used == false) {
                        yield return obstacle;
                    }
                }

                if (dictionary.TryGetValue(new Point(x, y), out obstacle) && obstacle.Used == false) {
                    yield return obstacle;
                }
                prevX = x;
                prevY = y;
                x += stepX;
                y += stepY;
            }

        }


        public void MoveChange(Robot robot, int turn, double fromX, double fromY, double toX, double toY) {
            clearUsages(movementObstacles.Values);
            var enumerator = getEnumerator(movementObstacles, fromX, fromY, toX, toY).GetEnumerator();
            List<IMoveInfluence> usedObstacle = new List<IMoveInfluence>();
            while (enumerator.MoveNext()) { 
                IMoveInfluence movementObstacle = enumerator.Current;
                if (movementObstacle == null) continue;
                movementObstacle.Used = true;
                usedObstacle.Add(movementObstacle);

                Segment leadSegment = new Segment(fromX, fromY, toX, toY);
                Point nearestIntersect = EuclideanSpaceUtils.GetNearestIntersect(leadSegment, movementObstacle.Segments());

                robot.X = nearestIntersect.X;
                robot.Y = nearestIntersect.Y;

                double prevFromX = fromX;
                double prevFromY = fromY;
                double prevToX = toX;
                double prevToY = toY;

                movementObstacle.Change(robot, turn, ref fromX, ref fromY, ref toX, ref toY);
                if (!fromX.DEquals(prevFromX) || !fromY.DEquals(prevFromY) || !toX.DEquals(prevToX) || toY.DEquals(prevToY)) {
                    enumerator.Dispose();
                    enumerator = getEnumerator(movementObstacles, fromX, fromY, toX, toY).GetEnumerator();
                }
            }
            enumerator.Dispose();

            robot.X = toX;
            robot.Y = toY;

            clearUsages(usedObstacle);
        }

        public void ShotChange(int turn, double fromX, double fromY, ref double toX, ref double toY) {
            var enumerator = getEnumerator(shotObstacles, fromX, fromY, toX, toY).GetEnumerator();
            while (enumerator.MoveNext()) {
                IShotInfluence obstacle = enumerator.Current;
                if (obstacle == null) continue;
                if (obstacle.Change(turn, fromX, fromY, ref toX, ref toY)) {
                    enumerator.Dispose();
                    Segment leadSegment = new Segment(fromX, fromY, toX, toY);
                    Point nearestIntersect = EuclideanSpaceUtils.GetNearestIntersect(leadSegment, obstacle.Segments());

                    fromX = nearestIntersect.X;
                    fromY = nearestIntersect.Y;
                    enumerator = getEnumerator(shotObstacles, fromX, fromY, toX, toY).GetEnumerator();
                }
            }
        }

        public bool CanScan(int turn, double fromX, double fromY, double toX, double toY) {
            foreach (var obstacle in getEnumerator(scanObstacles, fromX, fromY, toX, toY)) {
                if (!obstacle.CanScan(turn, fromX, fromY, toX, toY)) {
                    return false;
                }
            }
            return true;
        }


        private static void clearUsages<T>(IEnumerable<T> obstacles) where T : IObstacle {
            foreach (var obstacle in obstacles) {
                obstacle.Used = false;
            }
        }
    }
}
