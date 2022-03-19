using Microsoft.Data.SqlClient;
using roomMates.Models;
using System;
using System.Collections.Generic;


namespace roomMates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }
        public List<Chore> GetAll()
        {

            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id,Name FROM Chore";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> chores = new List<Chore>();
                        while (reader.Read())
                        {
                            // The "ordinal" is the numeric position of the column in the query results.
                            //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                            int idColumnPosition = reader.GetOrdinal("Id");

                            // We user the reader's GetXXX methods to get the value for a particular ordinal.
                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue,

                            };

                            // ...and add that room object to our list.
                            chores.Add(chore);
                        }

                        return chores;
                    }

                }

            }

        }

        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Chore chore = null;

                        // If we only expect a single row back from the database, we don't need a while loop.
                        if (reader.Read())
                        {
                            chore = new Chore
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name")),

                            };
                        }
                        return chore;
                    }

                }
            }


        }

        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);

                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }


        }

        public List<Chore> GetUnassignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {



                    cmd.CommandText = @"select * from Chore c left Join RoommateChore rc 
                            on c.id = rc.choreId
                            Where rc.RoommateId is null";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> choreList = new List<Chore>();

                        while (reader.Read())
                        {
                            Chore unassignedChore = new Chore()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            };
                            choreList.Add(unassignedChore);
                        }
                        return choreList;
                    }
                }


            }

        }

        // assign chore with a roommate
        public void AssignChore(int roommateId, int choreId)
        {
            //same issue with delete 
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO RoommateChore (ChoreId, RoommateId)
                                        VALUES (@ChoreId, @RoommateId)";
                    cmd.Parameters.AddWithValue("@ChoreId", choreId);
                    cmd.Parameters.AddWithValue("@RoommateId", roommateId);
                    cmd.ExecuteNonQuery();


                }




            }



        }
        // update chore database
        public void Update(Chore chore)
        {

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Chore
                                      SET Name = @name
                                      WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    cmd.Parameters.AddWithValue("@id", chore.Id);

                    cmd.ExecuteNonQuery();
                }
            }


        }
        //delete a chore
        public void delete(int id) 
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();


                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //when we delete a chore with a roommate assigned to it we get an error that it is conflicted with the REFERENCE constraint. How would we fix this issue?
                    cmd.CommandText = "DELETE FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }


        }
    }
}
       
