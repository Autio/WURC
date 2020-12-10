using Godot;
using System;

public class Main : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	int score;
	int baseScore;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	
	public void SetBaseScore(int s)
	{
		baseScore = s;

	}
	
	void UpdateScore()
	{
		
	}
}
