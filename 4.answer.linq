<Query Kind="Program" />

void Main()
{
	var file = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "4.sample.txt");
	var input = File.ReadAllLines(file);
	
	//var draw = input.First().Split(',').Select(s => int.Parse(s)).ToArray();
	
	var draw = "7,4,9,5,11,17,23,2,0,14,21,19,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1".Split(',').Select(s => int.Parse(s)).ToArray();
	
	var boards = ParseBoards(input);
	
	var (winningDraw, winningBoard) = Play(draw, boards);
	
	var finalScore = winningBoard.Score(winningDraw);
	
	Debug.Assert(finalScore == 4512);
}


static (int[], Board) Play(int[] draw, Board[] boards)
{
	for (var i = 0; i < draw.Length; i++)
	{
		foreach (var b in boards)
		{
			var currentDraw = draw.Take(i + 1).ToArray();
			var win = b.Wins(currentDraw);
			if (win)
			{
				return (currentDraw, b);
			}
		}
	}

	return (draw, null);
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
		var board = input.Select(i => Regex.Split(i.Trim(), @"\s+").Select(i => int.Parse(i)).ToArray()).ToArray();
		Debug.Assert(board.Length == 5);
		Debug.Assert(board.All(row => row.Length == 5));

		return new Board
		{
			board = board,
		};
	}
	
	public int[][] board;
	
	private bool[,] Mark(int[] draw)
	{
		var marked = new bool[5,5];
		
		for (var x = 0; x < 5; x++)
		for (var y = 0; y < 5; y++)
		foreach (var d in draw)
		{
			if (board[x][y] == d)
			{
				marked[x,y] = true;
				break;
			}
		}
		
		return marked;
	}
		
	public bool Wins(int[] draw)
	{
		var marked = Mark(draw);
		
		Enumerable.Range(0,5)
	}

	public int Score(int[] draw)
	{
		var score = 0;
		
		var marked = Mark(draw);
		for (var x = 0; x < 5; x++)
		for (var y = 0; y < 5; y++)
		{
			if (!marked[x][y])
			{
				score += board[x][y];
			}
		}
		
		score *= draw.Last();
		
		return score;
	}

	static bool AllTrue(bool[] bools) => bools.All(b => b == true);
}

