using Godot;
using System;

public class Mushroomholder : Node2D
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
			Position += new Vector2(0, -2960);
	}

}


