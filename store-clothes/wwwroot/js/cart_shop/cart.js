document.addEventListener("DOMContentLoaded", function () {
  const qtyButtons = document.querySelectorAll(".qty-btn");
  const subtotalElement = document.getElementById("cart-subtotal");

  if (!subtotalElement || qtyButtons.length === 0) {
    console.warn("Không có sản phẩm nào trong giỏ hàng.");
    return;
  }

  qtyButtons.forEach((button) => {
    button.addEventListener("click", function () {
      const id = this.getAttribute("data-id");
      const input = document.querySelector(`.qty-input[data-id='${id}']`);
      const priceElement = document.querySelector(
        `.total-price[data-id='${id}']`
      );

      if (!input || !priceElement) return;

      let quantity = parseInt(input.value) || 1;

      if (this.classList.contains("plus")) {
        quantity++;
      } else if (this.classList.contains("minus") && quantity > 1) {
        quantity--;
      }

      input.value = quantity;
      updateTotal(id, quantity);
      updateCartSubtotal();
    });
  });

  document.querySelectorAll(".qty-input").forEach((input) => {
    input.addEventListener("change", function () {
      const id = this.getAttribute("data-id");
      let quantity = parseInt(this.value);

      if (isNaN(quantity) || quantity < 1) {
        quantity = 1;
      }

      this.value = quantity;
      updateTotal(id, quantity);
    });
  });

  function updateTotal(id, quantity) {
    const priceElement = document.querySelector(
      `.total-price[data-id='${id}']`
    );
    const unitPrice = parseFloat(priceElement.getAttribute("data-price")) || 0;
    const totalPrice = unitPrice * quantity;

    priceElement.innerText = new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(totalPrice);

    updateCartSubtotal();
  }

  function updateCartSubtotal() {
    let total = 0;

    document.querySelectorAll(".total-price").forEach((priceElement) => {
      const priceText = priceElement.innerText.replace(/[^\d]/g, ""); // Lấy số từ giá tiền
      total += parseFloat(priceText) || 0;
    });

    document.getElementById("cart-subtotal").innerText = new Intl.NumberFormat(
      "vi-VN",
      {
        style: "currency",
        currency: "VND",
      }
    ).format(total);
  }

  updateCartSubtotal();
});
$(document).on("click", ".pay-btn", function (e) {
  e.preventDefault();

  let cartItems = [];

  $(".product-view__cart").each(function () {
    const quantity = parseInt($(this).find(".qty-input").val());
    const priceValue = parseFloat($(this).find(".total-price").data("price"));
    const name = $(this).find(".product-name").text().trim();
    const productId = $(this).find(".qty-input").data("id");
    const imageUrl = $(this).find(".product-image").attr("src");

    const item = {
      productId: productId,
      quantity: quantity,
      name: name,
      price: priceValue.toLocaleString("vi-VN") + " ₫",
      priceValue: priceValue,
      imageUrl: imageUrl,
      size: "M",
    };

    console.log(item);
    cartItems.push(item);
  });

  if (cartItems.length === 0) {
    alert("Giỏ hàng của bạn đang trống.");
    return;
  }

  //alert("Giỏ hàng của bạn:\n" + JSON.stringify(cartItems, null, 2));

  if (!confirm("Bạn có chắc chắn muốn tiếp tục thanh toán?")) {
    return;
  }

  $.ajax({
    url: "/Payments/StartCheckout",
    type: "POST",
    contentType: "application/json",
    data: JSON.stringify(cartItems),
    success: function (res) {
      if (res.success) {
        window.location.href = "/Payments";
      } else {
        alert("Không thể bắt đầu thanh toán.");
      }
    },
    error: function () {
      alert("Lỗi khi gửi dữ liệu giỏ hàng.");
    },
  });
});

// Xử lý xóa sản phẩm
$(document).on("click", ".delete-btn", function () {
  const cartItemId = $(this).data("id");

  if (confirm("Bạn có chắc chắn muốn xóa sản phẩm này khỏi giỏ hàng?")) {
    $.ajax({
      url: "/Cart/Delete",
      type: "POST",
      data: { id: cartItemId },
      success: function (response) {
        if (response.success) {
          // Xóa hàng chứa sản phẩm
          $(`tr.product-view__cart td[data-id="${cartItemId}"]`)
            .closest("tr")
            .remove();

          // Cập nhật tổng tiền
          updateCartSubtotal();

          // Kiểm tra nếu giỏ hàng trống
          if ($(".product-view__cart").length === 0) {
            location.reload(); // Tải lại trang để hiển thị giỏ hàng trống
          }
        } else {
          alert("Có lỗi xảy ra khi xóa sản phẩm!");
        }
      },
      error: function () {
        alert("Có lỗi xảy ra khi xóa sản phẩm!");
      },
    });
  }
});

// Xử lý xóa toàn bộ giỏ hàng
function clearCart() {
  if (confirm("Bạn có chắc chắn muốn xóa toàn bộ giỏ hàng?")) {
    $.ajax({
      url: "/Cart/ClearCart",
      type: "POST",
      success: function (response) {
        if (response.success) {
          // Xóa tất cả sản phẩm khỏi giao diện
          $(".product-view__cart").remove();

          // Cập nhật tổng tiền về 0
          $("#cart-subtotal").text("0 ₫");

          // Tải lại trang để hiển thị giỏ hàng trống
          location.reload();
        } else {
          alert(response.message || "Có lỗi xảy ra khi xóa giỏ hàng!");
        }
      },
      error: function () {
        alert("Có lỗi xảy ra khi xóa giỏ hàng!");
      },
    });
  }
}
