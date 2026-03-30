namespace asp_net_3.Models {
    public class Role {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<User> Users { get; set; }

        public Role() {
            Name = "";
            Users = new List<User>();
        }
    }
}
