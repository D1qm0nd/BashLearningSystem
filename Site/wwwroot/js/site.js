var lastRequest = ""

const getConsole = () => document.getElementById("terminalConsole")

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

const sendData = (method, url, body, action) => {
    sendHttpRequest(method, url, body).then(responseData => {
        action(responseData.result);
        return responseData;
    });
};

const execute = (url, id) => {
    let command = getDataFromTerminal().replace(lastRequest,"")
    let data = {'id': id,'command': command}
    if (data.command != "")
        sendData("POST", `${url}-execute`, data, (data) => {
            lastRequest += `${command}\n${data}\n`
            updateTerminal(lastRequest)
        })
}

const textEdit = (e) => {
    if (getDataFromTerminal().length < lastRequest.length)
        updateTerminal(lastRequest)
}

const getDataFromTerminal = () => {
    return getConsole().value;
}

const updateTerminal = (data) => {
    console.log(`responce: ${data}`)
    console.log(`update console text`)
    getConsole().value = data;
}
