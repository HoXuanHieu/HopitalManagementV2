﻿using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Client.DTOs.HospitalDTOs
{
    public class HospitalDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
