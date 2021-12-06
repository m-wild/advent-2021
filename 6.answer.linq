<Query Kind="Program" />

void Main()
{
	var file = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "6.sample.txt");
	var input = File.ReadAllLines(file);

	Debug.Assert(input.Length == 1);

	// --- Part One ---	
	//ModelPopulationGrowth(input, 80);
	var totalPopulaton = ModelPopulationGrowth2(input, days: 80);
	Debug.Assert(totalPopulaton.Dump() == 5934);
	//Debug.Assert(totalPopulaton.Dump() == 352872);


	//// --- Part Two ---
	//var totalPopulaton2 = ModelPopulationGrowth2(input, days: 256);
	//Debug.Assert(totalPopulaton2.Dump() == 26984457539);
	////Debug.Assert(totalPopulaton2.Dump() == );
}

double ModelPopulationGrowth2(string[] input, int days)
{
	var fish = input[0].Split(',').Select(x => int.Parse(x)).ToList();

	var pop = fish.Count ^ (days / 7);

		
		
	
	return pop;	
}


long ModelPopulationGrowth(string[] input, int days)
{
	var fish = input[0].Split(',').Select(x => int.Parse(x)).ToList();
	
	for (var d = 1; d <= days; d++)
	{
		var fishCountStartOfDay = fish.Count;
		for (var f = 0; f < fishCountStartOfDay; f++)
		{
			fish[f]--;
			if (fish[f] == -1)
			{
				fish[f] = 6;
				fish.Add(8);
			}
		}

		//Util.RawHtml($"<pre>Population size {fish.Count:0000000000} after {d:000} days</pre>").Dump();
	}
	
	var totalPopulaton = fish.LongCount();
	return totalPopulaton;
}