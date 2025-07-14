using System;
using System.Collections.Generic;

public class Town
{
    public void Enter(Player player)
    {
        while (true)
        {
            int selected = MenuSelector.Select("마을에 오신 것을 환영합니다!", new List<string>
            {
                "상점에 들어간다",
                "쉼터에서 휴식한다",
                "마을을 떠난다"
            }, true);

            Console.Clear();

            if (selected == -1 || selected == 2)
            {
                Console.WriteLine("마을을 떠납니다...");
                break;
            }

            switch (selected)
            {
                case 0:
                    EnterShop(player);
                    break;
                case 1:
                    RestAtInn(player);
                    break;
            }

            Console.WriteLine("\n아무 키나 눌러 계속...");
            Console.ReadKey();
        }
    }

    private void EnterShop(Player player)
    {
        Console.WriteLine("[상점]");
        Console.WriteLine("아직은 아이템 시스템이 구현되지 않았습니다!");
    }

    private void RestAtInn(Player player)
    {
        Console.WriteLine("[쉼터]");
        player.HP = player.MaxHP;
        Console.WriteLine($"{player.Name} 님이 휴식을 취하고 HP가 모두 회복되었습니다!");
    }
}

