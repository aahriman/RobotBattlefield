namespace CommunicationLibrary.equip
{
    public class Armor {
        public int MAX_HP { get; private set; }
        public int COST { get; private set; }
        public int ID { get; private set; }

        public Armor(int maxHp, int cost, int id) {
            MAX_HP = maxHp;
            COST = cost;
            ID = id;
        }
    }
}
