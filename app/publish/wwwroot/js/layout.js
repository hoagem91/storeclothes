document.addEventListener("DOMContentLoaded", function () {
    let searchContainer = document.querySelector(".search-container");
    let searchInput = document.getElementById("searchInput");
    let searchToggle = document.querySelector(".search-toggle");
    let userIcon = document.querySelector(".user-icon");
    let userDropdown = document.querySelector(".user-dropdown");

    // Ẩn ô tìm kiếm mặc định
    searchContainer.classList.remove("active");

    // Toggle ô tìm kiếm khi nhấn vào icon search
    searchToggle.addEventListener("click", function (event) {
        event.preventDefault();
        searchContainer.classList.toggle("active");
        if (searchContainer.classList.contains("active")) {
            searchInput.focus();
            adjustSearchWidth();
        } else {
            searchInput.value = ""; // Xóa nội dung khi đóng
        }
    });

    // Đóng ô tìm kiếm khi nhấn phím Esc
    document.addEventListener("keydown", function (event) {
        if (event.key === "Escape" && searchContainer.classList.contains("active")) {
            searchContainer.classList.remove("active");
            searchInput.value = "";
        }
    });

    // Điều chỉnh chiều rộng của ô tìm kiếm theo nội dung nhập
    searchInput.addEventListener("input", adjustSearchWidth);

    // Tìm kiếm sản phẩm khi nhấn Enter
    searchInput.addEventListener("keypress", function (event) {
        if (event.key === "Enter") {
            searchProducts();
        }
    });

    function adjustSearchWidth() {
        searchInput.style.width = searchInput.value.length ? searchInput.scrollWidth + "px" : "150px";
    }

    // Toggle dropdown user khi nhấn vào icon user
    userIcon.addEventListener("click", function (event) {
        event.preventDefault();
        userDropdown.classList.toggle("active");
    });

    // Ẩn dropdown khi click bên ngoài
    document.addEventListener("click", function (event) {
        if (!userIcon.contains(event.target) && !userDropdown.contains(event.target)) {
            userDropdown.classList.remove("active");
        }
    });
});

function searchProducts() {
    let query = document.getElementById("searchInput").value;
    if (query.trim() !== "") {
        window.location.href = "/Product/Search?query=" + encodeURIComponent(query);
    }
}
