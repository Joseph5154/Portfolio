package finalProject;

public class Mage extends Player
{
	private int magic;
	
	//constructor
	public Mage(String n, int h, int a, int d, int m)
	{
		super(n, h, a, d);
		magic = m;
	}
	
	//Mage has its own version of attack that uses the magic stat instead and does not take enemy defense into account
	public int attack()
	{
		return (int)(magic + (.25 * (int)(magic * Math.random())));
	}
	
	public void attack(Enemy enemy)
	{
		enemy.health -= (int)(magic + (.25 * (int)(magic * Math.random())));
	}
	
	//Mage has an attack, fire, that takes a number, then finds the sum of that number and all numbers before it, and multiplies that by 10, which is the damage dealt
	public int fire(int x)
	{
		if(x == 0)
		{
			return 0;
		}
		else
		{
			return x + fire(x-1);
		}
	}
	
	//Method that inputs a random number between 1 and 9 into fire and deals the damage it returns to the enemy
	public void fire(Enemy enemy)
	{
		enemy.health -= fire(1 + ((int)(Math.random() * 9))) * 10;
	}
	
	//Have an explosion attack that does the same thing as Mage's attack,
	//but takes a 2D array, and divides the damage by the number of elements in the array
	public void explosion(Enemy[][] enemies)
	{
		int numEnemies = enemies.length * enemies[0].length;
		for(int row = 0; row < enemies.length; row++)
		{
			for(int col = 0; col < enemies[row].length; col++)
			{
				if(enemies[row][col] != null)
				{
					enemies[row][col].health -= (int)(attack()/numEnemies);
				}
			}
		}
	}
	
	//act method
	public int act(String command, Enemy enemy, Enemy[][] enemies)
	{
		if(command.equalsIgnoreCase("explosion"))
		{
			explosion(enemies);
			return 1;
		}
		else if(command.equalsIgnoreCase("defend"))
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
			else if(command.equalsIgnoreCase("fire"))
			{
				fire(enemy);
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
