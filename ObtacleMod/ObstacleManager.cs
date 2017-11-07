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
        public static IObstacle GetObtacle(string obtacleName, int x, int y) {
            Type type = Type.GetType(obtacleName);
            if (type != null)
                return (IObstacle)Activator.CreateInstance(type, x, y);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies()) {
                type = asm.GetType(obtacleName);
                if (type != null)
                    return (IObstacle)Activator.CreateInstance(type, x, y);
            }
            return null;
        }

        public static void SaveObtaclesToFile(IEnumerable<IObstacle> obtacles, string filename) {
            SaveObtaclesToFile(obtacles.ToArray(), filename);
        }

        public static void SaveObtaclesToFile(IObstacle[] obtacles, string filename) {
            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine(JsonConvert.SerializeObject(obtacles));
            writer.Close();
        }

        public static List<IObstacle> LoadObtaclesFromFile(string filename) {

            List<IObstacle> obtacles = new List<IObstacle>();
            StreamReader reader = new StreamReader(filename);
            IEnumerable<JObject> deserializedObject = JsonConvert.DeserializeObject<IEnumerable<JObject>>(reader.ReadLine());
            foreach (var obtacle in deserializedObject) {
                JToken x = obtacle["X"];
                JToken y = obtacle["Y"];
                JToken typeName = obtacle["TypeName"];
                obtacles.Add(GetObtacle(typeName.ToString(), x.ToObject<int>(), y.ToObject<int>()));
            }
            reader.Close();
            return obtacles;
        }

        private readonly Random random = new Random();

        private Dictionary<Point, IMoveInfluence> movementObtacles = new Dictionary<Point, IMoveInfluence>();
        private Dictionary<Point, IShotInfluence> shotObtacles = new Dictionary<Point, IShotInfluence>();
        private Dictionary<Point, IScanInfluence> scanObtacles = new Dictionary<Point, IScanInfluence>();

        public ObstacleManager(IEnumerable<IObstacle> obtacles ) {
            IEnumerable<IMoveInfluence> movementObtacles = from o in obtacles
                                                           where o is IMoveInfluence
                                                           select o as IMoveInfluence;
            IEnumerable<IShotInfluence> shotObtacles = from o in obtacles
                                                       where o is IShotInfluence
                                                       select o as IShotInfluence;

            IEnumerable<IScanInfluence> scanObtacles = from o in obtacles
                                                       where o is IScanInfluence
                                                       select o as IScanInfluence;

            foreach (var obtacle in movementObtacles) {
                this.movementObtacles.Add(new Point(obtacle.X, obtacle.Y), obtacle);
            }

            foreach (var obtacle in shotObtacles) {
                this.shotObtacles.Add(new Point(obtacle.X, obtacle.Y), obtacle);
            }

            foreach (var obtacle in scanObtacles) {
                this.scanObtacles.Add(new Point(obtacle.X, obtacle.Y), obtacle);
            }
        }

        public void AddMovementObtacle(IMoveInfluence obtacle) {
            this.movementObtacles.Add(new Point(obtacle.X, obtacle.Y), obtacle);
        }

        public void AddShotObtacle(IShotInfluence obtacle) {
            this.shotObtacles.Add(new Point(obtacle.X, obtacle.Y), obtacle);
        }

        public void AddScanObtacle(IScanInfluence obtacle) {
            this.scanObtacles.Add(new Point(obtacle.X, obtacle.Y), obtacle);
        }

        public IObstacle[] GetObtaclesInPoints(Point[] points) {
            List<IObstacle> obtacles = new List<IObstacle>();
            IMoveInfluence moveInfluenceObtacle;
            IScanInfluence scanInfluenceObtacle;
            IShotInfluence shotInfluenceObtacle;
            foreach (var point in points) {
                if (movementObtacles.TryGetValue(point, out moveInfluenceObtacle)) {
                    obtacles.Add(moveInfluenceObtacle);
                } else if (scanObtacles.TryGetValue(point, out scanInfluenceObtacle)) {
                    obtacles.Add(scanInfluenceObtacle);
                } else if (shotObtacles.TryGetValue(point, out shotInfluenceObtacle)) {
                    obtacles.Add(shotInfluenceObtacle);
                }
            }
            return obtacles.ToArray();
        }

        public Point StartRobotPosition(int maxX, int maxY) {
            for (int i = 0; i < 10; i++) {
                int generatedX = random.Next(0, maxX);
                int generatedY = random.Next(0, maxY);

                for (int x = generatedX; x >= 0; x--) {
                    Point position = new Point(x, generatedY);
                    IMoveInfluence moveObtacle;
                    if (!movementObtacles.TryGetValue(position, out moveObtacle) || !moveObtacle.Standable) {
                        return new Point((position.X + random.NextDouble()),
                            (position.Y + random.NextDouble()));
                    }
                }

                for (int x = generatedX; x < maxX; x++) {
                    Point position = new Point(x, generatedY);
                    IMoveInfluence moveObtacle;
                    if (!movementObtacles.TryGetValue(position, out moveObtacle) || !moveObtacle.Standable) {
                        return new Point((position.X + random.NextDouble()),
                            (position.Y + random.NextDouble()));
                    }
                }

                for (int y = generatedY; y >= 0; y--) {
                    Point position = new Point(generatedX, y);
                    IMoveInfluence moveObtacle;
                    if (!movementObtacles.TryGetValue(position, out moveObtacle) || !moveObtacle.Standable) {
                        return new Point((position.X + random.NextDouble()),
                            (position.Y + random.NextDouble()));
                    }
                }

                for (int y = generatedY; y < maxY; y++) {
                    Point position = new Point(generatedX, y);
                    IMoveInfluence moveObtacle;
                    if (!movementObtacles.TryGetValue(position, out moveObtacle) || !moveObtacle.Standable) {
                        return new Point((position.X + random.NextDouble()),
                            (position.Y + random.NextDouble()));
                    }
                }
            }

            return manualRobotPosition(maxX, maxY);
        }

        private Point manualRobotPosition(int maxX, int maxY) {
            Console.WriteLine(
                $"Cannot geneterate start position for robot. Please write it manualy (first x, second y splitted by space, decimal point is '.'). X in [0, {maxX}). Y in [0,{maxY})");
            while (true) {
                string row = Console.ReadLine();
                string[] splitedRow = row.Split(' ');
                if (splitedRow.Length == 2) {
                    double x, y;
                    if (double.TryParse(splitedRow[0], out x) && double.TryParse(splitedRow[1], out y)) {
                        return new Point(x, y);
                    }
                }
                Console.WriteLine("Use right format");
            }
        }  

        private IEnumerable<T> getEnumerator<T>(IDictionary<Point, T> dictionary, double fromX, double fromY, double toX,
            double toY) where T : IObstacle {
            // TODO test this method
            T obtacle;
            if (fromX.DEquals(toX) && fromY.DEquals(toY)) {
                if (dictionary.TryGetValue(new Point(fromX, fromY), out obtacle)) {
                    yield return obtacle;
                } else {
                    yield break;
                }
            }

            // průchod po přímce a vracení obtacle na daném bodě
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
                    if (dictionary.TryGetValue(crossedSqueare, out obtacle) && obtacle.Used == false) {
                        yield return obtacle;
                    }
                }

                if (crossUpOrDown) {
                    Point crossedSqueare = new Point((int) prevX, (int) y);
                    if (dictionary.TryGetValue(crossedSqueare, out obtacle) && obtacle.Used == false) {
                        yield return obtacle;
                    }
                }

                if (dictionary.TryGetValue(new Point(x, y), out obtacle) && obtacle.Used == false) {
                    yield return obtacle;
                }
                prevX = x;
                prevY = y;
                x += stepX;
                y += stepY;
            }

        }


        public void MoveChange(Robot robot, int turn, double fromX, double fromY, double toX, double toY) {
            clearUsages(movementObtacles.Values);
            var enumerator = getEnumerator(movementObtacles, fromX, fromY, toX, toY).GetEnumerator();
            List<IMoveInfluence> usedObtacle = new List<IMoveInfluence>();
            while (enumerator.MoveNext()) {
                IMoveInfluence movemetObtacle = enumerator.Current;
                movemetObtacle.Used = true;
                usedObtacle.Add(movemetObtacle);

                Segment leadSegment = new Segment(fromX, fromY, toX, toY);
                Point nearestIntersect = EuclideanSpaceUtils.GetNearestIntersect(leadSegment, movemetObtacle.Segments());

                robot.X = nearestIntersect.X;
                robot.Y = nearestIntersect.Y;

                double prevFromX = fromX;
                double prevFromY = fromY;
                double prevToX = toX;
                double prevToY = toY;

                movemetObtacle.Change(robot, turn, ref fromX, ref fromY, ref toX, ref toY);
                if (!fromX.DEquals(prevFromX) || !fromY.DEquals(prevFromY) || !toX.DEquals(prevToX) || toY.DEquals(prevToY)) {
                    enumerator.Dispose();
                    enumerator = getEnumerator(movementObtacles, fromX, fromY, toX, toY).GetEnumerator();
                }
            }
            enumerator.Dispose();

            robot.X = toX;
            robot.Y = toY;

            clearUsages(usedObtacle);
        }

        public void ShotChange(int turn, double fromX, double fromY, ref double toX, ref double toY) {
            var enumerator = getEnumerator(shotObtacles, fromX, fromY, toX, toY).GetEnumerator();
            while (enumerator.MoveNext()) {
                IShotInfluence obtacle = enumerator.Current;
                if (obtacle.Change(turn, fromX, fromY, ref toX, ref toY)) {
                    enumerator.Dispose();
                    Segment leadSegment = new Segment(fromX, fromY, toX, toY);
                    Point nearestIntersect = EuclideanSpaceUtils.GetNearestIntersect(leadSegment, obtacle.Segments());

                    fromX = nearestIntersect.X;
                    fromY = nearestIntersect.Y;
                    enumerator = getEnumerator(shotObtacles, fromX, fromY, toX, toY).GetEnumerator();
                }
            }
        }

        public bool CanScan(int turn, double fromX, double fromY, double toX, double toY) {
            foreach (var obtacle in getEnumerator(scanObtacles, fromX, fromY, toX, toY)) {
                if (!obtacle.CanScan(turn, fromX, fromY, toX, toY)) {
                    return false;
                }
            }
            return true;
        }


        private static void clearUsages<T>(IEnumerable<T> obtacles) where T : IObstacle {
            foreach (var obtacle in obtacles) {
                obtacle.Used = false;
            }
        }
    }
}
