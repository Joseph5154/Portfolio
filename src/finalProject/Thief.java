package finalProject;

import java.util.ArrayList;

public class Thief extends Player
{
	//constructor
	public Thief(String n, int h, int a, int d)
	{
		super(n, h, a, d);
	}
	
	//Sneak attack deals .75 times the damage of attack and lowers the enemy's defense
	public void sneakAttack(Enemy enemy)
	{
		enemy.health -= (int)(attack(enemy.defense) * .75);
		enemy.defense -= 10;
	}
	
	
	//act method
	public int act(String command, Enemy enemy)
	{
		if(command.equalsIgnoreCase("defend"))
		{
			defend();
			return 1;
		}
		else if(enemy.getAlive() == true)
		{
			if(command.equalsIgnoreCase("attack"))
			{
				attack(enemy);
				return 1;
			}
			else if(command.equalsIgnoreCase("sneak"))
			{
				sneakAttack(enemy);
				return 1;
			}
			else
			{
				return 0;
			}
		}
		else
		{
			return 0;
		}
	}
}
