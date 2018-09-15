
package BaseLibrary.equipment;

import BaseLibrary.communication.protocol.AProtocol;
import BaseLibrary.communication.protocol.IInnerObject;

public class Zone implements IInnerObject {

	private static final String NAME = "ZONE";

	static {
		AProtocol.registerForDeserialize(NAME, Zone.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}

	/**
	 * Return first sufficient zone (<code>zone.DISTANCE > distance</code>).
	 * 
	 * @param zones
	 *            - Field of zones. Zones have to be sorted ASC by DISTANCE
	 * @param distnace
	 *            - Compared distance.
	 * @return Return sufficient zone or <code>NULL_ZONE</code>
	 */
	public static Zone GetZoneByDistance(Iterable<Zone> zones, double distance) {
		for (Zone zone : zones) {
			if (zone.DISTANCE > distance) {
				return zone;
			}
		}
		return NULL_ZONE;
	}

	/**
	 * Zone with no effect (<code>EFFECT = 0 </code>) and max distance
	 * (<code>DISTANCE = int.MaxValue</code>).
	 */
	public static final Zone NULL_ZONE = new Zone(Integer.MAX_VALUE, 0);

	/**
	 * How far this zone reach.
	 */
	public final int DISTANCE;

	/**
	 * Effect in this zone.
	 */
	public final int EFFECT;

	public Zone(int distance, int effect) {
		DISTANCE = distance;
		EFFECT = effect;
	}

	public boolean equals(Zone other) {
		if (DISTANCE != other.DISTANCE)
			return false;
		if (EFFECT != other.EFFECT)
			return false;
		return true;
	}

	@Override
	public String toString() {
		return "Zone [DISTANCE=" + DISTANCE + ", EFFECT=" + EFFECT + "]";
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + DISTANCE;
		result = prime * result + EFFECT;
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		Zone other = (Zone) obj;
		return equals(other);
	}

}
