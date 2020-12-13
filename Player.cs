using Godot;
using System;

public class Player : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export]
	public PackedScene BonusEffect;
	
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
	float distance = 0;
	
	bool charging = false;
	bool driving = false;
	bool creating = true;
	bool exhaust = true;
	Vector2 velocity = new Vector2();
	
	public override void _Process(float delta)
	{
		//DrawLine(Position, new Vector2(Position.x, Position.y + 100), new Color(255, 0, 0), 1);
		// Set the basic player score on the 
		GetNode<Main>("/root/Main").SetBaseScore(-(int)Position.y);

		if (Input.IsActionPressed("ui_down"))
		{
			if(!driving) 
			{
				charging = true;
			}
			if(charging)
			{
				power += 1.0f;
			}
		}
		
		// Allow left and right movement whilst driving
		if(driving)
		{
			if (Input.IsActionPressed("ui_left"))
			{
				power += 0.08f;
				Position -= new Vector2(lateral_speed * delta, 0);
				
			}
			
			if (Input.IsActionPressed("ui_right") && !Input.IsActionPressed("ui_left"))
			{
				power += 0.08f;
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
		if (driving)
		{
			velocity = new Vector2(0, power * 60);
			Position -= velocity * delta;
			if(power > 70)
			{
				power -= (0.5f + power / 150f);

			} else
			{
				power -= (0.16f + power / 750f);
			}
			if (power < 1.0f)
			{
				power = 0;
			}
			
			// Keep track of the distance traveled
			distance += velocity.y;
			int dist = (int)distance / 1000;

			// Generate level elements when a certain amount of distance has passed			
			if (dist % 50 == 0)
			{
				if (creating)
				{
					GD.Print("Creating bonus");
					GetNode<Level>("/root/Main/Level").CreateBonus();
					creating = false;
				}
			} else
			{
				creating = true;
			}


			// When the vehicle has stopped
			if (velocity.Length() <= 0 || velocity.y < 0)
			{
				driving = false;
				ToggleExhaust();

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
							power += 7.0f;
						}
						if (emb.ButtonIndex == (int)ButtonList.WheelDown){
							power -= 7.0f;
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

	void ToggleExhaust()
	{
		exhaust = !exhaust;

		foreach (Node n in this.GetChildren())
		{
			if (n is CPUParticles2D)
			{
				CPUParticles2D p = (CPUParticles2D)n;
				p.SetEmitting(exhaust);

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
					//b.Transform.Scaled(new Vector2(3, 3));
					// Reduce power
					//power /= 4;
					
					// Show some kind of effect
				}
				if(b is Bonus)
				{
					GD.Print("Bonus hit");
					var bonusInstance = (Label)BonusEffect.Instance();
					AddChild(bonusInstance);
					power += 25f;
					// Do something to the player
					//b.Transform.Scaled(new Vector2(3, 3));
					// Reduce power
					//power /= 4;
					
					// Show some kind of effect
				}
			}
	}
		
}

}

