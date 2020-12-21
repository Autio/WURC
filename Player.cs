using Godot;
using System;

public class Player : Area2D
{
	

	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export]
	public PackedScene BonusEffect, HazardEffect;
	
	private Vector2 _screenSize;
	private float drag_margin_top = 0.7f;
	private float top_limit = 1000000;
	int ticker = 0; // how many things have been hit

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_screenSize = GetViewport().Size;
	}

	public float power = 0;
	float lateral_speed = 500.0f; // How fast can the car go from side to side
	public float distance = 0;
	
	bool charging = false;
	bool driving = false;
	bool creating = true;
	bool exhaust = true;
	Vector2 velocity = new Vector2();
	
	public override void _Process(float delta)
	{
		//DrawLine(Position, new Vector2(Position.x, Position.y + 100), new Color(255, 0, 0), 1);
		// Set the basic player score on the 
		

		if (Input.IsActionPressed("ui_down"))
		{
			if(!driving) 
			{
				charging = true;
			}
			if(charging)
			{
				if (power < 150)
				{
					power += 1.0f;
				}
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
			driving = true;
			charging = false;
			if (!exhaust)
			{
				ToggleExhaust();
			}
			GetNode<Main>("/root/Main").GameStart();
		}
		if (driving)
		{

			GetNode<Main>("/root/Main").SetBaseScore((int)distance);
			velocity = new Vector2(0, power * 60);
			Position -= velocity * delta;
			// Extra drag if on the side. Note: Fixed locations, better if dynamic
			if(Position.x < 60 || Position.x > 660)
			{
				power -= (0.1f + power / 150f);
			}

			if (power > 75)
			{
				GetNode<Sprite>("Shield").Visible = true;
			} else
			{
				GetNode<Sprite>("Shield").Visible = false;
			}
			
			
			if(power > 90)
			{
				power -= (0.5f + power / 150f);

			} else if (power > 20)
			{
				power -= (0.08f + power / 1000f);
			}
			else
			{
				power -= (0.105f + power / 750f);
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
					if (GetNode<Level>("/root/Main/Level").RandRange(0, 10) > 4)
					{ 
						GetNode<Level>("/root/Main/Level").CreateBonus();
					}
					if(dist > 300)
					{
						if (GetNode<Level>("/root/Main/Level").RandRange(0, 10) > 9)
						{
							GetNode<Level>("/root/Main/Level").CreateHazard();
						}
					}
					else if (dist > 1000)
					{
						if (GetNode<Level>("/root/Main/Level").RandRange(0, 10) > 6)
						{
							GetNode<Level>("/root/Main/Level").CreateHazard();
						}
					} else if (dist > 6000)
					{
						if (GetNode<Level>("/root/Main/Level").RandRange(0, 10) > 3)
						{
							GetNode<Level>("/root/Main/Level").CreateHazard();
						}
					}
										
					// Don't create cars at the start. But then later on create more
					if (dist > 2000)
					{
						int limit = 7;
						if( dist > 6000)
						{
							limit = 5;
						}

						if (dist > 20000)
						{
							limit = 1;
						}
						if (GetNode<Level>("/root/Main/Level").RandRange(0, 10) > limit)
						{
							GetNode<Level>("/root/Main/Level").CreateCar();

						}
					}

					if (GetNode<Level>("/root/Main/Level").RandRange(0, 10) > 2)
					{
						int lineLength = 3;
						int roll = (int)GetNode<Level>("/root/Main/Level").RandRange(0, 10);
						if (roll > 5)
						{
							lineLength = 5;
						} 
						if (roll > 8)
						{
							lineLength = 7;
						}

						int stagger = (int)GetNode<Level>("/root/Main/Level").RandRange(0, 3);

						GetNode<Level>("/root/Main/Level").CreateStarLine(lineLength, stagger);

					}
					creating = false;
				}
			} else
			{
				creating = true;
			}


			// When the vehicle has stopped
			if (velocity.Length() <= 0 || velocity.y < 0)
			{
				// The game is over
				GetNode<Main>("/root/Main").GameOver();
				GD.Print("Stopped driving");
				driving = false;
				distance = 0;
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
				GetNode<Main>("/root/Main").GameStart();

				if (emb.IsPressed()){
					if(Input.IsActionPressed("middle_button")){
						charging = true;
						if (emb.ButtonIndex == (int)ButtonList.WheelUp){
							power += 6.0f;
						}
						if (emb.ButtonIndex == (int)ButtonList.WheelDown){
							power -= 6.0f;
						}
					}
					GD.Print(power);
				}
				if(charging && Input.IsActionJustReleased("middle_button"))
				{
					GD.Print("Driving");
					driving = true;
					charging = false;
					if(!exhaust)
					{
						ToggleExhaust();
					}
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
	}

private void _on_Player_body_entered(object body)
{
	if (body is RigidBody2D)
	{
			if (driving)
			{
				var bodies = GetOverlappingBodies();
				foreach (var b in bodies)
				{
					if (b is Hazard)
					{
						GD.Print("Hazard hit");

						if (power < 100)
						{
							power *= 0.80f;
							// var hazardInstance = (Label)BonusEffect.Instance();
							// AddChild(hazardInstance);
							GetNode<AudioStreamPlayer>("/root/Main/HazardSound").Play();

						}
						// Show some kind of effect
					}
					if (b is Bonus)
					{
						GD.Print("Bonus hit");
						// Only do if power is low
						if (power < 150)
						{
							GetNode<Main>("/root/Main").AddToOtherScore(200000);

							var bonusInstance = (Label)BonusEffect.Instance();
							bonusInstance.SetPosition(new Vector2(bonusInstance.GetPosition().x, bonusInstance.GetPosition().y + 100));
							AddChild(bonusInstance);
							power += 15f;
							GetNode<CPUParticles2D>("BonusParticles").Emitting = true;
							GetNode<AudioStreamPlayer>("/root/Main/BonusSound").Play();

						}

						// Show some kind of effect
					}
					if (b is Car)
					{
						// Only do if power is low
						if (power < 100)
						{
							GD.Print("Car hit");
							var hazardInstance = (Label)HazardEffect.Instance();
							hazardInstance.SetPosition(new Vector2(hazardInstance.GetPosition().x, hazardInstance.GetPosition().y + 100));

							AddChild(hazardInstance);
							power *= 0.6f;
							GetNode<CPUParticles2D>("CollisionParticles").Emitting = true;
							GetNode<AudioStreamPlayer>("/root/Main/CarSound").Play();


						}

						// Show some kind of effect
					}

					if (b is Star)
					{
						// Only do if power is low
						if (power < 75)
						{	
							GD.Print("Star hit");
							ticker++;
							if(ticker % 2 == 0)
							{
								//GetNode<AudioStreamPlayer>("/root/Main/StarSounds").Stop();
								//GetNode<AudioStreamPlayer>("/root/Main/StarSounds").Stream = GetNode<StarSounds>("/root/Main/StarSounds").starsounds[(int)GetNode<Level>("/root/Main/Level").RandRange(0, GetNode<StarSounds>("/root/Main/StarSounds").starsounds.Length)];
								GetNode<AudioStreamPlayer>("/root/Main/StarSounds").Play();
							} else
							{
								GetNode<AudioStreamPlayer>("/root/Main/StarSounds2").Play();
							}
							//var bonusInstance = (Label)HazardEffect.Instance();
							// AddChild(bonusInstance);
							
							GetNode<Main>("/root/Main").AddToOtherScore(20000);
							power += 2f;
							GetNode<CPUParticles2D>("StarParticles").Emitting = true;
						}
					}
				}
			}
	}
		
}

}

