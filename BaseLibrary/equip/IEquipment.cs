namespace BaseLibrary.equip {
    /// <summary>
    /// Interface for specified that instance is equipment, can be bought during merchant phase.
    /// </summary>
    public interface IEquipment {
        /// <summary>
        /// How many this equipment cost gold.
        /// </summary>
        int COST { get; }
        
        /// <summary>
        /// This equipment's id for buying.
        /// </summary>
        int ID { get; }
    }
}
