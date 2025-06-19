using System;

namespace DNUResourceBooker.Models.ViewModels.Booking
{
    public class BookingCreateViewModel
    {
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Purpose { get; set; }
        public string Note { get; set; }
    }
}