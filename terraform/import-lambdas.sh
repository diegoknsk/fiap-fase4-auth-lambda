#!/bin/bash

set -e

PROJECT_NAME="${PROJECT_NAME:-autenticacao}"
AWS_REGION="${AWS_REGION:-us-east-1}"

echo "🔍 Importando funções Lambda existentes para o state do Terraform..."
echo "Project Name: $PROJECT_NAME"
echo "AWS Region: $AWS_REGION"
echo ""

if [[ ! -d ".terraform" ]]; then
    echo "⚠️  Terraform não inicializado. Execute 'terraform init' primeiro."
    exit 1
fi

FUNCTION_NAME_1="${PROJECT_NAME}-auth-customer-lambda"
echo "📦 Importando: $FUNCTION_NAME_1"
if terraform import "module.auth_customer_lambda.aws_lambda_function.function" "$FUNCTION_NAME_1" 2>&1; then
    echo "✅ $FUNCTION_NAME_1 importada com sucesso!"
else
    echo "⚠️  Erro ao importar $FUNCTION_NAME_1. Verifique se a função existe."
fi

FUNCTION_NAME_2="${PROJECT_NAME}-auth-admin-lambda"
echo "📦 Importando: $FUNCTION_NAME_2"
if terraform import "module.auth_admin_lambda.aws_lambda_function.function" "$FUNCTION_NAME_2" 2>&1; then
    echo "✅ $FUNCTION_NAME_2 importada com sucesso!"
else
    echo "⚠️  Erro ao importar $FUNCTION_NAME_2. Verifique se a função existe."
fi

FUNCTION_NAME_3="${PROJECT_NAME}-auth-migrator-lambda"
echo "📦 Importando: $FUNCTION_NAME_3"
if terraform import "module.auth_migrator_lambda.aws_lambda_function.function" "$FUNCTION_NAME_3" 2>&1; then
    echo "✅ $FUNCTION_NAME_3 importada com sucesso!"
else
    echo "⚠️  Erro ao importar $FUNCTION_NAME_3. Verifique se a função existe."
fi

echo ""
echo "✅ Importação concluída!"
echo "💡 Execute 'terraform plan' para verificar se tudo está sincronizado."

