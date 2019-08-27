# Temp.FluxoCaixa
Utilizando .NET Core, RabbitMQ e XUnity

# Como Utilizar
<b>Debug:</b> é necessário subir os containers localizados na pasta docker no arquivo "rabbit-mongo.yml" e configurar a variavel de ambiente "ASPNETCORE_ENVIRONMENT" dentro dos projetos de FluxoDeCaixa.Agent e FluxoDeCaixa.Api com o valor "local".<br/>
<b>Via Containers:</b> é necessário subir os containers localizados na pasta docker no arquivo "docker-compose.yml", a API está disponível na porta 5101.

<b>Comando para subir os containers completos:</b> docker-compose up -d <br/>
<b>Comando para subir somente rabbitmq e mongo para debug:</b> docker-compose -f rabbit-mongo.yml up -d <br/><br/>
<b>Necessário adicionar a linha abaixo no hosts da máquina<br/>
<b>127.0.0.1	mongodb<br/>

# Informações sobre os projetos
A API possui documentação com o Swashbuckle, para acessar é necessário acessar a porta na qual a API está disponível, adicionando a rota /swagger.
<br/> EX: http://localhost:5101/swagger.<br/><br/>
O acompanhamento das filas do RabbitMQ pode ser feito através do portal próprio da ferramenta, atraves da url: http://localhost:15672, utilizando as credenciais abaixo.<br/>
<b>user:</b> guest <br/>
<b>password:</b> guest <br/><br/>
Está disponível também o admin-mongo para visualizar a estrutura de banco NoSQL atraves da url: http://localhost:8082, adicionando uma conexão com a connectionString abaixo.<br/>
<b>ConnectionString:</b> mongodb://mongodb:27017<br/>

# Codigos de Erro API Lancamento
<b>1 - Tipo de lancamento invalido, permitido somente (1 - Pagamento ou 2 - Recebimento)</b><br/>
<b>2 - Tipo de Conta invalido, permitido somente (1 - ContaCorrente ou 2 - Poupanca)</b><br/>
<b>3 - Data invalida, formato dd-mm-aaaa</b><br/>
<b>4 - Não é possível fazer lancamento retroativo.</b><br/>
<b>5 - Limite negativo atingido.</b><br/>
<b>6 - Valor lancamento inválido .</b><br/>
<b>7 - Valor encargo inválido .</b><br/>

# Exemplos de chamadas para a API de Lançamento

curl -X POST \
  http://localhost:5101/api/lancamento \
  -H 'Content-Type: application/json' \
  -H 'Postman-Token: 9a2a57e0-85f9-4441-81e4-8fb346a04710' \
  -H 'cache-control: no-cache' \
  -d '{
  "tipo_da_lancamento": 2,
  "descricao": "teste",
  "conta_destino": "02",
  "banco_destino": "02",
  "tipo_de_conta": 1,
  "cpf_cnpj_destino": "42105874860",
  "valor_do_lancamento": "R$ 10.000,00",
  "encargos": "R$ 120,00",
  "data_de_lancamento": "12-12-2018"
}'



curl -X GET \
  http://localhost:5101/api/lancamento \
  -H 'Postman-Token: c488d07c-a497-4770-928f-c4daf68a905c' \
  -H 'cache-control: no-cache'
