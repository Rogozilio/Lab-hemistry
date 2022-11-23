let progressScreen = document.querySelector(".progress-screen"); 
let progressFull = document.querySelector(".progress .full"); 
let progressEmpty = document.querySelector(".progress .empty"); 

function onProgress (progress) 
{
    let widthFull = progress * 100; 
    let widthEmpty = (1 - progress) * 100; 

    progressFull.style = "width: " + widthFull + "%"; 
    progressEmpty.style = "width: " + widthEmpty + "%"; 

    if (progress == 1) 
    {
        progressScreen.style = "display: none"; 
    }
}

