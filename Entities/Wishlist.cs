namespace GoDecola.API.Entities
{
    public class Wishlist
    {
        public int Id { get; set; } // id da wishlist
        public string? UserId { get; set; } // id do usuário 
        public User User { get; set; } // propriedade de navegação para o usuário
        public int TravelPackageId { get; set; } // id do pacote de viagem associado
        public TravelPackage TravelPackage { get; set; } // propriedade de navegação para o usuário
        public DateTime AddedDate { get; set; } = DateTime.UtcNow; // data que o pacote foi adicionado à wishlist
        public List<Wishlist> Items { get; set; } = new List<Wishlist>();  // lista de itens na wishlist
    }
}
