namespace BaseLibrary.equip
{
    public class Armor : Equipment{
        public int MAX_HP { get; private set; }
        public int COST { get; private set; }
        public int ID { get; private set; }

        public Armor(int MAX_HP, int COST, int ID) {
            this.MAX_HP = MAX_HP;
            this.COST = COST;
            this.ID = ID;
        }
    }
}
