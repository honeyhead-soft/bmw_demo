using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SupabaseMauiDemo
{
    [Table("testmsg")]
    public class testmsg : BaseModel
    {
        [PrimaryKey("id", false)]
        public int id { get; set; }

        [Column("created_at")]
        public DateTime created_at { get; set; }

        [Column("content")]
        public string content { get; set; }
    }
}
