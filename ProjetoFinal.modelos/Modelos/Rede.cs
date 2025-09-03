using System;

namespace ProjetoFinal.modelos;

public class Rede
{
    public int Numeracao { get; set; }
    public string Nome { get; set; }
    public Raridade Raridade { get; set; }
    public float Eficiencia { get; set; }

    public Rede(int numeracao, string nome, Raridade raridade, float eficiencia)
    {
        Numeracao = numeracao;
        Nome = nome;
        Raridade = raridade;
        Eficiencia = eficiencia;
    }

    public static List<Rede> ListaDeRedes()
    {
        return new List<Rede>
        {
            new Rede(4,"Rede Perfeita", Raridade.Lendario, 3),
            new Rede(3,"Rede Mestra", Raridade.Epico, 2.5f),
            new Rede(2,"Rede Avançada", Raridade.Raro, 2),
            new Rede(1,"Rede Básica", Raridade.Comum, 1.5f),
            new Rede(0,"Rede Furada", Raridade.Comum, 1)
        };
    }
}

