package finalProject;

import java.util.ArrayList;

public class Cleric extends Player
{
	//constructor
	public Cleric(String n, int h, int a, int d)
	{
		super(n, h, a, d);
	}
	
	//Create a heal method that takes an ArrayList of players and adds 100 to the health of each, and sets their health to their maxHealth is they exceed it.
	public void heal(ArrayList<Player> team)
	{
		for(int x = 0; x < team.size(); x++)
		{
			if(team.get(x).health > 0)
			{
				team.get(x).health += 100;
				if(team.get(x).health > team.get(x).getMaxHealth())
				{
					team.get(x).health = team.get(x).getMaxHealth();
				}
			}
		}
	}
	
	//Create a banish method that has a 1 in 5 chance of instantly killing an enemy
	public void banish(Enemy enemy)
	{
		int num = 1 + (int)(Math.random()*5);
		if(num == 5)
		{
			enemy.health = 0;
		}
		else
		{
			System.out.println("It failed...");
		}
	}
	
	//act method
	public int act(String command, Enemy enemy, ArrayList<Player> team)
	{
		if(command.equalsIgnoreCase("defend"))
		{
			defend();
			return 1;
		}
		else if(command.equalsIgnoreCase("heal"))
		{
			heal(team);
			return 1;
		}
		else if(enemy.getAlive() == true)
		{
			if(command.equalsIgnoreCase("attack"))
			{
				attack(enemy);
				return 1;
			}
			else if(command.equalsIgnoreCase("banish"))
			{
				banish(enemy);
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
