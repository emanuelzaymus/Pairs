using Pairs.Core;
using Pairs.InterfaceLibrary;

namespace Pairs.ConsoleApp
{
    class Program
    {
        static void Main()
        {
            var pairsGame = new PairsGame(GameLayout.ThreeTimesTwo, 2);
            var gameCli = new PairsGameCli(pairsGame);

            gameCli.Run();
        }
    }
}
