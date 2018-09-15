package BaseLibrary.communication.command.equipment;

import BaseLibrary.communication.command.ACommand;
import BaseLibrary.equipment.Motor;
import BaseLibrary.utils.ModUtils;

public abstract class AEquipmentCommand extends ACommand {
	
	static {
		ModUtils.loadClassFromPackage(Motor.class.getPackage().getName());
	}
}

