# WebAPIQuanLyNhungMonQua

## 📌 Giới thiệu
WebAPIQuanLyNhungMonQua là một Web API được phát triển bằng **ASP.NET Core 6** nhằm quản lý những món quà trong 1 hệ thống game, cho phép admin thực hiện các chức năng phát quà cho user với 3 lựa chọn, cho phép user mua quà bằng xu, xếp hạng người dùng theo tháng dựa vào điểm tích lũy khi mua quà.

## 🚀 Công nghệ sử dụng
- **.NET 6 (ASP.NET Core Web API)**
- **Entity Framework Core** (Code-First, Fluent API)
- **Fluent Validation**
- **Microsoft SQL Server** (Lưu trữ dữ liệu)
- **AutoMapper** (Chuyển đổi dữ liệu giữa DTO và Model)
- **JWT Authentication** (Xác thực API)
- **Background job** (Đặt lịch phát quà tự động )
- **Swagger UI** (Tài liệu API)
- **Repository Pattern** (Quản lý dữ liệu)
- **Custom Authorization**

## 📚 Cấu trúc thư mục
```
WebAPIQuanLyNhungMonQua/
│-- Authorization/       # Xác thực và phân quyền
│-- Controllers/         # Xử lý request và trả về response cho client
│-- Models/              # Data Transfer Objects
│-- Data/                # (Domain models) DbContext, Models và cấu hình EF Core
│-- Mappings/            # Cấu hình AutoMapper
│-- Migrations/          # Lưu trữ các migration của database
│-- Properties/          # Cấu hình dự án
│-- Repository/          # Repository Pattern
│-- Services/            # Chứa logic nghiệp vụ
│-- Upload/              # Thư mục lưu trữ file upload
│-- Validator/           # Kiểm tra dữ liệu đầu vào
│-- Program.cs           # Cấu hình ứng dụng
│-- appsettings.json     # Cấu hình database và JWT
```

## 🔑 Chức năng chính
✅ **Quản lý những món quà**: Tạo mới, cập nhật và xóa những món quà chính, quà khuyến mãi; Thiết lập quà khuyến mãi kèm theo quà chính.      
✅ **Tính năng phát quà**: Phát quà chính khi còn quà khuyến mãi, phát quà chính khi hết quà khuyến mãi, tự động phát quà theo ngày chọn trước.    
✅ **Tính năng mua quà và xem lịch sử mua quà**: Cho phép người dùng mua quà bằng xu, xem lịch sử quà đã mua trước đó      
✅ **Quản lý người dùng**: Xác thực, phân quyền, quản lý thông tin người dùng        
✅ **Bảng xếp hạng mua quà hàng tháng**: Cho phép xem bảng xếp hạng hàng tháng theo điểm tích lũy khi mua quà           
✅ **Xác thực & Phân quyền**     

## 📚 API Documentation
Sử dụng **Postman** để kiểm thử API
