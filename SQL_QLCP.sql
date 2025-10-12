
-- Tạo CSDL (tùy chọn)
CREATE DATABASE QuanLyCaPhe;
GO
USE QuanLyCaPhe;
GO

-- 1. Bảng NhanVien
CREATE TABLE NhanVien (
    MaNV INT PRIMARY KEY,
    TenNV NVARCHAR(100),
    Email NVARCHAR(100),
    MatKhau NVARCHAR(100),
    GioiTinh BIT,
    NamSinh DATE
);


-- 2. Bảng NguyenLieu
CREATE TABLE NguyenLieu (
    MaNguyenLieu NVARCHAR(10) PRIMARY KEY,
    TenNguyenLieu NVARCHAR(100),
    XuatXu NVARCHAR(100),
    SoLuongTon INT
);

-- 3. Bảng ThucDon
CREATE TABLE ThucDon (
    MaMon INT PRIMARY KEY,
    TenMon NVARCHAR(100),
    DonGia DECIMAL(18, 2),
    LoaiMon NVARCHAR(50)
);

-- 4. Bảng HoaDon
CREATE TABLE HoaDon (
    MaHoaDon INT PRIMARY KEY IDENTITY(1,1),
    NgayLap DATE,
    TongTien DECIMAL(18, 2)
);

-- 5. Bảng ChiTietHoaDon
CREATE TABLE ChiTietHoaDon (
    MaHoaDon INT,
    MaMon INT,
    SoLuong INT,
    DonGia DECIMAL(18, 2),
    PRIMARY KEY (MaHoaDon, MaMon),
    FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon),
    FOREIGN KEY (MaMon) REFERENCES ThucDon(MaMon)
);

-- 6. Bảng LichLamViec
CREATE TABLE LichLamViec (
    MaLich INT IDENTITY(1,1) PRIMARY KEY,
    MaNV INT,
    Ca NVARCHAR(50),
    NgayLam DATE,
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

CREATE TABLE NhapKho (
    MaNhap INT PRIMARY KEY IDENTITY(1,1),
    MaNguyenLieu NVARCHAR(10),
    GiaNhap INT,
    SoLuong INT,
    ThanhTien AS (GiaNhap * SoLuong) PERSISTED,
    NgayNhap DATE DEFAULT GETDATE(),
    FOREIGN KEY (MaNguyenLieu) REFERENCES NguyenLieu(MaNguyenLieu)
);


-- Thêm dữ liệu mẫu vào bảng NhanVien
INSERT INTO NhanVien (MaNV, TenNV, Email, MatKhau, GioiTinh, NamSinh)
VALUES 
(111, N'Nguyễn Văn A', 'a@gmail.com', 'matkhau1', 1, '1995-01-01'),
(222, N'Trần Thị B', 'b@gmail.com', 'matkhau2', 0, '1998-05-10');

-- Thêm dữ liệu mẫu vào bảng ThucDon
INSERT INTO ThucDon (MaMon, TenMon, DonGia, LoaiMon)
VALUES
(1, N'Cà phê sữa', 25000, N'Nóng'),
(2, N'Cà phê đen', 20000, N'Nóng'),
(3, N'Trà đào cam sả', 30000, N'Lạnh'),
(4, N'Trà vải', 28000, N'Lạnh'),
(5, N'Sinh tố bơ', 35000, N'Lạnh'),
(6, N'Bạc xỉu', 27000, N'Nóng'),
(7, N'Nước ép cà rốt', 29000, N'Lạnh'),
(8, N'Sữa tươi trân châu đường đen', 32000, N'Lạnh'),
(9, N'Hồng trà sữa', 31000, N'Lạnh'),
(10, N'Cacao đá', 26000, N'Lạnh');

-- Thêm dữ liệu mẫu vào bảng HoaDon
INSERT INTO HoaDon (MaHoaDon, NgayLap, TongTien)
VALUES
(1, '2025-06-23', 45000),
(2, '2025-06-24', 25000);

-- Thêm dữ liệu mẫu vào bảng ChiTietHoaDon
INSERT INTO ChiTietHoaDon (MaHoaDon, MaMon, SoLuong, DonGia)
VALUES
(1, 1, 1, 25000),
(1, 2, 1, 20000),
(2, 2, 1, 25000);

INSERT INTO NguyenLieu (MaNguyenLieu, TenNguyenLieu, XuatXu, SoLuongTon)
VALUES 
('CF01', N'Cà phê Robusta', N'Việt Nam', 100),
('CF02', N'Cà phê Arabica', N'Brazil', 80),
('CF03', N'Cà phê Culi', N'Lâm Đồng', 60),
('CF04', N'Cà phê Moka', N'Buôn Ma Thuột', 50),
('CF05', N'Cà phê Cherry', N'Cầu Đất', 70),
('CF06', N'Cà phê Espresso', N'Ý', 40),
('CF07', N'Cà phê rang xay', N'Gia Lai', 90),
('CF08', N'Cà phê bột', N'Huế', 120),
('CF09', N'Cà phê hạt', N'Tây Nguyên', 110),
('CF10', N'Cà phê hòa tan', N'Hà Nội', 150);

SELECT * FROM NhanVien