// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

function sendCommandToExecute(url)
{
    let terminal_console = document.getElementById('terminalConsole');
    let command=terminal_console.value;
    console.log(`Make request: ${command}`)
    
    let a = $.ajax({
        url: url+command, 
        method: "get",
        contentType: 'text/json',
        dataType: 'text/json'
    })
    a.onload = function() {
    if (a.status == 200) {
        let response = JSON.parse(a.responseText);
        terminal_console.value = response;
        console.log(`Command: ${command}\nAnswer: ${response}`)
    }
}
    console.log(a)
    // $.post({
    //     url: 'http://127.0.0.1:55555/BashExecutor',
    //     // dataType:'text/json',
    //     data: command,
    //     success: function (data) {
    //         terminal_console.value=data;
    //         console.log(`Success: ${data}`)
    //     },
    //     onabort: function (data) {
    //         console.log(`Abort: ${data}`)
    //     },
    //     onerror: function (data) {
    //         console.log(`Error: ${data}`)
    //     },
    //     complete: function() {
    //         console.log(`Complete`)
    //     }
    // })
}

// $.ajax(
//     {
//         url: `http://127.0.0.1:55555/BashExecutor/${command}`,
//         // url: `${@ExecutorIP}/BashExecutor/${command}`,
//         type: 'post',
//         dataType: 'text/json',
//         data: command,
//         success: function (data)
//         {
//             terminal_console.value=data;
//             console.log(`Success: ${data}`)
//         },
//         onabort: function (data)
//         {
//             console.log(`Abort: ${data}`)
//         },
//         onerror: function (data)
//         {
//             console.log(`Error: ${data}`)
//         },
//         complete: function()
//         {
//             console.log(`Complete`)
//         }
//     }
// )