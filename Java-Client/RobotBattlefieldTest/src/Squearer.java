import java.util.Scanner;

import BaseLibrary.utils.Utils;
import ClientLibrary.robot.ClientRobot;
import ClientLibrary.robot.MineLayer;
import ClientLibrary.robot.MineLayer.Mine;

public class Miner {

	static class Coordinate {
		public final double X;
		public final double Y;

		public Coordinate(double x, double y) {
			X = x;
			Y = y;
		}
	}

	public static final int NUMBER_OF_POINTS = 3;
	public static final double MINE_IS_NEAR = 5;
	public static final double CLOSE_TO_POINT = 5;
	public static final double DRIVE_POWER = 50;

	public static final Scanner SCANNER = new Scanner(System.in);

	public static void goToPoint(MineLayer robot, double x, double y) {
		while (Utils.distance(robot.X, robot.Y, x, y) > CLOSE_TO_POINT) {
			robot.drive(Utils.angleDegree(robot.X, robot.Y, x, y), DRIVE_POWER);
		}
	}

	public static boolean isAnyMineNear(MineLayer robot) {
		for (Mine mine : robot.PUT_MINES_LIST) {
			if (Utils.distance(mine.getPosition(), robot.getPosition()) < MINE_IS_NEAR) {
				return true;
			}
		}
		return false;
	}
	
	public static Mine theFarthestMine(MineLayer robot) {
		double distance = 0;
		Mine ret = null;
		for (Mine mine : robot.PUT_MINES_LIST) {
			double mineDistance = Utils.distance(mine.getPosition(), robot.getPosition()); 
			if (mineDistance > distance) {
				distance = mineDistance;
				ret = mine;
			}
		}
		return ret;
	}
	
	public static void detonateTheFatherstMine(MineLayer robot) {
		Mine mine = theFarthestMine(robot);
		robot.detonateMine(mine.ID);
	}

	public static void putMineIfNotNear(MineLayer robot) {
		boolean putMine = !isAnyMineNear(robot);
		if (putMine) {
			robot.putMine();
		}
	}

	public static Coordinate[] readCoordinates() {
		Coordinate[] coordinates = new Coordinate[NUMBER_OF_POINTS];
		for (int i = 0; i < coordinates.length; i++) {
			double x, y;
			System.out.println("Zadejte x souøadnici " + i + ". pozice:");
			x = SCANNER.nextDouble();
			System.out.println("Zadejte y souøadnici " + i + ". pozice:");
			y = SCANNER.nextDouble();
			coordinates[i] = new Coordinate(x, y);
		}
		return coordinates;
	}

	public static void main(String[] args) {
		ClientRobot.connect(args);
		MineLayer robot = new MineLayer("Muj tank");

		// naètení souøadnic
		Coordinate[] coordinates = readCoordinates();
		while (true) {
			// robot bude stále objíždìt zadané body
			for (int i = 0; i < coordinates.length; i++) {
				// robot jede do i-tého bodu
				goToPoint(robot, coordinates[i].X, coordinates[i].Y);
				if (robot.getPutMines() == robot.getMineGun().MAX_MINES - 1 || robot.getPutMines() == NUMBER_OF_POINTS - 1) {
					detonateTheFatherstMine(robot);
				}
				putMineIfNotNear(robot);
			}
		}

	}

}
