terraform {
  backend "s3" {
    bucket = "godz-fiap-fase4-tfstate"
    key    = "lambda-auth/terraform.tfstate"
    region = "us-east-1"
    encrypt = true
  }
}
