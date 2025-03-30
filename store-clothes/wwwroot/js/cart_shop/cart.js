document.addEventListener("DOMContentLoaded", function () {
    const qtyButtons = document.querySelectorAll(".qty-btn");
    const subtotalElement = document.getElementById("cart-subtotal");

    // Nếu không có sản phẩm nào, dừng script
    if (!subtotalElement || qtyButtons.length === 0) {
        console.warn("Không có sản phẩm nào trong giỏ hàng.");
        return;
    }

    qtyButtons.forEach(button => {
        button.addEventListener("click", function () {
            const id = this.getAttribute("data-id");
            const input = document.querySelector(`.qty-input[data-id='${id}']`);

            if (!input) return;

            let quantity = parseInt(input.value);

            if (this.classList.contains("plus")) {
                quantity++;
            } else if (this.classList.contains("minus") && quantity > 1) {
                quantity--;
            }

            input.value = quantity;
            updateTotal(id, quantity);
        });
    });

    function updateTotal(id, quantity) {
        const priceElement = document.querySelector(`.total-price[data-id='${id}']`);
        if (!priceElement) return;

        const price = parseFloat(priceElement.getAttribute("data-price")) || 0;
        const total = price * quantity;

        const formattedTotal = new Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND"
        }).format(total);

        priceElement.innerText = formattedTotal;

        updateCartSubtotal();
    }

    function updateCartSubtotal() {
        let total = 0;

        document.querySelectorAll(".total-price").forEach(el => {
            const id = el.getAttribute("data-id");
            const price = parseFloat(el.getAttribute("data-price")) || 0;
            const quantityInput = document.querySelector(`.qty-input[data-id='${id}']`);

            if (!quantityInput) return;

            const quantity = parseInt(quantityInput.value) || 0;
            total += price * quantity;
        });

        const formattedSubtotal = new Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND"
        }).format(total);

        subtotalElement.innerText = formattedSubtotal;
    }

    updateCartSubtotal(); // Gọi khi tải trang
});
