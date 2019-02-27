using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace FluxoDeCaixa.Application.Dominio
{
    public class ConsolidadoFluxo : Entidade
    {
        [BsonElement("Data")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Data { get; set; }
        [BsonElement("Entradas")]
        public List<DataValor> Entradas { get; set; }
        [BsonElement("Saidas")]
        public List<DataValor> Saidas { get; set; }
        [BsonElement("Encargos")]
        public List<DataValor> Encargos { get; set; }
        [BsonElement("Total")]
        public decimal Total { get; set; }
        [BsonElement("PosicaoDia")]
        public string PosicaoDia { get; set; }

        public ConsolidadoFluxo()
        {

        }
    }
}
