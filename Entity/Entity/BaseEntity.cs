using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entity
{

    public interface IEntityCommonBase
    {
        bool IsDeleted { set; get; }
    }

    public class EntityCommonBase : IEntityCommonBase
    {
        [DefaultValue(false)]
        public bool IsDeleted { set; get; }
    }

    public interface IBaseEntity<TKey>
    {
        public TKey Id { get; set; }
    }

    public class BaseEntity<TKey> : EntityCommonBase, IBaseEntity<TKey>
    {   
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        virtual public TKey Id { get; set; }

    }

    public class BaseEntity : BaseEntity<int>
    {
        public override int Id { set; get; } = -1;
    }
}
