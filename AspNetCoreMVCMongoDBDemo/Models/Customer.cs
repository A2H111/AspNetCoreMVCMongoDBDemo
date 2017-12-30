using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace AspNetCoreMVCMongoDBDemo.Models
{
	public class Customer
	{
		[BsonId]
		public ObjectId Id { get; set; }
		[BsonElement]
		public int CustomerId { get; set; }
		[BsonElement]
		public string CustomerName { get; set; }
		[BsonElement]
		public string Address { get; set; }
	}
}
