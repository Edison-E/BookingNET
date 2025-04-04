using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Infrastructure.Repositories.Appointment;

public class RoomRepository : Repository<Room>, IRoomRepository
{
    public RoomRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Room>> GetAllRoomByMotel(int motelId) =>
        await _context.Rooms.Where(x => x.MotelId == motelId).ToListAsync();

    public async Task<Room> GetRoomByMotel(int motelId) =>
        await _context.Rooms.Where(x => x.MotelId == motelId).FirstAsync();

    public async Task<bool> UpdateRoom(int roomId, Room room) =>
        await Update(room, roomId);
         
}
