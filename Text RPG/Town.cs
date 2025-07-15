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
                "인벤토리 열기",
                "마을을 떠난다"
            }, true);

            Console.Clear();

            if (selected == -1 || selected == 4)
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
                case 3: 
                    OpenInventory(player); 
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

    private void OpenInventory(Player player)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("🎒 [인벤토리 목록]");
            if (player.Inventory.Count == 0)
                Console.WriteLine(" - 인벤토리가 비었습니다.");
            else
                for (int i = 0; i < player.Inventory.Count; i++)
                    Console.WriteLine($"[{i}] {player.Inventory[i].Name} ({player.Inventory[i].Type})");

            Console.WriteLine($"\n[장착 중]");
            Console.WriteLine($"무기: {(player.EquippedWeapon?.Name ?? "없음")}");
            Console.WriteLine($"방어구: {(player.EquippedArmor?.Name ?? "없음")}");

            Console.WriteLine("\n아이템 번호 입력 → 장착/사용, X 입력 → 나가기");
            Console.Write("선택: ");
            string input = Console.ReadLine();
            if (input.ToLower() == "x") break;

            if (int.TryParse(input, out int idx) && idx >= 0 && idx < player.Inventory.Count)
                HandleItem(player, player.Inventory[idx]);
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
            }
        }
    }

    private void HandleItem(Player player, Item item)
    {
        switch (item.Type)
        {
            case ItemType.Weapon:
                player.EquippedWeapon = item;
                Console.WriteLine($"{item.Name}을(를) 무기로 장착했습니다!");
                break;
            case ItemType.Armor:
                player.EquippedArmor = item;
                Console.WriteLine($"{item.Name}을(를) 방어구로 장착했습니다!");
                break;
            case ItemType.Consumable:
                player.HP = Math.Min(player.MaxHP, player.HP + item.EffectValue);
                player.Inventory.Remove(item);
                Console.WriteLine($"{item.Name}을(를) 사용해 HP를 회복했습니다!");
                break;
        }
        Console.ReadKey();
    }
    public enum ItemType
    {
        Weapon,
        Armor,
        Consumable
    }

    public class Item
    {
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public int EffectValue { get; set; }  // 공격력/방어력/회복량 등

        public int Price { get; set; }
        public int SellPrice { get; set; }
        public int STR { get; set; }
        public int DEX { get; set; }
        public int INT { get; set; }
        public int DEF { get; set; }
    }

}

