using Godot;
using System;

// TODO | Road background movement
// DONE | Road background colour updating in periods
// TODO | Hazard generation and functionality
// TODO | High score display
// TODO | Score reset when starting to ride again 

public class Main : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	int score;
	int baseScore;
	int highScore;
	Label scoreLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scoreLabel = GetNode<Label>("/root/Main/GUI/ScoreLabel");
	}

	
	public void SetBaseScore(int s)
	{
		baseScore = s;
		if(baseScore < 0)
		{
			baseScore = 0;
		}
		UpdateScore();
	}
	
	public void ResetScore()
	{
		baseScore = 0;
		score = 0;
	}
	void UpdateScore()
	{
		score = baseScore;
		// add some modifiers to the score
		scoreLabel.Text = score.ToString("N0");
		if(score > highScore)
		{
			highScore = score;
		}
	}
}
