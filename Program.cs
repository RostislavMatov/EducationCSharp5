using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            class Vzaimodeistia
            {
                class Player
                {
                    //static int PlayerAttack(ref Enemy enemy, Player player)
                    //{
                    //}

                    //static int PlayerDefense(string Name)
                    //{
                    //    return
                    //}

                    //static int PlayerTakingDamage(string Name)
                    //{
                    //    return
                    //}
                }
                class Enemy
                {
                    //static int EnemyAttack(string Name)
                    //{
                    //    return
                    //}

                    //static int EnemyGainingExperience(string Name)
                    //{
                    //    return
                    //}

                    //static int EnemyDefense(string Name)
                    //{
                    //    return
                    //}

                    //static int EnemyTakingDamage(string Name)
                    //{
                    //    return;
                    //}
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
                    menu = new BottomMenu[3];

                    BottomMenu Exit = new BottomMenu();
                    Exit.Name = "exit";
                    Exit.Bottom = 0;

                    BottomMenu Save = new BottomMenu();
                    Save.Name = "save";
                    Save.Bottom = 9;

                    BottomMenu NewGame = new BottomMenu();
                    Save.Name = "New game";
                    Save.Bottom = 1;

                    BottomMenu СontinueBottom = new BottomMenu();
                    Save.Name = "Сontinue";
                    Save.Bottom = 2;

                    menu[0] = Exit;
                    menu[1] = Save;
                    menu[2] = NewGame;
                    menu[3] = СontinueBottom;

                }

                public static void WindowMenu(Menu menuObj)
                {
                    Console.WriteLine("___________Меню игры___________");
                    Console.WriteLine("Нажмите________________Действие");
                    for (int i = 0; i < menuObj.menu.Length; i++)
                    {
                        Console.WriteLine($"{menuObj.menu[i].Bottom}__________________________{menuObj.menu[i].Name}");
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
    }

