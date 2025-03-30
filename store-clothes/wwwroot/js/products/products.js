document.addEventListener("DOMContentLoaded", function () {
    fetch("/Product/GetFilterData")
        .then(response => response.json())
        .then(data => {
            renderFilterOptions(data.sizes, "size", ".size-filter", "checkbox");
            renderFilterOptions(data.priceRanges, "price", ".price-filter", "radio");
        })
        .catch(error => console.error("Error fetching filter data:", error));
});

function renderFilterOptions(items, name, containerSelector, type) {
    const container = document.querySelector(containerSelector);
    container.innerHTML = ""; // Xóa dữ liệu cũ

    items.forEach(item => {
        const div = document.createElement("div");
        div.classList.add("form-check"); // Thêm class Bootstrap

        div.innerHTML = `
            <input type="${type}" name="${name}" value="${item.Id}" class="form-check-input" id="${name}-${item.Id}">
            <label class="form-check-label" for="${name}-${item.Id}">${item.Name}</label>
        `;
        container.appendChild(div);
    });
}
document.addEventListener("DOMContentLoaded", function () {
    // Xử lý tìm kiếm
    const searchInput = document.querySelector(".search-input");
    const searchForm = document.querySelector(".search-form");

    // Tự động gửi form khi người dùng nhấn Enter hoặc sau khi ngừng gõ (debounce)
    let debounceTimeout;
    searchInput.addEventListener("input", function () {
        clearTimeout(debounceTimeout);
        debounceTimeout = setTimeout(() => {
            searchForm.submit();
        }, 500);
    });

    searchInput.addEventListener("keypress", function (e) {
        if (e.key === "Enter") {
            e.preventDefault();
            clearTimeout(debounceTimeout);
            searchForm.submit();
        }
    });

    // Xử lý bộ lọc
    const sizeInput = document.querySelector('input[name="size"]');
    const minPriceInput = document.querySelector('input[name="minPrice"]');
    const maxPriceInput = document.querySelector('input[name="maxPrice"]');
    const form = document.querySelector(".filter-form");

    if (form) {
        [sizeInput, minPriceInput, maxPriceInput].forEach(input => {
            if (input) {
                input.addEventListener("change", function () {
                    form.submit();
                });
            }
        });

        minPriceInput.addEventListener("input", function () {
            if (this.value < 0) this.value = 0;
        });

        maxPriceInput.addEventListener("input", function () {
            if (this.value < 0) this.value = 0;
            if (minPriceInput.value && this.value < minPriceInput.value) {
                this.value = minPriceInput.value;
                alert("Giá tối đa không thể nhỏ hơn giá tối thiểu!");
            }
        });

        // Kiểm tra form trước khi gửi
        form.addEventListener("submit", function (e) {
            if (minPriceInput.value && maxPriceInput.value && parseFloat(maxPriceInput.value) < parseFloat(minPriceInput.value)) {
                e.preventDefault();
                alert("Giá tối đa phải lớn hơn hoặc bằng giá tối thiểu!");
            }
        });
    }
});