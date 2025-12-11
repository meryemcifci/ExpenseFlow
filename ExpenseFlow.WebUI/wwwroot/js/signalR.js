src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/8.0.0/signalr.min.js"
  
//  Hub bağlantısı
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

// Server'dan gelecek bildirim
connection.on("ReceiveNotification", function (message) {
    // Badge sayısını 1 artır
    const badge = document.getElementById("notificationCount");
    let current = parseInt(badge.innerText) || 0;
    badge.innerText = current + 1;

    //  console’a da yazdırdım
    console.log("Yeni bildirim:", message);
});

//  Bağlantıyı başlat ve Manager olarak kaydol
connection.start()
    .then(function () {
        console.log("SignalR connected (Manager)");
        // Manager grubuna kayıt
        return connection.invoke("RegisterManager");
    })
    .catch(function (err) {
        console.error(err.toString());
    });

