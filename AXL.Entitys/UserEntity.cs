using DapperExtensions.Mapper;

namespace AXL.Entitys
{
    public class UserEntity
    {
        public long ID { get; set; }
        public string? UserName { get; set; }
        public string? UserNo { get; set; }
        public string? Address { get; set; }
        public string? UserTel { get; set; }
        public string? UserRoleName { get; set; }
    }
    public class UserEntityMapper : ClassMapper<UserEntity> 
    {
        public UserEntityMapper()
        {
            Table("users");
            //Schema("Axl_Project");
            Map(x => x.ID).Column("id").Key(KeyType.Identity);
            Map(x => x.UserName).Column("username");
            Map(x => x.UserNo).Column("userno");
            Map(x => x.Address).Column("address");
            Map(x => x.UserTel).Column("usertel");
            Map(x => x.UserRoleName).Ignore();
            AutoMap();
        } 
    }
}