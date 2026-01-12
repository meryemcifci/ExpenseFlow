document.addEventListener("DOMContentLoaded", () => {
    const countrySelect = document.getElementById("countrySelect");
    const citySelect = document.getElementById("citySelect");
    const districtSelect = document.getElementById("districtSelect");

    if (!countrySelect || !citySelect) {
        console.warn("Address selects not found on page");
        return;
    }

    // ---------- HELPERS ----------
    const resetSelect = (select, placeholder) => {
        select.innerHTML = `<option value="">${placeholder}</option>`;
        select.disabled = true;
    };

    const fillSelect = (select, items, placeholder) => {
        resetSelect(select, placeholder);
        if (items && items.length > 0) {
            items.forEach(item => {
                const option = document.createElement("option");
                option.value = item;
                option.textContent = item;
                select.appendChild(option);
            });
            select.disabled = false;
        }
    };

    // ---------- LOAD COUNTRIES ----------
    fetch("/api/address/countries")
        .then(res => {
            if (!res.ok) throw new Error("Countries API error");
            return res.json();
        })
        .then(data => {
            console.log("Countries loaded:", data);
            fillSelect(countrySelect, data, "Ülke Seçiniz");
        })
        .catch(err => {
            console.error("Country load error:", err);
        });

    // ---------- COUNTRY → CITIES ----------
    countrySelect.addEventListener("change", () => {
        const country = countrySelect.value;
        resetSelect(citySelect, "Şehir Seçiniz");
        

        if (!country) return;

        fetch(`/api/address/cities?country=${encodeURIComponent(country)}`)
            .then(res => {
                if (!res.ok) throw new Error("Cities API error");
                return res.json();
            })
            .then(data => {
                console.log("Cities loaded:", data);
                fillSelect(citySelect, data, "Şehir Seçiniz");
            })
            .catch(err => {
                console.error("City load error:", err);
            });
    });

   
});