using Entidades.Enumerados;


namespace Entidades.MetodosDeExtension
{
    public static class IngredientesExtension
    {

        public static double CalcularCostoIngrediente(this List<EIngrediente> ingredientes, int costoInicial)
        {
            double resu = 0;
            foreach (EIngrediente item in ingredientes)
            {
                resu += (((int)item) * costoInicial) / 100;
            }
            resu += costoInicial;
            return resu;
        }

        public static List<EIngrediente> IngredientesAleatorios(this Random rand)
        {
            List<EIngrediente> ingredientes = new List<EIngrediente>() {EIngrediente.QUESO, EIngrediente.PANCETA, EIngrediente.ADHERESO, EIngrediente.HUEVO, EIngrediente.JAMON };
            int valorRand = rand.Next(1,ingredientes.Count + 1);
            return  ingredientes.Take(valorRand).ToList();
        }
    }
}
