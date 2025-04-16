using DoAn_LTWeb.Data;
using DoAn_LTWeb.Repositories;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DoAn_LTWeb.Hubs
{
    public class BookingHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhongTroRepository _phongTroRepository;

        public BookingHub(ApplicationDbContext context, IPhongTroRepository phongTroRepository)
        {
            _context = context;
            _phongTroRepository = phongTroRepository;
        }

        public async Task RequestBooking(int roomId, string userName)
        {
            Console.WriteLine($"📩 Server nhận yêu cầu đặt phòng: Phòng {roomId}, Người dùng {userName}");
            await Clients.All.SendAsync("ReceiveBookingRequest", roomId, userName);
        }

        public async Task AcceptBooking(int roomId)
        {
            var room = await _phongTroRepository.GetByIdAsync(roomId);
            if (room != null)
            {
                room.TrangThai = "DaThue";
                await _context.SaveChangesAsync();
            }

            Console.WriteLine($"✅ Phòng {roomId} đã được chấp nhận!");
            await Clients.All.SendAsync("BookingStatusChanged", roomId, "Thành công");
        }

        public async Task RejectBooking(int roomId)
        {
            var room = await _phongTroRepository.GetByIdAsync(roomId);
            if (room != null)
            {
                room.TrangThai = "Trong"; // Hoặc trạng thái phù hợp
                await _context.SaveChangesAsync();
            }

            Console.WriteLine($"❌ Phòng {roomId} đã bị từ chối!");
            await Clients.All.SendAsync("BookingStatusChanged", roomId, "Từ chối");
        }
    }
}
