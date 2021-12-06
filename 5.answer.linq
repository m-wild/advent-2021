<Query Kind="Program" />

void Main()
{
	var file = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "5.input.txt");
	var input = File.ReadAllLines(file);
	
	var lines = ParseInput(input);
		
	var maxX = lines.SelectMany(l => l.Points).Select(p => p.X).Max() + 1;
	var maxY = lines.SelectMany(l => l.Points).Select(p => p.Y).Max() + 1;
	
	// --- Part One ---
	// Note: Java-drawing-like coordinates [y,x] makes linqpad Dump match sample output.
	var seabed1 = new int[maxY, maxX];
	foreach (var line in lines.Where(l => l.Simple))
	{
		line.Draw(ref seabed1);
	}

	Util.OnDemand("Click for Seabed data", () => seabed1).Dump();
	Bitmap(seabed1).Dump(Util.ScaleMode.ResizeTo(500), "Seabed One");


	var overlaps1 = 0;
	foreach (var p in seabed1)
	{
		if (p >= 2) overlaps1++;
	}
	
	overlaps1.Dump("Overlaps in Seabed One");
	//Debug.Assert(overlaps1 == 5);


	// --- Part Two ---
	var seabed2 = new int[maxY, maxX];
	foreach (var line in lines)
	{
		line.Draw(ref seabed2);
	}

	Util.OnDemand("Click for Seabed data", () => seabed2).Dump();
	Bitmap(seabed2).Dump(Util.ScaleMode.ResizeTo(500), "Seabed Two");

	var overlaps2 = 0;
	foreach (var p in seabed2)
	{
		if (p >= 2) overlaps2++;
	}

	overlaps2.Dump("Overlaps in Seabed Two");
	//Debug.Assert(overlaps2 == 12);
}

static System.Drawing.Bitmap Bitmap(int[,] seabed)
{
	var max = 0;
	foreach (var p in seabed)
	{
		if (p > max) max = p;	
	}
	var scale = 255 / max;
	
	var maxY = seabed.GetLength(0);
	var maxX = seabed.GetLength(1);
	
	var bitmap = new System.Drawing.Bitmap(maxX, maxY);
	for (var y = 0; y < maxY; y++)
	for (var x = 0; x < maxX; x++)
	{
		var green = seabed[y, x] * scale;
		var color = System.Drawing.Color.FromArgb(0, green, 0);
		bitmap.SetPixel(x, y, color);
	}

	return bitmap;
}


record Line(Point From, Point To)
{
	public IEnumerable<Point> Points => new[] { From, To };

	public bool Simple => From.X == To.X || From.Y == To.Y;

	public void Draw(ref int[,] seabed)
	{
		var dist = DiagonalDistance();
		for (var step = 0; step <= dist; step++)
		{
			var travel = dist == 0 ? 0.0 : step / dist;
			var x = (int)Math.Round(LinerInterpolate(From.X, To.X, travel));
			var y = (int)Math.Round(LinerInterpolate(From.Y, To.Y, travel));
			
			seabed[y,x]++;
		}
	}
	
	static double LinerInterpolate(int start, int end, double t)
	{
		return start + t * (end - start);
	}
	
	private double DiagonalDistance()
	{
		var dx = From.X - To.X;
		var dy = From.Y - To.Y;
		
		return Math.Max(Math.Abs(dx), Math.Abs(dy));
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
