# Script para corrigir o estado do Terraform removendo security groups que ja existem na AWS
# Este script remove os recursos do estado sem destrui-los na AWS

Write-Host "Iniciando correcao do estado do Terraform..." -ForegroundColor Yellow

# Navegar para o diretorio terraform
Set-Location $PSScriptRoot

# Verificar se terraform esta instalado
if (-not (Get-Command terraform -ErrorAction SilentlyContinue)) {
    Write-Host "Erro: Terraform nao encontrado no PATH" -ForegroundColor Red
    exit 1
}

# Inicializar o Terraform se necessario
Write-Host ""
Write-Host "Inicializando Terraform..." -ForegroundColor Cyan
terraform init

if ($LASTEXITCODE -ne 0) {
    Write-Host "Erro ao inicializar Terraform" -ForegroundColor Red
    exit 1
}

# Listar recursos no estado
Write-Host ""
Write-Host "Recursos relacionados a security groups no estado:" -ForegroundColor Cyan
terraform state list | Select-String "sg_lambda"

# Remover sg_lambda[0] do estado se existir
$sgLambdaExists = terraform state list | Select-String "aws_security_group.sg_lambda\[0\]"
if ($sgLambdaExists) {
    Write-Host ""
    Write-Host "Removendo aws_security_group.sg_lambda[0] do estado (sem destruir na AWS)..." -ForegroundColor Yellow
    terraform state rm 'aws_security_group.sg_lambda[0]'
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "OK: aws_security_group.sg_lambda[0] removido do estado com sucesso" -ForegroundColor Green
    } else {
        Write-Host "ERRO: Falha ao remover aws_security_group.sg_lambda[0] do estado" -ForegroundColor Red
    }
} else {
    Write-Host ""
    Write-Host "aws_security_group.sg_lambda[0] nao encontrado no estado" -ForegroundColor Gray
}

# Remover sg_lambdas_auth[0] do estado se existir
$sgLambdasAuthExists = terraform state list | Select-String "aws_security_group.sg_lambdas_auth\[0\]"
if ($sgLambdasAuthExists) {
    Write-Host ""
    Write-Host "Removendo aws_security_group.sg_lambdas_auth[0] do estado (sem destruir na AWS)..." -ForegroundColor Yellow
    terraform state rm 'aws_security_group.sg_lambdas_auth[0]'
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "OK: aws_security_group.sg_lambdas_auth[0] removido do estado com sucesso" -ForegroundColor Green
    } else {
        Write-Host "ERRO: Falha ao remover aws_security_group.sg_lambdas_auth[0] do estado" -ForegroundColor Red
    }
} else {
    Write-Host ""
    Write-Host "aws_security_group.sg_lambdas_auth[0] nao encontrado no estado" -ForegroundColor Gray
}

Write-Host ""
Write-Host "Correcao concluida!" -ForegroundColor Green
Write-Host "Agora voce pode executar terraform plan para verificar se o problema foi resolvido" -ForegroundColor Cyan
