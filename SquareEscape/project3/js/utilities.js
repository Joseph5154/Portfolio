//Checks if there is a collision between two objects
function rectsIntersect(a,b){
    var ab = a.getBounds();
    var bb = b.getBounds();
    return ab.x + ab.width > bb.x && ab.x < bb.x + bb.width && ab.y + ab.height > bb.y && ab.y < bb.y + bb.height;
}

//Adjusts the player(a)'s position when collided with a platform (b) (It is assumed that the collision has already been checked for and found) 
function platformCollision(a,b){
    let ab = a.getBounds();
    let bb = b.getBounds();
    //Variables that determine the "rectangle" of collision
    let cxmin = 0;
    let cymin = 0;
    let cxmax = 0;
    let cymax = 0;
    if(ab.x < bb.x)
    {
        cxmin = bb.x;
    }
    else
    {
        cxmin = ab.x;
    }
    if(ab.y < bb.y)
    {
        cymin = bb.y;
    }
    else
    {
        cymin = ab.y;
    }
    if(ab.x + ab.width > bb.x + bb.width)
    {
        cxmax = bb.x + bb.width;
    }
    else
    {
        cxmax = ab.x + ab.width;
    }
    if(ab.y + ab.height > bb.y + bb.height)
    {
        cymax = bb.y + bb.height;
    }
    else
    {
        cymax = ab.y + ab.height;
    }
    let cw = cxmax - cxmin;
    let ch = cymax - cymin;
    //Now that the collision rectangle has been determined, determine if the player should be moved vertically or horizaontally,
    //and then if they should be moved up of down/left or right
    if(cw > ch)
    {
        if(ab.y < bb.y)
        {
            a.y -= ch;
            a.dy = 30;
            a.grounded = true;
        }
        else
        {
            a.y += ch;
            a.dy = 0;
        }
    }
    else
    {
        if(ab.x > bb.x)
        {
            a.x += cw;
        }
        else
        {
            a.x -= cw;
        }
    }
}