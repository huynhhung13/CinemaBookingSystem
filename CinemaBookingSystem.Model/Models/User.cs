﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "char")]
        public string Username { get; set; }
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "char")]
        public string Password { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(100)]
        public string EmailAddress { get; set; }
        [Required]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(500)]
        public string Address { get; set; }
        [Required]
        public int RoleID { get; set; }
        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
