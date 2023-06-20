using System;
using System.ComponentModel.DataAnnotations;

namespace Hermes.Models
{
    public class HermesMessage
    {
        [Key]
        public int Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid? GroupId { get; set; }
        public string Message { get; set; }
        public bool Delivered { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
