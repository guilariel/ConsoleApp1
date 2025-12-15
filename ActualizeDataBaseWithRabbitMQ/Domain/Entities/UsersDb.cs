using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActualizeDataBaseWithRabbitMQ.Domain.Entities
{
    public class UsersDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("name")]
        public string name { get; set; } = string.Empty;

        [Column("funds")]
        public double funds { get; set; }

        [Column("password_hash")]
        public string password_hash { get; set; } = string.Empty;

    }
}
