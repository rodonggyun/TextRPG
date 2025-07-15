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
                "호텔에서 휴식한다",
                "대장간에 간다",
                "마을을 떠난다"
            }, true);

            Console.Clear();

            if (selected == -1 || selected == 3)
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
                    RestAtHotel(player);
                    break;
                case 2:
                    EnterForge(player);
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

    private void RestAtHotel(Player player)
    {
        Console.WriteLine("[호텔]");
        player.HP = player.MaxHP;
        Console.WriteLine($"{player.Name} 님이 호텔에서 휴식을 취하고 HP를 모두 회복했습니다!");
    }

    private void EnterForge(Player player)
    {
        Console.WriteLine("[대장간]");
        Console.WriteLine("장비 강화 기능은 아직 준비 중입니다.");
        // TODO: 장비 선택, 강화 확률, 강화 비용, 실패 여부 등 구현 예정
    }

    
}

