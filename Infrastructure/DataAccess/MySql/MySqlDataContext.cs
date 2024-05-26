using MySql.Data.MySqlClient;

namespace Infrastructure.DataAccess.MySql
{
    public class MySqlDataContext
    {
        private readonly CustomerRepository _customerRepository;

        public MySqlDataContext(string connectionString)
        {
            var connection = new MySqlConnection(connectionString);   
            _customerRepository = new CustomerRepository(connection);
        }

        public CustomerRepository CustomerRepository => _customerRepository;


    }
}
