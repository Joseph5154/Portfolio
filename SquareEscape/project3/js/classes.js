class Player extends PIXI.Graphics{
    constructor(color=0x00A2E8, x=0, y=0){
        super();
        this.beginFill(color);
        this.drawRect(0,0,60,80); //Edit if needed
        this.endFill();
        this.x = x;
        this.y = y;
        //variables
        this.dx = 0;
        this.dy = 30;
        this.grounded = true;
    }

    move(dt=1/60){
        this.y += this.dy * dt;
        this.dy += dt * 750;
        this.x += this.dx * dt;
    }
}

class Platform extends PIXI.Graphics{
    constructor(x=0, y=0, width=0, height=0, color=0xAAAAAA){
        super();
        this.beginFill(color);
        this.drawRect(x,y,width,height); //Edit if needed
        this.endFill();
        this.x = x;
        this.y = y;
    }
}

class Enemy extends PIXI.Graphics{
    constructor(x=0, y=0, color=0xED1C24){
        super();
        this.beginFill(color);
        this.drawRect(0,0,60,80); //Edit if needed
        this.endFill();
        this.x = x;
        this.y = y;
        this.isAlive = true;
    }
}

class Exit extends PIXI.Graphics{
    constructor(x=0, y=0, color=0x22B14C){
        super();
        this.beginFill(color);
        this.drawRect(0,0,60,80); //Edit if needed
        this.endFill();
        this.x = x;
        this.y = y;
    }
}

class Bullet extends PIXI.Graphics{
    constructor(x=0, y=0, dx=0, color=0x00A2E8){
        super();
        this.beginFill(color);
        this.drawRect(0,0,8,4);
        this.endFill();
        this.x = x;
        this.y = y;
        //variables
        this.dx = dx;
        this.isAlive = true;
    }

    move(dt=1/60){
        this.x += this.dx * dt;
    }
}