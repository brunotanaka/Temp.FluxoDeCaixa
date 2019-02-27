using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FluxoDeCaixa.Application.Dominio
{
    public abstract class Entidade
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        protected Entidade()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}
