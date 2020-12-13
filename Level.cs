using Godot;
using System;

public class Level : Node2D
{
	// Generate the map background and objects
	[Export]
	public PackedScene Bonus, Hazard, Backstripe;
	public float bonusRatio = 0.2f;

	private Vector2 _screenSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_screenSize = GetViewport().Size;
	}
	// How often do bonuses appear as opposed to hazards?

	private Random _random = new Random();


	public override void _Process(float delta)
	{
		if (Input.IsActionPressed("Spawn"))
		{

			CreateLevelThing();

		}
	}

	private float RandRange(float min, float max)
	{
		return (float)_random.NextDouble() * (max - min) + min;
	}

	public void CreateLevelThing()
	{
		float roll = RandRange(0.0f, 1.0f);

		// Needs to be way ahead of the player and should be triggered when a hazard / obstacle is hit 
		// OR when it goes out of the map bounds

		// Randomize which object to create
		if (roll < bonusRatio)
		{
			// Instantiate bonus
			var bonusInstance = (RigidBody2D)Bonus.Instance();
			AddChild(bonusInstance);
			float cameraY = GetNode<Camera2D>("/root/Main/Player/Camera2D").Position.y;
			bonusInstance.Position = new Vector2(RandRange(40, _screenSize.x - 40), cameraY - RandRange(1280,1880));

		}
		else if (roll >= bonusRatio)
		{
			// Instantiate hazard
			var hazardInstance = (RigidBody2D)Hazard.Instance();
			AddChild(hazardInstance);
			float cameraY = GetNode<Camera2D>("/root/Main/Player/Camera2D").Position.y;
			hazardInstance.Position = new Vector2(RandRange(40, _screenSize.x - 40), cameraY - RandRange(1280, 1880));

		}

	}

	public void CreateBonus()
	{
			// Instantiate bonus
			var bonusInstance = (RigidBody2D)Bonus.Instance();
			AddChild(bonusInstance);
			float player = GetNode<Player>("/root/Main/Player").Position.y;
			bonusInstance.Position = new Vector2(RandRange(60, _screenSize.x - 60), player - RandRange(1280,1880));

	}

	public void AddBackgroundShapes()
	{
		var backstripe = (Sprite)Backstripe.Instance();
		AddChild(backstripe);
		float playerPos = GetNode<Area2D>("/root/Main/Player").Position.y;
		GD.Print(playerPos);
		backstripe.Position = new Vector2(_screenSize.x/2, playerPos - RandRange(800, 900));

	}
}
