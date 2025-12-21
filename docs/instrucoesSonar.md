#ajustar configuracoes do kernel para rodar o sonar

docker --version
docker compose version

sudo sysctl -w vm.max_map_count=262144
echo "vm.max_map_count=262144" | sudo tee /etc/sysctl.d/99-sonarqube.conf
sudo sysctl --system

#Criar docker-compose.yml

services:
  sonarqube:
    image: sonarqube:lts-community
    container_name: sonarqube
    depends_on:
      - db
    environment:
      SONAR_JDBC_URL: jdbc:postgresql://db:5432/sonar
      SONAR_JDBC_USERNAME: sonar
      SONAR_JDBC_PASSWORD: sonar
    ports:
      - "9000:9000"
    volumes:
      - sonarqube_data:/opt/sonarqube/data
      - sonarqube_extensions:/opt/sonarqube/extensions
      - sonarqube_logs:/opt/sonarqube/logs

  db:
    image: postgres:15
    container_name: sonarqube_db
    environment:
      POSTGRES_USER: sonar
      POSTGRES_PASSWORD: sonar
      POSTGRES_DB: sonar
    volumes:
      - sonarqube_db:/var/lib/postgresql/data

volumes:
  sonarqube_data:
  sonarqube_extensions:
  sonarqube_logs:
  sonarqube_db:

#subir

mkdir ~/sonarqube
cd ~/sonarqube
touch docker-compose.yml
nano docker-compose.yml


cd ~/sonarqube
docker compose up -d
docker logs -f sonarqube


1) Criar o projeto no Sonar (local)

Clique em Manually

Preencha:

Project display name: FastFood.Auth (ou o nome que preferir)

Project key: algo simples e único, tipo: fastfood-auth

Next / Create

Conceito pra fixar (avaliação): Project Key é o identificador técnico usado pelo scanner pra “enviar” a análise pro projeto certo.

2) Gerar o token (obrigatório pro scanner)

Depois de criar o projeto, ele vai pedir token.

Selecione Generate a token

Nome do token: local-dev

Copie e guarde o token (você não vai ver de novo)

Boa prática: token não vai em código; no local você pode colocar em variável de ambiente.

3) Rodar análise local em .NET com cobertura
3.1 Instalar ferramentas (uma vez só)
dotnet tool install --global dotnet-sonarscanner
dotnet tool install --global dotnet-reportgenerator-globaltool
export PATH="$PATH:$HOME/.dotnet/tools"

3.2 Rodar o scan (na raiz da solution)

A ideia é sempre: begin → build/test → end.

export SONAR_HOST_URL="http://localhost:9000"
export SONAR_TOKEN="COLE_SEU_TOKEN_AQUI"
export SONAR_PROJECT_KEY="fastfood-auth"

dotnet sonarscanner begin \
  /k:"$SONAR_PROJECT_KEY" \
  /d:sonar.host.url="$SONAR_HOST_URL" \
  /d:sonar.token="$SONAR_TOKEN" \
  /d:sonar.cs.opencover.reportsPaths="**/TestResults/**/coverage.opencover.xml"

dotnet build


Agora rode os testes gerando cobertura (OpenCover):

dotnet test \
  --results-directory ./TestResults \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=opencover \
  /p:CoverletOutput=./TestResults/coverage/


Feche a análise:

dotnet sonarscanner end /d:sonar.token="$SONAR_TOKEN"

3.3 (Opcional) Gerar relatório HTML pra você ver localmente
reportgenerator \
  -reports:"./TestResults/**/coverage.opencover.xml" \
  -targetdir:"./TestResults/coveragereport" \
  -reporttypes:Html

4) Validar que a cobertura foi “achada”

Se no Sonar aparecer 0% coverage, normalmente é path do report.

Cheque se o arquivo existe:

find . -name "coverage.opencover.xml"


Se ele estiver em outro lugar, você ajusta o sonar.cs.opencover.reportsPaths para o caminho real.

5) Sobre o aviso “version is no longer active”

Isso é só um alerta de versão. Se você quiser “resolver” agora:

cd ~/sonarqube
docker compose down
docker pull sonarqube:lts-community
docker compose up -d

#### no projeto .net
dotnet tool install --global dotnet-sonarscanner
dotnet-sonarscanner --version
$env:SONAR_HOST_URL="http://localhost:9000"
$env:SONAR_TOKEN="SEU_TOKEN"
$env:SONAR_PROJECT_KEY="fastfood-auth"


### rodar sonar de fato

dotnet-sonarscanner begin /k:"$env:SONAR_PROJECT_KEY" /d:sonar.host.url="$env:SONAR_HOST_URL" /d:sonar.token="$env:SONAR_TOKEN" /d:sonar.cs.opencover.reportsPaths="**\TestResults\**\coverage.opencover.xml"


dotnet build
dotnet test --results-directory .\TestResults /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=.\TestResults\coverage\

dotnet-sonarscanner end /d:sonar.token="$env:SONAR_TOKEN"


-----------------------------------------------------------------
# CONFIGURAÇÃO CORRIGIDA PARA WINDOWS (PowerShell)
# Certifique-se de ter as variáveis de ambiente configuradas:
# $env:SONAR_HOST_URL="http://localhost:9000"
# $env:SONAR_TOKEN="seu_token_aqui"
# $env:SONAR_PROJECT_KEY="fastfood-auth"

1-> Iniciar análise SonarQube
# IMPORTANTE: O Coverlet gera o arquivo dentro da pasta do projeto de teste
# Use o caminho relativo ao projectBaseDir ou padrão recursivo
# Adicione sonar.cs.vstest.reportsPaths para o SonarQube detectar os testes executados
dotnet-sonarscanner begin `
  /k:"$env:SONAR_PROJECT_KEY" `
  /d:sonar.host.url="$env:SONAR_HOST_URL" `
  /d:sonar.token="$env:SONAR_TOKEN" `
  /d:sonar.projectBaseDir="C:\Projetos\Fiap\fiap-fase4-auth-lambda" `
  /d:sonar.sources="src" `
  /d:sonar.tests="src/tests" `
  /d:sonar.cs.opencover.reportsPaths="src/tests/FastFood.Auth.Tests.Unit/TestResults/coverage/coverage.opencover.xml" `
  /d:sonar.cs.vstest.reportsPaths="**/TestResults/*.trx" `
  /d:sonar.exclusions="**/bin/**,**/obj/**,**/.vs/**,**/.sonarqube/**,**/docs/**,**/rules/**,**/story/**,**/*.md,**/*.mdc,**/FastFood.Auth.Tests.Bdd/**"

2-> Build da solução
dotnet build .\FastFood.Auth.sln

3-> Executar testes com cobertura
# IMPORTANTE: O Coverlet gera o arquivo dentro da pasta do projeto de teste
# O arquivo será gerado em: src/tests/FastFood.Auth.Tests.Unit/TestResults/coverage/coverage.opencover.xml
# O --logger "trx" gera o arquivo de resultados de testes que o SonarQube precisa para contar os testes
dotnet test .\FastFood.Auth.sln `
  --results-directory .\TestResults `
  --logger "trx;LogFileName=test_results.trx" `
  /p:CollectCoverage=true `
  /p:CoverletOutputFormat=opencover `
  /p:CoverletOutput=TestResults\coverage\ `
  /p:CoverletOutputName="coverage.opencover.xml"

4-> Verificar se os arquivos foram gerados
# Verificar arquivo de cobertura (OpenCover)
Test-Path .\src\tests\FastFood.Auth.Tests.Unit\TestResults\coverage\coverage.opencover.xml

# Verificar arquivo de resultados de testes (TRX) - necessário para o SonarQube contar os testes
Test-Path .\TestResults\test_results.trx

# Se os arquivos não existirem, verificar onde foram gerados:
Get-ChildItem -Path . -Recurse -Filter "coverage.opencover.xml" | Select-Object FullName
Get-ChildItem -Path . -Recurse -Filter "*.trx" | Select-Object FullName

# ALTERNATIVA: Se o padrão não funcionar, executar testes por projeto e fazer merge
# dotnet test .\src\tests\FastFood.Auth.Tests.Unit\FastFood.Auth.Tests.Unit.csproj `
#   /p:CollectCoverage=true `
#   /p:CoverletOutputFormat=opencover `
#   /p:CoverletOutput=.\TestResults\coverage\ `
#   /p:CoverletOutputName="coverage.opencover.xml"

5-> Finalizar análise SonarQube
dotnet-sonarscanner end /d:sonar.token="$env:SONAR_TOKEN"

# TROUBLESHOOTING:
# Se o SonarQube não encontrar os testes OU não contar os testes:
# 
# IMPORTANTE: O SonarQube precisa de DOIS arquivos:
# 1. coverage.opencover.xml - para cobertura de código
# 2. *.trx - para contar os testes executados
#
# 1. Verifique se o arquivo coverage.opencover.xml foi gerado:
#    Test-Path .\src\tests\FastFood.Auth.Tests.Unit\TestResults\coverage\coverage.opencover.xml
# 2. Verifique se o arquivo TRX foi gerado:
#    Test-Path .\TestResults\test_results.trx
# 3. O caminho no Sonar deve ser relativo ao projectBaseDir:
#    src/tests/FastFood.Auth.Tests.Unit/TestResults/coverage/coverage.opencover.xml
# 4. O arquivo TRX é gerado no --results-directory especificado no comando dotnet test
# 5. Se houver múltiplos projetos de teste, você pode usar padrão recursivo:
#    /d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml"
#    /d:sonar.cs.vstest.reportsPaths="**/TestResults/*.trx"
# 6. Certifique-se de que o Coverlet está instalado nos projetos de teste (coverlet.msbuild e coverlet.collector)
# 7. O Coverlet sempre gera o arquivo relativo ao diretório do projeto de teste, não ao diretório de trabalho

# ALTERNATIVA: Usar reportgenerator para fazer merge dos arquivos de cobertura
# Após o passo 3, execute:
# reportgenerator `
#   -reports:"**/coverage.opencover.xml" `
#   -targetdir:".\TestResults\coverage" `
#   -reporttypes:"OpenCover" `
#   -classfilters:"-*Tests*"
# 
# Depois, ajuste o Sonar para usar o arquivo merged:
# /d:sonar.cs.opencover.reportsPaths=".\TestResults\coverage\coverage.opencover.xml"

 Invoke-WebRequest `  -Uri "$env:SONAR_HOST_URL/api/issues/search?componentKeys=$env:SONAR_PROJECT_KEY&ps=500" `  -Headers @{ Authorization = "Basic $([Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes($env:SONAR_TOKEN + ':')))" } `  -OutFile sonar-issues3.json
