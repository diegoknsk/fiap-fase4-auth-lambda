# Subtask 08: Atualizar workflow de deploy

## Descrição
Atualizar o workflow do GitHub Actions para construir e fazer push das 3 imagens ECR (Customer, Admin, Migrator) e atualizar as 3 Lambdas correspondentes.

## Passos de implementação
1. Atualizar `.github/workflows/deploy-lambda.yml`:
   - Renomear step `build-auth` para `build-customer`
   - Atualizar `Dockerfile.auth-lambda` para `Dockerfile.auth-customer-lambda`
   - Atualizar tags de imagem:
     - `auth-lambda-{sha}` → `auth-customer-lambda-{sha}`
     - `auth-lambda-latest` → `auth-customer-lambda-latest`
   - Atualizar output `lambda_auth_image_uri` para `lambda_auth_customer_image_uri`
   - Manter steps de `build-admin` e `build-migrator` (já devem estar corretos)

2. Atualizar job de atualização de Lambda:
   - Renomear step de atualização de `auth-lambda` para `auth-customer-lambda`
   - Atualizar variável de output usada
   - Manter steps de atualização de `auth-admin-lambda` e `auth-migrator-lambda`

3. Verificar que o workflow:
   - Cria repositório ECR (se necessário)
   - Constrói 3 imagens Docker
   - Faz push das 3 imagens para ECR
   - Atualiza as 3 funções Lambda com as novas imagens

4. Atualizar outputs do job `build-and-push-images`:
   ```yaml
   outputs:
     lambda_auth_customer_image_uri: ${{ steps.meta-customer.outputs.image }}
     lambda_auth_admin_image_uri: ${{ steps.meta-admin.outputs.image }}
     lambda_auth_migrator_image_uri: ${{ steps.meta-migrator.outputs.image }}
   ```

5. Verificar que os secrets e variáveis de ambiente estão corretos

## Arquivos a modificar
- `.github/workflows/deploy-lambda.yml`

## Estrutura esperada do workflow

### Job: build-and-push-images
```yaml
jobs:
  build-and-push-images:
    outputs:
      lambda_auth_customer_image_uri: ${{ steps.meta-customer.outputs.image }}
      lambda_auth_admin_image_uri: ${{ steps.meta-admin.outputs.image }}
      lambda_auth_migrator_image_uri: ${{ steps.meta-migrator.outputs.image }}
    
    steps:
      - name: Build and push auth-customer-lambda image
        uses: docker/build-push-action@v5
        with:
          file: ./Dockerfile.auth-customer-lambda
          tags: |
            .../fiap-fase4-auth-lambda:auth-customer-lambda-${{ github.sha }}
            .../fiap-fase4-auth-lambda:auth-customer-lambda-latest
      
      - name: Set auth-customer-lambda image URI
        id: meta-customer
        run: |
          echo "image=..." >> $GITHUB_OUTPUT
      
      # ... steps para admin e migrator
```

### Job: update-lambda-functions
```yaml
  update-lambda-functions:
    needs: build-and-push-images
    steps:
      - name: Update auth-customer-lambda
        run: |
          aws lambda update-function-code \
            --function-name ${{ secrets.LAMBDA_CUSTOMER_FUNCTION_NAME }} \
            --image-uri ${{ needs.build-and-push-images.outputs.lambda_auth_customer_image_uri }}
      
      # ... steps para admin e migrator
```

## Como testar
- Fazer push para branch e verificar que o workflow executa
- Verificar que as 3 imagens são construídas
- Verificar que as 3 imagens são enviadas para ECR
- Verificar que as 3 Lambdas são atualizadas
- Verificar logs do workflow para garantir que não há erros

## Critérios de aceitação
- [ ] Workflow atualizado com build de 3 imagens
- [ ] Tags de imagem atualizadas corretamente
- [ ] Outputs do job atualizados
- [ ] Steps de atualização de Lambda atualizados
- [ ] Workflow executa sem erros
- [ ] 3 imagens são construídas e enviadas para ECR
- [ ] 3 Lambdas são atualizadas corretamente

