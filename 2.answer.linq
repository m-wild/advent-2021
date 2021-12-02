<Query Kind="Program" />

void Main()
{
	var file = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "2.input.txt");
	var input = File.ReadAllLines(file)
		.Select(x => new // Line format is `<action><space><amount>` e.g.`up 2` 
		{
			Action = x.Split(' ')[0],
			AbsAmount = int.Parse(x.Split(' ')[1]),
		})
		.ToList();

	// Part 1
	var totals = input
		.Select(x => new 
		{
			Direction = x.Action is "up" or "down" ? "depth" : "horz",
			Amount = x.Action is "up" ? -x.AbsAmount : x.AbsAmount // "up" reduces depth
		})
		.GroupBy(x => x.Direction, x => x.Amount)
		.ToDictionary(x => x.Key, x => x.Sum());
		
	var finalPos = totals["depth"] * totals["horz"];
	finalPos.Dump();


	// Part 2




}

// You can define other methods, fields, classes and namespaces here