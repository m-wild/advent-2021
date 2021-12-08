<Query Kind="Program" />

void Main()
{
	var file = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "8.sample.txt");
	var input = File.ReadAllLines(file);

	//  aaaa
	// b    c
	// b    c
	//  dddd
	// e    f
	// e    f
	//  gggg

	var segments = new Dictionary<int, HashSet<char>>
	{
		// digit = segment[]
		[0] = new("abcefg"),
		[1] = new("cf"),
		[2] = new("acdeg"),
		[3] = new("acdfg"),
		[4] = new("bcdf"),
		[5] = new("abdfg"),
		[6] = new("abdefg"),
		[7] = new("acf"),
		[8] = new("abcdefg"),
		[9] = new("abcdfg"),
	};

	var possible = segments.GroupBy(s => s.Value.Count)
		.OrderBy(g => g.Key)
		.ToDictionary(
			g => g.Key,							// Number of segments in 7S Display
			g => g.Select(x => x.Key).ToArray() // Possible Digit
		);

	var samples = input.Select(s => Sample.Parse(s)).ToList();
	
	// --- Part One ---
	var easy = samples.SelectMany(s => s.OutputDigits)
		.Where(d => possible[d.Length].Length == 1)
		.Count()
		.Dump("Part One: Easy Digits");
	Debug.Assert(easy == 26);
	//Debug.Assert(easy == 493);

	// --- Part Two ---
	
	var possibleSegmentMap = new Dictionary<char, HashSet<char>>
	{
		['a'] = new("abcdefg"),
		['b'] = new("abcdefg"),
		['c'] = new("abcdefg"),
		['d'] = new("abcdefg"),
		['e'] = new("abcdefg"),
		['f'] = new("abcdefg"),
		['g'] = new("abcdefg"),
	};


	var sample = samples[0].UniquePatterns
		.Select(p => new
		{
			Pattern = p,
			PossibleDigit = possible[p.Length],
			Digit = possible[p.Length].Length == 1 ? possible[p.Length].Single() : (int?)null,
		})
		.ToList()
		.Dump();
	
	
	foreach (var item in sample)
	{
		if (item.Digit != null)
		{
			var digitPositions = segments[item.Digit.Value];
			
			foreach (var position in digitPositions)
			{
				// WTF am I doing...
				possibleSegmentMap[position].RemoveMany(item.Pattern);
			}
		}
	}
	
	foreach (var kv in possibleSegmentMap)
	{
		possibleSegmentMap[kv.Key] = new (kv.Value.OrderBy(v => v));
	}
	
	possibleSegmentMap.OrderBy(kv => kv.Value.Count).Dump();

}

class Sample
{
	public static Sample Parse(string s)
	{
		var split = s.Split(" | ");
		Debug.Assert(split.Length == 2);
		
		var seg = new Sample
		{
			//Raw = s,
			UniquePatterns = split[0].Split(" ").Select(x => x.Trim()).ToList(),
			OutputDigits = split[1].Split(" ").Select(x => x.Trim()).ToList(),
		};
		Debug.Assert(seg.UniquePatterns.Count == 10);
		Debug.Assert(seg.OutputDigits.Count == 4);

		return seg;
	}
	
	//public string Raw;
	public List<string> UniquePatterns;
	public List<string> OutputDigits;
}

static class Extensions
{
	public static void Merge<T>(this ISet<T> set, IEnumerable<T> values)
	{
		foreach (var v in values)
		{
			if (!set.Contains(v))
			{
				set.Add(v);
			}
		}
	}
	
	public static void RemoveMany<T>(this ISet<T> set, IEnumerable<T> values)
	{
		foreach (var v in values)
		{
			set.Remove(v);
		}
	}
}
