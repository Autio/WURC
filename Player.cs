using Godot;
using System;

public class Player : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	float power = 0;
	bool charging = false;
	bool driving = false;
	Vector2 velocity = new Vector2();
	
	public override void _Process(float delta)
	{

		//var velocity = new Vector2(); // The player's movement vector.

		if (Input.IsActionPressed("ui_down"))
		{
			if(!driving) 
			{
				charging = true;
			}
			if(charging)
			{
				power += 1.0f;
				// velocity.y += 1;
				GD.Print(power);
			}
		}
		

		
		
		if(Input.IsActionJustReleased("ui_down"))
		{
			GD.Print("Released");
			driving = true;
		}
		if(driving)
		{
			velocity = new Vector2(0, power * 60);
			Position -= velocity * delta;
			power -= 1.0f;
			GD.Print(Position);
			GD.Print(velocity);
			if(velocity.Length() <= 0)
			{
				driving = false;
			}
		}
		
	}
	public override void _UnhandledInput(InputEvent @event){
		if (@event is InputEventMouseButton){
			InputEventMouseButton emb = (InputEventMouseButton)@event;
			if(!driving)
			{
				if (emb.IsPressed()){
					if(Input.IsActionPressed("middle_button")){
						charging = true;
						if (emb.ButtonIndex == (int)ButtonList.WheelUp){
							power += 1.0f;
						}
						if (emb.ButtonIndex == (int)ButtonList.WheelDown){
							power -= 1.0f;
						}
					}
					GD.Print(power);
				}
				if(charging && Input.IsActionJustReleased("middle_button"))
				{
					GD.Print("Driving");
					driving = true;
					charging = false;
				}
			}
		}
	}
}
