document.addEventListener("DOMContentLoaded", function () {
    // Điều hướng đến trang đăng ký khi click "Shop Now"
    const shopNowBtn = document.querySelector(".shop-now");
    if (shopNowBtn) {
        shopNowBtn.addEventListener("click", function () {
            window.location.href = "/Account/Register";
        });
    }

    // Filter sản phẩm theo danh mục
    const buttons = document.querySelectorAll(".filter-btn");
    const productCards = document.querySelectorAll(".product-card");

    if (buttons.length > 0) {
        buttons.forEach(button => {
            button.addEventListener("click", function () {
                const selectedCategory = this.getAttribute("data-category");

                // Reset tất cả nút về btn-light
                buttons.forEach(btn => {
                    btn.classList.remove("btn-dark", "active");
                    btn.classList.add("btn-light");
                });

                // Gán active cho nút được chọn
                this.classList.remove("btn-light");
                this.classList.add("btn-dark", "active");

                // Hiển thị sản phẩm theo danh mục
                productCards.forEach(card => {
                    const productCategory = card.getAttribute("data-category");
                    card.style.display = (productCategory === selectedCategory) ? "block" : "none";
                });
            });
        });

        // Kích hoạt danh mục mặc định nếu có
        const activeButton = document.querySelector(".filter-btn.active");
        if (activeButton) {
            activeButton.click();
        }
    }

    // Chuyển đổi giữa form đăng nhập & đăng ký
    const loginToggle = document.getElementById("loginToggle");
    const registerToggle = document.getElementById("registerToggle");
    const loginForm = document.getElementById("loginForm");
    const registerForm = document.getElementById("registerForm");

    if (loginToggle && registerToggle && loginForm && registerForm) {
        loginToggle.addEventListener("click", function () {
            loginForm.classList.remove("hidden");
            registerForm.classList.add("hidden");
            loginToggle.classList.add("active");
            registerToggle.classList.remove("active");
        });

        registerToggle.addEventListener("click", function () {
            registerForm.classList.remove("hidden");
            loginForm.classList.add("hidden");
            registerToggle.classList.add("active");
            loginToggle.classList.remove("active");
        });
    }

    // Chế độ Dark Mode có lưu trạng thái
    const toggleSwitch = document.getElementById("darkModeToggle");
    if (toggleSwitch) {
        if (localStorage.getItem("darkMode") === "enabled") {
            document.body.classList.add("dark-mode");
            toggleSwitch.checked = true;
        }

        toggleSwitch.addEventListener("change", function () {
            if (this.checked) {
                document.body.classList.add("dark-mode");
                localStorage.setItem("darkMode", "enabled");
            } else {
                document.body.classList.remove("dark-mode");
                localStorage.setItem("darkMode", "disabled");
            }
        });
    }

    // Nút View More điều hướng đến Login
    const viewMoreBtn = document.getElementById("btn-view-more");
    if (viewMoreBtn) {
        viewMoreBtn.addEventListener("click", function () {
            window.location.href = "/Login/Index";
        });
    }
});
