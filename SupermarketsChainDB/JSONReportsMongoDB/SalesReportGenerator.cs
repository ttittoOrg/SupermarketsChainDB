namespace JSONReportsMongoDB
{
    using MongoDB.Driver;
    
    using SupermarketsChainDB.Data;

    public class SalesReportGenerator
    {
        private SupermarketSystemData data = new SupermarketSystemData();
        private string[] mongoDbConnectionString = Connection.GetMongoConnectionString();

       // private MongoDatabase monogoDb = GetDatabase(mongoDbConnectionString);

        // private method for connecting to the mongo server and getting the database
        private static MongoDatabase GetDatabase(string[] mongoDbConnectionString)
        {
            var mongoClient = new MongoClient(mongoDbConnectionString[0]);
            var server = mongoClient.GetServer();
            return server.GetDatabase(mongoDbConnectionString[1]);
        }
    }
}
