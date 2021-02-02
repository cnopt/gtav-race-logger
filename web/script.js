
    const fileSelector  = document.getElementById('file-selector');
    const xPos = document.getElementById('xPos');
    const yPos = document.getElementById('yPos');

    var c = lineCanvas;
    var ctx = c.getContext("2d");
    ctx.fillStyle = "#202124";

    var minx,miny,maxx,maxy,range,scale,rangeX,rangeY;


    
    function dataCallback(results) {
        results.data.forEach((item, i) => {
            setTimeout(() => {
                console.log (i);
                // maybe this is where i should animate the line, as this could hold a 'frame count' of sorts
                xPos.innerHTML = item['X'];
            }, i*16);
        });
        xyCallback(results);
    }

    async function f(results) {
        miny = minx = Infinity
        maxx = maxy = -Infinity;
        results.data.forEach(dat => {
            minx = Math.min(minx,Object.values(dat)[8]);
            miny = Math.min(miny,Object.values(dat)[9]);
            maxx = Math.max(maxx,Object.values(dat)[8]);
            maxy = Math.max(maxy,Object.values(dat)[9]);          
        });
        rangeX = maxx - minx;
        rangeY = maxy - miny;
        console.log("async function complete");
    }


    function xyCallback(results) {
        f(results).then(
            function() {
                console.log("now doing the next one");
                console.log(maxx, maxy);
                console.log(minx,miny)
                range = Math.max(rangeX,rangeY);
                scale = Math.min(c.width,c.height);
                ctx.beginPath();
                results.data.forEach(dat => {
                    var x = Object.values(dat)[8];
                    var y = Object.values(dat)[9];
                    x = ((x-minx) / range) * scale;
                    y = ((y-miny) / range) * scale;
                    ctx.lineTo(x,y);
                });
                ctx.strokeStyle = "white"
                ctx.stroke();
            }
        )      
    }
    
    function parseData(url, callBack) {
        Papa.parse(url, {
            header:true,
            skipEmptyLines:true,
            complete: function(results) {
                dataCallback(results)
            }
        });
    }
    
    fileSelector.addEventListener('change', (event) => {
        const fileList = event.target.files;
        parseData(fileList[0]);
    });


    