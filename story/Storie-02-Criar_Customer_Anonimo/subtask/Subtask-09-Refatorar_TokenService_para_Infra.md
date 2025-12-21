# Subtask 09: Refatorar TokenService para projeto FastFood.Auth.Infra

## Descrição
Refatorar a localização do TokenService, movendo-o do projeto `FastFood.Auth.Infra.Persistence` para um novo projeto `FastFood.Auth.Infra`, seguindo o princípio de separação de responsabilidades da Clean Architecture. O TokenService não tem relação com persistência de dados, sendo um serviço de infraestrutura geral.

## Contexto
O TokenService foi inicialmente implementado em `Infra.Persistence` por conveniência, mas semanticamente não faz sentido estar lá, pois:
- Não acessa banco de dados
- Não usa Entity Framework Core
- É um serviço de infraestrutura geral (geração de tokens JWT)
- Futuros serviços como ICognitoService também precisarão de um local adequado

## Passos de implementação
- Criar novo projeto class library `FastFood.Auth.Infra` na pasta `src/`
- Configurar o projeto:
  - TargetFramework: net8.0
  - Adicionar pacotes: `System.IdentityModel.Tokens.Jwt` e `Microsoft.Extensions.Configuration.Abstractions`
  - Adicionar referência ao projeto `FastFood.Auth.Application`
- Criar diretório `Services/` no novo projeto
- Mover arquivo `TokenService.cs` de `Infra.Persistence/Services/` para `Infra/Services/`
- Atualizar namespace de `FastFood.Auth.Infra.Persistence.Services` para `FastFood.Auth.Infra.Services`
- Remover pacotes JWT do projeto `Infra.Persistence` (não são mais necessários)
- Adicionar referência ao projeto `FastFood.Auth.Infra` no `FastFood.Auth.Lambda`
- Atualizar `Program.cs` para usar o novo namespace `FastFood.Auth.Infra.Services`
- Remover arquivo antigo `TokenService.cs` de `Infra.Persistence/Services/`
- Remover diretório `Services/` de `Infra.Persistence` se estiver vazio

## Como testar
- Executar `dotnet build` na solução (deve compilar sem erros)
- Verificar que o TokenService está no projeto correto
- Validar que todas as referências foram atualizadas
- Executar a aplicação e testar o endpoint `/api/customer/anonymous` (deve funcionar normalmente)

## Critérios de aceite
- Projeto `FastFood.Auth.Infra` criado como class library
- TokenService movido para `FastFood.Auth.Infra/Services/`
- Namespace atualizado para `FastFood.Auth.Infra.Services`
- Referências de projeto atualizadas corretamente
- Program.cs atualizado com novo namespace
- Pacotes JWT removidos de `Infra.Persistence`
- Arquivo antigo removido de `Infra.Persistence`
- Código compila sem erros
- Aplicação funciona normalmente após refatoração

## Benefícios
- ✅ Separação clara de responsabilidades (persistência vs serviços gerais)
- ✅ Estrutura mais adequada para futuros serviços (ICognitoService, IMessageBus, etc.)
- ✅ Melhor organização seguindo princípios da Clean Architecture
- ✅ Facilita manutenção e evolução do código











