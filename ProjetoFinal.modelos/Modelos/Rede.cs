using System;

namespace ProjetoFinal.modelos;

public class Rede
{
    public string Nome { get; set; }
    public Raridade Raridade { get; set; }
    public int Eficiencia { get; set; }

    public Rede(string nome, Raridade raridade, int eficiencia)
    {
        Nome = nome;
        Raridade = raridade;
        Eficiencia = eficiencia;
    }
}

