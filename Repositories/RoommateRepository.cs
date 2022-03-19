using Microsoft.Data.SqlClient;
using roomMates.Models;
using System;
using System.Collections.Generic;

namespace roomMates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT FirstName,RentPortion, r.Name 
                        FROM Roommate rm
                        Join Room r on r.id = rm.RoomId
                        WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        // If we only expect a single row back from the database, we don't need a while loop.
                        if (reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                Room = new Room { Name = reader.GetString(reader.GetOrdinal("Name")) }

                            };
                        }
                        return roommate;
                    }
                }
            }
        }
        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT id,FirstName,LastName FROM Roommate";
                    List<Roommate> RoommateList = new List<Roommate>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            // The "ordinal" is the numeric position of the column in the query results.
                            //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                            int idColumnPosition = reader.GetOrdinal("Id");

                            // We user the reader's GetXXX methods to get the value for a particular ordinal.
                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("FirstName");
                            string nameValue = reader.GetString(nameColumnPosition);
                            int nameColumnPosition1 = reader.GetOrdinal("LastName");
                            string nameValue1 = reader.GetString(nameColumnPosition);



                           
                            Roommate roommates = new Roommate
                            {
                                Id = idValue,
                                FirstName = nameValue,
                                LastName=nameValue1,
                            };

                            RoommateList.Add(roommates);

                            
                        }
                     

                    }
                    return RoommateList;

                }


                
            }
        }
    }
}
    