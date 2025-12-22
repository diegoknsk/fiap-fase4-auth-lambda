# Storie-08: Deploy Lambda via ECR e GitHub Actions

## Descrição
Como desenvolvedor, quero automatizar o deploy do Lambda através de build de imagem Docker, push para ECR e atualização da função Lambda, para que o processo de deploy seja automatizado e confiável.

## Objetivo
Criar workflow do GitHub Actions que automatiza o processo completo de deploy: build da imagem Docker do Lambda, push para ECR da AWS e atualização da função Lambda com a nova imagem, garantindo deploy automatizado após merge na branch main.

## Escopo Técnico
- Tecnologias: Docker, AWS ECR, AWS Lambda, GitHub Actions, AWS CLI
- Arquivos afetados:
  - `Dockerfile` (na raiz ou em diretório específico)
  - `.github/workflows/deploy-lambda.yml`
  - `.dockerignore` (opcional, mas recomendado)
- Recursos AWS:
  - Repositório ECR para armazenar imagem Docker
  - Função Lambda configurada para usar imagem de container
  - IAM permissions para GitHub Actions acessar ECR e Lambda

## Subtasks

- [Subtask 01: Criar Dockerfile para Lambda](./subtask/Subtask-01-Criar_Dockerfile_Lambda.md)
- [Subtask 02: Criar arquivo .dockerignore](./subtask/Subtask-02-Criar_dockerignore.md)
- [Subtask 03: Criar workflow GitHub Actions para build e push ECR](./subtask/Subtask-03-Criar_workflow_build_push_ECR.md)
- [Subtask 04: Adicionar step para atualizar função Lambda](./subtask/Subtask-04-Adicionar_step_atualizar_Lambda.md)
- [Subtask 05: Configurar secrets e variáveis no GitHub](./subtask/Subtask-05-Configurar_secrets_GitHub.md)
- [Subtask 06: Testar deploy completo no ambiente de desenvolvimento](./subtask/Subtask-06-Testar_deploy_completo.md)

## Critérios de Aceite da História

- [ ] Dockerfile criado e funcional para Lambda .NET 8
- [ ] Arquivo .dockerignore criado para otimizar build
- [ ] Workflow GitHub Actions criado para build e push ECR
- [ ] Workflow faz build da imagem Docker corretamente
- [ ] Workflow faz push da imagem para ECR com tag apropriada
- [ ] Workflow atualiza função Lambda com nova imagem
- [ ] Secrets configurados no GitHub (AWS credentials, ECR repository, Lambda function name)
- [ ] Workflow executa automaticamente após merge na main
- [ ] Workflow pode ser executado manualmente (workflow_dispatch)
- [ ] Imagem Docker otimizada (tamanho razoável)
- [ ] Lambda atualizado com sucesso após deploy
- [ ] Logs do workflow mostram execução bem-sucedida
- [ ] Código compila sem erros
- [ ] Sem violações críticas de Sonar

