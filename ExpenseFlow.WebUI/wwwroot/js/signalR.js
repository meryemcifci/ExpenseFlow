console.log("signalR.js yüklendi");

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

connection.on("ReceiveNotification", function (notification) {

    const badge = document.getElementById("notificationCount");
    if (badge) {
        let count = parseInt(badge.innerText || "0");
        badge.innerText = count + 1;
    }

    const dropdown = document.querySelector(".dropdown-menu");
    if (!dropdown) return;

    const item = document.createElement("button");
    item.type = "button";
    item.className = "dropdown-item notification-item";
    item.dataset.id = notification.id;
    item.dataset.url = notification.redirectUrl;

    item.innerHTML = `
        <div class="small text-gray-800">${notification.message}</div>
        <div class="text-muted small">Az önce</div>
    `;

    dropdown.insertBefore(item, dropdown.children[1]);
});

connection.start()
    .then(() => console.log("SignalR bağlandı"))
    .catch(err => console.error("SignalR hata:", err));
