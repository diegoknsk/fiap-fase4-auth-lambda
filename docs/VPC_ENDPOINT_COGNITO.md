# VPC Endpoint para Cognito - Solução para Lambda em VPC

## Problema

Quando um Lambda está configurado em VPC, ele **perde acesso à internet pública** por padrão. Isso significa que:

- ✅ **Funciona**: Acesso ao RDS (dentro da VPC)
- ❌ **Não funciona**: Acesso ao Cognito (serviço público da AWS)
- ❌ **Não funciona**: Acesso a outros serviços AWS públicos

## Solução: VPC Endpoint

Criamos um **VPC Endpoint** para o Cognito, que permite que o Lambda acesse o Cognito **sem sair da VPC** (sem precisar de NAT Gateway).

### Vantagens do VPC Endpoint

- ✅ **Mais seguro**: Tráfego fica dentro da rede AWS
- ✅ **Mais econômico**: Sem custo de transferência de dados (apenas ~$7/mês por endpoint)
- ✅ **Mais rápido**: Latência menor que NAT Gateway
- ✅ **Sem dependência de NAT Gateway**: Não precisa configurar NAT Gateway

### Desvantagens

- ⚠️ Custo adicional: ~$7/mês por VPC Endpoint (Interface type)
- ⚠️ Precisa estar em todas as subnets onde o Lambda está

## Como Aplicar

### 1. Aplicar o Terraform

O arquivo `terraform/vpc_endpoint_cognito.tf` já está criado. Aplique com:

```bash
cd terraform
terraform init
terraform plan
terraform apply
```

### 2. Verificar se foi criado

```bash
# Listar VPC Endpoints
aws ec2 describe-vpc-endpoints --region us-east-1 --query 'VpcEndpoints[*].[VpcEndpointId,ServiceName,State]' --output table

# Verificar Security Group do VPC Endpoint
aws ec2 describe-security-groups --filters "Name=group-name,Values=vpc-endpoint-cognito-sg" --region us-east-1
```

### 3. Verificar Security Group do Lambda

O Security Group do Lambda (`fiap-fase4-auth-sg`) precisa permitir **saída HTTPS (porta 443)**. Verifique:

```bash
# Verificar regras de saída do Security Group da Lambda
aws ec2 describe-security-groups --filters "Name=group-name,Values=fiap-fase4-auth-sg" --region us-east-1 --query 'SecurityGroups[0].IpPermissionsEgress'
```

Se não houver regra de saída HTTPS, adicione:

**Via AWS Console:**
1. VPC → Security Groups
2. Selecione `fiap-fase4-auth-sg`
3. Outbound rules → Edit outbound rules
4. Add rule:
   - Type: HTTPS
   - Protocol: TCP
   - Port: 443
   - Destination: 0.0.0.0/0 (ou o Security Group do VPC Endpoint)
   - Description: "Allow HTTPS to Cognito via VPC Endpoint"
5. Save rules

**Via AWS CLI:**
```bash
# Obter Security Group ID da Lambda
LAMBDA_SG_ID=$(aws ec2 describe-security-groups --filters "Name=group-name,Values=fiap-fase4-auth-sg" --region us-east-1 --query 'SecurityGroups[0].GroupId' --output text)

# Adicionar regra de saída HTTPS
aws ec2 authorize-security-group-egress \
  --group-id $LAMBDA_SG_ID \
  --protocol tcp \
  --port 443 \
  --cidr 0.0.0.0/0 \
  --region us-east-1
```

## Alternativa: NAT Gateway

Se preferir usar NAT Gateway (mais caro, mas permite acesso a qualquer serviço externo):

### Custos
- NAT Gateway: ~$32/mês + custos de transferência de dados
- VPC Endpoint: ~$7/mês (sem custo de transferência)

### Quando usar NAT Gateway
- Se precisar acessar múltiplos serviços AWS públicos
- Se precisar acessar serviços externos (APIs públicas, etc.)
- Se já tiver NAT Gateway configurado na VPC

## Verificação

Após aplicar o VPC Endpoint, teste o Lambda:

```bash
# Testar endpoint de admin login
curl -X POST https://<lambda-function-url>/api/admin/login \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "senha123"}'
```

Se funcionar, o VPC Endpoint está configurado corretamente.

## Troubleshooting

### Erro: "Unable to reach Cognito"

**Causa**: Security Group do Lambda não permite saída HTTPS

**Solução**: Adicione regra de saída HTTPS (porta 443) no Security Group do Lambda

### Erro: "VPC Endpoint not found"

**Causa**: VPC Endpoint não foi criado ou está em outra VPC

**Solução**: Verifique se o VPC Endpoint foi criado na mesma VPC do Lambda

### Erro: "Timeout connecting to Cognito"

**Causa**: VPC Endpoint não está nas subnets corretas

**Solução**: Verifique se o VPC Endpoint está em todas as subnets onde o Lambda está

## Referências

- [AWS VPC Endpoints](https://docs.aws.amazon.com/vpc/latest/privatelink/vpc-endpoints.html)
- [VPC Endpoint for Cognito](https://docs.aws.amazon.com/cognito/latest/developerguide/vpc-endpoints.html)
- [Lambda in VPC](https://docs.aws.amazon.com/lambda/latest/dg/configuration-vpc.html)


