namespace EventManager.Web.Services
{
    /// <summary>Uma seção do material de estudo: título, explicação e (opcionalmente) um exemplo de código.</summary>
    public record StudySection(string Title, string Text, string? Code = null);

    /// <summary>Conteúdo de estudo de um curso.</summary>
    public record StudyTopic(string Subtitle, string Intro, StudySection[] Sections, string[] KeyPoints);

    /// <summary>
    /// Textos dos materiais em PDF. Cada curso oficial da 7Academy tem seu conteúdo aqui;
    /// o PDF é montado por <see cref="StudyMaterialService"/>.
    /// </summary>
    public static class StudyContent
    {
        public static StudyTopic? ForEvent(string title)
        {
            var t = title.ToLowerInvariant();
            if (t.Contains("java +")) return Java;
            if (t.Contains("c# e .net")) return CSharp;
            if (t.Contains("python")) return Python;
            if (t.Contains("html")) return Web;
            if (t.Contains("sql server")) return Sql;
            if (t.Contains("git")) return Git;
            if (t.Contains("inteligência artificial") || t.Contains("inteligencia artificial")) return AI;
            return null;
        }

        // ------------------------------------------------------------------ JAVA
        private static readonly StudyTopic Java = new(
            "Classes, Herança, Polimorfismo, Encapsulamento e Coleções",
            "Java é uma linguagem orientada a objetos, fortemente tipada e compilada para bytecode, executado pela JVM. Neste material você revê os pilares da Programação Orientada a Objetos (POO) e as coleções mais usadas no dia a dia.",
            new[]
            {
                new StudySection("1. Classes e Objetos",
                    "Uma classe é o molde que define atributos (dados) e métodos (comportamentos). Um objeto é uma instância da classe, criada com a palavra-chave new. O construtor é um método especial, com o mesmo nome da classe, chamado na criação do objeto.\n\nMembros declarados com static pertencem à classe e são compartilhados por todas as instâncias.",
"""
public class Pessoa {
    private String nome;
    private int idade;

    public Pessoa(String nome, int idade) {   // construtor
        this.nome = nome;
        this.idade = idade;
    }

    public String apresentar() {
        return "Olá, eu sou " + nome + " e tenho " + idade + " anos.";
    }
}

Pessoa p = new Pessoa("Ana", 20);
System.out.println(p.apresentar());
"""),
                new StudySection("2. Encapsulamento",
                    "Encapsular é esconder os atributos de uma classe (private) e expô-los apenas por métodos públicos (getters e setters), permitindo validar os dados antes de alterá-los. Os modificadores de acesso são: private (só a própria classe), default (mesmo pacote), protected (pacote e subclasses) e public (todos).",
"""
public class ContaBancaria {
    private double saldo;

    public double getSaldo() { return saldo; }

    public void depositar(double valor) {
        if (valor > 0) {          // regra de validação protegida
            saldo += valor;
        }
    }
}
"""),
                new StudySection("3. Herança",
                    "A herança permite que uma classe (subclasse) reaproveite atributos e métodos de outra (superclasse) usando a palavra-chave extends. Java permite herdar de apenas uma classe, mas uma classe pode implementar várias interfaces. O construtor da superclasse é chamado com super(...). A anotação @Override indica que um método da superclasse está sendo reescrito.",
"""
public class Animal {
    public String som() { return "Som genérico"; }
}

public class Cachorro extends Animal {
    @Override
    public String som() { return "Au au!"; }
}
"""),
                new StudySection("4. Polimorfismo, Classes Abstratas e Interfaces",
                    "Polimorfismo é a capacidade de objetos diferentes responderem à mesma chamada de método de formas distintas. Uma variável do tipo da superclasse pode referenciar objetos das subclasses, e o método executado é o da classe real do objeto.\n\nUma classe abstract não pode ser instanciada e pode ter métodos sem corpo. Uma interface define um contrato de métodos que as classes implementam com implements.",
"""
Animal[] bichos = { new Cachorro(), new Gato(), new Animal() };
for (Animal a : bichos) {
    System.out.println(a.som());   // cada objeto responde do seu jeito
}

public interface Pagavel {
    double calcularValor();
}
public class Boleto implements Pagavel {
    public double calcularValor() { return 150.0; }
}
"""),
                new StudySection("5. Coleções",
                    "O Java Collections Framework oferece estruturas prontas. List mantém ordem e aceita duplicados (ArrayList, LinkedList). Set não aceita elementos duplicados (HashSet). Map guarda pares chave-valor (HashMap).",
"""
List<String> nomes = new ArrayList<>();
nomes.add("Ana"); nomes.add("Bia"); nomes.add("Ana");   // 3 itens

Set<String> unicos = new HashSet<>(nomes);              // 2 itens

Map<String, Integer> notas = new HashMap<>();
notas.put("Ana", 10);
int notaAna = notas.get("Ana");
"""),
            },
            new[]
            {
                "Encapsulamento: atributos private e acesso por métodos.",
                "Herança: extends (apenas uma superclasse); interfaces: implements (várias).",
                "Polimorfismo: sobrescrita (@Override) e referência da superclasse para objeto da subclasse.",
                "Classe abstract não é instanciada; static pertence à classe.",
                "HashSet não aceita duplicados; HashMap guarda chave-valor.",
            });

        // ------------------------------------------------------------------ C#
        private static readonly StudyTopic CSharp = new(
            "LINQ, Propriedades, Async/Await e Interfaces",
            "C# é uma linguagem moderna, fortemente tipada, da plataforma .NET. Este material revisa propriedades, herança, interfaces, consultas LINQ e programação assíncrona.",
            new[]
            {
                new StudySection("1. Classes e Propriedades",
                    "Propriedades são membros com get/set que encapsulam um campo. As propriedades automáticas geram o campo por trás dos panos. Em C#, a herança usa dois-pontos (:) e uma classe pode ser selada (sealed) para impedir que seja herdada.\n\nClasses são tipos de referência; structs são tipos de valor.",
"""
public class Aluno
{
    public string Nome { get; set; } = "";
    public int Idade { get; private set; }     // set restrito

    public Aluno(string nome, int idade)
    {
        Nome = nome;
        Idade = idade;
    }
}
"""),
                new StudySection("2. Herança e Polimorfismo",
                    "O método da classe base deve ser marcado como virtual para poder ser sobrescrito com override na classe filha. base acessa membros da classe pai. Classes abstract não podem ser instanciadas.",
"""
public class Animal
{
    public virtual string Som() => "Som genérico";
}

public class Gato : Animal
{
    public override string Som() => "Miau!";
}

Animal a = new Gato();
Console.WriteLine(a.Som());   // Miau!
"""),
                new StudySection("3. Interfaces",
                    "Uma interface define um contrato: assinaturas de métodos e propriedades que as classes implementadas devem fornecer. Uma classe pode implementar várias interfaces, o que favorece baixo acoplamento e testes. Por convenção, os nomes começam com I.",
"""
public interface IPagamento
{
    decimal Calcular(decimal valor);
}

public class Pix : IPagamento
{
    public decimal Calcular(decimal valor) => valor;            // sem taxa
}
public class Cartao : IPagamento
{
    public decimal Calcular(decimal valor) => valor * 1.03m;    // 3% de taxa
}
"""),
                new StudySection("4. LINQ",
                    "LINQ (Language Integrated Query) consulta e transforma coleções com uma sintaxe integrada à linguagem. Os métodos mais usados: Where (filtra), Select (projeta/transforma), OrderBy (ordena), GroupBy (agrupa), Any, Count, First/FirstOrDefault e Sum/Average. As consultas são de execução adiada: só rodam quando o resultado é percorrido (ToList, foreach).",
"""
var alunos = new List<Aluno> { /* ... */ };

var aprovados = alunos
    .Where(a => a.Nota >= 7)
    .OrderBy(a => a.Nome)
    .Select(a => a.Nome)
    .ToList();

double media = alunos.Average(a => a.Nota);
"""),
                new StudySection("5. Async / Await",
                    "A programação assíncrona libera a thread enquanto se espera uma operação lenta (rede, banco de dados, arquivos). Um método async retorna Task ou Task<T>; dentro dele, await aguarda o resultado sem bloquear a thread. Não confunda com paralelismo: async/await trata de não bloquear, não de executar em várias threads.",
"""
public async Task<string> BuscarAsync(string url)
{
    using var http = new HttpClient();
    string conteudo = await http.GetStringAsync(url);   // não bloqueia
    return conteudo;
}

var texto = await BuscarAsync("https://exemplo.com");
"""),
                new StudySection("6. Operadores úteis",
                    "?? retorna o valor da direita quando o da esquerda é null (nome ?? \"Anônimo\"). ?. acessa um membro apenas se o objeto não for nulo (pessoa?.Nome). Interpolação de strings: $\"Olá, {nome}\"."),
            },
            new[]
            {
                "Propriedades: get/set encapsulam campos; sealed impede herança.",
                "virtual + override para polimorfismo; interface define contrato.",
                "LINQ: Where filtra, Select transforma, OrderBy ordena.",
                "async/await: libera a thread sem bloquear; retorna Task.",
                "class = tipo de referência; struct = tipo de valor.",
            });

        // ------------------------------------------------------------------ PYTHON
        private static readonly StudyTopic Python = new(
            "Dicionários, __init__, Herança e List Comprehension",
            "Python é uma linguagem de sintaxe simples e tipagem dinâmica. A indentação define os blocos de código. Aqui você revê classes, herança, estruturas de dados e compreensões de lista.",
            new[]
            {
                new StudySection("1. Classes, __init__ e self",
                    "Classes são definidas com class. O método __init__ é o inicializador (chamado ao criar o objeto) e o parâmetro self representa a própria instância. Atributos criados em self.atributo pertencem ao objeto. Por convenção, um nome iniciado com _ indica uso interno.",
"""
class Pessoa:
    def __init__(self, nome, idade):
        self.nome = nome
        self.idade = idade

    def apresentar(self):
        return f"Olá, eu sou {self.nome} e tenho {self.idade} anos."

p = Pessoa("Ana", 20)
print(p.apresentar())
"""),
                new StudySection("2. Herança e super()",
                    "A subclasse recebe a superclasse entre parênteses. super() chama métodos da classe pai (como o __init__). Python permite herança múltipla. Reescrever um método na subclasse é a base do polimorfismo.",
"""
class Animal:
    def __init__(self, nome):
        self.nome = nome
    def som(self):
        return "..."

class Cachorro(Animal):
    def __init__(self, nome, raca):
        super().__init__(nome)
        self.raca = raca
    def som(self):
        return "Au au!"
"""),
                new StudySection("3. Métodos especiais (mágicos)",
                    "Métodos cercados por dois underlines definem comportamentos especiais: __str__ controla o texto exibido por print(); __repr__ é a representação técnica; __len__, __eq__ e __add__ permitem usar len(), == e + em seus objetos.",
"""
class Produto:
    def __init__(self, nome, preco):
        self.nome, self.preco = nome, preco
    def __str__(self):
        return f"{self.nome} - R$ {self.preco:.2f}"

print(Produto("Caneta", 3.5))   # Caneta - R$ 3.50
"""),
                new StudySection("4. Listas, Tuplas, Conjuntos e Dicionários",
                    "list é mutável e ordenada. tuple é imutável. set não aceita duplicados. dict (dicionário) guarda pares chave-valor, com chaves únicas e acesso rápido por chave.",
"""
nomes = ["Ana", "Bia"]            # list
ponto = (10, 20)                  # tuple (imutável)
unicos = {1, 2, 2, 3}             # set -> {1, 2, 3}

notas = {"Ana": 10, "Bia": 8}     # dict
notas["Caio"] = 7
for nome, nota in notas.items():
    print(nome, nota)
print(notas.get("Zé", 0))         # 0 (valor padrão)
"""),
                new StudySection("5. List Comprehension",
                    "É uma forma compacta de criar listas a partir de outra sequência, com filtro opcional. Também existem compreensões de dicionário e de conjunto.",
"""
quadrados = [x * x for x in range(5)]         # [0, 1, 4, 9, 16]
pares = [x for x in range(10) if x % 2 == 0]  # [0, 2, 4, 6, 8]
tamanhos = {nome: len(nome) for nome in ["Ana", "Bia"]}
"""),
            },
            new[]
            {
                "__init__ inicializa o objeto; self é a instância atual.",
                "Herança: class Filha(Pai); super() chama o método da classe pai.",
                "dict = chave-valor; tuple é imutável; set não repete elementos.",
                "[x for x in lista if condição] cria listas de forma compacta.",
                "__str__ define o texto usado por print().",
            });

        // ------------------------------------------------------------------ WEB
        private static readonly StudyTopic Web = new(
            "DOM, Eventos, Flexbox, Promises e Tags Semânticas",
            "HTML estrutura o conteúdo, CSS cuida da aparência e JavaScript dá comportamento à página. Juntos formam a base do desenvolvimento front-end.",
            new[]
            {
                new StudySection("1. HTML5 Semântico",
                    "Tags semânticas descrevem o significado do conteúdo, ajudando acessibilidade e SEO: header, nav, main, section, article, aside e footer. Evite usar div para tudo. Formulários usam atributos como required, type=\"email\" e placeholder para validação nativa.",
"""
<header><h1>7Academy</h1></header>
<nav><a href="/">Início</a></nav>
<main>
  <article>
    <h2>Meu artigo</h2>
    <p>Conteúdo...</p>
  </article>
</main>
<footer>© 2026</footer>
"""),
                new StudySection("2. CSS: seletores e especificidade",
                    "Seletores: tag (p), classe (.card), id (#menu). Especificidade, do menor para o maior: tag < classe < id < estilo inline. Unidades: px (fixa), em (relativa à fonte do elemento), rem (relativa à fonte do html) e % (relativa ao pai).",
"""
p { color: gray; }
.destaque { color: gold; }
#titulo { color: purple; }     /* o id vence a classe e a tag */
"""),
                new StudySection("3. Flexbox",
                    "Flexbox organiza itens em uma dimensão (linha ou coluna). No container: display: flex; flex-direction; justify-content (alinha no eixo principal); align-items (alinha no eixo transversal); gap (espaço entre itens); flex-wrap. CSS Grid trabalha em duas dimensões (linhas e colunas).",
"""
.menu {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 16px;
}
"""),
                new StudySection("4. JavaScript e o DOM",
                    "O DOM (Document Object Model) é a representação da página como árvore de objetos. Com document.querySelector seleciona-se um elemento e com addEventListener reage-se a eventos (click, input, submit). Use let para variáveis que mudam, const para as que não são reatribuídas. O operador === compara valor e tipo.",
"""
const botao = document.querySelector("#salvar");
const msg = document.querySelector("#mensagem");

botao.addEventListener("click", () => {
  msg.textContent = "Salvo com sucesso!";
  msg.classList.add("ok");
});
"""),
                new StudySection("5. Promises e async/await",
                    "Uma Promise representa o resultado futuro de uma operação assíncrona e tem três estados: pending, fulfilled e rejected. Use then/catch ou, de forma mais legível, async/await com try/catch. A função fetch retorna uma Promise.",
"""
async function carregarEventos() {
  try {
    const resposta = await fetch("/api/eventos");
    const dados = await resposta.json();
    console.log(dados);
  } catch (erro) {
    console.error("Falhou:", erro);
  }
}
"""),
            },
            new[]
            {
                "Semântica: header, nav, main, section, article, footer.",
                "Especificidade CSS: tag < classe < id.",
                "Flexbox: justify-content (eixo principal) e align-items (eixo transversal).",
                "querySelector seleciona; addEventListener escuta eventos.",
                "Promise: pending, fulfilled, rejected; async/await simplifica o uso.",
            });

        // ------------------------------------------------------------------ SQL
        private static readonly StudyTopic Sql = new(
            "SELECT, JOINs, GROUP BY, Chaves Primárias e Foreign Keys",
            "SQL (Structured Query Language) é a linguagem dos bancos de dados relacionais. O Microsoft SQL Server usa a variante T-SQL. Aqui você revê criação de tabelas, consultas, junções e agregações.",
            new[]
            {
                new StudySection("1. Criando tabelas e chaves (DDL)",
                    "A PRIMARY KEY identifica cada linha de forma única e não aceita nulos. A FOREIGN KEY liga uma coluna à chave primária de outra tabela, garantindo integridade referencial (não existe aluno em curso inexistente). IDENTITY gera números automáticos.",
"""
CREATE TABLE Cursos (
    Id    INT IDENTITY(1,1) PRIMARY KEY,
    Nome  NVARCHAR(100) NOT NULL
);

CREATE TABLE Alunos (
    Id       INT IDENTITY(1,1) PRIMARY KEY,
    Nome     NVARCHAR(100) NOT NULL,
    CursoId  INT NOT NULL,
    CONSTRAINT FK_Alunos_Cursos FOREIGN KEY (CursoId) REFERENCES Cursos(Id)
);
"""),
                new StudySection("2. Manipulando dados (DML)",
                    "INSERT adiciona linhas, UPDATE altera e DELETE remove. Sempre use WHERE em UPDATE e DELETE; sem ele, todas as linhas serão afetadas.",
"""
INSERT INTO Cursos (Nome) VALUES (N'Java');
UPDATE Alunos SET Nome = N'Ana Julia' WHERE Id = 1;
DELETE FROM Alunos WHERE Id = 5;
"""),
                new StudySection("3. SELECT, WHERE e ORDER BY",
                    "SELECT escolhe as colunas, FROM indica a tabela, WHERE filtra linhas, ORDER BY ordena (ASC/DESC) e TOP limita a quantidade de linhas no SQL Server (no lugar do LIMIT de outros bancos). LIKE faz busca por padrão, com % como curinga.",
"""
SELECT TOP 5 Nome
FROM Alunos
WHERE Nome LIKE 'A%'
ORDER BY Nome ASC;
"""),
                new StudySection("4. JOINs",
                    "INNER JOIN retorna apenas as linhas com correspondência nas duas tabelas. LEFT JOIN retorna todas as linhas da tabela da esquerda, com NULL quando não há correspondência. RIGHT JOIN faz o inverso e FULL JOIN traz tudo de ambos os lados.",
"""
SELECT a.Nome AS Aluno, c.Nome AS Curso
FROM Alunos a
INNER JOIN Cursos c ON c.Id = a.CursoId;

-- cursos mesmo sem alunos
SELECT c.Nome, a.Nome
FROM Cursos c
LEFT JOIN Alunos a ON a.CursoId = c.Id;
"""),
                new StudySection("5. GROUP BY, HAVING e funções de agregação",
                    "Funções de agregação: COUNT, SUM, AVG, MIN e MAX. GROUP BY agrupa linhas para agregar por grupo. WHERE filtra linhas antes do agrupamento; HAVING filtra os grupos depois do agrupamento.",
"""
SELECT c.Nome, COUNT(*) AS TotalAlunos
FROM Alunos a
INNER JOIN Cursos c ON c.Id = a.CursoId
GROUP BY c.Nome
HAVING COUNT(*) > 10;
"""),
                new StudySection("6. Índices",
                    "Um índice (INDEX) acelera a busca em colunas muito consultadas, como um índice de livro, mas ocupa espaço e deixa INSERT/UPDATE um pouco mais lentos.",
"""
CREATE INDEX IX_Alunos_Nome ON Alunos (Nome);
"""),
            },
            new[]
            {
                "PRIMARY KEY: único e não nulo; FOREIGN KEY: integridade referencial.",
                "INNER JOIN: só correspondências; LEFT JOIN: tudo da esquerda.",
                "WHERE filtra linhas; HAVING filtra grupos do GROUP BY.",
                "TOP n limita linhas no SQL Server.",
                "Índices aceleram consultas, com custo em espaço e escrita.",
            });

        // ------------------------------------------------------------------ GIT
        private static readonly StudyTopic Git = new(
            "Commit, Push, Pull, Branch, Merge e Git Flow",
            "Git é um sistema de controle de versão distribuído: guarda o histórico do projeto e permite trabalhar em equipe sem sobrescrever o trabalho dos outros. GitHub é a plataforma na nuvem que hospeda repositórios Git.",
            new[]
            {
                new StudySection("1. O fluxo básico",
                    "Seus arquivos passam por três áreas: o diretório de trabalho (onde você edita), a área de preparação ou staging (git add) e o repositório local (git commit). Depois os commits são enviados ao repositório remoto com git push.",
"""
git init                       # cria um repositório
git status                     # mostra o que mudou
git add .                      # prepara as alterações (staging)
git commit -m "feat: tela de login"   # registra um snapshot
git log --oneline              # histórico de commits
"""),
                new StudySection("2. Repositório remoto: clone, push e pull",
                    "git clone copia um repositório remoto. git push envia seus commits locais ao remoto. git pull busca as novidades do remoto e as integra na sua branch (é um fetch seguido de merge). git fetch apenas baixa, sem integrar.",
"""
git clone https://github.com/usuario/projeto.git
git push origin main
git pull origin main
"""),
                new StudySection("3. Branches e Merge",
                    "Uma branch é uma linha independente de desenvolvimento. Criar uma branch por funcionalidade mantém a main estável. git merge integra o trabalho de uma branch em outra. Se duas branches alteram a mesma parte de um arquivo, ocorre um conflito de merge, que deve ser resolvido manualmente, depois git add e novo commit.",
"""
git branch                        # lista as branches
git checkout -b feature/quiz      # cria e entra na branch
git add . && git commit -m "feat: prova"
git checkout main
git merge feature/quiz            # traz as alterações para a main
git branch -d feature/quiz        # apaga a branch já integrada
"""),
                new StudySection("4. Pull Request",
                    "Pull Request (PR) é um pedido, feito no GitHub, para revisar e mesclar as alterações de uma branch em outra. É o momento de revisão de código, comentários e testes automáticos antes do merge."),
                new StudySection("5. Git Flow",
                    "O Git Flow é um modelo de organização de branches: main guarda as versões estáveis em produção; develop reúne o desenvolvimento em andamento; feature/* para novas funcionalidades (saem e voltam para a develop); release/* para preparar uma versão; hotfix/* para correções urgentes direto na main."),
                new StudySection("6. Boas práticas e .gitignore",
                    "Faça commits pequenos, com mensagens claras (por exemplo: feat:, fix:, docs:). O arquivo .gitignore lista o que o Git deve ignorar, como pastas bin/ e obj/, senhas e arquivos temporários. git restore desfaz alterações não commitadas e git revert cria um commit que desfaz outro."),
            },
            new[]
            {
                "add → commit (local) → push (remoto).",
                "pull = fetch + merge.",
                "Branch isola o trabalho; merge o integra; conflitos são resolvidos manualmente.",
                "Pull Request: revisão antes de mesclar.",
                "Git Flow: main, develop, feature, release e hotfix.",
            });

        // ------------------------------------------------------------------ IA
        private static readonly StudyTopic AI = new(
            "Conceitos de ML, Redes Neurais, Treinamento/Teste e LLMs",
            "Inteligência Artificial é a área que cria sistemas capazes de executar tarefas que normalmente exigem inteligência humana. O Aprendizado de Máquina (Machine Learning) é o ramo em que o sistema aprende padrões a partir de dados, em vez de seguir regras escritas à mão.",
            new[]
            {
                new StudySection("1. Tipos de aprendizado",
                    "Supervisionado: treina com dados rotulados (entrada + resposta correta), como classificar e-mails em spam ou não spam (classificação) ou prever o preço de um imóvel (regressão). Não supervisionado: encontra estruturas em dados sem rótulo, como agrupar clientes parecidos (clustering). Por reforço: um agente aprende por tentativa e erro, recebendo recompensas e punições do ambiente."),
                new StudySection("2. Treinamento, validação e teste",
                    "Os dados são divididos em três conjuntos: treino (o modelo aprende), validação (ajuste de hiperparâmetros e detecção de overfitting) e teste (avaliação final, com dados nunca vistos). Misturar informação do teste no treino é o vazamento de dados (data leakage) e infla o desempenho de forma enganosa.\n\nOverfitting: o modelo decora o treino e vai mal em dados novos (erro de treino baixo, erro de teste alto). Underfitting: o modelo é simples demais e vai mal até no treino. Para reduzir o overfitting: mais dados, regularização (L1/L2), dropout, parada antecipada e modelos mais simples.",
"""
# Exemplo em Python (scikit-learn)
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LogisticRegression

X_treino, X_teste, y_treino, y_teste = train_test_split(X, y, test_size=0.2)
modelo = LogisticRegression().fit(X_treino, y_treino)
print(modelo.score(X_teste, y_teste))   # acurácia nos dados de teste
"""),
                new StudySection("3. Métricas de avaliação",
                    "Acurácia: proporção de acertos totais. Ela engana em bases desbalanceadas (se só 1% dos e-mails é spam, um modelo que sempre responde \"não é spam\" tem 99% de acurácia e é inútil). Por isso usa-se também: precisão (dos que o modelo marcou como positivos, quantos realmente eram) e recall (dos positivos reais, quantos o modelo encontrou). O F1-score combina precisão e recall."),
                new StudySection("4. Redes neurais",
                    "Redes neurais artificiais são inspiradas nos neurônios do cérebro: camadas de unidades que recebem entradas, multiplicam por pesos, somam e aplicam uma função de ativação (ReLU, sigmoide, softmax). Têm camada de entrada, camadas ocultas e camada de saída; com muitas camadas ocultas, fala-se em aprendizado profundo (deep learning).\n\nNo treinamento, calcula-se o erro com uma função de perda; a retropropagação (backpropagation) calcula o gradiente do erro em relação aos pesos e o gradiente descendente os ajusta, repetindo o processo por várias épocas."),
                new StudySection("5. LLMs e Transformers",
                    "LLM (Large Language Model) é um modelo de linguagem de grande porte, treinado com enormes volumes de texto para prever o próximo token. Baseia-se na arquitetura Transformer, cujo mecanismo de atenção relaciona todas as palavras da sequência em paralelo, o que superou as redes recorrentes (RNNs).\n\nConceitos importantes: token (pedaço de texto), embedding (representação vetorial em que itens semelhantes ficam próximos), prompt (instrução enviada ao modelo), temperatura (quanto maior, mais variada e criativa, e menos previsível, é a resposta) e janela de contexto (quantidade máxima de tokens considerados)."),
                new StudySection("6. Alucinações e RAG",
                    "Alucinação é quando o modelo gera uma informação falsa apresentada com confiança, porque ele prevê texto provável, não verifica fatos. Uma técnica para reduzir esse problema é o RAG (Retrieval-Augmented Generation): o sistema busca trechos de documentos confiáveis e os entrega ao modelo junto com a pergunta, para que a resposta se baseie neles."),
                new StudySection("7. Ética e uso responsável",
                    "Modelos aprendem com dados históricos e podem herdar o viés (bias) presente neles, gerando decisões discriminatórias em crédito, contratação ou justiça. Outras preocupações: privacidade e proteção de dados (LGPD), transparência e explicabilidade das decisões, direitos autorais, uso de deepfakes e a necessidade de supervisão humana em decisões críticas."),
            },
            new[]
            {
                "Supervisionado (rótulos), não supervisionado (sem rótulos) e por reforço (recompensas).",
                "Treino, validação e teste; overfitting decora o treino; data leakage infla o resultado.",
                "Acurácia engana em bases desbalanceadas: use precisão, recall e F1.",
                "Backpropagation calcula gradientes; gradiente descendente ajusta pesos.",
                "LLMs usam Transformers e atenção; RAG reduz alucinações; viés vem dos dados.",
            });
    }
}
