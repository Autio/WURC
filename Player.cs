using Godot;
using System;

public class Player : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private Vector2 _screenSize;
	private float drag_margin_top = 0.7f;
	private float top_limit = 1000000;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_screenSize = GetViewport().Size;
	}

	float power = 0;
	float lateral_speed = 500.0f; // How fast can the car go from side to side
	
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
		
		// Allow left and right movement whilst driving
		if(driving)
		{
			if (Input.IsActionPressed("ui_left"))
			{
				power += 0.16f;
				Position -= new Vector2(lateral_speed * delta, 0);
				
			}
			
			if (Input.IsActionPressed("ui_right"))
			{
				power += 0.16f;
				Position += new Vector2(lateral_speed * delta, 0);
			}
		}
		Position = new Vector2(
			x: Mathf.Clamp(Position.x, 0, _screenSize.x),
			y: Position.y
		) ;

		if (Input.IsActionJustReleased("ui_down"))
		{
			GD.Print("Released");
			driving = true;
		}
		if(driving)
		{
			velocity = new Vector2(0, power * 60);
			Position -= velocity * delta;
			power -= (0.5f + power / 250f);
			if ( power < 1.0f)
			{
				power = 0;
			}
			GD.Print(Position);
			GD.Print(velocity);
			if(velocity.Length() <= 0 || velocity.y < 0)
			{
				driving = false;
				
			}
		}
		
	}

	// Handle middle button
	public override void _UnhandledInput(InputEvent @event){
		if (@event is InputEventMouseButton){
			InputEventMouseButton emb = (InputEventMouseButton)@event;
			if(!driving)
			{
				if (emb.IsPressed()){
					if(Input.IsActionPressed("middle_button")){
						charging = true;
						if (emb.ButtonIndex == (int)ButtonList.WheelUp){
							power += 10.0f;
						}
						if (emb.ButtonIndex == (int)ButtonList.WheelDown){
							power -= 10.0f;
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

	public void _UpdateViewport()
	{
		Transform2D canvas_transform = GetViewport().CanvasTransform;
		canvas_transform.origin = -GetGlobalPosition() + _screenSize / 2.0f;
		GetViewport().SetCanvasTransform(canvas_transform);
		GD.Print(canvas_transform);
	}

private void _on_Player_body_entered(object body)
{
	if (body is RigidBody2D)
	{
			var bodies = GetOverlappingBodies();
			foreach (var b in bodies)
			{
				if(b is Hazard)
				{
					GD.Print("Hazard hit");
					// Do something to the player
					
					// Reduce power
					power /= 4;
					
					// Show some kind of effect
				}
			}
	}
		
}

}

