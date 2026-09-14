# UniEvent — Sistema de Gestão de Eventos Acadêmicos

<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/csharp/csharp-plain.svg" align="left" width="50" height="50" /> 
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/dotnetcore/dotnetcore-original.svg" align="left" width="50" height="50" /> 
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/entityframeworkcore/entityframeworkcore-original.svg" align="left" width="50" height="50" /> 
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/microsoftsqlserver/microsoftsqlserver-original.svg" align="left" width="50" height="50" />
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/html5/html5-plain.svg" align="left" width="50" height="50" />
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/css3/css3-plain.svg" align="left" width="50" height="50" />
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/bootstrap/bootstrap-original.svg" align="left" width="50" height="50" />
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/git/git-original.svg" align="left" width="50" height="50" />
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/github/github-original.svg" align="center" width="50" height="50" />

O **UniEvent** é um sistema web desenvolvido para gerenciamento de eventos acadêmicos, permitindo organizar eventos, controlar inscrições e presença dos participantes e automatizar a emissão de certificados.

O projeto centraliza em uma única aplicação o fluxo de participação em eventos acadêmicos, desde a criação e divulgação do evento até o check-in e a emissão do certificado, substituindo processos manuais como listas de presença impressas e formulários separados.

## Principais Funcionalidades

* Cadastro e autenticação de usuários.
* Controle de acesso por perfil de **Organizador** e **Participante**.
* Criação, edição, visualização e exclusão de eventos acadêmicos.
* Definição de data, horário, carga horária e limite de vagas.
* Visualização dos eventos disponíveis.
* Inscrição de participantes em eventos com controle automático de vagas.
* Bloqueio de inscrições duplicadas no mesmo evento.
* Check-in e confirmação de presença pelos organizadores.
* Histórico de eventos e participações do usuário.
* Emissão de certificados em PDF para participantes com presença confirmada.
* Geração de código único de validação para cada certificado.
* Página pública para consulta e validação da autenticidade dos certificados.

## Tecnologias Utilizadas

* **Back-end:** C#, ASP.NET Core MVC, Entity Framework Core
* **Front-end:** HTML5, CSS3, Bootstrap
* **Banco de Dados:** Microsoft SQL Server
* **Autenticação e Autorização:** ASP.NET Core Identity e Roles
* **Arquitetura:** MVC (Model-View-Controller)
* **Controle de Versão:** Git e GitHub

## Arquitetura do Projeto

O UniEvent utiliza o padrão **MVC (Model-View-Controller)** para manter as responsabilidades da aplicação organizadas e facilitar sua manutenção e evolução.

A estrutura principal é dividida em:

* **Models:** entidades e modelos utilizados pela aplicação.
* **Views:** interfaces responsáveis pela interação com o usuário.
* **Controllers:** responsáveis pela comunicação entre as Views, regras da aplicação e dados.
* **Services:** concentração das regras de negócio que exigem tratamento específico, como controle de vagas e geração de certificados.
* **Data:** configuração do contexto e acesso aos dados utilizando Entity Framework Core.

O `ApplicationDbContext` utiliza o ASP.NET Core Identity como base para gerenciamento dos usuários e autenticação da aplicação.

## Perfis de Usuário

O sistema possui dois perfis principais, controlados através de **Roles** do ASP.NET Core Identity.

### Organizador

Representa professores e membros da coordenação responsáveis pela organização dos eventos.

Pode:

* Criar e gerenciar eventos.
* Definir quantidade de vagas.
* Acompanhar participantes inscritos.
* Realizar o check-in e confirmar a presença dos participantes.

### Participante

Representa os alunos que participam dos eventos acadêmicos.

Pode:

* Visualizar os eventos disponíveis.
* Realizar inscrições.
* Consultar seu histórico de participação.
* Emitir certificados quando sua presença estiver confirmada.

## Certificados

Os certificados são disponibilizados somente para participantes que tiveram sua presença confirmada pelo organizador do evento.

Cada certificado contém informações como:

* Nome do participante.
* Nome do evento.
* Data do evento.
* Carga horária.
* Data de emissão.
* Código único de validação.

O sistema também possui uma página pública para consulta desse código, permitindo que terceiros verifiquem a autenticidade do certificado sem necessidade de login.

## Regras de Negócio

Algumas das principais regras implementadas no sistema são:

* Um participante não pode se inscrever duas vezes no mesmo evento.
* Novas inscrições são bloqueadas quando o limite de vagas é atingido.
* Apenas organizadores podem criar e gerenciar eventos ou confirmar presença.
* Participantes podem emitir somente os próprios certificados.
* Um certificado só pode ser emitido após a confirmação da presença.
* Cada certificado possui um código único para validação de autenticidade.

## Segurança

A autenticação e autorização dos usuários são realizadas através do **ASP.NET Core Identity**, responsável pelo gerenciamento seguro das contas, armazenamento protegido das senhas e controle de acesso baseado em Roles.

Informações sensíveis e credenciais utilizadas durante o desenvolvimento não são armazenadas diretamente no código-fonte ou versionadas no repositório.

## Documentação

A documentação técnica e acadêmica do projeto está disponível na pasta [`docs`](./docs).

* [Resultado 1 — Análise, Modelagem e Estrutura do UniEvent](docs/Cronograma_Estagio_assinado.pdf)

A documentação acompanha a evolução do sistema durante as etapas de desenvolvimento.

## Status do Projeto

**Em desenvolvimento.**

O UniEvent está sendo desenvolvido de forma incremental durante o **Estágio Supervisionado III**, com novas funcionalidades, testes e documentação sendo adicionados conforme a evolução do projeto.

## Autora

Ana Júlia de Lima Aguiar Leite

<a href="https://www.linkedin.com/in/anajulialimaleite/" style="text-decoration:none" target="_blank" rel="noopener noreferrer">
    <img src="https://img.shields.io/badge/Linkedin-%231C003F?style=for-the-badge&logo=LinkedIn&logoColor=white" alt="LinkedIn"/>
</a>

## License

[![MIT License](https://img.shields.io/badge/License-MIT-%231C003F.svg)](./LICENSE)
