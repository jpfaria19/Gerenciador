using ATNET.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace ATNET.Repository
{
    public class PersonRepository
    {
        string strConString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = '" + AppDomain.CurrentDomain.BaseDirectory + "App_Data\\Clientes.mdf'; Integrated Security = True";

        public void AddPerson(string name, string surName, DateTime birthday)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                //con.Open();
                string query = "INSERT INTO dbo.people (name, surname, birth_date) VALUES (@n, @sn, @bd)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@sn", surName);
                cmd.Parameters.AddWithValue("@bd", birthday);
                cmd.ExecuteNonQuery();
            }
        }
        
        public List<Person> GetPeople()
        {
            List<Person> people = new List<Person>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.people", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Person p = new Person()
                        {
                            id = reader.GetInt32(0),
                            name = reader.GetString(1),
                            surName = reader.GetString(2),
                            birthday = reader.GetDateTime(3)
                        };
                        people.Add(p);
                    }
                }
            }

            return people;
        }


        public Person GetPerson(int personId)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.people WHERE Id = @i", con);
                cmd.Parameters.AddWithValue("@i", personId);
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        return new Person()
                        {
                            id = reader.GetInt32(0),
                            name = reader.GetString(1),
                            surName = reader.GetString(2),
                            birthday = reader.GetDateTime(3)
                        };
                }
            }

            return null;
        }

        public List<Person> GetBirthdayPeople()
        {
            List<Person> people = new List<Person>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM people WHERE DAY(birth_date) = DAY(getdate()) AND MONTH(birth_date) = MONTH(getdate())", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        people.Add(new Person()
                        {
                            id = reader.GetInt32(0),
                            name = reader.GetString(1),
                            surName = reader.GetString(2),
                            birthday = reader.GetDateTime(3)
                        });
                }
            }

            return people;
        }

        public List<Person> GetCloseBirthdays()
        {
            List<Person> people = new List<Person>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT *" 
                                            + " FROM people WHERE MONTH(birth_date) = MONTH(getdate())"
                                            + " AND ABS(DATEDIFF(day, DAY(birth_date), DAY(getdate()))) < 10"
                                            + " AND DAY(birth_date) > DAY(getdate()) ORDER BY DAY(birth_date) ASC", con);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        people.Add(new Person()
                        {
                            id = reader.GetInt32(0),
                            name = reader.GetString(1),
                            surName = reader.GetString(2),
                            birthday = reader.GetDateTime(3)
                        });
                }
            }

            return people;
        }

        public List<Person> SearchByName(string name)
        {
            List<Person> people = new List<Person>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM people WHERE name LIKE '%' + @n + '%' OR surname LIKE '%' + @n + '%' ", con);
                cmd.Parameters.AddWithValue("@n", name);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        people.Add(new Person()
                        {
                            id = reader.GetInt32(0),
                            name = reader.GetString(1),
                            surName = reader.GetString(2),
                            birthday = reader.GetDateTime(3)
                        });
                }
            }

            return people;
        }

        public void UpdatePerson(Person p)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "UPDATE dbo.people SET name = @n, surname = @sn, birth_date = @bd WHERE Id = @i";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@i", p.id);
                cmd.Parameters.AddWithValue("@n", p.name);
                cmd.Parameters.AddWithValue("@sn", p.surName);
                cmd.Parameters.AddWithValue("@bd", p.birthday);
                cmd.ExecuteNonQuery();
            }
        }

        public void RemovePerson(int personId)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "DELETE FROM dbo.people WHERE Id = @i";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@i", personId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}