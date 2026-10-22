using EventManager.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Web.Data
{
    /// <summary>
    /// Cria os 7 eventos oficiais da 7Academy, cada um com 10 questões de prova.
    /// Executa apenas uma vez: eventos que já existem (mesmo título) não são duplicados.
    /// </summary>
    public static class QuizSeeder
    {
        private record Q(string Text, string A, string B, string C, string D, string Correct);

        private record SeedEvent(string Title, string Description, DateTime Date, int Hours, int Slots, Q[] Questions);

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Organizador responsável pelos eventos oficiais
            var organizer = await userManager.FindByEmailAsync("anajulia_aninha2@hotmail.com")
                            ?? (await userManager.GetUsersInRoleAsync("Organizador")).FirstOrDefault();
            if (organizer == null) return; // sem organizador ainda: tenta de novo na próxima inicialização

            foreach (var seed in GetEvents())
            {
                var ev = await context.Events.Include(e => e.Questions)
                                             .FirstOrDefaultAsync(e => e.Title == seed.Title);
                if (ev == null)
                {
                    ev = new Event
                    {
                        Title = seed.Title,
                        Description = seed.Description,
                        DateTime = seed.Date,
                        WorkloadHours = seed.Hours,
                        TotalSlots = seed.Slots,
                        OrganizerId = organizer.Id
                    };
                    context.Events.Add(ev);
                }

                if (!ev.Questions.Any())
                {
                    foreach (var q in seed.Questions)
                    {
                        ev.Questions.Add(new Question
                        {
                            Text = q.Text, OptionA = q.A, OptionB = q.B, OptionC = q.C, OptionD = q.D,
                            CorrectOption = q.Correct
                        });
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        private static IEnumerable<SeedEvent> GetEvents() => new[]
        {
            new SeedEvent("Java + Programação Orientada a Objetos",
                "Fundamentos de Java e dos pilares da POO: classes, objetos, encapsulamento, herança, polimorfismo, abstração e coleções.",
                new DateTime(2026, 10, 20, 19, 0, 0), 20, 0, new[]
            {
                new Q("Qual pilar da POO consiste em esconder os atributos de uma classe e expô-los apenas por métodos?", "Herança", "Encapsulamento", "Polimorfismo", "Abstração", "B"),
                new Q("Qual palavra-chave Java é usada para uma classe herdar de outra?", "implements", "inherits", "extends", "super", "C"),
                new Q("Qual modificador de acesso torna um membro visível apenas dentro da própria classe?", "public", "protected", "default", "private", "D"),
                new Q("O que é um construtor em Java?", "Um método especial chamado ao criar um objeto, com o mesmo nome da classe", "Um método que destrói objetos", "Uma variável estática", "Um tipo de interface", "A"),
                new Q("Uma classe Java pode implementar quantas interfaces?", "Apenas uma", "Nenhuma", "Várias", "No máximo duas", "C"),
                new Q("Sobrescrever um método da superclasse na subclasse é um exemplo de:", "Encapsulamento", "Polimorfismo", "Sobrecarga de operador", "Composição", "B"),
                new Q("Qual anotação indica que um método sobrescreve outro da superclasse?", "@Override", "@Super", "@Inherit", "@Extends", "A"),
                new Q("Uma classe declarada como 'abstract':", "Pode ser instanciada normalmente", "Não pode ter métodos concretos", "Não pode ser instanciada diretamente", "Não pode ter construtor", "C"),
                new Q("Qual coleção Java NÃO permite elementos duplicados?", "ArrayList", "LinkedList", "HashSet", "Vector", "C"),
                new Q("O que a palavra-chave 'static' indica em um atributo?", "Que ele não pode mudar", "Que pertence à classe e é compartilhado por todas as instâncias", "Que é privado", "Que é coletado pelo garbage collector", "B"),
            }),

            new SeedEvent("C# e .NET + Programação Orientada a Objetos",
                "Desenvolvimento com C# e .NET: tipos, propriedades, interfaces, LINQ, async/await e POO aplicada a projetos reais.",
                new DateTime(2026, 10, 27, 19, 0, 0), 20, 0, new[]
            {
                new Q("Em C#, qual símbolo é usado para indicar herança de uma classe?", "extends", ":", "=>", "::", "B"),
                new Q("O que é uma 'property' (propriedade) em C#?", "Um membro com get/set que encapsula um campo", "Um tipo de namespace", "Um método estático obrigatório", "Uma classe selada", "A"),
                new Q("Qual palavra-chave impede que uma classe seja herdada?", "static", "abstract", "sealed", "readonly", "C"),
                new Q("Para que serve o LINQ?", "Criar interfaces gráficas", "Consultar e manipular coleções e dados com sintaxe integrada", "Gerenciar memória manualmente", "Compilar código C++", "B"),
                new Q("Qual palavra-chave permite que um método seja sobrescrito na classe filha?", "virtual", "sealed", "const", "base", "A"),
                new Q("O que 'async' e 'await' permitem em C#?", "Herança múltipla", "Programação assíncrona sem bloquear a thread", "Criação de structs", "Tipagem dinâmica", "B"),
                new Q("Qual a diferença principal entre 'class' e 'struct' em C#?", "Não há diferença", "class é tipo de referência; struct é tipo de valor", "struct suporta herança e class não", "class não pode ter métodos", "B"),
                new Q("Qual método LINQ filtra elementos de uma coleção por uma condição?", "Select", "OrderBy", "Where", "GroupBy", "C"),
                new Q("Uma interface em C# pode conter:", "Apenas campos privados", "Assinaturas de métodos e propriedades que as classes devem implementar", "Construtores", "Apenas variáveis estáticas", "B"),
                new Q("Qual operador C# retorna o valor da direita se o da esquerda for null?", "??", "?:", "&&", "!=", "A"),
            }),

            new SeedEvent("Python + Programação Orientada a Objetos",
                "Python moderno: sintaxe, estruturas de dados, funções, classes, herança e métodos especiais aplicados à POO.",
                new DateTime(2026, 11, 3, 19, 0, 0), 20, 0, new[]
            {
                new Q("Qual método é o construtor de uma classe em Python?", "__new__", "__init__", "__construct__", "__start__", "B"),
                new Q("O que representa o parâmetro 'self' nos métodos de uma classe?", "A classe pai", "Um parâmetro opcional", "A própria instância do objeto", "Um atributo estático", "C"),
                new Q("Como se declara que a classe Cachorro herda de Animal?", "class Cachorro extends Animal:", "class Cachorro(Animal):", "class Cachorro : Animal", "class Cachorro inherits Animal:", "B"),
                new Q("Qual estrutura de dados Python armazena pares chave-valor?", "list", "tuple", "set", "dict", "D"),
                new Q("Qual destas estruturas é imutável?", "list", "dict", "tuple", "set", "C"),
                new Q("Por convenção, um atributo iniciado com '_' (um underline) indica que:", "É público obrigatório", "É de uso interno (protegido)", "É uma constante", "É um método mágico", "B"),
                new Q("Qual função chama o método da classe pai dentro da classe filha?", "parent()", "base()", "super()", "this()", "C"),
                new Q("O resultado de [x * 2 for x in range(3)] é:", "[0, 2, 4]", "[2, 4, 6]", "[1, 2, 3]", "[0, 1, 2]", "A"),
                new Q("Qual método especial define a representação em texto de um objeto para print()?", "__str__", "__text__", "__print__", "__repr__only", "A"),
                new Q("Qual palavra-chave define uma função em Python?", "function", "func", "def", "lambda only", "C"),
            }),

            new SeedEvent("HTML5, CSS3 e JavaScript",
                "Desenvolvimento web front-end: HTML semântico, layouts com Flexbox e Grid, manipulação do DOM, eventos e JavaScript moderno.",
                new DateTime(2026, 11, 10, 19, 0, 0), 20, 0, new[]
            {
                new Q("Qual tag HTML5 é semanticamente indicada para o conteúdo principal da página?", "<div>", "<main>", "<section>", "<body>", "B"),
                new Q("Qual propriedade CSS ativa o layout Flexbox em um container?", "display: flex;", "position: flex;", "flex: on;", "layout: flex;", "A"),
                new Q("No CSS, qual seletor tem MAIOR especificidade?", "Seletor de tag (p)", "Seletor de classe (.item)", "Seletor de ID (#menu)", "Seletor universal (*)", "C"),
                new Q("Qual método JavaScript seleciona o primeiro elemento que corresponde a um seletor CSS?", "document.getElement()", "document.querySelector()", "document.find()", "document.select()", "B"),
                new Q("Qual a diferença entre 'let' e 'const'?", "Nenhuma", "const não pode ser reatribuída; let pode", "let é global e const é local", "const só aceita números", "B"),
                new Q("Qual método adiciona um ouvinte de evento a um elemento?", "addEventListener()", "onEvent()", "attachClick()", "listen()", "A"),
                new Q("O operador '===' em JavaScript compara:", "Apenas o valor", "Apenas o tipo", "Valor e tipo", "Referência de memória apenas", "C"),
                new Q("Qual atributo torna um campo de formulário obrigatório no HTML5?", "needed", "required", "mandatory", "validate", "B"),
                new Q("Qual unidade CSS é relativa ao tamanho da fonte do elemento raiz (html)?", "px", "em", "rem", "%", "C"),
                new Q("O que uma Promise representa em JavaScript?", "Um laço de repetição", "O resultado futuro de uma operação assíncrona", "Uma variável constante", "Um tipo de seletor CSS", "B"),
            }),

            new SeedEvent("Microsoft SQL Server",
                "Bancos de dados relacionais com SQL Server: modelagem, DDL, DML, consultas com JOIN, agregações, chaves e índices.",
                new DateTime(2026, 11, 17, 19, 0, 0), 20, 0, new[]
            {
                new Q("Qual comando SQL recupera dados de uma tabela?", "GET", "SELECT", "FETCH", "READ", "B"),
                new Q("Qual cláusula filtra linhas em uma consulta SELECT?", "ORDER BY", "GROUP BY", "WHERE", "HAVING ONLY", "C"),
                new Q("O que garante uma PRIMARY KEY?", "Valores únicos e não nulos para identificar cada linha", "Que a coluna aceita nulos", "Ordenação automática", "Relacionamento com outra tabela", "A"),
                new Q("Qual JOIN retorna apenas registros com correspondência nas duas tabelas?", "LEFT JOIN", "RIGHT JOIN", "FULL JOIN", "INNER JOIN", "D"),
                new Q("Uma FOREIGN KEY serve para:", "Criptografar dados", "Garantir integridade referencial entre tabelas", "Acelerar backups", "Criar usuários", "B"),
                new Q("Qual comando DDL cria uma nova tabela?", "CREATE TABLE", "INSERT TABLE", "NEW TABLE", "MAKE TABLE", "A"),
                new Q("Qual função de agregação conta o número de linhas?", "SUM()", "COUNT()", "TOTAL()", "MAX()", "B"),
                new Q("A cláusula HAVING é usada para:", "Filtrar resultados após um GROUP BY", "Ordenar resultados", "Renomear colunas", "Excluir tabelas", "A"),
                new Q("No SQL Server, qual comando retorna apenas as 5 primeiras linhas?", "SELECT LIMIT 5 *", "SELECT TOP 5 *", "SELECT FIRST 5 *", "SELECT * ROWS 5", "B"),
                new Q("Qual o principal objetivo de um índice (INDEX)?", "Aumentar o espaço em disco", "Acelerar a busca de dados", "Impedir exclusões", "Duplicar a tabela", "B"),
            }),

            new SeedEvent("Git e GitHub na Prática",
                "Controle de versão com Git e colaboração no GitHub: commits, branches, merge, pull requests e resolução de conflitos.",
                new DateTime(2026, 11, 24, 19, 0, 0), 20, 0, new[]
            {
                new Q("Qual comando inicializa um novo repositório Git?", "git start", "git init", "git new", "git create", "B"),
                new Q("Qual comando adiciona arquivos à área de preparação (staging)?", "git add", "git commit", "git push", "git stage-all", "A"),
                new Q("O que o comando 'git commit' faz?", "Envia para o GitHub", "Registra um snapshot das alterações no histórico local", "Baixa alterações remotas", "Apaga arquivos", "B"),
                new Q("Qual comando envia os commits locais para o repositório remoto?", "git pull", "git fetch", "git push", "git send", "C"),
                new Q("Qual comando cria uma nova branch e já muda para ela?", "git branch -d nova", "git checkout -b nova", "git merge nova", "git switch --delete nova", "B"),
                new Q("O que é um Pull Request no GitHub?", "Um pedido para revisar e mesclar alterações de uma branch em outra", "Um backup automático", "Um comando para apagar o repositório", "Uma issue fechada", "A"),
                new Q("Qual comando mostra o histórico de commits?", "git history", "git status", "git log", "git show-all", "C"),
                new Q("Para que serve o arquivo .gitignore?", "Listar colaboradores", "Indicar arquivos que o Git deve ignorar", "Configurar o GitHub Pages", "Guardar senhas", "B"),
                new Q("Um conflito de merge acontece quando:", "O repositório está vazio", "Duas branches alteram a mesma parte de um arquivo de formas diferentes", "Não há internet", "O commit não tem mensagem", "B"),
                new Q("Qual comando baixa e integra as alterações do repositório remoto na branch atual?", "git pull", "git clone", "git push", "git init", "A"),
            }),

            new SeedEvent("Inteligência Artificial (IA)",
                "Introdução à Inteligência Artificial: aprendizado de máquina, redes neurais, dados de treino e teste, LLMs e ética em IA.",
                new DateTime(2026, 12, 1, 19, 0, 0), 20, 0, new[]
            {
                new Q("Um classificador de e-mails atinge 99% de acurácia em uma base onde só 1% das mensagens é spam. Qual a conclusão mais adequada?", "O modelo é excelente e já pode ir para produção", "A acurácia sozinha engana: prever sempre 'não é spam' já daria 99%, então é preciso olhar precisão e recall", "A acurácia alta confirma que não existe overfitting", "O modelo precisa ser retreinado com mais camadas ocultas", "B"),
                new Q("Qual é o principal papel do conjunto de validação durante o treinamento de um modelo?", "Aumentar a quantidade de exemplos usados no treino", "Medir o desempenho final e definitivo que será apresentado ao cliente", "Substituir o conjunto de teste para reduzir o custo computacional", "Ajustar hiperparâmetros e detectar overfitting sem tocar no conjunto de teste", "D"),
                new Q("Em uma rede neural, o que a retropropagação (backpropagation) realiza?", "Calcula o gradiente do erro em relação aos pesos para que eles sejam atualizados", "Embaralha os neurônios da camada oculta a cada época", "Converte textos em vetores numéricos antes do treino", "Remove os neurônios menos ativados após o treinamento", "A"),
                new Q("Qual destas técnicas ajuda a reduzir o overfitting em redes neurais?", "Treinar por mais épocas até o erro de treino chegar a zero", "Usar somente os dados de treino mais recentes", "Dropout e regularização (L1/L2)", "Retirar a função de ativação das camadas ocultas", "C"),
                new Q("Por que os modelos baseados em Transformers superaram as RNNs em tarefas de linguagem?", "Porque processam uma palavra por vez, o que garante mais precisão", "Porque dispensam grandes volumes de dados de treino", "Porque o mecanismo de atenção relaciona todas as palavras da sequência em paralelo", "Porque não utilizam embeddings", "C"),
                new Q("O que é um 'embedding' em IA?", "Uma representação vetorial numérica em que itens semelhantes ficam próximos no espaço", "Uma camada que remove ruído de imagens", "Um algoritmo de compressão de arquivos de modelo", "Um método de rotulagem manual de dados", "A"),
                new Q("Um modelo de crédito nega mais empréstimos a certo grupo porque o histórico usado no treino refletia decisões enviesadas. Isso ilustra:", "Overfitting", "Alucinação do modelo", "Vazamento de dados (data leakage)", "Viés algorítmico herdado dos dados de treino", "D"),
                new Q("O que é 'data leakage' (vazamento de dados) em Machine Learning?", "Perda de dados por falha no armazenamento", "Informação do conjunto de teste ou do futuro chegando ao treino e inflando o desempenho de forma artificial", "O modelo expondo dados pessoais em suas respostas", "Falta de valores em colunas do dataset", "B"),
                new Q("Em um LLM, aumentar o parâmetro 'temperatura' tende a:", "Tornar as respostas mais determinísticas e repetíveis", "Aumentar o limite de tokens de entrada", "Deixar a geração mais variada e criativa, com maior risco de imprecisão", "Acelerar o treinamento do modelo", "C"),
                new Q("Qual abordagem reduz alucinações ao entregar ao LLM trechos de documentos confiáveis no momento da consulta?", "RAG (Retrieval-Augmented Generation)", "Aumentar a temperatura de geração", "Quantização do modelo", "Treinar com aprendizado por reforço sem dados", "A"),
            }),
        };
    }
}
