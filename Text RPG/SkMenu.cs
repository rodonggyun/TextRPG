using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_RPG;

namespace Text_RPG
{
    internal class SkMenu
    {
        public void UseSkillMenu(Player player, MonsterInfo enemy)
        {
            List<Skill> availableSkills = new List<Skill>(); //스킬 사용이 가능한 스킬들 모은 리스트

            foreach (Skill s in player.Skills) //플레이어 스킬들 순차대입
            {
                if (s != null) 
                {
                    // 나중에 쿨타임, 잠김 조건 등도 여기에 추가 가능
                    availableSkills.Add(s); //리스트에 사용가능한 스킬 추가
                }
            }
            if (availableSkills.Count == 0) //스킬이 하나도 없다면
            {
                Console.WriteLine("\n아직 배운 스킬이 없습니다.\n");
            }

            List<string> skillNames = new List<string>(); //스킬 보여줄 수 있도록 만든 str 리스트
            foreach (var s in availableSkills) //스킬 이름들 추가
            {
                skillNames.Add($"{s.Name} ({s.Description})");
            }

            skillNames.Add("일반 공격 선택");

            int selection = ShowMenu.ShowMenus(skillNames);
            if (selection >= 0)
            {
                if (selection == availableSkills.Count)
                { //마지막은 일반 공격이니깐
                    Random rand = new Random();
                    double variation = rand.NextDouble() * 0.2 - 0.1;  // 오차범위 +-10%. -0.1 ~ +0.1
                    double rawDamage = player.AttackPower * (1 + variation);
                    int finalDamage = (int)Math.Ceiling(rawDamage); //올림

                    int damage = player.AttackPower;
                    enemy.hp -= damage;
                    Console.WriteLine($"\n▶ 일반 공격! {enemy.name}에게 {damage}의 피해를 입혔습니다!");
                }
                else {
                    Console.WriteLine("사용할 스킬을 선택하세요!\n");
                    Skill selectedSkill = availableSkills[selection]; //선택한 스킬
                    Console.WriteLine($"\n▶ {selectedSkill.Name} 스킬을 사용합니다!");
                    //선택한 스킬 효과 적용할 코드 필요함
                }

                if (enemy.hp <= 0) {
                    enemy.dead = true;
                    Console.WriteLine($"\n{enemy.name}을(를) 처치했습니다!\n");
                    player.Gold += enemy.gold;
                    Console.WriteLine($"{enemy.gold}의 골드를 획득했습니다!");
                    player.GainExp(enemy.exp); //경험치 획득 출력, 레벨업까지 있음
                    enemy.hp = 0;
                }
                Console.WriteLine("계속하려면 아무 키나 누르세요...");
                Console.ReadKey(true);
                Console.Clear();
            }
        }
    }
}
