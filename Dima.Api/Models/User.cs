using Microsoft.AspNetCore.Identity;

namespace Dima.Api.Models
{
    public class User : IdentityUser<long>
    {
        //RBAC - Claims -> afirmações
        public List<IdentityRole<long>>? Roles { get; set; }    
    }
}
