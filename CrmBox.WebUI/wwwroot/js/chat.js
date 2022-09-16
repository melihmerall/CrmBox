// Bağlantımı kurdum
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .withAutomaticReconnect([0, 1000, 5000, null])
    .build();

// hublarım ile bağlantı kurdum.
connection.on("ReceiveUserConnected", function (userId, userName) {

    addMessage(`${userName} has openned a connection`);

});

connection.on("ReceiveUserDisconnected", function (userId, userName) {

    addMessage(`${userName} has closed a connection`);

});

connection.on("ReceivePrivateMessage", function (senderId, senderName, receiverId, message, chatId, receiverName) {
    addPrivateMessage(`[${receiverName} ]${senderName} : ${message}`)
});

connection.on("receiveMessage", message => (
    $("div").append(message + "<br>")));

function sendMessage() {
    let inputMsg = document.getElementById("txtMessageBox");


function sendPrivateMessage() {
    let inputMsg = document.getElementById('txtPrivateMessage');
    let ddlSelUser = document.getElementById('ddlSelUser');

    let receiverId = ddlSelUser.value;
    let receiverName = ddlSelUser.options[ddlSelUser.selectedIndex].text;
    var message = inputMsg.value;

    connection.send("SendPrivateMessage", receiverId, message, receiverName);
    inputMsg.value = '';
}


document.addEventListener('DOMContentLoaded', (event) => {
    fillRoomDropDown();
    fillUserDropDown();
})

function fillUserDropDown() {

    $.getJSON('/ChatRooms/GetChatUser')
        .done(function (json) {

            var ddlSelUser = document.getElementById("ddlSelUser");

            ddlSelUser.innerText = null;

            json.forEach(function (item) {
                var newOption = document.createElement("option");

                newOption.text = item.userName;//item.whateverProperty
                newOption.value = item.id;
                ddlSelUser.add(newOption);


            });

        })
        .fail(function (jqxhr, textStatus, error) {

            var err = textStatus + ", " + error;
            console.log("Request Failed: " + jqxhr.detail);
        });

}

function fillRoomDropDown() {

    $.getJSON('/ChatRooms/GetChatRoom')
        .done(function (json) {
            var ddlDelRoom = document.getElementById("ddlDelRoom");
            var ddlSelRoom = document.getElementById("ddlSelRoom");

            ddlDelRoom.innerText = null;
            ddlSelRoom.innerText = null;

            json.forEach(function (item) {
                var newOption = document.createElement("option");

                newOption.text = item.name;
                newOption.value = item.id;
                ddlDelRoom.add(newOption);


                var newOption1 = document.createElement("option");

                newOption1.text = item.name;
                newOption1.value = item.id;
                ddlSelRoom.add(newOption1);

            });

        })
        .fail(function (jqxhr, textStatus, error) {

            var err = textStatus + ", " + error;
            console.log("Request Failed: " + jqxhr.detail);
        });

}
function addPrivateMessage(msg) {
    if (msg == null && msg == '') {
        return;
    }
    else {
        let ui = document.getElementById('privateMessage')
        let li = document.createElement("li");
        li.innerHTML = msg;
        ui.appendChild(li);
    }

}

function addMessage(msg) {
    if (msg == null && msg == '') {
        return;
    }
    else {
        let ui = document.getElementById('messagesList');
        let li = document.createElement("li");
        li.innerHTML = msg;
        ui.appendChild(li);
    }



}

connection.start();