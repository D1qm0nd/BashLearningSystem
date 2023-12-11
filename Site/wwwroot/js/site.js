

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

const MakeRequest = (method, url, body, action) => {
    sendHttpRequest(method, url, body).then(responseData => {
        if (responseData?.result != null)
            action(responseData.result);
        else action(responseData)
        return responseData;
    });
};