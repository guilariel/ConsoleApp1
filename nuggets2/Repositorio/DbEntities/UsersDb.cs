using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQAndGenericRepository.Repositorio.DbEntities
{
    public class UsersDb : IEFEntity<int>
    {
        [NotMapped]
        public int key => id;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; protected set; }
        public string name { get; set; }
        public string password_hash { get; set; }

    }
}
