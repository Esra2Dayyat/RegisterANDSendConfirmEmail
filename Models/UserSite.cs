using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Data.Entity;
using System.Net;
using Microsoft.AspNet.Identity.EntityFramework;







namespace LoginRegisterDemo.Models
{
    public class UserSite
    {   [Key]
        public int ID { set; get; }
         public string UserName { get; set; }
        public string Email { set; get;  }
         public string Password { get; set; } 



    }
 /*public class ApplicationUser : IdentityUser
        {
        }*/
    public class UserSiteContext: DbContext
    {
       

        public UserSiteContext ( ) :base("name=UserSiteContext")
        {


        }

        public DbSet<UserSite> newUser { get; set; }
    }

}