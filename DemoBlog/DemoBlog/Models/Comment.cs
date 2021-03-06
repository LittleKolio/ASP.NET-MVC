﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DemoBlog.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Date = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [DataType (DataType.MultilineText)]
        public string Body { get; set; }

        [Required]
        [DataType (DataType.DateTime)]
        public DateTime Date { get; set; }

        public Post PostComm { get; set; }

        public ApplicationUser Author { get; set; }
    }
}