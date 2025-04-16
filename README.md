# Quản Lý Nhà Trọ

Hiện nay, nhiều khu nhà trọ với quy mô lớn được xây dựng nhằm phục vụ nhu cầu lưu trú của sinh viên và người lao động xa quê. Tuy nhiên, việc quản lý số lượng lớn phòng trọ và các dịch vụ đi kèm đang dần trở nên phức tạp và tốn thời gian cho chủ nhà hoặc người quản lý.
Xuất phát từ thực tế đó, cả nhóm đã quyết định thực hiện website Quản Lý Nhà Trọ với mục tiêu hỗ trợ người quản lý thực hiện các tác vụ như theo dõi phòng, hợp đồng, dịch vụ, hóa đơn,… một cách nhanh chóng và thuận tiện mà không cần phải đến từng phòng. Hệ thống giúp đơn giản hóa quy trình quản lý, tiết kiệm thời gian và nâng cao hiệu quả vận hành.

## 📸 Ảnh giao diện website Quản Lý Nhà Trọ

Ảnh chụp màn hình hoặc video demo (có thể dùng [shields.io](https://shields.io/) hoặc ảnh GIF).

## 🚀 Tính năng chính

- ✅ Đặt lịch hẹn giúp admin có thể quản lý được các cuộc hẹn của khách hàng đến xem trọ.
- ✅ Quản lý (CRUD) người dùng, (CRUD) xe được chứa trong nhà xe thuộc nhà trọ, các điều khoản và hợp đồng.
- ✅ Thông báo với admin khi có người dùng vừa đặt lịch xem phòng với một phòng nào đó.
- ✅ Admin có thể viết thông báo cho người dùng ở khu trọ thông qua website.
- ✅ Xuất hóa đơn của phòng ra file PDF kèm theo một số thông tin và mã QR thanh toán.
- ✅ Đăng ký, đăng nhập, quên mật khẩu, đặt lại mật khẩu và chia role.

## 🔧 Công nghệ sử dụng

- Ngôn ngữ: C#.
- Framework: ASP.Net Core MVC.
- CSDL: SQL Server.

## 🏁 Cài đặt và chạy local

```bash
# 1. Clone repo
git clone https://github.com/DucKhai84/DoAn_LTWeb

# 2. Cập nhật file appsettings.json và migration
"ConnectionStrings": {
  "DefaultConnection": "Server=your_server;Database=your_db;User Id=your_user;Password=your_password;TrustServerCertificate=True;"
}

# 3. Cài đặt các Nuget cần thiết 
- Microsoft.AspNetCore.Identity.EntityFrameWorkCore
- Microsoft.AspNetCore.Identity.UI
- Microsoft.AspNetCore.SignalR
- Microsoft.AspNetCore.SignalR.Core
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Sqlite
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.VisualStudio.Web.CodeGeneration.Design
- ReportViewCore.NETCore
