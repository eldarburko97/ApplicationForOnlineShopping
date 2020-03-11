using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Models
{
    public class Role:IdentityRole<int>
    {
        public Role():base()
        {

        }
        public Role(string Name):base(Name)
        {

        }
       
    }
}
