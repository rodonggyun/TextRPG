using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG
{
    public class BossInfo : MonsterInfo
    {
        public string SpecialSkill { get; set; }

        public BossInfo(int type, string name, int baseHp, int level, string specialSkill)
        {
            this.type = type;
            this.name = name;
            this.level = level;
            this.hp = baseHp + level * 15;
            this.atk = level * 4;
            this.exp = level * 50;
            this.gold = level * 20;
            this.dead = false;
            this.SpecialSkill = specialSkill;
        }

        public void UseSpecialSkill(Player player)
        {
            int damage = atk * 2;
            Console.WriteLine($"\n{name}의 특수기 [{SpecialSkill}] 발동! {damage} 데미지!");
            player.HP -= damage;
        }
    }

    public class BossManager
    {
        public List<BossInfo> BossList { get; private set; }

        public BossManager()
        {
            BossList = new List<BossInfo>
            {
            new BossInfo(97, "대왕 슬라임", 50, 10, "부식의 침"),
            new BossInfo(98, "황금 고블린", 100, 20, "부의 저주"),
            new BossInfo(99, "블랙 오크", 150, 30, "지옥 참격")
            };
        }

    }
}
