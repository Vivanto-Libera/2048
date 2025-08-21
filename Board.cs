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

	private void GameStart() 
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
				string row = new string("");
				switch (i) 
				{
					case 1:
						row = "A";
						break;
					case 2:
						row = "B";
						break;
					case 3:
						row = "C";
						break;
					case 4:
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
	}
}
