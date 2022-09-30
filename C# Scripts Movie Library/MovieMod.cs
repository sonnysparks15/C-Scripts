// Movie Model Class


namespace Movie_Library.Models;

public class MovieMod
{
    public string FilmTitle {get; set; }
    public string Genre {get; set; }
    public string Studio {get; set; }
    public byte AudienceScore {get; set; }
    public Int16 Year {get; set; }

    public MovieMod()
    {
        FilmTitle = "Nothing";
        Genre = "Nothing";
        Studio = "Nothing";
        AudienceScore = 0;
        Year = 0;
    }

    public MovieMod(string filmTitle, string genre, string studio, byte audienceScore, Int16 year)
    {
        FilmTitle = filmTitle;
        Genre = genre;
        Studio = studio;
        AudienceScore = audienceScore;
        Year = year;
    }
}