document.addEventListener("click", function (e) {
    const item = e.target.closest(".notification-item");
    if (!item) return;

    const notificationId = item.dataset.id;
    const redirectUrl = item.dataset.url;

    fetch("/Notification/ReadAndRedirect", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            notificationId: notificationId
        })
    })
        .then(() => {
            item.remove();

            const badge = document.getElementById("notificationCount");
            if (badge) {
                let count = parseInt(badge.innerText || "0");
                badge.innerText = Math.max(0, count - 1);
            }

            if (redirectUrl) {
                window.location.href = redirectUrl;
            }
        });
});
