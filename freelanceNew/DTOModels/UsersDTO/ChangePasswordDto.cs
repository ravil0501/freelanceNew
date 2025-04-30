﻿using System.ComponentModel.DataAnnotations;

namespace freelanceNew.DTOModels.UsersDTO
{
    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
    }
}
