package finalProject;

public class Enemy extends Player
{
	
	//Constructor
	public Enemy(String n, int h, int a, int d)
	{
		super(n, h, a, d);
	}
	
	
	//create a new defend method that increases defense less when isPlayer is false
	public void defend()
	{
		super.defense += 10;
	}
	
	//act method
	public int act(Player player)
	{
		int random = 1 + (int)(Math.random() * 2);
		
		if(random == 2)
		{
			defend();
			System.out.println(getName() + " is defending!");
			return 1;
		}
		else if(player.getAlive() == true)
		{
			if(random == 1)
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
