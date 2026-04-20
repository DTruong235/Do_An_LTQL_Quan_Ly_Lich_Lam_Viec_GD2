# 🚀 Hệ Thống Quản Lý Lịch Làm Việc – Giai đoạn 2

<p align="center">
  <img src="https://img.shields.io/badge/C%23-.NET-blue.svg" alt="C#">
  <img src="https://img.shields.io/badge/Framework-Windows%20Forms-green.svg" alt="WinForms">
  <img src="https://img.shields.io/badge/ORM-Entity%20Framework%20Core-orange.svg" alt="EF Core">
  <img src="https://img.shields.io/badge/Database-SQL%20Server-red.svg" alt="SQL Server">
</p>

---

## 📖 Giới thiệu

Đây là giai đoạn 2 của đồ án **Hệ thống Quản lý Lịch Làm Việc**, tiếp nối từ nền tảng dữ liệu đã xây dựng ở giai đoạn 1.

Ở giai đoạn này, hệ thống tập trung phát triển **giao diện người dùng (UI)** và **các chức năng nghiệp vụ cơ bản**, giúp người dùng có thể thao tác trực tiếp với dữ liệu thông qua phần mềm.

---

## 🎯 Mục tiêu

* Xây dựng giao diện quản lý bằng Windows Forms
* Kết nối UI với cơ sở dữ liệu thông qua Entity Framework Core
* Triển khai các chức năng CRUD (Create, Read, Update, Delete)
* Đảm bảo các ràng buộc nghiệp vụ cơ bản

---

## ⚙️ Chức năng chính

### 👨‍💼 Quản lý Nhân viên

* Hiển thị danh sách nhân viên
* Thêm / sửa / xóa nhân viên
* Binding dữ liệu với `BindingSource`
* Sử dụng `ComboBox` cho:

  * Phòng ban
  * Chức vụ
* Kiểm tra dữ liệu đầu vào
* Tự động bật/tắt trạng thái nút chức năng

---

### 📅 Quản lý Lịch làm việc

* Hiển thị danh sách lịch làm việc
* Sắp xếp lịch mới nhất lên đầu
* Thêm / sửa / xóa lịch làm việc
* Hiển thị tên thay vì ID (loại công việc, địa điểm)
* Kiểm tra:

  * Thời gian hợp lệ (bắt đầu < kết thúc)
  * Trùng lịch theo địa điểm

---

## 🗄️ Cơ sở dữ liệu

Trong giai đoạn này, hệ thống đã:

* Cập nhật migration mới
* Bổ sung cột **Địa chỉ** cho bảng Nhân viên
* Đồng bộ lại mô hình dữ liệu với EF Core

---

## ✅ Kết quả đạt được

* Hoàn thiện giao diện cho các chức năng cốt lõi
* Thực hiện đầy đủ thao tác CRUD
* Kết nối thành công UI với database
* Áp dụng các ràng buộc nghiệp vụ (validate dữ liệu, chống trùng lịch)

---

* Sinh viên: *[Tên của bạn]*
* Môn học: Lập trình Quản lý
* Năm học: 2025

---

⭐ Nếu thấy project hữu ích, hãy ⭐ repo để ủng hộ!
