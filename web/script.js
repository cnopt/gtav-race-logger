
    const fileSelector  = document.getElementById('file-selector');
    const speed         = document.getElementById('mph-val');
    const rpm           = document.getElementById('rpm-meter');
    const gear          = document.getElementById('gear-val');
    const throttle      = document.getElementById('throttle-val');
    const brake         = document.getElementById('brake-val');
    const angle         = document.getElementById('angle');
    const clutch        = document.getElementById('clutch');
    const wheel         = document.getElementById('wheel');
    var c = lineCanvas;
    var ctx = c.getContext("2d");
    ctx.fillStyle = "#202124";
    var minx,miny,maxx,maxy,range,scale,rangeX,rangeY;

    
    function dataCallback(results) { // maybe this is where i should animate the line, as this could hold a 'frame count' of sorts
        results.data.forEach((item, i) => {
            setTimeout(() => {
                var scaledRPM = scaleRawValues(item['RPM'], 0.2, 1, 0, 400);
                rpm.style.width = fastFloor(scaledRPM) + 'px';
                speed.innerHTML = fastFloor(item['Wheel Speed']);
                gear.innerHTML = item['Gear'];
            }, i*16);
        });
        drawXY(results);
    }


    function drawXY(results) {
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

        console.log(maxx, maxy);
        console.log(minx,miny);
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
    

    function parseData(url, callBack) {
        Papa.parse(url, {
            header:true,
            skipEmptyLines:true,
            complete: function(results) {
                dataCallback(results)
            }
        });
    }

    const scaleRawValues = (num, in_min, in_max, out_min, out_max) => {
        return (num - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    function fastFloor(f) {
        return ~~(f*1.0)
    }

    

    fileSelector.addEventListener('change', (event) => {
        const fileList = event.target.files;
        parseData(fileList[0]);
    });


    