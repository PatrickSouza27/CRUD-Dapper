using System;
using System.Text.RegularExpressions;
using Dapper;
using Microsoft.Data.SqlClient;
using Modulo4N;

namespace Prog
{
    class Program
    {
        static readonly string Opcs = "1)Categoria\n2)Curso\n3)Sair\nDigite o que você quer fazer no banco : ";
        static readonly string ConnectionString = "String do Banco aqui";

        static void LimpaTelaClick()
        {
            Console.ReadKey();
            Console.Clear();
        }
        static int ValidandoEntrada(string? opc)
        {
            bool IsValid = false;
            while (!IsValid)
            {
                try
                {
                    IsValid = (!Regex.IsMatch(opc, "^[1234]$")) ? throw new ArgumentException($"Valor Invalido! \n{Opcs}") : true;
                }
                catch (ArgumentException ex)
                {
                    Console.Clear();
                    Console.Write(ex.Message);
                    opc = Console.ReadLine();
                }
            }
            return int.Parse(opc);

        }
        static void FeedBackDeEvento(int linhasEvento, string evento) => Console.WriteLine(linhasEvento > 0 ? $"Informação {evento}!" : "Informação Incorreta! Verifique os Dados");
        static void ListarCategorias(SqlConnection conectar)
        {
            var Lista = conectar.Query<Categoria>("SELECT [Id], [Name] FROM [Categoria] ORDER BY [Id];");
            foreach (var categoria in Lista)
            {
                Console.WriteLine(categoria);
            }
            LimpaTelaClick();

        }
        static void AdicionarCategorias(SqlConnection conectar, string nomeCategoria)
        {
            int update = conectar.Execute("INSERT INTO [Categoria]([Name]) VALUES (@nomeCategoria)", new { nomeCategoria });
            FeedBackDeEvento(update, "Inseridas");
            LimpaTelaClick();
        }

        static void UpdateCategorias(SqlConnection conectar, string id, string name)
        {
            int update = 0;
            string QueryUpdate = "UPDATE [Categoria] SET [Name] = @name WHERE [Id] = ";
            if (int.TryParse(id, out int val))
            {
                update = conectar.Execute(QueryUpdate + "@id", new { name, id = val });
                
            }
            else {
                update = conectar.Execute(QueryUpdate + "(SELECT [Id] FROM [Categoria] WHERE [Name] = @id);", new { name, id });
            }
            FeedBackDeEvento(update, "Atualizada");
            LimpaTelaClick();
        }
        static void ExcluirCategorias(SqlConnection conectar, string id)
        {
            int update = 0;
            string QueryDelete = "DELETE [Categoria] WHERE [Id] = ";
            if (int.TryParse(id, out int val))
            {
                update = conectar.Execute(QueryDelete + "@id", new { id = val });
            }
            else
            {
                update = conectar.Execute(QueryDelete + "(SELECT [Id] FROM [Categoria] WHERE [Name] = @id)", new { id });
            }
            FeedBackDeEvento(update, "Excluida");
            LimpaTelaClick();
        }

        static void TelaCategoria(SqlConnection connection)
        {
            Console.Clear();
            Console.Write("1)Listar Categorias\n2)Adicionar Categorias\n3)Atualizar Categorias\n4)Excluir Categorias\nDigite Oque Você quer fazer em Categoria: ");
            var opc = ValidandoEntrada(Console.ReadLine());
            switch (opc)
            {
                case 1:
                    ListarCategorias(connection);
                    break;
                case 2:
                    Console.Write("Digite o Nome da Categoria que Deseja Adicionar:");
                    AdicionarCategorias(connection, Console.ReadLine());
                    break;
                case 3:
                    Console.Write("Digite o Id/Nome que deseja Atualizar: ");
                    string Id = Console.ReadLine();
                    Console.Write("Digite A Informação Atualizada: ");
                    UpdateCategorias(connection, Id, Console.ReadLine());
                    break;
                case 4:
                    Console.Write("Digite o Id/Nome que deseja Excluir: ");
                    ExcluirCategorias(connection, Console.ReadLine());
                    break;
            }
        }
        static void Main(string[] args)
        {
            bool exit = false;
            do
            {
                Console.Write(Opcs);
                var opc = ValidandoEntrada(Console.ReadLine());
                switch (opc)
                {
                    case 1:
                        using (var connection = new SqlConnection(ConnectionString))
                        {                         
                            TelaCategoria(connection);
                        }
                        break;
                    case 2:
                        //
                        break;
                    case 3 :
                        exit = true;
                        break;
                }
            } while (exit == false);

        }
    }
}
