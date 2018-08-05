using System.Collections.Generic;

namespace BaseLibrary.equipment {
    public class Zone {

        /// <summary>
        /// Return first sufficient zone (<code>zone.DISTANCE > distance</code>).
        /// </summary>
        /// <param name="zones">Field of zones. Zones have to be sorted ASC by DISTANCE</param>
        /// <param name="distance">Compared distance.</param>
        /// <returns>Return sufficient zone or <code>NULL_ZONE</code></returns>
        /// <seealso cref="NULL_ZONE"/>
        public static Zone GetZoneByDistance(Zone[] zones, double distance) {
            foreach (Zone zone in zones) {
                if (zone.DISTANCE > distance) {
                    return zone;
                }
            }
            return NULL_ZONE;
        }

        /// <summary>
        /// Return first sufficient zone (<code>zone.DISTANCE > distance</code>).
        /// </summary>
        /// <param name="zones">List of zones. Zones have to be sorted ASC by DISTANCE</param>
        /// <param name="distance">Compared distance.</param>
        /// <returns>Return sufficient zone or <code>NULL_ZONE</code></returns>
        /// <seealso cref="NULL_ZONE"/>
        public static Zone GetZoneByDistance(IEnumerable<Zone> zones, double distance) {
            foreach (Zone zone in zones) {
                if (zone.DISTANCE > distance) {
                    return zone;
                }
            }
            return NULL_ZONE;
        }

        /// <summary>
        /// Zone with no effect (<code>EFFECT = 0 </code>) and max distance (<code>DISTANCE = int.MaxValue</code>).
        /// </summary>
        public static readonly Zone NULL_ZONE = new Zone(int.MaxValue, 0);

        /// <summary>
        /// How far this zone reach.
        /// </summary>
        public int DISTANCE { get; private set; }

        /// <summary>
        /// Effect in this zone.
        /// </summary>
        public int EFFECT { get; private set; }

        public Zone(int distance, int effect) {
            DISTANCE = distance;
            EFFECT = effect;
        }

        /// <summary>
        /// Compare two zones.
        /// </summary>
        /// <param name="other">Compared zone this this zone</param>
        /// <returns></returns>
        protected bool Equals(Zone other) {
            return DISTANCE == other.DISTANCE && EFFECT == other.EFFECT;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Zone)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            unchecked {
                return (DISTANCE * 397) ^ EFFECT;
            }
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{nameof(DISTANCE)}: {DISTANCE}, {nameof(EFFECT)}: {EFFECT}";
        }
    }
}
