using FluxoDeCaixa.Application.Dominio;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Agent
{
    public interface IConsumer
    {
        Task ConsumirFila(string fila);
    }
}
