<Query Kind="Program" />

void Main()
{
	var file = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "4.input.txt");
	var input = File.ReadAllLines(file);

	var draw = input.First().Split(',').Select(s => int.Parse(s)).ToArray();
	var boards = ParseBoards(input);

	// --- Part One ---
	var winner = Play(draw, boards).Dump();
	
	//Debug.Assert(winner.Score == 4512);


	// --- Part Two ---

	var loser = FindLoser(draw, boards).Dump();
	
	//Debug.Assert(loser.Board == boards[1]);
	//Debug.Assert(loser.Score == 1924);
}


struct GameResult
{
	public string Draw;
	public Board Board;
	public int Score;
}

static GameResult Play(int[] draw, Board[] boards)
{
	for (var i = 0; i < draw.Length; i++)
	{
		foreach (var b in boards)
		{
			var currentDraw = draw.Take(i + 1).ToArray();
			var (win, score) = b.Play(currentDraw);
			if (win)
			{
				return new GameResult
				{
					Draw = string.Join(", ", currentDraw),
					Board = b,
					Score = score,
				};
			}
		}
	}

	return default;
}


static GameResult FindLoser(int[] draw, Board[] boards)
{
	var remaining = boards.ToList();
	for (var i = 0; i < draw.Length; i++)
	{
		foreach (var b in remaining.ToList())
		{
			var currentDraw = draw.Take(i + 1).ToArray();
			var (win, score) = b.Play(currentDraw);
			if (win && remaining.Count > 1)
			{
				remaining.Remove(b);
			}
			else if (win && remaining.Count == 1)
			{
				return new GameResult
				{
					Draw = string.Join(", ", currentDraw),
					Board = b,
					Score = score,
				};
			}
		}
	}
	
	return default;
}


static Board[] ParseBoards(string[] input)
{	
	Debug.Assert(input.Skip(1).Count() % 6 == 0);
	var boardCount = input.Skip(1).Count() / 6; // first line is the draw, each board is 5 lines + 1 empty line.
	
	var boards = new Board[boardCount];
	for (var i = 0; i < boardCount; i++)
	{
		var boardLines = input
			.Skip(1) 	 // draw line
			.Skip(i * 6) // seek to current board (i)
			.Skip(1)     // empty line
			.Take(5)     // board lines
			.ToArray();
		var board = Board.Parse(boardLines);
		boards[i] = board;
	}
	
	return boards;
}


class Board
{
	public static Board Parse(string[] input)
	{
		Debug.Assert(input.Length == 5);
		
		var board = new int[5,5];
		for (var y = 0; y < 5; y++)
		{
			// e.g. row: " 6 10  3 18  5" => int[] {6, 10, 3, 18, 5}
			var row = Regex.Split(input[y].Trim(), @"\s+").Select(n => int.Parse(n)).ToArray();
			Debug.Assert(row.Length == 5);

			for (var x = 0; x < 5; x++)
			{
				board[y,x] = row[x];
			}
		}
		
		return new Board
		{
			_board = board,
		};
	}
	
	public int[,] _board;
	
	public (bool, int) Play(int[] draw)
	{
		var played = new (int num, bool marked)[5, 5];
		var score = 0;

		for (var y = 0; y < 5; y++)
		for (var x = 0; x < 5; x++)
		{
			var num = _board[y, x];
			var marked = draw.Any(d => d == num);

			played[y, x] = (num, marked);
			score += marked ? 0 : num;
		}

		score *= draw.Last();

		var wins = Enumerable.Range(0,5).Any(y => Enumerable.Range(0, 5).Select(x => played[y,x]).All(b => b.marked == true))
				|| Enumerable.Range(0,5).Any(x => Enumerable.Range(0, 5).Select(y => played[y,x]).All(b => b.marked == true));
				
		return (wins, score);
	}
}

