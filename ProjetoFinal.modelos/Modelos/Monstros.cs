using System;

namespace ProjetoFinal.modelos;

public class Monstros
{
    public int Codigo { get; set; }
    public string Nome { get; set; }
    public Raridade Raridade { get; set; }
    public string Tipo1 { get; set; }
    public string Tipo2 { get; set; }
    public int DificuldadedeCaptura { get; set; }
    public int PontosDeExperiencia { get; set; }

    public Monstros(int codigo, string nome, Raridade raridade, string tipo1, string tipo2, int dificultadedeCaptura, int pontosDeExperiencia)
    {
        Codigo = codigo;
        Nome = nome;
        Raridade = raridade;
        Tipo1 = tipo1;
        Tipo2 = tipo2;
        DificuldadedeCaptura = dificultadedeCaptura;
        PontosDeExperiencia = pontosDeExperiencia;
    }

    public static List<Monstros> ListaDeMonstros()
    {
        return new List<Monstros>
        {
            new Monstros(1, "Bulbossauro", Raridade.Comum, "Planta", "Veneno", 200, 50),
            new Monstros(4, "Brasagarto", Raridade.Comum, "Fogo", "Fogo", 200, 50),
            new Monstros(7, "Jataruga", Raridade.Comum, "Agua", "Agua", 200, 50),
            new Monstros(2, "Herassauro", Raridade.Incomum, "Planta", "Veneno", 100, 200),
            new Monstros(5, "Fogarto", Raridade.Incomum, "Fogo", "Fogo", 100, 200),
            new Monstros(8, "Tancaruga", Raridade.Incomum, "Agua", "Agua", 100, 200),
            new Monstros(3, "Venossauro", Raridade.Raro, "Planta", "Veneno", 60, 500),
            new Monstros(6, "Chagarto", Raridade.Raro, "Fogo", "Voador", 60, 500),
            new Monstros(9, "Jabutanque", Raridade.Raro, "Agua", "Agua", 60, 500),
            new Monstros(10, "Gengar", Raridade.Epico, "Fantasma", "Venenoso", 30, 1000),
            new Monstros(11, "Quasaray", Raridade.Lendario, "Dragao", "Voador", 10, 3000)
        };
    }

}