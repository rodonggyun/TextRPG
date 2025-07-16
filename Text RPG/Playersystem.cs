using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_RPG;
using static Text_RPG.Skill;
using static Town;


public class Player
{
    public int TotalSTR => STR
    + (EquippedWeapon != null ? EquippedWeapon.STR : 0)
    + (EquippedArmor != null ? EquippedArmor.STR : 0);

    public int TotalDEX => DEX
        + (EquippedWeapon != null ? EquippedWeapon.DEX : 0)
        + (EquippedArmor != null ? EquippedArmor.DEX : 0);

    public int TotalINT => INT
        + (EquippedWeapon != null ? EquippedWeapon.INT : 0)
        + (EquippedArmor != null ? EquippedArmor.INT : 0);

    public int TotalDEF => DEF
        + (EquippedWeapon != null ? EquippedWeapon.DEF : 0)
        + (EquippedArmor != null ? EquippedArmor.DEF : 0);
    public string Name { get; set; }
    public string Job { get; set; }
    public string SubJob;
    public string ThirdJob;
    public List<Skill> Skills = new List<Skill>();
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public int STR { get; set; }
    public int DEX { get; set; }
    public int INT { get; set; }
    public int DEF { get; set; }
    public int Level { get; set; } = 1;
    public int Exp { get; set; } = 0;
    public int StatPoints { get; set; } = 0;
    public DateTime SaveTime { get; set; } = DateTime.Now;
    public int ExpToLevel => Level * 20;
    public int Gold { get; set; } = 500; // 골드긴한데 원으로 바꿀까요 ??

    public List<Item> Inventory = new List<Item>();
    public Item EquippedWeapon = null;
    public Item EquippedArmor = null;

    public string FormatStat(string label, int baseVal, int bonus)
    {
        return bonus > 0 ? $"{label}: {baseVal} (+{bonus})" : $"{label}: {baseVal}";
    }
    public void DisplayStats(bool allowStatAllocation = false)
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine($"\n[{Name} - {Job}] (Lv. {Level})");
            Console.WriteLine($"HP: {HP}/{MaxHP}");
            Console.WriteLine($"{FormatStat("STR", STR, TotalSTR - STR)} | " +
                              $"{FormatStat("DEX", DEX, TotalDEX - DEX)} | " +
                              $"{FormatStat("INT", INT, TotalINT - INT)} | " +
                              $"{FormatStat("DEF", DEF, TotalDEF - DEF)}");
            Console.WriteLine($"EXP: {Exp}/{ExpToLevel} | 남은 스탯 포인트: {StatPoints}");
            Console.WriteLine($"▶ 공격력: {AttackPower} | 회피율: {Evasion} | 방어력: {Defense}");
            Console.WriteLine($"▶ 경험치 보정률: {ExpBonus * 100:F0}%");
            Console.WriteLine($"마지막 저장: {SaveTime:yyyy-MM-dd HH:mm:ss}");

            if (!allowStatAllocation || StatPoints <= 0)
            {
                Console.WriteLine("\n아무 키나 누르면 계속...");
                Console.ReadKey(true);
                break;
            }

            { 
                int sel = MenuSelector.Select("\n올릴 스탯을 선택하세요", new List<string> { "STR", "DEX", "INT", "DEF" }, false);

                switch (sel)
                {
                    case 0: STR++; break;
                    case 1: DEX++; break;
                    case 2: INT++; break;
                    case 3: DEF++; break;
                }

                StatPoints--;
            }
        }
    }

    public void GainExp(int baseAmount)
    {
        int actualAmount = (int)(baseAmount * ExpBonus); // 보정 적용
        Exp += actualAmount;

        Console.WriteLine($"{baseAmount}의 경험치를 획득했습니다! (보정 적용: {actualAmount})");

        while (Exp >= ExpToLevel)
        {
            Exp -= ExpToLevel;
            Level++;
            StatPoints += 5;
            Console.WriteLine($"레벨이 {Level}로 올랐습니다! 스탯 포인트 +5");
        }
    }
    public void AllocateStatPoints()
    {
        List<string> statOptions = new List<string>
    {
        "STR (힘)", "DEX (민첩)", "INT (지능)", "DEF (방어)"
    };

        while (StatPoints > 0)
        {
            Console.Clear();
            Console.WriteLine($"남은 스탯 포인트: {StatPoints}\n");
            DisplayStats();

            int selected = MenuSelector.Select("올릴 스탯을 선택하세요", statOptions, false);

            switch (selected)
            {
                case 0: STR++; break;
                case 1: DEX++; break;
                case 2: INT++; break;
                case 3: DEF++; break;
            }

            StatPoints--;
        }

        Console.Clear();
        Console.WriteLine("스탯 분배를 완료했습니다.");
        Console.WriteLine("아무 키나 누르면 계속...");
        Console.ReadKey(true);
    }


    public int AttackPower
    {
        get
        {
            return Job switch
            {
                "전사" => TotalSTR,
                "도적" => TotalDEX,
                "마법사" => TotalINT,
                "성기사" => TotalSTR,   
                _ => 0
            };
        }
    }

    public int Evasion
    {
        get
        {
            return Job switch
            {
                "전사" => DEX,
                "도적" => INT,
                "마법사" => DEX,
                "성기사" => DEX,
                _ => 0
            };
        }
    }

    public double ExpBonus
    {
        get
        {
            int stat = Job switch
            {
                "전사" => INT,
                "도적" => STR,
                "마법사" => STR,
                "성기사" => INT,
                _ => 0
            };
            return 1.0 + (stat * 0.1);
        }
    }

    public int Defense => TotalDEF;

    public void CheckForSecondJobChange()
    {
        if (Level >= 10 && SubJob == null)
        {
            var options = GetSecondJobOptions(Job);
            int selection = MenuSelector.Select($"{Job}의 2차 전직을 선택하세요!", options);
            if (selection >= 0)
            {
                SubJob = options[selection];
                Console.WriteLine($"\n▶ 당신은 {SubJob}으로 전직했습니다!\n");
                ApplySecondJobBonus(SubJob);
            }
        }
    }

    public void CheckForThirdJobChange()
    {
        if (Level >= 20 && ThirdJob == null && SubJob != null)
        {
            var options = GetThirdJobOptions(SubJob);
            int selection = MenuSelector.Select($"{SubJob}의 3차 전직을 선택하세요!", options);
            if (selection >= 0)
            {
                ThirdJob = options[selection];
                Console.WriteLine($"\n▶ 당신은 {ThirdJob}으로 3차 전직했습니다!!\n");
                ApplyThirdJobBonus(ThirdJob);
            }
        }
    }

    private List<string> GetSecondJobOptions(string job)
    {
        return job switch
        {
            "전사" => new List<string> { "버서커", "가디언" },
            "도적" => new List<string> { "어쌔신", "섀도우댄서" },
            "마법사" => new List<string> { "아크메이지", "소서러" },
            "성기사" => new List<string> { "크루세이더", "템플러" },
            _ => new List<string>()
        };
    }

    private List<string> GetThirdJobOptions(string subJob)
    {
        return subJob switch
        {
            "버서커" => new List<string> { "블러드나이트" },
            "가디언" => new List<string> { "아이언월" },
            "어쌔신" => new List<string> { "나이트크로우" },
            "섀도우댄서" => new List<string> { "팬텀댄서" },
            "아크메이지" => new List<string> { "엘더위치" },
            "소서러" => new List<string> { "디멘터" },
            "크루세이더" => new List<string> { "팔라딘" },
            "템플러" => new List<string> { "세인트" },
            _ => new List<string>()
        };
    }

    private void ApplySecondJobBonus(string subJob)
    {
        switch (subJob)
        {
            case "버서커": STR += 10;
                UnlockSkill(new Skill("광폭화", "3턴간 공격력 2배", 5, (self, enemy) =>
                {
                    Console.WriteLine("▶ 광폭화! 3턴 동안 공격력이 2배가 됩니다!");
                    // 상태 버프 처리 필요
                })); 
                break;

            case "가디언": DEF += 10;
                UnlockSkill(new Skill("철벽", "이번 턴 받는 데미지 0", 3, (self, enemy) =>
                {
                    Console.WriteLine("▶ 철벽 발동! 이번 턴 받는 피해가 0이 됩니다.");
                    // 데미지 무효 처리 필요
                }));
                break;

            case "어쌔신": DEX += 10;
                UnlockSkill(new Skill("급소 찌르기", "치명타 데미지를 입힙니다.", 4, (self, enemy) =>
                {
                    int damage = self.STR * 2;
                    enemy.HP -= damage;
                    Console.WriteLine($"▶ 급소 찌르기! {damage} 데미지를 입혔습니다!");
                })); 
                break;

            case "섀도우댄서": DEX += 5; INT += 5;
                UnlockSkill(new Skill("연막", "회피율을 3턴간 상승시킵니다.", 4, (self, enemy) =>
                {
                    Console.WriteLine("▶ 연막 사용! 회피율이 상승합니다.");
                    // 회피 버프 처리 필요
                })); 
                break;

            case "아크메이지": INT += 10;
                UnlockSkill(new Skill("메테오", "모든 적에게 마법 피해를 줍니다.", 6, (self, enemy) =>
                {
                    int damage = self.INT * 2;
                    enemy.HP -= damage;
                    Console.WriteLine($"▶ 메테오! {damage} 마법 피해를 입혔습니다!");
                })); 
                break;

            case "소서러": INT += 7; DEF += 3;
                UnlockSkill(new Skill("저주", "적의 공격력과 방어력을 감소시킵니다.", 5, (self, enemy) =>
                {
                    Console.WriteLine("▶ 저주 발동! 적의 능력치가 감소합니다.");
                    // 적 능력치 감소 로직 필요
                })); 
                break;

            case "크루세이더": STR += 5; INT += 5;
                UnlockSkill(new Skill("신의 일격", "강력한 성속성 공격", 4, (self, enemy) =>
                {
                    int damage = self.STR + self.INT;
                    enemy.HP -= damage;
                    Console.WriteLine($"▶ 신의 일격! {damage}의 신성 데미지!");
                })); 
                break;

            case "템플러": DEF += 5; INT += 5;
                UnlockSkill(new Skill("빛의 치유", "체력을 30 회복합니다.", 3, (self, enemy) =>
                {
                    self.HP += 30;
                    Console.WriteLine("▶ 빛의 치유로 30 회복했습니다!");
                })); 
                break;
        }
    }
    public void UnlockSkill(Skill skill)
    {
        Skills.Add(skill);
        Console.WriteLine($"▶ 새로운 스킬 '{skill.Name}'을(를) 배웠습니다! - {skill.Description}");
    }
    private void ApplyThirdJobBonus(string thirdJob)
    {
        switch (thirdJob)
        {
            case "블러드나이트": STR += 15; DEF -= 5;
                UnlockSkill(new Skill("피의 분노", "3턴간 공격 시 체력 흡수", 6, (self, enemy) =>
                {
                    Console.WriteLine("▶ 피의 분노 발동! 이제 공격 시 체력을 흡수합니다!");
                    // 버프 상태 등록 필요
                }));
                UnlockSkill(new Skill("【궁극기】피의 폭풍", "모든 적에게 3배 데미지 + 체력 흡수", 10, (self, enemy) =>
                {
                    int damage = self.STR * 3;
                    enemy.HP -= damage;
                    self.HP += damage / 2;
                    Console.WriteLine($"▶ 궁극기 발동! 피의 폭풍으로 {damage} 데미지 + 체력 {damage / 2} 회복!");
                }));
                break;

            case "아이언월": DEF += 15; UnlockSkill(new Skill("철갑 태세", "받는 피해 3턴간 반감", 6, (self, enemy) =>
            {
                Console.WriteLine("▶ 철갑 태세 발동! 피해를 절반만 받습니다!");
                // 버프 상태 등록 필요
            }));
                UnlockSkill(new Skill("【궁극기】절대 방어", "이번 턴 모든 피해 무효 + 적 반격", 10, (self, enemy) =>
                {
                    Console.WriteLine("▶ 궁극기 절대 방어 발동! 이번 턴 무적, 다음 턴 적에게 2배 반격!");
                    // 무적, 반격 효과 처리 필요
                }));
                break;

            case "나이트크로우": DEX += 15;
                INT += 15;
               
                break;

            case "팬텀댄서": INT += 10; DEX += 5;
                break;

            case "엘더위치": INT += 15;
                UnlockSkill(new Skill("시간 왜곡", "적 턴 스킵 + 쿨다운 초기화", 7, (self, enemy) =>
                {
                    Console.WriteLine("▶ 시간 왜곡! 적의 턴을 날리고 스킬 쿨다운이 초기화됩니다.");
                    // 턴 제어, 쿨다운 초기화 필요
                }));
                UnlockSkill(new Skill("【궁극기】대마법 폭격", "모든 적에게 INT x 4 데미지", 12, (self, enemy) =>
                {
                    int damage = self.INT * 4;
                    enemy.HP -= damage;
                    Console.WriteLine($"▶ 궁극기 대마법 폭격! {damage}의 마법 데미지!");
                })); break;

            case "디멘터": INT += 10; DEF += 5; break;
            case "팔라딘": STR += 7; DEF += 7; break;
            case "세인트": INT += 10; HP += 20;
                UnlockSkill(new Skill("부활의 기도", "사망 시 1회 자동 부활", 15, (self, enemy) =>
                {
                    Console.WriteLine("▶ 부활의 기도가 발동됩니다! 사망 시 자동 부활이 1회 저장됩니다.");
                    // 상태 등록
                }));
                UnlockSkill(new Skill("【궁극기】신성광휘", "모든 아군 체력 완전 회복 + 상태이상 해제", 12, (self, enemy) =>
                {
                    Console.WriteLine("▶ 궁극기 신성광휘 발동! 파티 전원 완전 회복!");
                    self.HP = self.MaxHP;
                    // 파티 전체 회복 로직 추가
                }));
                break;
        }
    }
   
}