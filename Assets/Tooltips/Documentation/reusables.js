
//  Header  ---------------------------------------------------------
var header = document.createElement("div");

var path = window.location.pathname;
var page = path.split("/").pop();

if (page == "index.html") 
{
    header.innerHTML = 
    `
    <div class="header">
        <a class="header-button selected" type="button" href="index.html">Обзор</a>
        <a class="header-button"          type="button" href="reference.html">Компоненты</a>
    </div>
    `;
}
else 
{
    header.innerHTML = 
    `
    <div class="header">
        <a class="header-button"          type="button" href="index.html">Обзор</a>
        <a class="header-button selected" type="button" href="reference.html">Компоненты</a>
    </div>
    `;
}

document.body.insertBefore(header, document.body.firstChild);



//  Footer  ---------------------------------------------------------
var footer = document.createElement("div");

footer.innerHTML = 
`
<div class="footer"></div>
`;

document.body.appendChild(footer);

