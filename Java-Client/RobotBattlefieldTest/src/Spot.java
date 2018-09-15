import java.io.IOException;

import BaseLibrary.communication.command.common.ScanAnswerCommand;
import ClientLibrary.robot.ClientRobot;
import ClientLibrary.robot.Tank;

public class Spot {
	public static void main(String[] args) throws IOException {
		ClientRobot.connect(args);

		Tank spot = new Tank("Spot");

		int direction = 90;
		while (true) {
			if (spot.Power == 0) {
				spot.drive(direction, 100);
			} else if ((direction == 90 && spot.Y > 575) || (direction == 270 && spot.Y < 425)
					|| (direction == 180 && spot.X < 150) || (direction == 0 && spot.X > 850)) {
				spot.drive(direction, 0);
				direction = (direction + 90) % 360;
			} else {
				for (int angle = 0; angle < 360; angle += 30) {
					ScanAnswerCommand scanAnswer;
					if ((scanAnswer = spot.scan(angle, 10)).getEnemyID() != spot.ID) {
						spot.shoot(angle, scanAnswer.getRange());
					}
				}
			}
		}
	}

}
