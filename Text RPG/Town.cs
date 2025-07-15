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
    // 전사 무기
    private List<Item> WarriorWeapons = new List<Item>
    {
        new Item("나무막대기", ItemType.Weapon, 30, str:1),
        new Item("수련용 목검", ItemType.Weapon, 50, str:2),
        new Item("모험자의 검", ItemType.Weapon, 100, str:3),
        new Item("기사의 장검", ItemType.Weapon, 200, str:5),
        new Item("영웅의 외날검", ItemType.Weapon, 400, str:8),
        new Item("순백의 양날도끼", ItemType.Weapon, 800, str:12),
        new Item("부흥한 왕의 검", ItemType.Weapon, 1600, str:18),
        new Item("배부른 히드라", ItemType.Weapon, 3200, str:25),
        new Item("발가락 분쇄기", ItemType.Weapon, 5000, str:35)
    };
    // 도적 무기
    private List<Item> ThiefWeapons = new List<Item>
    {
        new Item("나무젓가락", ItemType.Weapon, 30, dex:1),
        new Item("수련용 나무단검", ItemType.Weapon, 50, dex:2),
        new Item("모험자의 단검", ItemType.Weapon, 100, dex:3),
        new Item("쿠나이", ItemType.Weapon, 200, dex:5),
        new Item("어쌔신의 암살검", ItemType.Weapon, 400, dex:8),
        new Item("천둥폭풍검", ItemType.Weapon, 800, dex:12),
        new Item("요우무의 유령검", ItemType.Weapon, 1600, dex:18),
        new Item("뱀의 송곳니", ItemType.Weapon, 3200, dex:25),
        new Item("변칙의 원형낫", ItemType.Weapon, 5000, dex:35)
    };
    // 마법사 무기
    private List<Item> MageWeapons = new List<Item>
    {
        new Item("찢어진 스크롤", ItemType.Weapon, 30, intel:1),
        new Item("수련용 스태프", ItemType.Weapon, 50, intel:2),
        new Item("모험자의 지팡이", ItemType.Weapon, 100, intel:3),
        new Item("전투마법서", ItemType.Weapon, 200, intel:5),
        new Item("대마법사의 마도서", ItemType.Weapon, 400, intel:8),
        new Item("모렐노로미콘", ItemType.Weapon, 800, intel:12),
        new Item("찰나의 지팡이", ItemType.Weapon, 1600, intel:18),
        new Item("수평선의 초점", ItemType.Weapon, 3200, intel:25),
        new Item("빛불꽃 횃불", ItemType.Weapon, 5000, intel:35)
    };
    // 성기사 무기
    private List<Item> PaladinWeapons = new List<Item>
    {
        new Item("목공용망치", ItemType.Weapon, 30, def:1),
        new Item("수련용 해머", ItemType.Weapon, 50, def:2),
        new Item("모험자의 방패", ItemType.Weapon, 100, def:3),
        new Item("성기사의 대검", ItemType.Weapon, 200, def:5),
        new Item("교단장의 건틀랫", ItemType.Weapon, 400, def:8),
        new Item("끝없는 희망", ItemType.Weapon, 800, def:12),
        new Item("충만한 광휘", ItemType.Weapon, 1600, def:18),
        new Item("혹서의 손길", ItemType.Weapon, 3200, def:25),
        new Item("태양불꽃 망토", ItemType.Weapon, 5000, def:35)
    };
    // 공통 방어구
    private List<Item> ArmorList = new List<Item>
    {
        new Item("천갑옷", ItemType.Armor, 60, def:2),
        new Item("쇠사슬 조끼", ItemType.Armor, 120, def:4),
        new Item("수정 팔 보호구", ItemType.Armor, 250, def:7),
        new Item("비상의 월갑", ItemType.Armor, 500, def:11),
        new Item("파수꾼의 갑옷", ItemType.Armor, 1000, def:16),
        new Item("군단의 방패", ItemType.Armor, 2000, def:23),
        new Item("음전자 망토", ItemType.Armor, 3500, def:32),
        new Item("거인의 허리띠", ItemType.Armor, 6000, def:45)
    };
    private List<Item> GetJobWeaponList(string job)
    {
        switch (job)
        {
            case "전사": return WarriorWeapons;
            case "도적": return ThiefWeapons;
            case "마법사": return MageWeapons;
            case "성기사": return PaladinWeapons;
            default: return new List<Item>();
        }
    }
    private void EnterForge(Player player)
    {
        while (true)
        {
            int selected = MenuSelector.Select($"[대장간] 보유 골드: {player.Gold}G\n무엇을 하시겠습니까?", new List<string>
        {
            "무기 구매", "방어구 구매", "장비 판매", "나가기"
        }, true);

            Console.Clear();
            if (selected == -1 || selected == 3) break;

            switch (selected)
            {
                case 0: BuyItem(player, GetJobWeaponList(player.Job)); break;
                case 1: BuyItem(player, ArmorList); break;
                case 2: SellItem(player); break;
            }
        }
    }
    private void BuyItem(Player player, List<Item> items)
    {
        
        List<Item> buyable = new List<Item>();
        List<string> options = new List<string>();

        foreach (var item in items)
        {
            
            bool alreadyOwned = player.Inventory.Exists(i => i.Name == item.Name);
            if (!alreadyOwned)
            {
                buyable.Add(item);
                options.Add($"{item.Name} [가격:{item.Price}G] (STR:{item.STR} DEX:{item.DEX} INT:{item.INT} DEF:{item.DEF})");
            }
        }

        if (buyable.Count == 0)
        {
            Console.WriteLine("구매 가능한 아이템이 없습니다! (이미 모두 소유함)");
            Console.ReadKey();
            return;
        }

        int selected = MenuSelector.Select($"[보유 골드: {player.Gold}G]\n구매할 아이템을 선택하세요", options, true);
        if (selected == -1) return;

        var itemToBuy = buyable[selected];

        if (player.Gold < itemToBuy.Price)
        {
            Console.WriteLine("골드가 부족합니다!");
        }
        else
        {
            player.Gold -= itemToBuy.Price;
            player.Inventory.Add(itemToBuy);
            Console.WriteLine($"{itemToBuy.Name}을(를) 구매했습니다! 남은 골드: {player.Gold}G");
        }
        Console.ReadKey();
    }

    private void SellItem(Player player)
    {
        var sellables = player.Inventory.FindAll(i => i.Type == ItemType.Weapon || i.Type == ItemType.Armor);
        if (sellables.Count == 0)
        {
            Console.WriteLine("판매할 장비가 없습니다.");
            Console.ReadKey();
            return;
        }

        List<string> options = new List<string>();
        foreach (var item in sellables)
            options.Add($"{item.Name} [판매가:{item.SellPrice}G] (STR:{item.STR} DEX:{item.DEX} INT:{item.INT} DEF:{item.DEF})");

        int selected = MenuSelector.Select($"[보유 골드: {player.Gold}G]\n판매할 장비를 선택하세요", options, true);
        if (selected == -1) return;

        var itemToSell = sellables[selected];
        player.Gold += itemToSell.SellPrice;
        player.Inventory.Remove(itemToSell);
        Console.WriteLine($"{itemToSell.Name}을(를) 판매했습니다! 현재 골드: {player.Gold}G");
        Console.ReadKey();
    }

    private void OpenInventory(Player player)
    {
        // 무기/방어구로 분리와 정렬
        var weapons = player.Inventory.FindAll(i => i.Type == ItemType.Weapon).OrderBy(i => i.Name).ToList();
        var armors = player.Inventory.FindAll(i => i.Type == ItemType.Armor).OrderBy(i => i.Name).ToList();

        List<Item> allItems = new List<Item>();
        allItems.AddRange(weapons);
        allItems.AddRange(armors);

        int total = allItems.Count;
        if (total == 0)
        {
            Console.Clear();
            Console.WriteLine("🎒 인벤토리가 비었습니다.");
            Console.ReadKey();
            return;
        }

        int selected = 0;
        ConsoleKey key;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("🎒 [인벤토리 - 방향키 ↑↓로 이동, Z: 장착/해제, X: 나가기]\n");

            int idx = 0;
            Console.WriteLine("[무기]");
            foreach (var w in weapons)
            {
                bool isEquipped = player.EquippedWeapon == w;
                if (selected == idx)
                    Console.Write($"> ");
                else
                    Console.Write("  ");
                Console.WriteLine($"{w.Name} (STR:{w.STR} DEX:{w.DEX} INT:{w.INT} DEF:{w.DEF}){(isEquipped ? " [장착중]" : "")}");
                idx++;
            }

            Console.WriteLine("\n[방어구]");
            foreach (var a in armors)
            {
                bool isEquipped = player.EquippedArmor == a;
                if (selected == idx)
                    Console.Write($"> ");
                else
                    Console.Write("  ");
                Console.WriteLine($"{a.Name} (STR:{a.STR} DEX:{a.DEX} INT:{a.INT} DEF:{a.DEF}){(isEquipped ? " [장착중]" : "")}");
                idx++;
            }

            Console.WriteLine($"\n[현재 장착]");
            Console.WriteLine($"무기: {(player.EquippedWeapon?.Name ?? "없음")}");
            Console.WriteLine($"방어구: {(player.EquippedArmor?.Name ?? "없음")}");
            Console.WriteLine("\nX 입력 시 인벤토리 닫기");

            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow)
                selected = (selected - 1 + total) % total;
            else if (key == ConsoleKey.DownArrow)
                selected = (selected + 1) % total;
            else if (key == ConsoleKey.Z)
            {
                if (selected < weapons.Count)
                {
                    var w = weapons[selected];
                    if (player.EquippedWeapon == w)
                    {
                        player.EquippedWeapon = null;
                        Console.WriteLine($"{w.Name} 무기 장착 해제!");
                    }
                    else
                    {
                        player.EquippedWeapon = w;
                        Console.WriteLine($"{w.Name} 무기 장착!");
                    }
                }
                else
                {
                    var a = armors[selected - weapons.Count];
                    if (player.EquippedArmor == a)
                    {
                        player.EquippedArmor = null;
                        Console.WriteLine($"{a.Name} 방어구 장착 해제!");
                    }
                    else
                    {
                        player.EquippedArmor = a;
                        Console.WriteLine($"{a.Name} 방어구 장착!");
                    }
                }
                Console.ReadKey();
            }
            else if (key == ConsoleKey.X)
            {
                break;
            }
        }
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
        public int EffectValue { get; set; }  

        public int Price { get; set; }
        public int SellPrice { get; set; }
        public int STR { get; set; }
        public int DEX { get; set; }
        public int INT { get; set; }
        public int DEF { get; set; }
        public Item() { }

        public Item(string name, ItemType type, int price, int str = 0, int dex = 0, int intel = 0, int def = 0)
        {
            Name = name;
            Type = type;
            Price = price;
            SellPrice = (int)(price * 0.7);
            STR = str;
            DEX = dex;
            INT = intel;
            DEF = def;
        }
    }

}

