using System;

namespace DNUResourceBooker.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ResourceId { get; set; }
        public Resource Resource { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Purpose { get; set; }
        public string BookingStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}