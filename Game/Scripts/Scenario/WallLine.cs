using Godot;

public class WallLine
{
	private readonly Vector2 _pointA;
	private readonly Vector2 _pointB;

	public WallLine(Vector2 pointA, Vector2 pointB)
	{
		_pointA = pointA;
		_pointB = pointB;
	}

	public bool Intersects(Vector2 pointA, Vector2 pointB)
	{
		return Intersects(pointA, pointB, _pointA, _pointB);
	}

	// The main function that returns true if line segment 'p1q1' 
	// and 'p2q2' intersect. 
	private static bool Intersects(Vector2 p1, Vector2 q1, Vector2 p2, Vector2 q2)
	{
		// Find the four orientations needed for general and 
		// special cases 
		int o1 = orientation(p1, q1, p2);
		int o2 = orientation(p1, q1, q2);
		int o3 = orientation(p2, q2, p1);
		int o4 = orientation(p2, q2, q1);

		// General case 
		if(o1 != o2 && o3 != o4)
			return true;

		// Special Cases 
		// p1, q1 and p2 are collinear and p2 lies on segment p1q1 
		if(o1 == 0 && onSegment(p1, p2, q1)) return true;

		// p1, q1 and q2 are collinear and q2 lies on segment p1q1 
		if(o2 == 0 && onSegment(p1, q2, q1)) return true;

		// p2, q2 and p1 are collinear and p1 lies on segment p2q2 
		if(o3 == 0 && onSegment(p2, p1, q2)) return true;

		// p2, q2 and q1 are collinear and q1 lies on segment p2q2 
		if(o4 == 0 && onSegment(p2, q1, q2)) return true;

		return false; // Doesn't fall in any of the above cases 
	}

	// Given three collinear points p, q, r, the function checks if 
	// point q lies on line segment 'pr' 
	static bool onSegment(Vector2 p, Vector2 q, Vector2 r)
	{
		if(q.X <= Mathf.Max(p.X, r.X) && q.X >= Mathf.Min(p.X, r.X) &&
		   q.Y <= Mathf.Max(p.Y, r.Y) && q.Y >= Mathf.Min(p.Y, r.Y))
			return true;

		return false;
	}

	// To find orientation of ordered triplet (p, q, r). 
	// The function returns following values 
	// 0 --> p, q and r are collinear 
	// 1 --> Clockwise 
	// 2 --> Counterclockwise 
	static int orientation(Vector2 p, Vector2 q, Vector2 r)
	{
		// See https://www.geeksforgeeks.org/orientation-3-ordered-points/ 
		// for details of below formula. 
		float val = (q.Y - p.Y) * (r.X - q.X) -
		            (q.X - p.X) * (r.Y - q.Y);

		if(Mathf.Abs(val) < 0.001f)
		{
			// collinear 
			return 0;
		}

		return (val > 0) ? 1 : 2; // clock or counterclock wise 
	}
}