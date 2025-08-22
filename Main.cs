//Created by Vivanto(GitHub:Vivanto-Libera)(E-Mail:1425078256@qq.com)

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
	public async void _on_board_game_over() 
	{
		GetNode<Label>("GameOver").Show();
		Timer timer = GetNode<Timer>("Timer");
		timer.Start();
		await ToSignal(timer, Timer.SignalName.Timeout);
		GetNode<Label>("GameOver").Hide();
		GetNode<Board>("Board").GameStart();
		score = 0;
		GetNode<Label>("Score").Text = score.ToString();
	}
}
