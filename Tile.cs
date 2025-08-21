using Godot;
using System;
using System.Numerics;

public partial class Tile : Label
{
	public int number
	{
		get
		{
			return number;
		}
		private set
		{
			number = value;
			if (number != 0)
			{
				Text = number.ToString();
			}
			else
			{
				Text = "";
			}
		}
	}
	public Tile() 
	{
		number = 0;
	}
	private void setColor(int newNumber) 
	{
		int bit = 31 - BitOperations.LeadingZeroCount((uint)newNumber);
		StyleBoxFlat aStyleBox = (StyleBoxFlat)GetThemeStylebox("normal").Duplicate();
		float GandB = 1 - (float)bit / 16;
		aStyleBox.BgColor = new Color(1, GandB, GandB);
		AddThemeStyleboxOverride("normal", aStyleBox);
	}
	public void setNumber(int newNumber) 
	{
		number = newNumber;
		setColor(newNumber);
	}
	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
	}
}
