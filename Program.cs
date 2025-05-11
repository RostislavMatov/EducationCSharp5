using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SqlServer.Server;

namespace EducationCSharp5
{
    internal class Program
    {
        class Player
        {
            public string Name { get; set; }
            public int Health { get; set; }
            public int Strength { get; set; }
            public int Defense { get; set; }
            public int Experience { get; set; }

            private Random random = new Random();

            public static void CreatePlayer(out Player player, string namePlayer)
            {
                player = new Player
                {
                    Name = namePlayer,
                    Health = 10,
                    Strength = 2,
                    Defense = 1,
                    Experience = 0
                };
            }

            public void Attack(Enemy enemy)
            {
                int damage = random.Next(1, Strength + 3) - enemy.Defense;
                if (damage > 0)
                {
                    enemy.Health -= damage;
                    Console.WriteLine($"{Name} атакует {enemy.Name} и наносит {damage} урона!");
                }
                else
                {
                    Console.WriteLine($"{Name} не может нанести урон {enemy.Name} из-за его защиты!");
                }
            }

            public void GainExperience(int amount)
            {
                Experience += amount;
                Console.WriteLine($"{Name} получает {amount} опыта! Текущий опыт: {Experience}");

                // Проверка на возможность повышения уровня (например, каждые 10 очков опыта)
                if (Experience >= 10)
                {
                    LevelUp();
                }
            }

            private void LevelUp()
            {
                Console.WriteLine($"{Name} достиг уровня! Выберите, что улучшить:");
                Console.WriteLine("1. Здоровье (+5)");
                Console.WriteLine("2. Сила (+1)");
                Console.WriteLine("3. Защита (+1)");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Health += 5;
                        break;
                    case "2":
                        Strength += 1;
                        break;
                    case "3":
                        Defense += 1;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        return;
                }

                // Сброс опыта после повышения уровня
                Experience -= 10;

                Console.WriteLine($"Новые характеристики: Здоровье: {Health}, Сила: {Strength}, Защита: {Defense}");
            }
        }

        class Enemy
        {
            public string Name { get; set; }
            public int Health { get; set; }
            public int Strength { get; set; }
            public int Defense { get; set; }

            private Random random = new Random();

            public Enemy(string name, int health, int strength, int defense)
            {
                Name = name;
                Health = health;
                Strength = strength;
                Defense = defense;
            }

            public void Attack(Player player)
            {
                int damage = random.Next(1, Strength + 3) - player.Defense;
                if (damage > 0)
                {
                    player.Health -= damage;
                    Console.WriteLine($"{Name} атакует {player.Name} и наносит {damage} урона!");
                }
                else
                {
                    Console.WriteLine($"{Name} не может нанести урон {player.Name} из-за его защиты!");
                }
            }

            // Метод для улучшения характеристик врага
            public void Improve()
            {
                // Случайный выбор характеристики для улучшения
                Random rand = new Random();
                int improvementType = rand.Next(3); // Генерируем число от 0 до 2

                switch (improvementType)
                {
                    case 0:
                        Health += rand.Next(1, 4); // Увеличиваем здоровье на случайное значение от 1 до 3
                        Console.WriteLine($"{Name} улучшает здоровье! Новое здоровье: {Health}");
                        break;

                    case 1:
                        Strength += rand.Next(1, 2); // Увеличиваем силу на случайное значение от 1 до 1 (т.е. на единицу)
                        Console.WriteLine($"{Name} улучшает силу! Новая сила: {Strength}");
                        break;

                    case 2:
                        Defense += rand.Next(0, 2); // Увеличиваем защиту на случайное значение от 0 до 1 (т.е. может не увеличиться)
                        Console.WriteLine($"{Name} улучшает защиту! Новая защита: {Defense}");
                        break;

                    default:
                        break;
                }
            }
        }

        class SaveLoad
        {
            public static void SavePlayer(Player player)
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string saveFolder = Path.Combine(documentsPath, "MyGame");

                if (!Directory.Exists(saveFolder))
                {
                    Directory.CreateDirectory(saveFolder);
                }

                string saveFilePath = Path.Combine(saveFolder, "player.txt");

                using (StreamWriter writer = new StreamWriter(saveFilePath))
                {
                    writer.WriteLine(player.Name);
                    writer.WriteLine(player.Health);
                    writer.WriteLine(player.Defense);
                    writer.WriteLine(player.Strength);
                    writer.WriteLine(player.Experience);
                }
            }

            public static Player LoadPlayer()
            {

                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string saveFolder = Path.Combine(documentsPath, "MyGame");
                string filePath = Path.Combine(saveFolder, "player.txt");

                if (!File.Exists(filePath))
                    return null;

                string[] lines = File.ReadAllLines(filePath);

                return new Player
                {
                    Name = lines[0],
                    Health = int.Parse(lines[1]),
                    Defense = int.Parse(lines[2]),
                    Strength = int.Parse(lines[3]),
                    Experience = int.Parse(lines[4])
                };
            }
        }

        class Menu
        {
            public BottomMenu[] menu { get; set; }
            public struct BottomMenu
            {
                public string Name { get; set; }
                public int Bottom { get; set; }
            }
            public Menu()
            {
                menu = new BottomMenu[4];

                BottomMenu Exit = new BottomMenu();
                Exit.Name = "exit";
                Exit.Bottom = 4;

                BottomMenu Save = new BottomMenu();
                Save.Name = "save";
                Save.Bottom = 3;

                BottomMenu NewGame = new BottomMenu();
                NewGame.Name = "New game";
                NewGame.Bottom = 2;

                BottomMenu СontinueBottom = new BottomMenu();
                СontinueBottom.Name = "Сontinue";
                СontinueBottom.Bottom = 1;

                menu[0] = СontinueBottom;
                menu[1] = NewGame;
                menu[2] = Save;
                menu[3] = Exit;

            }

            public static void WindowMenu(Menu menuObj)
            {
                Console.WriteLine("            Меню игры            ");
                //Console.WriteLine("Нажмите                  Действие");
                for (int i = 0; i < menuObj.menu.Length; i++)
                {
                    Console.WriteLine($"            {menuObj.menu[i].Bottom}. {menuObj.menu[i].Name}        ");
                }
            }
        }

        static void Main(string[] args)
        {

            Menu menu = new Menu();
            string nomberMenu;
            bool newGame = false;

            Menu.WindowMenu(menu);

            nomberMenu = Console.ReadLine()?.Trim();

            if (int.TryParse(nomberMenu, out int nomberMenuInt))
            {
                if (nomberMenuInt == 0)
                {
                    return;
                }
                else if (nomberMenuInt == 1)
                {
                    newGame = true;
                }
                else if (nomberMenuInt == 2)
                {

                }
                else if (nomberMenuInt == 9)
                {

                }
            }

            List<Player> list = new List<Player>();

            string namePlayer;
            Console.WriteLine("Введите имя игрока: ");
            namePlayer = Console.ReadLine()?.Trim();
            Player player;

            if (string.IsNullOrWhiteSpace(namePlayer))
            {
                player = list.Find(p => p.Name.Equals(namePlayer, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                Player.CreatePlayer(out player, namePlayer);
            }

            Console.WriteLine(player.Name);
            Console.WriteLine(player.Health);

            Menu.WindowMenu(menu);


        }

    }       
}

