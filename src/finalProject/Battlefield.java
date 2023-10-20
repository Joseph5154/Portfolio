//Joseph Davidson
//Period 6/7
package finalProject;
import java.util.*;
public class Battlefield 
{	
	//Create a method for the enemies to decide who to attack randomly and determine if they can attack
	public static void enemyAttack(ArrayList<Player> team, Enemy enemy)
	{
		if(enemy.getAlive() == true)
		{
			int target = (int)(Math.random() * team.size());
			while(enemy.act(team.get(target)) == 0)
			{
				target = (int)(Math.random() * team.size());
			}
		}
	}
	
	//Two methods that change the defenses of the players and enemies back to normal
	public static void returnDefensePlayers(ArrayList<Player> team)
	{
		for(int x = 0; x < team.size(); x++)
		{
			team.get(x).stopDefend();
		}
	}
	public static void returnDefenseEnemies(Enemy[][] enemies)
	{
		for(int row = 0; row < enemies.length; row++)
		{
			for(int col = 0; col < enemies[row].length; col ++)
			{
				enemies[row][col].stopDefend();
			}
		}
	}

	public static void main(String[] args) 
	{
		Scanner sc = new Scanner(System.in);
		
		//Create variables for input and two variables that tell if their is one player alive and if one enemy is alive
		String action = "";
		int enemyRow = 0;
		int enemyCol = 0;
		boolean teamAlive = true;
		boolean foesAlive = true;
		
		System.out.println("Your traveling party of Eric the Mage, Todd the Warrior, Jerry the Thief, and Sarah the Cleric, have suddenly been attacked by a horde of goblins!");
		System.out.println("You must fend them off!");
		System.out.println("Each team member has a standard attack and defend, and their own abilities.");
		System.out.println();
		System.out.println("Eric's attack ignores enemye defense. He has the ability Fire, which generates a random number between 1 and 9, adds up the number");
		System.out.println("you got with all the numbers before it, then multiplies the result by 10 to find the damage that is returned. He also has Explosion");
		System.out.println("which does damage to all enemies, but is weaker than the regular attack.");
		System.out.println();
		System.out.println("Todd has Wide Slash, which deals less than normal damage to an entire row of enemies, and power slash, which does 2.5 times damage to an enemy,");
		System.out.println("but lowers Todd's defense and prevents him from attacking next turn. (To use these abilites, enter \"wide\" and \"power\" respectively.");
		System.out.println();
		System.out.println("Jerry has sneak attack, which does .75 damage, but lowers the enemy's defense. (To use this, enter \"sneak\"");
		System.out.println();
		System.out.println("Sarah has Heal, which grants each party memeber 100 more health, but ceach member cannot exceed their maximum health, and Banish");
		System.out.println("which has a 1 in 5 chance of instantly killing an enemy.");
		System.out.println();
		System.out.println("To use these abilities in battle, just enter the name of the command when prompted, and enter the position of the enemy you wish to attack.");
		System.out.println();
		System.out.println("OK! Let the battle begin!");
		
		//Create 4 player characters and create an array list of players that they will be added in to
		Mage eric = new Mage("Eric", 200, 0, 20, 20);
		Warrior todd = new Warrior("Todd", 400, 30, 20);
		Thief jerry = new Thief("Jerry", 250, 20, 10);
		Cleric sarah = new Cleric("Sarah", 200, 10, 10);
		ArrayList<Player> team = new ArrayList<Player>();
		team.add(eric);
		team.add(todd);
		team.add(jerry);
		team.add(sarah);
		
		//Create 15 enemies and then create a 2D array with 3 rows and 3 columns that has an enemy as each element
		Enemy enemy1 = new Enemy("0,0", 100, 20, 10);
		Enemy enemy2 = new Enemy("0,1", 100, 20, 10);
		Enemy enemy3 = new Enemy("0,2", 100, 20, 10);
		Enemy enemy4 = new Enemy("1,0", 100, 20, 10);
		Enemy enemy5 = new Enemy("1,1", 100, 20, 10);
		Enemy enemy6 = new Enemy("1,2", 100, 20, 10);
		Enemy enemy7 = new Enemy("2,0", 100, 20, 10);
		Enemy enemy8 = new Enemy("2,1", 100, 20, 10);
		Enemy enemy9 = new Enemy("2,2", 100, 20, 10);
		Enemy[][] enemies = {{enemy1, enemy2, enemy3},
							 {enemy4, enemy5, enemy6},
							 {enemy7, enemy8, enemy9}};
		
		//Create the main loop that runs while at least one player and at least one enemy is alive
		while(teamAlive == true && foesAlive == true)
		{	
			//Display each player's health
			for(int i = 0; i < team.size(); i++)
			{
				System.out.println(team.get(i).getName() + "'s HP: " + team.get(i).health);
			}
			
			returnDefensePlayers(team);
			
			//Play through each character's turn if they are alive in the order of Jerry, Eric, Sarah, and Todd
			if(jerry.getAlive() == true && foesAlive == true)
			{
				
				//Ask the player for the action they want and the row and column numbers of the enemy to attack
				System.out.println("It is Jerry's turn, please select an attack");
				action = sc.next();
				System.out.println("Enter the row number of the enemy you wish to attack");
				enemyRow = sc.nextInt();
				System.out.println("Enter the column number of the enemy you wish to attack");
				enemyCol = sc.nextInt();
				while((enemyRow < 0 || enemyRow > 2) || (enemyCol < 0 || enemyCol > 2))
				{
					System.out.println("Enter a valid row/column number (0, 1, or 2)");
					enemyRow = sc.nextInt();
					enemyCol = sc.nextInt();
				}
				//If a valid command has not been entered, ask the player for each prompt again
				while(jerry.act(action, enemies[enemyRow][enemyCol]) == 0)
				{
					System.out.println("You entered an invalid command, please try again");
					action = sc.next();
					enemyRow = sc.nextInt();
					enemyCol = sc.nextInt();
					while((enemyRow < 0 || enemyRow > 2) || (enemyCol < 0 || enemyCol > 2))
					{
						System.out.println("Enter a valid row/column number (0, 1, or 2)");
						enemyRow = sc.nextInt();
						enemyCol = sc.nextInt();
					}
				}
			}
				
			//After each character's turn, check to check the living status of each enemy and the status of foes alive
			foesAlive = false;
			for(int row = 0; row < enemies.length; row++)
			{
				for(int col = 0; col < enemies[row].length; col++)
				{
					enemies[row][col].checkIfAlive();
					if(enemies[row][col].getAlive() == true)
					{
						foesAlive = true;
					}
				}
			}
			
			if(eric.getAlive() == true && foesAlive == true)
			{
				System.out.println("It is Eric's turn, please select an attack");
				action = sc.next();
				System.out.println("Enter the row number of the enemy you wish to attack");
				enemyRow = sc.nextInt();
				System.out.println("Enter the column number of the enemy you wish to attack");
				enemyCol = sc.nextInt();
				while((enemyRow < 0 || enemyRow > 2) || (enemyCol < 0 || enemyCol > 2))
				{
					System.out.println("Enter a valid row/column number (0, 1, or 2)");
					enemyRow = sc.nextInt();
					enemyCol = sc.nextInt();
				}
				while(eric.act(action, enemies[enemyRow][enemyCol], enemies) == 0)
				{
					System.out.println("You entered an invalid command, please try again");
					action = sc.next();
					enemyRow = sc.nextInt();
					enemyCol = sc.nextInt();
					while((enemyRow < 0 || enemyRow > 2) || (enemyCol < 0 || enemyCol > 2))
					{
						System.out.println("Enter a valid row/column number (0, 1, or 2)");
						enemyRow = sc.nextInt();
						enemyCol = sc.nextInt();
					}
				}
			}
			
			foesAlive = false;
			for(int row = 0; row < enemies.length; row++)
			{
				for(int col = 0; col < enemies[row].length; col++)
				{
					enemies[row][col].checkIfAlive();
					if(enemies[row][col].getAlive() == true)
					{
						foesAlive = true;
					}
				}
			}
			
			if(todd.getAlive() == true && foesAlive == true)
			{
				System.out.println("It is Todd's turn, please select an attack");
				action = sc.next();
				System.out.println("Enter the row number of the enemy you wish to attack");
				enemyRow = sc.nextInt();
				System.out.println("Enter the column number of the enemy you wish to attack");
				enemyCol = sc.nextInt();
				while((enemyRow < 0 || enemyRow > 2) || (enemyCol < 0 || enemyCol > 2))
				{
					System.out.println("Enter a valid row/column number (0, 1, or 2)");
					enemyRow = sc.nextInt();
					enemyCol = sc.nextInt();
				}
				while(todd.act(action, enemies[enemyRow][enemyCol], enemies[enemyRow]) == 0)
				{
					System.out.println("You entered an invalid command, please try again");
					action = sc.next();
					enemyRow = sc.nextInt();
					enemyCol = sc.nextInt();
					while((enemyRow < 0 || enemyRow > 2) || (enemyCol < 0 || enemyCol > 2))
					{
						System.out.println("Enter a valid row/column number (0, 1, or 2)");
						enemyRow = sc.nextInt();
						enemyCol = sc.nextInt();
					}
				}
			}
			
			foesAlive = false;
			for(int row = 0; row < enemies.length; row++)
			{
				for(int col = 0; col < enemies[row].length; col++)
				{
					enemies[row][col].checkIfAlive();
					if(enemies[row][col].getAlive() == true)
					{
						foesAlive = true;
					}
				}
			}
			
			if(sarah.getAlive() == true && foesAlive == true)
			{
				System.out.println("It is Sarah's turn, please select an attack");
				action = sc.next();
				System.out.println("Enter the row number of the enemy you wish to attack");
				enemyRow = sc.nextInt();
				System.out.println("Enter the column number of the enemy you wish to attack");
				enemyCol = sc.nextInt();
				while((enemyRow < 0 || enemyRow > 2) || (enemyCol < 0 || enemyCol > 2))
				{
					System.out.println("Enter a valid row/column number (0, 1, or 2)");
					enemyRow = sc.nextInt();
					enemyCol = sc.nextInt();
				}
				while(sarah.act(action, enemies[enemyRow][enemyCol], team) == 0)
				{
					System.out.println("You entered an invalid command, please try again");
					action = sc.next();
					enemyRow = sc.nextInt();
					enemyCol = sc.nextInt();
					while((enemyRow < 0 || enemyRow > 2) || (enemyCol < 0 || enemyCol > 2))
					{
						System.out.println("Enter a valid row/column number (0, 1, or 2)");
						enemyRow = sc.nextInt();
						enemyCol = sc.nextInt();
					}
				}
			}
			
			foesAlive = false;
			for(int row = 0; row < enemies.length; row++)
			{
				for(int col = 0; col < enemies[row].length; col++)
				{
					enemies[row][col].checkIfAlive();
					if(enemies[row][col].getAlive() == true)
					{
						foesAlive = true;
					}
				}
			}
			
			//return the enemies's defenses to normal
			returnDefenseEnemies(enemies);
			
			//Loop through the 2D array of enemies, have each perform their action, and check if the players are alive
			System.out.println("It is the enemy's turn!");
			for(int row = 0; row < enemies.length; row++)
			{
				for(int col = 0; col < enemies[row].length; col++)
				{
					if(teamAlive == true) 
					{
						enemyAttack(team, enemies[row][col]);
						teamAlive = false;
						for(int x = 0; x < team.size(); x++)
						{
							team.get(x).checkIfAlive();
							
							if(team.get(x).getAlive() == true)
							{
								teamAlive = true;
							}
						}
					}
				}
			}
				
		}
		System.out.println("The battle is over!");
		if(teamAlive == false)
		{
			System.out.println("Your team was wiped...");
			System.out.println("GAME OVER!");
		}
		else
		{
			System.out.println("The enemies have all been defeated!");
			System.out.println("Congratulations, you won!");
		}
		
		
	}

}
