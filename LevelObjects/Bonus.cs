using Godot;
using System;

public class Bonus : RigidBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      

   	private void _on_Bonus_body_entered(object body)
	{
		GD.Print("EntereD");
		
		if(body is Area2D)
		// Replace with function body.
		{
			Transform.Scaled(new Vector2(3,3));
		}
	}

	// Let the Level know that the thing is now visible by removing this from the upcoming bonuses list
	private void _on_VisibilityNotifier2D_screen_entered()
	{
		GetNode<Level>("/root/Main/Level").UpcomingBonuses.Remove(this);
	}


	private void _on_VisibilityNotifier2D_screen_exited()
	{
		
		QueueFree();
	}

private void _on_Area2D_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
{
		GetNode<Level>("/root/Main/Level").UpcomingBonuses.Remove(this);
		QueueFree();
}
}







