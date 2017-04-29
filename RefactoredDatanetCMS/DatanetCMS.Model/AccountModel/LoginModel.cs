namespace DatanetCMS.Model.AccountModel
{
    public class LoginModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Logo { get; set; }
        public CustomerModel.CustomerModel Customer { get; set; }
    }
}
