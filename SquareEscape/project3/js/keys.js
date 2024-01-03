const keyboard = Object.freeze({
	W: 			87,
	A:          65,
    D:          68
});

// this is the "key daemon" that we poll every frame
const keys = [];

window.onkeyup = (e) => {
//	console.log("keyup=" + e.keyCode);
	keys[e.keyCode] = false;
	e.preventDefault();
};

window.onkeydown = (e)=>{
//	console.log("keydown=" + e.keyCode);
	keys[e.keyCode] = true;
};