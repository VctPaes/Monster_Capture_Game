using System.Text;
using System.Globalization;
using ProjetoFinal.modelos;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;

Console.WriteLine("Bem-vindo ao jogo de captura de monstros!");

const string arquivoUsuarios = "usuarios.json";
List<Usuario> usuarios = CarregarUsuarios();

string nomeCompleto = LerEntrada("\nDigite seu nome de usuário: ");
Usuario usuario = BuscarOuCriarUsuario(nomeCompleto, usuarios, SalvarUsuarios);

while (true)
{
    Console.WriteLine("\nMenu:\n");
    Console.WriteLine("1. Novo Treinador");
    Console.WriteLine("2. Escolher Treinador");
    Console.WriteLine("---------------------");
    Console.WriteLine("3. Gerenciar Usuário");
    Console.WriteLine("0. Sair");

    string opcao = LerEntrada("\nEscolha uma opção: ");

    switch (opcao)
    {
        case "1":
            NovoTreinador(usuario, usuarios, SalvarUsuarios);
            goto MenuEscolhaTreinador;
        case "2":
            MenuEscolhaTreinador:
            EscolherTreinador(usuario, usuarios);
            if (usuario.Treinadores.Count == 0)
            {
                Console.WriteLine("\nVocê não possui treinadores.");
                string resposta = LerEntrada("Deseja criar um novo treinador para começar sua jornada? (S/N):").ToUpperInvariant();
                if (resposta != "S" && resposta != "N")
                {
                    Console.WriteLine("Resposta inválida. Digite 'S' para sim ou 'N' para não.");
                    return;
                }
                if (resposta == "S")
                {
                    NovoTreinador(usuario, usuarios, SalvarUsuarios);
                }
                else
                {
                    Console.WriteLine("Voltando ao menu principal...");;
                }
            }
            break;
        case "3":
            GerenciarUsuario(usuario, usuarios, SalvarUsuarios);
            break;
        case "0":
            SalvarUsuarios(usuarios);
            Console.WriteLine("\nSaindo...");
            return;
        default:
            Console.WriteLine("\nOpção inválida. Tente novamente.");
            break;
    }
}


// Funções
static List<Usuario> CarregarUsuarios()
{
    if (File.Exists(arquivoUsuarios))
    {
        string json = File.ReadAllText(arquivoUsuarios);
        return JsonConvert.DeserializeObject<List<Usuario>>(json) ?? new List<Usuario>();
    }
    return new List<Usuario>();
}

static void SalvarUsuarios(List<Usuario> usuarios)
{
    var settings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };
    string json = JsonConvert.SerializeObject(usuarios, settings);
    File.WriteAllText(arquivoUsuarios, json);
}

static string Normalizar(string texto) =>
    new string(texto.Normalize(NormalizationForm.FormD)
        .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
        .ToArray())
    .ToLowerInvariant()
    .Trim();

static Usuario BuscarOuCriarUsuario(string nomeCompleto, List<Usuario> usuarios, Action<List<Usuario>> salvar)
{
    string nomeNormalizado = Normalizar(nomeCompleto);
    Usuario? usuario = usuarios.FirstOrDefault(u => Normalizar(u.NomeCompleto) == nomeNormalizado);

    if (usuario == null)
    {
        usuario = new Usuario(nomeCompleto);
        usuarios.Add(usuario);
        Console.WriteLine($"\nUsuário {usuario.NomeCompleto} criado!");
        salvar(usuarios);
    }
    else
    {
        Console.WriteLine($"\nBem-vindo de volta, {usuario.NomeCompleto}!");
    }
    return usuario;
}

static string LerEntrada(string mensagem)
{
    string? entrada;
    do
    {
        Console.Write(mensagem);
        entrada = Console.ReadLine()?.Trim();
    } while (string.IsNullOrEmpty(entrada));
    return entrada;
}

static string? LerTeclaComTimeout(string mensagem, int timeoutMs)
{
    Console.Write(mensagem);

    DateTime inicio = DateTime.Now;
    while ((DateTime.Now - inicio).TotalMilliseconds < timeoutMs)
    {
        if (Console.KeyAvailable)
        {
            var tecla = Console.ReadKey(true);
            return tecla.KeyChar.ToString();
        }
        Thread.Sleep(50);
    }

    return null;
}

static void NovoTreinador(Usuario usuario, List<Usuario> usuarios, Action<List<Usuario>> salvar)
{
    string nomeTreinador = LerEntrada("Digite o nome do novo treinador: ");
    string generoTreinador = LerEntrada("Digite o gênero do treinador(a) (M/F): ").ToUpperInvariant();
    if (generoTreinador != "M" && generoTreinador != "F")
    {
        Console.WriteLine("Gênero inválido. Use 'M' para masculino ou 'F' para feminino.");
        return;
    }
    int genero = generoTreinador == "M" ? 1 : 2;
    try
    {
        Treinador treinador = new(nomeTreinador, genero);
        usuario.AdicionarTreinador(treinador);


        string artigoTipo1 = Treinador.ArtigoTipo1(genero);
        string artigoTipo2 = Treinador.ArtigoTipo2(genero);
        Console.WriteLine($"Treinador{artigoTipo1} {treinador.Nome} criad{artigoTipo2} com sucesso!");

        salvar(usuarios);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao criar treinador: {ex.Message}");
    }
}

static void ObterArtigo(int genero, out string artigoTipo1, out string artigoTipo2, out string artigoTipo3)
{
    artigoTipo1 = Treinador.ArtigoTipo1(genero);
    artigoTipo2 = Treinador.ArtigoTipo2(genero);
    artigoTipo3 = Treinador.ArtigoTipo3(genero);
}

static void EscolherTreinador(Usuario usuario, List<Usuario> usuarios)
{
    if (usuario.Treinadores.Count == 0)
    {
        return;
    }
    while (true)
    {
        Console.WriteLine("\nSeus treinadores:\n");
        for (int i = 0; i < usuario.Treinadores.Count; i++)
        {
            var treinador = usuario.Treinadores[i];
            Console.WriteLine($"{i + 1}. {treinador.Nome} | (Nível {treinador.Nivel})");
        }
        Console.WriteLine("---------------------");
        Console.WriteLine("0. Voltar");

        string entrada = LerEntrada("\nDigite o número do treinador que deseja selecionar: ");
        if (entrada == "0")
            break;

        if (int.TryParse(entrada, out int indice) &&
            indice >= 1 && indice <= usuario.Treinadores.Count)
        {
            Treinador treinadorSelecionado = usuario.Treinadores[indice - 1];
            JogarComTreinador(treinadorSelecionado, usuario, usuarios, SalvarUsuarios);
            break;
        }
        else
        {
            Console.WriteLine("\nOpção inválida. Tente novamente.");
        }
    }
}

static void GerenciarUsuario(Usuario usuario, List<Usuario> usuarios, Action<List<Usuario>> salvar)
{
    while (true)
    {
        Console.WriteLine($"\nUsuário atual: {usuario.NomeCompleto}");
        Console.WriteLine("1. Alterar Nome de Usuário");
        Console.WriteLine("2. Excluir Usuário");
        Console.WriteLine("---------------------");
        Console.WriteLine("0. Voltar");
        string opcao = LerEntrada("\nEscolha uma opção: ");

        switch (opcao)
        {
            case "1":
                string novoNome = LerEntrada("Digite o novo nome de usuário: ");
                usuario.NomeCompleto = novoNome;
                salvar(usuarios);
                Console.WriteLine($"\nNome de usuário alterado para {usuario.NomeCompleto} com sucesso!");
                break;
            case "2":
                string confirmacao = LerEntrada("Tem certeza que deseja excluir este usuário? (S/N): ").ToUpperInvariant();
                if (confirmacao != "S" && confirmacao != "N")
                {
                    Console.WriteLine("Resposta inválida. Digite 'S' para sim ou 'N' para não.");
                    return;
                }
                if (confirmacao == "S")
                {
                    usuarios.Remove(usuario);
                    salvar(usuarios);
                    Console.WriteLine("\nUsuário excluído com sucesso!");
                    Environment.Exit(0);
                }
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Opção inválida. Tente novamente.");
                break;
        }
    }
}

static void CapturarMonstro(Treinador treinador, Monstros monstro)
{
    Rede rede = ObterRede(treinador.Nivel);

    double fatorCaptura = monstro.DificuldadedeCaptura * rede.Eficiencia;

    const double limiteCaptura = 200.0;

    double chanceCaptura = fatorCaptura / (fatorCaptura + limiteCaptura);

    Random random = new Random();
    double sorteio = random.NextDouble();

    Console.WriteLine($"\nTentando capturar {monstro.Nome} com {rede.Nome}...\n");

    if (sorteio <= chanceCaptura)
    {
        Console.WriteLine($"Parabéns! Você capturou um {monstro.Nome}!");
        Console.WriteLine($"Pontos de Experiência ganhos: {monstro.PontosDeExperiencia}.");

        treinador.Experiencia += monstro.PontosDeExperiencia;
        CalcularNivelTreinador(treinador);
        
        treinador.MonstrosCapturados.Add(monstro);
    }
    else
    {
        Console.WriteLine($"{monstro.Nome} escapou!");
    }
}

static void CalcularNivelTreinador(Treinador treinador)
{
    int nivelInicial = treinador.Nivel;
    double experienciaNecessaria = 0.8 * treinador.Nivel * treinador.Nivel * treinador.Nivel;

    while (treinador.Experiencia >= experienciaNecessaria)
    {
        treinador.Experiencia -= (int)experienciaNecessaria;
        treinador.Nivel++;
        experienciaNecessaria = 0.8 * treinador.Nivel * treinador.Nivel * treinador.Nivel;
    }

    Rede redeAtual = ObterRede(treinador.Nivel);
    treinador.Rede = redeAtual.Numeracao;

    if (treinador.Nivel > nivelInicial)
    {
        Console.WriteLine($"\n{treinador.Nome} agora está no Nível {treinador.Nivel}!");

        if (treinador.Nivel == 20)
        {
            Console.WriteLine($"{treinador.Nome} agora possui a Rede Perfeita!");
        }
        else if (treinador.Nivel == 15)
        {
            Console.WriteLine($"{treinador.Nome} agora possui a Rede Mestra!");
        }
        else if (treinador.Nivel == 10)
        {
            Console.WriteLine($"{treinador.Nome} agora possui a Rede Avançada!");
        }
        else if (treinador.Nivel == 5)
        {
            Console.WriteLine($"{treinador.Nome} agora possui a Rede Básica!");
        }
    }
}

static Rede ObterRede(int nivel)
{
    var redes = Rede.ListaDeRedes();
    if (nivel >= 20)
        return redes.First(redeAtual => redeAtual.Numeracao == 4);
    else if (nivel >= 15)
        return redes.First(redeAtual => redeAtual.Numeracao == 3);
    else if (nivel >= 10)
        return redes.First(redeAtual => redeAtual.Numeracao == 2);
    else if (nivel >= 5)
        return redes.First(redeAtual => redeAtual.Numeracao == 1);
    else
        return redes.First(redeAtual => redeAtual.Numeracao == 0);
}

static void EncontrarMonstro(Treinador treinador)
{
    var monstros = Monstros.ListaDeMonstros();

    Dictionary<Raridade, double> pesosPorRaridade = new()
    {
        { Raridade.Comum, 1.0 },
        { Raridade.Incomum, 0.5 },
        { Raridade.Raro, 0.2 },
        { Raridade.Epico, 0.1 },
        { Raridade.Lendario, 0.05 }
    };

    var pesos = monstros.Select(m => pesosPorRaridade[m.Raridade]).ToList();
    double somaPesos = pesos.Sum();

    var random = new Random();
    double valor = random.NextDouble() * somaPesos;
    double acumulado = 0;
    for (int i = 0; i < monstros.Count; i++)
    {
        acumulado += pesos[i];
        if (valor <= acumulado)
        {
            string? entrada = LerTeclaComTimeout($"\n{monstros[i].Nome} | ({monstros[i].Raridade}) encontrado! Pressione 'C' para tentar capturá-lo  (5 Segundos)\n", 5000);
            if (!string.IsNullOrEmpty(entrada) && entrada.Trim().Equals("C", StringComparison.OrdinalIgnoreCase))
            {
                CapturarMonstro(treinador, monstros[i]);
            }
            else
            {
                Console.WriteLine("\nVocê não pressionou 'C' a tempo. O monstro fugiu...");
            }
            return;
        }
    }
}

static void AbrirMochila(Treinador treinador)
{
    if (treinador.MonstrosCapturados.Count == 0)
    {
        Console.WriteLine("\nSua mochila está vazia. Nenhum monstro capturado ainda.");
        return;
    }

    Console.WriteLine("\nMonstros capturados:");
    var agrupados = treinador.MonstrosCapturados
        .GroupBy(m => m.Nome)
        .OrderBy(g => g.First().Codigo)
        .Select(g => new { Nome = g.Key, Quantidade = g.Count(), Raridade = g.First().Raridade });

    int i = 1;
    foreach (var monstro in agrupados)
    {
        Console.WriteLine($"{monstro.Quantidade} x {monstro.Nome} | {monstro.Raridade}");
        i++;
    }
}

static void JogarComTreinador(Treinador treinador, Usuario usuario, List<Usuario> usuarios, Action<List<Usuario>> salvar)
{
    ObterArtigo(treinador.Genero, out string artigoTipo1, out string artigoTipo2, out string artigoTipo3);
    Console.WriteLine($"\nVocê está jogando com {artigoTipo2} treinador{artigoTipo1} {treinador.Nome} | (Nível {treinador.Nivel}).\n");

    while (true)
    {
    MenuJogo:
        Console.WriteLine("\n1. Procurar Monstro");
        Console.WriteLine("2. Mochila");
        Console.WriteLine("---------------------");
        Console.WriteLine("0. Menu de Opções");

        string opcao = LerEntrada("\nEscolha uma opção: ");

        switch (opcao)
        {
            case "1":
                Console.WriteLine("Procurando Monstro...\n");
                EncontrarMonstro(treinador);
                break;
            case "2":
                AbrirMochila(treinador);
                break;
            case "0":
                while (true)
                {
                    Console.WriteLine("\nMenu de Opções:\n");
                    Console.WriteLine($"1. Alterar Nome d{artigoTipo2} Treinador{artigoTipo1}");
                    Console.WriteLine($"2. Alterar Gênero d{artigoTipo2} Treinador{artigoTipo1}");
                    Console.WriteLine($"3. Excluir Treinador{artigoTipo1}");
                    Console.WriteLine("4. Voltar");
                    Console.WriteLine("---------------------");
                    Console.WriteLine("0. Voltar para o Menu Principal");

                    string menuopcoes = LerEntrada("\nEscolha uma opção: ");

                    switch (menuopcoes)
                    {
                        case "1":
                            string novoNome = LerEntrada($"Digite o novo nome d{artigoTipo1} treinador{artigoTipo2}: ");
                            treinador.Nome = novoNome;
                            salvar(usuarios);
                            Console.WriteLine($"\nNome d{artigoTipo2} treinador{artigoTipo1} alterado para {treinador.Nome} com sucesso!");
                            break;
                        case "2":
                            string novoGenero = LerEntrada($"Digite o novo gênero d{artigoTipo2} treinador{artigoTipo1} (M/F): ").ToUpperInvariant();
                            if (novoGenero != "M" && novoGenero != "F")
                            {
                                Console.WriteLine("Gênero inválido. Use 'M' para masculino ou 'F' para feminino.");
                                return;
                            }
                            treinador.Genero = novoGenero == "M" ? 1 : 2;
                            salvar(usuarios);
                            ObterArtigo(treinador.Genero, out artigoTipo1, out artigoTipo2, out artigoTipo3);
                            Console.WriteLine($"\nGênero d{artigoTipo2} treinador{artigoTipo1} alterado com sucesso!");
                            break;
                        case "3":
                            string confirmacao = LerEntrada($"Tem certeza que deseja excluir est{artigoTipo3} Treinador{artigoTipo1}? (S/N):").ToUpperInvariant();
                            if (confirmacao != "S" && confirmacao != "N")
                            {
                                Console.WriteLine("Resposta inválida. Digite 'S' para sim ou 'N' para não.");
                                return;
                            }
                            if (confirmacao == "S")
                            {
                                Console.WriteLine($"Removendo Treinador{artigoTipo1} {treinador.Nome}...");
                                usuario.Treinadores.Remove(treinador);
                                Console.WriteLine("Treinador excluído com sucesso!");
                                salvar(usuarios);
                                return;
                            }
                            break;
                        case "4":
                            goto MenuJogo;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Opção inválida. Tente novamente.");
                            break;
                    }
                }
            default:
                Console.WriteLine("Opção inválida. Tente novamente.");
                break;
        }
    }
}
