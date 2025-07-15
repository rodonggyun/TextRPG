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
            if (player.Skills.Count == 0)
            {
                Console.WriteLine("아직 배운 스킬이 없습니다.");
                return;
            }

            List<string> skillNames = new List<string>();
            foreach (var s in player.Skills)
                skillNames.Add($"{s.Name} ({s.Description})");

            int selection = MenuSelector.Select("사용할 스킬을 선택하세요!", skillNames, true);
            if (selection >= 0)
            {
                var skill = player.Skills[selection];
                //skill.Effect(player, enemy);
                // 쿨다운 처리 추가 가능
            }
        }
    }
}
