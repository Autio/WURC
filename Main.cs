using Godot;
using System;

// DONE | Road background movement
// DONE | Road background colour updating in periods
// DONE | Hazard generation and functionality
// DONE | High score display
// DONE | Score reset when starting to ride again 
// DONE | Score display centrally when a ride is finished
// DONE | Show boosts and hazards as texts

// TODO | Progressive generation of hazards
// TODO | Better instructions
// TODO | Implement Scoreboosts
// TODO | SFX for car
// TODO | SFX for hazards
// TODO | SFX for bonuses

public class Main : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	int score;
	int baseScore;
	int highScore;
	Label scoreLabel, scoreLabel2;
	Label highscoreLabel;
	Panel scorePanel;
	Vector2 originalScorePos, originalScorePanelPos, originalScorePanelSize;

	Tween _scoreTween, _panelTween;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scoreLabel = GetNode<Label>("/root/Main/GUI/ScoreLabel");
		scoreLabel2 = GetNode<Label>("/root/Main/GUI/ScoreLabel/ScoreLabel2");
		highscoreLabel = GetNode<Label>("/root/Main/GUI/HighscoreLabel");
		scorePanel = GetNode<Panel>("/root/Main/GUI/ScorePanel");
		originalScorePos = scoreLabel.RectPosition;
		originalScorePanelPos = scorePanel.RectPosition;
		originalScorePanelSize = scorePanel.RectSize;
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

		GD.Print("Resetting score");
		baseScore = 0;
		score = 0;
		scoreLabel.Text = score.ToString("N0");

	}

	void UpdateScore()
	{
		score = baseScore;
		// add some modifiers to the score
		scoreLabel.Text = score.ToString("N0");
		if(score > highScore)
		{
			highScore = score;
			highscoreLabel.Text = "Highscore: " + score.ToString("N0");
		}
	}

	public void GameOver()
	{
		float effectTime = .5f;
		// Show the end screen
		_scoreTween = new Tween();
		_scoreTween.InterpolateProperty(scoreLabel, "rect_position", scoreLabel.RectPosition, new Vector2(0, 500), effectTime,Tween.TransitionType.Elastic);
		AddChild(_scoreTween);
		_scoreTween.Start();
		_scoreTween.Connect("tween_all_completed", _scoreTween, "queue_free");

		_panelTween = new Tween();
		_panelTween.InterpolateProperty(scorePanel, "rect_position", scorePanel.RectPosition, new Vector2(scorePanel.RectPosition.x, 500), effectTime, Tween.TransitionType.Elastic);
		AddChild(_panelTween);
		_panelTween.Start();
		_panelTween.Connect("tween_all_completed", _panelTween, "queue_free");

		_panelTween = new Tween();
		_panelTween.InterpolateProperty(scorePanel, "rect_size", scorePanel.RectSize, new Vector2(scorePanel.RectSize.x, 164), effectTime, Tween.TransitionType.Elastic);
		AddChild(_panelTween);
		_panelTween.Start();
		_panelTween.Connect("tween_all_completed", _panelTween, "queue_free");

		ShowGameOverText(effectTime - .05f);
	}

	async void ShowGameOverText(float delay = 0.5f)
	{
		var t = new Timer();
		t.WaitTime = delay;
		t.OneShot = true;
		AddChild(t);
		t.Start();
		await ToSignal(t, "timeout");
		scoreLabel2.Visible = true;

	}

	public void GameStart()
	{
		ResetScore();
		// Show the end screen
		_scoreTween = new Tween();
		_scoreTween.InterpolateProperty(scoreLabel, "rect_position", scoreLabel.RectPosition, originalScorePos, 0.2f);
		AddChild(_scoreTween);
		_scoreTween.Start();
		_scoreTween.Connect("tween_all_completed", _scoreTween, "queue_free");

		_panelTween = new Tween();
		_panelTween.InterpolateProperty(scorePanel, "rect_position", scorePanel.RectPosition, originalScorePanelPos, .2f); ;
		AddChild(_panelTween);
		_panelTween.Start();
		_panelTween.Connect("tween_all_completed", _panelTween, "queue_free");

		_panelTween = new Tween();
		_panelTween.InterpolateProperty(scorePanel, "rect_size", scorePanel.RectSize, originalScorePanelSize, .25f);
		AddChild(_panelTween);
		_panelTween.Start();
		_panelTween.Connect("tween_all_completed", _panelTween, "queue_free");

		scoreLabel2.SetVisible(false);
	}
}
