﻿using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.Model.Abstract
{
    public abstract class Auditable : IAuditable
    {
        public DateTime CreatedDate { get; set; }
        [MaxLength(256)]
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [MaxLength(256)]
        public string UpdatedBy { get; set; }
    }
}
