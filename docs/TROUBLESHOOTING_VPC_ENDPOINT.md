# Troubleshooting VPC Endpoint para Cognito

## Problema: Lambda não consegue acessar Cognito mesmo com VPC Endpoint

### Verificações Necessárias

#### 1. Verificar se o VPC Endpoint está ativo

```bash
aws ec2 describe-vpc-endpoints \
  --vpc-endpoint-ids vpce-0148fe446877baea7 \
  --region us-east-1 \
  --query 'VpcEndpoints[0].[State,PrivateDnsEnabled]'
```

**Esperado:**
- `State`: `available`
- `PrivateDnsEnabled`: `true`

#### 2. Verificar subnets do VPC Endpoint

```bash
# Verificar em quais subnets o VPC Endpoint está
aws ec2 describe-vpc-endpoints \
  --vpc-endpoint-ids vpce-0148fe446877baea7 \
  --region us-east-1 \
  --query 'VpcEndpoints[0].SubnetIds'
```

**Importante:** O VPC Endpoint deve estar em pelo menos uma subnet de cada zona de disponibilidade onde o Lambda está.

#### 3. Verificar subnets do Lambda

```bash
# Verificar em quais subnets o Lambda está
aws lambda get-function-configuration \
  --function-name fiap-fase4-infra-auth-lambda \
  --region us-east-1 \
  --query 'VpcConfig.SubnetIds'
```

**Problema comum:** Se o Lambda estiver em uma subnet que não tem VPC Endpoint, ele não conseguirá acessar o Cognito.

#### 4. Verificar DNS da VPC

O VPC precisa ter DNS resolution e DNS hostnames habilitados:

```bash
# Verificar configurações DNS da VPC
aws ec2 describe-vpcs \
  --vpc-ids vpc-03796bd4448b4d1c2 \
  --region us-east-1 \
  --query 'Vpcs[0].[EnableDnsSupport,EnableDnsHostnames]'
```

**Esperado:**
- `EnableDnsSupport`: `true`
- `EnableDnsHostnames`: `true`

Se estiver `false`, habilite:

```bash
# Habilitar DNS support
aws ec2 modify-vpc-attribute \
  --vpc-id vpc-03796bd4448b4d1c2 \
  --enable-dns-support \
  --region us-east-1

# Habilitar DNS hostnames
aws ec2 modify-vpc-attribute \
  --vpc-id vpc-03796bd4448b4d1c2 \
  --enable-dns-hostnames \
  --region us-east-1
```

#### 5. Verificar Security Groups

**Security Group do Lambda:**
```bash
# Verificar regras de saída HTTPS
aws ec2 describe-security-groups \
  --group-ids sg-0aa0b150737ac5232 \
  --region us-east-1 \
  --query 'SecurityGroups[0].IpPermissionsEgress[?FromPort==`443`]'
```

**Security Group do VPC Endpoint:**
```bash
# Verificar regras de entrada
aws ec2 describe-security-groups \
  --group-ids sg-029080b6e25b4fd54 \
  --region us-east-1 \
  --query 'SecurityGroups[0].IpPermissions[?FromPort==`443`]'
```

#### 6. Verificar entradas DNS privadas

O AWS cria automaticamente entradas DNS privadas quando `private_dns_enabled = true`. Verifique:

```bash
# Listar hosted zones privadas da VPC
aws route53 list-hosted-zones-by-vpc \
  --vpc-id vpc-03796bd4448b4d1c2 \
  --region us-east-1
```

#### 7. Testar resolução DNS do Lambda

Crie um teste simples no Lambda para verificar se o DNS está resolvendo corretamente:

```csharp
// Adicionar no CognitoService temporariamente para debug
var dnsResult = System.Net.Dns.GetHostAddresses("cognito-idp.us-east-1.amazonaws.com");
// Deve retornar IP privado do VPC Endpoint, não IP público
```

### Soluções Comuns

#### Solução 1: Garantir que VPC Endpoint está em todas as zonas do Lambda

Se o Lambda está em 6 subnets (todas as zonas) mas o VPC Endpoint está apenas em algumas, adicione o VPC Endpoint em todas as subnets suportadas.

#### Solução 2: Habilitar DNS na VPC

Se DNS não estiver habilitado, o Lambda não conseguirá resolver o endpoint privado.

#### Solução 3: Aguardar propagação DNS

Após criar o VPC Endpoint, pode levar alguns minutos para as entradas DNS serem propagadas. Aguarde 5-10 minutos e teste novamente.

#### Solução 4: Verificar Route Tables

As subnets do Lambda precisam ter route tables que permitam comunicação com o VPC Endpoint (geralmente automático, mas verifique).

### Teste Manual

Teste se o Lambda consegue resolver o DNS:

1. Crie um endpoint de teste no Lambda:
```csharp
[HttpGet("test-dns")]
public IActionResult TestDns()
{
    try
    {
        var addresses = System.Net.Dns.GetHostAddresses("cognito-idp.us-east-1.amazonaws.com");
        return Ok(new { 
            resolved = true, 
            addresses = addresses.Select(a => a.ToString()).ToArray() 
        });
    }
    catch (Exception ex)
    {
        return BadRequest(new { resolved = false, error = ex.Message });
    }
}
```

2. Chame o endpoint e verifique se retorna IPs privados (172.31.x.x) ao invés de IPs públicos.

### Logs do CloudWatch

Verifique os logs do Lambda para erros específicos:

```bash
aws logs tail /aws/lambda/fiap-fase4-infra-auth-lambda --follow --region us-east-1
```

Procure por:
- Timeout errors
- DNS resolution errors
- Connection refused errors

