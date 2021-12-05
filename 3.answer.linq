<Query Kind="Program" />

void Main()
{
	var file = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "3.input.txt");
	var input = File.ReadAllLines(file);

	var bitLength = input.First().Length;
	Debug.Assert(input.All(b => b.Length == bitLength));

	// --- Part One ---
	var stats = Enumerable.Range(0, bitLength)
		.Select(i => new BitStats
		{
			Pos = i,
			Count0 = input.Select(b => b[i]).Count(b => b == '0'),
			Count1 = input.Select(b => b[i]).Count(b => b == '1'),
		});

	var gamma = Convert.ToInt32(string.Concat(stats.Select(b => b.MostCommon)), fromBase: 2);
	var epsilon = Convert.ToInt32(string.Concat(stats.Select(b => b.LeastCommon)), fromBase: 2);

	var power = gamma * epsilon;

	new { gamma, epsilon, power, }.Dump();


	// --- Part Two ---
	var oxygen = GetRating(input, (stats, c) => c == stats.MostCommon);
	var co2 = GetRating(input, (stats, c) => c == stats.LeastCommon);

	var lifeSupport = oxygen * co2;

	new { oxygen, co2, lifeSupport, }.Dump();
}

static int GetRating(string[] input, Func<BitStats, char, bool> filter)
{
	var keep = input.ToList();
	for (var i = 0; i < input.First().Length; i++)
	{
		var stats = new BitStats
		{
			Pos = i,
			Count0 = keep.Select(b => b[i]).Count(b => b == '0'),
			Count1 = keep.Select(b => b[i]).Count(b => b == '1'),
		};
		
		keep = keep.Count == 1 ? keep : keep.Where(b => filter(stats, b[i])).ToList();
	}

	Debug.Assert(keep.All(b => b == keep.First()));
	Debug.Assert(keep.Count == 1);
	
	return Convert.ToInt32(keep.First(), fromBase: 2);
}

class BitStats
{
	public int Pos;
	public int Count0 = 0;
	public int Count1 = 0;
	public bool Tie => Count0 == Count1;
	public char MostCommon => Tie || Count1 > Count0 ? '1' : '0';
	public char LeastCommon => Tie || Count0 < Count1 ? '0' : '1';
}