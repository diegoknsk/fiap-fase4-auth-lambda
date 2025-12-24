resource "aws_lambda_function" "function" {
  function_name = "${var.project_name}-${var.function_name}"
  handler       = var.package_type == "Image" ? null : var.handler
  runtime       = var.package_type == "Image" ? null : var.runtime
  role          = var.role_arn

  timeout      = var.timeout
  memory_size  = var.memory_size
  package_type = var.package_type

  # Para ZIP: usar filename e source_code_hash
  # O arquivo placeholder.zip deve existir no diretório do módulo
  filename         = var.package_type == "Zip" ? "${path.module}/placeholder.zip" : null
  source_code_hash = var.package_type == "Zip" ? (var.source_code_hash != "placeholder" ? var.source_code_hash : filebase64sha256("${path.module}/placeholder.zip")) : null

  # Para Image: usar image_uri
  image_uri = var.package_type == "Image" ? var.image_uri : null

  dynamic "environment" {
    for_each = length(var.environment_variables) > 0 ? [1] : []
    content {
      variables = var.environment_variables
    }
  }

  reserved_concurrent_executions = var.reserved_concurrent_executions

  dynamic "vpc_config" {
    for_each = var.vpc_config != null ? [var.vpc_config] : []
    content {
      security_group_ids = vpc_config.value.security_group_ids
      subnet_ids         = vpc_config.value.subnet_ids
    }
  }

  tags = merge(
    {
      Name        = "${var.project_name}-${var.function_name}"
      Environment = var.env
      Project     = var.project_name
      ManagedBy   = "Terraform"
    },
    var.common_tags
  )

  lifecycle {
    ignore_changes = [
      filename,
      source_code_hash,
      image_uri
    ]
  }
}

