using System.ComponentModel.DataAnnotations.Schema;

namespace BookPro.Domain.Entitys.Appointment;

public class Room
{
    public int Id { get; set; }
    public string RoomType { get; set; }
    public DateTime CheckOut { get; set; }
    public bool Available { get; set; }

    public int MotelId { get; set; }
    [ForeignKey("MotelId")]
    public virtual Motel Motel { get; set; }
}
