namespace Modulo4N
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Categoria() { }
        public Categoria(int id, string name, string nomeCategoria)
        {
            Id = id;
            Name = name;

        }
        public override string ToString()
        {
            return "\t| " + Id + " - " + Name + "\t |";
        }
    }
}
