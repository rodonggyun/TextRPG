﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Text_RPG;


public class MonsterInfo
{
    public int type;
    public string name;
    public int hp;
    public int maxHp;
    public int level;
    public int exp;
    public bool dead;
    public int gold;
    public int atk;
}

public class BattleInfo
{
    public bool myTurn = true; //플레이어 차례, 기본값 true
    public bool eTurn = false; //몬스터 차례
}

// 전투 및 선택지 관련 클래스
public class Dungeon
{
    Town inventory = new Town();

    public void Enter(Player player)
    {
        while (true)
        {
            int selected = MenuSelector.Select("이곳은 던전 입구입니다.", new List<string>
            {
                "타워에 들어간다",
                "인벤토리 열기",
                "마을로 돌아간다"
            }, true);

            Console.Clear();

            if (selected == -1 || selected == 2)
            {
                Console.WriteLine("마을로 돌아갑니다...");
                break;
            }

            switch (selected)
            {
                case 0:
                    Battle(player);
                    break;
                case 1:
                    inventory.OpenInventory(player);
                    break;
            }

            Console.WriteLine("\n아무 키나 눌러 계속...");
            Console.ReadKey(true);
        }
    }

    List<MonsterInfo> monsterList = new List<MonsterInfo>();
    List<MonsterInfo> appearMonsters = new List<MonsterInfo>();

    BattleInfo battleInfo = new BattleInfo();
    
    BossManager bossManager = new BossManager();
    Town town = new Town();

    public Dungeon()
    { //몬스터 리스트 생성자, 생성자에서 초기화
        monsterList.Add(CreateMonster(1, "슬라임", 5, 1, 1));  //슬라임 레벨 1일때 체력 20부터 등장
        monsterList.Add(CreateMonster(2, "고블린", 6, 1, 1)); //고블린 1레벨은 체력 15부터 등장
        monsterList.Add(CreateMonster(3, "오크", 7, 1, 1));
    }

    MonsterInfo CreateMonster(int type, string name, int baseHp, int minLevel, int maxLevel)
    { //몬스터 생성
        Random rand = new Random();
        int level = rand.Next(minLevel, maxLevel + 1); //레벨은 최소 레벨 ~ 최대 레벨 랜덤 범위
        int damageMultiplier = rand.Next(1, 4); //데미지 범위 1~3배
        int atk = level * damageMultiplier;

        int findHp = baseHp + level * 10; //몬스터 최종 체력

        return new MonsterInfo() //객체 생성+초기화
        {
            type = type,
            name = name,
            hp = findHp,
            maxHp = findHp, //최대체력도 초기화. 1레벨이면 hp+10, 2레벨이면 hp+20, ...
            level = level,
            exp = level * 20,
            gold = level * 5,
            atk = atk,
            dead = false
        };
    }

    void Battle(Player player) //전투 화면
    {
        Console.Clear();
        MonsterAppear(player);

        while (true)
        {
            Console.Clear();

            if (appearMonsters.All(m => m.dead))
            { //몬스터가 다 죽었으면
                BattleEnd(player); //전투 종료
                break;
            }

            MonsterPrint(player);
            PlayerInfo(player);

            int selected = ShowMenu.ShowMenus(new List<string>
            {
                "공격한다.", "도망간다."
            });

            if (selected == 0)
            {
                PlayerPhase(player);
            }
            else if (selected == 1)
            {
                Run();
                break;
            }
        }
    }

    void MonsterAppear(Player player)
    {
        appearMonsters.Clear();  // 이전 몬스터들 삭제

        int floor = player.Floor;
        BossInfo boss = bossManager.GetBossByFloor(floor);   // 몇층인지 확인

        int minLevel = 1;
        int maxLevel = 1;
        if (floor >= 1 && floor <= 5) //1~5층이라면 몬스터 1~3레벨로 등장
        {
            minLevel = 1;
            maxLevel = 3;
        }
        else if (floor > 5 && floor <= 10)
        {
            minLevel = 2;
            maxLevel = 5;
        }
        else if (floor > 10)
        { //10층 이상부터는 몬스터 4~10레벨
            minLevel = 4;
            maxLevel = 10;
        }


        // 보스 몬스터 생성
        if (boss != null)
        {
            appearMonsters.Add(boss);  // 층에 맞는 보스 출현
            Console.WriteLine($"{floor}층의 보스 \"{boss.name}\"가 나타났습니다!");
            Console.WriteLine($"Lv.{boss.level} {boss.name} (HP: {boss.hp})");
            return; // 일반 몬스터 생성 스킵
        }

        // 일반 몬스터 생성
        else
        {
            Random rand = new Random();
            int monsterCount = rand.Next(1, 5); //한 번에 나오는 몬스터는 1~4마리(랜덤)

            for (int i = 0; i < monsterCount; i++)
            { //뽑은 숫자만큼
                int index = rand.Next(monsterList.Count); //몬스터 랜덤 등장
                MonsterInfo original = monsterList[index]; //몬스터는 리스트에서 무작위로 가져옴.(얘는 복사한 객체가 아니라 원본 몬스터임)

                MonsterInfo clone = CreateMonster( //몬스터 복제해서 생성
                    original.type,
                    original.name,
                    original.hp,
                    minLevel,
                    maxLevel);

                appearMonsters.Add(clone); //출현 몬스터 리스트에 복제한 값 넣기
            }
            MonsterPrint(player);
        }
    }

    void MonsterPrint(Player player)
    { //등장한 몬스터 출력
        Console.WriteLine($"▶ 몬스터가 나타났다! 무엇을 하시겠습니까?  ({player.Floor}층)\n");
        for (int i = 0; i < appearMonsters.Count; i++)
        {
            MonsterInfo m = appearMonsters[i];  //appearMonsters의 자료형이 MonsterInfo라 m 앞에 붙임
            if (m.dead)
                Console.ForegroundColor = ConsoleColor.DarkGray; // 죽은 몬스터는 회색으로
            else
                Console.ResetColor(); // 살아있는 몬스터는 기본색
            Console.WriteLine($"Lv.{m.level} {m.name} (HP: {m.hp}/{m.maxHp})");
        }
        Console.ResetColor(); // 색상 초기화 (다른 곳에 영향 안 주도록)
    }

    void PlayerInfo(Player player)
    {
        Console.WriteLine("\n\n[내 정보]");
        Console.WriteLine($"[{player.Name} - {player.Job}] (Lv. {player.Level})");
        Console.WriteLine($"HP: {player.HP}/{player.MaxHP} | EXP: {player.Exp}/{player.ExpToLevel} | Gold : {player.Gold}");
        Console.WriteLine($"공격력: {player.AttackPower} | 회피율: {player.Evasion} | 방어력: {player.Defense}\n");
    }


    void PlayerPhase(Player player)
    { //플레이어 턴
        Console.Clear();

        List<string> monsterOptions = new List<string>(); //화면 출력용 문자열로 변환
        List<MonsterInfo> livingMonsters = new List<MonsterInfo>(); //실제로 살아있는 몬스터들
        for (int i = 0; i < appearMonsters.Count; i++)
        {
            MonsterInfo m = appearMonsters[i];
            if (!m.dead) //살아있는 몬스터라면
            {
                monsterOptions.Add($"Lv.{m.level} {m.name} (HP: {m.hp}/{m.maxHp})"); // 화면에 출력
                livingMonsters.Add(m); //살아있는 몬스터 리스트에 추가
            }
        }
        Console.WriteLine("공격할 대상을 선택하세요!\n");
        int selected = ShowMenu.ShowMenus(monsterOptions);

        if (selected >= 0 && selected < livingMonsters.Count) //선택한 몬스터가 범위 내라면
        {
            MonsterInfo target = livingMonsters[selected]; //살아있는 몬스터 선택
            if (!target.dead) //몬스터가 살아있다면(혹시모를 예외처리용)
            {
                SkMenu skMenu = new SkMenu();
                skMenu.UseSkillMenu(player, target); //선택한 몬스터만 공격(스킬사용)
                battleInfo.myTurn = false;
                battleInfo.eTurn = true;
                EnemyPhase(player); //턴 교체
            }
            else
            {
                Console.WriteLine("\n해당 몬스터는 이미 죽었습니다.");
                Console.WriteLine("\n아무 키나 눌러 계속...");
                Console.ReadKey(true);
            }

            if (player.HP <= 0)
            {
                BattleEnd(player);
            }
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
        }
    }

    void EnemyPhase(Player player)
    {
        if (!battleInfo.myTurn && battleInfo.eTurn)
        {
            foreach (var monster in appearMonsters)   // 생성된 몬스터 하나하나씩 턴 주기
            {
                if (monster.dead) continue;           // foreach에서 몬스터 죽어 있으면 continue로 건너뛰고 다른 몬스터 턴 시작

                Console.WriteLine("▶ 몬스터의 차례입니다!\n");
                Console.WriteLine($"Lv.{monster.level} {monster.name}의 공격!");

                // 보스 여부 확인 및 스킬 사용
                if (monster is BossInfo boss)
                {
                    // MaxHp가 있어야 함 (생성 시 저장해둬야 함)
                    if (boss.hp <= boss.maxHp * 20 / 100)
                    {
                        boss.UseSpecialSkill(player);
                        Console.WriteLine($"\n[{player.Name}]의 HP: {player.HP}");
                        Console.WriteLine("계속하려면 아무 키나 누르세요...");
                        Console.ReadKey(true);
                        Console.Clear();
                        continue;
                    }
                }

                // 일반 몬스터의 기본 공격
                Console.WriteLine($"{monster.atk}만큼의 데미지를 입었습니다.\n");
                Console.WriteLine($"\n[{player.Name} - {player.Job}] (Lv. {player.Level})");
                Console.Write($"HP: {player.HP}");
                player.HP -= monster.atk;
                Console.WriteLine($" -> {player.HP}\n");

                Console.WriteLine("계속하려면 아무 키나 누르세요...");
                Console.ReadKey(true);
                Console.Clear();
            }

            battleInfo.eTurn = false;
            battleInfo.myTurn = true;
        }
    }

    void Run()
    {
        Console.Clear();
        Console.WriteLine("당신은 전투에서 도망쳤습니다!");
        Console.WriteLine("의지가 꺾이는 기분이 듭니다...\n");
        Console.WriteLine("당신의 자존감 -10\n");
        Console.ReadKey(true); //키 입력 시 넘어감. 던전 입구로 돌아감
    }

    void BattleEnd(Player player)
    { //전투 종료
        Console.Clear();
        if (player.HP <= 0)
        {
            Console.WriteLine("당신은 사망했습니다.");

            Console.WriteLine($"\n[{player.Name} - {player.Job}] (Lv. {player.Level})");
            Console.WriteLine($"HP: {player.HP}/{player.MaxHP} | EXP: {player.Exp}/{player.ExpToLevel}\n");

            player.HP = 10;

            Console.WriteLine("계속하려면 아무 키나 누르세요...");
            Console.ReadKey(true); //키 입력 대기, true는 입력문자 안 보이게

            town.Enter(player);
        }
        else if (player.HP > 0)
        {
            if (appearMonsters.All(m => m.dead))
            {
                Console.WriteLine($"▶ {player.Floor++}층의 몬스터를 전부 토벌했다!");

                Console.WriteLine($"\n[{player.Name} - {player.Job}] (Lv. {player.Level} | Gold : {player.Gold})");
                Console.WriteLine($"HP: {player.HP}/{player.MaxHP} | EXP: {player.Exp}/{player.ExpToLevel}\n");

                int selected = ShowMenu.ShowMenus(new List<string>
                {
                    "계속 탑을 오른다.", "마을로 돌아간다."
                });

                if (selected == 0)
                {
                    Battle(player);
                }
                else if (selected == 1)
                {
                    town.Enter(player);
                }
            }
        }
    }
}