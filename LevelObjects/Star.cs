using Godot;
using System;

public class Star : RigidBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	private void _on_VisibilityNotifier2D_screen_exited()
	{
		QueueFree();
	}
	
	private void _on_Area2D_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
	{

		QueueFree();
	}
}










