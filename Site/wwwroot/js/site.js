const sendHttpRequest = (method, url, data) => {
    const promise = new Promise((resolve, reject) => {
        const xhr = new XMLHttpRequest();
        xhr.open(method, url);
        xhr.responseType = 'json';
        xhr.setRequestHeader("Content-Type", "text/json")
        xhr.onload = () => {
            resolve(xhr.response)
        }
        xhr.body = data;
        xhr.send(JSON.stringify(data));
    });
    return promise;
};

const getData = (url) => {
    sendHttpRequest("GET", url).then(responseData => {
        console.log(responseData);
        return responseData;
    });
}

const sendData = (method, url, body) => {
    sendHttpRequest(method, url, body).then(responseData => {
        updateTerminal(responseData.result);
        return responseData;
    });
};

const create = (url, id) => {
    sendData("POST", `${url}-create`, id)
}

const start = (url, id) => {
    sendData("POST", `${url}-start`, id)
}

const stop = (url, id) => {
    sendData("POST", `${url}-stop`, id)
}

const execute = (url, data) => {
    if (data.command != "")
        sendData("POST", `${url}-execute`, data)
}

const getDataFromTerminal = () => {
    let console = document.getElementById('terminalConsole');
    return console.value;
}

const updateTerminal = (data) => {
    console.log(`responce: ${data}`)
    let _console = document.getElementById("terminalConsole");
    console.log(`update console text`)
    _console.value = data;
}

const remove = (url, id) => {
    sendData("DELETE", `${url}-remove`, id)
}

const exists = (url, id) => {
    sendData("POST", `${url}-exists`, id)
}
