package finalProject;

public class Warrior extends Player
{
	private boolean canAct;
	
	//Constructor
	public Warrior(String n, int h, int a, int d)
	{
		super(n, h, a, d);
		canAct = true;
	}
	
	//setter method for canAct
	public void setCanAct(boolean x)
	{
		canAct = x;
	}
	
	//Getter method for canAct
	public boolean getCanAct()
	{
		return canAct;
	}
	
	//Wide Slash takes an array of Enemies and divides the damage from attack by length of the array
	public void wideSlash(Enemy[] enemies)
	{
		for(int x = 0; x < enemies.length; x++)
		{
			if(enemies[x] != null) 
			{
				enemies[x].health -= (int)(attack(enemies[x].defense)/enemies.length);
			}
		}
	}
	
	//Power Slash does 2.5 times the damage of attack, but makes it so the warrior cannot act next turn
	public void powerSlash(Enemy enemy)
	{
		canAct = false;
		enemy.health -= (int)(attack(enemy.defense) * 2.5);
		this.defense -= 15;
	}
	
	//act method
	public int act(String command, Enemy enemy, Enemy[] enemies)
	{
		boolean rowAlive = false;
		for(int x = 0; x < enemies.length; x++)
		{
			if(enemies[x].getAlive() == true)
			{
				rowAlive = true;
			}
		}
		
		if(rowAlive == true && command.equalsIgnoreCase("wide"))
		{
			wideSlash(enemies);
			return 1;
		}
		else if(command.equalsIgnoreCase("defend"))
		{
			defend();
			return 1;
		}
		else if(canAct == true && enemy.getAlive() == true && rowAlive == true)
		{
			if(command.equalsIgnoreCase("attack"))
			{
				attack(enemy);
				return 1;
			}
			else if(command.equalsIgnoreCase("power"))
			{
				powerSlash(enemy);
				return 1;
			}
			else
			{
				return 0;
			}
		}
		else if(canAct == false)
		{
			canAct = true;
			System.out.println("You cannot act!");
			return 1;
		}
		else
		{
			return 0;
		}
	}
}
