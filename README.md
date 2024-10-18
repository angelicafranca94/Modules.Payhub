# Modules.PayHub

O projeto Modules.PayHub, concebido entre 2023/2024, foi idealizado em um momento em que a célula .NET da   identificou a necessidade de centralizar sistemas que possuem o mesmo contexto com a ideia de reduzir o grande número de aplicações de mesmo contexto que possuímos hoje em nosso repositório. O contexto da aplicação Modules.Payhub é meios de pagamento: uma aplicação que centraliza as lógicas de conciliação de todos os meios de pagamento que a  /Modulo possui.  


## Arquitetura 

A arquitetura do projeto **Modules.PayHub** pode ser definida como Monolito Modular que possui o seguinte conceito: 

O conceito de monolito modular combina aspectos do desenvolvimento monolítico e de microsserviços. Ele permite que uma aplicação monolítica seja gradualmente decomposta em módulos independentes e interconectados, sem a necessidade de uma reescrita completa do código. 

Aplicando-se ao Modules.PayHub, cada módulo, representa um contexto da aplicação como: Pix, Boleto, Pagamento Recorrente, Cartão e qualquer outro contexto de meio de pagamento que a   possa querer implementar futuramente. Ou seja, futuras features que tenha relação com esse contexto, deve ser analisado a possibilidade de implementação nesse projeto.  

Além dos módulos que se aplicam ao contexto do sistema, o Modules.PayHub também pode ser dividido em 3 partes e cada uma dessas partes nós chamamos de serviço: 

**Aplicação**: parte do sistema que é a ponte entre o back e o front, a porta de entrada das informações que serão tratadas, armazenamento de toda a lógica da aplicação.  

**Consumer**: parte do sistema responsável pelo processamento de filas de sistemas de mensagerias (atualmente usamos o RabbitMQ) 

**Jobs**: parte do sistema responsável pelo monitoramento dos jobs dos bancos da   que tem relação com as formas de pagamento. Atualmente, somente os boleto possuem processos relacionados a jobs. 


## Pastas 

O esquema de pastas do projeto possui 2 pastas principais: **src** e **tests** 
 
Em src está contida toda a parte lógica da aplicação  
 
**CrossCutting**  - aqui está centralizado 2 bibliotecas responsáveis por centralizar tudo que é compartilhado entre os serviços e módulos e a centralização de toda a injeção de dependência dos serviços e módulos. 

**Modules** - Responsável pela centralização da lógica dos contextos da aplicação, utilizando os princípios da Clean Architecture. 

**Services** - responsável pela centralização dos serviços utilizados na aplicação. No caso do Modules.Payhub, como dito anteriormente, os serviços são 3: Aplicação, Consumer e Jobs 

**Tests** – responsável pela centralização dos Testes unitários separados por módulos. 

 
## Rodando na máquina

Para rodar e testar o projeto na sua máquina, são necessários alguns itens:

- [ ] .NET Core a partir da versão 8
- [ ] IDE de sua preferencia - Visual Studio, Visual Studio Code ou qualquer outra que build projetos .NET
- [ ] Software para testar requisições - Postman, HTTPie ou outro similar de sua preferência 
- [ ] Python instalado na máquina -  para rodar o script de criptografia do código da FnDebitos para fazer os testes
- [ ] O arquivo .pem para descriptografar o código da FNDebitos
- [ ] Certificados do itaú em uma pasta na sua máquina para enviar no header das requisições
- [ ] Docker e Docker for Desktop - caso queira testar o projeto publicado no docker

