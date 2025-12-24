$ErrorActionPreference = "Stop"

$PROJECT_NAME = if ($env:PROJECT_NAME) { $env:PROJECT_NAME } else { "autenticacao" }
$AWS_REGION = if ($env:AWS_REGION) { $env:AWS_REGION } else { "us-east-1" }

Write-Host "🔍 Importando funções Lambda existentes para o state do Terraform..." -ForegroundColor Cyan
Write-Host "Project Name: $PROJECT_NAME"
Write-Host "AWS Region: $AWS_REGION"
Write-Host ""

if (-not (Test-Path ".terraform")) {
    Write-Host "⚠️  Terraform não inicializado. Execute 'terraform init' primeiro." -ForegroundColor Yellow
    exit 1
}

$FUNCTION_NAME_1 = "$PROJECT_NAME-auth-lambda"
Write-Host "📦 Importando: $FUNCTION_NAME_1" -ForegroundColor Yellow
$result1 = terraform import "module.auth_lambda.aws_lambda_function.function" $FUNCTION_NAME_1 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ $FUNCTION_NAME_1 importada com sucesso!" -ForegroundColor Green
} else {
    Write-Host "⚠️  Erro ao importar $FUNCTION_NAME_1. Verifique se a função existe." -ForegroundColor Red
    Write-Host $result1 -ForegroundColor Red
}

$FUNCTION_NAME_2 = "$PROJECT_NAME-auth-admin-lambda"
Write-Host "📦 Importando: $FUNCTION_NAME_2" -ForegroundColor Yellow
$result2 = terraform import "module.auth_admin_lambda.aws_lambda_function.function" $FUNCTION_NAME_2 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ $FUNCTION_NAME_2 importada com sucesso!" -ForegroundColor Green
} else {
    Write-Host "⚠️  Erro ao importar $FUNCTION_NAME_2. Verifique se a função existe." -ForegroundColor Red
    Write-Host $result2 -ForegroundColor Red
}

$FUNCTION_NAME_3 = "$PROJECT_NAME-auth-migrator-lambda"
Write-Host "📦 Importando: $FUNCTION_NAME_3" -ForegroundColor Yellow
$result3 = terraform import "module.auth_migrator_lambda.aws_lambda_function.function" $FUNCTION_NAME_3 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ $FUNCTION_NAME_3 importada com sucesso!" -ForegroundColor Green
} else {
    Write-Host "⚠️  Erro ao importar $FUNCTION_NAME_3. Verifique se a função existe." -ForegroundColor Red
    Write-Host $result3 -ForegroundColor Red
}

Write-Host ""
Write-Host "✅ Importação concluída!" -ForegroundColor Green
Write-Host "💡 Execute 'terraform plan' para verificar se tudo está sincronizado." -ForegroundColor Cyan

