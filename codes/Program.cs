using ConsoleApp3;

namespace ConsoleApp3
{
    internal class Program
    {
        static void Main()
        {
            string[] options = { "전투한다", "도망친다", "아이템 사용" }; //선택할 수 있는 항목
            int choice = ShowMenu(options);


            // 선택 결과 출력
            Console.WriteLine($"\n당신은 '{options[choice]}'를 선택했습니다!");


            string[] options2 = { "안녕하세요", "테스트입니다", "연속으로도", "뜬답니다", "확인차원" };
            int choice2 = ShowMenu(options2);

            Console.WriteLine($"\n당신은 '{options2[choice2]}'를 선택했습니다!");
            Console.WriteLine("아무 키나 눌러 종료하세요...");
            Console.ReadKey();
        }

        static int ShowMenu(string[] options) {
            int selected = 0; //현재 선택된 인덱스, 초기값

            ConsoleKey key; //키보드 입력 저장할 변수
            do //한번은 무조건 실행
            {
                Console.Clear(); //반복마다 화면 깨끗하게 지우기
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
                ConsoleKeyInfo keyInfo = Console.ReadKey(true); //사용자가 누른 키를 가져옴, true는 입력된키를 화면에 보이지 않도록 처리
                key = keyInfo.Key;

                if (key == ConsoleKey.UpArrow) //누른 부분이 방향키 위라면
                {
                    selected--; //인덱스 감소시켜서 위 메뉴로 이동
                    if (selected < 0) selected = options.Length - 1; //0일때 누르면 마지막 인덱스 위치로 변경
                }
                else if (key == ConsoleKey.DownArrow) //아래키를 누르면
                {
                    selected++; //인덱스++
                    if (selected >= options.Length) selected = 0; //마지막일때 누르면 맨 위 인덱스로 이동
                }

            } while (key != ConsoleKey.Enter && key != ConsoleKey.Spacebar); //key가 엔터가 아니고, space바도 아님.

            return selected;
        }
    }
}
