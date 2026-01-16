document.addEventListener("DOMContentLoaded", function () {

    // Department Bar Chart
    const deptCanvas = document.getElementById("departmentExpenseChart");

    if (deptCanvas) {
        const labels = JSON.parse(deptCanvas.dataset.labels || "[]");
        const values = JSON.parse(deptCanvas.dataset.values || "[]");

        new Chart(deptCanvas, {
            type: "bar",
            data: {
                labels: labels,
                datasets: [{
                    label: "Onaylanmış Masraf Sayısı",
                    data: values,
                    backgroundColor: "rgba(78, 115, 223, 0.6)",   
                    borderColor: "rgba(78, 115, 223, 1)",
                    borderWidth: 1
                }]
            },
            options: {
                maintainAspectRatio: false,
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: { stepSize: 1 }
                    }
                }
            }
        });
    }
    

    // Category Pie Chart
    const catCanvas = document.getElementById("categoryPieChart");

    if (catCanvas) {
        const labels = JSON.parse(catCanvas.dataset.labels || "[]");
        const values = JSON.parse(catCanvas.dataset.values || "[]");

        new Chart(catCanvas, {
            type: "doughnut",
            data: {
                labels: labels,
                datasets: [{
                    data: values,
                    backgroundColor: [
                        "#4e73df", // Mavi
                        "#1cc88a", // Yeşil
                        "#36b9cc", // Turkuaz
                        "#f6c23e", // Sarı
                        "#e74a3b", // Kırmızı
                        "#858796"  // Gri
                    ],
                    hoverOffset: 6
                }]
            },
            options: {
                maintainAspectRatio: false,
                cutout: "70%",
                plugins: {
                    legend: {
                        position: "bottom"
                    }
                }
            }
        });
    }

});
