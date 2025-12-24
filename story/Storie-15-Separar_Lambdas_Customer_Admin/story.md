# Story 15: Separar Lambdas Customer e Admin em Projetos Independentes

## Descrição
Refatorar a arquitetura atual que usa uma única API Lambda com lógica condicional (`LAMBDA_MODE`) para separar em dois projetos Lambda independentes: `FastFood.Auth.Lambda.Customer` e `FastFood.Auth.Lambda.Admin`. Isso elimina a necessidade de filtros de controllers e convenções complexas, resultando em código mais limpo, manutenível e com deploy mais simples.

## Contexto
Atualmente, o projeto `FastFood.Auth.Lambda` usa uma variável de ambiente `LAMBDA_MODE` para determinar qual controller deve ser exposto (Customer, Admin ou Migrator). Isso resulta em:
- Código complexo com `ControllerFilterConvention` e `ActionFilterConvention`
- Lógica condicional espalhada no `Program.cs` e `LambdaEntryPoint.cs`
- Dificuldade de manutenção e entendimento
- Deploy mais complexo

## Objetivos
1. Criar projeto `FastFood.Auth.Lambda.Customer` dedicado ao fluxo de customer
2. Criar projeto `FastFood.Auth.Lambda.Admin` dedicado ao fluxo de admin
3. Manter `FastFood.Auth.Migrator` como projeto separado (já está correto)
4. Remover toda lógica de `LAMBDA_MODE` e filtros de controllers
5. Simplificar código removendo convenções desnecessárias
6. Atualizar Dockerfiles para cada projeto
7. Atualizar Terraform para usar os novos projetos
8. Atualizar workflow de deploy para gerar 3 imagens ECR e subir nos 3 lambdas

## Benefícios
- ✅ Código mais limpo e fácil de entender
- ✅ Separação clara de responsabilidades
- ✅ Deploy mais simples e direto
- ✅ Manutenção facilitada
- ✅ Melhor testabilidade
- ✅ Redução de complexidade ciclomática

## Estrutura Final Esperada
```
src/
├── FastFood.Auth.Lambda.Customer/     # Nova Lambda para Customer
│   ├── Controllers/
│   │   └── CustomerController.cs
│   ├── Program.cs
│   └── LambdaEntryPoint.cs
├── FastFood.Auth.Lambda.Admin/        # Nova Lambda para Admin
│   ├── Controllers/
│   │   └── AdminController.cs
│   ├── Program.cs
│   └── LambdaEntryPoint.cs
└── FastFood.Auth.Migrator/            # Mantém como está (já separado)
    └── Program.cs
```

## Subtasks
1. [Subtask 01: Criar projeto FastFood.Auth.Lambda.Customer](./subtask/Subtask-01-Criar_projeto_Lambda_Customer.md)
2. [Subtask 02: Criar projeto FastFood.Auth.Lambda.Admin](./subtask/Subtask-02-Criar_projeto_Lambda_Admin.md)
3. [Subtask 03: Mover CustomerController para Lambda.Customer](./subtask/Subtask-03-Mover_CustomerController.md)
4. [Subtask 04: Mover AdminController para Lambda.Admin](./subtask/Subtask-04-Mover_AdminController.md)
5. [Subtask 05: Remover lógica LAMBDA_MODE e convenções](./subtask/Subtask-05-Remover_logica_LAMBDA_MODE.md)
6. [Subtask 06: Criar Dockerfiles para novos projetos](./subtask/Subtask-06-Criar_Dockerfiles.md)
7. [Subtask 07: Atualizar Terraform para novos projetos](./subtask/Subtask-07-Atualizar_Terraform.md)
8. [Subtask 08: Atualizar workflow de deploy](./subtask/Subtask-08-Atualizar_workflow_deploy.md)
9. [Subtask 09: Remover projeto Lambda antigo](./subtask/Subtask-09-Remover_projeto_Lambda_antigo.md)
10. [Subtask 10: Testar deploy completo](./subtask/Subtask-10-Testar_deploy_completo.md)

## Critérios de Aceitação
- [ ] Projeto `FastFood.Auth.Lambda.Customer` criado e funcionando
- [ ] Projeto `FastFood.Auth.Lambda.Admin` criado e funcionando
- [ ] Todos os controllers movidos para projetos corretos
- [ ] Lógica de `LAMBDA_MODE` completamente removida
- [ ] `ControllerFilterConvention` e `ActionFilterConvention` removidos
- [ ] Dockerfiles criados e funcionando para ambos projetos
- [ ] Terraform atualizado e funcionando
- [ ] Workflow de deploy atualizado e gerando 3 imagens ECR
- [ ] Projeto `FastFood.Auth.Lambda` antigo removido
- [ ] Todas as 3 Lambdas (Customer, Admin, Migrator) funcionando corretamente
- [ ] Testes unitários atualizados (se necessário)
- [ ] Documentação atualizada

## Notas Técnicas
- O projeto `FastFood.Auth.Migrator` já está separado e não precisa de alterações
- As dependências (Domain, Application, Infra) permanecem inalteradas
- A estrutura de pastas e namespaces deve seguir o padrão existente
- Manter compatibilidade com variáveis de ambiente existentes (exceto `LAMBDA_MODE`)

