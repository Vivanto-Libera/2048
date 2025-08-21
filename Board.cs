using Godot;
using System;
using System.Reflection;

public partial class Board : Node
{
	[Signal]
	public delegate void MergedEventHandler(int newNumber);
	[Signal]
	public delegate void GameOverEventHandler();

	private Tile[,] tiles = new Tile[4, 4];
	private int emptyTileNumber = 16;

	public void GameStart() 
	{
		ClearTiles();
		CreatTile();
		CreatTile();
	}
	private void ClearTiles() 
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				tiles[i, j].setNumber(0);
			}
		}
		emptyTileNumber = 16;
	}
	private void CreatTile() 
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		int index = rng.RandiRange(1, emptyTileNumber);
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				if (tiles[i, j].number != 0) 
				{
					continue;
				}
				index--;
				if(index == 0) 
				{
					tiles[i, j].setNumber(2);
					emptyTileNumber--;
				}
			}
		}
	}
	private int[] MoveLine(int a, int b, int c, int d) 
	{
		if (d == 0)
		{
			if (c == 0)
			{
				if (b == 0)
				{
					if (a != 0)
					{
						d = a;
						a = 0;
					}
				}
				else
				{
					d = b;
					c = a;
					a = 0;
					b = 0;
				}
			}
			else
			{
				d = c;
				c = b;
				b = a;
				a = 0;
			}
		}
		if (c == 0)
		{
			if (b == 0)
			{
				if (a != 0)
				{
					c = a;
					a = 0;
				}
			}
			else
			{
				c = b;
				b = a;
				a = 0;
			}
		}
		if (b == 0)
		{
			if (a != 0)
			{
				b = a;
				a = 0;
			}
		}
		if (d == c && d != 0)
		{
			d = c << 1;
			emptyTileNumber++;
			EmitSignal(SignalName.Merged, d);
			if (a == b && a != 0)
			{
				c = a << 1;
				a = 0;
				b = 0;
				emptyTileNumber++;
				EmitSignal(SignalName.Merged, c);
			}
			else
			{
				c = b;
				b = a;
				a = 0;
			}
		}
		else if (c == b && c != 0)
		{
			c = b << 1;
			b = a;
			a = 0;
			emptyTileNumber++;
			EmitSignal(SignalName.Merged, c);
		}
		else if (b == a && b != 0)
		{
			b = a << 1;
			a = 0;
			emptyTileNumber++;
			EmitSignal(SignalName.Merged, b);
		}
		GD.Print(a, b, c, d);
		int[] newLine = { a, b, c, d };
		return newLine;
	}
	private void MoveUp() 
	{
		for(int i = 0; i < 4; i++) 
		{
			int[] newLine = MoveLine(tiles[3, i].number, tiles[2, i].number,
									 tiles[1, i].number, tiles[0, i].number);
			for (int j = 3; j >= 0; j--)
			{
				tiles[j, i].setNumber(newLine[3 - j]);
			}
		}
	}
	private void MoveDown()
	{
		for (int i = 0; i < 4; i++)
		{
			int[] newLine = MoveLine(tiles[0, i].number, tiles[1, i].number,
									 tiles[2, i].number, tiles[3, i].number);
			for (int j = 3; j >= 0; j--)
			{
				tiles[j, i].setNumber(newLine[j]);
			}
		}
	}
	private void MoveLeft()
	{
		for (int i = 0; i < 4; i++)
		{
			int[] newLine = MoveLine(tiles[i, 3].number, tiles[i, 2].number,
									 tiles[i, 1].number, tiles[i, 0].number);
			for (int j = 3; j >= 0; j--)
			{
				tiles[i, j].setNumber(newLine[3 - j]);
			}
		}
	}
	private void MoveRight()
	{
		for (int i = 0; i < 4; i++)
		{
			int[] newLine = MoveLine(tiles[i, 0].number, tiles[i, 1].number,
									 tiles[i, 2].number, tiles[i, 3].number);
			for (int j = 3; j >= 0; j--)
			{
				tiles[i, j].setNumber(newLine[j]);
			}
		}
	}

	private void IsLose() 
	{
		if (emptyTileNumber != 0)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				if (i < 3)
				{
					if (tiles[i, j] == tiles[i + 1, j])
					{
						return;
					}
				}
				if (j < 3)
				{
					if (tiles[i, j] == tiles[i, j + 1])
					{
						return;
					}
				}
			}
		}
		EmitSignal(SignalName.GameOver);
	}
	public override void _Ready()
	{
		initTiles();
		GameStart();
	}
	private void initTiles() 
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				string row = "";
				switch (i) 
				{
					case 0:
						row = "A";
						break;
					case 1:
						row = "B";
						break;
					case 2:
						row = "C";
						break;
					case 3:
						row = "D";
						break;
				}
				string column = j.ToString();
				string tile = row + column;
				tiles[i, j] = GetNode<Tile>(tile);
			}
		}
	}
	public override void _Process(double delta)
	{
		if (Input.IsActionJustReleased("Up"))
		{
			MoveUp();
			CreatTile();
			IsLose();
		}

		if (Input.IsActionJustReleased("Down"))
		{
			MoveDown();
			CreatTile();
			IsLose();
		}

		if (Input.IsActionJustReleased("Left"))
		{
			MoveLeft();
			CreatTile();
			IsLose();
		   
		}

		if (Input.IsActionJustReleased("Right"))
		{
			MoveRight();
			CreatTile();
			IsLose();
		}
	}
}
