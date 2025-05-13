$(document).ready(function () {
    // Gọi loadMiniCartItems ngay khi trang tải để cập nhật số lượng trên logo giỏ hàng
    loadMiniCartItems();

    // Hiển thị hoặc ẩn Mini Cart khi nhấp vào logo giỏ hàng
    $("#cart-toggle").click(function (e) {
        e.preventDefault();
        $("#miniCart").toggle();
        loadMiniCartItems();
    });

    // Ẩn Mini Cart khi nhấp vào nút đóng
    $("#close-cart").click(function (e) {
        e.preventDefault();
        $("#miniCart").hide();
    });

    // Thêm sản phẩm vào giỏ hàng từ trang danh sách sản phẩm
    $(".add-to-cart-btn").click(function (e) {
        e.preventDefault();
        const productId = $(this).data("product-id");
        const selectedSize = $(`input[name="selectedSize-${productId}"]:checked`).val();

        if (!selectedSize) {
            alert("Vui lòng chọn kích cỡ trước khi thêm vào giỏ hàng!");
            return;
        }

        $.ajax({
            url: "/Cart/AddToCart",
            method: "POST",
            data: { productId: productId, quantity: 1, selectedSize: selectedSize },
            success: function (response) {
                if (response.success) {
                    loadMiniCartItems();
                    alert("Đã thêm sản phẩm vào giỏ hàng!");
                } else {
                    alert("Có lỗi xảy ra: " + response.message);
                }
            },
            error: function (xhr, status, error) {
                alert("Lỗi kết nối đến server: " + error);
                console.log("Chi tiết lỗi:", xhr.responseText);
            }
        });
    });

    // Thêm sản phẩm vào giỏ hàng từ trang chi tiết sản phẩm
    $("#add-to-cart-form").on("submit", function (e) {
        e.preventDefault();
        const productId = $(this).data("product-id");
        const selectedSize = $("input[name='selectedSize']:checked").val();

        if (!selectedSize) {
            alert("Vui lòng chọn kích cỡ trước khi thêm vào giỏ hàng!");
            return;
        }

        $.ajax({
            url: "/Cart/AddToCart",
            method: "POST",
            data: { productId: productId, quantity: 1, selectedSize: selectedSize },
            success: function (response) {
                if (response.success) {
                    loadMiniCartItems();
                    alert("Đã thêm sản phẩm vào giỏ hàng!");
                } else {
                    alert("Có lỗi xảy ra: " + response.message);
                }
            },
            error: function (xhr, status, error) {
                alert("Lỗi kết nối đến server: " + error);
                console.log("Chi tiết lỗi:", xhr.responseText);
            }
        });
    });

    // Xóa sản phẩm khỏi Mini Cart
    $(document).on("click", ".product-item_remove", function (e) {
        e.preventDefault();
        const cartId = $(this).data("id");

        $.ajax({
            url: "/Cart/RemoveFromCart",
            method: "POST",
            data: { cartId: cartId },
            success: function (response) {
                if (response.success) {
                    loadMiniCartItems();
                    alert("Đã xóa sản phẩm khỏi giỏ hàng!");
                } else {
                    alert("Có lỗi xảy ra: " + response.message);
                }
            },
            error: function (xhr, status, error) {
                alert("Lỗi kết nối đến server: " + error);
                console.log("Chi tiết lỗi:", xhr.responseText);
            }
        });
    });

    // Tải dữ liệu Mini Cart từ server
    function loadMiniCartItems() {
        $.get("/Cart/GetMiniCartItems", function (data) {
            $("#cart-items").empty();
            let subtotal = 0;

            console.log("Dữ liệu Mini Cart:", data);

            data.forEach(item => {
                const price = Number(item.priceValue) || 0;
                const quantity = Number(item.quantity) || 0;
                subtotal += price * quantity;

                $("#cart-items").append(`
                    <li class="minicart-product">
                        <a class="product-item_remove" href="#" data-id="${item.id}"><i class="fa-solid fa-xmark"></i></a>
                        <a href="/Product/Details/${item.productId}" class="product-item_img">
                            <img class="img-full" src="${item.imageUrl}" alt="${item.name}" onerror="this.src='/images/default-product.jpg'">
                        </a>
                        <div class="product-item_content">
                            <a class="product-item_title" href="/Product/Details/${item.productId}">${item.name}</a>
                            <span class="product-item_quantity">${quantity} x ${item.price} ${item.size ? `(Size: ${item.size})` : ''}</span>
                        </div>
                    </li>
                `);
            });

            $("#cart-subtotal").text(`Tổng cộng: ${subtotal.toLocaleString('vi-VN')} đ`);
            $("#cart-count").text(data.length);
        }).fail(function (xhr, status, error) {
            console.error("Lỗi khi tải Mini Cart:", error);
            $("#cart-subtotal").text("Tổng cộng: 0 đ");
            $("#cart-count").text("0");
        });
    }

    // Thêm/Xóa sản phẩm khỏi danh sách yêu thích (Toggle)
    $(document).on("click", ".favorite-btn", function (e) {
        e.preventDefault();
        const $btn = $(this);
        const productId = $btn.data("product-id");
        const isFavorited = $btn.hasClass("favorited");

        const url = isFavorited ? "/Favorites/RemoveFromFavorites" : "/Favorites/AddToFavorites";
        const actionMessage = isFavorited ? "Xóa khỏi yêu thích" : "Thêm vào yêu thích";

        $.ajax({
            url: url,
            method: "POST",
            data: { productId: productId },
            success: function (response) {
                if (response.success) {
                    $btn.toggleClass("favorited");
                    $btn.attr("title", isFavorited ? "Thêm vào yêu thích" : "Xóa khỏi yêu thích");
                    alert(response.message || `${actionMessage} thành công!`);
                } else {
                    alert(response.message || `Có lỗi khi ${actionMessage.toLowerCase()}!`);
                }
            },
            error: function (xhr, status, error) {
                alert("Lỗi kết nối đến server: " + error);
                console.log("Chi tiết lỗi:", xhr.responseText);
            }
        });
    });

    // Xóa sản phẩm khỏi danh sách yêu thích trong trang Favorites
    $(document).on("click", ".remove-favorite-btn", function (e) {
        e.preventDefault();
        const $btn = $(this);
        const productId = $btn.data("product-id");
        const $row = $btn.closest(".product-view__favorites");

        if (confirm("Bạn có chắc muốn xóa sản phẩm này khỏi danh sách yêu thích?")) {
            $.ajax({
                url: "/Favorites/RemoveFromFavorites",
                method: "POST",
                data: { productId: productId },
                success: function (response) {
                    if (response.success) {
                        $row.remove();
                        alert("Đã xóa sản phẩm khỏi danh sách yêu thích!");
                    } else if (response.message === "Sản phẩm không có trong danh sách yêu thích!") {
                        $row.remove();
                        alert("Sản phẩm đã được xóa khỏi danh sách yêu thích trước đó!");
                    } else {
                        alert("Có lỗi xảy ra: " + response.message);
                    }

                    if ($(".favorites-table tbody tr").length === 0) {
                        $(".favorites-container").html(`
                            <div class="empty-favorites">
                                <svg width="80" height="80" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" id="Capa_1" x="0px" y="0px" viewBox="0 0 201.387 201.387" style="enable-background:new 0 0 201.387 201.387;" xml:space="preserve">
                                    <g>
                                        <g>
                                            <path d="M129.413,24.885C127.389,10.699,115.041,0,100.692,0C91.464,0,82.7,4.453,77.251,11.916    c-1.113,1.522-0.78,3.657,0.742,4.77c1.517,1.109,3.657,0.78,4.768-0.744c4.171-5.707,10.873-9.115,17.93-9.115    c10.974,0,20.415,8.178,21.963,19.021c0.244,1.703,1.705,2.932,3.376,2.932c0.159,0,0.323-0.012,0.486-0.034    C128.382,28.479,129.679,26.75,129.413,24.885z"></path>
                                        </g>
                                    </g>
                                    <g>
                                        <g>
                                            <path d="M178.712,63.096l-10.24-17.067c-0.616-1.029-1.727-1.657-2.927-1.657h-9.813c-1.884,0-3.413,1.529-3.413,3.413    s1.529,3.413,3.413,3.413h7.881l6.144,10.24H31.626l6.144-10.24h3.615c1.884,0,3.413-1.529,3.413-3.413s-1.529-3.413-3.413-3.413    h-5.547c-1.2,0-2.311,0.628-2.927,1.657l-10.24,17.067c-0.633,1.056-0.648,2.369-0.043,3.439s1.739,1.732,2.97,1.732h150.187    c1.231,0,2.364-0.662,2.97-1.732S179.345,64.15,178.712,63.096z"></path>
                                        </g>
                                    </g>
                                    <g>
                                        <g>
                                            <path d="M161.698,31.623c-0.478-0.771-1.241-1.318-2.123-1.524l-46.531-10.883c-0.881-0.207-1.809-0.053-2.579,0.423    c-0.768,0.478-1.316,1.241-1.522,2.123l-3.509,15c-0.43,1.835,0.71,3.671,2.546,4.099c1.835,0.43,3.673-0.71,4.101-2.546    l2.732-11.675l39.883,9.329l-6.267,26.795c-0.43,1.835,0.71,3.671,2.546,4.099c0.263,0.061,0.524,0.09,0.782,0.09    c1.55,0,2.953-1.062,3.318-2.635l7.045-30.118C162.328,33.319,162.176,32.391,161.698,31.623z"></path>
                                        </g>
                                    </g>
                                    <g>
                                        <g>
                                            <path d="M102.497,39.692l-3.11-26.305c-0.106-0.899-0.565-1.72-1.277-2.28c-0.712-0.56-1.611-0.816-2.514-0.71l-57.09,6.748    c-1.871,0.222-3.209,1.918-2.988,3.791l5.185,43.873c0.206,1.737,1.679,3.014,3.386,3.014c0.133,0,0.27-0.009,0.406-0.024    c1.87-0.222,3.208-1.918,2.988-3.791l-4.785-40.486l50.311-5.946l2.708,22.915c0.222,1.872,1.91,3.202,3.791,2.99    C101.379,43.261,102.717,41.564,102.497,39.692z"></path>
                                        </g>
                                    </g>
                                    <g>
                                        <g>
                                            <path d="M129.492,63.556l-6.775-28.174c-0.212-0.879-0.765-1.64-1.536-2.113c-0.771-0.469-1.696-0.616-2.581-0.406L63.613,46.087    c-1.833,0.44-2.961,2.284-2.521,4.117l3.386,14.082c0.44,1.835,2.284,2.964,4.116,2.521c1.833-0.44,2.961-2.284,2.521-4.117    l-2.589-10.764l48.35-11.626l5.977,24.854c0.375,1.565,1.775,2.615,3.316,2.615c0.265,0,0.533-0.031,0.802-0.096    C128.804,67.232,129.932,65.389,129.492,63.556z"></path>
                                        </g>
                                    </g>
                                    <g>
                                        <g>
                                            <path d="M179.197,64.679c-0.094-1.814-1.592-3.238-3.41-3.238H25.6c-1.818,0-3.316,1.423-3.41,3.238l-6.827,133.12    c-0.048,0.934,0.29,1.848,0.934,2.526c0.645,0.677,1.539,1.062,2.475,1.062h163.84c0.935,0,1.83-0.384,2.478-1.062    c0.643-0.678,0.981-1.591,0.934-2.526L179.197,64.679z M22.364,194.56l6.477-126.293h143.701l6.477,126.293H22.364z"></path>
                                        </g>
                                    </g>
                                    <g>
                                        <g>
                                            <path d="M126.292,75.093c-5.647,0-10.24,4.593-10.24,10.24c0,5.647,4.593,10.24,10.24,10.24c5.647,0,10.24-4.593,10.24-10.24    C136.532,79.686,131.939,75.093,126.292,75.093z M126.292,88.747c-1.883,0-3.413-1.531-3.413-3.413s1.531-3.413,3.413-3.413    c1.882,0,3.413,1.531,3.413,3.413S128.174,88.747,126.292,88.747z"></path>
                                        </g>
                                    </g>
                                    <g>
                                        <g>
                                            <path d="M75.092,75.093c-5.647,0-10.24,4.593-10.24,10.24c0,5.647,4.593,10.24,10.24,10.24c5.647,0,10.24-4.593,10.24-10.24    C85.332,79.686,80.739,75.093,75.092,75.093z M75.092,88.747c-1.882,0-3.413-1.531-3.413-3.413s1.531-3.413,3.413-3.413    s3.413,1.531,3.413,3.413S76.974,88.747,75.092,88.747z"></path>
                                        </g>
                                    </g>
                                    <g>
                                        <g>
                                            <path d="M126.292,85.333h-0.263c-1.884,0-3.413,1.529-3.413,3.413c0,0.466,0.092,0.911,0.263,1.316v17.457    c0,12.233-9.953,22.187-22.187,22.187s-22.187-9.953-22.187-22.187V88.747c0-1.884-1.529-3.413-3.413-3.413    s-3.413,1.529-3.413,3.413v18.773c0,15.998,13.015,29.013,29.013,29.013s29.013-13.015,29.013-29.013V88.747    C129.705,86.863,128.176,85.333,126.292,85.333z"></path>
                                        </g>
                                    </g>
                                </svg>
                                <p class="empty-favorites-message">Không có sản phẩm nào trong danh sách yêu thích của bạn</p>
                            </div>
                        `);
                    }
                },
                error: function (xhr, status, error) {
                    alert("Lỗi kết nối đến server: " + error);
                    console.log("Chi tiết lỗi:", xhr.responseText);
                }
            });
        }
    });

    // Xử lý nút Apply Filters
    $("#filter-form").on("submit", function (e) {
        // Không chặn submit để form gửi dữ liệu
        console.log("Filter form submitted: " + $(this).serialize());
    });
    $("#reset-filters").on("click", function (e) {
        e.preventDefault();
        // Bỏ chọn tất cả radio button trong form
        $("#filter-form input[type='radio']").prop("checked", false);
        // Submit form để gửi yêu cầu với các giá trị trống
        $("#filter-form").submit();
    });
});