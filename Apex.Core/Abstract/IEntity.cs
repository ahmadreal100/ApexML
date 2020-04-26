using System;
using Apex.Shared.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Apex.Core.Abstract
{
    public interface IEntity
    {
        long Id { get; set; }
        DateTime AddedDate { get; set; }
        DateTime ModifiedDate { get; set; }
    }

    public abstract class Entity : IEntity
    {
        protected Entity()
        {
            AddedDate = DateHelper.Now;
            ModifiedDate = AddedDate;
        }

        public long Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class Identity : IdentityUser<long, UserLogin, UserRole, UserClaim>, IEntity
    {
        public Identity()
        {
            AddedDate = DateHelper.Now;
            ModifiedDate = AddedDate;
        }

        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }


    public class UserRole : IdentityUserRole<long>
    {
    }

    public class UserLogin : IdentityUserLogin<long>
    {
    }

    public class Role : IdentityRole<long, UserRole>
    {

    }

    public class UserClaim : IdentityUserClaim<long>
    {
    }
}
