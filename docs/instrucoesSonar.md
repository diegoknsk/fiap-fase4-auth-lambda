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
