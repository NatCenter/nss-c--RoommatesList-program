using roomMates.Models;
using roomMates.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace roomMates
{

    class Program
    {
       
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);



            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());
                         
                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                        //this will do somthing later.
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores) 
                        {
                            Console.WriteLine($"{c.Id} -{c.Name} ");
                        
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();

                        break;

                    case ("Search for a chore"):
                        Console.Write("Chore Id: ");
                        int choreId = int.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetById(choreId);

                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        string choreName = Console.ReadLine();          

                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName,
                            
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for a roommate"):
                       Console.Write("Roommate Id: ");
                        int RoommateId = int.Parse(Console.ReadLine());

                        Roommate roommates = roommateRepo.GetById(RoommateId);

                        Console.WriteLine($"{roommates.Id} - {roommates.FirstName}-{roommates.LastName}-{roommates.RentPortion}-{roommates.Room.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();


                        break;
                    case ("See all unassigned Chores"):
                        List<Chore> choreList = choreRepo.GetUnassignedChores();
                        foreach (Chore c in choreList)
                        {
                            Console.WriteLine($"{c.Name} is not assigned to a roommate");

                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Assign chore to roommate"):
                       
                        Console.WriteLine("Select what chore that you want to assign");

                        List<Chore> chores1 = choreRepo.GetAll();
                        foreach (Chore c in chores1)
                        {
                            
                            Console.WriteLine($" {c.Id}-{c.Name} ");

                        }
                        int selectChoreId = int.Parse(Console.ReadLine());


                        
                        List<Roommate> RoommateList = roommateRepo.GetAll();
                        Console.WriteLine("Select what what roommate you want to assign the chore to.");
                        foreach (Roommate r in RoommateList)
                        {
                            Console.WriteLine($"{ r.Id} -{r.FirstName} -{r.LastName}");
                        }
                        int selectRoommateId = int.Parse(Console.ReadLine());
                        choreRepo.AssignChore(selectChoreId, selectRoommateId);
                        Console.WriteLine($"Success : {roommateRepo.GetById(selectRoommateId).FirstName} has been assigned to {choreRepo.GetById(selectChoreId).Name}");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        

                        break;
                    case ("Update Room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete Room"):
                        List<Room> listOfRooms = roomRepo.GetAll();
                        foreach (Room r in listOfRooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }

                        Console.WriteLine("Select room that you want to delete by Id");
                        int roomChoice = int.Parse(Console.ReadLine());
                        roomRepo.Delete(roomChoice);

                        Console.WriteLine("Room has been Successfuly deleted");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update Chore"):


                        List<Chore> choreOptions = choreRepo.GetAll();
                        foreach (Chore c in choreOptions)
                        {
                            Console.WriteLine($"Id:{c.Id} Name of chore {c.Name}");
                        }

                        Console.Write("Which chore would you like to update? ");
                        int userChoiceChore = int.Parse(Console.ReadLine());
                        Chore selectChorebyId = choreOptions.FirstOrDefault(c => c.Id == userChoiceChore);

                        Console.Write("New Name: ");
                        selectChorebyId.Name = Console.ReadLine();

                        

                        choreRepo.Update(selectChorebyId);

                        Console.WriteLine("Chore  has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete Chore"):
                        List<Chore> listOfChores = choreRepo.GetAll();
                        foreach (Chore c in listOfChores) 
                        {
                            Console.WriteLine($"{c.Name} has an Id of : {c.Id}");
                        }
                        Console.WriteLine("Which chore would you like to delete");
                        int selectedChoreId=int.Parse(Console.ReadLine());
                        choreRepo.delete(selectedChoreId);
                        Console.WriteLine("You have deleted a chore");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                     case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Show all chores",
                "Search for a chore",
                "Add a chore",
                "Search for a roommate",
                "See all unassigned Chores",
                "Assign chore to roommate",
                "Update Room",
                "Delete Room",
                "Update Chore",
                "Delete Chore",

                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}