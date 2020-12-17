document.getElementById("for_label").innerText = "Select image"
setBindings()
var url = 'https://localhost:5001/server/'

function setBindings()
{
    var image_input = $('#image_input')
    image_input.change(e => LoadImageAndRecognize(e))

    var display_all_button = $('#display_all')
    display_all_button.click(display_all)
}



async function LoadImageAndRecognize(e)
{
    var file = e.target.files[0]
    ctx = document.getElementById('image_canvas').getContext('2d')
    var reader = new FileReader()
    reader.readAsDataURL(file)
    var img = new Image()
    img.onload = function () {
        ctx.drawImage(img, 0, 0, 150, 150)
    }
    document.getElementById("for_label").innerText = "Processing..."
    reader.onload = async function (e) {
        img.src=reader.result
                    
        var img_and_path = []
        img_and_path[0] = reader.result.split(',')[1]
        img_and_path[1] = "from_browser"

        var response = await fetch(url + 'single_img', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(img_and_path)
        })
        console.log("recognized")
        var js_resp = await response.json()
        document.getElementById("for_label").innerText = "Image Label: " + js_resp['label']
        console.log(js_resp)
    }
}

async function display_all()
{
    var response = await fetch(url + 'display_all', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(response => 
        response.json().then(data => ({
            data: data,
            status: response.status
            
        })
    ).then(res => {
        console.log(res.status)
        console.log(res.data)
            
            $('#myTableId tbody > tr').remove();

            var ourTable = document.getElementById("myTableId");
            var ourTableBody = document.createElement("tbody");

            var keys = ["Image", "Label", "Confidence", "Path"]
           
            
            for (var i = 0; i < res.data.length; i++) {
              let row = document.createElement("tr");
              let single_pred = res.data[i]

              
              for (var j = 0; j < keys.length; j++) {
                let cell = document.createElement("td");
                if (keys[j] == "Image") {
                    let canvas = document.createElement('canvas')
                        canvas.width = 50
                        canvas.height = 50
                        let ctx = canvas.getContext('2d')
                        let img = new Image()
                        img.onload = function () {
                            ctx.drawImage(img, 0, 0, 50, 50)
                        }
                        img.src = 'data:image/jpg;base64, ' + single_pred[keys[j]]
                        cell.appendChild(canvas);
                        row.appendChild(cell);
                } else {
                    let cellText = document.createTextNode(single_pred[keys[j]]);
                    console.log(single_pred[keys[j]] + " qq")
                    cell.appendChild(cellText);
                    row.appendChild(cell);
                }
              }
              ourTableBody.appendChild(row);
            }
            ourTable.appendChild(ourTableBody);
            body.appendChild(ourTable);
            ourTable.setAttribute("border", "2");
    }));
}

