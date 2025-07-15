using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_RPG;


public class MonsterInfo
{
    public int type;
    public string name;
    public int hp;
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
    public int floor = 1;      // 타워 층
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
            Console.ReadKey();
        }
    }

    /* main 함수에서
    Dungeon dungeon = new Dungeon();
    dungeon.Battle();
    으로 함수 호출 */

    List<MonsterInfo> monsterList = new List<MonsterInfo>();
    List<MonsterInfo> appearMonsters = new List<MonsterInfo>();

    BattleInfo battleInfo = new BattleInfo();
    Player player = new Player();
    BossManager bossManager = new BossManager();


    public Dungeon()
    { //몬스터 리스트 생성자, 생성자에서 초기화
        monsterList.Add(CreateMonster(1, "슬라임", 10, 1, 4));  //슬라임 레벨 1일때 체력 20부터 등장
        monsterList.Add(CreateMonster(2, "고블린", 15, 1, 6)); //고블린 1레벨은 체력 15부터 등장
        monsterList.Add(CreateMonster(3, "오크", 20, 2, 7));
    }

    MonsterInfo CreateMonster(int type, string name, int baseHp, int minLevel, int maxLevel)
    { //몬스터 생성
        Random rand = new Random();
        int level = rand.Next(minLevel, maxLevel + 1); //레벨은 최소 레벨 ~ 최대 레벨 랜덤 범위

        return new MonsterInfo() //객체 생성+초기화
        {
            type = type,
            name = name,
            hp = baseHp + level * 10, //1레벨이면 hp+10, 2레벨이면 hp+20, ...
            level = level,
            exp = level * 10,
            gold = level * 5,
            atk = level * 2,
            dead = false
        };
    }

    void Battle(Player player) //전투 화면
    {
        while (true)
        {
            Console.Clear();
            MonsterAppear();
            PlayerInfo();

            int selected = ShowMenu.ShowMenus(new List<string>
            {
                "공격한다.", "도망간다."
            });

            if (selected == 0)
            {
                PlayerPhase();
            }
            else if (selected == 1)
            {
                Run();
                break;
            }
        }
    }

    void MonsterAppear()
    { //몬스터 랜덤 등장
        appearMonsters.Clear();  // 이전 몬스터들 삭제

        if (battleInfo.floor == 10)
        {
            BossInfo boss = bossManager.GetBossByFloor(battleInfo.floor);

            if (boss != null)
            {
                appearMonsters.Add(boss);  // 보스를 출현시킴
                Console.WriteLine($"10층의 보스 \"{boss.name}\"가 나타났습니다!");
                Console.WriteLine($"Lv.{boss.level} {boss.name} (HP: {boss.hp})");
            }
        }

        else if (battleInfo.floor == 20)
        {
            BossInfo boss = bossManager.GetBossByFloor(battleInfo.floor);

            if (boss != null)
            {
                appearMonsters.Add(boss);  // 보스를 출현시킴
                Console.WriteLine($"20층의 보스 \"{boss.name}\"가 나타났습니다!");
                Console.WriteLine($"Lv.{boss.level} {boss.name} (HP: {boss.hp})");
            }
        }

        else if (battleInfo.floor == 30)
        {
            BossInfo boss = bossManager.GetBossByFloor(battleInfo.floor);

            if (boss != null)
            {
                appearMonsters.Add(boss);  // 보스를 출현시킴
                Console.WriteLine($"30층의 보스 \"{boss.name}\" 가 나타났습니다!");
                Console.WriteLine($"Lv.{boss.level} {boss.name} (HP: {boss.hp})");
            }
        }

        else
        {
            Random rand = new Random();
            int monsterCount = rand.Next(1, 5); //한 번에 나오는 몬스터는 1~4마리(랜덤)

            for (int i = 0; i < monsterCount; i++)
            { //뽑은 숫자만큼
                int index = rand.Next(monsterList.Count); //몬스터 랜덤 등장
                MonsterInfo original = monsterList[index]; //몬스터는 리스트에서 무작위로 가져옴.(얘는 복사한 객체가 아니라 원본 몬스터임)

                MonsterInfo clone = new MonsterInfo() //원본 몬스터에게 피해 안 가게끔 깊은 복사
                {
                    type = original.type,
                    name = original.name,
                    hp = original.hp,
                    level = original.level,
                    exp = original.exp,
                    dead = false,
                    gold = original.gold,
                    atk = original.atk
                };

                appearMonsters.Add(clone); //출현 몬스터 리스트에 복제한 값 넣기
            }
            MonsterPrint();
        } 
    }

    void MonsterPrint()
    { //등장한 몬스터 출력
        Console.WriteLine("몬스터가 나타났다! 무엇을 하시겠습니까?\n");
        for (int i = 0; i < appearMonsters.Count; i++)
        {
            MonsterInfo m = appearMonsters[i];  //appearMonsters의 자료형이 MonsterInfo라 m 앞에 붙임
            Console.WriteLine($"Lv.{m.level} {m.name} (HP: {m.hp})");
        }
    }

    void PlayerInfo()
    {
        Console.WriteLine("\n[내 정보]");
        Console.WriteLine($"\n[{player.Name} - {player.Job}] (Lv. {player.Level})");
        Console.WriteLine($"HP: {player.HP}/{player.MaxHP}");
        Console.WriteLine($"EXP: {player.Exp}/{player.ExpToLevel} | 남은 스탯 포인트: {player.StatPoints}");
        Console.WriteLine($"공격력: {player.AttackPower} | 회피율: {player.Evasion} | 방어력: {player.Defense}\n");

    }


    void PlayerPhase()
    { //플레이어 턴
        Console.Clear();

        List<string> monsterOptions = new List<string>(); //화면 출력용 문자열로 변환
        for (int i = 0; i < appearMonsters.Count; i++)
        {
            MonsterInfo m = appearMonsters[i];
            monsterOptions.Add($"Lv.{m.level} {m.name} (HP: {m.hp})");
        }
        Console.WriteLine("공격할 대상을 선택하세요!\n");
        int selected = ShowMenu.ShowMenus(monsterOptions);

        if (selected >= 0 && selected < appearMonsters.Count) //선택한 몬스터가 범위 내라면(혹시 모를 예외처리)
        {
            MonsterInfo target = appearMonsters[selected]; //몬스터 선택
            if (!target.dead) //몬스터가 살아있다면
            {
                SkMenu skMenu = new SkMenu();
                skMenu.UseSkillMenu(player, target); //선택한 몬스터만 공격(스킬사용)
            }
            else
            {
                Console.WriteLine("해당 몬스터는 이미 죽었습니다.");
            }
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
        }
        //때릴 수 있는 몬스터 선택
        //후 현재 보유중인 스킬목록 출력
        //이후 스킬 선택
        //공격
        //전투 종료 메서드 출력
    }

    void EnemyPhase()
    {
        if (!battleInfo.myTurn && battleInfo.eTurn) //몬스터 차례일 때
        {
            for (int i = 0; i < appearMonsters.Count; i++)
            {
                if (!appearMonsters[i].dead) //등장한 몬스터가 죽어있지 않을 때
                {
                    Console.WriteLine($"Battle!!\n");
                    Console.WriteLine($"Lv.{appearMonsters[i].level} {appearMonsters[i].name}의 공격!");
                    Console.WriteLine($"{appearMonsters[i].atk}만큼의 데미지가 달았습니다.\n");
                    Console.WriteLine($"\n[{player.Name} - {player.Job}] (Lv. {player.Level})");
                    Console.Write($"HP: {player.HP}");
                    player.HP -= appearMonsters[i].atk;
                    Console.WriteLine($" -> {player.HP}\n");
                    Console.WriteLine("계속하려면 아무 키나 누르세요...");
                    Console.ReadKey(true); //키 입력 대기, true는 입력문자 안 보이게
                }
                battleInfo.eTurn = false;
                battleInfo.myTurn = true;
            }
        }
    }

    void Run()
    {
        Console.Clear();
        Console.WriteLine("당신은 전투에서 도망쳤습니다!");
        Console.WriteLine("의지가 꺾이는 기분이 듭니다...\n");
        Console.WriteLine("당신의 자존감 -10\n");
        Console.ReadKey(true);
        //Enter(player); 이거 player 어떤값을 받는 건가요? 마을로 이동하려는데
    }

    void BattleEnd()
    { //전투 종료
        //마을로 돌아가기, 전투 계속하기 선택 가능
        //전투 계속하기 누르면 Battle() 메서드 호출
        //추후 탑 층수 올라가는 변수, 방 고르는 함수 필요함
    }
}