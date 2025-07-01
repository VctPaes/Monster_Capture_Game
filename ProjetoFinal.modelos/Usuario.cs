using System;
using System.Text.Json.Serialization;

namespace ProjetoFinal.modelos;

public class Usuario
{
    public string NomeCompleto { get; set; }

    public List<Treinador> Treinadores { get; set; } = new();

    [JsonIgnore]
    public IReadOnlyCollection<Treinador> TreinadoresReadOnly => Treinadores;

    public Usuario(string nomeCompleto)
    {
        NomeCompleto = nomeCompleto;
    }

    public void AdicionarTreinador(Treinador treinador)
    {
        Treinadores.Add(treinador);
    }
}
