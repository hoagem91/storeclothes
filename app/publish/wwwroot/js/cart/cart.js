document.addEventListener("DOMContentLoaded", function () {
    const qtyButtons = document.querySelectorAll(".qty-btn");
    const subtotalElement = document.getElementById("cart-subtotal");

    if (!subtotalElement || qtyButtons.length === 0) {
        console.warn("Không có sản phẩm nào trong giỏ hàng.");
        return;
    }

    // Xử lý tăng/giảm số lượng khi nhấn nút
    qtyButtons.forEach(button => {
        button.addEventListener("click", function () {
            const id = this.getAttribute("data-id");
            const input = document.querySelector(`.qty-input[data-id='${id}']`);

            if (!input) return;

            let quantity = parseInt(input.value) || 1;

            if (this.classList.contains("plus")) {
                quantity++;
            } else if (this.classList.contains("minus") && quantity > 1) {
                quantity--;
            }

            input.value = quantity;
            updateTotal(id, quantity);
        });
    });

    // Xử lý khi nhập số lượng bằng tay
    document.querySelectorAll(".qty-input").forEach(input => {
        input.addEventListener("input", function () {
            const id = this.getAttribute("data-id");
            let quantity = parseInt(this.value) || 1;
            if (quantity < 1) quantity = 1;
            this.value = quantity;
            updateTotal(id, quantity);
        });
    });

    function updateTotal(id, quantity) {
        const priceElement = document.querySelector(`.price[data-id='${id}']`);
        const totalElement = document.querySelector(`.total-price[data-id='${id}']`);

        if (!priceElement || !totalElement) return;

        // Lấy giá từ cột "price"
        const price = parseFloat(priceElement.getAttribute("data-price")) || 0;
        const total = price * quantity;

        // Cập nhật tổng giá cho sản phẩm
        totalElement.innerText = new Intl.NumberFormat("vi-VN", { style: "currency", currency: "VND" }).format(total);

        updateCartSubtotal();
    }

    function updateCartSubtotal() {
        let total = 0;

        document.querySelectorAll(".total-price").forEach(el => {
            const id = el.getAttribute("data-id");
            const priceElement = document.querySelector(`.price[data-id='${id}']`);
            const quantityInput = document.querySelector(`.qty-input[data-id='${id}']`);

            if (!priceElement || !quantityInput) return;

            const price = parseFloat(priceElement.getAttribute("data-price")) || 0;
            const quantity = parseInt(quantityInput.value) || 0;

            total += price * quantity;
        });

        subtotalElement.innerText = new Intl.NumberFormat("vi-VN", { style: "currency", currency: "VND" }).format(total);
    }

    updateCartSubtotal();
});
document.addEventListener("DOMContentLoaded", function () {
    document.querySelector(".pay-btn").addEventListener("click", function () {
        const userId = this.getAttribute("data-user-id");
        let orderItems = [];

        document.querySelectorAll(".product-view__cart").forEach(row => {
            const productId = row.getAttribute("data-id");
            const productName = row.querySelector(".product-info span").textContent;
            const price = parseFloat(row.querySelector(".price").getAttribute("data-price"));
            const quantity = parseInt(row.querySelector(".qty-input").value);
            orderItems.push({ productId, productName, price, quantity });
        });

        const orderData = {
            userId: userId,
            orderItems: orderItems,
            totalPrice: orderItems.reduce((sum, item) => sum + item.price * item.quantity, 0)
        };

        sessionStorage.setItem("orderData", JSON.stringify(orderData));
        window.location.href = "/payment";
    });
});


