//using System.Threading.Tasks;

//using MongoDB.Driver;


//namespace Pacco.Services.Availability.Infrastructure.Mongo.Queries.Handlers
//{
//    internal sealed class GetResourceHandler : IQueryHandler<GetResource, ResourceDto>
//    {
//        private readonly IMongoDatabase _database;

//        public GetResourceHandler(IMongoDatabase database)
//        {
//            _database = database;
//        }

//        public async Task<ResourceDto> HandleAsync(GetResource query)
//        {
//            var document = await _database.GetCollection<ChargeGroupDocument>("resources")
//                .Find(r => r.Id == query.ResourceId)
//                .SingleOrDefaultAsync();
            
//            return document?.AsDto();
//        }
//    }
//}