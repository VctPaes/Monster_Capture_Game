using System.Text;
using System.Globalization;
using ProjetoFinal.modelos;
using Newtonsoft.Json;

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
            break;
        case "2":
            EscolherTreinador(usuario, usuarios);
            if (usuario.Treinadores.Count == 0)
            {
                Console.WriteLine("Deseja criar um novo treinador para começar sua jornada? (S/N)");

                string resposta;
                do
                {
                    resposta = Normalizar(Console.ReadLine() ?? "");
                    if (resposta != "s" && resposta != "n")
                    {
                        Console.WriteLine("Resposta inválida. Digite 'S' para sim ou 'N' para não.");
                    }
                } while (resposta != "s" && resposta != "n");

                if (resposta == "s")
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

static void EscolherTreinador(Usuario usuario, List<Usuario> usuarios)
{
    if (usuario.Treinadores.Count == 0)
    {
        Console.WriteLine("\nVocê ainda não possui treinadores.");
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

static void ExcluirTreinador(Usuario usuario, List<Usuario> usuarios, Action<List<Usuario>> salvar)
{
    string nomeTreinador = LerEntrada("\nDigite o nome do treinador que deseja excluir: ");
    Treinador? treinador = usuario.Treinadores.FirstOrDefault(t => t.Nome.Equals(nomeTreinador, StringComparison.OrdinalIgnoreCase));

    if (treinador != null)
    {
        usuario.Treinadores.Remove(treinador);
        string artigoTipo1 = Treinador.ArtigoTipo1(treinador.Genero);
        string artigoTipo2 = Treinador.ArtigoTipo2(treinador.Genero);
        string artigoTipo3 = Treinador.ArtigoTipo3(treinador.Genero);
        Console.WriteLine($"\nTreinador{artigoTipo1} {treinador.Nome} excluíd{artigoTipo2} com sucesso!");
        salvar(usuarios);
    }
    else
    {
        Console.WriteLine("\nTreinador não encontrado.");
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
                Console.WriteLine("\nNome de usuário alterado com sucesso!");
                break;
            case "2":
                Console.WriteLine("Tem certeza que deseja excluir este usuário? (S/N)");
                string confirmacao;
                do
                {
                    confirmacao = Normalizar(Console.ReadLine() ?? "");
                    if (confirmacao != "s" && confirmacao != "n")
                    {
                        Console.WriteLine("Resposta inválida. Digite 'S' para sim ou 'N' para não.");
                    }
                } while (confirmacao != "s" && confirmacao != "n");
                if (confirmacao == "s")
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

static void JogarComTreinador(Treinador treinador, Usuario usuario, List<Usuario> usuarios, Action<List<Usuario>> salvar)
{
    string artigoTipo1 = Treinador.ArtigoTipo1(treinador.Genero);
    string artigoTipo2 = Treinador.ArtigoTipo2(treinador.Genero);
    string artigoTipo3 = Treinador.ArtigoTipo3(treinador.Genero);
    Console.WriteLine($"\nVocê está jogando com {artigoTipo2} treinador{artigoTipo1} {treinador.Nome} | (Nível {treinador.Nivel}).\n");

    while (true)
    {
        Console.WriteLine("1. Procurar Monstro");
        Console.WriteLine("2. Mochila");
        Console.WriteLine("---------------------");
        Console.WriteLine("0. Menu de Opções");

        string opcao = LerEntrada("\nEscolha uma opção: ");

        switch (opcao)
        {
            case "1":
                Console.WriteLine("Procurando Monstro...\n");
                // Função encontrar monstro (placeholder)
                Console.WriteLine("\n**(Nome do Monstro)** encontrado! Pressione 'C' para capturá-lo\n");
                break;
            case "2":
                Console.WriteLine("\nMochila Vazia...\n");
                break;
            case "0":
                while (true)
                {
                    Console.WriteLine("\nMenu de Opções:\n");
                    Console.WriteLine("1. Alterar Nome do Treinador");
                    Console.WriteLine("2. Alterar Gênero do Treinador");
                    Console.WriteLine("3. Excluir Treinador");
                    Console.WriteLine("4. Voltar");
                    Console.WriteLine("---------------------");
                    Console.WriteLine("0. Voltar para o Menu Principal");

                    string menuopcoes = LerEntrada("\nEscolha uma opção: ");

                    switch (menuopcoes)
                    {
                        case "1":
                            //altera nome
                            return;
                        case "2":
                            //altera genero
                            return;
                        case "3":
                            Console.WriteLine($"Tem certeza que deseja excluir est{artigoTipo3} Treinador{artigoTipo1}? (S/N)");
                            string confirmacao;
                            do
                            {
                                confirmacao = Normalizar(Console.ReadLine() ?? "");
                                if (confirmacao != "s" && confirmacao != "n")
                                {
                                    Console.WriteLine("Resposta inválida. Digite 'S' para sim ou 'N' para não.");
                                }
                            } while (confirmacao != "s" && confirmacao != "n");
                            if (confirmacao == "s")
                            {
                                Console.WriteLine($"\nRemovendo Treinador{artigoTipo1} {treinador.Nome}...\n");
                                usuario.Treinadores.Remove(treinador);
                                Console.WriteLine("\nTreinador excluído com sucesso!");
                                salvar(usuarios);
                                return;
                            }
                            break;
                        default:
                            Console.WriteLine("Opção inválida. Tente novamente.");
                            break;
                    }

                    //switch (menuopcoes)
                }

                break;
            default:
                Console.WriteLine("Opção inválida. Tente novamente.");
                break;
        }
    }
}
