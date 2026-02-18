-- Adatbázis létrehozása
CREATE DATABASE IF NOT EXISTS mini_webshop
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_hungarian_ci;

USE mini_webshop;

-- Régi tábla törlése
DROP TABLE IF EXISTS products;

-- Tábla létrehozása
CREATE TABLE products (
  id INT AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(100) NOT NULL,
  price DECIMAL(10,2) NOT NULL CHECK (price > 0),
  category VARCHAR(50) NOT NULL,
  inStock TINYINT(1) NOT NULL
);

-- Mintaadatok
INSERT INTO products (name, price, category, inStock) VALUES
('NVIDIA GeForce RTX 4060 8GB', 139990.00, 'Videókártya', 1),
('AMD Radeon RX 7600 8GB', 129990.00, 'Videókártya', 1),
('NVIDIA GeForce RTX 3060 12GB', 119990.00, 'Videókártya', 0),
('AOC 24G2U 24\" 144Hz', 59990.00, 'Monitor', 1),
('LG 27MP400 27\" IPS', 52990.00, 'Monitor', 1),
('Samsung Odyssey G5 32\"', 119990.00, 'Monitor', 0),
('Samsung 970 EVO Plus 1TB NVMe', 29990.00, 'SSD', 1),
('WD Blue SN570 500GB NVMe', 18990.00, 'SSD', 1),
('Kingston A400 480GB SATA', 16990.00, 'SSD', 0),
('MSI B550M PRO-VDH WiFi', 38990.00, 'Alaplap', 1),
('ASUS PRIME B760M-A', 44990.00, 'Alaplap', 1),
('Gigabyte B450M DS3H', 29990.00, 'Alaplap', 0),
('Kingston Fury Beast 16GB (2x8GB) DDR4 3200MHz', 15990.00, 'RAM', 1),
('Corsair Vengeance 32GB (2x16GB) DDR4 3600MHz', 32990.00, 'RAM', 0),
('G.Skill Ripjaws 16GB DDR4 3600MHz', 17990.00, 'RAM', 1),
('AMD Ryzen 5 5600', 47990.00, 'Processzor', 1),
('Intel Core i5-12400F', 58990.00, 'Processzor', 1),
('AMD Ryzen 7 5800X', 89990.00, 'Processzor', 0),
('Corsair CX650 650W', 24990.00, 'Tápegység', 1),
('Seasonic Focus GX-750 750W', 47990.00, 'Tápegység', 0),
('Logitech G102 Gaming Egér', 6990.00, 'Periféria', 1),
('HyperX Alloy Core RGB Billentyűzet', 12990.00, 'Periféria', 1),
('AOC GK500 Mechanikus Billentyűzet', 19990.00, 'Periféria', 0);
