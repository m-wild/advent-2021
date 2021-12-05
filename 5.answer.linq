<Query Kind="Program" />

void Main()
{
	var file = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "5.sample.txt");
	var input = File.ReadAllLines(file);
	
	input.Dump();
	
	var lines = ParseInput(input).Dump();
	
	var maxX = lines.SelectMany(l => l.Points).Select(p => p.X).Max();
	var maxY = lines.SelectMany(l => l.Points).Select(p => p.Y).Max();
	
	var seabed = new int[maxX, maxY];
	seabed.Dump();
	
	for (var x = 0; x < maxX; x++)
	for (var y = 0; y < maxY; y++)
	{
		foreach (var line in lines.Where(l => l.Simple))
		{
			if (line.Intersects(new Point(x, y)))
			{
				seabed[x,y]++;
			}
		}	
	}
	
	seabed.Dump();
	
}

record Line(Point From, Point To)
{
	public IEnumerable<Point> Points => new[] { From, To };
	
	public bool Simple => From.X == To.X || From.Y == To.Y;
	
	public bool Intersects(Point p)
	{
		Debug.Assert(Simple);

//		if (p.X == From.X && p.X == To.X)
//		{
//			if (p.Y >= From.Y && p.Y <= To.Y)
//			{
//				return true;
//			}
//		}
//
//		if (p.Y == From.Y && p.Y == To.Y)
//		{
//			if (p.X >= From.X && p.X <= To.X)
//			{
//				return true;
//			}
//		}

		return false;
	}
};

record Point(int X, int Y);

List<Line> ParseInput(string[] input)
{
	var lines = new List<Line>();
	foreach (var i in input)
	{
		var point = i.Split(" -> ").Select(c => new Point(int.Parse(c.Split(',')[0]), int.Parse(c.Split(',')[1]))).ToArray();
		lines.Add(new Line(point[0], point[1]));
	}
	
	return lines;
}
