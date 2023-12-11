var lastRequest = ""

const getConsole = () => document.getElementById("terminalConsole")
const execute = (url, id) => {
    let command = getDataFromTerminal().replace(lastRequest,"")
    let data = {'id': id,'command': command}
    if (data.command != "")
        MakeRequest("POST", `${url}-execute`, data, (data) => {
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
