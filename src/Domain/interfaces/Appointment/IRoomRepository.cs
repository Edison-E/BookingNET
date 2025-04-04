using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Domain.interfaces.Appointment;

public interface IRoomRepository
{
    Task<List<Room>> GetAllRoomByMotel(int motelId);
    Task<Room> GetRoomByMotel(int motelId);
    Task<bool> UpdateRoom(int roomId, Room room);
}
