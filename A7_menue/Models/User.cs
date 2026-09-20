namespace A7_menue.Models
{
    public class User
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatdAt { get; set; }

        //Navication property
        public Tenant Tenant { get; set; } = null!;

    }
}
