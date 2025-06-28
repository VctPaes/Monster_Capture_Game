using System;
using System.Text.Json.Serialization;

namespace ProjetoFinal.modelos;

public class Usuario
{
    public string NomeCompleto { get; private set; }

    private List<Treinador> _treinadores = new List<Treinador>();
    public IReadOnlyCollection<Treinador> Treinadores => _treinadores.AsReadOnly();

    [JsonConstructor]
    private Usuario(string nomeCompleto, List<Treinador> treinadores = null)
    {
        NomeCompleto = nomeCompleto;
        if (treinadores != null)
        {
            _treinadores = treinadores;
        }
    }

    public Usuario(string nomeCompleto)
    {
        if (string.IsNullOrWhiteSpace(nomeCompleto))
        {
            throw new ArgumentException("O nome de usuário não pode ser vazio.", nameof(nomeCompleto));
        }

        NomeCompleto = nomeCompleto;
    }

    public void AdicionarTreinador(Treinador treinador)
    {
        if (treinador == null)
        {
            throw new ArgumentNullException(nameof(treinador), "O treinador não pode ser nulo.");
        }
        _treinadores.Add(treinador);
    }
}
