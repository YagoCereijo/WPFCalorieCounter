namespace Controles1
{
    class Amigo
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public int edad { get; set; }

        public Amigo(string nombre, string apellido, int edad)
        {
            this.nombre = nombre;
            this.apellido = apellido;
            this.edad = edad;
        }

        public override string ToString()
        {
            return nombre + " " + apellido + " " + edad;
        }
    }
}
