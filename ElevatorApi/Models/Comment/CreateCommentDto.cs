﻿using System.ComponentModel.DataAnnotations;

namespace ElevatorApi.Models.Comment
{
    public class CreateCommentDto
    {
        [Required]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "The {0} value must be between {1} and {2} chars long")]
        public string Message { get; set; }
    }
}
