# Projeto Simples Fazendo CRUD no Banco de Dados
#### Projetinho de CRUD que peguei pra brincar final de semana, coloquei um tempo de 1 Hora e Meia para o termino do CRUD na tabela de Categoria, onde o Usuário consegue Adicionar, Excluir, Listar e Atualizar a Categoria de Cursos. 
``` Nuget Usado: Microsoft.Data.SqlClient e Dapper ```

### Banco de Dados
```SQL
CREATE DATABASE [Dapper];
CREATE TABLE [Categoria](Id INT CONSTRAINT PK_Categoria PRIMARY KEY IDENTITY(1, 1), [Name] NVARCHAR(20) NOT NULL CONSTRAINT Unique_NameCategoria UNIQUE)
```
### Docker e SQL Server 2019 / Azure Data Studio.

* [Instalação do Docker](https://balta.io/blog/docker-instalacao-configuracao-e-primeiros-passos?utm_source=github&utm_medium=2805-repo&utm_campaign=readme).
* [Instalação do SQL Server no Docker](https://balta.io/blog/sql-server-docker?utm_source=github&utm_medium=2805-repo&utm_campaign=readme)
* [Download do Azure Data Studio](https://docs.microsoft.com/pt-br/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver15)

### Validação Usando Regex
```C#
IsValid = (!Regex.IsMatch(opc, "^[1234]$")) ? throw new ArgumentException($"Valor Invalido! \n{Opcs}") : true;
```
### Usuário Adicionar Categoria
```C#
static void AdicionarCategorias(SqlConnection conectar, string nomeCategoria)
      {
          int update = conectar.Execute("INSERT INTO [Categoria]([Name]) VALUES (@nomeCategoria)", new { nomeCategoria });
          FeedBackDeEvento(update, "Inseridas"); //Estou passando a quantidade de linhas alteradas e caso for maior que 0, estou passando a ação de sucesso
          LimpaTelaClick();
      }
```
### FeedBackDeEvento
  Nessa função, Estou Apenas verificando se as linhas foram inseridas, se linhas alteradas for mais que 0, pegando o exemplo de Adicionar,
  Ele ira escrever no console que a ação passada deu certo, nesse caso seria "Informação Inseridas".
```C#
static void FeedBackDeEvento(int linhasEvento, string evento) => Console.WriteLine(linhasEvento > 0 ? $"Informação {evento}!" : "Informação Incorreta! Verifique os Dados");
```
### Update
#### Tanto Update quanto o Delete fiz na mesma lógica, Estou recebendo como parâmetro o conteúdo que devo editar, e o novo conteúdo, que seria o novo nome da Categoria.
Estou Verificando se o Identificador passado é o Id ou o Nome da Categoria, Lembrando que no Banco a Tabela [Categoria] tem o Campo Nome como Único: ```SQL CONSTRAINT Unique_NameCategoria UNIQUE ```.
#### Se o Id passado for um inteiro, Então ele entra no escopo 1, se não, ele vai entrar no segundo, onde vai tentar pegar o Id através do Nome da Categoria.
A variavel update foi declarada para pegar o numero de linhas alteradas conforme a função FeedBackDeEvento Explicado. Se for 0, Significa que o Update não foi feito, além, de passar o FeedBack de Sucesso se update for mais que 0 => Informação {evento} // Informação Atualizada

```C#
static void UpdateCategorias(SqlConnection conectar, string id, string name //Novo Nome)
        {
            string QueryUpdate = "UPDATE [Categoria] SET [Name] = @name WHERE [Id] = ";
            int update;
            if (int.TryParse(id, out int val))
            {
                update = conectar.Execute(QueryUpdate + "@id", new { name, id = val });

            }
            else
            {
                update = conectar.Execute(QueryUpdate + "(SELECT [Id] FROM [Categoria] WHERE [Name] = @id);", new { name, id });
            }
            FeedBackDeEvento(update, "Atualizada");
            LimpaTelaClick();
        }
```
### Caso Alguém tenha Interesse em Continuar, Vou deixar aqui uma atividade para você, jovem Padawan!
Crie essa Tabela no Banco [Cursos] conforme a tabela a baixo. Nas Opções No código, deixe o usuário fazer o Create, Reader, Update e Delete. Faça todas as validações com Regex, e trate as Exception.

![image](https://user-images.githubusercontent.com/77933748/227757318-83047b2d-771a-48f0-b1d9-ae3bf14e7dba.png)

Para Listar, Deixei já duas views Prontas:
##### A Primeira vai pegar toda a tabela Curso e juntar com o Nome da  Categoria.
##### A Segunda Ira Mostrar quantos Cursos tem em Cada Categoria, Exemplo: Mobile[Kotlin, Flutter, React Native] = 3;
```SQL
CREATE OR ALTER VIEW ListaCompleta AS
SELECT [Curso].[Id], [Curso].[Name], [Categoria].[Name] AS [NomeCategoria] FROM [Curso] 
    INNER JOIN [Categoria] ON [Curso].[FK_Categoria] = [Categoria].[Id];
```
```SQL
CREATE OR ALTER VIEW QtdPorCategoria AS
SELECT [Categoria].[Name], COUNT([Curso].[FK_Categoria]) AS [QuantidadeCategorias] FROM [Curso] 
    INNER JOIN [Categoria] ON [Curso].[FK_Categoria] = [Categoria].[Id]
        GROUP BY [Categoria].[Name], [Curso].[FK_Categoria];
```
Caso tenha Alguma dúvida, me chama no [Linkedin](https://www.linkedin.com/in/patrickaquinodesouza/)
