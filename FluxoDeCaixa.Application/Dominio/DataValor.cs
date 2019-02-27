using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluxoDeCaixa.Application.Dominio
{
    public class DataValor
    {
        [BsonElement("Data")]
        public string Data { get; set; }
        [BsonElement("Valor")]
        public decimal Valor { get; set; }

        public DataValor(string data, decimal valor)
        {
            Data = data;
            Valor = valor;
        }

        protected DataValor()
        {
                
        }
    }
}
