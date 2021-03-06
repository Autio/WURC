using Godot;
using System;

public class Car : RigidBody2D
{
	[Export]
	public Texture[] carSprites; 
	
	public bool goingUp = false;
	float speed;
	Vector2 velocity;

	public void SetDirection(bool d)
	{
		goingUp = d;
	}
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		speed = GetNode<Level>("/root/Main/Level").RandRange(100, 200);
		
		GetNode<Sprite>("Sprite").Texture = carSprites[(int)GetNode<Level>("/root/Main/Level").RandRange(0, carSprites.Length)];
		
		
		GD.Print(this.Position.x);
		if(Position.x > 0)
		{
			GD.Print("Going up");

			goingUp = true;
		}

		if(goingUp)
		{
			velocity = new Vector2(0, -speed);
		} else
		{
			velocity = new Vector2(0, speed);
		}
	}

	public override void _Process(float delta)
	{

		Position += velocity * delta;

	}

	private void _on_VisibilityNotifier2D_screen_exited()
	{	
		GetNode<Level>("/root/Main/Level").UpcomingCars.Remove(this);
		// Replace with function body.
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      	GetNode<Level>("/root/Main/Level").UpcomingHazards.Remove(this);
//  }
	private void _on_Area2D_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
	{
		GetNode<Level>("/root/Main/Level").UpcomingCars.Remove(this);
		QueueFree();
	}

}






