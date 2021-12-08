<Query Kind="Program" />

void Main()
{
	var file = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "7.input.txt");
	var input = File.ReadAllLines(file);

	Debug.Assert(input.Length == 1);
	var crabs = input.First().Split(',').Select(s => int.Parse(s));
	
	// --- Part One ---
	var opts1 = Enumerable.Range(crabs.Min(), crabs.Max())
		.ToDictionary(
			pos => pos, 
			pos => crabs.Select(c => Math.Abs(c - pos)).Sum()
		);

	opts1.Dump("Part One: All movement options", collapseTo: 0);
	
	var ans1 = opts1.MinBy(x => x.Value).Dump("Part One: Most efficient movement");
	//Debug.Assert(ans1.Key == 2 && ans1.Value == 37);
	Debug.Assert(ans1.Value == 340056);


	// --- Part Two ---
	var opts2 = Enumerable.Range(crabs.Min(), crabs.Max())
	.ToDictionary(
		pos => pos,
								 // Triangle number = n(n+1) / 2
		pos => crabs.Select(c => (Math.Abs(c-pos)*(Math.Abs(c-pos) + 1))/2.0   ).Sum()
	);

	opts2.Dump("Part Two: All movement options", collapseTo: 0);

	var ans2 = opts2.MinBy(x => x.Value).Dump("Part Two: Most efficient movement");
	//Debug.Assert(ans2.Key == 5 && ans2.Value == 168);
	Debug.Assert(ans2.Value == 96592275);

}