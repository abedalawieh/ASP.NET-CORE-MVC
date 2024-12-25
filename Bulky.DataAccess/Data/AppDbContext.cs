
using Bulky.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Bulky.DataAccess.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<ShopCart> ShopCarts { get; set; }
        public DbSet<ShopCartItem> ShopCartItems { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, DisplayOrder = 1, Name = "Análise de Sistemas e Design" },
                new Category() { Id = 2, DisplayOrder = 2, Name = "Lógica, Linguagem Política e Ciências Sociais" },
                new Category() { Id = 3, DisplayOrder = 3, Name = "Probabilidade e Estatística" },
                new Category() { Id = 4, DisplayOrder = 3, Name = "Política, Literatura e Ficção" },
                new Category() { Id = 5, DisplayOrder = 3, Name = "Ficção Literária" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Domain-Driven Design: Atacando as complexidades no coração do software",
                    Author = "Eric Evans",
                    Description = "\n\r\rA comunidade de desenvolvimento de softwares reconhece que a modelagem de domínios é fundamental para o design de softwares. Através de modelos de domínios, os desenvolvedores de software conseguem expressar valiosas funcionalidades e traduzi-las em uma implementação de software que realmente atenda às necessidades de seus usuários. Mas, apesar de sua óbvia importância, existem poucos recursos práticos que explicam como incorporar uma modelagem de domínios eficiente no processo de desenvolvimento de softwares.O Domain-Driven Design atende essa necessidade. Este não é um livro sobre tecnologias específicas. Ele oferece aos leitores uma abordagem sistemática com relação ao domain-driven design, ou DDD, apresentando um conjunto abrangente de práticas ideais de design, técnicas baseadas em experiências e princípios fundamentais que facilitam o desenvolvimento de projetos de software que enfrentam domínios complexos. Reunindo práticas de design e implementação, este livro incorpora vários exemplos baseados em projetos que ilustram a aplicação do design dirigido por domínios no desenvolvimento de softwares na vida real. Com este livro em mãos, desenvolvedores orientados a objetos, analistas de sistema e designers terão a orientação de que precisam para organizar e concentrar seu trabalho, criar modelos de domínio valiosos e úteis, e transformar esses modelos em implementações de software duradouras e de alta qualidade.",
                    ISBN13 = "978-8550800653",
                    PriceList = 121.08m,
                    PriceStandart = 100.90m,
                    Price50More = 85.76m,
                    Price100More = 72.90m,
                    CategoryId = 1,
                },
                new Product
                {
                    Id = 2,
                    Title = "Designing Data-Intensive Applications: The Big Ideas Behind Reliable, Scalable, and Maintainable Systems",
                    Author = " Martin Kleppmann",
                    Description = "\n\r\rWant to know how the best software engineers and architects structure their applications to make them scalable, reliable, and maintainable in the long term? This book examines the key principles, algorithms, and trade-offs of data systems, using the internals of various popular software packages and frameworks as examples.\r\n\r\nTools at your disposal are evolving and demands on applications are increasing, but the principles behind them remain the same. You’ll learn how to determine what kind of tool is appropriate for which purpose, and how certain tools can be combined to form the foundation of a good application architecture. You’ll learn how to develop an intuition for what your systems are doing, so that you’re better able to track down any problems that arise.",
                    ISBN13 = "978-1449373320",
                    PriceList = 390.79m,
                    PriceStandart = 325.90m,
                    Price50More = 276.25m,
                    Price100More = 234.81m,
                    CategoryId = 1,
                },
                new Product
                {
                    Id = 3,
                    Title = "Código Limpo: Habilidades práticas do Agile software",
                    Author = "Robert C. Martin",
                    Description = "\n\r\rMesmo um código ruim pode funcionar. Mas se ele não for limpo, pode acabar com uma empresa de desenvolvimento. Perdem-se a cada ano horas incontáveis e recursos importantes devido a um código mal escrito. Mas não precisa ser assim.O renomado especialista em software, Robert C. Martin, apresenta um paradigma revolucionário com Código limpo: Habilidades Práticas do Agile Software. Martin se reuniu com seus colegas do Mentor Object para destilar suas melhores e mais ágeis práticas de limpar códigos “dinamicamente” em um livro que introduzirá gradualmente dentro de você os valores da habilidade de um profissional de softwares e lhe tornar um programador melhor –mas só se você praticar.Que tipo de trabalho você fará? Você lerá códigos aqui, muitos códigos. E você deverá descobrir o que está correto e errado nos códigos. E, o mais importante, você terá de reavaliar seus valores profissionais e seu comprometimento com o seu ofício.Código limpo está divido em três partes. Na primeira há diversos capítulos que descrevem os princípios, padrões e práticas para criar um código limpo.A segunda parte consiste em diversos casos de estudo de complexidade cada vez maior. Cada um é um exercício para limpar um código – transformar o código base que possui alguns problemas em um melhor e eficiente. A terceira parte é a compensação: um único capítulo com uma lista de heurísticas e “odores” reunidos durante a criação dos estudos de caso. O resultado será um conhecimento base que descreve a forma como pensamos quando criamos, lemos e limpamos um código.Após ler este livro os leitores saberão:✔ Como distinguir um código bom de um ruim✔ Como escrever códigos bons e como transformar um ruim em um bom✔ Como criar bons nomes, boas funções, bons objetos e boas classes✔ Como formatar o código para ter uma legibilidade máxima✔ Como implementar completamente o tratamento de erro sem obscurecer a lógica✔ Como aplicar testes de unidade e praticar o desenvolvimento dirigido a testesEste livro é essencial para qualquer desenvolvedor, engenheiro de software, gerente de projeto, líder de equipes ou analistas de sistemas com interesse em construir códigos melhores.",
                    ISBN13 = "978-8576082675",
                    PriceList = 87.48m,
                    PriceStandart = 72.90m,
                    Price50More = 61.96m,
                    Price100More = 52.67m,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 4,
                    Title = "O Codificador Limpo: Um código de conduta para programadores profissionais",
                    Author = "Robert C. Martin",
                    Description = "\n\r\rEntão você quer ser um profissional do desenvolvimento de softwares. Quer erguer a cabeça e declarar para o mundo: “Eu sou um profissional!”.\r\n\r\nQuer que as pessoas olhem para você com respeito e o tratem com consideração.\r\n\r\nVocê quer isso tudo. Certo?\r\n\r\nO termo “Profissionalismo” é, sem dúvida, um distintivo de honra e orgulho, mas também é um marcador de incumbência e responsabilidade, que inclui trabalhar bem e honestamente.\r\n\r\nVerdadeiros profissionais praticam e trabalham firme para manter suas habilidades afiadas e prontas. Não é o bastante simplesmente fazer suas tarefas diárias e chamar isso de prática. Realizar seu trabalho diário é performance, e não prática. Prática é quando você especificamente exercita as habilidades fora do seu ambiente de trabalho com o único propósito de potencializá-las.\r\n\r\nO Codificador Limpo contém muitos conselhos pragmáticos que visam transformar o comportamento do profissional de software. O autor transmite valiosos ensinamentos sobre ética, respeito, responsabilidade, sinceridade e comprometimento, através de sua experiência como programador.",
                    ISBN13 = "978-8576086475",
                    PriceList = 75.42m,
                    PriceStandart = 62.85m,
                    Price50More = 53.42m,
                    Price100More = 45.40m,
                    CategoryId = 1,
                },
                new Product
                {
                    Id = 5,
                    Title = "O Mito De Sísifo",
                    Author = "Albert Camus",
                    Description = "\n\r\rDe um dos mais importantes e representativos autores do século XX e Prêmio Nobel de Literatura, O mito de sísifo traz ensaios sobre o absurdo e o irracional, tornando-se uma importante contribuição filosófico-existencial que exerce influência profunda sobre toda uma geração.\r\n\r\n \r\n\r\nAlbert Camus, um dos escritores e intelectuais mais influentes do século XX, publicou O mito de Sísifo em 1942. Este ensaio sobre o absurdo tornou-se uma importante contribuição filosófico-existencial e exerceu profunda influência sobre toda uma geração. Camus destaca o mundo imerso em irracionalidades e lembra Sísifo, condenado pelos deuses a empurrar incessantemente uma pedra até o alto da montanha, de onde ela tornava a cair, caracterizando seu trabalho como inútil e sem esperança.\r\n\r\nO autor faz um retrato do mundo em que vivemos e do dilema enfrentado pelo homem contemporâneo: “Ou não somos livres e o responsável pelo mal é Deus todo-poderoso, ou somos livres e responsáveis, mas Deus não é todo-poderoso.” Quando Camus publicou O mito de Sísifo, em 1942, em plena Segunda Guerra Mundial, o mundo parecia mesmo absurdo. A guerra, a ocupação da França, o triunfo aparente da violência e da injustiça, tudo se opunha de forma brutal e desmentida à ideia do universo racional. Os deuses que condenaram Sísifo a empurrar incessantemente uma pedra até o alto da montanha, de ela tornava a cair, caracterizaram um trabalho inútil e sem esperança que podia exprimir a situação contemporânea.\r\n\r\nCamus diz em O mito de Sísifo que “sempre houve homens para defender os direitos do irracional”. A época atual vê renascer sistemas paradoxais que se empenham em fazer a razão tropeçar. O terrorismo individual sucede o terrorismo de Estado, e vice-versa.\r\n\r\n“Em O mito de Sísifo, Camus formulou ideias sobre a gratuidade da existência, o confronto entre a opacidade das coisas e nosso ‘apetite de clareza’, sobre o ‘divórcio entre o homem e sua vida, entre o ator e seu cenário’.” – Manuel da Costa Pinto.",
                    ISBN13 = "978-8501111647",
                    PriceList = 51.51m,
                    PriceStandart = 42.92m,
                    Price50More = 36.48m,
                    Price100More = 31.00m,
                    CategoryId = 2
                },
                new Product
                {
                    Id = 6,
                    Title = "O Andar Do Bêbado: Como o acaso determina nossas vidas",
                    Author = "Leonard Mlodinow",
                    Description = "\n\r\rBest-seller nacional e internacional, com cerca de 180 mil exemplares vendidos no Brasil! Esta edição comemorativa celebra os 10 anos de lançamento do best-seller O andar do bêbado, do físico e matemático Leonard Mlodinow. Não estamos preparados para lidar com o aleatório e por isso não percebemos como o acaso interfere em nossas vidas. Nesse livro notável, Mlodinow combina os mais diferentes exemplos para mostrar que as notas escolares, diagnósticos médicos, sucesso de bilheteria e resultados eleitorais são, como muitas outras coisas, determinados por eventos imprevisíveis. Este livro instigante pões em xeque tudo que acreditamos saber sobre como o mundo funciona. E, assim, nos ajuda a fazer escolhas mais acertadas e a conviver melhor com os fatores que não podemos controlar. \"Um guia maravilhoso e acessível sobre como o aleatório afeta nossas vidas\" Stephen Hawking \"Mlodinow escreve num estilo leve, intercalando desafios probabilísticos com perfis de cientistas... O resultado é um curso intensivo, de leitura agradável, sobre aleatoriedade e estatística.\" George Johnson, New York Times",
                    ISBN13 = "978-8537818107",
                    PriceList = 49.86m,
                    PriceStandart = 41.55m,
                    Price50More = 35.31m,
                    Price100More = 30.01m,
                    CategoryId = 3
                },
                new Product
                {
                    Id = 7,
                    Title = "A Revolução Dos Bichos: Um conto de fadas",
                    Author = "George Orwell",
                    Description = "\n\r\rVerdadeiro clássico moderno, concebido por um dos mais influentes escritores do século XX, A revolução dos bichos é uma fábula sobre o poder. Narra a insurreição dos animais de uma granja contra seus donos. Progressivamente, porém, a revolução degenera numa tirania ainda mais opressiva que a dos humanos.\r\n\r\nEscrita em plena Segunda Guerra Mundial e publicada em 1945 depois de ter sido rejeitada por várias editoras, essa pequena narrativa causou desconforto ao satirizar ferozmente a ditadura stalinista numa época em que os soviéticos ainda eram aliados do Ocidente na luta contra o eixo nazifascista.\r\nDe fato, são claras as referências: o despótico Napoleão seria Stálin, o banido Bola-de-Neve seria Trotsky, e os eventos políticos - expurgos, instituição de um estado policial, deturpação tendenciosa da História - mimetizam os que estavam em curso na União Soviética.\r\nCom o acirramento da Guerra Fria, as mesmas razões que causaram constrangimento na época de sua publicação levaram A revolução dos bichos a ser amplamente usada pelo Ocidente nas décadas seguintes como arma ideológica contra o comunismo. O próprio Orwell, adepto do socialismo e inimigo de qualquer forma de manipulação política, sentiu-se incomodado com a utilização de sua fábula como panfleto.\r\nDepois das profundas transformações políticas que mudaram a fisionomia do planeta nas últimas décadas, a pequena obra-prima de Orwell pode ser vista sem o viés ideológico reducionista. Mais de sessenta anos depois de escrita, ela mantém o viço e o brilho de uma alegoria perene sobre as fraquezas humanas que levam à corrosão dos grandes projetos de revolução política. É irônico que o escritor, para fazer esse retrato cruel da humanidade, tenha recorrido aos animais como personagens. De certo modo, a inteligência política que humaniza seus bichos é a mesma que animaliza os homens.\r\nEscrito com perfeito domínio da narrativa, atenção às minúcias e extraordinária capacidade de criação de personagens e situações, A revolução dos bichos combina de maneira feliz duas ricas tradições literárias: a das fábulas morais, que remontam a Esopo, e a da sátira política, que teve talvez em Jonathan Swift seu representante máximo.\r\n\r\n\"A melhor sátira já escrita sobre a face negra da história moderna.\" - Malcolm Bradbury\r\n\r\n\"Um livro para todos os tipos de leitor, seu brilho ainda intacto depois de sessenta anos.\" - Ruth Rendell ",
                    ISBN13 = "978-8535909555",
                    PriceList = 13.20m,
                    PriceStandart = 11.00m,
                    Price50More = 09.35m,
                    Price100More = 07.94m,
                    CategoryId = 4
                },
                new Product
                {
                    Id = 8,
                    Title = "Memórias póstumas de Brás Cubas",
                    Author = "George Orwell",
                    Description = "\n\r\rBrás Cubas está morto. Mas isso não o impede de relatar em seu livro os acontecimentos de sua existência e de sua grande ideia fixa: lançar o Emplasto Brás Cubas. Deus te livre, leitor, de uma ideia fixa. O medicamento anti-hipocondríaco torna-se o estopim de uma série de lembranças, reminiscências e digressões da vida do defunto autor.\r\n\r\nPublicado em 1881, escrito com a pena da galhofa e a tinta da melancolia, Memórias Póstumas de Brás Cubas é, possivelmente, o mais importante romance brasileiro de todos os tempos. Inovador, irônico, rebelde, toca no que há de mais profundo no ser humano. Mas vale avisar: há na alma desse livro, por mais risonho que pareça, um sentimento amargo e áspero.\r\n\r\nA edição da Antofágica conta com 88 ilustrações de um dos expoentes da arte no Brasil, Candido Portinari, que chegam pela primeira vez ao grande público e dão uma nova camada de interpretação ao clássico.\r\n\r\nO livro traz ainda com notas inéditas e posfácio de Rogério Fernandes dos Santos, especialista na obra machadiana, um perfil do autor escrito por Ale Santos (@savagefiction), além de uma introdução de Isabela Lubrano, do canal Ler Antes de Morrer.",
                    ISBN13 = "978-6580210015",
                    PriceList = 86.74m,
                    PriceStandart = 72.29m,
                    Price50More = 61.44m,
                    Price100More = 52.22m,
                    CategoryId = 5
                }
            );

            modelBuilder.Entity<Address>().HasData(
                new Address
                {
                    Id = 1,
                    StreetAddress = "123 Main St",
                    City = "City A",
                    State = "State A",
                    PostalCode = "12345"
                },
                new Address
                {
                    Id = 2,
                    StreetAddress = "456 Elm St",
                    City = "City B",
                    State = "State B",
                    PostalCode = "23456"
                },
                new Address
                {
                    Id = 3,
                    StreetAddress = "789 Oak St",
                    City = "City C",
                    State = "State C",
                    PostalCode = "34567"
                },
                new Address
                {
                    Id = 4,
                    StreetAddress = "101 Pine St",
                    City = "City D",
                    State = "State D",
                    PostalCode = "45678"
                },
                new Address
                {
                    Id = 5,
                    StreetAddress = "202 Cedar St",
                    City = "City E",
                    State = "State E",
                    PostalCode = "56789"
                }
                );

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = 1,
                    Name = "Company A",
                    CNPJ = "12345678901234",
                    AddressId = 1
                },
                new Company
                {
                    Id = 2,
                    Name = "Company B",
                    CNPJ = "56789012345678",
                    AddressId = 2
                },
                new Company
                {
                    Id = 3,
                    Name = "Company C",
                    CNPJ = "90123456789012",
                    AddressId = 3
                },
                new Company
                {
                    Id = 4,
                    Name = "Company D",
                    CNPJ = "23456789012345",
                    AddressId = 4
                },
                new Company
                {
                    Id = 5,
                    Name = "Company E",
                    CNPJ = "67890123456789",
                    AddressId = 5
                }
            );
        }
    }
}
