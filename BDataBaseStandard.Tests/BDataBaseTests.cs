using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BDataBaseStandard.Tests
{
    public class BDataBaseTests
    {
        private const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OpenResume;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        [Fact]
        public void Update()
        {
            IDataBase database = DataBase.fnOpenConnection(connectionString, DataBase.enmDataBaseType.MsSql);

            string command = $@"update users SET
                                login = @LOGIN
                                    output inserted.id 
                                where
                                    id = @ID
                                ";

            Dictionary<string, object> par = new Dictionary<string, object>();
            par.Add("ID", 1);
            par.Add("LOGIN", "Lovecraft");

            int result = database.fnExecute<int>(command, par).FirstOrDefault();

            Assert.Equal(1, result);

        }
    }
}
