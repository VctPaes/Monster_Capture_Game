using System;

namespace ProjetoFinal.modelos;

public class Treinador
{
    public string Nome { get; private set; }
    public int Gênero { get; set; }
    public int Nivel { get; private set; }
    public int Experiencia { get; private set; }
    public List<Monstros> MonstrosCapturados { get; private set; }

    private Treinador(string nome, int gênero, int nivel, int experiencia)
    {
        Nome = nome;
        Gênero = gênero;
        Nivel = nivel;
        Experiencia = experiencia;
        MonstrosCapturados = new List<Monstros>();
    }

    public Treinador(string nome, int gênero)
        : this(nome, gênero, 1, 0)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("O nome do treinador(a) não pode ser vazio.", nameof(nome));
        }
    }

    public void CapturarMonstro(Monstros monstro)
    {
        MonstrosCapturados.Add(monstro);
        Console.WriteLine($"{Nome} capturou um {monstro.Nome}!");
    }

    public void GanharExperiencia(int pontos)
    {
        Experiencia += pontos;
        if (Experiencia >= Nivel * 100)
        {
            Nivel++;
            Console.WriteLine($"{Nome} subiu para o nível {Nivel}!");
        }
    }
}
