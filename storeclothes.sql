
USE storeclothes;

-- Tạo bảng users
CREATE TABLE users (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `email` VARCHAR(45) NOT NULL,
  `password` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `email_UNIQUE` (`email`)
);

-- Chèn dữ liệu vào bảng users
INSERT INTO users (id, name, email, password) 
VALUES (1, 'Vu Hoang Anh', 'vana@example.com', '12345');

ALTER TABLE storeclothes.users MODIFY id INT AUTO_INCREMENT;
ALTER TABLE storeclothes.orders DROP FOREIGN KEY user_id;
ALTER TABLE storeclothes.cart DROP FOREIGN KEY fk_cart_user;
ALTER TABLE storeclothes.payments DROP FOREIGN KEY payments_user_id;
ALTER TABLE storeclothes.users AUTO_INCREMENT = 2;
SELECT MAX(id) FROM storeclothes.users;
DELETE FROM storeclothes.users;
ALTER TABLE storeclothes.users AUTO_INCREMENT = 1;
SET SQL_SAFE_UPDATES = 1;
ALTER TABLE storeclothes.payments ADD CONSTRAINT payments_user_id FOREIGN KEY (user_id) REFERENCES storeclothes.users(id);

CREATE TABLE storeclothes.categories (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `description` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
);

-- Chèn dữ liệu vào bảng categories
INSERT INTO categories (id, name, description) VALUES
(1, 'T-Shirt', 'Casual and comfortable short-sleeve shirt'),
(2, 'Ao Polo', 'Collared shirt, stylish and semi-formal'),
(3, 'Shirt', 'Formal or casual button-up shirt'),
(4, 'Hoodie', 'Warm and cozy hooded sweatshirt'),
(5, 'Jacket', 'Outerwear for warmth and style');


-- Tạo bảng products
CREATE TABLE products (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(225) NOT NULL,
  `description` TEXT,
  `price` DECIMAL(10,2) DEFAULT NULL,
  `category_id` INT DEFAULT NULL,
  `image_url` VARCHAR(225) DEFAULT NULL,
  `size` VARCHAR(50),
  PRIMARY KEY (`id`),
  KEY `category_id_idx` (`category_id`),
  CONSTRAINT `category_id` FOREIGN KEY (`category_id`) REFERENCES `categories` (`id`)
);


-- Chèn dữ liệu vào bảng products
INSERT INTO products (id, name, description, price, category_id, image_url, size) VALUES
(1, "Áo Thun Local Brand Unisex Teelab Seasonal Tshirt TS295", "Thông tin sản phẩm:\n- Chất liệu: Cotton\n- Form: Oversize\n- Màu sắc: Đen/Trắng/Nâu Nhạt/Xanh Navy\n- Thiết kế: in lụa\n\nMàu sắc: Trắng", 195000, 1, "/t-shirts/shirt-1.webp", "M, L, XL"),
(2, "Áo Thun Local Brand Unisex Teelab Summer Fresh Tshirt TS282", "Thông tin sản phẩm:\n- Chất liệu: Cotton\n- Form: Oversize\n- Màu sắc: Đen/Kem\n- Thiết kế: In lụa\n\nMàu sắc: Đen", 195000, 1, "/t-shirts/shirt-2.webp", "S, M, L"),
(3, "Áo Thun Dài Tay Local Brand Unisex Teelab Academy LongSleeve Tshirt TS292", "Thông tin sản phẩm:\n- Chất liệu: Cotton\n- Form: Oversize\n- Màu sắc: Xám Tiêu/Đen/Trắng/Xám Đậm\n- Thiết kế: In lụa\n\nMàu sắc: Xám Tiêu", 210000, 1, "/t-shirts/shirt-3.webp", "L, XL, XXL"),
(4, "Áo Thun Local Brand Unisex Teelab Dino Petite Tshirt TS287", "Thông tin sản phẩm:\n- Chất liệu: Cotton\n- Form: Oversize\n- Màu sắc: Xám Tiêu/Kem\n- Thiết kế: In lụa\n\nMàu sắc: Xám Tiêu", 195000, 1, "/t-shirts/shirt-4.webp", "M, L"),
(5, "Áo Thun Dài Tay Local Brand Unisex Teelab No.88 Jersey LongSleeve Tshirt TS290", "Thông tin sản phẩm:\n- Chất liệu: Vải thể thao\n- Form: Oversize\n- Màu sắc: Trắng/Đen\n- Thiết kế: In lụa\n\nMàu sắc: Trắng", 210000, 1, "/t-shirts/shirt-5.webp", "S, M, L, XL"),
(6, "Áo Thun Teelab Local Brand Unisex Royal Jersey wash Tshirt TS281", "Thông tin sản phẩm:\n- Chất liệu: Cotton\n- Form: Oversize\n- Màu sắc: Đen\n- Thiết kế: In lụa\n\nMàu sắc: Wash", 210000, 1, "/t-shirts/shirt-6.webp", "M, L, XL, XXL"),
(7, "Áo Thun Teelab Local Brand Unisex Carita Feliz Flower Tshirt TS279", "Thông tin sản phẩm:\n- Chất liệu: Cotton\n- Form: Oversize\n- Màu sắc: Đỏ/Xanh/Vàng\n- Thiết kế: Thêu.", 200000, 1, "/t-shirts/shirt-7.webp", "S, M"),
(8, "Áo Thun Teelab Local Brand Unisex No.27 Baseball Jersey Tshirt TS283", "Thông tin sản phẩm:\n- Chất liệu: Vải thể thao\n- Form: Oversize\n- Màu sắc: Trắng\n- Thiết kế: In lụa\n\nMàu sắc: Trắng", 195000, 1, "/t-shirts/shirt-8.webp", "M, L"),
(9, "Áo Thun Teelab Local Brand Unisex Athletic club Tshirt TS275", "Thông tin sản phẩm:\n- Chất liệu: Cotton\n- Form: Oversize\n- Màu sắc: Đen/Đỏ\n- Thiết kế: in lụa.\n\nMàu sắc: Đỏ", 195000, 1, "/t-shirts/shirt-9.webp", "L, XL"),
(10, "Áo Thun Teelab Local Brand Unisex Struck by Cupid Tshirt TS273", "Thông tin sản phẩm:\n- Chất liệu: Cotton\n- Form: Oversize\n- Màu sắc: Wash\n- Thiết kế: in lụa\n\nMàu sắc: Wash", 210000, 1, "/t-shirts/shirt-10.webp", "M, L, XL, XXL"),
(11, "Áo Thun Teelab Local Brand Unisex N0.19 Jersey Soccer Tshirt TS276", "Thông tin sản phẩm:\n- Chất liệu: Vải thể thao\n- Form: Oversize\n- Màu sắc: Trắng\n- Thiết kế: In lụa\n\nMàu sắc: Trắng", 195000, 1, "/t-shirts/shirt-11.webp", "S, M, XL"),
(12, "Áo Thun Teelab Local Brand Unisex Kansai Wave Tshirt TS271", "Thông tin sản phẩm:\n- Chất liệu: Cotton\n- Form: Oversize\n- Màu sắc: Xám/Nâu/Đen\n- Thiết kế: Thêu\n\nMàu sắc: Xám", 210000, 1, "/t-shirts/shirt-12.webp", "M, L, XXL"),
(13, "Áo Polo Teelab Local Brand Unisex Contrast Collar Metalic Symbol TLB Polo Shirt AP054", "Thông tin sản phẩm:\n- Chất liệu: TC cá sấu\n- Form: Oversize\n- Màu sắc: Xám/Đen\n- Thiết kế: In lụa\n\nMàu sắc: Xám", 195000, 2, "/polo-shirt/polo-1.webp", "S, L, XL"),
(14, "Áo Polo Teelab Local Brand Unisex Sporty Stripes Royal Club AP061", "Thông tin sản phẩm:\n- Chất liệu: Vải lưới thể thao\n- Form: Oversize\n- Màu sắc: Đen/Xanh/Hồng\n- Thiết kế: In lụa\n\nMàu sắc: Đen", 195000, 2, "/polo-shirt/polo-2.webp", "L, XL, XXL"),
(15, "Áo Polo Teelab Local Brand Unisex Tyrannosaurus Polo shirt AP060", "Thông tin sản phẩm:\n- Chất liệu: Cotton\n- Form: Oversize\n- Màu sắc: Xám\n- Thiết kế: In lụa\n\nMàu sắc: Xám Melane", 195000, 2, "/polo-shirt/polo-3.webp", "S, M"),
(16, "Áo Polo Teelab Local Brand Unisex Flame AP055", "Thông tin sản phẩm:\n- Chất liệu Messrs: Cotton\n- Form: Oversize\n- Màu sắc: Đen\n- Thiết kế: In lụa\n\nMàu sắc: Đen", 195000, 2, "/polo-shirt/polo-4.webp", "M, L"),
(17, "Áo Polo Teelab Local Brand Unisex Stripe Lines Jersey AP058", "Thông tin sản phẩm:\n- Chất liệu: Vải thể thao\n- Form: Oversize\n- Màu sắc: Đen/Xanh/Đỏ\n- Thiết kế: In lụa\n\nMàu sắc: Đen", 195000, 2, "/polo-shirt/polo-5.webp", "S, XL"),
(18, "Áo Polo Teelab Local Brand Unisex Football Vintage Polo Shirt AP053", "Thông tin sản phẩm:\n- Chất liệu: Vải thể thao\n- Form: Oversize\n- Màu sắc: Đen/Hồng\n- Thiết kế: In lụa\n\nMàu sắc: Đen", 175000, 2, "/polo-shirt/polo-6.webp", "M, XL"),
(19, "Áo Polo Teelab Local Brand Unisex Racing Master Polo Shirt AP049", "Thông tin sản phẩm:\n- Chất liệu: Cotton\n- Form: Oversize\n- Màu sắc: Kem/Đen/Xám\n- Thiết kế: In lụa\n\nMàu sắc: Kem", 195000, 2, "/polo-shirt/polo-7.webp", "S, M, L"),
(20, "Áo Babytee Polo Teelab Local Brand Unisex Studio Track Baby BT012", "Thông tin sản phẩm:\n- Chất liệu: Cotton\n- Form: Oversize\n- Màu sắc: Đen/Trắng\n- Thiết kế: In lụa\n\nMàu sắc: Trắng", 195000, 2, "/polo-shirt/polo-8.webp", "L, XL, XXL"),
(21, "Áo Sơ Mi Dài Tay Teelab Local Brand Unisex Eco Oxford Mandarin Shirt SS073", "Thông tin sản phẩm:\n- Chất liệu: Vải Oxford\n- Form: Oversize\n- Màu sắc: Trắng/Xanh Dương/Xanh Than\n- Thiết kế: In lụa.\n\nMàu sắc: Trắng", 219000, 3, "/shirts/somi-1.webp", "M, L"),
(22, "Áo Sơ Mi Cộc Tay Teelab Local Brand Unisex Ocean Creatures Shirt SS072", "Thông tin sản phẩm:\n- Chất liệu: Vải Oxford\n- Form: Oversize\n- Màu sắc: Xanh/Hồng\n- Thiết kế: Thêu.\n\nMàu sắc: Xanh", 219000, 3, "/shirts/somi-2.webp", "S, M, L, XL"),
(23, "Áo Sơ Mi Cộc Tay Teelab Local Brand Unisex Eco Oxford Logo Signature Shirt SS068", "Thông tin sản phẩm:\n- Chất liệu: Vải Oxford\n- Form: Oversize\n- Màu sắc: Đen/Hồng/Xanh Than/Xanh Dương/Trắng\n- Thiết kế: In lụa.\n\nMàu sắc: Đen", 210000, 3, "/shirts/somi-3.webp", "M, XL"),
(24, "Áo Sơ Mi Dài Tay Teelab Local Brand Unisex Oxford shirts SS066", "Thông tin sản phẩm:\n- Chất liệu: Vải Oxford\n- Form: Oversize\n- Màu sắc: Trắng/Đen/Xanh Than/Xanh Dương\n- Thiết kế: Thêu\n\nMàu sắc: Trắng", 219000, 3, "/shirts/somi-4.webp", "S, L, XXL"),
(25, "Áo Sơ Mi Ngắn Tay Teelab Local Brand Unisex Studio Oxford Shirt SS052", "Thông tin sản phẩm:\n- Chất liệu: Vải Oxford\n- Form: Oversize\n- Màu sắc: Hồng\n- Thiết kế: Thêu\n\nMàu sắc: Hồng", 210000, 3, "/shirts/somi-5.webp", "M, L, XL"),
(26, "Áo Sơ Mi Ngắn Tay Teelab Graffiti Oversize Shirt SS049", "Thông tin sản phẩm:\n- Chất liệu: Vải Oxford\n- Form: Oversize\n- Màu sắc: Xanh\n- Thiết kế: In lụa.\n\nMàu sắc: Xanh", 210000, 3, "/shirts/somi-6.webp", "L, XL"),
(27, "Áo Sơ Mi Teelab Symbol Basic Logo SS047", "Thông tin sản phẩm:\n- Chất liệu: Vải Oxford\n- Form: Oversize\n- Màu sắc: Xanh\n- Thiết kế: In lụa.\n\nMàu sắc: Xanh", 219000, 3, "/shirts/somi-7.webp", "S, M, L"),
(28, "Áo Hoodie Teelab Local Brand Unisex Bunny Cake Hoodie HD107", "Thông tin sản phẩm:\n- Chất liệu: Nỉ\n- Form: Oversize\n- Màu sắc: Kem/Xám Tiêu\n- Thiết kế: In lụa\nMàu sắc: Kem", 275000, 4, "/hoddie/hoddie-1.webp", "M, L, XXL"),
(29, "Áo Sweatshirt Local Brand Unisex Teelab Love is in the air Sweatshirt HD109", "Thông tin sản phẩm:\n- Chất liệu: Nỉ\n- Form: Oversize\n- Màu sắc: Kem/Xám Tiêu\n- Thiết kế: In lụa\nMàu sắc: Kem", 269000, 4, "/hoddie/hoddie-2.webp", "S, XL"),
(30, "Áo Hoodie Zipup Local Brand Unisex Teelab Classic Hoodie Zipup Boxy HD100", "Thông tin sản phẩm:\n- Chất liệu: Nỉ bông\n- Form: Oversize\n- Màu sắc: Đen/Xanh Navy/Xám Tiêu/Xám Đậm\n- Thiết kế: In lụa\nMàu sắc: Đen", 275000, 4, "/hoddie/hoddie-3.webp", "M, L, XL, XXL"),
(31, "Áo Hoodie Local Brand Unisex Teelab Tulipvalley Hoodie HD105", "Thông tin sản phẩm:\n- Chất liệu: Nỉ\n- Form: Oversize\n- Màu sắc: Đen/Kem/Xám Tiêu\n- Thiết kế: In lụa\nMàu sắc: Kem", 350000, 4, "/hoddie/hoddie-4.webp", "S, M"),
(32, "Áo Hoodie Local Brand Unisex Teelab Dino Christmas Hoodie HD098", "Thông tin sản phẩm:\n- Chất liệu: Nỉ\n- Form: Oversize\n- Màu sắc: Kem/Xám Tiêu\n- Thiết kế: In lụa\nMàu sắc: Kem", 330000, 4, "/hoddie/hoddie-5.webp", "L, XL"),
(33, "Áo Hoodie Worldwide Studio Collection Teelab Local Brand Unisex Form Oversize HD091", "Thông tin sản phẩm:\n- Chất liệu: Nỉ bông\n- Form: Oversize\n- Màu sắc: Nâu/Hồng/Tím/Kem/Đen/Xám Tiêu/Be/Xám Đậm\n- Thiết kế: In cao thành\nMàu sắc: Đen", 330000, 4, "/hoddie/hoddie-6.webp", "M, L"),
(34, "Áo Hoodie Teelab Local Brand Unisex PREMIUM Basic 8 Màu Thêu Logo Basic HD074", "Thông tin sản phẩm:\n- Chất liệu: Nỉ bông\n- Form: Oversize\n- Thiết kế: Th spazzêu\nMàu sắc: Đen", 275000, 4, "/hoddie/hoddie-7.webp", "S, M, L"),
(35, "Áo Hoodie Teelab Local Brand Unisex Morning Star Bunny HD062", "Thông tin sản phẩm:\n- Chất liệu: Nỉ bông 360gsm\n- Form: Oversize\n- Màu sắc: Kem\n- Thiết kế: Thêu\nMàu sắc: Kem", 330000, 4, "/hoddie/hoddie-8.webp", "L, XXL"),
(36, "Áo Hoodie Zip Teelab Local Brand Unisex Basic Kéo Khóa HD036", "Thông tin sản phẩm:\n- Chất liệu: Nỉ bông\n- Form: Oversize\n- Thiết kế: In lụa cao cấp\nColor: Kem", 249000, 4, "/hoddie/hoddie-9.webp", "S, M, XL"),
(37, "Áo khoác Local Brand Unisex Teelab Preppy Varsity Jacket AK122", "Thông tin sản phẩm:\n- Chất liệu: Dạ\n- Form: Oversize\n- Màu sắc: Xám Tiêu/Xanh Than\n- Thiết kế: Thêu\nMàu sắc: Xanh Than", 350000, 5, "/jacket/jacket-1.webp", "M, L, XL"),
(38, "Áo Khoác Len Local Brand Unisex Teelab 'Simple love with Teelab' AK125", "Thông tin sản phẩm:\n- Chất liệu: Len\n- Form: Oversize\n- Màu sắc: Đen/Kem\n- Thiết kế: Thêu\nMàu sắc: Đen", 299000, 5, "/jacket/jacket-2.webp", "L, XL, XXL"),
(39, "Áo khoác Local Brand Unisex Teelab Tech Pack Military Unlined Vest AK118", "Thông tin sản phẩm:\n- Chất liệu: Vải gió dù\n- Form: Oversize\n- Màu sắc: Đen/Xám/Kem\n- Thiết kế: In lụa.", 275000, 5, "/jacket/jacket-3.webp", "S, M, L, XL"),
(40, "Áo khoác Gió Local Brand Unisex Teelab Diode Insulated Jacket AK120", "Thông tin sản phẩm:\n- Chất liệu: Vải gió\n- Form: Oversize\n- Màu sắc: Đen/Xám/Nâu Nhạt\n- Thiết kế: In lụa.\nMàu sắc: Đen", 299000, 5, "/jacket/jacket-4.webp", "M, L, XXL"),
(41, "Áo khoác Local Brand Unisex Teelab Classic Varsity AK117", "Thông tin sản phẩm:\n- Chất liệu: Dạ\n- Form: Oversize\n- Màu sắc: Đen/Be/Xanh Than/Xám Đậm\n- Thiết kế: Thêu.\nMàu sắc: Đen", 320000, 5, "/jacket/jacket-5.webp", "S, L, XL");

-- Tạo bảng orders
CREATE TABLE orders (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT DEFAULT NULL,
  `total_price` DECIMAL(10,2) NOT NULL,
  `status` VARCHAR(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `user_id_idx` (`user_id`),
  CONSTRAINT `user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`)
);

-- Tạo bảng orders_items
CREATE TABLE orders_items (
  `id` INT NOT NULL AUTO_INCREMENT,
  `order_id` INT DEFAULT NULL,
  `product_id` INT DEFAULT NULL,
  `quantity` INT NOT NULL,
  `price` DECIMAL(10,2) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `order_id_idx` (`order_id`),
  KEY `product_id_idx` (`product_id`),
  CONSTRAINT `order_id` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`),
  CONSTRAINT `product_id` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`)
);


-- Tạo bảng cart
CREATE TABLE cart (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT DEFAULT NULL,
  `product_id` INT DEFAULT NULL,
  `quantity` INT NOT NULL,
  PRIMARY KEY (`id`),
  KEY `user_id_idx` (`user_id`),
  KEY `product_id_idx` (`product_id`),
  CONSTRAINT `fk_cart_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`),
  CONSTRAINT `fk_cart_product` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`)
);
-- cart-item để tạm lưu trữ đơn hàng được thanh toán 
CREATE TABLE CartItem (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL CHECK (quantity > 0),
    price DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE
);
-- cách join bảng payment với cart để lấy thông tin sản phẩm trong giỏ hàng
SELECT 
    c.id AS cartID, 
    p.name AS ProductName, 
    p.price, 
    c.quantity
FROM storeclothes.cart c
JOIN storeclothes.products p ON c.product_id = p.id
WHERE c.user_id = 1;

-- Tạo bảng payments
CREATE TABLE payments (
  `id` INT NOT NULL AUTO_INCREMENT,
  `order_id` INT DEFAULT NULL,
  `user_id` INT DEFAULT NULL,
  `paymet_method` ENUM('credit_card', 'paypal', 'cod') NOT NULL,
  `payment_status` ENUM('pending', 'completed', 'failed') DEFAULT 'pending',
  `transaction_id` VARCHAR(225) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `transaction_id_UNIQUE` (`transaction_id`),
  KEY `order_id_idx` (`order_id`),
  KEY `user_id_idx` (`user_id`),
  CONSTRAINT `payments_order_id` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`),
  CONSTRAINT `payments_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`)
);

-- Tạo bảng favorites
CREATE TABLE favorites (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT NOT NULL,
  `product_id` INT NOT NULL,
  `created_at` TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `user_id_idx` (`user_id`),
  KEY `product_id_idx` (`product_id`),
  CONSTRAINT `fk_favorites_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE,
  CONSTRAINT `fk_favorites_product` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`) ON DELETE CASCADE,
  UNIQUE KEY `user_product_unique` (`user_id`, `product_id`)
);

-- Xem dữ liệu bảng users
SELECT * FROM users;