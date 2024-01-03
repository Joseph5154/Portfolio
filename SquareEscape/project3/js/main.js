// We will use `strict mode`, which helps us by having the browser catch many common JS mistakes
// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Strict_mode
"use strict";
const app = new PIXI.Application({
    width: 600,
    height: 600
});
document.body.appendChild(app.view);

// constants
const sceneWidth = app.view.width;
const sceneHeight = app.view.height;

//aliases
let stage;

//game variables
let startScene;
let player;
let exit;
let jumpSound;
let shootSound;
let deathSound;
let clearSound;
let victorySound;
let levelScene;
let gameWinScene;
let gameOverScene;

let enemies = [];
let bullets = [];
let platforms = [];
let levelNum = 1;
let paused = true;

//Sets up the game by making all of the scenes, loading the sound, making the player, exit, and buttons,
//starting the game loop, and setting the shoot method to onclick
function setup()
{
    stage = app.stage;
    //Initialize all scenes
    startScene = new PIXI.Container();
    startScene.visible = true;
    stage.addChild(startScene);
    levelScene = new PIXI.Container();
    levelScene.visible = false;
    stage.addChild(levelScene);
    gameWinScene = new PIXI.Container();
    gameWinScene.visible = false;
    stage.addChild(gameWinScene);
    gameOverScene = new PIXI.Container();
    gameOverScene.visible = false;
    stage.addChild(gameOverScene);

    //Create the labels and initialize the player and exit, which will be used for each of the levels
    createLabelsAndButtons();

    player = new Player();
    exit = new Exit();

    //move the player so that the clear sound does not play
    player.x = 300;

    //Load sounds
    jumpSound = new Howl({
	    src: ['sounds/jump.wav']
    });
    shootSound = new Howl({
	    src: ['sounds/shoot.wav']
    });
    deathSound = new Howl({
	    src: ['sounds/death.wav']
    });
    clearSound = new Howl({
	    src: ['sounds/clear.wav']
    });
    victorySound = new Howl({
	    src: ['sounds/victory.wav']
    });

    //Set the game loop and the shoot method
    app.ticker.add(gameLoop);

    app.view.onclick = shoot;
}

//The primary loop of the game
function gameLoop()
{
    //Have the player succumb to gravity
    let dt = 1/app.ticker.FPS;
    if (dt > 1/12) dt=1/12;

    //Have the player jump when spacebar is pressed and if they are considered grounded
    if(keys[keyboard.W] && player.grounded)
    {
        player.dy = -600;
        player.grounded = false;
        jumpSound.play();
    }

    //Have the player move left and right when a and d are pressed
    if(keys[keyboard.A]){
        player.dx = -200;
    }else if(keys[keyboard.D]) {
        player.dx = 200;
    }else{
        player.dx = 0;
    }
    if(levelScene.visible)
    {
        player.move(dt);
    }

    //CHeck collisions with the platforms
    for(let p of platforms)
    {
        //If the player collides with a platform, then handle collision
        if(rectsIntersect(player, p))
        {
            platformCollision(player, p);
        }

        //If a bullet collides with a platformremove it from the scene
        for(let b of bullets)
        {
            if(rectsIntersect(p, b)){
                levelScene.removeChild(b)
                b.isAlive = false;
            }
        }
    }

    //Handle enemy collisions
    for(let e of enemies)
    {
        //bullets
        for(let b of bullets)
        {
            if(rectsIntersect(e,b))
            {
                deathSound.play();
                levelScene.removeChild(e)
                e.isAlive = false
                levelScene.removeChild(b)
                b.isAlive = false;
            }
        }
        //If the player collides with an enemy, sned them to the game over screen
        if(rectsIntersect(e, player))
        {
            levelScene.visible = false;
            gameOverScene.visible = true;
            player.x = 0;
            deathSound.play();
        }
    }

    //If the pllayer collids with the exit, move to the next level
    if(rectsIntersect(player, exit))
    {
        levelClear();
    }

    for(let b of bullets)
    {
        b.move(dt);
    }

    //Get rid of dead enemies and bullets
    bullets = bullets.filter(b=>b.isAlive);
    enemies = enemies.filter(e=>e.isAlive);
}

//Makes the labels and buttons that will be used on the start, win, and loss screens
function createLabelsAndButtons()
{
    let buttonStyle = new PIXI.TextStyle({
        fill: 0x00A2E8,
        fontSize: 50
    });

    //startScene setup
    let startLabel1 = new PIXI.Text("Square Escape");
    startLabel1.style = new PIXI.TextStyle({
        fill: 0xFFFFFF,
        fontSize: 80,
        stroke: 0x00A2E8,
        strokeThickness: 5
    });
    startLabel1.x = 30;
    startLabel1.y = 120;
    startScene.addChild(startLabel1);

    let startButton = new PIXI.Text("Start Game");
    startButton.style = buttonStyle;
    startButton.x = 150;
    startButton.y = 450;
    startButton.interactive = true;
    startButton.buttonMode = true;
    startButton.on("pointerup", startGame); //startGame is a funtion reference
    startButton.on('pointerover', e => e.target.alpha = 0.7);
    startButton.on('pointerout', e => e.currentTarget.alpha = 1.0);
    startScene.addChild(startButton);

    //Win screen setup
    let winLabel = new PIXI.Text("You Win!");
    winLabel.style = new PIXI.TextStyle({
        fill: 0xFFFFFF,
        fontSize: 80,
        stroke: 0x00A2E8,
        strokeThickness: 5
    });
    winLabel.x = 125;
    winLabel.y = 120;
    gameWinScene.addChild(winLabel);

    let winButton = new PIXI.Text("Play Again?");
    winButton.style = buttonStyle;
    winButton.x = 150;
    winButton.y = 450;
    winButton.interactive = true;
    winButton.buttonMode = true;
    winButton.on("pointerup", startGame); //startGame is a funtion reference
    winButton.on('pointerover', e => e.target.alpha = 0.7);
    winButton.on('pointerout', e => e.currentTarget.alpha = 1.0);
    gameWinScene.addChild(winButton);

    //Lose screen setup
    let loseLabel = new PIXI.Text("You lose...");
    loseLabel.style = new PIXI.TextStyle({
        fill: 0xFFFFFF,
        fontSize: 80,
        stroke: 0x00A2E8,
        strokeThickness: 5
    });
    loseLabel.x = 110;
    loseLabel.y = 120;
    gameOverScene.addChild(loseLabel);

    let loseButton = new PIXI.Text("Try Again?");
    loseButton.style = buttonStyle;
    loseButton.x = 150;
    loseButton.y = 450;
    loseButton.interactive = true;
    loseButton.buttonMode = true;
    loseButton.on("pointerup", startGame); //startGame is a funtion reference
    loseButton.on('pointerover', e => e.target.alpha = 0.7);
    loseButton.on('pointerout', e => e.currentTarget.alpha = 1.0);
    gameOverScene.addChild(loseButton);
}

//Once the level has been cleared, the level scene gets reset and the next level is made.
//If the final level has been cleared, it moves to the game win screen
function levelClear()
{
    resetLevel();
    player.x = 100;
    //Play the clear or victory sounds
    if(levelNum < 5)
    {
        clearSound.play();
    }
    else{
        victorySound.play();
    }

    if(levelNum == 1)
    {
        player.y = 450;
        exit.x = 500;
        exit.y = 60;
        
        enemies.push(new Enemy(200, 260));
        enemies.push(new Enemy(260, 60));
        platforms.push(new Platform(0, 170, 450, 30));
        platforms.push(new Platform(100, 70, 450, 30));
    }
    else if(levelNum == 2)
    {
        player.y = 75;
        exit.x = 500;
        exit.y = 100;

        enemies.push(new Enemy(180, 460));
        enemies.push(new Enemy(240, 460));
        enemies.push(new Enemy(300, 460));
        enemies.push(new Enemy(360, 460));
        platforms.push(new Platform(0, 90, 200, 600));
        platforms.push(new Platform(200, 90, 200, 600));
    }
    else if(levelNum == 3)
    {
        player.y = 450;
        exit.x = 500;
        exit.y = 100;

        enemies.push(new Enemy(30,320));
        enemies.push(new Enemy(300, 200));
        platforms.push(new Platform(0, 200, 100, 30))
        platforms.push(new Platform(150, 140, 60, 30))
        platforms.push(new Platform(250, 90, 80, 30));
    }
    else if(levelNum == 4)
    {
        player.y = 70;
        exit.x = 510;
        exit.y = 70;

        enemies.push(new Enemy(100, 520));
        enemies.push(new Enemy(160, 520));
        enemies.push(new Enemy(220, 520));
        enemies.push(new Enemy(280, 520));
        enemies.push(new Enemy(340, 520));
        enemies.push(new Enemy(400, 520));
        enemies.push(new Enemy(460, 520));
        enemies.push(new Enemy(520, 520));
        platforms.push(new Platform(0, 75, 150, 600));
        platforms.push(new Platform(250, 75, 100, 600));
        platforms.push(new Platform(150, 0, 40, 300));
        platforms.push(new Platform(130, 100, 60, 30));
        platforms.push(new Platform(160, 100, 60, 30));
        platforms.push(new Platform(70, 170, 60, 30));
        platforms.push(new Platform(230, 170, 60, 30));
        platforms.push(new Platform(150, 200, 60, 30));
    }
    else if(levelNum == 5)
    {
        levelScene.visible = false;
        gameWinScene.visible = true;
    }
    levelNum++;
    populatePlatformsAndEnemies();
}

//Moves from the start, game over, or win scenes to the actual game, and creates the first level
function startGame()
{
    startScene.visible = false;
    gameWinScene.visible = false;
    gameOverScene.visible = false;
    levelNum = 1;
    levelScene.visible = true;
    levelScene.addChild(player);
    levelScene.addChild(exit);
    player.x = 100;
    player.y = 450;

    //create the first level
    levelScene.visible = true;
    levelScene.addChild(player);
    levelScene.addChild(exit);
    player.x = 100;
    player.y = 450;
    exit.x = 500;
    exit.y = 280;

    resetLevel();

    enemies.push(new Enemy(250, 460));

    platforms.push(new Platform(200, 180, 300, 50));
    platforms.push(new Platform(120, 0, 60, 400));

    populatePlatformsAndEnemies();
}

//Allows the player to shoot
function shoot(e)
{
    //Depending on the mouse position, shoot to either position of the player
    if(levelScene.visible)
    {
        let b;
        let mousePosition = app.renderer.plugins.interaction.mouse.global;
        if(mousePosition.x > player.x +(.5 * player.getBounds().width))
        {
            b = new Bullet(player.x + player.getBounds().width, player.y +(.5 * player.getBounds().height), 300);
        }
        else{
            b = new Bullet(player.x, player.y +(.5 * player.getBounds().height), -300)
        }
        shootSound.play();
        bullets.push(b);
        levelScene.addChild(b);
    }
}

//Clears the platforms, enemies, and bullets arrays and creates the box of platforms around each level
function resetLevel()
{
    //Remove all enemies and platforms from the scene
    for(let p of platforms)
    {
        levelScene.removeChild(p);
    }
    for(let e of enemies)
    {
        levelScene.removeChild(e);
    }
    for(let b of bullets)
    {
        levelScene.removeChild(b);
    }
    
    //Reset the three lists
    platforms = [];
    enemies = [];
    bullets = [];

    //Create the main 4 platforms
    platforms.push(new Platform(0, 270, 600, 100));
    platforms.push(new Platform(0, 0, 30, 600));
    platforms.push(new Platform(0, 0, 600, 30));
    platforms.push(new Platform(285, 0, 30, 600));
}

//Add the platforms and enemies to the level scene
function populatePlatformsAndEnemies()
{
    for(let e of enemies)
    {
        levelScene.addChild(e);
    }
    for(let p of platforms)
    {
        levelScene.addChild(p);
    }
}

app.loader.onProgress.add(e => { console.log(`progress=${e.progress}`) });
app.loader.onComplete.add(setup);
app.loader.load();