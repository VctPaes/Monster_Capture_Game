using System;

namespace ProjetoFinal.modelos;

public class Monstros
{
    public string Nome { get; set; }
    public int Nivel { get; set; }
    public int Vida { get; set; }
    public int Ataque { get; set; }
    public int Defesa { get; set; }
    public int Velocidade { get; set; }
    public string Tipo1 { get; set; }
    public string Tipo2 { get; set; }

    public Monstros(string nome, int nivel, int vida, int ataque, int defesa, int velocidade, string tipo1, string tipo2)
    {
        Nome = nome;
        Nivel = nivel;
        Vida = vida;
        Ataque = ataque;
        Defesa = defesa;
        Velocidade = velocidade;
        Tipo1 = tipo1;
        Tipo2 = tipo2;
    }
}
