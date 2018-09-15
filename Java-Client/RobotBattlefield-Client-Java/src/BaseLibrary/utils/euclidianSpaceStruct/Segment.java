package BaseLibrary.utils.euclidianSpaceStruct;


public class Segment {

    public final Point From;
    public final Point To;

    public Segment(double fromX, double fromY, double toX, double toY) {
    	this(new Point(fromX, fromY), new Point(toX, toY));
    }

    public Segment(Point from, Point to) {
        From = from;
        To = to;
    }

    

    @Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + ((From == null) ? 0 : From.hashCode());
		result = prime * result + ((To == null) ? 0 : To.hashCode());
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
		Segment other = (Segment) obj;
		if (From == null) {
			if (other.From != null)
				return false;
		} else if (!From.equals(other.From))
			return false;
		if (To == null) {
			if (other.To != null)
				return false;
		} else if (!To.equals(other.To))
			return false;
		return true;
	}


	

	@Override
	public String toString() {
		return "Segment{From=" + From + ", To=" + To + "}";
	}
}
