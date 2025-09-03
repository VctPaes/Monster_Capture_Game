#  Monster Capture Game

Este repositório apresenta um jogo simples, programado em **C#**, de captura de montros através de interações no terminal. Ele foi desenvolvido como projeto final da disciplina **Programação Orientada a Objetos**, apresentando conceitos de **POO** apresentados durante o período letivo.

##  Funcionalidades

-  Registro de usuários através do nome escolhido.
-  Possibilidade Alteração do nome e exclusão do usuário.
-  Criação de treinadores associados ao usuário, com opção de gênero do treinador.
-  Possibilidade de lteração do nome e do gênero e exclusão do treinador.
-  Tentativa de captura de um monstro aleatório.
-  Ganho de XP e recompensa por marco de nível.
-  Acesso à mochila com todos os monstros capturados.

##  Como Executar

1. Certifique-se de ter o .NET SDK instalado.
2. Navegue até o diretório do projeto e insira os seguintes comandos caso seja a primeira vez executando o projeto:
```bash
   cd "C:\Users\Exemplo\Downloads\Monster_Capture_Game-main\ProjetoFinal.console"

   dotnet run
```
3. Após a primeira execução, é possível executar o projeto utilizando apenas o comando:
```bash
   dotnet run
```

##  Conceitos Aplicados

-  Herança
-  Associações
-  Relacionamentos
-  Polimorfismo
-  Modelagem de classes
-  Enums
-  Interfaces
-  Persistência em Memória e em Arquivo

##  Estrutura do Projeto

```
/Monster_Capture_Game-main/
│
├── ProjetoFinal.console/
│   ├── Program.cs
│   ├── ProjetoFinal.console.csproj
│   ├── usuarios.json
│
├── ProjetoFinal.modelos/
│   ├── Enums
│       ├── Raridade.cs
│   ├── Modelos
│       ├── Monstros.cs
│       ├── Rede.cs
│       ├── Treinador.cs
│       ├── Usuário.cs
│   ├── ProjetoFinal.modelos.csproj
│
├── .gitattributes
├── LICENSE
├── ProjetoFinal.sln
├── README.md
```

##  Tecnologias Utilizadas

-  **C#**: Linguagem de programação principal.
-  **.NET SDK**: Framework para desenvolvimento e execução do projeto.
-  **JSON**: Formato de armazenamento de dados.

##  Créditos

Este jogo foi desenvolvido por Victor Luiz De Oliveira Paes como projeto final da disciplina de Programação Orientada a Objetos.

##  Contato
Email: victorpaes@alunos.utfpr.edu.br
GitHub: github.com/VctPaes 
