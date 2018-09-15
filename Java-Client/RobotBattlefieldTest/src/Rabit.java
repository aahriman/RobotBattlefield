import java.util.Arrays;
import java.util.Random;

import BaseLibrary.utils.Utils;
import ClientLibrary.config.ClientConfig;
import ClientLibrary.robot.ClientRobot;
import ClientLibrary.robot.Tank;

public class Rabit {

	private static final Random RANDOM = new Random();

	public static void main(String[] args) {
		System.out.println(Arrays.toString(ClientConfig.SUPPORTED_PROTOCOLS));
		Tank tank = new Tank("Rabbit");
		ClientRobot.connect(args);
		System.out.println("Rabbit is ready for action.");
		while (true) {
			double toX = RANDOM.nextInt(800) + 100;
			double toY = RANDOM.nextInt(800) + 100;

			while (Math.abs(toX - tank.X) > 50 && Math.abs(toY - tank.Y) > 50) {
				double angle = Utils.angleDegree(tank.X, tank.Y, toX, toY);
				if (Math.abs(tank.AngleDrive - angle) > 3) {
					tank.drive(angle, tank.Motor.ROTATE_IN);
				} else {
					tank.doNothing();
				}
			}
		}
	}
}
