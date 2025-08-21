using Godot;
using System;
using System.Numerics;

public partial class Tile : Label
{
	public int number
	{
		get;
		private set;
	}
	public Tile() 
	{
		setNumber(0);
	}
	private void setText(int num) 
	{
		if(num != 0)

			{
			Text = num.ToString();
		}
		else
		{
			Text = "";
		}
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
		setText(newNumber);
	}
	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
	}
}
