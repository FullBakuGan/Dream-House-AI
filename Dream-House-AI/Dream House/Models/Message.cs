using Dream_House.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Dream_House.Models
{
    [Table("message")]
    public class Message
    {
        [Column("username")]
        public string UserName { get; set; }
        [Column("text")]

        public string Text { get; set; }
        [Column("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
