document.addEventListener("DOMContentLoaded", function () {
    const qtyButtons = document.querySelectorAll(".qty-btn");

    qtyButtons.forEach(button => {
        button.addEventListener("click", function () {
            const id = this.getAttribute("data-id");
            const input = document.querySelector(`.qty-input[data-id='${id}']`);
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
        const price = parseFloat(priceElement.getAttribute("data-price"));
        priceElement.innerText = `$${(price * quantity).toFixed(2)}`;

        updateCartSubtotal();
    }

    function updateCartSubtotal() {
        let total = 0;
        document.querySelectorAll(".total-price").forEach(el => {
            total += parseFloat(el.innerText.replace("$", ""));
        });
        document.getElementById("cart-subtotal").innerText = `$${total.toFixed(2)}`;
    }
});
