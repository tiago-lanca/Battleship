# Battleship (Projeto Batalha Naval em Consola)
 Projeto foi desenvolvido por Tiago Lança.
 
 <!-- TABLE OF CONTENTS -->
### Indice
- [💻 Sobre o projeto](#-sobre-o-projeto)
- [🚀 Como executar o projeto](#-como-executar-o-projeto)
- [Organização e Estruturação do Projeto](#organizacao-e-estruturacao-do-projeto)
- [📂 Estruturas de Dados](#-estruturas-de-dados)


## 💻 Sobre o projeto
Este projeto consta no jogo da Batalha Naval em consola desenvolvido na disciplina Programação e Algoritmos.



O jogo consiste em opor duas
frotas, cada uma posicionada numa grelha de 10 x 10. Cada jogador só conhece o posicionamento da
sua frota. Cada navio na frota ocupa um conjunto de posições na grelha, de acordo com as suas
características.

Alternadamente, cada jogador dispara tiros para posições na grelha do adversário, podendo acertar
num navio ou na água. Assim que um navio recebe tiros em todas as posições que ocupa, fica
afundado. O jogo termina quando um jogador afunda toda a frota do adversário.
 
## 🚀 Como executar o projeto

1. Clonar ou descarregar o repositório do github.

2. Entrar na pasta do repositório e abrir o projeto em:
   - `bin/Debug/net8.0/Battleship.exe` (Se for para executar simplesmente o jogo).

     Ou
     
   - `Battleship.sln` (Se for para abrir o projeto e executá-lo dentro do IDE).
     * Clicar no botão Start (F5) para iniciar o executável para abrir uma janela de terminal separada.

       Ou
       
     * Abrir o terminal em `View/Terminal` (Ctrl+ç) e inserir:
       ```
        dotnet run
       ```

## Organização e Estruturação do Projeto

Este projeto foi organizado e estruturado com base no modelo MVC (Model-View-Controller) usando tambem Interfaces para implementar funcionalidades.

- Models
  * Player (Guarda todos os dados de um jogador e algumas funcionalidades relativas ao jogador).
  * PlayerList (Uma lista de jogadores onde é possivel mostrar todos os jogadores, registar novos jogadores, eliminar, etc).
  * Ship (Guarda todos os dados relacionados com um Navio e várias funcionalidades referentes a este).
  * Location (Para criar uma localização X e Y).
  * Board (Para a tabela visual).
 
- Views
  * ViewConsole (Responsável por imprimir todo o conteudo visual, mensagens, jogo, etc).

- Controllers
   * PlayerController (Responsável por controlar as funcionalidades relacionadas ao `Player`).
   * CommandController (Responsável por controlar o comando de input do utilizador na consola).
   * GameController (Controla todo o procedimento do jogo, inserção de navios, inicio de jogo, jogadas, etc).

 Foi implementado uma ViewModel para desenhar a estrutura de um jogo iniciado, com os dados envolventes.
 - GameViewModel

 Foram usadas interfaces para implementar funcionalidades:
 - ICommandManager (Referente aos comandos de input do utilizador no jogo).
 - IPlayerList (Referente à lista de jogadores).
 - IPlayerManager (Referente ao jogador).
 - IShip (Referente ao Navio).
 - IGameViewModel (Referente ao jogo)


## 📂 Estruturas de Dados
Neste projeto foram usadas diversas estruturas de dados, como:
- Arrays (Para armazenar jogadores em jogo).
- Arrays Bidimensionais (Para a estruturação do tabuleiro de jogo).
- Tuplas (Foram usadas na função GameViewModel.IsEmptyAround para guardar o X e Y, inteiros, de uma localização).
- Listas Genéricas (Para criar listas de jogadores, navios, etc).



