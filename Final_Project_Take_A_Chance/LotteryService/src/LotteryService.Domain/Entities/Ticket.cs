using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryService.Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public DateTime BoughtAt { get; set; }
        public required string Code { get; set; }
        public int LotteryId { get; set; }

        //relational
        public required Lottery Lottery { get; set; }
    }
}
