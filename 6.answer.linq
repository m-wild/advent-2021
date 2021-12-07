<Query Kind="Program" />

void Main()
{
	var file = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "6.input.txt");
	var input = File.ReadAllLines(file);

	Debug.Assert(input.Length == 1);
	var fish = input[0].Split(',').Select(x => int.Parse(x)).ToList(); 

	// --- Part One ---	
	var totalPopulaton = ModelPopulationGrowth(fish, days: 80);
	//Debug.Assert(totalPopulaton.Dump() == 5934);
	Debug.Assert(totalPopulaton.Dump() == 352872);


	// --- Part Two ---
	var totalPopulaton2 = ModelPopulationGrowth(fish, days: 256);
	//Debug.Assert(totalPopulaton2.Dump() == 26984457539);
	Debug.Assert(totalPopulaton2.Dump() == 1604361182149);
}

long ModelPopulationGrowth(List<int> fish, int days)
{
	checked
	{
		var shoal = Enumerable.Range(0, 9).ToDictionary(i => i, _ => 0L);

		foreach (var f in fish)
		{
			shoal[f]++;
		}

		for (int d = 0; d < days; d++)
		{
			var newShoal = Enumerable.Range(0, 9).ToDictionary(i => i, _ => 0L);

			foreach (var (key, value) in shoal)
			{
				if (key > 0)
				{
					newShoal[key - 1] = shoal[key]; // all non-zero fish move 1 day closer to spawning
				}
			}

			newShoal[6] += shoal[0]; // all spawning fish return to 6
			newShoal[8] = shoal[0]; // all spawning fish spawn a new fish at day 8
			shoal = newShoal;
		}

		return shoal.Sum(s => s.Value);
	}
}
