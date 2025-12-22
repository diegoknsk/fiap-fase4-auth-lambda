# Subtask 01: Criar Command RegisterCustomerCommand

## Descrição
Criar classe RegisterCustomerCommand na camada Application que representa o comando para registrar um customer com CPF.

## Passos de implementação
- Criar diretório `src/FastFood.Auth.Application/Commands/Customer/` se não existir
- Criar arquivo `src/FastFood.Auth.Application/Commands/Customer/RegisterCustomerCommand.cs`
- Criar classe `RegisterCustomerCommand` com propriedade `string Cpf { get; set; }`
- Adicionar validação básica (pode usar Data Annotations ou FluentValidation)

## Como testar
- Executar `dotnet build` no projeto Application (deve compilar sem erros)
- Verificar que a classe está acessível

## Critérios de aceite
- Arquivo RegisterCustomerCommand.cs criado
- Classe tem propriedade Cpf
- Código compila sem erros

