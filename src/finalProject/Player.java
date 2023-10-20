package finalProject;

public class Player 
{
	//Attributes for player to be used for each Class
	private String name;
	private int maxHealth;
	public int health;
	private int attack;
	private int originalDef;
	public int defense;
	private boolean alive;
	
	//Constructor
	public Player(String n, int h, int a, int d)
	{
		name = n;
		maxHealth = h;
		health = h;
		attack = a;
		originalDef = d;
		defense = d;
		alive = true;
	}
	
	//player methods
	//setter method for alive and defense variables
	public void setAlive(boolean x)
	{
		alive = x;
	}
	
	public void setDefense(int x)
	{
		defense = x;
	}
	
	//getter methods for all variables
	public String getName()
	{
		return name;
	}
	
	public int getAttack()
	{
		return attack;
	}
	
	public boolean getAlive()
	{
		return alive;
	}
	
	public int getMaxHealth()
	{
		return maxHealth;
	}
	
	//attack method returns the damage that will be dealt to an enemy which is the player's attack stat 
	//plus the 1/4 of the player's attack stat multiplied by a random number minus the enemy's defense stat
	public int attack(int enemyDefense)
	{
		return (int)(attack + (.25 * (int)(attack * (1 + Math.random())) - enemyDefense/2));
	}
	
	//attack method that subtracts the damage returned from attack from the Enemy's health
	public void attack(Player player)
	{
		player.health -= attack(player.defense);
	}
	
	//create a defend method that increases the Player's defense
	public void defend()
	{
		defense += 20;
	}
	
	//create a method that returns defense to normal
	public void stopDefend()
	{
		defense = originalDef;
	}
	
	//method that checks if the Player has less than or equal to 0 health, and sets alive to false if it is.
	public void checkIfAlive()
	{
		if(health <= 0 && alive == true)
		{
			alive = false;
			System.out.println(name + " is down!");
		}
	}
	
	//Create a method that inputs a string, and if the string matches the name of a command, that command is performed and 1 is returned
	//and if it does not, then 0 is returned. This will be used and slightly modified for each class
	public int act(String command, Player player)
	{
		if(command.equalsIgnoreCase("defend"))
		{
			defend();
			return 1;
		}
		else if(player.getAlive() == true)
		{
			if(command.equalsIgnoreCase("attack"))
			{
				attack(player);
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
