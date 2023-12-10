const cardContainer = document.querySelector(".cards-container");
const searchPlace = document.querySelector(".search-container-place");

var Source = ""; //Source
var UserId = ""; //guid


const RemoveCards = () => {
    while (cardContainer.childElementCount != 0) {
        let card = cardContainer.children.item(0)
        cardContainer.removeChild(card)
    }
}



const CreateSearchField = () => {
    field = document.createElement("div", {is: {height: "inherit"}});
    field.innerHTML = `
        <div class="search-container">
            <input class="search-field" type="text" value="" id="search-field">
            <button class="search-button" onclick="search()">
                <img src="Images/glass.png" alt="Поиск" style="height: inherit;"/>
            </button>
        </div>
    `
    searchPlace.appendChild(field)
}

CreateSearchField();






const CommandsCount = (theme) => {
    if (theme.commands != null)
        return theme.commands.length
    else return 0;
}


const CreateThemeCard = (theme) => {
    MakeRequest("POST", Source + "/theme-is-read", {themeId: theme.id, userId: UserId}, (data) => {
        let isRead = data.response;
        let commandsCount = CommandsCount(theme);
        let card = document.createElement("div");
        let img = "";
        if (isRead)
            img = `<img class="card-verifier" src="/Images/verifyIcon.png" alt="verifyIcon">`;
        let html = `
            <form method="get" class="card" id="${theme.id}">
                ${img}
                <h2>${theme.name}</h2>
                <a>Количество команд: ${commandsCount}</a>
                <p>Изменено: ${Date(theme.updatedUTC).toString('ru-RU')}</p>
                <a class="card-hyper-text" href="Theme/${theme.id}">Читать</a
            </form>
        `;
        card.innerHTML = html;
        cardContainer.appendChild(card);
    })
}



const CreateCards = (themes) => {
    RemoveCards()
    themes.forEach(theme => {
        CreateThemeCard(theme);
    });
}

const GetAllThemes = (action) => {
    MakeRequest("GET", Source + "/get-all-themes", undefined, (themes) => {
        action(themes);
    })
}

const GetReadThemes = (action) => {
    MakeRequest("POST", Source + "/get-read-themes", UserId, (themes) => {
        action(themes);
    })
}

const GetUnreadThemes = (action) => {
    MakeRequest("POST", Source + "/get-unread-themes", UserId, (themes) => {
        action(themes);
    })
}
const search = () => {
    let field = document.getElementById("search-field");
    let query = field?.value.toLowerCase();
    if (query.includes("непрочитано"))
        GetUnreadThemes(CreateCards);
    else if (query.includes("прочитано"))
        GetReadThemes(CreateCards);
    else {
        GetAllThemes((themes => {
            CreateCards(themes.filter(theme => theme.name.toLowerCase().includes(query)))
        }))
    }
}




GetAllThemes(CreateCards) //Начальная загрузка контента




