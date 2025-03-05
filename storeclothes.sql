CREATE DATABASE storeclothes;
CREATE TABLE storeclothes.users (
  `id` int NOT NULL,
  `name` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `email_UNIQUE` (`email`)
);
INSERT INTO storeclothes.users (id,name, email,password) 
VALUES (1,'Vu Hoang Anh', 'vana@example.com', "12345");

CREATE TABLE storeclothes.categories (
  `id` int NOT NULL,
  `name` varchar(45) NOT NULL,
  `description` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
);
INSERT INTO storeclothes.categories (id, name, description) VALUES
(1, 'T-Shirt', 'Casual and comfortable short-sleeve shirt'),
(2, 'Ao Polo', 'Collared shirt, stylish and semi-formal'),
(3, 'Shirt', 'Formal or casual button-up shirt'),
(4, 'Hoodie', 'Warm and cozy hooded sweatshirt'),
(5, 'Jacket', 'Outerwear for warmth and style');

CREATE TABLE storeclothes.products (
  `id` int NOT NULL,
  `name` varchar(225) NOT NULL,
  `description` text,
  `price` decimal(10,2) DEFAULT NULL,
  `category_id` int DEFAULT NULL,
  `image_url` varchar(225) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `category_id_idx` (`category_id`),
  CONSTRAINT `category_id` FOREIGN KEY (`category_id`) REFERENCES `categories` (`id`)
);
INSERT INTO storeclothes.products (id, name, description,price,category_id,image_url) VALUES
(1, "Áo khoác Local Brand Unisex Teelab Preppy Varsity Jacket AK122", "Thông tin sản phẩm:
- Chất liệu: Dạ
- Form: Oversize
- Màu sắc: Xám Tiêu/Xanh Than
- Thiết kế: Thêu
Màu sắc: Xanh Than", 350000, 5, "/jacket/jacket-1.webp"),

(2, "Áo Khoác Len Local Brand Unisex Teelab 'Simple love with Teelab' AK125", "Thông tin sản phẩm:
- Chất liệu: Len
- Form: Oversize
- Màu sắc: Đen/Kem
- Thiết kế: Thêu
Màu sắc: Đen", 299000, 5, "/jacket/jacket-2.webp"),

(3, "Áo khoác Local Brand Unisex Teelab Tech Pack Military Unlined Vest AK118", "Thông tin sản phẩm:
- Chất liệu: Vải gió dù
- Form: Oversize
- Màu sắc: Đen/Xám/Kem
- Thiết kế: In lụa.", 275000, 5, "/jacket/jacket-3.webp"),

(4, "Áo khoác Gió Local Brand Unisex Teelab Diode Insulated Jacket AK120", "Thông tin sản phẩm:
- Chất liệu: Vải gió
- Form: Oversize
- Màu sắc: Đen/Xám/Nâu Nhạt
- Thiết kế: In lụa.
Màu sắc: Đen", 299000, 5, "/jacket/jacket-4.webp"),

(5, "Áo khoác Local Brand Unisex Teelab Classic Varsity AK117", "Thông tin sản phẩm:
- Chất liệu: Dạ
- Form: Oversize
- Màu sắc: Đen/Be/Xanh Than/Xám Đậm
- Thiết kế: Thêu.
Màu sắc: Đen", 320000, 5, "/jacket/jacket-5.webp");

(
6,"Áo Thun Teelab Local Brand Unisex Royal Jersey wash Tshirt TS281","Thông tin sản phẩm:
- Chất liệu: Cotton
- Form: Oversize
- Màu sắc: Đen
- Thiết kế: In lụa

Màu sắc: Wash",210000,1,"assests\products\t-shirts\shirt-6.webp"
),

(
7,"Áo Thun Teelab Local Brand Unisex Carita Feliz Flower Tshirt TS279","Thông tin sản phẩm:
- Chất liệu: Cotton
- Form: Oversize
- Màu sắc: Đỏ/Xanh/Vàng
- Thiết kế: Thêu.",200000,1,"assests\products\t-shirts\shirt-7.webp"
),

(
8,"Áo Thun Teelab Local Brand Unisex No.27 Baseball Jersey Tshirt TS283","Thông tin sản phẩm:
- Chất liệu: Vải thể thao
- Form: Oversize
- Màu sắc: Trắng
- Thiết kế: In lụa

Màu sắc: Trắng",195000,1,"assests\products\t-shirts\shirt-8.webp"
 ),

(
9,"Áo Thun Teelab Local Brand Unisex Athletic club Tshirt TS275","Thông tin sản phẩm:
- Chất liệu: Cotton
- Form: Oversize
- Màu sắc: Đen/Đỏ
- Thiết kế: in lụa.

Màu sắc: Đỏ",195000,1,"assests\products\t-shirts\shirt-9.webp"
),

(
10,"Áo Thun Teelab Local Brand Unisex Struck by Cupid Tshirt TS273","Thông tin sản phẩm:
- Chất liệu: Cotton
- Form: Oversize
- Màu sắc: Wash
- Thiết kế: in lụa

Màu sắc: Wash",210000,1,"assests\products\t-shirts\shirt-10.webp"
),

(
11,"Áo Thun Teelab Local Brand Unisex N0.19 Jersey Soccer Tshirt TS276","Thông tin sản phẩm:
- Chất liệu: Vải thể thao
- Form: Oversize
- Màu sắc: Trắng
- Thiết kế: In lụa

Màu sắc: Trắng",195000,1,"assests\products\t-shirts\shirt-11.webp"
),

(
12,"Áo Thun Teelab Local Brand Unisex Kansai Wave Tshirt TS271","Thông tin sản phẩm:
- Chất liệu: Cotton
- Form: Oversize
- Màu sắc: Xám/Nâu/Đen
- Thiết kế: Thêu

Màu sắc: Xám",210000,1,"assests\products\t-shirts\shirt-12.webp"
),

(
13,"Áo Polo Teelab Local Brand Unisex Contrast Collar Metalic Symbol TLB Polo Shirt AP054","Thông tin sản phẩm:
- Chất liệu: TC cá sấu
- Form: Oversize
- Màu sắc: Xám/Đen
- Thiết kế: In lụa

Màu sắc: Xám",195000,2,"assests\products\polo-shirt\polo-1.webp"
),

(
14,"Áo Polo Teelab Local Brand Unisex Sporty Stripes Royal Club AP061","Thông tin sản phẩm:
- Chất liệu: Vải lưới thể thao
- Form: Oversize
- Màu sắc: Đen/Xanh/Hồng
- Thiết kế: In lụa

Màu sắc: Đen",195000,2,"assests\products\polo-shirt\polo-2.webp"
),

(
15,"Áo Polo Teelab Local Brand Unisex Tyrannosaurus Polo shirt AP060","Thông tin sản phẩm:
- Chất liệu:  Cotton
- Form: Oversize
- Màu sắc: Xám
- Thiết kế: In lụa

Màu sắc: Xám Melane",195000,2,"assests\products\polo-shirt\polo-3.webp"
),

(
16,"Áo Polo Teelab Local Brand Unisex Flame AP055","Thông tin sản phẩm:
- Chất liệu: Cotton
- Form: Oversize
- Màu sắc: Đen
- Thiết kế: In lụa

Màu sắc: Đen",195000,2,"assests\products\polo-shirt\polo-4.webp"
),

(
17,"Áo Polo Teelab Local Brand Unisex Stripe Lines Jersey AP058","Thông tin sản phẩm:
- Chất liệu: Vải thể thao
- Form: Oversize
- Màu sắc: Đen/Xanh/Đỏ
- Thiết kế: In lụa

Màu sắc: Đen
",195000,2,"assests\products\polo-shirt\polo-5.webp"
),

(
18,"Áo Polo Teelab Local Brand Unisex Football Vintage Polo Shirt AP053","Thông tin sản phẩm:
- Chất liệu: Vải thể thao
- Form: Oversize
- Màu sắc: Đen/Hồng
- Thiết kế: In lụa

Màu sắc: Đen
",175000,2,"assests\products\polo-shirt\polo-6.webp"
),

(
19,"Áo Polo Teelab Local Brand Unisex Racing Master Polo Shirt AP049","Thông tin sản phẩm:
- Chất liệu: Cotton
- Form: Oversize
- Màu sắc: Kem/Đen/Xám
- Thiết kế: In lụa

Màu sắc: Kem
",195000,2,"assests\products\polo-shirt\polo-7.webp"
),

(
20,"Áo Babytee Polo Teelab Local Brand Unisex Studio Track Baby BT012","Thông tin sản phẩm:
- Chất liệu: Cotton
- Form: Oversize
- Màu sắc: Đen/Trắng
- Thiết kế: In lụa

Màu sắc: Trắng
",195000,2,"assests\products\polo-shirt\polo-8.webp"
),

(
21,"Áo Sơ Mi Dài Tay Teelab Local Brand Unisex Eco Oxford Mandarin Shirt SS073","Thông tin sản phẩm:
- Chất liệu: Vải Oxford 
- Form: Oversize
- Màu sắc: Trắng/Xanh Dương/Xanh Than
- Thiết kế: In lụa.

Màu sắc: Trắng
",219000,3,"assests\products\shirts\somi-1.webp"
),

(
22,"Áo Sơ Mi Cộc Tay Teelab Local Brand Unisex Ocean Creatures Shirt SS072","Thông tin sản phẩm:
- Chất liệu: Vải Oxford
- Form: Oversize
- Màu sắc: Xanh/Hồng
- Thiết kế: Thêu.

Màu sắc: Xanh
",219000,3,"assests\products\shirts\somi-2.webp"
),

(
23,"Áo Sơ Mi Cộc Tay Teelab Local Brand Unisex Eco Oxford Logo Signature Shirt SS068","Thông tin sản phẩm:
- Chất liệu: Vải Oxford 
- Form: Oversize
- Màu sắc: Đen/Hồng/Xanh Than/Xanh Dương/Trắng
- Thiết kế: In lụa.

Màu sắc: Đen
",210000,3,"assests\products\shirts\somi-3.webp"
),

(
24,"Áo Sơ Mi Dài Tay Teelab Local Brand Unisex Oxford shirts SS066","Thông tin sản phẩm:
- Chất liệu: Vải Oxford 
- Form: Oversize
- Màu sắc: Trắng/Đen/Xanh Than/Xanh Dương
- Thiết kế: Thêu

Màu sắc: Trắng
",219000,3,"assests\products\shirts\somi-4.webp"
),

(
25,"Áo Sơ Mi Ngắn Tay Teelab Local Brand Unisex Studio Oxford Shirt SS052","Thông tin sản phẩm:
- Chất liệu: Vải Oxford 
- Form: Oversize
- Màu sắc: Hồng
- Thiết kế: Thêu

Màu sắc: Hồng
",210000,3,"assests\products\shirts\somi-5.webp"
),

(
26,"Áo Sơ Mi Ngắn Tay Teelab Graffiti Oversize Shirt SS049","Thông tin sản phẩm:

- Chất liệu: Vải Oxford
- Form: Oversize
- Màu sắc: Xanh
- Thiết kế: In lụa.

Màu sắc: Xanh
",210000,3,"assests\products\shirts\somi-6.webp"
),

(
27,"Áo Sơ Mi Teelab Symbol Basic Logo SS047","Thông tin sản phẩm:
- Chất liệu: Vải Oxford
- Form: Oversize
- Màu sắc: Xanh
- Thiết kế: In lụa.

Màu sắc: Xanh
",219000,3,"assests\products\shirts\somi-7.webp"
),

(28,"Áo Hoodie Teelab Local Brand Unisex Bunny Cake Hoodie HD107","Thông tin sản phẩm:
- Chất liệu: Nỉ
- Form: Oversize
- Màu sắc: Kem/Xám Tiêu
- Thiết kế: In lụa
Màu sắc: Kem",275000,4,"assests\products\hoddie\hoddie-1.webp"),

(29,"Áo Sweatshirt Local Brand Unisex Teelab  Love is in the air Sweatshirt HD109","Thông tin sản phẩm:
- Chất liệu: Nỉ
- Form: Oversize
- Màu sắc: Kem/Xám Tiêu
- Thiết kế: In lụa
Màu sắc: Kem",269000,4,"assests\products\hoddie\hoddie-2.webp"),

(30,"Áo Hoodie Zipup Local Brand Unisex Teelab Classic Hoodie Zipup Boxy HD100","Thông tin sản phẩm:
- Chất liệu: Nỉ bông
- Form: Oversize
- Màu sắc: Đen/Xanh Navy/Xám Tiêu/Xám Đậm
- Thiết kế: In lụa
Màu sắc: Đen",275000,4,"assests\products\hoddie\hoddie-3.webp"),

(31,"Áo Hoodie Local Brand Unisex Teelab Tulipvalley Hoodie HD105","Thông tin sản phẩm:
- Chất liệu: Nỉ
- Form: Oversize
- Màu sắc: Đen/Kem/Xám Tiêu
- Thiết kế: In lụa
Màu sắc: Kem",350000,4,"assests\products\hoddie\hoddie-4.webp"),

(32,"Áo Hoodie Local Brand Unisex Teelab Dino Christmas Hoodie HD098","Thông tin sản phẩm:
- Chất liệu: Nỉ
- Form: Oversize
- Màu sắc: Kem/Xám Tiêu
- Thiết kế: In lụa
Màu sắc: Kem",330000,4,"assests\products\hoddie\hoddie-5.webp"),

(33,"Áo Hoodie Worldwide Studio Collection Teelab Local Brand Unisex Form Oversize HD091","Thông tin sản phẩm:
- Chất liệu: Nỉ bông
- Form: Oversize
- Màu sắc: Nâu/Hồng/Tím/Kem/Đen/Xám Tiêu/Be/Xám Đậm
- Thiết kế: In cao thành
Màu sắc: Đen",330000,4,"assests\products\hoddie\hoddie-6.webp"),

(34,"Áo Hoodie Teelab Local Brand Unisex PREMIUM Basic 8 Màu Thêu Logo Basic HD074","Thông tin sản phẩm:
- Chất liệu: Nỉ bông
- Form: Oversize
- Thiết kế: Thêu
Màu sắc: Đen",275000,4,"assests\products\hoddie\hoddie-7.webp"),

(35,"Áo Hoodie Teelab Local Brand Unisex Morning Star Bunny HD062","Thông tin sản phẩm:
- Chất liệu: Nỉ bông 360gsm
- Form: Oversize
- Màu sắc: Kem
- Thiết kế: Thêu
Màu sắc: Kem",330000,4,"assests\products\hoddie\hoddie-8.webp"),

(36,"Áo Hoodie Zip Teelab Local Brand Unisex Basic Kéo Khóa HD036","Thông tin sản phẩm: 
- Chất liệu: Nỉ bông
- Form: Oversize
- Thiết kế: In lụa cao cấp
Color: Kem",249000,4,"assests\products\hoddie\hoddie-9.webp"),


(37,"Áo khoác Local Brand Unisex Teelab Preppy Varsity Jacket AK122","Thông tin sản phẩm:
- Chất liệu: Dạ
- Form: Oversize
- Màu sắc: Xám Tiêu/Xanh Than
- Thiết kế: Thêu
Màu sắc: Xanh Than",350000,5,"assests\products\jacket\jacket-1.webp"),


(38,"Áo Khoác Len Local Brand Unisex Teelab 'Simple love with Teelab' AK125","Thông tin sản phẩm:
- Chất liệu: Len
- Form: Oversize
- Màu sắc: Đen/Kem
- Thiết kế: Thêu
Màu sắc: Đen",299000,5,"assests\products\jacket\jacket-2.webp"),


(39,"Áo khoác Local Brand Unisex Teelab Tech Pack Military Unlined Vest AK118","Thông tin sản phẩm:
- Chất liệu: Vải gió dù
- Form: Oversize
- Màu sắc: Đen/Xám/Kem
- Thiết kế: In lụa.",275000,5,"assests\products\jacket\jacket-3.webp"),


(40,"Áo khoác Gió Local Brand Unisex Teelab Diode Insulated Jacket AK120","Thông tin sản phẩm:
- Chất liệu: Vải gió
- Form: Oversize
- Màu sắc: Đen/Xám/Nâu Nhạt
- Thiết kế: In lụa.
Màu sắc: Đen",299000,5,"assests\products\jacket\jacket-4.webp"),


(41,"Áo khoác Local Brand Unisex Teelab Classic Varsity AK117","Thông tin sản phẩm:
- Chất liệu: Dạ
- Form: Oversize
- Màu sắc: Đen/Be/Xanh Than/Xám Đậm
- Thiết kế: Thêu.
Màu sắc: Đen",320000,5,"assests\products\jacket\jacket-5.webp");

CREATE TABLE storeclothes.orders (
  `id` int NOT NULL,
  `user_id` int DEFAULT NULL,
  `total_price` decimal(10,2) NOT NULL,
  `status` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `user_id_idx` (`user_id`),
  CONSTRAINT `user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`)
);

CREATE TABLE storeclothes.orders_items (
  `id` int NOT NULL,
  `order_id` int DEFAULT NULL,
  `product_id` int DEFAULT NULL,
  `quantity` int NOT NULL,
  `price` decimal(10,2) NOT NULL,
  KEY `order_id_idx` (`order_id`),
  KEY `product_id_idx` (`product_id`),
  CONSTRAINT `order_id` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`),
  CONSTRAINT `product_id` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`)
);

CREATE TABLE storeclothes.cart (
  `id` int NOT NULL,
  `user_id` int DEFAULT NULL,
  `product_id` int DEFAULT NULL,
  `quantity` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `user_id_idx` (`user_id`),
  KEY `product_id_idx` (`product_id`),
  CONSTRAINT `fk_cart_product` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`),
  CONSTRAINT `fk_cart_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`)
);

CREATE TABLE storeclothes.payments (
  `id` int NOT NULL,
  `order_id` int DEFAULT NULL,
  `user_id` int DEFAULT NULL,
  `paymet_method` enum('credit_card','paypal','cod') NOT NULL,
  `payment_status` enum('pending','completed','failed') DEFAULT 'pending',
  `transaction_id` varchar(225) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `transaction_id_UNIQUE` (`transaction_id`),
  KEY `order_id_idx` (`order_id`),
  KEY `user_id_idx` (`user_id`),
  CONSTRAINT `payments_order_id` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`),
  CONSTRAINT `payments_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`)
);

