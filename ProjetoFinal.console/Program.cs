using System.Text;
using System.Globalization;
using ProjetoFinal.modelos;
using Newtonsoft.Json;

Console.WriteLine("Bem-vindo ao jogo de captura de monstros!");

const string arquivoUsuarios = "usuarios.json";
List<Usuario> usuarios = CarregarUsuarios();

string nomeCompleto = LerEntrada("Digite seu nome de usuário: ");
Usuario usuario = BuscarOuCriarUsuario(nomeCompleto, usuarios, SalvarUsuarios);

while (true)
{
    Console.WriteLine("\nMenu:");
    Console.WriteLine("1. Novo Treinador");
    Console.WriteLine("2. Escolher Treinador");
    Console.WriteLine("3. Sair");

    string opcao = LerEntrada("Escolha uma opção: ");

    switch (opcao)
    {
        case "1":
            NovoTreinador(usuario, usuarios, SalvarUsuarios);
            break;
        case "2":
            // Lógica para ver treinadores
            break;
        case "3":
            Console.WriteLine("Saindo...");
            return;
        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
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
        Console.WriteLine($"Usuário {usuario.NomeCompleto} criado!");
        salvar(usuarios);
    }
    else
    {
        Console.WriteLine($"Bem-vindo de volta, {usuario.NomeCompleto}!");
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
    Treinador treinador = new(nomeTreinador, genero);
    usuario.AdicionarTreinador(treinador);

    string artigoTipo1 = generoTreinador == "M" ? "" : "a"; // O artigo masculino permanece vazio
    string artigoTipo2 = generoTreinador == "M" ? "o" : "a"; // Adiciona artigo "o" para masculino
    Console.WriteLine($"Treinador{artigoTipo1} {treinador.Nome} criad{artigoTipo2} com sucesso!");

    salvar(usuarios);
}
