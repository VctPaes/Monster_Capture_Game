using System;
using System.Text.Json.Serialization;

namespace ProjetoFinal.modelos
{
    public class Treinador
    {
        public string Nome { get; private set; }
        public int Genero { get; set; }
        public int Nivel { get; private set; }
        public int Experiencia { get; private set; }
        public List<Monstros> MonstrosCapturados { get; private set; }

        [JsonConstructor]
        private Treinador(string nome, int genero, int nivel, int experiencia)
        {
            Nome = nome;
            Genero = genero;
            Nivel = nivel;
            Experiencia = experiencia;
            MonstrosCapturados = new List<Monstros>();
        }

        public Treinador(string nome, int genero)
            : this(nome, genero, 1, 0)
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
        public static string ArtigoTipo1(int genero)
        {
            return genero == 1 ? "" : "a"; // O artigo masculino permanece vazio
        }

        public static string ArtigoTipo2(int genero)
        {
            return genero == 1 ? "o" : "a"; // Adiciona artigo "o" para masculino
        }

        public static string ArtigoTipo3(int genero)
        {
            return genero == 1 ? "e" : "a"; // Adiciona artigo "e" para masculino
        }
    }
}