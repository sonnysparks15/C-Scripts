// MSSQL DataBase Query Commands

using Movie_Library.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
// ReSharper disable All
namespace Movie_Library.Data;
/* //////////////////////////////////////////////////////////////////////// */

internal class MovieDAO
{
    private string connectionString = @"Server=localhost; Database=MovieDataBase; User=sa; Password=#SonnySparks3;
                                        TrustServerCertificate=True; Encrypt=False; ApplicationIntent=ReadWrite";

    
/* ////////////////////////////////////////////////////////////////////// */
    // Preforms All Data retrieving functions 
       public List<MovieMod> ReadIn()
       {
           List<MovieMod> returnList = new List<MovieMod>();
           
           // Access Database
           using (SqlConnection connection = new SqlConnection(connectionString))
           {
               string sqlQuery = "SELECT * from MovieDataBase.dbo.movies";

               SqlCommand command = new SqlCommand(sqlQuery, connection);

               connection.Open();
               SqlDataReader reader = command.ExecuteReader();

               if (reader.HasRows)
               {
                   while (reader.Read())
                   {
                       // Create New Movie
                       MovieMod movieMod = new MovieMod();
                       movieMod.FilmTitle = reader.GetString(0);
                       movieMod.Genre = reader.GetString(1);
                       movieMod.Studio = reader.GetString(2);
                       movieMod.AudienceScore = reader.GetByte(3);
                       movieMod.Year = reader.GetInt16(7);
                       
                       returnList.Add(movieMod);
                   }
               }
           }

           return returnList;
       }
/* //////////////////////////////////////////////////////////////////////// */

    public void Create(MovieMod movieMod)
    {
        // Access Database
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sqlQuery = "INSERT INTO MovieDataBase.dbo.movies(Film,Genre,Lead_Studio,Audience_score,Profitability,Rotten_Tomatoes,Worldwide_Gross,Year) " +
                              "Values(@FilmTitle, @Genre, @Studio, @AudienceScore, '', '', '', @Year)";

            SqlCommand command = new SqlCommand(sqlQuery, connection);

            command.Parameters.Add("@FilmTitle", System.Data.SqlDbType.VarChar, 100).Value = movieMod.FilmTitle;
            command.Parameters.Add("@Genre", System.Data.SqlDbType.VarChar, 100).Value = movieMod.Genre;
            command.Parameters.Add("@Studio", System.Data.SqlDbType.VarChar, 100).Value = movieMod.Studio;
            command.Parameters.Add("@AudienceScore", System.Data.SqlDbType.TinyInt, 100).Value = movieMod.AudienceScore;
            command.Parameters.Add("@Year", System.Data.SqlDbType.SmallInt, 100).Value = movieMod.Year;
            
            connection.Open();
            command.ExecuteNonQuery();

            return;
        }
    }
/* //////////////////////////////////////////////////////////////////////// */

    public MovieMod Details(string Film)
    {

        // Access Database
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // 
            string sqlQuery = "SELECT * from MovieDataBase.dbo.movies WHERE Film = @Film";
            

            SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.Parameters.Add("@Film", System.Data.SqlDbType.VarChar).Value = Film;

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            MovieMod movieMod = new MovieMod();
            
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    // Create New Movie
                    movieMod.FilmTitle = reader.GetString(0);
                    movieMod.Genre = reader.GetString(1);
                    movieMod.Studio = reader.GetString(2);
                    movieMod.AudienceScore = reader.GetByte(3);
                    movieMod.Year = reader.GetInt16(7);
                       
                    
                }
            }
            return movieMod;
        }
        
    }
    /* //////////////////////////////////////////////////////////////////////// */

    public void Delete(string filmTitle)
    {
        // Access Database
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sqlQuery = "DELETE FROM MovieDataBase.dbo.movies WHERE Film = @FilmTitle";

            SqlCommand command = new SqlCommand(sqlQuery, connection);

            command.Parameters.Add("@FilmTitle", System.Data.SqlDbType.VarChar, 100).Value = filmTitle;

            connection.Open();
            command.ExecuteNonQuery();

            return;
        }
    }
    
    /* //////////////////////////////////////////////////////////////////////// */

}
