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

