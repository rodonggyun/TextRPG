﻿using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Town
{
    public void Enter(Player player) // 마을에 입장하는 메소드
    {
        while (true)
        {
            int selected = MenuSelector.Select("마을에 오신 것을 환영합니다!", new List<string>
            {
                "상점에 들어간다",
                "호텔에서 휴식한다",
                "대장간에 간다",
                "인벤토리 열기",
                "던전 입구로 이동",
                "마을을 떠난다"
            }, true);

            Console.Clear();

            if (selected == -1 || selected == 5)
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
                case 4:  // 던전 입구 이동
                    Dungeon dungeon = new Dungeon();
                    dungeon.Enter(player);
                    break;
            }

            Console.WriteLine("\n아무 키나 눌러 계속...");
            Console.ReadKey();
        }
    }
    private List<Item> ShopItems = new List<Item> // 상점에서 판매하는 아이템 목록(추가하고싶으면 여기서)
    {
         new Item("체력 회복 포션", ItemType.Consumable, 100, 50, "HP")
        
    };
    private void EnterShop(Player player) // 상점에 입장하는 메소드
    {
        while (true)
        {
            int selected = MenuSelector.Select($"[상점] 보유 골드: {player.Gold}G\n무엇을 하시겠습니까?", new List<string>
        {
            "아이템 구매", "아이템 판매", "나가기"
        }, true);

            Console.Clear();
            if (selected == -1 || selected == 2) break;

            switch (selected)
            {
                case 0: BuyItemFromShop(player, ShopItems); break; // 상점에서 아이템 구매
                case 1: SellConsumable(player); break;             // 상점에서 아이템 판매 
            }
        }
    }
    private void BuyItemFromShop(Player player, List<Item> items) // 상점에서 소모품 구매 메소드
    {
        List<Item> buyable = new List<Item>(); // 구매 가능한 아이템 목록
        List<string> options = new List<string>(); // 옵션 목록

        foreach (var item in items) 
        {
            buyable.Add(item); // 상점 아이템을 구매 가능한 목록에 추가
            string desc = item.Type == ItemType.Consumable
                ? $"{item.Name} [가격:{item.Price}G] (효과: {item.EffectType} +{item.EffectValue})" 
                : $"{item.Name} [가격:{item.Price}G]";
            options.Add(desc); // 옵션 목록에 아이템 설명 추가
        }

        int selected = MenuSelector.Select($"구매할 아이템을 선택하세요", options, true);
        if (selected == -1) return;

        var itemToBuy = buyable[selected]; // 선택한 아이템
        if (player.Gold < itemToBuy.Price) // 골드가 부족한 경우
        {
            Console.WriteLine("골드가 부족합니다!");
        }
        else // 골드가 충분한 경우
        {
            player.Gold -= itemToBuy.Price;
            player.Inventory.Add(new Item(itemToBuy.Name, itemToBuy.Type, itemToBuy.Price, effectValue: itemToBuy.EffectValue, effectType: itemToBuy.EffectType));
            Console.WriteLine($"{itemToBuy.Name}을(를) 구매했습니다! 남은 골드: {player.Gold}G");
        }
        Console.ReadKey();
    }

    private void SellConsumable(Player player) // 상점에서 소모품 판매 메소드
    {
        var sellables = player.Inventory.FindAll(i => i.Type == ItemType.Consumable); // 판매 가능한 소모품 목록
        if (sellables.Count == 0) // 판매 가능한 소모품이 없는 경우
        {
            Console.WriteLine("판매할 소모품이 없습니다.");
            Console.ReadKey();
            return;
        }

        List<string> options = new List<string>(); // 옵션 목록
        foreach (var item in sellables) // 판매 가능한 소모품을 옵션 목록에 추가
            options.Add($"{item.Name} [판매가:{item.SellPrice}G] (효과: {item.EffectType} +{item.EffectValue})");

        int selected = MenuSelector.Select($"판매할 소모품을 선택하세요", options, true); 
        if (selected == -1) return;

        var itemToSell = sellables[selected]; 
        player.Gold += itemToSell.SellPrice; 
        player.Inventory.Remove(itemToSell); 
        Console.WriteLine($"{itemToSell.Name}을(를) 판매했습니다! 현재 골드: {player.Gold}G");
        Console.ReadKey();
    }


    private void RestAtHotel(Player player) // 호텔에서 휴식하는 메소드
    {
        int cost = (int)(player.Gold * 0.1); // 전체 소지금의 10%를 숙박료로 설정
        if (cost <= 0)
        {
            Console.WriteLine("[호텔] 소지금이 너무 적어 호텔에서 쉴 수 없습니다.");
            return;
        }
        Console.WriteLine($"[호텔] 숙박료는 전체 소지금의 10% ({cost}G) 입니다.");
        Console.Write("숙박하시겠습니까? (Y/N): ");
        var input = Console.ReadLine();
        if (input.ToLower() != "y")
        {
            Console.WriteLine("호텔 이용을 취소했습니다.");
            return;
        }

        if (player.Gold < cost)
        {
            Console.WriteLine("골드가 부족하여 호텔을 이용할 수 없습니다.");
            return;
        }

        player.Gold -= cost;
        player.HP = player.MaxHP;
        Console.WriteLine($"{player.Name} 님이 {cost}G를 내고 호텔에서 휴식, HP를 모두 회복했습니다!");
        Console.WriteLine($"남은 골드: {player.Gold}G");
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
        new Item("목공용망치", ItemType.Weapon, 30, str:1),
        new Item("수련용 해머", ItemType.Weapon, 50, str:2),
        new Item("모험자의 방패", ItemType.Weapon, 100, str:3),
        new Item("성기사의 대검", ItemType.Weapon, 200, str:5),
        new Item("교단장의 건틀랫", ItemType.Weapon, 400, str:8),
        new Item("끝없는 희망", ItemType.Weapon, 800, str:12),
        new Item("충만한 광휘", ItemType.Weapon, 1600, str:18),
        new Item("혹서의 손길", ItemType.Weapon, 3200, str:25),
        new Item("태양불꽃 망토", ItemType.Weapon, 5000, str:35)
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
    private List<Item> GetJobWeaponList(string job) // 직업에 따라 무기 목록을 반환하는 메소드
    {
        switch (job) 
        {
            case "전사": return WarriorWeapons;
            case "도적": return ThiefWeapons;
            case "마법사": return MageWeapons;
            case "성기사": return PaladinWeapons;
            default: return new List<Item>(); // 알 수 없는 직업일 경우 빈 목록 반환
        }
    }
    private void EnterForge(Player player) // 대장간에 입장하는 메소드
    {
        while (true) 
        {
            int selected = MenuSelector.Select($"[대장간] 보유 골드: {player.Gold}G\n무엇을 하시겠습니까?", new List<string>
        {
            "무기 구매", "방어구 구매", "장비 판매", "무기 강화", "나가기"
        }, true);

            Console.Clear();
            if (selected == -1 || selected == 4) break;

            switch (selected)
            {
                case 0: BuyItem(player, GetJobWeaponList(player.Job)); break; // 직업에 맞는 무기 구매
                case 1: BuyItem(player, ArmorList); break;                    // 방어구 구매
                case 2: SellItem(player); break;                              // 장비 판매
                case 3: UpgradeWeapon(player); break;                         // 무기 강화
            }
        }
    }
    private void BuyItem(Player player, List<Item> items) // 아이템 구매 메소드
    {
        
        List<Item> buyable = new List<Item>(); 
        List<string> options = new List<string>();

        foreach (var item in items) // 아이템 목록을 순회하며
        {
            
            bool alreadyOwned = player.Inventory.Exists(i => i.Name == item.Name); // 이미 소유한 아이템인지 확인
            if (!alreadyOwned)// 구매 가능한 아이템만 추가
            {
                buyable.Add(item);
                options.Add($"{item.Name} [가격:{item.Price}G] (STR:{item.STR} DEX:{item.DEX} INT:{item.INT} DEF:{item.DEF})");
            }
        }

        if (buyable.Count == 0) // 구매 가능한 아이템이 없는 경우
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

    private void SellItem(Player player) // 장비 판매 메소드
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

    public void OpenInventory(Player player) // 인벤토리 열기 메소드
    {

        int selected = 0; 
        ConsoleKey key;

        while (true) 
        {

            // 무기, 방어구, 소모품을 각각 분류하여 정렬
            var weapons = player.Inventory.FindAll(i => i.Type == ItemType.Weapon).OrderBy(i => i.Name).ToList();
            var armors = player.Inventory.FindAll(i => i.Type == ItemType.Armor).OrderBy(i => i.Name).ToList();
            var consumables = player.Inventory.FindAll(i => i.Type == ItemType.Consumable).OrderBy(i => i.Name).ToList();

            List<Item> allItems = new List<Item>(); // 모든 아이템을 하나의 리스트로 합침
            allItems.AddRange(weapons);
            allItems.AddRange(armors);
            allItems.AddRange(consumables);

            int total = allItems.Count; // 전체 아이템 수
            if (total == 0)
            {
                Console.Clear();
                Console.WriteLine("인벤토리가 비었습니다.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            

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

            Console.WriteLine("\n[소모품]");
            foreach (var c in consumables)
            {
                if (selected == idx)
                    Console.Write("> ");
                else
                    Console.Write("  ");
                Console.WriteLine($"{c.Name} (효과: {c.EffectType} +{c.EffectValue})");
                idx++;
            }

            Console.WriteLine($"\n[현재 장착]");
            Console.WriteLine($"무기: {(player.EquippedWeapon?.Name ?? "없음")}");
            Console.WriteLine($"방어구: {(player.EquippedArmor?.Name ?? "없음")}");
            Console.WriteLine();
            Console.WriteLine("[방향키 ↑↓로 이동, Z: 장착/해제/사용, X: 나가기]\n");

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
                else if (selected < weapons.Count + armors.Count)
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
                else
                {
                    // 소모품 사용
                    var c = consumables[selected - weapons.Count - armors.Count];
                    if (c.EffectType == "HP")
                    {
                        player.HP = Math.Min(player.MaxHP, player.HP + c.EffectValue);
                        Console.WriteLine($"{c.Name}을(를) 사용해 HP {c.EffectValue}만큼 회복!");
                    }
                    
                    player.Inventory.Remove(c);
                    Console.ReadKey();

                    if (selected >= player.Inventory.Count)
                        selected = Math.Max(0, player.Inventory.Count - 1);
                }
                
            }
            else if (key == ConsoleKey.X)
            {
                break;
            }
        }
    }
    private void UpgradeWeapon(Player player) // 무기 강화 메소드
    {
        // 무기만 골라서 정렬
        var weapons = player.Inventory.FindAll(i => i.Type == ItemType.Weapon);
        if (weapons.Count == 0)
        {
            Console.WriteLine("강화할 무기가 없습니다.");
            Console.ReadKey();
            return;
        }

        List<string> options = new List<string>(); 
        foreach (var w in weapons)
            options.Add($"{(w.UpgradeLevel > 0 ? $"+{w.UpgradeLevel} " : "")}{w.Name} (STR:{w.STR} DEX:{w.DEX} INT:{w.INT} DEF:{w.DEF})");

        int selected = MenuSelector.Select("강화할 무기를 선택하세요", options, true);
        if (selected == -1) return;

        var weapon = weapons[selected];

        if (weapon.UpgradeLevel >= 10)
        {
            Console.WriteLine("최대 +10강입니다.");
            Console.ReadKey();
            return;
        }

        if (player.Gold < 1000)
        {
            Console.WriteLine("골드가 부족합니다! (1000G 필요)");
            Console.ReadKey();
            return;
        }

        int nextUpgrade = weapon.UpgradeLevel + 1;
        int upgradeChance = Math.Max(1, 16 - nextUpgrade); // +1강: 15%, ... +10강: 1%
        int destroyChance = 0;
        if (nextUpgrade >= 5)
            destroyChance = 2 * (nextUpgrade - 4); // +5:2%, +6:4%, ... +10:12%

        Console.WriteLine($"\n[{weapon.Name}]");
        Console.WriteLine($"+{weapon.UpgradeLevel} → +{nextUpgrade} 강화 시도");
        Console.WriteLine();
        Console.WriteLine($"강화 확률: {upgradeChance}% / 파괴 확률: {destroyChance}%");
        Console.WriteLine($"강화 성공 시 능력치가 {nextUpgrade * nextUpgrade}만큼 증가");
        Console.WriteLine();
        Console.WriteLine("강화에는 1000골드가 필요합니다.");
        Console.WriteLine();
        Console.WriteLine("Z: 강화 시도, X: 취소");

        var key = Console.ReadKey(true).Key;
        if (key != ConsoleKey.Z) return;

        player.Gold -= 1000;
        Random rand = new Random();
        int roll = rand.Next(1, 101);

        if (roll <= upgradeChance)
        {
            weapon.UpgradeLevel++;
            // 강화 능력치 적용(원래 스탯에 덮어쓰기/누적 중 택1: 여기서는 누적 적용)
            if (weapon.STR > 0) weapon.STR = weapon.STR + (weapon.UpgradeLevel * weapon.UpgradeLevel - (weapon.UpgradeLevel - 1) * (weapon.UpgradeLevel - 1)); 
            if (weapon.DEX > 0) weapon.DEX = weapon.DEX + (weapon.UpgradeLevel * weapon.UpgradeLevel - (weapon.UpgradeLevel - 1) * (weapon.UpgradeLevel - 1));
            if (weapon.INT > 0) weapon.INT = weapon.INT + (weapon.UpgradeLevel * weapon.UpgradeLevel - (weapon.UpgradeLevel - 1) * (weapon.UpgradeLevel - 1));
            if (weapon.DEF > 0) weapon.DEF = weapon.DEF + (weapon.UpgradeLevel * weapon.UpgradeLevel - (weapon.UpgradeLevel - 1) * (weapon.UpgradeLevel - 1));

            Console.WriteLine($"강화 성공! [{weapon.Name}]이(가) +{weapon.UpgradeLevel}강이 되었습니다!");
        }
        else if (destroyChance > 0 && rand.Next(1, 101) <= destroyChance)
        {
            Console.WriteLine("강화에 실패하고 무기가 파괴되었습니다!");
            player.Inventory.Remove(weapon);
        }
        else
        {
            Console.WriteLine("강화에 실패했습니다. 무기는 안전합니다.");
        }
        Console.ReadKey();
    }


    public enum ItemType
    {
        Weapon,
        Armor,
        Consumable
    }

    public class Item // 아이템 클래스
    {
        public string Name { get; set; } 
        public ItemType Type { get; set; }
        public int EffectValue { get; set; } // 효과 값 (예: HP 회복량, MP 회복량 등)

        public string EffectType { get; set; }  // "HP" 또는 "MP"
        public int UpgradeLevel { get; set; } = 0; // 무기 강화 레벨 (0부터 시작, 최대 10까지 가능)
        public int Price { get; set; } // 아이템 구매 가격
        public int SellPrice { get; set; } // 아이템 판매 가격 (구매 가격의 70%로 설정)
        public int STR { get; set; }
        public int DEX { get; set; }
        public int INT { get; set; }
        public int DEF { get; set; }
        public Item() { } // 기본 생성자

        public Item(string name, ItemType type, int price, int str = 0, int dex = 0, int intel = 0, int def = 0) // 아이템 생성자 (무기/방어구용)
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
        public Item(string name, ItemType type, int price, int effectValue, string effectType) // 아이템 생성자 (소모품용)
        {
            Name = name;
            Type = type;
            Price = price;
            SellPrice = (int)(price * 0.7);
            EffectValue = effectValue;
            EffectType = effectType; 
        }
    }

}

