using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG
{
    class MonsterInfo
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

    // 전투 및 선택지 관련 클래스
    static class Dungeon
    {
        static List<MonsterInfo> monsterList = new List<MonsterInfo>() {
            new MonsterInfo() {type=1, name="슬라임", hp=100, level=1, exp=10, dead=false, gold=10, atk=1},
            new MonsterInfo() {type=2, name="미니언", hp=100, level=3, exp=30, dead=false, gold=30, atk=3},
            new MonsterInfo() {type=3, name="대포미니언", hp=100, level=5, exp=50, dead=false, gold=20, atk=10}
        };


        static void Battle()
        {
            Console.WriteLine("Battle!!\n\n");

            for (int i = 0; i < monsterList.Count; i++)
            {
                Console.WriteLine($"{monsterList[i].level} {monsterList[i].name}\t HP:{monsterList[i].level}");
            }

            Console.WriteLine("[내 정보]");
            //상태창 가져오기

            string[] options = { "공격한다.", "도망간다.", "춤을 춘다." };
            int choice = ShowMenu(options);

            Console.WriteLine($"\n당신은 '{options[choice]}'를 선택했습니다!");

            if (choice == 0)
            { //공격한다 선택
                Random rand = new Random;
                int monsterCount = rand.Next(1, 5); //한 번에 나오는 몬스터는 1~4마리(랜덤)

                List<MonsterInfo> appearMonsters = new List<MonsterInfo>(); //출현 몬스터들을 새 리스트로 생성

                for (int i = 0; i < monsterCount;) {
                    int index = rand.Next(monsterList.Count); //몬스터 랜덤 등장

                }
            }
        }




        static int ShowMenu(string[] options) //방향키 움직이는 함수
        {
            int selected = 0;

            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("무엇을 할까요?\n");

                // 선택지 출력
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selected)
                        Console.WriteLine("▶ " + options[i]);
                    else
                        Console.WriteLine("  " + options[i]);
                }

                // 키 입력 감지
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                key = keyInfo.Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selected--;
                    if (selected < 0) selected = options.Length - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selected++;
                    if (selected >= options.Length) selected = 0;
                }

            } while (key != ConsoleKey.Enter && key != ConsoleKey.Spacebar); //z키로 확인할거면 수정 필요

            return selected;
        }
    }
}