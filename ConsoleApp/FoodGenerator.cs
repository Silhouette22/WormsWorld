namespace ConsoleApp
{
    public static class FoodGenerator
    {
        private static Coords GetRandomCoords() => WormGenerator.GetRandomCoords();

        public static IObject GetFood()
        {
            return new Food(GetRandomCoords());
        }
    }
}