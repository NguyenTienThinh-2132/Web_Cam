# Tài liệu Hệ thống Thiết kế: Heritage Lens (Optic Heritage)

Hệ thống thiết kế này được xây dựng để tôn vinh vẻ đẹp vượt thời gian của nhiếp ảnh analog, kết hợp sự sang trọng của phong cách cổ điển với trải nghiệm hiện đại.

## 1. Bảng màu (Color Palette)

Hệ màu sử dụng các tông màu đất, gỗ và kim loại để tạo cảm giác hoài cổ nhưng vẫn cao cấp.

| Vai trò | Mã màu | Tên biến (CSS) | Mô tả |
| :--- | :--- | :--- | :--- |
| **Surface** | `#FCF9F8` | `--color-surface` | Nền chính, màu kem nhạt tạo cảm giác giấy mỹ thuật cao cấp. |
| **Primary** | `#3D2B1F` | `--color-primary` | Nâu gỗ trầm, dùng cho tiêu đề chính, nút bấm (CTA) và các thành phần quan trọng. |
| **Secondary** | `#D4A373` | `--color-secondary` | Vàng đồng (Gold), dùng cho icon, nhãn (labels) và các chi tiết trang trí. |
| **Text (On Surface)** | `#2D241E` | `--color-on-surface` | Nâu đen đậm cho văn bản nội dung, dễ đọc và không gắt như đen thuần. |
| **Accent** | `#A67C52` | `--color-accent` | Tông nâu trung tính cho các đường kẻ hoặc trạng thái hover nhẹ. |

## 2. Phông chữ (Typography)

Sự kết hợp giữa font có chân (Serif) cho cảm xúc và không chân (Sans-serif) cho sự rõ ràng.

### Tiêu đề (Headings)
- **Font-family:** `Playfair Display`, serif.
- **Đặc điểm:** Nét thanh đậm rõ rệt, mang tính nghệ thuật cao.
- **Sử dụng:** Hero titles, Section headers, Product names.

### Nội dung (Body)
- **Font-family:** `Montserrat` hoặc `Inter`, sans-serif.
- **Đặc điểm:** Hiện đại, dễ đọc trên mọi kích thước màn hình.
- **Sử dụng:** Mô tả sản phẩm, bài viết giới thiệu, nhãn biểu mẫu.

## 3. Hệ thống Icon (Visual Assets)

Các icon được thiết kế theo dạng đường nét (Line art) với màu Vàng đồng (`#D4A373`) để duy trì sự thanh lịch.

- **Dịch vụ:** Shield (Bảo hành), Wrench (Vệ sinh), Percentage (Trả góp), Rocket (Giao hàng).
- **Hành động:** Shopping Cart (Giỏ hàng - `#3D2B1F`), Search (Tìm kiếm).
- **Điều hướng:** Arrow right (Khám phá), User (Tài khoản).

## 4. Thành phần chính (Core Components)

### Nút bấm (Buttons)
- **Primary:** Background `#3D2B1F`, Text `#FCF9F8`, Font `Playfair Display`, Padding `12px 24px`.
- **Secondary:** Border `1px solid #3D2B1F`, Background transparent, Text `#3D2B1F`.

### Thẻ sản phẩm (Product Cards)
- **Nền:** `#FFFFFF` hoặc `#F6F3F2`.
- **Đổ bóng:** Soft elevation (nhẹ), không dùng shadow quá gắt.
- **Hình ảnh:** Tông màu ấm, focus vào chi tiết cơ khí của máy ảnh.

### Form (Input Fields)
- **Border-bottom:** `1px solid #DCD9D9`.
- **Label:** Font `Montserrat`, uppercase, màu `#A67C52`.

---
*Tài liệu này là cơ sở để phát triển UI/UX cho thương hiệu Optic Heritage.*