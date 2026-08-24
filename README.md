# UniEvent — Sistema de Gestão de Eventos Acadêmicos

Bem-vindo ao repositório do **UniEvent**, um sistema web completo para gerenciamento de eventos acadêmicos, desenvolvido em C# com a plataforma ASP.NET Core MVC. 

## 🎯 Objetivo do Sistema

O UniEvent foi projetado para centralizar a organização, inscrições, controle de presença (check-in), emissão automatizada de certificados em PDF e validação pública de autenticidade para eventos acadêmicos. 
Ele resolve o problema das listas de presença impressas e formulários genéricos, automatizando o fluxo desde o planejamento da agenda até a entrega do comprovante de horas complementares.

## 🛠️ Tecnologias Utilizadas

* **Linguagem**: C# 12+
* **Framework Web**: ASP.NET Core MVC (.NET 8+)
* **Acesso a Dados**: Entity Framework Core
* **Banco de Dados**: SQL Server
* **Autenticação/Autorização**: ASP.NET Core Identity
* **Frontend**: HTML5, CSS3, e Bootstrap

## 🏗️ Arquitetura e Estrutura

O sistema adota a arquitetura **MVC (Model-View-Controller)**, balanceando uma estruturação robusta e simples sem complexidade artificial:
- **Models/Entities**: Modelagem de domínio e banco de dados.
- **Views**: Interfaces do usuário responsivas.
- **Controllers**: Intermediação de dados.
- **Services**: Isolamento das regras de negócio críticas (como limite de vagas e geração de PDF).
- **Data**: Contexto de persistência do Entity Framework.

## 👥 Perfis de Usuário
O sistema é dividido em dois perfis principais gerenciados por *Roles* do ASP.NET Core Identity:
1. **Organizadores (Coordenação/Professores)**: Podem criar eventos, controlar vagas e validar presença.
2. **Participantes (Alunos)**: Podem visualizar a agenda, realizar inscrições e emitir seus próprios certificados de presença confirmada.

## 📖 Documentação do Projeto

A evolução da modelagem e regras de negócio está sendo documentada na pasta `docs/`.
- [Resultado 1: Análise e Modelagem Inicial](docs/Resultado_1.md)

---
*Este projeto está sendo construído de forma incremental, seguindo as entregas do Estágio Supervisionado III, focado na robustez arquitetural para portfólio profissional.*
