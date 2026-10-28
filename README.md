# 7Academy — Sistema de Gestão de Eventos Acadêmicos

<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/csharp/csharp-plain.svg" align="left" width="50" height="50" /> 
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/dotnetcore/dotnetcore-original.svg" align="left" width="50" height="50" /> 
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/entityframeworkcore/entityframeworkcore-original.svg" align="left" width="50" height="50" /> 
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/microsoftsqlserver/microsoftsqlserver-original.svg" align="left" width="50" height="50" />
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/html5/html5-plain.svg" align="left" width="50" height="50" />
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/css3/css3-plain.svg" align="left" width="50" height="50" />
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/javascript/javascript-plain.svg" align="left" width="50" height="50" />
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/bootstrap/bootstrap-original.svg" align="left" width="50" height="50" />
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/git/git-original.svg" align="left" width="50" height="50" />
<img src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/github/github-original.svg" align="center" width="50" height="50" />

O **7Academy** é uma plataforma web de capacitação e gestão de eventos acadêmicos. O aluno se inscreve em um curso, estuda pelo material em PDF, realiza uma prova online com correção automática e, ao ser aprovado, emite seu certificado na hora.

O projeto centraliza em uma única aplicação todo o fluxo de participação — da divulgação do curso à emissão do certificado — substituindo processos manuais como listas de presença impressas, correção de provas em papel e certificados preenchidos à mão.

**Fluxo do aluno:**

```
Inscrição no curso  →  Material de estudo (PDF)  →  Prova online  →  Nota ≥ 7,0  →  Certificado (PDF)
```

## Principais Funcionalidades

* Cadastro de usuários com **nome completo**, e-mail e senha.
* Controle de acesso por perfil de **Organizador** e **Participante**.
* Criação, edição, visualização e exclusão de cursos e eventos.
* **Catálogo oficial com 7 cursos** de 20 horas cada: Java + POO, C# e .NET + POO, Python + POO, HTML5/CSS3/JavaScript, Microsoft SQL Server, Git e GitHub e Inteligência Artificial.
* Inscrições sem limite de vagas, com bloqueio de inscrição duplicada.
* **Material de estudo em PDF** para cada curso, com explicações, exemplos de código e resumo para a prova.
* **Prova online** com 10 questões por curso (70 no total) e **correção automática**.
* **Questões e alternativas embaralhadas** a cada tentativa.
* **Tempo de espera de 30 minutos** para nova tentativa após reprovação.
* Alunos aprovados podem refazer a prova quando quiserem, mantendo sempre a maior nota.
* **Histórico de notas** com data, nota e resultado de cada tentativa.
* **Certificado em PDF** gerado automaticamente com nome completo, curso, carga horária, nota e data da aprovação.
* Gerenciamento de presença e notas pelo organizador.
* Painel de perfil traduzido para português, com **download e exclusão dos dados pessoais (LGPD)**.

## Tecnologias Utilizadas

* **Back-end:** C#, .NET 8, ASP.NET Core MVC, Entity Framework Core
* **Front-end:** HTML5, CSS3, JavaScript, Bootstrap 5 e Bootstrap Icons
* **Banco de Dados:** Microsoft SQL Server (LocalDB)
* **Autenticação e Autorização:** ASP.NET Core Identity e Roles
* **Geração de PDF:** QuestPDF (fontes Montserrat e Dancing Script)
* **Arquitetura:** MVC (Model-View-Controller)
* **Controle de Versão:** Git e GitHub

## Arquitetura do Projeto

O 7Academy utiliza o padrão **MVC (Model-View-Controller)** para manter as responsabilidades da aplicação organizadas e facilitar sua manutenção e evolução.

```
EventManager.Web/
├── Controllers/   → EventsController (eventos e inscrições) e QuizController (provas e materiais)
├── Models/        → ApplicationUser, Event, EventRegistration, Question, QuizAttempt
├── Views/         → Telas Razor (.cshtml) no tema roxo e dourado da 7Academy
├── Services/      → CertificateService, StudyMaterialService e StudyContent (geração de PDFs)
├── Data/          → ApplicationDbContext, DbInitializer e QuizSeeder (cursos e questões)
├── Migrations/    → Histórico de alterações do banco de dados
├── Areas/Identity → Páginas de login, cadastro e perfil (ASP.NET Core Identity)
└── wwwroot/       → CSS, JavaScript, imagens e fontes
```

O `ApplicationDbContext` utiliza o ASP.NET Core Identity como base para o gerenciamento dos usuários e da autenticação.

## Perfis de Usuário

O sistema possui dois perfis principais, controlados através de **Roles** do ASP.NET Core Identity.

### Organizador

Representa professores e membros da coordenação responsáveis pelos cursos.

Pode:

* Criar, editar e excluir cursos e eventos.
* Acompanhar os participantes inscritos.
* Confirmar presença e consultar ou ajustar notas.

### Participante

Representa os alunos da plataforma.

Pode:

* Visualizar os cursos disponíveis e se inscrever.
* Baixar o material de estudo em PDF.
* Realizar a prova online e consultar o histórico de notas.
* Emitir o próprio certificado após a aprovação.
* Baixar ou excluir seus dados pessoais.

## Prova e Certificados

* Cada curso possui **10 questões de múltipla escolha**.
* A nota é calculada automaticamente de **0 a 10**, proporcional aos acertos.
* Com nota **igual ou superior a 7,0**, a presença é confirmada e o certificado é liberado imediatamente.
* Com nota **inferior a 7,0**, o aluno aguarda **30 minutos** para tentar novamente, com as questões em nova ordem.

O certificado é gerado em **PDF (A4 paisagem, página única)** e contém:

* Nome completo do participante.
* Nome do curso e carga horária.
* Nota obtida e percentual de aproveitamento.
* Data da aprovação como data de emissão.
* Assinatura da Diretora Acadêmica e Selo Dourado 7Academy.

Alunos com **nota 10** recebem o **Certificado Purple Master**; os demais aprovados recebem o **Certificado Oficial**.

## Regras de Negócio

* Um participante não pode se inscrever duas vezes no mesmo curso.
* Apenas organizadores podem criar e gerenciar cursos ou confirmar presença.
* A nota mínima para aprovação é **7,0**.
* Após uma reprovação, a nova tentativa só é liberada depois de **30 minutos** (regra validada também no servidor).
* É mantida sempre a **maior nota** obtida pelo aluno.
* O certificado só é emitido para alunos aprovados e com **nome completo** cadastrado.
* Participantes acessam somente os próprios materiais, provas e certificados.
* Organizadores com cursos cadastrados não podem excluir a conta sem antes remover ou transferir seus cursos.

## Segurança

A autenticação e a autorização são realizadas pelo **ASP.NET Core Identity**, responsável pelo gerenciamento seguro das contas, armazenamento protegido das senhas (hash) e controle de acesso baseado em Roles.

Os formulários utilizam proteção **Anti-Forgery Token** e a exclusão de conta exige a **confirmação da senha**. Informações sensíveis e credenciais não são armazenadas diretamente no código-fonte nem versionadas no repositório.

## Como Executar

### Pré-requisitos

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* Microsoft SQL Server ou **SQL Server LocalDB** (instalado junto com o Visual Studio)
* Visual Studio 2022 ou superior (recomendado) ou VS Code
* Ferramenta do Entity Framework: `dotnet tool install --global dotnet-ef`

### Passo a passo

1. Clone o repositório:

   ```bash
   git clone https://github.com/anajulialeite/7Academy.git
   ```

2. Acesse a pasta do projeto web:

   ```bash
   cd 7Academy/EventManager.Web
   ```

3. Confira a string de conexão em `appsettings.json` (por padrão, usa o LocalDB):

   ```json
   "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=aspnet-EventManager.Web;Trusted_Connection=True;MultipleActiveResultSets=true"
   ```

4. Crie o banco de dados aplicando as migrations:

   ```bash
   dotnet ef database update
   ```

5. Execute a aplicação:

   ```bash
   dotnet run
   ```

6. Acesse no navegador o endereço exibido no terminal (ex.: `https://localhost:7119`).

Na primeira execução, o sistema cria automaticamente os perfis **Organizador** e **Participante**. Os **7 cursos oficiais e as 70 questões** são cadastrados assim que existir um usuário com o perfil de Organizador.

## Documentação

A documentação técnica e acadêmica do projeto está disponível na pasta [`docs`](./docs).

* [Resultado 1 — Análise, Modelagem e Estrutura do 7Academy](docs/Cronograma_Estagio_assinado.pdf)

A documentação acompanha a evolução do sistema durante as etapas de desenvolvimento, incluindo o Relatório de Testes e Homologação.

## Autora

Ana Júlia de Lima Aguiar Leite

<a href="https://www.linkedin.com/in/anajulialimaleite/" style="text-decoration:none" target="_blank" rel="noopener noreferrer">
    <img src="https://img.shields.io/badge/Linkedin-%231C003F?style=for-the-badge&logo=LinkedIn&logoColor=white" alt="LinkedIn"/>
</a>

## License

[![MIT License](https://img.shields.io/badge/License-MIT-%231C003F.svg)](./LICENSE)
