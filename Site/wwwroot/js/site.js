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
    }).done( function(data) {
            let response = JSON.parse(data);
            terminal_console.value = response;
            console.log(`Command: ${command}\nAnswer: ${response}`)
    })
    console.log(a)
}