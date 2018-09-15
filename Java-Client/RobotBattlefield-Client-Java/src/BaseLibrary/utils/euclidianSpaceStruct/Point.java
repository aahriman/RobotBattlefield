package BaseLibrary.utils.euclidianSpaceStruct;



public class Point implements Comparable<Point> {

    public final double X;
    public final double Y;

    public Point(double x, double y) {
        X = x;
        Y = y;
    }


    @Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		long temp;
		temp = Double.doubleToLongBits(X);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		temp = Double.doubleToLongBits(Y);
		result = prime * result + (int) (temp ^ (temp >>> 32));
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
		Point other = (Point) obj;
		if (Double.doubleToLongBits(X) != Double.doubleToLongBits(other.X))
			return false;
		if (Double.doubleToLongBits(Y) != Double.doubleToLongBits(other.Y))
			return false;
		return true;
	}




	@Override
    public String toString() {
        return String.format("Point[x={%d}, y={%d}]", X, Y);
    }

	@Override
	public int compareTo(Point other) {
		int xComparison = Double.compare(X, other.X);
        if (xComparison != 0) return xComparison;
        return Double.compare(Y, other.Y);
	}
}

