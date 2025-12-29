document.addEventListener("DOMContentLoaded", function () {

    const dataHolder = document.getElementById("expense-chart-data");
    if (!dataHolder) return;

    const monthlyData = JSON.parse(dataHolder.dataset.monthly);
    const weeklyData = JSON.parse(dataHolder.dataset.weekly);

    const ctx = document.getElementById("chart-sales").getContext("2d");

    const monthLabels = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    const weekLabels = ["W1", "W2", "W3", "W4", "W5"];

    const chart = new Chart(ctx, {
        type: "line",
        data: {
            labels: monthLabels,
            datasets: [{
                data: monthlyData,
                borderColor: "#5e72e4",
                backgroundColor: "rgba(94,114,228,0.15)",
                tension: 0.4,
                fill: true
            }]
        },
        options: {
            responsive: true,
            plugins: { legend: { display: false } },
            scales: { y: { beginAtZero: true } }
        }
    });

    btnMonth?.addEventListener("click", function (e) {
        e.preventDefault();

        chart.data.labels = monthLabels;
        chart.data.datasets[0].data = monthlyData;
        chart.update();

        btnMonth.classList.add("active");
        btnWeek.classList.remove("active");
    });

    btnWeek?.addEventListener("click", function (e) {
        e.preventDefault();

        chart.data.labels = weekLabels;
        chart.data.datasets[0].data = weeklyData;
        chart.update();

        btnWeek.classList.add("active");
        btnMonth.classList.remove("active");
    });

    const statusEl = document.getElementById("status-distribution-data");
    if (statusEl) {
        const paid = parseFloat(statusEl.dataset.paid);
        const pending = parseFloat(statusEl.dataset.pending);
        const rejected = parseFloat(statusEl.dataset.rejected);

        const statusCanvas = document.getElementById("chart-orders");
        if (statusCanvas) {
            new Chart(statusCanvas, {
                type: "doughnut",
                data: {
                    labels: ["Paid", "Pending", "Rejected"],
                    datasets: [{
                        data: [paid, pending, rejected],
                        backgroundColor: ["#2dce89", "#fb6340", "#f5365c"]
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: { position: "bottom" }
                    }
                }
            });
        }
    }
    //kategori için chart 
    const categoryEl = document.getElementById("category-chart-data");
    if (categoryEl) {
        const categoryLabels = JSON.parse(categoryEl.dataset.labels);
        const categoryValues = JSON.parse(categoryEl.dataset.values);

        const categoryCanvas = document.getElementById("chart-category");
        if (categoryCanvas) {
            new Chart(categoryCanvas, {
                type: "bar",
                data: {
                    labels: categoryLabels,
                    datasets: [{
                        label: "Expense Amount (₺)",
                        data: categoryValues,
                        backgroundColor: "#5e72e4"
                    }]
                },
                options: {
                    responsive: true,
                    scales: {
                        y: {
                            beginAtZero: true,
                            ticks: {
                                callback: value => "₺" + value
                            }
                        }
                    }
                }
            });
        }
    }

});
