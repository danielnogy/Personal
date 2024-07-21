using System;

namespace BinaryPlate.BlazorPlate.Helpers
{
    public static class OthersHelper
    {
        private static Random random = new Random();
        public static int GetRandomIntExcluding(List<int> exclusionList, int minValue, int maxValue)
        {
            int randomInt;

            do
            {
                randomInt = random.Next(minValue, maxValue);
            } while (exclusionList.Contains(randomInt));

            return randomInt;
        }
    }
}
