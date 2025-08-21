using Godot;
using System;
using System.Net.Sockets;

public partial class Main : Node
{
	public int score = 0;
	public void _on_board_merged(int newNumber) 
	{
		score += newNumber;
		GetNode<Label>("Score").Text = score.ToString();
	}
	public void _on_board_game_over() 
	{
		GetNode<Board>("Board").GameStart();
		score = 0;
		GetNode<Label>("Score").Text = score.ToString();
	}
}
